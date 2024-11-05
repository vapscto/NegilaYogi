using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Services
{
    public class EmployeeYearlyReportImpl:Interfaces.EmployeeYearlyReportInterface
    {
        public FOContext _FOContext;
        private object multipletype;

    
        public EmployeeYearlyReportImpl(FOContext fOContext)
        {
            _FOContext = fOContext;
        }

        public EmployeeYearlyReportDTO getdata(EmployeeYearlyReportDTO data)
        {
            try
            {
                List<HR_Master_GroupType> staf_types = new List<HR_Master_GroupType>();
                staf_types = _FOContext.HR_Master_GroupType_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMGT_ActiveFlag == true).ToList(); //
                data.filltypes = staf_types.Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public EmployeeYearlyReportDTO get_departments(EmployeeYearlyReportDTO data)
        {
            var dd = data.multipletype.Split(',');
            List<long> list = new List<long>();
            for (int i = 0; i < dd.Length; i++)
            {
                list.Add(Convert.ToInt64(dd[i]));
            }
            data.filldepartment = (from a in _FOContext.HR_Master_Employee_DMO
                                   from b in _FOContext.HR_Master_Department_DMO
                                   from c in _FOContext.HR_Master_GroupType_DMO
                                   where (a.HRMD_Id == b.HRMD_Id && a.HRMGT_Id == c.HRMGT_Id && c.HRMGT_ActiveFlag == true
                                       && b.HRMD_ActiveFlag == true && a.HRME_ActiveFlag == true && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && list.Contains(c.HRMGT_Id))
                                   select new EmployeeYearlyReportDTO
                                   {
                                       HRMD_Id = b.HRMD_Id,
                                       HRMD_DepartmentName = b.HRMD_DepartmentName,
                                   }
                     ).Distinct().ToArray();

            return data;
        }

        public EmployeeYearlyReportDTO get_designation(EmployeeYearlyReportDTO data)
        {
            var dd = data.multipledep.Split(',');
            List<long> list = new List<long>();
            for (int i = 0; i < dd.Length; i++)
            {
                list.Add(Convert.ToInt64(dd[i]));
            }
            data.filldesignation = (from a in _FOContext.HR_Master_Employee_DMO
                                    from b in _FOContext.HR_Master_Designation_DMO
                                    from c in _FOContext.HR_Master_Department_DMO
                                    where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                                    && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true
                                    && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && list.Contains(c.HRMD_Id))
                                    select new EmployeeYearlyReportDTO
                                    {
                                        HRMDES_Id = b.HRMDES_Id,
                                        HRMDES_DesignationName = b.HRMDES_DesignationName,
                                    }
                     ).Distinct().ToArray();

            return data;
        }
        public EmployeeYearlyReportDTO get_employee(EmployeeYearlyReportDTO data)
        {
            var desig = data.multipledes.Split(',');
            List<long> list = new List<long>();
            for (int i = 0; i < desig.Length; i++)
            {
                list.Add(Convert.ToInt64(desig[i]));
            }
            var dept = data.multipledep.Split(',');
            List<long> list2 = new List<long>();
            for (int i = 0; i < dept.Length; i++)
            {
                list2.Add(Convert.ToInt64(dept[i]));
            }
            var typ = data.multipletype.Split(',');
            List<long> list3 = new List<long>();
            for (int i = 0; i < typ.Length; i++)
            {
                list3.Add(Convert.ToInt64(typ[i]));
            }
            data.fillemployee = (from c in _FOContext.HR_Master_Employee_DMO
                                 where (c.MI_Id == data.MI_Id && list.Contains(c.HRMDES_Id) && list3.Contains(c.HRMGT_Id) && list2.Contains(c.HRMD_Id) && c.HRME_ActiveFlag == true && c.HRME_LeftFlag == false)
                                 select new EmployeeInOutReportDTO
                                 {
                                     HRME_Id = c.HRME_Id,
                                     // ename = a.HRME_EmployeeFirstName + ' ' + a.HRME_EmployeeMiddleName + ' ' + a.HRME_EmployeeLastName,
                                     ename = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : " " + c.HRME_EmployeeFirstName)
                                     + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == "" || c.HRME_EmployeeMiddleName == "0" ? "" : " " + c.HRME_EmployeeMiddleName) + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == "" || c.HRME_EmployeeLastName == "0" ? "" : " " + c.HRME_EmployeeLastName)).Trim(),
                                     HRME_EmployeeCode = c.HRME_EmployeeCode
                                 }
               ).Distinct().ToArray();

            return data;
        }
        public async Task<EmployeeYearlyReportDTO> getreport(EmployeeYearlyReportDTO data)
        {

            try
            {

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_Emp_Monthly_yearly_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                        SqlDbType.VarChar)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                       SqlDbType.VarChar)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@multiplehrmeid",
                   SqlDbType.NVarChar)
                    {
                        Value = data.multiplehrmeid
                    });
                    cmd.Parameters.Add(new SqlParameter("@miid",
                   SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
                  SqlDbType.VarChar)
                    {
                        Value = "yearly"
                    });
                    cmd.Parameters.Add(new SqlParameter("@cols",
                 SqlDbType.NVarChar, 2000)
                    {
                        Direction = ParameterDirection.Output
                    });
                    cmd.Parameters.Add(new SqlParameter("@totalpresent",
                 SqlDbType.VarChar, 10)
                    {
                        Direction = ParameterDirection.Output
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();


                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader1 = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader1.ReadAsync())
                            {

                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.filldata = retObject1.ToArray();
                        data.columnnames = cmd.Parameters["@cols"].Value.ToString();
                        data.totalworkingdays = cmd.Parameters["@totalpresent"].Value.ToString();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }


            return data;
        }
    }
}
