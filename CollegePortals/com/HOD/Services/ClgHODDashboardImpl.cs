using PreadmissionDTOs.com.vaps.College.Portals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vaps.admission;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;
using DomainModel.Model.com.vapstech.Portals.Employee;
using DomainModel.Model.com.vapstech.Portals.HOD;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Portals.IVRM;
using PreadmissionDTOs.com.vaps.Portals.Employee;

namespace CollegePortals.com.Student.Services
{
    public class ClgHODDashboardImpl : Interfaces.ClgHODDashboardInterface
    {
        private static ConcurrentDictionary<string, ClgStudentDashboardDTO> _login =
           new ConcurrentDictionary<string, ClgStudentDashboardDTO>();
        private CollegeportalContext _ClgPortalContext;
        public ClgHODDashboardImpl(CollegeportalContext ClgPortalContext)
        {
            _ClgPortalContext = ClgPortalContext;
        }
        public async Task<ClgStudentDashboardDTO> Getdetails(ClgStudentDashboardDTO data)
        {
            try
            {

                List<MasterAcademic> acade = new List<MasterAcademic>();
                acade = _ClgPortalContext.academicYearDMO.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToList();
                data.yearlist = acade.ToArray();



                var year = _ClgPortalContext.HR_MasterLeaveYear.Where(t => t.MI_Id == data.MI_Id && t.HRMLY_ActiveFlag == true).OrderBy(t => t.HRMLY_Id).ToList();
              //  data.yearlist = year.ToArray();

                if (data.HRMLY_LeaveYear == null || data.HRMLY_LeaveYear == "")
                {
                    var HRMLY_Id = year[0].HRMLY_Id;
                    var HRMLY_LeaveYear = year[0].HRMLY_LeaveYear;
                    data.HRMLY_LeaveYear = HRMLY_LeaveYear;
                }

                var emp_Id = _ClgPortalContext.Staff_User_Login.Where(c => c.Id == data.user_id && c.MI_Id == data.MI_Id).Distinct().ToList();

                if (emp_Id.Count>0)
                {
                    data.HRME_Id = emp_Id.FirstOrDefault().Emp_Code;
                }
               

                var currentyear = DateTime.Now.Year;
                //currentyear = 2017;
                data.salarylist = (from m in _ClgPortalContext.HR_Employee_Salary
                                  from n in _ClgPortalContext.HR_Employee_Salary_Details
                                  from o in _ClgPortalContext.HR_Master_EarningsDeductions
                                  where m.HRES_Id == n.HRES_Id && n.HRMED_Id == o.HRMED_Id && m.MI_Id == data.MI_Id && m.HRME_Id == data.HRME_Id && m.HRES_Year == Convert.ToString(currentyear) && o.HRMED_EarnDedFlag.Equals("Earning")
                                  group new { m, n, o }
                                    by new { m.HRES_Month } into g
                                  select new EmployeeDashboardDTO
                                  {
                                      salary = g.Sum(d => d.n.HRESD_Amount),
                                      monthName = g.FirstOrDefault().m.HRES_Month,

                                  }
                               ).ToArray();


                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_Employee_Dashboard";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
              SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.filldepartment = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                data.filldepartment = (from a in _ClgPortalContext.MasterEmployee
                                       from b in _ClgPortalContext.HR_Master_Department
                                       from c in _ClgPortalContext.HR_Master_DesignationDMO

                                       where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_ActiveFlag == true && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id)
                                      select new EmployeeDashboardDTO
                                      {

                                          HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),

                                          HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                          HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                          HRME_DOJ = a.HRME_DOJ,
                                          HRMD_DepartmentName = b.HRMD_DepartmentName,
                                          HRMDES_DesignationName = c.HRMDES_DesignationName,
                                          HRME_EmployeeCode = a.HRME_EmployeeCode,
                                          HRME_DOB = a.HRME_DOB,
                                          HRME_PhotoNo = a.HRME_Photo,
                                          DeviceID = a.HRME_AppDownloadedDeviceId
                                      }).Distinct().ToArray();


                data.mobile = (from a in _ClgPortalContext.Emp_MobileNo
                              where (a.HRME_Id == data.HRME_Id && a.HRMEMNO_DeFaultFlag == "default")
                              select new EmployeeDashboardDTO
                              {
                                  HRME_MobileNo = a.HRMEMNO_MobileNo,
                              }).Distinct().ToArray();


