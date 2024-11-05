using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model.com.vapstech.HRMS;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class PeriodwseLeaveReportServices : PeriodWseLeavReportInterface
    {
        public LMContext _lmContext;
        public DomainModelMsSqlServerContext _db;
        public TTContext _ttcontext;
        public PeriodwseLeaveReportServices(LMContext ttcategory, DomainModelMsSqlServerContext abc, TTContext tT)
        {
            _lmContext = ttcategory; 
            _db = abc;
            _ttcontext = tT;
        }

        public LeaveCreditDTO getdata(LeaveCreditDTO data)
        {
            try
            {
            
                data.Time_Table = _ttcontext.TT_Master_PeriodDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMP_ActiveFlag == true).ToList().ToArray();
                var staf_types = _lmContext.HR_Master_GroupType_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMGT_ActiveFlag == true).ToList(); 
                data.filltypes = staf_types.Distinct().ToArray();
                data.fillyear = (from a in _lmContext.HR_Master_LeaveYear_DMO
                                 where (a.MI_Id == data.MI_Id && a.HRMLY_ActiveFlag == true)
                                 select new HR_Master_LeaveYearDTO
                                 {
                                     HRMLY_Id = Convert.ToInt32(a.HRMLY_Id),
                                     HRMLY_LeaveYear = a.HRMLY_LeaveYear

                                 }).Distinct().OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();
                data.leavelist = _lmContext.HR_Master_Leave_DMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                var q = (from a in _lmContext.IVRM_Month_DMO
                         where (a.Is_Active == true)
                         select new
                         {
                             monthid = a.IVRM_Month_Id,
                             monthname = a.IVRM_Month_Name,
                         }).Distinct().ToArray();

                var query = q.Distinct().ToArray();
                data.fillmonth = (from a in query
                                  select new LeaveCreditDTO
                                  {
                                      monthid = Convert.ToInt32(a.monthid),
                                      monthname = a.monthname
                                  }).Distinct().OrderBy(t => t.monthid).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LeaveCreditDTO get_departments(LeaveCreditDTO data)
        {
            var dd = data.multipletype.Split(',');
            List<long> list = new List<long>();
            for (int i = 0; i < dd.Length; i++)
            {
                list.Add(Convert.ToInt64(dd[i]));
            }
            data.filldepartment = (from a in _lmContext.HR_Master_Employee_DMO
                                   from b in _lmContext.HR_Master_Department_DMO
                                   from c in _lmContext.HR_Master_GroupType_DMO
                                   where (a.HRMD_Id == b.HRMD_Id && a.HRMGT_Id == c.HRMGT_Id && c.HRMGT_ActiveFlag == true
                                       && b.HRMD_ActiveFlag == true && a.HRME_ActiveFlag == true && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && list.Contains(c.HRMGT_Id))
                                   select new LeaveCreditDTO
                                   {
                                       HRMD_Id = b.HRMD_Id,
                                       HRMD_DepartmentName = b.HRMD_DepartmentName,
                                   }
                      ).Distinct().ToArray();

            return data;
        }
        public LeaveCreditDTO get_designation(LeaveCreditDTO data)
        {
            var dd = data.multipledep.Split(',');
            List<long> list = new List<long>();
            for (int i = 0; i < dd.Length; i++)
            {
                list.Add(Convert.ToInt64(dd[i]));
            }

            data.filldesignation = (from a in _lmContext.HR_Master_Employee_DMO
                                    from b in _lmContext.HR_Master_Designation_DMO
                                    from c in _lmContext.HR_Master_Department_DMO
                                    where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                                    && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true
                                    && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && list.Contains(c.HRMD_Id))
                                    select new LeaveCreditDTO
                                    {
                                        HRMDES_Id = b.HRMDES_Id,
                                        HRMDES_DesignationName = b.HRMDES_DesignationName,
                                    }
                     ).Distinct().ToArray();

            return data;
        }
        public LeaveCreditDTO get_employee(LeaveCreditDTO data)
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
            data.fillemployee = (from a in _lmContext.HR_Master_Employee_DMO
                                 where (a.MI_Id == data.MI_Id && list.Contains(a.HRMDES_Id) && list3.Contains(a.HRMGT_Id) && list2.Contains(a.HRMD_Id) && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                 select new LeaveCreditDTO
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
        public LeaveCreditDTO getreport(LeaveCreditDTO data)
        {
            try
            {
                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {                    
                    cmd.CommandText = "Periodwise_Emp_Leave_Report";
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

                    cmd.Parameters.Add(new SqlParameter("@status",
                  SqlDbType.VarChar)
                    {
                        Value = data.punchtype
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.filldata = retObject.ToArray();
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
        public LeaveCreditDTO getsiglerpt(LeaveCreditDTO data)
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
                    int lastday = _lmContext.IVRM_Month_DMO.Where(t => t.IVRM_Month_Id.ToString() == data.selectmonth).Select(t => t.IVRM_Month_Max_Days).FirstOrDefault();
                    string llastdate = data.selectyear + "-" + data.selectmonth + "-" + lastday.ToString();
                    data.fromdate = ffirstdate;
                    data.todate = llastdate;
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
