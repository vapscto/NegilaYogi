using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;


namespace CollegeServiceHub.Impl
{
    public class ClgAttendanceEntryImpl : Interface.ClgAttendanceEntryInterface
    {
        public ClgAdmissionContext _ClgAdmissionContext;
        ILogger<ClgAttendanceEntryImpl> _logatt;
        public ClgAttendanceEntryImpl(ClgAdmissionContext ClgAdmissionContext, ILogger<ClgAttendanceEntryImpl> log)
        {
            _ClgAdmissionContext = ClgAdmissionContext;
            _logatt = log;
        }

        public ClgAttendanceEntryDTO getalldetails(ClgAttendanceEntryDTO data)
        {
            try
            {
                data.getYear = _ClgAdmissionContext.AcademicYear.Where(a => a.MI_Id.Equals(data.MI_Id) && a.ASMAY_Id == data.ASMAY_Id).ToArray();
                var attendance_entrytype = _ClgAdmissionContext.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                if (attendance_entrytype.Count == 0)
                {
                    data.message = "Please Map The Attendance Entry Type  i.e., Absent / Present Type In Admission Standard";
                    return data;
                }
                else
                {
                    var check_rolename = (from a in _ClgAdmissionContext.MasterRoleType
                                          where (a.IVRMRT_Id == data.roleId)
                                          select new ClgAttendanceEntryDTO
                                          {
                                              rolename = a.IVRMRT_Role,
                                          }).ToList();

                    var empcode_check = (from a in _ClgAdmissionContext.Staff_User_Login
                                         where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                                         select new ClgAttendanceEntryDTO
                                         {
                                             Emp_Code = a.Emp_Code,
                                         }).ToList();

                    if (empcode_check.Count > 0)
                    {
                        data.getCourse = (from a in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                          from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                          from c in _ClgAdmissionContext.AcademicYear
                                          from d in _ClgAdmissionContext.MasterCourseDMO
                                          where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMCO_Id == d.AMCO_Id && a.MI_Id == data.MI_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.ASMAY_Id == data.ASMAY_Id && d.AMCO_ActiveFlag == true && b.ACALD_ActiveFlag == true)
                                          select new ClgAttendanceEntryDTO
                                          {
                                              AMCO_Id = d.AMCO_Id,
                                              AMCO_CourseName = d.AMCO_CourseName
                                          }).Distinct().ToArray();
                    }
                    else
                    {
                        data.message = "For This Staff There Is No Privileges To Enter Attendance.. Please Contact Administrator";
                    }
                    data.getPeriod = _ClgAdmissionContext.TT_Master_PeriodDMO.Where(a => a.MI_Id.Equals(data.MI_Id)).ToArray();
                }

            }
            catch (Exception ex)
            {
                _logatt.LogInformation("Attendance Entry getalldetails :" + ex.Message);
            }
            return data;
        }

        //public ClgAttendanceEntryDTO getalldetails(ClgAttendanceEntryDTO data)
        //{
        //    try
        //    {
        //        data.getYear = _ClgAdmissionContext.AcademicYear.Where(a => a.MI_Id.Equals(data.MI_Id)).ToArray();

        //        data.getSeatsdetails = (from a in _ClgAdmissionContext.Clg_Adm_College_Seat_DistributionDMO
        //                                from b in _ClgAdmissionContext.ClgMasterBranchDMO
        //                                from c in _ClgAdmissionContext.MasterCourseDMO
        //                                from d in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
        //                                from e in _ClgAdmissionContext.Clg_Adm_College_QuotaDMO
        //                                from f in _ClgAdmissionContext.Clg_Adm_College_Quota_CategoryDMO

        //                                where (a.AMCO_Id == c.AMCO_Id && a.AMB_Id == b.AMB_Id && a.AMSE_Id == d.AMSE_Id && a.ACQ_Id == e.ACQ_Id && a.ACQC_Id == f.ACQC_Id && a.MI_Id == b.MI_Id && c.MI_Id == d.MI_Id && e.MI_Id == f.MI_Id && a.MI_Id == data.MI_Id)
        //                                select new ClgAttendanceEntryDTO
        //                                {
        //                                    ACSCD_Id = a.ACSCD_Id,
        //                                    AMCO_CourseName = c.AMCO_CourseName,
        //                                    AMB_BranchName = b.AMB_BranchName,
        //                                    AMSE_SEMName = d.AMSE_SEMName,
        //                                    ACQ_QuotaName = e.ACQ_QuotaName,
        //                                    ACQC_CategoryName = f.ACQC_CategoryName,
        //                                    ACSCD_SeatPer = a.ACSCD_SeatPer,
        //                                    ACSCD_SeatNos = a.ACSCD_SeatNos
        //                                }).Distinct().ToArray();

        //        

        //    }
        //    catch (Exception ex)
        //    {
        //        _logatt.LogInformation("Attendance Entry getalldetails :" + ex.Message);
        //    }
        //    return data;
        //}

        //public ClgAttendanceEntryDTO getCoursedata(ClgAttendanceEntryDTO data)
        //{
        //    try
        //    {
        //        data.getCourse = (from a in _ClgAdmissionContext.MasterCourseDMO
        //                          from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
        //                          from c in _ClgAdmissionContext.AcademicYear
        //                          from d in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
        //                          where (a.AMCO_Id == b.AMCO_Id && b.ASMAY_Id == c.ASMAY_Id && d.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && d.ACALU_Id == data.userId)
        //                          select new ClgAttendanceEntryDTO
        //                          {
        //                              AMCO_Id = a.AMCO_Id,
        //                              AMCO_CourseName = a.AMCO_CourseName,
        //                              AMCO_CourseCode = a.AMCO_CourseCode
        //                          }).Distinct().ToArray();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logatt.LogInformation("Attendance Entry getCoursedata :" + ex.Message);
        //    }
        //    return data;
        //}