                data.email = (from a in _ClgPortalContext.Emp_Email_Id

                             where (a.HRME_Id == data.HRME_Id && a.HRMEM_DeFaultFlag == "default")
                             select new EmployeeDashboardDTO
                             {
                                 HRME_EmailId = a.HRMEM_EmailId,
                             }).Distinct().ToArray();



                DateTime now = DateTime.Now;
                int A = now.Month;
                data.coedata = (from m in _ClgPortalContext.COE_Master_EventsDMO
                               from n in _ClgPortalContext.COE_EventsDMO
                               where m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && n.COEE_EStartDate.Value.Month == A
                               select new EmployeeDashboardDTO
                               {
                                   eventName = m.COEME_EventName,
                                   eventDesc = m.COEME_EventDesc,
                                   COEE_EStartDate = n.COEE_EStartDate,
                                   COEE_EEndDate = n.COEE_EEndDate,
                               }).ToArray();


                //data.stdabsentlist = (from m in _ClgPortalContext.COE_Master_EventsDMO
                //                from n in _ClgPortalContext.COE_EventsDMO
                //                where m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && n.COEE_EStartDate.Value.Month == A
                //                select new EmployeeDashboardDTO
                //                {
                //                    eventName = m.COEME_EventName,
                //                    eventDesc = m.COEME_EventDesc,
                //                    COEE_EStartDate = n.COEE_EStartDate,
                //                    COEE_EEndDate = n.COEE_EEndDate,
                //                }).ToArray();

                



                //data.calenderlist = (from m in _ClgPortalContext.COE_Master_EventsDMO
                //                    from n in _ClgPortalContext.COE_EventsDMO
                //                    from o in _ClgPortalContext.School_Adm_Y_StudentDMO
                //                    where (m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && o.ASMAY_Id == data.ASMAY_Id && n.COEE_EStartDate != null)
                //                    select new EmployeeDashboardDTO
                //                    {
                //                        COEME_EventName = m.COEME_EventName,
                //                        COEME_EventDesc = m.COEME_EventDesc,
                //                        COEE_EStartDate = n.COEE_EStartDate,
                //                        COEE_EEndDate = n.COEE_EEndDate,
                //                        COEE_ReminderDate = n.COEE_ReminderDate
                //                    }).OrderByDescending(c => c.COEE_EStartDate).Distinct().ToArray();









                //long loginData = _ClgPortalContext.Staff_User_Login.Where(d => d.Id == data.user_id).FirstOrDefault().Emp_Code;

                //data.studentdetails = (from a in _ClgPortalContext.HR_Master_Department
                //                       from b in _ClgPortalContext.MasterEmployee
                //                    where (a.HRMD_Id==b.HRMD_Id && a.MI_Id==data.MI_Id && a.HRMD_ActiveFlag==true && b.HRME_ActiveFlag==true && b.HRME_Id== loginData)
                //                    select new ClgStudentDashboardDTO
                //                    {
                //                        HRME_Id = b.HRME_Id,
                //                        HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName)
                //                         + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                //                        HRME_EmployeeCode = b.HRME_EmployeeCode,
                //                        HRMD_DepartmentName=a.HRMD_DepartmentName,
                //                        amst_mobile = b.HRME_MobileNo.ToString(),
                //                        amst_email_id = b.HRME_EmailId,
                //                    }
                //).OrderByDescending(t => t.HRME_Id).ToArray();




                var hod_stf_ids = _ClgPortalContext.HOD_DMO.Where(t => t.MI_Id == data.MI_Id).Select(t => t.HRME_Id).Distinct().ToList();
              
                data.hodlist = (from h in _ClgPortalContext.HOD_DMO
                                from a in _ClgPortalContext.MasterEmployee
                                where (a.HRME_Id == h.HRME_Id && h.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                select new ClgStudentDashboardDTO
                                {
                                    IHOD_Id = h.IHOD_Id,
                                    HRME_Id = h.HRME_Id,
                                    HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName)
                                      + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                    IHOD_ActiveFlag = h.IHOD_ActiveFlag,
                                    IHOD_Flg = h.IHOD_Flg,
                                    HRME_EmployeeCode = a.HRME_EmployeeCode

                                }
                   ).Distinct().ToArray();



