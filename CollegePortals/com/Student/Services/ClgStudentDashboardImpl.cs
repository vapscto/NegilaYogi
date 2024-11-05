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
using DomainModel.Model.com.vapstech.MobileApp;
using DomainModel.Model.com.vapstech.College.Portals.IVRM;

namespace CollegePortals.com.Student.Services
{
    public class ClgStudentDashboardImpl : Interfaces.ClgStudentDashboardInterface
    {
        private static ConcurrentDictionary<string, ClgStudentDashboardDTO> _login =
           new ConcurrentDictionary<string, ClgStudentDashboardDTO>();
        private CollegeportalContext _ClgPortalContext;
        private readonly DomainModelMsSqlServerContext _db;
        public ClgStudentDashboardImpl(CollegeportalContext ClgPortalContext, DomainModelMsSqlServerContext db)
        {
            _ClgPortalContext = ClgPortalContext;
            _db = db;
        }
        public async Task<ClgStudentDashboardDTO> Getdetails(ClgStudentDashboardDTO data)
        {
            try
            {
                data.yearlist = _ClgPortalContext.academicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                #region Course Branch & Semester Id
                //var ids = (from a in _ClgPortalContext.Adm_Master_College_StudentDMO
                //           from b in _ClgPortalContext.Adm_Master_College_StudentDMO
                //           where (a.AMCST_Id == b.AMCST_Id && a.AMCO_Id == b.AMCO_Id && a.AMB_Id == b.AMB_Id && a.AMSE_Id == b.AMSE_Id && a.AMCST_ActiveFlag == true && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id)
                //           select new ClgStudentDashboardDTO
                //           {
                //               AMCST_Id = a.AMCST_Id,
                //               AMCO_Id = a.AMCO_Id,
                //               AMB_Id = a.AMB_Id,
                //               AMSE_Id = a.AMSE_Id
                //           }).Distinct().ToList();


                var ids = (from a in _ClgPortalContext.Adm_Master_College_StudentDMO
                           from b in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                           where (a.AMCST_Id == b.AMCST_Id && a.AMCST_ActiveFlag == true && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && b.ASMAY_Id == data.ASMAY_Id)
                           select new ClgStudentDashboardDTO
                           {
                               AMCST_Id = a.AMCST_Id,
                               AMCO_Id = b.AMCO_Id,
                               AMB_Id = b.AMB_Id,
                               AMSE_Id = b.AMSE_Id
                           }).Distinct().ToList();

                if (ids.Count() == 0)
                {
                    var yearid = _ClgPortalContext.Adm_College_Yearly_StudentDMO.Where(s => s.AMCST_Id == data.AMCST_Id).Select(d => d.ACYST_Id).ToArray().Max();
                    data.ASMAY_Id = _ClgPortalContext.Adm_College_Yearly_StudentDMO.Where(a => a.ACYST_Id == yearid).Select(s => s.ASMAY_Id).FirstOrDefault();


                }
                #endregion

                #region Student Details
                using (var cmd1 = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "CLG_PORTAL_STUDENTDETAILS";
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
               SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@AMCST_Id",
              SqlDbType.BigInt)
                    {
                        Value = data.AMCST_Id
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
                        data.studentdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion

                #region Student Bus Pass DETAILS
                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Student_BusPass_Details_studentwise";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
              SqlDbType.BigInt)
                    {
                        Value = data.AMCST_Id
                    });

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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.studentbuspassdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion
                #region Attendance Details
                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_PORTAL_STUDENT_MONTHLY_ATTENDANCE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMCST_Id
                    });

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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.attendancedetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion

                #region Fee Details

                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_PORTAL_STUDENT_MONTHLY_FEE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMCST_Id
                    });
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.feedetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion

                #region Notice Board

                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_PORTAL_NoticeBoard_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMCST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                    SqlDbType.BigInt)
                    {
                        Value = ids.FirstOrDefault().AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                  SqlDbType.BigInt)
                    {
                        Value = ids.FirstOrDefault().AMB_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                  SqlDbType.BigInt)
                    {
                        Value = ids.FirstOrDefault().AMSE_Id
                    });

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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.noticeboard = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion

               

                #region COE EVENTS and Calendar Details
                //============================= COE Current Month Events
                int month = DateTime.Now.Month;
                data.coereportlist = (from m in _ClgPortalContext.COE_Master_EventsDMO
                                      from n in _ClgPortalContext.COE_EventsDMO
                                      from y in _ClgPortalContext.COE_Events_CourseBranchDMO
                                      from o in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                                      where m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && o.ASMAY_Id == data.ASMAY_Id && n.COEE_Id == y.COEE_Id && o.AMCO_Id == y.AMCO_Id && o.AMB_Id == y.AMB_Id && o.AMCST_Id == data.AMCST_Id && n.COEE_EStartDate.Value.Month == month
                                      select new ClgStudentDashboardDTO
                                      {
                                          COEME_Id = m.COEME_Id,
                                          COEME_EventName = m.COEME_EventName,
                                          COEME_EventDesc = m.COEME_EventDesc,
                                          COEE_EStartDate = n.COEE_EStartDate,
                                          COEE_EEndDate = n.COEE_EEndDate,
                                          COEE_ReminderDate = n.COEE_ReminderDate,
                                          ASMAY_Id = o.ASMAY_Id,
                                      }).Distinct().OrderBy(c => c.COEME_Id).ToArray();

                //============================ COE All Years Events
                data.calenderlist = (from m in _ClgPortalContext.COE_Master_EventsDMO
                                     from n in _ClgPortalContext.COE_EventsDMO
                                     from y in _ClgPortalContext.COE_Events_CourseBranchDMO
                                     from o in _ClgPortalContext.Adm_Master_College_StudentDMO
                                     where (m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.COEE_Id == y.COEE_Id && o.AMCO_Id == y.AMCO_Id && o.AMB_Id == y.AMB_Id && n.COEE_EStartDate != null)
                                     select new ClgStudentDashboardDTO
                                     {
                                         COEME_EventName = m.COEME_EventName,
                                         COEME_EventDesc = m.COEME_EventDesc,
                                         COEE_EStartDate = n.COEE_EStartDate,
                                         COEE_EEndDate = n.COEE_EEndDate,
                                         COEE_ReminderDate = n.COEE_ReminderDate
                                     }).OrderByDescending(c => c.COEE_EStartDate).Distinct().ToArray();
                #endregion

                #region LIBRARY DETAILS
                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_Portal_LibraryDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
              SqlDbType.BigInt)
                    {
                        Value = data.AMCST_Id
                    });

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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.librarydetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion
               

                #region MobileapppagePrivileges
                List<IVRM_Role_MobileApp_Privileges> Staffmobileappprivileges = new List<IVRM_Role_MobileApp_Privileges>();
                Staffmobileappprivileges = _db.IVRM_Role_MobileApp_Privileges.Where(f => f.IVRMRT_Id == data.roleid && f.MI_ID == data.MI_Id).ToList();
                if (Staffmobileappprivileges.Count() > 0)
                {
                    data.Staffmobileappprivileges = (from Mobilepage in _db.IVRM_MobileApp_Page
                                                     from MobileRolePrivileges in _db.IVRM_Role_MobileApp_Privileges
                                                     where (Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id && MobileRolePrivileges.IVRMRT_Id == data.roleId && MobileRolePrivileges.MI_ID == data.MI_Id)
                                                     select new ClgStudentDashboardDTO
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public ClgStudentDashboardDTO ViewStudentProfile(ClgStudentDashboardDTO data)
        {
            try
            {

                //data.viewstudentjoineddetails = (from a in _ClgPortalContext.Adm_Master_College_StudentDMO
                //                                 from b in _ClgPortalContext.academicYearDMO
                //                                 from c in _ClgPortalContext.MasterCourseDMO
                //                                 where (a.ASMAY_Id == b.ASMAY_Id && a.AMCO_Id == c.AMCO_Id && a.AMCST_Id == data.AMCST_Id
                //                                 && a.MI_Id == data.MI_Id)
                //                                 select new ClgStudentDashboardDTO
                //                                 {
                //                                     studentname = ((a.AMCST_FirstName == null || a.AMCST_FirstName == "" ? "" : a.AMCST_FirstName) +
                //                                     (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "" ? "" : " " + a.AMCST_MiddleName) +
                //                                     (a.AMCST_LastName == null || a.AMCST_LastName == "" ? "" : " " + a.AMCST_LastName)).Trim(),
                //                                     AMCST_AdmNo = a.AMCST_AdmNo,
                //                                     AMCST_RegistrationNo = a.AMCST_RegistrationNo,
                //                                     ASMAY_Year = b.ASMAY_Year,
                //                                     AMCO_CourseName = c.AMCO_CourseName,
                //                                   //  AMST_Photoname = a.AMST_Photoname,
                //                                     AMCST_Sex = a.AMCST_Sex,
                //                                     AMCST_SOL = a.AMCST_SOL,
                //                                     AMCST_Date = a.AMCST_Date,
                //                                     AMCST_DOB = a.AMCST_DOB,
                //                                 }).Distinct().ToArray();


                var ids = (from a in _ClgPortalContext.Adm_Master_College_StudentDMO
                           from b in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                           where (a.AMCST_Id == b.AMCST_Id && a.AMCST_ActiveFlag == true && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && b.ASMAY_Id == data.ASMAY_Id)
                           select new ClgStudentDashboardDTO
                           {
                               AMCST_Id = a.AMCST_Id,
                               AMCO_Id = b.AMCO_Id,
                               AMB_Id = b.AMB_Id,
                               AMSE_Id = b.AMSE_Id
                           }).Distinct().ToList();

                if (ids.Count() == 0)
                {
                    var yearid = _ClgPortalContext.Adm_College_Yearly_StudentDMO.Where(s => s.AMCST_Id == data.AMCST_Id).Select(d => d.ACYST_Id).ToArray().Max();
                    data.ASMAY_Id = _ClgPortalContext.Adm_College_Yearly_StudentDMO.Where(a => a.ACYST_Id == yearid).Select(s => s.ASMAY_Id).FirstOrDefault();
                }


                using (var cmd1 = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "CLG_PORTAL_STUDENTDETAILS";
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
               SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@AMCST_Id",
              SqlDbType.BigInt)
                    {
                        Value = data.AMCST_Id
                    });

                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd1.ExecuteReader())
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
                        data.viewstudentjoineddetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.viewstudentdetails = _ClgPortalContext.Adm_Master_College_StudentDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id).ToArray();
                var viewstudentdetails_current = _ClgPortalContext.Adm_College_Yearly_StudentDMO.Where(a => a.AMCST_Id == data.AMCST_Id).OrderByDescending(s => s.AMCST_Id).ToArray();
                var viewstudentacademicyeardetails = _ClgPortalContext.Adm_College_Yearly_StudentDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id
                 && a.AMCST_Id == data.AMCST_Id).ToArray();

                data.viewstudentacademicyeardetails = (from a in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                                                       from b in _ClgPortalContext.academicYearDMO
                                                       from c in _ClgPortalContext.MasterCourseDMO
                                                       from d in _ClgPortalContext.ClgMasterBranchDMO
                                                       from e in _ClgPortalContext.CLG_Adm_Master_SemesterDMO
                                                       where (a.ASMAY_Id == b.ASMAY_Id && a.AMCO_Id == c.AMCO_Id && a.AMB_Id == d.AMB_Id && e.AMSE_Id == a.AMSE_Id
                                                       && a.AMCST_Id == data.AMCST_Id)
                                                       select new ClgStudentDashboardDTO
                                                       {
                                                           ASMAY_Year = b.ASMAY_Year,
                                                           AMCO_CourseName = c.AMCO_CourseName,
                                                           AMB_BranchName = d.AMB_BranchName,
                                                           AMSE_SEMName = e.AMSE_SEMName,
                                                           order = b.ASMAY_Order,
                                                           ASMAY_Id = a.ASMAY_Id,
                                                           // AMAY_RollNo = a.AMAY_RollNo,
                                                           AMAY_RollNo = a.ACYST_RollNo,
                                                           Status_Flag = a.ASMAY_Id == data.ASMAY_Id ? "Current Year" : ""
                                                       }).Distinct().OrderByDescending(a => a.order).ToArray();

                // data.viewstudentguardiandetails = _ClgPortalContext.StudentGuardianDMO.Where(a => a.AMST_Id == data.AMST_Id).ToArray();
                var AMB_Id = viewstudentdetails_current.FirstOrDefault().AMB_Id;
                var AMSE_Id = viewstudentdetails_current.FirstOrDefault().AMSE_Id;
                var AMCO_Id = viewstudentdetails_current.FirstOrDefault().AMCO_Id;
                var ASMAY_Id = viewstudentdetails_current.FirstOrDefault().ASMAY_Id;


                data.viewstudentsubjectdetails = (from a in _ClgPortalContext.Exm_Col_Studentwise_SubjectsDMO
                                                  from b in _ClgPortalContext.IVRM_Master_SubjectsDMO
                                                  where (a.ISMS_Id == b.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id
                                                  && a.ECSTSU_ActiveFlg == true && a.AMCO_Id == AMCO_Id
                                                  && a.AMSE_Id == AMSE_Id && a.AMB_Id == AMB_Id)
                                                  select new ClgStudentDashboardDTO
                                                  {
                                                      ISMS_Id = a.ISMS_Id,
                                                      ISMS_SubjectName = b.ISMS_SubjectName,
                                                      subjorder = b.ISMS_OrderFlag,
                                                      ECSTSU_ElectiveFlag = a.ECSTSU_ElectiveFlag
                                                  }).Distinct().OrderBy(a => a.subjorder).ToArray();

                ////Over All Attendance
                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_View_StudentWise_Attendance_Clg";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMCST_Id });

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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.viewstudentattendancetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                // Year Month Wise Attendance 
                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_View_StudentWise_Attendance_MonthWise_Clg";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMCST_Id });

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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.viewstudentattendanceMonthdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                ////Over All Fee
                //using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "Portal_View_StudentWise_FeeDetails";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                //    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

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
                //                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        data.viewstudentfeedetails = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}

                //// Year Wise Fee Paid  Details
                //using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "Portal_View_StudentWise_Fee_YearDetails";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                //    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

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
                //                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        data.viewstudenfeeyeardetails = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}

                ////Student Complaints
                //using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "Adm_StudentCompliants_View";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                //    {
                //        Value = data.MI_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                //    {
                //        Value = data.ASMAY_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                //    {
                //        Value = data.AMST_Id
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
                //        data.studentdivlist = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}

                ////Student Exam
                //using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "Portal_View_Exam_YearWise_Details";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                //    {
                //        Value = data.MI_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                //    {
                //        Value = data.ASMAY_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                //    {
                //        Value = data.AMST_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                //    {
                //        Value = data.student_staffflag
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
                //        data.viewstudentwiseexamdetails = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}

                ////Student Address
                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_StudentWise_Address_Details_clg";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = data.AMCST_Id });

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
                                    dataRow.Add(dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.viewstudentaddressdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public ClgStudentDashboardDTO onclick_syllabus(ClgStudentDashboardDTO dto)
        {
            try
            {


                //var clssec1 = (from a in _studentDashboardContext.Adm_M_Student
                //               from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                //               from c in _studentDashboardContext.School_M_Class
                //               from s in _studentDashboardContext.School_M_Section
                //               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == dto.MI_Id
                //               && b.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id && b.AMST_Id == dto.AMST_Id)
                //               select new StudentDashboardDTO
                //               {
                //                   ASMCL_Id = c.ASMCL_Id,
                //                   ASMCL_ClassName = c.ASMCL_ClassName,
                //                   ASMS_Id = s.ASMS_Id,
                //                   ASMC_SectionName = s.ASMC_SectionName
                //               }).Distinct().ToList();

                var clssec1 = _ClgPortalContext.Adm_College_Yearly_StudentDMO.Where(a => a.ASMAY_Id == dto.ASMAY_Id && a.AMCST_Id == dto.AMCST_Id
               && a.ACYST_ActiveFlag == 1).ToList();


                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {
                    long Course_Id = clssec1.FirstOrDefault().AMCO_Id;
                    long Branch_Id = clssec1.FirstOrDefault().AMB_Id;
                    long Semester_Id = clssec1.FirstOrDefault().AMSE_Id;

                    var date = DateTime.Now;
                    //using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                    //{
                    //    cmd.CommandText = "Portal_NoticeBoardYearWise_clg";
                    //    cmd.CommandType = CommandType.StoredProcedure;

                    //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    //    {
                    //        Value = dto.MI_Id
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar)
                    //    {
                    //        Value = Course_Id
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar)
                    //    {
                    //        Value = Branch_Id
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar)
                    //    {
                    //        Value = Semester_Id
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    //    {
                    //        Value = dto.ASMAY_Id
                    //    });

                    //    cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar)
                    //    {
                    //        Value = dto.AMCST_Id
                    //    });

                    //    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    //    {
                    //        Value = dto.flag
                    //    });

                    //    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)
                    //    {
                    //        Value = "Student"
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
                    //        dto.noticelist = retObject.ToArray();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(ex.Message);
                    //    }
                    //}


                    using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_NoticeBoardYearWise_clg";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.VarChar)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                        SqlDbType.VarChar)
                        {
                            Value = Course_Id
                            // Value=dto.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                        SqlDbType.VarChar)
                        {
                            Value = Branch_Id
                            // Value=dto.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                        SqlDbType.VarChar)
                        {
                            Value = Semester_Id
                            // Value=dto.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                        {
                            Value = dto.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                   SqlDbType.VarChar)
                        {
                            Value = dto.AMCST_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                        {
                            Value = dto.flag
                        });

                        cmd.Parameters.Add(new SqlParameter("@Type",
                    SqlDbType.VarChar)
                        {
                            Value = "Student"
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
                            dto.noticelist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;




        }

        public ClgStudentDashboardDTO onclick_notice(ClgStudentDashboardDTO dto)
        {
            try
            {

                var asmay_year = (from a in _ClgPortalContext.AcademicYear                                  where a.MI_Id == dto.MI_Id && a.ASMAY_ActiveFlag == 1 && a.ASMAY_To_Date >= DateTime.Now                                  select new ClgStudentDashboardDTO                                  {                                      ASMAY_Id = a.ASMAY_Id,                                      ASMAY_Year = a.ASMAY_Year                                  }).OrderBy(s => s.ASMAY_Year).ToArray();                if (asmay_year.Length > 0)                {                    for (long i = 0; i < asmay_year.Length; i++)                    {                        var exist = (from z in _ClgPortalContext.Adm_College_Yearly_StudentDMO                                     from y in _ClgPortalContext.Adm_Master_College_StudentDMO                                     where (z.AMCST_Id == y.AMCST_Id && y.AMCST_SOL == "S" && y.AMCST_ActiveFlag == true && y.ASMAY_Id == asmay_year[i].ASMAY_Id && y.AMCST_Id == dto.AMCST_Id)                                     select y).ToArray();                        if (exist.Length > 0)                        {                            dto.ASMAY_Id = asmay_year[i].ASMAY_Id;                        }                    }                }

                var clssec1 = _ClgPortalContext.Adm_College_Yearly_StudentDMO.Where(a => a.ASMAY_Id == dto.ASMAY_Id && a.AMCST_Id == dto.AMCST_Id
                && a.ACYST_ActiveFlag == 1).ToList();

                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {
                    long Course_Id = clssec1.FirstOrDefault().AMCO_Id;
                    long Branch_Id = clssec1.FirstOrDefault().AMB_Id;
                    long Semester_Id = clssec1.FirstOrDefault().AMSE_Id;

                    var date = DateTime.Now;
                    using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_NoticeBoardYearWise_clg";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.VarChar)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                        SqlDbType.VarChar)
                        {
                            Value = Course_Id
                            // Value=dto.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                        SqlDbType.VarChar)
                        {
                            Value = Branch_Id
                            // Value=dto.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                        SqlDbType.VarChar)
                        {
                            Value = Semester_Id
                            // Value=dto.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                        {
                            Value = dto.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                   SqlDbType.VarChar)
                        {
                            Value = dto.AMCST_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                        {
                            Value = dto.flag
                        });

                        cmd.Parameters.Add(new SqlParameter("@Type",
                    SqlDbType.VarChar)
                        {
                            Value = "Student"
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
                            dto.noticelist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public ClgStudentDashboardDTO onclick_noticeboard_seen(ClgStudentDashboardDTO dto)
        {
            try
            {
                //added
                var duplicate = _ClgPortalContext.IVRM_NoticeBoard_Student_College_ViewedDMO.Where(b => b.INTB_Id == dto.INTB_Id && b.ACMST_Id == dto.AMCST_Id && b.INTBCSTDCV_ActiveFlag == true).ToList();
                if (duplicate.Count > 0)
                {

                }
                else
                {
                    var check = _ClgPortalContext.IVRM_NoticeBoard_Student_College_ViewedDMO.Where(a => a.INTB_Id == dto.INTB_Id && a.ACMST_Id == dto.AMCST_Id).ToList();
                    if (check.Count > 0)
                    {
                        var cud = _ClgPortalContext.IVRM_NoticeBoard_Student_College_ViewedDMO.Single(t => t.INTB_Id.Equals(dto.INTB_Id) && t.ACMST_Id == dto.AMCST_Id);
                        //  IVRM_HomeWork_Upload_DMO cud = new IVRM_HomeWork_Upload_DMO();

                        cud.INTBCSTDCV_ActiveFlag = true;
                        cud.INTBCSTDCV_UpdatedDate = DateTime.Now;
                        cud.INTBCSTDCV_UpdatedBy = dto.user_id;
                        _ClgPortalContext.Update(cud);

                    }
                    else
                    {
                        IVRM_NoticeBoard_Student_College_ViewedDMO cud = new IVRM_NoticeBoard_Student_College_ViewedDMO();
                        cud.INTB_Id = dto.INTB_Id;
                        cud.ACMST_Id = dto.AMCST_Id;

                        cud.INTBCSTDCV_ActiveFlag = true;
                        cud.INTBCSTDCV_CreatedDate = DateTime.Now;
                        cud.INTBCSTDCV_UpdatedDate = DateTime.Now;
                        cud.INTBCSTDCV_UpdatedBy = dto.user_id;
                        cud.INTBCSTDCV_CreatedBy = dto.user_id;


                        _ClgPortalContext.Add(cud);
                    }


                    var std = _ClgPortalContext.SaveChanges();
                }

                //
                //var clssec1 = (from a in _ClgPortalContext.Adm_Master_College_StudentDMO
                //               from b in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                //               from c in _ClgPortalContext.MasterCourseDMO
                //               from s in _ClgPortalContext.ClgMasterBranchDMO
                //               from d in _ClgPortalContext.CLG_Adm_Master_SemesterDMO
                //               where (b.AMCO_Id == c.AMCO_Id && b.AMB_Id == s.AMB_Id && b.AMSE_Id == d.AMSE_Id && a.MI_Id == c.MI_Id && a.MI_Id == dto.MI_Id
                //               && a.AMCST_Id == dto.AMCST_Id && b.AMCST_Id == dto.AMCST_Id)
                //               select new ClgStudentDashboardDTO
                //               {
                //                   AMCO_Id = c.AMCO_Id,
                //                   AMCO_CourseName = c.AMCO_CourseName,
                //                   AMSE_Id = d.AMSE_Id,
                //                   AMB_Id = s.AMB_Id,
                //                   AMB_BranchName = s.AMB_BranchName,
                //                   AMSE_SEMName = d.AMSE_SEMName
                //               }).Distinct().ToList();


                var asmay_year = (from a in _ClgPortalContext.AcademicYear                                  where a.MI_Id == dto.MI_Id && a.ASMAY_ActiveFlag == 1 && a.ASMAY_To_Date >= DateTime.Now                                  select new ClgStudentDashboardDTO                                  {                                      ASMAY_Id = a.ASMAY_Id,                                      ASMAY_Year = a.ASMAY_Year                                  }).OrderBy(s => s.ASMAY_Year).ToArray();                if (asmay_year.Length > 0)                {                    for (long i = 0; i < asmay_year.Length; i++)                    {                        var exist = (from z in _ClgPortalContext.Adm_College_Yearly_StudentDMO                                     from y in _ClgPortalContext.Adm_Master_College_StudentDMO                                     where (z.AMCST_Id == y.AMCST_Id && y.AMCST_SOL == "S" && y.AMCST_ActiveFlag == true && y.ASMAY_Id == asmay_year[i].ASMAY_Id && y.AMCST_Id == dto.AMCST_Id)                                     select y).ToArray();                        if (exist.Length > 0)                        {                            dto.ASMAY_Id = asmay_year[i].ASMAY_Id;                        }                    }                }


                var clssec1 = (from a in _ClgPortalContext.Adm_Master_College_StudentDMO
                               from b in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                               where (a.AMCST_Id == b.AMCST_Id && a.AMCST_ActiveFlag == true && a.MI_Id == dto.MI_Id && a.AMCST_Id == dto.AMCST_Id && b.ASMAY_Id == dto.ASMAY_Id)
                               select new ClgStudentDashboardDTO
                               {
                                   AMCST_Id = a.AMCST_Id,
                                   AMCO_Id = b.AMCO_Id,
                                   AMB_Id = b.AMB_Id,
                                   AMSE_Id = b.AMSE_Id
                               }).Distinct().ToList();


                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {

                    long? Course_Id = clssec1.FirstOrDefault().AMCO_Id;
                    long? Branch_Id = clssec1.FirstOrDefault().AMB_Id;
                    long? Semester_Id = clssec1.FirstOrDefault().AMSE_Id;
                    //   using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                    //   {
                    //       cmd.CommandText = "Portal_NoticeBoardYearWise_seenId_clg";
                    //       cmd.CommandType = CommandType.StoredProcedure;

                    //       cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    //SqlDbType.VarChar)
                    //       {
                    //           Value = dto.MI_Id
                    //       });
                    //       cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                    //       SqlDbType.VarChar)
                    //       {
                    //           //Value = Class_Id

                    //           Value = Branch_Id
                    //       });
                    //       cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                    //     SqlDbType.VarChar)
                    //       {
                    //           //Value = Class_Id

                    //           Value = Semester_Id
                    //       });
                    //       cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                    //     SqlDbType.VarChar)
                    //       {
                    //           //Value = Class_Id

                    //           Value = Course_Id
                    //       });
                    //       cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    //   SqlDbType.VarChar)
                    //       {
                    //           Value = dto.ASMAY_Id
                    //       });

                    //       cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    //  SqlDbType.VarChar)
                    //       {
                    //           Value = dto.AMCST_Id
                    //       });

                    //       cmd.Parameters.Add(new SqlParameter("@Flag",
                    //   SqlDbType.VarChar)
                    //       {
                    //           Value = dto.flag
                    //       });

                    //       cmd.Parameters.Add(new SqlParameter("@Type",
                    //   SqlDbType.VarChar)
                    //       {
                    //           Value = "Student"
                    //       });
                    //       cmd.Parameters.Add(new SqlParameter("@INTB_Id",
                    // SqlDbType.VarChar)
                    //       {
                    //           Value = dto.INTB_Id
                    //       });

                    //       if (cmd.Connection.State != ConnectionState.Open)
                    //           cmd.Connection.Open();

                    //       var retObject = new List<dynamic>();
                    //       try
                    //       {
                    //           using (var dataReader = cmd.ExecuteReader())
                    //           {
                    //               while (dataReader.Read())
                    //               {
                    //                   var dataRow = new ExpandoObject() as IDictionary<string, object>;
                    //                   for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                    //                   {
                    //                       dataRow.Add(
                    //                           dataReader.GetName(iFiled),
                    //                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                    //                       );
                    //                   }

                    //                   retObject.Add((ExpandoObject)dataRow);
                    //               }
                    //           }
                    //           dto.noticelist_byid = retObject.ToArray();
                    //       }
                    //       catch (Exception ex)
                    //       {
                    //           Console.WriteLine(ex.Message);
                    //       }
                    //   }
                    using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_NoticeBoardYearWise_seenId_clg";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.VarChar)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                  SqlDbType.VarChar)
                        {
                            //Value = Class_Id

                            Value = Course_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                        SqlDbType.VarChar)
                        {
                            //Value = Class_Id

                            Value = Branch_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                      SqlDbType.VarChar)
                        {
                            //Value = Class_Id

                            Value = Semester_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                        {
                            Value = dto.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                   SqlDbType.VarChar)
                        {
                            Value = dto.AMCST_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                        {
                            Value = dto.flag
                        });

                        cmd.Parameters.Add(new SqlParameter("@Type",
                    SqlDbType.VarChar)
                        {
                            Value = "Student"
                        });
                        cmd.Parameters.Add(new SqlParameter("@INTB_Id",
                  SqlDbType.VarChar)
                        {
                            Value = dto.INTB_Id
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
                            dto.noticelist_byid = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public ClgStudentDashboardDTO ViewMonthWiseAttendance(ClgStudentDashboardDTO data)
        {
            try
            {
                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_View_StudentWise_Attendance_MonthWise_Clg";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMCST_Id });

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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.viewstudentattendanceMonthdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}