        public ClgAttendanceEntryDTO getBranchdata(ClgAttendanceEntryDTO data)
        {
            try
            {
                var empcode_check = (from a in _ClgAdmissionContext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                                     select new ClgAttendanceEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                data.getBranch = (from a in _ClgAdmissionContext.ClgMasterBranchDMO
                                  from d in _ClgAdmissionContext.AcademicYear
                                  from e in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                  from f in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                  where (a.AMB_Id == e.AMB_Id && f.ASMAY_Id == d.ASMAY_Id && e.ACALU_Id == f.ACALU_Id && d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && e.AMCO_Id == data.AMCO_Id && f.HRME_Id == empcode_check.FirstOrDefault().Emp_Code)
                                  select new ClgAttendanceEntryDTO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_BranchCode = a.AMB_BranchCode,
                                      AMB_BranchType = a.AMB_BranchType,
                                      AMB_AidedUnAided = a.AMB_AidedUnAided
                                  }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logatt.LogInformation("Attendance Entry getBranchdata :" + ex.Message);
            }
            return data;
        }
        public ClgAttendanceEntryDTO getSemesterdata(ClgAttendanceEntryDTO data)
        {
            try
            {
                var empcode_check = (from a in _ClgAdmissionContext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                                     select new ClgAttendanceEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                data.getSemester = (from a in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                    from c in _ClgAdmissionContext.AcademicYear
                                    from d in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                    from e in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                    where (d.AMSE_Id == a.AMSE_Id && e.ACALU_Id == d.ACALU_Id && a.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.HRME_Id == empcode_check.FirstOrDefault().Emp_Code)
                                    select new ClgAttendanceEntryDTO
                                    {
                                        AMSE_Id = a.AMSE_Id,
                                        AMSE_SEMName = a.AMSE_SEMName,
                                        AMSE_SEMCode = a.AMSE_SEMCode,
                                        AMSE_EvenOdd = a.AMSE_EvenOdd
                                    }).Distinct().ToArray();
                //}

            }
            catch (Exception ex)
            {
                _logatt.LogInformation("Attendance Entry getSemesterdata :" + ex.Message);
            }
            return data;
        }

        public ClgAttendanceEntryDTO getSectiondata(ClgAttendanceEntryDTO data)
        {
            try
            {
                var empcode_check = (from a in _ClgAdmissionContext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                                     select new ClgAttendanceEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                data.getSection = (from a in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                                   from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                   from f in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                   from g in _ClgAdmissionContext.AcademicYear

                                   where (a.ACMS_Id == b.ACMS_Id && b.AMB_Id == b.AMB_Id && b.AMCO_Id == b.AMCO_Id && b.AMSE_Id == b.AMSE_Id && f.ASMAY_Id == g.ASMAY_Id && b.AMB_Id == data.AMB_Id && b.AMCO_Id == data.AMCO_Id && b.ACALU_Id == f.ACALU_Id && b.AMSE_Id == data.AMSE_Id && a.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && f.HRME_Id == empcode_check.FirstOrDefault().Emp_Code)
                                   select new ClgAttendanceEntryDTO
                                   {
                                       ACMS_Id = a.ACMS_Id,
                                       ACMS_SectionName = a.ACMS_SectionName,
                                       ACMS_SectionCode = a.ACMS_SectionCode,
                                       ACMS_Order = a.ACMS_Order
                                   }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logatt.LogInformation("Attendance Entry getSectiondata :" + ex.Message);
            }
            return data;
        }

        public ClgAttendanceEntryDTO getSubjdata(ClgAttendanceEntryDTO data)
        {
            try
            {
                var empcode_check = (from a in _ClgAdmissionContext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                                     select new ClgAttendanceEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                data.getSubject = (from e in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                                   from b in _ClgAdmissionContext.ClgMasterBranchDMO
                                   from c in _ClgAdmissionContext.MasterCourseDMO
                                   from d in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                   from a in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                   from f in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                   from g in _ClgAdmissionContext.AcademicYear
                                   from h in _ClgAdmissionContext.Adm_College_Master_SectionDMO


                                   where (a.ISMS_Id == e.ISMS_Id && a.AMSE_Id == d.AMSE_Id && b.AMB_Id == a.AMB_Id && c.AMCO_Id == a.AMCO_Id && a.ACMS_Id == h.ACMS_Id && f.ASMAY_Id == g.ASMAY_Id && a.ACALU_Id == f.ACALU_Id && b.AMB_Id == data.AMB_Id && c.AMCO_Id == data.AMCO_Id && h.ACMS_Id == data.ACMS_Id && d.AMSE_Id == data.AMSE_Id && e.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && f.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.ACALD_ActiveFlag == true)
                                   select new ClgAttendanceEntryDTO
                                   {
                                       ISMS_Id = a.ISMS_Id,
                                       ISMS_SubjectName = e.ISMS_SubjectName,
                                       ISMS_SubjectCode = e.ISMS_SubjectCode
                                   }).Distinct().ToArray();

             

            }
            catch (Exception ex)
            {
                _logatt.LogInformation("Attendance Entry getSubjdata :" + ex.Message);
            }
            return data;
        }

        public ClgAttendanceEntryDTO getBatchdata(ClgAttendanceEntryDTO data)
        {
            try
            {
                data.getBatch = (from a in _ClgAdmissionContext.Adm_College_Attendance_BatchDMO
                                 from b in _ClgAdmissionContext.Adm_College_Atten_Batch_SubjectsDMO
                                 from c in _ClgAdmissionContext.Adm_College_Atten_Batch_Subject_StudentsDMO
                                 from d in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                                 from e in _ClgAdmissionContext.AcademicYear
                                 from f in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                                 from g in _ClgAdmissionContext.ClgMasterBranchDMO
                                 from h in _ClgAdmissionContext.MasterCourseDMO
                                 from i in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                 where (a.ACAB_Id == b.ACAB_Id && b.ACABS_Id == c.ACABS_Id && b.ISMS_Id == d.ISMS_Id && b.AMB_Id == g.AMB_Id && b.AMCO_Id == h.AMCO_Id && b.AMSE_Id == i.AMSE_Id && b.ACMS_Id == f.ACMS_Id && b.ASMAY_Id == e.ASMAY_Id && e.ASMAY_Id == data.ASMAY_Id && g.AMB_Id == data.AMB_Id && h.AMCO_Id == data.AMCO_Id && i.AMSE_Id == data.AMSE_Id && f.ACMS_Id == data.ACMS_Id && d.ISMS_Id == data.ISMS_Id && a.MI_Id == data.MI_Id)
                                 select new ClgAttendanceEntryDTO
                                 {
                                     ACAB_Id = a.ACAB_Id,
                                     ACAB_BatchName = a.ACAB_BatchName,
                                     ACAB_StudentStrength = a.ACAB_StudentStrength
                                 }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                _logatt.LogInformation("Attendance Entry getBatchdata :" + ex.Message);
            }
            return data;
        }

        public ClgAttendanceEntryDTO getStudentdata(ClgAttendanceEntryDTO data)
        {
            try
            {
                var attendance_entrytype = _ClgAdmissionContext.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();

                var check_attendance_entrytype = _ClgAdmissionContext.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();

                data.check_attendance_entrytype = check_attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type;

                List<ClgAttendanceEntryTempDTO> obj = new List<ClgAttendanceEntryTempDTO>();
                List<ClgAttendanceEntryTempDTO> studentList1 = new List<ClgAttendanceEntryTempDTO>();
                List<ClgAttendanceEntryTempDTO> result = new List<ClgAttendanceEntryTempDTO>();
                List<ClgAttendanceEntryDTO> check_period = new List<ClgAttendanceEntryDTO>();             

                DateTime fromdatecon = DateTime.Now;
                string confromdate = "";
                if (data.ACSA_AttendanceDate != null)
                {
                    try
                    {
                        fromdatecon = Convert.ToDateTime(data.ACSA_AttendanceDate.Value.Date.ToString("yyyy-MM-dd"));
                        //confromdate = fromdatecon.ToString();
                        confromdate = fromdatecon.ToString("yyyy-MM-dd");
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                string ordderby = "";
                if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 1)
                {
                    ordderby = "Order By AMCST_Sex";
                }
                else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 2)
                {
                    ordderby = "Order By AMCST_Sex desc";
                }
                else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 3)
                {
                    ordderby = "Order By ACYST_RollNo ";
                }
                else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 4)
                {
                    ordderby = "Order By AMCST_FirstName";
                }
                else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 5)
                {
                    ordderby = "Order By AMCST_FirstName  desc";
                }
                else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 6)
                {
                    ordderby = "Order By AMCST_RegistrationNo";
                }
                else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 7)
                {
                    ordderby = "Order By AMCST_AdmNo";
                }
                else
                {
                    ordderby = "Order By AMCST_FirstName";
                }

                //************** Check Period ATTEDNACE IS ENTERD OR NOT **************//
                List<ClgAttendanceEntryDTO> obj1 = new List<ClgAttendanceEntryDTO>();
                using (var command = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SELECT distinct [c].[ACSA_Id], [c].[TTMP_Id]FROM [CLG].[Adm_College_Student_Attendance] AS [a] INNER JOIN[CLG].[Adm_College_Student_Attendance_Students] AS[b] on[a].[ACSA_Id] = [b].[ACSA_Id] INNER JOIN[CLG].[Adm_College_Student_Attendance_Periodwise] AS[c] on[a].[ACSA_Id] = [c].[ACSA_Id] INNER JOIN[Adm_School_M_Academic_Year] AS[d] on[a].[ASMAY_Id] = [d].[ASMAY_Id] INNER JOIN[CLG].[Adm_Master_Course] AS[e] on[a].[AMCO_Id] = [e].[AMCO_Id] INNER JOIN[CLG].[Adm_Master_Branch] AS[f] on[a].[AMB_Id] = [f].[AMB_Id] INNER JOIN[CLG].[Adm_Master_Semester] AS[g] on[a].[AMSE_Id] = [g].[AMSE_Id] INNER JOIN[CLG].[Adm_College_Master_Section] AS[h] on[a].[ACMS_Id] = [h].[ACMS_Id] INNER JOIN[TT_Master_Period] AS[i] on[i].[TTMP_Id] = [c].[TTMP_Id]  WHERE  [a].[ASMAY_Id] = "+data.ASMAY_Id+" AND [a].[AMCO_Id] = "+ ""+data.AMCO_Id+" AND [a].[AMB_Id] = "+data.AMB_Id+" AND [a].[AMSE_Id] = "+data.AMSE_Id+" AND [a].[ACMS_Id] = "+data.ACMS_Id+" AND [c].[TTMP_Id] = "+data.TTMP_Id+" AND [a].[ISMS_Id] = "+data.ISMS_Id+" AND [a].[ACSA_AttendanceDate] = '"+ confromdate + "' ";
                    _ClgAdmissionContext.Database.OpenConnection();
                    using (var resultc = command.ExecuteReader())
                    {
                        while (resultc.Read())
                        {
                            obj1.Add(new ClgAttendanceEntryDTO
                            {
                                ACSA_Id = Convert.ToInt64(resultc["ACSA_Id"]),
                                TTMP_Id = Convert.ToInt64(resultc["TTMP_Id"]),
                            });
                        }
                        check_period = obj1.ToList();
                    }
                }

                //var check_period = (from a in _ClgAdmissionContext.Adm_College_Student_AttendanceDMO
                //                    from b in _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO
                //                    from c in _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO
                //                    from d in _ClgAdmissionContext.AcademicYear
                //                    from e in _ClgAdmissionContext.MasterCourseDMO
                //                    from f in _ClgAdmissionContext.ClgMasterBranchDMO
                //                    from g in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                //                    from h in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                //                    from i in _ClgAdmissionContext.TT_Master_PeriodDMO
                //                    where (a.ACSA_Id == b.ACSA_Id && a.ACSA_Id == c.ACSA_Id && a.ASMAY_Id == d.ASMAY_Id
                //                         && a.AMCO_Id == e.AMCO_Id && a.AMB_Id == f.AMB_Id && a.AMSE_Id == g.AMSE_Id && a.ACMS_Id == h.ACMS_Id
                //                         && i.TTMP_Id == c.TTMP_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id
                //                         && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACMS_Id == data.ACMS_Id
                //                         && c.TTMP_Id == data.TTMP_Id && a.ISMS_Id == data.ISMS_Id && a.ACSA_AttendanceDate == data.ACSA_AttendanceDate)
                //                    select new ClgAttendanceEntryDTO
                //                    {
                //                        ACSA_Id = c.ACSA_Id,
                //                        TTMP_Id = c.TTMP_Id
                //                    }).Distinct().ToList();

                if (check_period.Count() > 0)
                {
                    var check_period_subject = (from a in _ClgAdmissionContext.Adm_College_Student_AttendanceDMO
                                                from b in _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO
                                                from c in _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO
                                                from d in _ClgAdmissionContext.AcademicYear
                                                from e in _ClgAdmissionContext.MasterCourseDMO
                                                from f in _ClgAdmissionContext.ClgMasterBranchDMO
                                                from g in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                                from h in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                                                from i in _ClgAdmissionContext.TT_Master_PeriodDMO
                                                from j in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                                                where (a.ACSA_Id == b.ACSA_Id && a.ACSA_Id == c.ACSA_Id && a.ASMAY_Id == d.ASMAY_Id
                                                     && a.AMCO_Id == e.AMCO_Id && a.AMB_Id == f.AMB_Id && a.AMSE_Id == g.AMSE_Id && a.ACMS_Id == h.ACMS_Id
                                                     && i.TTMP_Id == c.TTMP_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id
                                                     && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACMS_Id == data.ACMS_Id
                                                     && c.TTMP_Id == data.TTMP_Id && a.ISMS_Id == data.ISMS_Id && a.ISMS_Id == j.ISMS_Id
                                                     && a.ISMS_Id == data.ISMS_Id && a.ACSA_AttendanceDate == data.ACSA_AttendanceDate
                                                     && a.ACSA_Regular_Extra == data.ACSA_Regular_Extra)
                                                select new ClgAttendanceEntryDTO
                                                {
                                                    ACSA_Id = c.ACSA_Id,
                                                    TTMP_Id = c.TTMP_Id,
                                                    ISMS_Id = a.ISMS_Id
                                                }).Distinct().ToList();

                    if (check_period_subject.Count() > 0)
                    {
                        using (var command = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                        {
                            command.CommandText = "Select b.ACSAS_ClassAttended as ASA_Class_Attended, b.ACSAS_Id as ACSAS_Id, a.ACSA_Id as ACSA_Id, " +
                                "(d.AMCST_Id)AMCST_Id,(CASE WHEN AMCST_FirstName is null or AMCST_FirstName = ''  then '' else AMCST_FirstName end + " +
                               "CASE WHEN AMCST_MiddleName is null or AMCST_MiddleName = '' or AMCST_MiddleName = '0' then ''  ELSE ' ' + AMCST_MiddleName END +" +
                               "CASE WHEN AMCST_LastName is null or AMCST_LastName = '' or AMCST_LastName = '0' then ''  ELSE ' ' + AMCST_LastName END) AS " +
                               "AMCST_FirstName,(dd.AMCST_AdmNo)AMCST_AdmNo, (d.ACYST_RollNo)ACYST_RollNo, AMCST_Sex , (AMCST_RegistrationNo) as AMCST_RegistrationNo " +
                               "from clg.Adm_College_Student_Attendance a INNER JOIN clg.Adm_College_Student_Attendance_Students b on a.ACSA_Id = b.ACSA_Id" +
                               " inner join clg.Adm_College_Student_Attendance_Periodwise c on c.ACSA_Id = a.ACSA_Id Inner Join  clg.Adm_College_Yearly_Student d on " +
                               "d.AMCST_Id = b.AMCST_Id inner join clg.Adm_Master_College_Student dd on dd.AMCST_Id = d.AMCST_Id inner join " +
                               "clg.Adm_Master_Course e on e.AMCO_Id = a.AMCO_Id and e.AMCO_Id = d.AMCO_Id inner join clg.Adm_Master_Branch f " +
                               "on f.AMB_Id = a.AMB_Id and f.AMB_Id = d.AMB_Id inner join clg.Adm_Master_Semester g on g.AMSE_Id = a.AMSE_Id " +
                               "and g.AMSE_Id = d.AMSE_Id inner join clg.Adm_College_Master_Section h on h.ACMS_Id = a.ACMS_Id and h.ACMS_Id = d.ACMS_Id " +
                               "inner join IVRM_Master_Subjects i on i.ISMS_Id = a.ISMS_Id inner join TT_Master_Period j on j.TTMP_Id = c.TTMP_Id where a.AMCO_Id = " + data.AMCO_Id + " and a.AMB_Id = " + data.AMB_Id + " and a.AMSE_Id = " + data.AMSE_Id + " and a.ACMS_Id = " + data.ACMS_Id + " " +
                               "and ACSA_AttendanceDate ='" + confromdate + "' and a.MI_Id = " + data.MI_Id + " and a.ISMS_Id = " + data.ISMS_Id + " and c.TTMP_Id = " + data.TTMP_Id + " and d.AMCO_Id = " + data.AMCO_Id + " and d.AMB_Id = " + data.AMB_Id + " and d.AMSE_Id = " + data.AMSE_Id + " and d.ACMS_Id = " + data.ACMS_Id + " and a.ASMAY_Id = " + data.ASMAY_Id + " and d.ASMAY_Id = " + data.ASMAY_Id + "  and dd.AMCST_SOL = 'S' and dd.AMCST_ActiveFlag = 1  and a.ACSA_Regular_Extra = '" + data.ACSA_Regular_Extra + "' " + ordderby + " ";
                            _ClgAdmissionContext.Database.OpenConnection();
                            using (var result1 = command.ExecuteReader())
                            {
                                while (result1.Read())
                                {
                                    obj.Add(new ClgAttendanceEntryTempDTO
                                    {
                                        AMCST_Id = Convert.ToInt64(result1["AMCST_Id"]),
                                        AMCST_FirstName = result1["AMCST_FirstName"].ToString(),
                                        AMCST_AdmNo = result1["AMCST_AdmNo"].ToString(),
                                        ACYST_RollNo = Convert.ToInt64(result1["ACYST_RollNo"]),
                                        AMCST_RegistrationNo = result1["AMCST_RegistrationNo"].ToString(),
                                        pdays = Convert.ToInt64(result1["ASA_Class_Attended"]),
                                        ACSAS_Id = Convert.ToInt64(result1["ACSAS_Id"]),
                                        ACSA_Id = Convert.ToInt64(result1["ACSA_Id"]),
                                    });
                                }
                                studentList1 = obj.ToList();
                            }
                        }

                        for (int i = 0; i < result.Count; i++)
                        {
                            studentList1.Add(result[i]);
                        }

                        data.getStudentdetails = studentList1.ToArray();
                    }
                    else
                    {
                        data.message = "Already Attendance Is Enter For This Period";
                    }
                }
                else
                {
                    //&& b.ACYST_ActiveFlag == 1
                    data.getStudentdetails = (from a in _ClgAdmissionContext.Adm_Master_College_StudentDMO
                                              from b in _ClgAdmissionContext.Adm_College_Yearly_StudentDMO
                                              from c in _ClgAdmissionContext.AcademicYear
                                              from d in _ClgAdmissionContext.ClgMasterBranchDMO
                                              from e in _ClgAdmissionContext.MasterCourseDMO
                                              from f in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                              from g in _ClgAdmissionContext.Adm_College_Master_SectionDMO

                                              where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMB_Id == d.AMB_Id && b.AMCO_Id == e.AMCO_Id && b.AMSE_Id == f.AMSE_Id && b.ACMS_Id == g.ACMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMB_Id == data.AMB_Id && b.AMCO_Id == data.AMCO_Id && b.AMSE_Id == data.AMSE_Id && b.ACMS_Id == data.ACMS_Id && a.AMCST_SOL == "S" && a.AMCST_ActiveFlag == true )
                                              select new ClgAttendanceEntryDTO
                                              {
                                                  AMCST_Id = a.AMCST_Id,
                                                  AMCST_FirstName = ((a.AMCST_FirstName == null || a.AMCST_FirstName == "" ? "" : " " + a.AMCST_FirstName) + (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "" || a.AMCST_MiddleName == "0" ? "" : " " + a.AMCST_MiddleName) + (a.AMCST_LastName == null || a.AMCST_LastName == "" || a.AMCST_LastName == "0" ? "" : " " + a.AMCST_LastName)).Trim(),
                                                  AMCST_AdmNo = a.AMCST_AdmNo,
                                                  AMCST_RegistrationNo = a.AMCST_RegistrationNo,
                                                  ACYST_RollNo = b.ACYST_RollNo
                                              }).Distinct().ToArray();
                }


            }
            catch (Exception ex)
            {
                _logatt.LogInformation("Attendance Entry getStudentdata :" + ex.Message);
            }
            return data;
        }

        public ClgAttendanceEntryDTO saveatt(ClgAttendanceEntryDTO data)
        {
            try
            {

                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                var MAACAdd = sMacAddress;


                var empcode_check = (from a in _ClgAdmissionContext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                                     select new ClgAttendanceEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();



                var emp_att_login_check = (from a in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                           from c in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                           where (a.ACALU_Id == c.ACALU_Id && a.MI_Id == data.MI_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && c.AMCO_Id == data.AMCO_Id && c.AMB_Id == data.AMB_Id && a.ASMAY_Id == data.ASMAY_Id && c.ACMS_Id == data.ACMS_Id
                                           && c.ISMS_Id == data.ISMS_Id && c.AMSE_Id == data.AMSE_Id)
                                           select new ClgAttendanceEntryDTO
                                           {
                                               ACALU_Id = a.ACALU_Id,
                                           }).ToList();


                if (emp_att_login_check.Count == 0)
                {
                    data.message = "For This Staff There Is No Privileges To Enter Attendance.. Please Map The Staff In Attendance Privileges";
                    return data;
                }


                var check_attendance_entrytype = _ClgAdmissionContext.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();

                if (check_attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type == "A")
                {
                    var check_duplicate = (from a in _ClgAdmissionContext.Adm_College_Student_AttendanceDMO
                                           from b in _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO
                                           from c in _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO
                                           from d in _ClgAdmissionContext.AcademicYear
                                           from e in _ClgAdmissionContext.MasterCourseDMO
                                           from f in _ClgAdmissionContext.ClgMasterBranchDMO
                                           from g in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                           from h in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                                           from i in _ClgAdmissionContext.TT_Master_PeriodDMO
                                           from j in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                                           where (a.ACSA_Id == b.ACSA_Id && a.ACSA_Id == c.ACSA_Id && a.ASMAY_Id == d.ASMAY_Id
                                           && a.AMCO_Id == e.AMCO_Id && a.AMB_Id == f.AMB_Id && a.AMSE_Id == g.AMSE_Id && a.ACMS_Id == h.ACMS_Id
                                           && i.TTMP_Id == c.TTMP_Id && a.ISMS_Id == j.ISMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id
                                           && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACMS_Id == data.ACMS_Id && c.TTMP_Id == data.TTMP_Id && a.ISMS_Id == data.ISMS_Id && a.ACSA_AttendanceDate == data.ACSA_AttendanceDate && a.ACSA_Regular_Extra == data.ACSA_Regular_Extra)
                                           select new ClgAttendanceEntryDTO
                                           {
                                               ACSA_Id = a.ACSA_Id,
                                           }).ToList();

                    if (check_duplicate.Count > 0)
                    {
                        for (int i = 0; i < data.ClgAttendanceEntryTempDTO.Count(); i++)
                        {
                            if (i == 0)
                            {
                                var result = _ClgAdmissionContext.Adm_College_Student_AttendanceDMO.Single(d => d.ACSA_Id == data.ClgAttendanceEntryTempDTO[i].ACSA_Id);
                                result.ACSA_Att_EntryType = "Absent";
                                result.ACALU_Id = emp_att_login_check.FirstOrDefault().ACALU_Id;
                                result.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                                result.UpdatedDate = DateTime.Now;
                                _ClgAdmissionContext.Adm_College_Student_AttendanceDMO.Update(result);

                                var resulttt = _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO.Single(d => d.ACSA_Id == data.ClgAttendanceEntryTempDTO[i].ACSA_Id && d.TTMP_Id == data.TTMP_Id);
                                resulttt.UpdatedDate = DateTime.Now;
                                _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO.Update(resulttt);

                            }
                            if (data.ClgAttendanceEntryTempDTO[i].ACSAS_Id != null)
                            {
                                var result3 = _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO.Single(a => a.ACSA_Id == data.ClgAttendanceEntryTempDTO[i].ACSA_Id && a.AMCST_Id == data.ClgAttendanceEntryTempDTO[i].AMCST_Id);

                                //Adm_College_Student_Attendance_StudentsDMO std = Mapper.Map<Adm_College_Student_Attendance_StudentsDMO>(data.ClgAttendanceEntryTempDTO[i]);


                                if (data.ClgAttendanceEntryTempDTO[i].Selected == false)
                                {
                                    result3.ACSAS_AttendanceFlag = "Present";
                                    result3.ACSAS_ClassAttended = 1;

                                }
                                else if (data.ClgAttendanceEntryTempDTO[i].Selected == true)
                                {
                                    result3.ACSAS_AttendanceFlag = "Absent";
                                    result3.ACSAS_ClassAttended = 0;
                                }

                                result3.UpdatedDate = DateTime.Now;
                                _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO.Update(result3);
                            }
                            else
                            {
                                Adm_College_Student_Attendance_StudentsDMO stdperiod = new Adm_College_Student_Attendance_StudentsDMO();

                                stdperiod.ACSA_Id = check_duplicate.FirstOrDefault().ACSA_Id;
                                stdperiod.AMCST_Id = Convert.ToInt64(data.ClgAttendanceEntryTempDTO[i].AMCST_Id);

                                if (data.ClgAttendanceEntryTempDTO[i].Selected == true)
                                {
                                    stdperiod.ACSAS_AttendanceFlag = "Absent";
                                    stdperiod.ACSAS_ClassAttended = 0;
                                }
                                else if (data.ClgAttendanceEntryTempDTO[i].Selected == false)
                                {
                                    stdperiod.ACSAS_AttendanceFlag = "Present";
                                    stdperiod.ACSAS_ClassAttended = 1;
                                }

                                stdperiod.CreatedDate = DateTime.Now;
                                stdperiod.UpdatedDate = DateTime.Now;
                                _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO.Add(stdperiod);
                            }
                        }
                        int n = _ClgAdmissionContext.SaveChanges();
                        if (n > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else
                    {
                        Adm_College_Student_AttendanceDMO enq = new Adm_College_Student_AttendanceDMO();
                        enq.MI_Id = data.MI_Id;
                        enq.ASMAY_Id = data.ASMAY_Id;
                        enq.ACSA_Att_EntryType = "Absent";
                        enq.AMCO_Id = data.AMCO_Id;
                        enq.AMB_Id = data.AMB_Id;
                        enq.AMSE_Id = data.AMSE_Id;
                        enq.ACMS_Id = data.ACMS_Id;
                        enq.ISMS_Id = data.ISMS_Id;
                        enq.ACSA_Entry = DateTime.Now;
                        enq.ACSA_AttendanceDate =Convert.ToDateTime(data.ACSA_AttendanceDate);
                        enq.ACSA_ClassHeld = 1;
                        enq.ACSA_NetworkIP = data.networkip;
                        enq.ACSA_MAACAdd = MAACAdd;
                        enq.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                        enq.ACALU_Id = emp_att_login_check.FirstOrDefault().ACALU_Id;
                        enq.ACSA_Regular_Extra = data.ACSA_Regular_Extra;
                        enq.ACSA_ActiveFlag = true;
                        enq.CreatedDate = DateTime.Now;
                        enq.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Adm_College_Student_AttendanceDMO.Add(enq);

                        if (data.ClgAttendanceEntryTempDTO != null && data.ClgAttendanceEntryTempDTO.Count() > 0)
                        {

                            Adm_College_Student_Attendance_PeriodwiseDMO attperiodwise = new Adm_College_Student_Attendance_PeriodwiseDMO();
                            attperiodwise.ACSA_Id = enq.ACSA_Id;
                            attperiodwise.TTMP_Id = data.TTMP_Id;
                            attperiodwise.CreatedDate = DateTime.Now;
                            attperiodwise.UpdatedDate = DateTime.Now;
                            try
                            {
                                _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO.Add(attperiodwise);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            for (int i = 0; i < data.ClgAttendanceEntryTempDTO.Count(); i++)
                            {
                                Adm_College_Student_Attendance_StudentsDMO std = new Adm_College_Student_Attendance_StudentsDMO();
                                std.ACSA_Id = enq.ACSA_Id;
                                if (data.ClgAttendanceEntryTempDTO[i].Selected == false)
                                {
                                    std.ACSAS_AttendanceFlag = "Present";
                                    std.ACSAS_ClassAttended = 1;
                                }
                                else if (data.ClgAttendanceEntryTempDTO[i].Selected == true)
                                {
                                    std.ACSAS_AttendanceFlag = "Absent";
                                    std.ACSAS_ClassAttended = 0;
                                }
                                std.AMCST_Id = Convert.ToInt64(data.ClgAttendanceEntryTempDTO[i].AMCST_Id);
                                std.CreatedDate = DateTime.Now;
                                std.UpdatedDate = DateTime.Now;
                                _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO.Add(std);
                            }
                            var contactExists = _ClgAdmissionContext.SaveChanges();
                            if (contactExists >= 1)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                    }
                }
                else if (check_attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type == "P")
                {
                    var check_duplicate = (from a in _ClgAdmissionContext.Adm_College_Student_AttendanceDMO
                                           from b in _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO
                                           from c in _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO
                                           from d in _ClgAdmissionContext.AcademicYear
                                           from e in _ClgAdmissionContext.MasterCourseDMO
                                           from f in _ClgAdmissionContext.ClgMasterBranchDMO
                                           from g in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                           from h in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                                           from i in _ClgAdmissionContext.TT_Master_PeriodDMO
                                           from j in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                                           where (a.ACSA_Id == b.ACSA_Id && a.ACSA_Id == c.ACSA_Id && a.ASMAY_Id == d.ASMAY_Id
                                           && a.AMCO_Id == e.AMCO_Id && a.AMB_Id == f.AMB_Id && a.AMSE_Id == g.AMSE_Id && a.ACMS_Id == h.ACMS_Id
                                           && i.TTMP_Id == c.TTMP_Id && a.ISMS_Id == j.ISMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id
                                           && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACMS_Id == data.ACMS_Id && c.TTMP_Id == data.TTMP_Id && a.ISMS_Id == data.ISMS_Id && a.ACSA_AttendanceDate == data.ACSA_AttendanceDate && a.ACSA_Regular_Extra == data.ACSA_Regular_Extra)
                                           select new ClgAttendanceEntryDTO
                                           {
                                               ACSA_Id = a.ACSA_Id,
                                           }).ToList();

                    if (check_duplicate.Count > 0)
                    {
                        for (int i = 0; i < data.ClgAttendanceEntryTempDTO.Count(); i++)
                        {
                            if (i == 0)
                            {
                                var result = _ClgAdmissionContext.Adm_College_Student_AttendanceDMO.Single(d => d.ACSA_Id == data.ClgAttendanceEntryTempDTO[i].ACSA_Id);
                                result.ACSA_Att_EntryType = "Present";
                                result.ACALU_Id = emp_att_login_check.FirstOrDefault().ACALU_Id;
                                result.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                                result.UpdatedDate = DateTime.Now;
                                _ClgAdmissionContext.Adm_College_Student_AttendanceDMO.Update(result);

                                var resulttt = _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO.Single(d => d.ACSA_Id == data.ClgAttendanceEntryTempDTO[i].ACSA_Id && d.TTMP_Id == data.TTMP_Id);
                                resulttt.UpdatedDate = DateTime.Now;
                                _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO.Update(resulttt);

                            }
                            if (data.ClgAttendanceEntryTempDTO[i].ACSAS_Id != null)
                            {
                                var result3 = _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO.Single(a => a.ACSA_Id == data.ClgAttendanceEntryTempDTO[i].ACSA_Id && a.AMCST_Id == data.ClgAttendanceEntryTempDTO[i].AMCST_Id);

                                //Adm_College_Student_Attendance_StudentsDMO std = Mapper.Map<Adm_College_Student_Attendance_StudentsDMO>(data.ClgAttendanceEntryTempDTO[i]);


                                if (data.ClgAttendanceEntryTempDTO[i].Selected == true)
                                {
                                    result3.ACSAS_AttendanceFlag = "Present";
                                    result3.ACSAS_ClassAttended = 1;

                                }
                                else if (data.ClgAttendanceEntryTempDTO[i].Selected == false)
                                {
                                    result3.ACSAS_AttendanceFlag = "Absent";
                                    result3.ACSAS_ClassAttended = 0;
                                }

                                result3.UpdatedDate = DateTime.Now;
                                _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO.Update(result3);
                            }
                            else
                            {
                                Adm_College_Student_Attendance_StudentsDMO stdperiod = new Adm_College_Student_Attendance_StudentsDMO();

                                stdperiod.ACSA_Id = check_duplicate.FirstOrDefault().ACSA_Id;
                                stdperiod.AMCST_Id = Convert.ToInt64(data.ClgAttendanceEntryTempDTO[i].AMCST_Id);

                                if (data.ClgAttendanceEntryTempDTO[i].Selected == false)
                                {
                                    stdperiod.ACSAS_AttendanceFlag = "Absent";
                                    stdperiod.ACSAS_ClassAttended = 0;
                                }
                                else if (data.ClgAttendanceEntryTempDTO[i].Selected == true)
                                {
                                    stdperiod.ACSAS_AttendanceFlag = "Present";
                                    stdperiod.ACSAS_ClassAttended = 1;
                                }

                                stdperiod.CreatedDate = DateTime.Now;
                                stdperiod.UpdatedDate = DateTime.Now;
                                _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO.Add(stdperiod);
                            }
                        }
                        int n = _ClgAdmissionContext.SaveChanges();
                        if (n > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else
                    {
                        Adm_College_Student_AttendanceDMO enq = new Adm_College_Student_AttendanceDMO();
                        enq.MI_Id = data.MI_Id;
                        enq.ASMAY_Id = data.ASMAY_Id;
                        enq.ACSA_Att_EntryType = "Present";
                        enq.AMCO_Id = data.AMCO_Id;
                        enq.AMB_Id = data.AMB_Id;
                        enq.AMSE_Id = data.AMSE_Id;
                        enq.ACMS_Id = data.ACMS_Id;
                        enq.ISMS_Id = data.ISMS_Id;
                        enq.ACSA_Entry = DateTime.Now;
                        enq.ACSA_AttendanceDate =Convert.ToDateTime(data.ACSA_AttendanceDate);
                        enq.ACSA_ClassHeld = 1;
                        enq.ACSA_NetworkIP = data.networkip;
                        enq.ACSA_MAACAdd = MAACAdd;
                        enq.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                        enq.ACALU_Id = emp_att_login_check.FirstOrDefault().ACALU_Id;
                        enq.ACSA_Regular_Extra = data.ACSA_Regular_Extra;
                        enq.ACSA_ActiveFlag = true;
                        enq.CreatedDate = DateTime.Now;
                        enq.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Adm_College_Student_AttendanceDMO.Add(enq);

                        if (data.ClgAttendanceEntryTempDTO != null && data.ClgAttendanceEntryTempDTO.Count() > 0)
                        {

                            Adm_College_Student_Attendance_PeriodwiseDMO attperiodwise = new Adm_College_Student_Attendance_PeriodwiseDMO();
                            attperiodwise.ACSA_Id = enq.ACSA_Id;
                            attperiodwise.TTMP_Id = data.TTMP_Id;
                            attperiodwise.CreatedDate = DateTime.Now;
                            attperiodwise.UpdatedDate = DateTime.Now;
                            try
                            {
                                _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO.Add(attperiodwise);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            for (int i = 0; i < data.ClgAttendanceEntryTempDTO.Count(); i++)
                            {
                                Adm_College_Student_Attendance_StudentsDMO std = new Adm_College_Student_Attendance_StudentsDMO();
                                std.ACSA_Id = enq.ACSA_Id;
                                if (data.ClgAttendanceEntryTempDTO[i].Selected == true)
                                {
                                    std.ACSAS_AttendanceFlag = "Present";
                                    std.ACSAS_ClassAttended = 1;
                                }
                                else if (data.ClgAttendanceEntryTempDTO[i].Selected == false)
                                {
                                    std.ACSAS_AttendanceFlag = "Absent";
                                    std.ACSAS_ClassAttended = 0;
                                }
                                std.AMCST_Id = Convert.ToInt64(data.ClgAttendanceEntryTempDTO[i].AMCST_Id);
                                std.CreatedDate = DateTime.Now;
                                std.UpdatedDate = DateTime.Now;
                                _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO.Add(std);
                            }
                            var contactExists = _ClgAdmissionContext.SaveChanges();
                            if (contactExists >= 1)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                    }
                }
                else
                {
                    data.message = "Map The Attendance Entry Type";
                }

            }
            catch (Exception ex)
            {
                _logatt.LogInformation("Attendance Entry saveatt :" + ex.Message);
            }
            return data;
        }
    }
}