              //  var hrme_id = _ClgPortalContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id==data.user_id).Select(t => t.Emp_Code);

            //   var hrme_id = _ClgPortalContext.Staff_User_Login.Where(c => c.Id == data.user_id && c.MI_Id == data.MI_Id).FirstOrDefault().Emp_Code;

            //&& b.HRME_Id == data.HRME_Id

                var saved_hods_stf = (from a in _ClgPortalContext.IVRM_HOD_Staff_DMO
                                      from b in _ClgPortalContext.HOD_DMO
                                      from c in _ClgPortalContext.MasterEmployee
                                      where (a.IHOD_Id == b.IHOD_Id && b.MI_Id == data.MI_Id && c.HRME_ActiveFlag == true && c.HRME_Id == a.HRME_Id )
                                      select new ClgStudentDashboardDTO
                                      {
                                          IHOD_Id = a.IHOD_Id,
                                          HRME_Id = a.HRME_Id,
                                          HRME_EmployeeFirstName = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : " " + c.HRME_EmployeeFirstName)
                                       + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == "" || c.HRME_EmployeeMiddleName == "0" ? "" : " " + c.HRME_EmployeeMiddleName) + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == "" || c.HRME_EmployeeLastName == "0" ? "" : " " + c.HRME_EmployeeLastName)).Trim(),
                                          IHODS_ActiveFlag = a.IHODS_ActiveFlag,
                                          IHOD_Flg = b.IHOD_Flg,
                                          HRME_EmployeeCode=c.HRME_EmployeeCode,
                                      }).Distinct().ToList();
                data.saved_hods_stf = saved_hods_stf.ToArray();


                var branchlist = (from a in _ClgPortalContext.ClgMasterBranchDMO
                                  from b in _ClgPortalContext.CLG_Adm_College_AY_CourseDMO
                                  from c in _ClgPortalContext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                  select new ClgMasterBranchDMO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_BranchCode = a.AMB_BranchCode,
                                      AMB_BranchInfo = a.AMB_BranchInfo,
                                      AMB_BranchType = a.AMB_BranchType,
                                      AMB_StudentCapacity = a.AMB_StudentCapacity,
                                      AMB_Order = a.AMB_Order,
                                      AMB_AidedUnAided = a.AMB_AidedUnAided
                                  }).Distinct().ToList();
                data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();


                data.query01 = (from a in _ClgPortalContext.MasterEmployee
                                where (a.HRME_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRME_LeftFlag == false)
                                select new ClgStudentDashboardDTO
                                {
                                    HRME_Id = a.HRME_Id,
                                    HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName)
                                         + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                    HRME_EmployeeCode = a.HRME_EmployeeCode
                                }
                    ).Distinct().ToArray();



                var saved_hods = (from a in _ClgPortalContext.HOD_DMO
                                  from b in _ClgPortalContext.MasterEmployee
                                  from c in _ClgPortalContext.IVRM_HOD_Branch_DMO
                                  where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.IHOD_Id == c.IHOD_Id && b.HRME_ActiveFlag == true && b.HRME_Id == a.HRME_Id)
                                  select new ClgStudentDashboardDTO
                                  {
                                      IHOD_Id = a.IHOD_Id,
                                      HRME_Id = a.HRME_Id,
                                      HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName)
                                       + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                      IHOD_ActiveFlag = a.IHOD_ActiveFlag,
                                      IHOD_Flg = a.IHOD_Flg,
                                  }).Distinct().ToList();
                data.saved_hods = saved_hods.ToArray();


                var saved_hods_cls = (from a in _ClgPortalContext.IVRM_HOD_Branch_DMO
                                      from b in saved_hods
                                      from c in _ClgPortalContext.ClgMasterBranchDMO
                                      where (a.IHOD_Id == b.IHOD_Id && c.MI_Id == data.MI_Id && c.AMB_ActiveFlag == true && c.AMB_Id == a.AMB_Id)
                                      select new ClgStudentDashboardDTO
                                      {
                                          IHOD_Id = a.IHOD_Id,
                                          AMB_Id = a.AMB_Id,
                                          branchname = c.AMB_BranchName,
                                          IHODB_ActiveFlag = a.IHODB_ActiveFlag,
                                          IHOD_Flg = b.IHOD_Flg,
                                      }).Distinct().ToList();
                data.saved_hods_cls = saved_hods_cls.ToArray();


                data.hodbranch = (from p in _ClgPortalContext.HOD_DMO
                                 from q in _ClgPortalContext.IVRM_HOD_Branch_DMO
                                 from r in _ClgPortalContext.ClgMasterBranchDMO
                                 from s in _ClgPortalContext.IVRM_HOD_Staff_DMO
                                 from a in _ClgPortalContext.MasterEmployee
                                 where (p.IHOD_Id == q.IHOD_Id && q.AMB_Id == r.AMB_Id && p.IHOD_Id == s.IHOD_Id && s.HRME_Id == a.HRME_Id && p.MI_Id == data.MI_Id)
                                 select new ClgStudentDashboardDTO
                                 {
                                     IHOD_Id = p.IHOD_Id,
                                     HRME_Id = a.HRME_Id,
                                     branchname = r.AMB_BranchName,
                                     HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName)
                                       + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                     IHODB_ActiveFlag = q.IHODB_ActiveFlag,
                                     HRME_EmployeeCode = a.HRME_EmployeeCode,
                                     IHODS_ActiveFlag = s.IHODS_ActiveFlag

                                 }
               ).Distinct().ToArray();



                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_HOD_SALARY";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@year",
                     SqlDbType.VarChar)
                    {
                        Value = data.HRMLY_LeaveYear.Trim()
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.user_id
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.Fillstudentstrenth = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }



                //using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "CLG_HOD_SALARY_EMP";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //      SqlDbType.BigInt)
                //    {
                //        Value = data.MI_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@year",
                //     SqlDbType.VarChar)
                //    {
                //        Value = data.HRMLY_LeaveYear.Trim()
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                //      SqlDbType.BigInt)
                //    {
                //        Value = data.user_id
                //    });

                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        data.FillstudentstrenthEMP = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.Write(ex.Message);
                //    }
                //}


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }




        public ClgStudentDashboardDTO savedata(ClgStudentDashboardDTO data)
        {
            try
            {
                var empId = data.employee.Select(d => d.HRME_Id).ToList();
                var query = _ClgPortalContext.HOD_DMO.Where(q => empId.Contains(q.HRME_Id)).Select(d => d.HRME_Id).ToList();
                if (query.Count > 0)
                {

                }
                else
                {
                    for (int i = 0; i < data.employee.Count(); i++)
                    {

                        HOD_DMO objpge = new HOD_DMO();

                        objpge.HRME_Id = data.employee[i].HRME_Id;
                        objpge.IHOD_ActiveFlag = true;
                        objpge.MI_Id = data.MI_Id;
                        objpge.IHOD_Flg = data.IHOD_Flg;
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        _ClgPortalContext.Add(objpge);

                    }
                    var contactExists = _ClgPortalContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                        data.returnsavestatus = "saved";
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                data.hodlist = (from h in _ClgPortalContext.HOD_DMO
                                from a in _ClgPortalContext.MasterEmployee

                                where (a.HRME_Id == h.HRME_Id && h.IHOD_ActiveFlag == true && h.MI_Id == data.MI_Id && h.IHOD_ActiveFlag == true && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                select new ClgStudentDashboardDTO
                                {

                                    HRME_Id = h.HRME_Id,
                                    HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName)
                                      + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                    IHOD_ActiveFlag = h.IHOD_ActiveFlag,
                                    IHOD_Flg = h.IHOD_Flg,

                                }
                   ).Distinct().ToArray();
            }
            catch (Exception ex)
            {

                //throw;
            }
            return data;
        }

        public ClgStudentDashboardDTO mappHODdata(ClgStudentDashboardDTO data)
        {
            try
            {
                var empId = data.employee.Select(d => d.HRME_Id).ToList();
                if (empId.Count > 0)
                {
                    var query = _ClgPortalContext.IVRM_HOD_Staff_DMO.Where(q => empId.Contains(q.HRME_Id)).Select(d => d.HRME_Id).ToList();

                    if (query.Count == 0)
                    {

                    }
                    else
                    {
                        for (int i = 0; i < data.employee.Count(); i++)
                        {

                            IVRM_HOD_Staff_DMO objpge = new IVRM_HOD_Staff_DMO();

                            objpge.HRME_Id = data.employee[i].HRME_Id;
                            objpge.IHODS_ActiveFlag = true;
                            objpge.IHOD_Id = data.IHOD_Id;
                            objpge.CreatedDate = DateTime.Now;
                            objpge.UpdatedDate = DateTime.Now;
                            _ClgPortalContext.Add(objpge);

                        }
                        var contactExists = _ClgPortalContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                            data.returnsavestatus = "saved";
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }


                var clss_id = data.class_lst.Select(d => d.AMB_Id).ToList();
                if (clss_id.Count > 0)
                {
                    var query2 = _ClgPortalContext.IVRM_HOD_Branch_DMO.Where(p => clss_id.Contains(p.AMB_Id) && p.IHODB_ActiveFlag == true).Select(t => t.IHODB_Id).ToList();
                    if (query2.Count > 0)
                    {

                    }
                    else
                    {

                        for (int i = 0; i < data.class_lst.Count(); i++)
                        {

                            IVRM_HOD_Branch_DMO objpge1 = new IVRM_HOD_Branch_DMO();

                            objpge1.AMB_Id = data.class_lst[i].AMB_Id;
                            objpge1.IHODB_ActiveFlag = true;
                            objpge1.IHOD_Id = data.IHOD_Id;
                            objpge1.CreatedDate = DateTime.Now;
                            objpge1.UpdatedDate = DateTime.Now;
                            _ClgPortalContext.Add(objpge1);

                        }

                        for (int i = 0; i < data.employee.Count(); i++)
                        {

                            IVRM_HOD_Staff_DMO objpge = new IVRM_HOD_Staff_DMO();

                            objpge.HRME_Id = data.employee[i].HRME_Id;
                            objpge.IHODS_ActiveFlag = true;
                            objpge.IHOD_Id = data.IHOD_Id;
                            objpge.CreatedDate = DateTime.Now;
                            objpge.UpdatedDate = DateTime.Now;
                            _ClgPortalContext.Add(objpge);

                        }



                        var contactExists = _ClgPortalContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                            data.returnsavestatus = "saved";
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }

                



                data.hodlist = (from h in _ClgPortalContext.HOD_DMO
                                from a in _ClgPortalContext.MasterEmployee

                                where (a.HRME_Id == h.HRME_Id && h.IHOD_ActiveFlag == true && h.MI_Id == data.MI_Id)
                                select new ClgStudentDashboardDTO
                                {

                                    HRME_Id = h.HRME_Id,
                                    HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName)
                                      + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                    IHOD_ActiveFlag = h.IHOD_ActiveFlag,
                                    IHOD_Flg = h.IHOD_Flg,

                                }
                   ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                //throw;
            }
            return data;
        }

        public ClgStudentDashboardDTO updateHOD(ClgStudentDashboardDTO data)
        {
            try
            {
                var query = _ClgPortalContext.HOD_DMO.Where(a => a.IHOD_Id == data.IHOD_Id).ToList();
                if (query.Count > 0)
                {
                    var update = _ClgPortalContext.HOD_DMO.Single(s => s.IHOD_Id == data.IHOD_Id);
                    if (update.IHOD_ActiveFlag == true)
                    {
                        update.IHOD_ActiveFlag = false;
                    }
                    else
                    {
                        update.IHOD_ActiveFlag = true;
                    }
                    update.UpdatedDate = DateTime.Now;
                    _ClgPortalContext.Update(update);
                    var contactExists = _ClgPortalContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                        data.returnsavestatus = "updated";
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {

                //throw;
            }
            return data;
        }

        public ClgStudentDashboardDTO deactiveY(ClgStudentDashboardDTO data)
        {
            try
            {
                var result = _ClgPortalContext.HOD_DMO.Single(a => a.MI_Id == data.MI_Id && a.IHOD_Id == data.IHOD_Id);
                if (result.IHOD_ActiveFlag == true)
                {
                    result.IHOD_ActiveFlag = false;
                }
                else if (result.IHOD_ActiveFlag == false)
                {
                    result.IHOD_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _ClgPortalContext.Update(result);
                int rowAffected = _ClgPortalContext.SaveChanges();
                if (rowAffected > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }








    }
}
