using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Services
{
    public class EmployeeLogReportImpl : Interfaces.EmployeeLogReportInterface
    {
        public FOContext _FOContext;
        private object multipletype;


        public EmployeeLogReportImpl(FOContext fOContext)
        {
            _FOContext = fOContext;
        }
        public EmployeeLogReportDTO getdata(EmployeeLogReportDTO data)
        {
            try
            {
                List<HR_Master_GroupType> staf_types = new List<HR_Master_GroupType>();
                staf_types = _FOContext.HR_Master_GroupType_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMGT_ActiveFlag == true).ToList(); //
                data.filltypes = staf_types.Distinct().ToArray();
                data.fillyear = (from a in _FOContext.HR_Master_LeaveYearDMO
                                 where (a.MI_Id == data.MI_Id && a.HRMLY_ActiveFlag == true)
                                 select new HR_Master_LeaveYearDTO
                                 {
                                     HRMLY_Id = Convert.ToInt32(a.HRMLY_Id),
                                     HRMLY_LeaveYear = a.HRMLY_LeaveYear

                                 }).Distinct().OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();


                data.fillmonth = (from a in _FOContext.Month
                                  select new EmployeeLogReportDTO
                                  {
                                      monthid = Convert.ToInt32(a.IVRM_Month_Id),
                                      monthname = a.IVRM_Month_Name
                                  }).Distinct().OrderBy(t => t.monthid).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public EmployeeLogReportDTO get_departments(EmployeeLogReportDTO data)
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
                                   select new EmployeeLogReportDTO
                                   {
                                       HRMD_Id = b.HRMD_Id,
                                       HRMD_DepartmentName = b.HRMD_DepartmentName,
                                   }
                      ).Distinct().ToArray();

            return data;
        }
        public EmployeeLogReportDTO get_designation(EmployeeLogReportDTO data)
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
                                    select new EmployeeLogReportDTO
                                    {
                                        HRMDES_Id = b.HRMDES_Id,
                                        HRMDES_DesignationName = b.HRMDES_DesignationName,
                                    }
                     ).Distinct().ToArray();

            return data;
        }
        public EmployeeLogReportDTO get_employee(EmployeeLogReportDTO data)
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
            data.fillemployee = (from a in _FOContext.HR_Master_Employee_DMO
                                 where (a.MI_Id == data.MI_Id && list.Contains(a.HRMDES_Id) && list3.Contains(a.HRMGT_Id) && list2.Contains(a.HRMD_Id) && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                 select new EmployeeLogReportDTO
                                 {
                                     HRME_Id = a.HRME_Id,
                                     HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                     HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName ?? " ",
                                     HRME_EmployeeLastName = a.HRME_EmployeeLastName ?? " ",
                                     HRME_EmployeeCode = a.HRME_EmployeeCode
                                 }
                     ).Distinct().ToArray();

            return data;
        }
        public async Task<EmployeeLogReportDTO> getreport(EmployeeLogReportDTO data)
        {
            try
            {


                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 600;
                    cmd.CommandText = "Fo_Emp_Log_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@date",
                        SqlDbType.VarChar)
                    {
                        Value = data.selectdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@month",
                       SqlDbType.VarChar)
                    {
                        Value = data.selectmonth
                    });
                    cmd.Parameters.Add(new SqlParameter("@year",
                   SqlDbType.VarChar)
                    {
                        Value = data.selectyear
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
                                          //dataReader1.IsDBNull(iFiled1) ? null : dataReader1[iFiled1]
                                    dataReader1.IsDBNull(iFiled1) ? "-" : dataReader1[iFiled1] // use null instead of {}
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
        public async Task<EmployeeLogReportDTO> getsiglerpt(EmployeeLogReportDTO data)
        {
            try
            {
                if (data.punchtype == "datewise")
                {
                    data.fromdate = data.selectdate;
                    data.todate = data.selectdate;
                }
                else if (data.punchtype == "monthwise")
                {
                    string ffirstdate = data.selectyear + "-" + data.selectmonth + "-01";
                    int lastday = _FOContext.Month.Where(t => t.IVRM_Month_Id.ToString() == data.selectmonth).Select(t => t.IVRM_Month_Max_Days).FirstOrDefault();
                    string llastdate = data.selectyear + "-" + data.selectmonth + "-" + lastday.ToString();
                    data.fromdate = ffirstdate;
                    data.todate = llastdate;
                }

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 300;
                    cmd.CommandText = "FO_SingleLog";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.NVarChar)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Multihrme_Id", SqlDbType.VarChar)
                    {
                        Value = data.multiplehrmeid
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
                                        dataReader1.IsDBNull(iFiled1) ? "-" : dataReader1[iFiled1]
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
    }
}
