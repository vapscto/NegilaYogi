using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.LeaveManagement;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class PolicyImpl : PlicyInterface
    {
        public DomainModelMsSqlServerContext _db;
        public FOContext _FOContext;
        public LMContext _lmContext;
        public DomainModelMsSqlServerContext _context;
        ILogger<PolicyImpl> _log;
        public PolicyImpl(FOContext ttcntx, LMContext loggerFactor, DomainModelMsSqlServerContext biomet, DomainModelMsSqlServerContext context , ILogger<PolicyImpl> _logger)
        {
            _FOContext = ttcntx;
            _lmContext = loggerFactor;
            _db = biomet;
            _context=context;
            _log = _logger;
    }

        //public LeavepolicyDTO getpolicy(LeavepolicyDTO data)
        //{
        //    double total;
        //    double perday;
        //    DateTime date;
        //    perday = 0;
        //    double p60a = 0;
        //    string flag = "";
        //    int cnt = 0;
        //    total = 0;

        //    //double abtotal=0;
        //    string empcode = "";
        //    string name = "";
        //    string mobileno = "";
        //    int dept_code = 0;
        //    try
        //    {
        //        var query1 = (from e in _db.HR_Master_Employee_DMO
        //                      from m in _db.Multiple_Mobile_DMO
        //            where e.HRME_Id == m.HRME_Id && e.MI_Id == data.MI_Id && e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false
        //                      select new LeavepolicyDTO
        //                      {
        //                          HRME_Id = e.HRME_Id,
        //                          HRME_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null || e.HRME_EmployeeFirstName == "" ? "" : " " + e.HRME_EmployeeFirstName)
        //                             + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == "" || e.HRME_EmployeeMiddleName == "0" ? "" : " " + e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == "" || e.HRME_EmployeeLastName == "0" ? "" : " " + e.HRME_EmployeeLastName)).Trim(),
        //                          HRME_MobileNo=m.HRMEMNO_MobileNo,
        //                          HRME_EmployeeCode=e.HRME_EmployeeCode,
        //                          HRME_EmailId=e.HRME_EmailId,
        //                          HRMDES_Id=e.HRMDES_Id

        //        }
        //        ).ToList();

        //        if (query1.Count > 0)
        //        {
        //            foreach(LeavepolicyDTO row in query1)
        //            {
        //                try
        //                {
        //                    using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
        //                    {
        //                        cmd.CommandTimeout = 300;
        //                        cmd.CommandText = "Fo_Emp_Log_Report";
        //                        cmd.CommandType = CommandType.StoredProcedure;
        //                        cmd.Parameters.Add(new SqlParameter("@date",
        //                            SqlDbType.VarChar)
        //                        {
        //                            Value = ""// DateTime.Now.Date
        //                        });
        //                        cmd.Parameters.Add(new SqlParameter("@month",
        //                           SqlDbType.VarChar)
        //                        {
        //                            Value = 6
        //                        });
        //                        cmd.Parameters.Add(new SqlParameter("@year",
        //                       SqlDbType.VarChar)
        //                        {
        //                            Value = "2018"
        //                        });
        //                        cmd.Parameters.Add(new SqlParameter("@fromdate",
        //                       SqlDbType.VarChar)
        //                        {
        //                            Value = ""
        //                        });
        //                        cmd.Parameters.Add(new SqlParameter("@todate",
        //                      SqlDbType.NVarChar)
        //                        {
        //                            Value = ""
        //                        });
        //                        cmd.Parameters.Add(new SqlParameter("@miid",
        //                      SqlDbType.BigInt)
        //                        {
        //                            Value = data.MI_Id
        //                        });
        //                        cmd.Parameters.Add(new SqlParameter("@multiplehrmeid",
        //                      SqlDbType.VarChar)
        //                        {
        //                            Value = row.HRME_Id
        //                        });

        //                        cmd.Parameters.Add(new SqlParameter("@punchtype",
        //                      SqlDbType.VarChar)
        //                        {
        //                             Value = "late"
        //                        });
        //                        if (cmd.Connection.State != ConnectionState.Open)
        //                            cmd.Connection.Open();

        //                        var list = new List<dynamic>();
        //                        try
        //                        {

        //                            using (var dataReader = cmd.ExecuteReader())
        //                            {
        //                                while (dataReader.Read())
        //                                {
        //                                    list.Add(new LeavepolicyDTO
        //                                    {
        //                                        HRME_Id = Convert.ToInt64(dataReader["HRME_Id"]),
        //                                        MI_Id = data.MI_Id,
        //                                       // l_date = ,
        //                                        //MAC_P_Flag = Convert.ToString(dataReader["MAC_P_Flag"]),
        //                                        //LATEIn = Convert.ToInt32(dataReader["LATEIn"]),
        //                                        //HRELT_LeaveReason = "Late_Lop",
        //                                    });

        //                                }
        //                            }

        //                            //data.filldata = retObject1.ToArray();
        //                            foreach (LeavepolicyDTO row2 in list)
        //                            {
        //                                var latetime = _lmContext.HR_Leave_Policy_Config_DMO.Where(q => q.HRLPC_LateInFlag == true && q.MI_Id == row2.MI_Id).Select(w => w.HRLPC_LateInTime);
        //                                if(latetime!=null && latetime.ToString()!="")
        //                                {
        //                                    if (row2.LATEIn >= Convert.ToInt32(latetime))//max late in time comparison
        //                                    {
        //                                        var query2 = (from a in _lmContext.HR_Emp_Leave_Trans_DMO
        //                                                      from b in _lmContext.HR_Emp_Leave_Trans_Details_DMO
        //                                                      from c in _lmContext.HR_Master_Leave_DMO
        //                                                      where a.HRELT_Id == b.HRELT_Id && a.HRELT_LeaveId == c.HRML_Id && a.HRME_Id == row2.HRME_Id && a.HRELT_FromDate == row2.l_date && a.HRELT_ToDate == row2.l_date && a.HRELT_LeaveReason == "Late_Lop"
        //                                                      select new LeavepolicyDTO
        //                                                      {
        //                                                          HRELT_Id = a.HRELT_Id,
        //                                                          MI_Id = a.MI_Id,
        //                                                          HRME_Id = a.HRME_Id,
        //                                                          HRMLY_Id = a.HRMLY_Id,
        //                                                          HRELT_LeaveReason = a.HRELT_LeaveReason,
        //                                                          HRELT_FromDate = a.HRELT_FromDate,
        //                                                          HRELT_ToDate = a.HRELT_ToDate
        //                                                      }).ToArray();

        //                                        if (query2.Length > 0)
        //                                        {
        //                                            row2.LATEIn = 0;
        //                                        }
        //                                        else
        //                                        {
        //                                            string Message = "Dear " + " " + row.HRME_EmployeeFirstName + ", " + "  Today('" + row2.l_date + "') you came late by " + row2.LATEIn + ", according to KGI HR policies it is considered as half day LOP for you. To avoid half day LOP kindly apply half day CL through employee portal with in 16:00:00 (" + DateTime.Now.ToString("yyyy-MM-dd") + ") with sensible reason. If you are fail to apply half day leave with in the above mentioned time or get it rejected by your supervisor, it will consider as half day LOP.";

        //                                            SENDSMS_Staff(row2.HRME_Id, row.HRME_MobileNo, Message, row2.l_date);
        //                                            HR_Emp_Leave_Trans_DMO objpge = new HR_Emp_Leave_Trans_DMO();
        //                                            objpge.HRME_Id = row.HRME_Id;
        //                                            objpge.HRMLY_Id = row.HRMLY_Id;
        //                                            objpge.MI_Id = row.MI_Id;
        //                                            objpge.HRELT_FromDate = row.l_date;
        //                                            objpge.HRELT_ToDate = row.l_date;
        //                                            objpge.HRELT_Id = row.HRELT_Id;
        //                                            objpge.CreatedDate = DateTime.Now;
        //                                            objpge.HRELT_ActiveFlag = true;
        //                                            objpge.HRELT_LeaveId = row.HRELT_LeaveId;
        //                                            objpge.HRELT_LeaveReason = row.HRELT_LeaveReason;
        //                                            objpge.UpdatedDate = DateTime.Now;
        //                                            objpge.HRELT_TotDays = 0.5;
        //                                            _lmContext.Add(objpge);
        //                                            var data1 = _lmContext.SaveChanges();
        //                                            row.LATEIn = 0;
        //                                        }
        //                                    }
        //                                    //---------------------------------------------------------------------------------------------------------------------------------
        //                                    else if (row2.LATEIn < Convert.ToInt32(latetime)) //max late in time comparison
        //                                    {

        //                                        total = total + row2.LATEIn;

        //                                        var cum_time = _lmContext.HR_Leave_Policy_Config_DMO.Where(q => q.HRLPC_CummulativeTimeFlag == true && q.MI_Id == row2.MI_Id).Select(w => w.HRLPC_CummulativeTime);
        //                                        if (cum_time != null && cum_time.ToString() != "")
        //                                        {

        //                                            if (total >= Convert.ToUInt32(cum_time) && cnt == 0)//to leave that day when it reach 60 min
        //                                            {
        //                                                cnt++;
        //                                                var query3 = (from a in _lmContext.HR_Emp_Leave_Trans_DMO
        //                                                              from b in _lmContext.HR_Emp_Leave_Trans_Details_DMO
        //                                                              from c in _lmContext.HR_Master_Leave_DMO
        //                                                              where a.HRELT_Id == b.HRELT_Id && a.HRELT_LeaveId == c.HRML_Id && a.HRME_Id == row2.HRME_Id && a.HRELT_FromDate == row2.l_date && a.HRELT_ToDate == row2.l_date && a.HRELT_LeaveReason == "Late_Lop"
        //                                                              select new LeavepolicyDTO
        //                                                              {
        //                                                                  HRELT_Id = a.HRELT_Id,
        //                                                                  MI_Id = a.MI_Id,
        //                                                                  HRME_Id = a.HRME_Id,
        //                                                                  HRMLY_Id = a.HRMLY_Id,
        //                                                                  HRELT_LeaveReason = a.HRELT_LeaveReason,
        //                                                                  HRELT_FromDate = a.HRELT_FromDate,
        //                                                                  HRELT_ToDate = a.HRELT_ToDate
        //                                                              }).ToArray();
        //                                                if (query3.Length > 0)
        //                                                {
        //                                                    total = 0;
        //                                                }
        //                                                else
        //                                                {
        //                                                    string Message = "Dear " + " " + row.HRME_EmployeeFirstName + ", " + "  Today('" + DateTime.Now.Date + "') you came late by " + row2.LATEIn + " and your relaxation time got exhausted by " + row2.l_date + " minutes which is more than "+ cum_time + "minutes , according to HR policies it is considered as half day LOP for you. To avoid half day LOP kindly apply half day CL through Leave portal within 04:00 pm (" + DateTime.Now.ToString("yyyy-MM-dd") + ") with sensible reason. If you are fail to apply half day leave with in the above mentioned time or get it rejected by your supervisor, it will consider as half day LOP.";
        //                                                    SENDSMS_Staff(row2.HRME_Id, row.HRME_MobileNo, Message, row2.l_date);

        //                                                    HR_Emp_Leave_Trans_DMO objpge = new HR_Emp_Leave_Trans_DMO();
        //                                                    objpge.HRME_Id = row.HRME_Id;
        //                                                    objpge.HRMLY_Id = row.HRMLY_Id;
        //                                                    objpge.MI_Id = row.MI_Id;
        //                                                    objpge.HRELT_FromDate = row.l_date;
        //                                                    objpge.HRELT_ToDate = row.l_date;
        //                                                    objpge.HRELT_Id = row.HRELT_Id;
        //                                                    objpge.CreatedDate = DateTime.Now;
        //                                                    objpge.HRELT_ActiveFlag = true;
        //                                                    objpge.HRELT_LeaveId = row.HRELT_LeaveId;
        //                                                    objpge.HRELT_LeaveReason = row.HRELT_LeaveReason;
        //                                                    objpge.UpdatedDate = DateTime.Now;
        //                                                    objpge.HRELT_TotDays = 0.5;
        //                                                    _lmContext.Add(objpge);
        //                                                    var data1 = _lmContext.SaveChanges();

        //                                                }
        //                                            }

        //                                            else if (total > Convert.ToUInt32(cum_time) && cnt != 0)//to get start 10 min count from 0
        //                                            {
        //                                                p60a = p60a + row2.LATEIn;
        //                                                var cnt_time = _lmContext.HR_Leave_Policy_Config_DMO.Where(q => q.MI_Id == row2.MI_Id).Select(w => w.HRLPC_AfterCummulativeTime);
        //                                                if (cnt_time != null && cnt_time.ToString() != "")
        //                                                {
        //                                                    if (p60a > Convert.ToInt32(cnt_time))
        //                                                {
        //                                                    var query4 = (from a in _lmContext.HR_Emp_Leave_Trans_DMO
        //                                                                  from b in _lmContext.HR_Emp_Leave_Trans_Details_DMO
        //                                                                  from c in _lmContext.HR_Master_Leave_DMO
        //                                                                  where a.HRELT_Id == b.HRELT_Id && a.HRELT_LeaveId == c.HRML_Id && a.HRME_Id == row2.HRME_Id && a.HRELT_FromDate == row2.l_date && a.HRELT_ToDate == row2.l_date && a.HRELT_LeaveReason == "Late_Lop"
        //                                                                  select new LeavepolicyDTO
        //                                                                  {
        //                                                                      HRELT_Id = a.HRELT_Id,
        //                                                                      MI_Id = a.MI_Id,
        //                                                                      HRME_Id = a.HRME_Id,
        //                                                                      HRMLY_Id = a.HRMLY_Id,
        //                                                                      HRELT_LeaveReason = a.HRELT_LeaveReason,
        //                                                                      HRELT_FromDate = a.HRELT_FromDate,
        //                                                                      HRELT_ToDate = a.HRELT_ToDate
        //                                                                  }).ToArray();
        //                                                    if (query4.Length > 0)
        //                                                    {
        //                                                        p60a = 0;
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        string Message = "Dear " + " " + row.HRME_EmployeeFirstName + ", " + "  Today('" + DateTime.Now.Date + "') you came late on " + row2.l_date + " and your relaxation time got exhausted by " + row2.LATEIn + " minutes which is more than "+ cum_time + "minutes , according to HR policies it is considered as half day LOP for you. To avoid half day LOP kindly apply half day CL through employee portal with in 04:00 pm (" + DateTime.Now.ToString("yyyy-MM-dd") + ") with sensible reason. If you are fail to apply half day leave with in the above mentioned time or get it rejected by your supervisor, it will consider as half day LOP.";
        //                                                        SENDSMS_Staff(row2.HRME_Id, row.HRME_MobileNo, Message, row2.l_date);

        //                                                        HR_Emp_Leave_Trans_DMO objpge = new HR_Emp_Leave_Trans_DMO();
        //                                                        objpge.HRME_Id = row.HRME_Id;
        //                                                        objpge.HRMLY_Id = row.HRMLY_Id;
        //                                                        objpge.MI_Id = row.MI_Id;
        //                                                        objpge.HRELT_FromDate = row.l_date;
        //                                                        objpge.HRELT_ToDate = row.l_date;
        //                                                        objpge.HRELT_Id = row.HRELT_Id;
        //                                                        objpge.CreatedDate = DateTime.Now;
        //                                                        objpge.HRELT_ActiveFlag = true;
        //                                                        objpge.HRELT_LeaveId = row.HRELT_LeaveId;
        //                                                        objpge.HRELT_LeaveReason = row.HRELT_LeaveReason;
        //                                                        objpge.UpdatedDate = DateTime.Now;
        //                                                        objpge.HRELT_TotDays = 0.5;
        //                                                        _lmContext.Add(objpge);
        //                                                        var data1 = _lmContext.SaveChanges();
        //                                                        p60a = 0;
        //                                                    }
        //                                                }
        //                                            }

        //                                        }
        //                                    }
        //                                    //  con.Close();
        //                                }
        //                                //----------------------------------------------------------------------------------------------------------------------------------------
        //                            }
        //                        }

        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            Console.WriteLine(ex.Message);
        //                        }
        //                    }


        //                }
        //                catch (Exception ee)
        //                {
        //                    Console.WriteLine(ee.Message);
        //                }
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //    return data;
        //}

        public async Task<LeavepolicyDTO> getpolicyAsync(LeavepolicyDTO data)
        {
            double total;
            double perday;
            DateTime date;
            perday = 0;
            double p60a = 0;
            string flag = "";
            int cnt = 0;
            total = 0;

            //double abtotal=0;
            string empcode = "";
            string name = "";
            string mobileno = "";
            int dept_code = 0;
            try
            {


                DateTime fromdatecon = DateTime.Now;
                string confromdate = "";
                if (data.l_date != null)
                {
                    try
                    {
                        fromdatecon = DateTime.Now.Date;
                        //confromdate = fromdatecon.ToString();
                        confromdate = fromdatecon.ToString("yyyy-MM-dd");
                    }

                    catch (Exception ex)
                    {
                        _log.LogInformation("confromdate : " + confromdate);
                        _log.LogInformation("confromdate Error: " + ex.Message);
                        Console.WriteLine(ex.Message);
                    }
                }



                var query1 = (from e in _db.HR_Master_Employee_DMO
                              from m in _db.Multiple_Mobile_DMO
                              where e.HRME_Id == m.HRME_Id && e.MI_Id == data.MI_Id && e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false
                              select new LeavepolicyDTO
                              {
                                  HRME_Id = e.HRME_Id,
                                  HRME_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null || e.HRME_EmployeeFirstName == "" ? "" : " " + e.HRME_EmployeeFirstName)
                                     + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == "" || e.HRME_EmployeeMiddleName == "0" ? "" : " " + e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == "" || e.HRME_EmployeeLastName == "0" ? "" : " " + e.HRME_EmployeeLastName)).Trim(),
                                  HRME_MobileNo = m.HRMEMNO_MobileNo,
                                  HRME_EmployeeCode = e.HRME_EmployeeCode,
                                  HRME_EmailId = e.HRME_EmailId,
                                  HRMDES_Id = e.HRMDES_Id

                              }
                ).ToList();

                string hrmeid = "0";
                string multiplehrmeid = "";
                for (int kid = 0; kid < query1.Count(); kid++)
                {
                    hrmeid = hrmeid + "," + query1[kid].HRME_Id.ToString();
                }
                multiplehrmeid = hrmeid;

                if (query1.Count > 0)
                {
                    List<LeavepolicyDTO> result = new List<LeavepolicyDTO>();
                    try
                    {
                        using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandTimeout = 300000;
                            cmd.CommandText = "Absent_Lop_Report";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            using (var dataReader = cmd.ExecuteReader())
                            {

                            }

                        }

                        using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandTimeout = 300000;
                            cmd.CommandText = "EmpLogForSalCalc";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@date",
                                SqlDbType.VarChar)
                            {
                                Value = confromdate
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
                                Value = ""
                            });
                            cmd.Parameters.Add(new SqlParameter("@todate",
                          SqlDbType.NVarChar)
                            {
                                Value = ""
                            });
                            cmd.Parameters.Add(new SqlParameter("@miid",
                          SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@multiplehrmeid",
                          SqlDbType.VarChar)
                            {
                                Value = multiplehrmeid
                            });

                            cmd.Parameters.Add(new SqlParameter("@punchtype",
                          SqlDbType.VarChar)
                            {
                                Value = "late"
                            });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var PolocyList = new List<dynamic>();
                            try
                            {

                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        try
                                        {
                                            result.Add(new LeavepolicyDTO
                                            {
                                                HRME_Id = Convert.ToInt64(dataReader["HRME_Id"]),
                                                HRML_Id = Convert.ToInt64(dataReader["HRML_Id"]),
                                                HRELT_LeaveReason = dataReader["LType"].ToString(),
                                                HRME_PFDate = Convert.ToDateTime(dataReader["punchdate"]),
                                                MI_Id = data.MI_Id,

                                            });
                                        }
                                        catch(Exception ex)
                                        {                                          
                                            _log.LogInformation("continue Error: " + ex.Message);
                                            continue;
                                        }
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            if (result.Count > 0)
                            {
                                for(int i=0;i< result.Count; i++)
                                {
                                    var que1 = _FOContext.HR_Master_Employee_DMO.Where(a => a.HRME_Id == result[i].HRME_Id && a.MI_Id==data.MI_Id).Select(t => t.HRME_MobileNo).FirstOrDefault();
                                    if (que1 != null && que1.ToString() != "")
                                    {
                                        SMS msg = new SMS(_context);
                                        long mob_no =(long) que1;
                                        //long mob_no = 9686061628;
                                        long MI_Id = data.MI_Id;
                                        //string Template = "LeavePolicy";
                                        long UserID = 0;
                                        string s=await msg.sendSms(MI_Id, mob_no, "LeavePolicy", UserID);

                                    }
                                }
                            }
                           

                        }


                    }
                    catch (Exception ee)
                    {
                        _log.LogInformation("Final Error: " + ee.Message);
                        Console.WriteLine(ee.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Final Error1: " + ex.Message);
            }
            return data;
        }

        public void SENDSMS_Staff(long HRME_Id, long HRME_MobileNo, string Message, DateTime l_date)
        {
            //return data;
        }


    }
}
