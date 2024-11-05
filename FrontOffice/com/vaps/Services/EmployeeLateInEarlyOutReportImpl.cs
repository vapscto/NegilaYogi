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
    public class EmployeeLateInEarlyOutReportImpl : Interfaces.EmployeeLateInEarlyOutReportInterface
    {

        public FOContext _FOContext;

        private object multipletype;

        public EmployeeLateInEarlyOutReportImpl(FOContext fOContext)
        {
            _FOContext = fOContext;
        }
        public EmployeeLateInEarlyOutReportDTO getdata(EmployeeLateInEarlyOutReportDTO data)
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
        public EmployeeLateInEarlyOutReportDTO get_departments(EmployeeLateInEarlyOutReportDTO data)
        {
            var dd = data.multipletype.Split(',');
            List<long> list = new List<long>();
            for (int i = 0; i < dd.Length; i++)
            {
                list.Add(Convert.ToInt64(dd[i]));
            }
            data.filldepartment = (from a in _FOContext.HRGroupDeptDessgDMO
                                   from b in _FOContext.HR_Master_Department_DMO
                                   where (a.HRMD_Id == b.HRMD_Id 
                                       && b.HRMD_ActiveFlag == true && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && list.Contains(a.HRMGT_Id))
                                   select new EmployeeLateInEarlyOutReportDTO
                                   {
                                       HRMD_Id = b.HRMD_Id,
                                       HRMD_DepartmentName = b.HRMD_DepartmentName,
                                   }
                     ).Distinct().ToArray();

            return data;
        }

        public EmployeeLateInEarlyOutReportDTO get_designation(EmployeeLateInEarlyOutReportDTO data)
        {
            var dd = data.multipledep.Split(',');
            List<long> list = new List<long>();
            for (int i = 0; i < dd.Length; i++)
            {
                list.Add(Convert.ToInt64(dd[i]));
            }
            data.filldesignation = (from a in _FOContext.HRGroupDeptDessgDMO
                                    from b in _FOContext.HR_Master_Designation_DMO
                                    where (a.HRMDES_Id == b.HRMDES_Id
                                    && b.HRMDES_ActiveFlag == true 
                                    && a.MI_Id == b.MI_Id  && a.MI_Id == data.MI_Id && list.Contains(a.HRMD_Id))
                                    select new EmployeeLateInEarlyOutReportDTO
                                    {
                                        HRMDES_Id = b.HRMDES_Id,
                                        HRMDES_DesignationName = b.HRMDES_DesignationName,
                                    }
                     ).Distinct().ToArray();

            return data;
        }
        public EmployeeLateInEarlyOutReportDTO get_employee(EmployeeLateInEarlyOutReportDTO data)
        {
            var desig = data.multipledes.Split(',');
            List<long> list = new List<long>();
            for(int i = 0; i < desig.Length; i++)
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
            data.fillemployee = (from a in _FOContext.HR_Master_Employee_DMO
                                 where (a.MI_Id == data.MI_Id && list.Contains(a.HRMDES_Id) && list3.Contains(a.HRMGT_Id) && list2.Contains(a.HRMD_Id) &&  a.HRME_ActiveFlag==true && a.HRME_LeftFlag==false)
                                 select new EmployeeLateInEarlyOutReportDTO
                                 {
                                     HRME_Id = a.HRME_Id,
                                     ename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName)
                                     + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                     HRME_EmployeeCode = a.HRME_EmployeeCode
                                 }
                ).Distinct().ToArray();


            return data;
        }
        public async Task<EmployeeLateInEarlyOutReportDTO> getreport(EmployeeLateInEarlyOutReportDTO data)
        {
            try
            {
                data.columnnames = (from a in Enumerable.Range(0, (Convert.ToDateTime(data.todate) - Convert.ToDateTime(data.fromdate)).Days + 1)
                                    let columndate = Convert.ToDateTime(data.fromdate).AddDays(a)
                                    select columndate).Distinct().ToArray();

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fo_Emp_Log_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@date",
                        SqlDbType.VarChar)
                    {
                        Value = ""
                    });
                    cmd.Parameters.Add(new SqlParameter("@month",
                       SqlDbType.VarChar)
                    {
                        Value = ""
                    });
                    cmd.Parameters.Add(new SqlParameter("@year",
                   SqlDbType.VarChar)
                    {
                        Value = ""
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                   SqlDbType.VarChar)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                  SqlDbType.NVarChar)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@miid",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@multiplehrmeid",
                  SqlDbType.VarChar)
                    {
                        Value = data.multiplehrmeid
                    });
                    cmd.Parameters.Add(new SqlParameter("@punchtype",
                  SqlDbType.VarChar)
                    {
                        Value = data.punchtype
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

        public async Task<EmployeeInOutReportDTO> lateIn_details(EmployeeInOutReportDTO data)
        {
            try
            {
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_sp_mg_paycare_latein_details";
                    cmd.CommandType = CommandType.StoredProcedure;


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.activityIds = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    //leaveApprovalStatus(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("late In report error");
                //_log.LogInformation("Late In Report error");
                //_log.LogDebug(ex.Message);
            }
            return data;
        }
    }
}
