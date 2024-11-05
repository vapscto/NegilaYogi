using PreadmissionDTOs.com.vaps.College.Portals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;
using PreadmissionDTOs.com.vaps.College.Portals.Staff;
using PreadmissionDTOs.com.vaps.College.Portals.Student;
using DomainModel.Model.com.vapstech.MobileApp;
using DataAccessMsSqlServerProvider;
using CommonLibrary;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;

namespace CollegePortals.com.Staff.Services
{
    public class ClgStaffDashboardImpl : Interfaces.ClgStaffDashboardInterface
    {
        private static ConcurrentDictionary<string, ClgPortalAttendanceDTO> _login =
           new ConcurrentDictionary<string, ClgPortalAttendanceDTO>();
        private readonly DomainModelMsSqlServerContext _db;
        private CollegeportalContext _ClgPortalContext;
        private PortalContext _PortalContext;
        public ClgStaffDashboardImpl(CollegeportalContext ClgPortalContext, DomainModelMsSqlServerContext db, PortalContext _Portal)
        {
            _ClgPortalContext = ClgPortalContext;
            _db = db;
            _PortalContext = _Portal;
        }

        public async Task<ClgStaffDashboardDTO> getloaddata(ClgStaffDashboardDTO data)
        {
            try
            {
                data.HRME_Id = _ClgPortalContext.Staff_User_Login.FirstOrDefault(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;

                #region Employee Details
                using (var cmd1 = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "CLG_PORTAL_EMPLOYEEDETAILS";
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd1.Parameters.Add(new SqlParameter("@HRME_Id",
               SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });

                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd1.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.employeedetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                data.empmobileno = (from a in _ClgPortalContext.Emp_MobileNo
                                    where (a.HRME_Id == data.HRME_Id && a.HRMEMNO_DeFaultFlag == "default")
                                    select new ClgStaffDashboardDTO
                                    {
                                        HRMEMNO_MobileNo = a.HRMEMNO_MobileNo,
                                    }).Distinct().ToArray();


                data.empemailid = (from a in _ClgPortalContext.Emp_Email_Id

                                   where (a.HRME_Id == data.HRME_Id && a.HRMEM_DeFaultFlag == "default")
                                   select new ClgStaffDashboardDTO
                                   {
                                       HRMEM_EmailId = a.HRMEM_EmailId,
                                   }).Distinct().ToArray();
                #endregion

                #region COE EVENTS and Calendar Details
                //============================= COE Current Month Events
                int month = DateTime.Now.Month;

              //  var year = _ClgPortalContext.academicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Order == 4).Select(a => a.ASMAY_Id).SingleOrDefault();
                var checkcoe = (from m in _ClgPortalContext.COE_Master_EventsDMO
                                from n in _ClgPortalContext.COE_EventsDMO
                                from y in _ClgPortalContext.COE_Events_EmployeesDMO
                                where m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && n.COEE_EStartDate.Value.Month == month && n.COEE_Id == y.COEE_Id
                                select new ClgStudentDashboardDTO
                                {
                                    COEME_Id = m.COEME_Id,
                                    COEME_EventName = m.COEME_EventName,
                                    COEME_EventDesc = m.COEME_EventDesc,
                                    COEE_EStartDate = n.COEE_EStartDate,
                                    COEE_EEndDate = n.COEE_EEndDate,
                                    COEE_ReminderDate = n.COEE_ReminderDate,
                                }).Distinct().OrderBy(c => c.COEME_Id).ToArray();
                if (checkcoe.Length > 0)
                {
                    data.coereportlist = (from m in _ClgPortalContext.COE_Master_EventsDMO
                                          from n in _ClgPortalContext.COE_EventsDMO
                                          from y in _ClgPortalContext.COE_Events_EmployeesDMO
                                          where m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && n.COEE_EStartDate.Value.Month == month && n.COEE_Id == y.COEE_Id
                                          select new ClgStudentDashboardDTO
                                          {
                                              COEME_Id = m.COEME_Id,
                                              COEME_EventName = m.COEME_EventName,
                                              COEME_EventDesc = m.COEME_EventDesc,
                                              COEE_EStartDate = n.COEE_EStartDate,
                                              COEE_EEndDate = n.COEE_EEndDate,
                                              COEE_ReminderDate = n.COEE_ReminderDate,
                                          }).Distinct().OrderBy(c => c.COEE_EStartDate).ToArray();

                    data.coereportlist1 = (from m in _ClgPortalContext.COE_Master_EventsDMO
                                          from n in _ClgPortalContext.COE_EventsDMO
                                          from y in _ClgPortalContext.COE_Events_EmployeesDMO
                                          where m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && n.COEE_EStartDate.Value.Month == month && n.COEE_Id == y.COEE_Id
                                          select new ClgStudentDashboardDTO
                                          {
                                              COEME_Id = m.COEME_Id,
                                              COEME_EventName = m.COEME_EventName,
                                              COEME_EventDesc = m.COEME_EventDesc,
                                              COEE_EStartDate = n.COEE_EStartDate,
                                              COEE_EEndDate = n.COEE_EEndDate,
                                              COEE_ReminderDate = n.COEE_ReminderDate,
                                          }).Distinct().OrderBy(c => c.COEME_Id).ToArray();

                }

                var checkcoelist = (from m in _ClgPortalContext.COE_Master_EventsDMO
                                    from n in _ClgPortalContext.COE_EventsDMO
                                    from y in _ClgPortalContext.COE_Events_EmployeesDMO
                                    where (m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.COEE_EStartDate != null && n.COEE_Id == y.COEE_Id)
                                    select new ClgStudentDashboardDTO
                                    {
                                        COEME_EventName = m.COEME_EventName,
                                        COEME_EventDesc = m.COEME_EventDesc,
                                        COEE_EStartDate = n.COEE_EStartDate,
                                        COEE_EEndDate = n.COEE_EEndDate,
                                        COEE_ReminderDate = n.COEE_ReminderDate
                                    }).OrderByDescending(c => c.COEE_EStartDate).Distinct().ToArray();
                if (checkcoelist.Length > 0)
                {
                    //============================ COE All Years Events
                    data.calenderlist = (from m in _ClgPortalContext.COE_Master_EventsDMO
                                         from n in _ClgPortalContext.COE_EventsDMO
                                         from y in _ClgPortalContext.COE_Events_EmployeesDMO
                                         where (m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.COEE_EStartDate != null && n.COEE_Id == y.COEE_Id)
                                         select new ClgStudentDashboardDTO
                                         {
                                             COEME_EventName = m.COEME_EventName,
                                             COEME_EventDesc = m.COEME_EventDesc,
                                             COEE_EStartDate = n.COEE_EStartDate,
                                             COEE_EEndDate = n.COEE_EEndDate,
                                             COEE_ReminderDate = n.COEE_ReminderDate
                                         }).OrderByDescending(c => c.COEE_EStartDate).Distinct().ToArray();
                }

                #endregion

                //var checkcoelist = (from m in _ClgPortalContext.COE_Master_EventsDMO
                //                    from n in _ClgPortalContext.COE_EventsDMO
                //                    from y in _ClgPortalContext.COE_Events_EmployeesDMO
                //                    where (m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.COEE_EStartDate != null && n.COEE_Id == y.COEE_Id)
                //                    select new ClgStudentDashboardDTO
                //                    {
                //                        COEME_EventName = m.COEME_EventName,
                //                        COEME_EventDesc = m.COEME_EventDesc,
                //                        COEE_EStartDate = n.COEE_EStartDate,
                //                        COEE_EEndDate = n.COEE_EEndDate,
                //                        COEE_ReminderDate = n.COEE_ReminderDate
                //                    }).OrderByDescending(c => c.COEE_EStartDate).Distinct().ToArray();
                //if (checkcoelist.Length > 0)
                //{
                //    //============================ COE All Years Events
                //    data.calenderlist = (from m in _ClgPortalContext.COE_Master_EventsDMO
                //                         from n in _ClgPortalContext.COE_EventsDMO
                //                         from y in _ClgPortalContext.COE_Events_EmployeesDMO
                //                         where (m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.COEE_EStartDate != null && n.COEE_Id == y.COEE_Id)
                //                         select new ClgStudentDashboardDTO
                //                         {
                //                             COEME_EventName = m.COEME_EventName,
                //                             COEME_EventDesc = m.COEME_EventDesc,
                //                             COEE_EStartDate = n.COEE_EStartDate,
                //                             COEE_EEndDate = n.COEE_EEndDate,
                //                             COEE_ReminderDate = n.COEE_ReminderDate
                //                         }).OrderByDescending(c => c.COEE_EStartDate).Distinct().ToArray();
                //}
                //data.coereportlist = (from m in _ClgPortalContext.COE_Master_EventsDMO
                //                      from n in _ClgPortalContext.COE_EventsDMO
                //                      from y in _ClgPortalContext.COE_Events_EmployeesDMO
                //                      where m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && n.COEE_EStartDate.Value.Month == month && n.COEE_Id==y.COEE_Id 
                //                      select new ClgStudentDashboardDTO
                //                      {
                //                          COEME_Id = m.COEME_Id,
                //                          COEME_EventName = m.COEME_EventName,
                //                          COEME_EventDesc = m.COEME_EventDesc,
                //                          COEE_EStartDate = n.COEE_EStartDate,
                //                          COEE_EEndDate = n.COEE_EEndDate,
                //                          COEE_ReminderDate = n.COEE_ReminderDate,                                         
                //                      }).Distinct().OrderBy(c => c.COEME_Id).ToArray();

                ////============================ COE All Years Events
                //data.calenderlist = (from m in _ClgPortalContext.COE_Master_EventsDMO
                //                     from n in _ClgPortalContext.COE_EventsDMO
                //                     from y in _ClgPortalContext.COE_Events_EmployeesDMO
                //                     where (m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.COEE_EStartDate != null && n.COEE_Id == y.COEE_Id)
                //                     select new ClgStudentDashboardDTO
                //                     {
                //                         COEME_EventName = m.COEME_EventName,
                //                         COEME_EventDesc = m.COEME_EventDesc,
                //                         COEE_EStartDate = n.COEE_EStartDate,
                //                         COEE_EEndDate = n.COEE_EEndDate,
                //                         COEE_ReminderDate = n.COEE_ReminderDate
                //                     }).OrderByDescending(c => c.COEE_EStartDate).Distinct().ToArray();
                //#endregion
                #region  Notice Board and Assignment

                var notice = (from a in _ClgPortalContext.IVRM_NoticeBoardDMO
                              from b in _ClgPortalContext.MasterEmployee
                              from c in _ClgPortalContext.Adm_College_Atten_Login_UserDMO
                              from d in _ClgPortalContext.Adm_College_Atten_Login_DetailsDMO
                              from e in _ClgPortalContext.IVRM_NoticeBoard_CoBranchDMO
                              where (b.HRME_Id == c.HRME_Id && c.ACALU_Id == d.ACALU_Id && d.ACALD_ActiveFlag == true && d.AMCO_Id == e.AMCO_Id && d.AMB_Id == e.AMB_Id && e.AMSE_Id == e.AMSE_Id && e.INTBCB_ActiveFlag == true && a.INTB_Id == e.INTB_Id && a.INTB_ActiveFlag == true && a.MI_Id == data.MI_Id && b.HRME_Id == data.HRME_Id)
                              select new ClgStudentDashboardDTO
                              {
                                  //MI_Id = a.MI_Id,
                                  INTB_Id = a.INTB_Id,
                                  INTB_Title = a.INTB_Title,
                                  INTB_Description = a.INTB_Description,
                                  NTB_TTSylabusFlg = a.NTB_TTSylabusFlg,
                                  INTB_Attachment = a.INTB_Attachment,
                                  INTB_FilePath = a.INTB_FilePath,
                                  INTB_DisplayDate = a.INTB_DisplayDate,
                                  INTB_StartDate = a.INTB_StartDate,
                                  INTB_EndDate = a.INTB_EndDate,
                              }).Distinct().OrderByDescending(d => d.INTB_StartDate).ToArray();

                if (notice.Length > 0)
                {
                    data.noticelist = (from a in _ClgPortalContext.IVRM_NoticeBoardDMO
                                       from b in _ClgPortalContext.MasterEmployee
                                       from c in _ClgPortalContext.Adm_College_Atten_Login_UserDMO
                                       from d in _ClgPortalContext.Adm_College_Atten_Login_DetailsDMO
                                       from e in _ClgPortalContext.IVRM_NoticeBoard_CoBranchDMO
                                       where (b.HRME_Id == c.HRME_Id && c.ACALU_Id == d.ACALU_Id && d.ACALD_ActiveFlag == true && d.AMCO_Id == e.AMCO_Id && d.AMB_Id == e.AMB_Id && e.AMSE_Id == e.AMSE_Id && e.INTBCB_ActiveFlag == true  && a.INTB_ActiveFlag == true && a.MI_Id == data.MI_Id && b.HRME_Id == data.HRME_Id) /*&& a.INTB_Id == e.INTB_Id*/
                                       select new ClgStudentDashboardDTO
                                       {
                                           //MI_Id = a.MI_Id,
                                           INTB_Id = a.INTB_Id,
                                           INTB_Title = a.INTB_Title,
                                           INTB_Description = a.INTB_Description,
                                           NTB_TTSylabusFlg = a.NTB_TTSylabusFlg,
                                           INTB_Attachment = a.INTB_Attachment,
                                           INTB_FilePath = a.INTB_FilePath,
                                           INTB_DisplayDate = a.INTB_DisplayDate,
                                           INTB_StartDate = a.INTB_StartDate,
                                           INTB_EndDate = a.INTB_EndDate,
                                       }).Distinct().OrderByDescending(d => d.INTB_StartDate).ToArray();
                }
                #endregion

                //#region  Notice Board and Assignment
                //data.noticelist = (from a in _ClgPortalContext.IVRM_NoticeBoardDMO
                //                   from b in _ClgPortalContext.MasterEmployee
                //                   from c in _ClgPortalContext.Adm_College_Atten_Login_UserDMO
                //                   from d in _ClgPortalContext.Adm_College_Atten_Login_DetailsDMO
                //                   from e in _ClgPortalContext.IVRM_NoticeBoard_CoBranchDMO
                //                   where (b.HRME_Id == c.HRME_Id && c.ACALU_Id == d.ACALU_Id && d.ACALD_ActiveFlag == true && d.AMCO_Id == e.AMCO_Id && d.AMB_Id == e.AMB_Id && e.AMSE_Id == e.AMSE_Id && e.INTBCB_ActiveFlag == true && a.INTB_Id == e.INTB_Id && a.INTB_ActiveFlag == true && a.MI_Id == data.MI_Id && b.HRME_Id == data.HRME_Id)
                //                   select new ClgStudentDashboardDTO
                //                   {
                //                       MI_Id = a.MI_Id,
                //                       INTB_Id = a.INTB_Id,
                //                       INTB_Title = a.INTB_Title,
                //                       INTB_Description = a.INTB_Description,
                //                       NTB_TTSylabusFlg = a.NTB_TTSylabusFlg,
                //                       INTB_Attachment = a.INTB_Attachment,
                //                       INTB_FilePath = a.INTB_FilePath,
                //                       INTB_DisplayDate = a.INTB_DisplayDate,
                //                       INTB_StartDate = a.INTB_StartDate,
                //                       INTB_EndDate = a.INTB_EndDate,
                //                   }).Distinct().OrderByDescending(d => d.INTB_StartDate).ToArray();
                //#endregion


                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_StudentCount_College";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.fillstudent = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (data.fillstudent.Length >= 0)
                {
                    data.studentcount = data.fillstudent.Length;
                }

                #region MobileapppagePrivileges
                List<IVRM_User_MobileApp_Login_Privileges> Staffmobileappprivileges = new List<IVRM_User_MobileApp_Login_Privileges>();
                Staffmobileappprivileges = _db.IVRM_User_MobileApp_Login_Privileges.Where(t => t.IVRMUL_Id == data.UserId && t.MI_Id == data.MI_Id).ToList();
                if (Staffmobileappprivileges.Count() > 0)
                {
                    data.Staffmobileappprivileges = (from Mobilepage in _db.IVRM_MobileApp_Page
                                                     from MobileRolePrivileges in _db.IVRM_Role_MobileApp_Privileges
                                                     from UserRolePrivileges in _db.IVRM_User_MobileApp_Login_Privileges
                                                     where (MobileRolePrivileges.MI_ID == UserRolePrivileges.MI_Id && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id && Mobilepage.IVRMMAP_Id == UserRolePrivileges.IVRMMAP_Id && MobileRolePrivileges.IVRMRT_Id == data.roleid && MobileRolePrivileges.MI_ID == data.MI_Id && UserRolePrivileges.IVRMUL_Id == data.UserId)
                                                     select new ClgStaffDashboardDTO
                                                     {
                                                         Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                         Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                         Pageurl = Mobilepage.IVRMMAP_AppPageURL,
                                                         IVRMRMAP_Id = MobileRolePrivileges.IVRMRMAP_Id
                                                     }).OrderBy(d => d.IVRMRMAP_Id).ToArray();
                    data.mobileprivileges = "true";
                }
                else
                {
                    data.mobileprivileges = "false";
                }
                #endregion

                #region Payment Notfication
                //if (data.PaymentNootificationCollegeStaff == 0)
                //{
                //    SubscriptionPaymentNotification _sub = new SubscriptionPaymentNotification(_PortalContext);
                //    data.getpaymentnotificationdetails = _sub.getnotificationdetails(data.MI_Id, data.UserId);
                //}
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



    }
}
