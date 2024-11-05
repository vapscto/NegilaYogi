using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class CollegeAttendanceEntryNewImpl : Interface.CollegeAttendanceEntryNewInterface
    {
        public ClgAdmissionContext _ClgAdmissionContext;
        public DomainModelMsSqlServerContext _db;
        ILogger<ClgAttendanceEntryImpl> _logatt;
        private readonly UserManager<ApplicationUser> _userManager;
        public CollegeAttendanceEntryNewImpl(ClgAdmissionContext ClgAdmissionContext, DomainModelMsSqlServerContext para2, ILogger<ClgAttendanceEntryImpl> log, UserManager<ApplicationUser> userManager)
        {
            _ClgAdmissionContext = ClgAdmissionContext;
            _logatt = log;
            _db = para2;
            _userManager = userManager;
        }
        public CollegeMultiHoursAttendanceEntryDTO getalldetails(CollegeMultiHoursAttendanceEntryDTO data)
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
                                          select new CollegeMultiHoursAttendanceEntryDTO
                                          {
                                              rolename = a.IVRMRT_Role,
                                          }).ToList();

                    var userid = getuserid(data.username);

                    var empcode_check = (from a in _ClgAdmissionContext.Staff_User_Login
                                         where (a.MI_Id == data.MI_Id && a.Id.Equals(userid))
                                         select new CollegeMultiHoursAttendanceEntryDTO
                                         {
                                             Emp_Code = a.Emp_Code,
                                         }).ToList();

                    if (empcode_check.Count > 0)
                    {
                        data.getBranch = (from a in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                          from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                          from c in _ClgAdmissionContext.AcademicYear
                                          from d in _ClgAdmissionContext.ClgMasterBranchDMO
                                          where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMB_Id == d.AMB_Id && a.MI_Id == data.MI_Id
                                          && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.ASMAY_Id == data.ASMAY_Id && d.AMB_ActiveFlag == true
                                          && b.ACALD_ActiveFlag == true)
                                          select new CollegeMultiHoursAttendanceEntryDTO
                                          {
                                              AMB_Id = d.AMB_Id,
                                              AMB_BranchName = d.AMB_BranchName,
                                              AMB_Order = d.AMB_Order
                                          }).Distinct().OrderBy(a => a.AMB_Order).ToArray(); 
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
        public CollegeMultiHoursAttendanceEntryDTO getsubjectslist(CollegeMultiHoursAttendanceEntryDTO data)
        {
            try
            {

                List<long> branchid = new List<long>();

                foreach (var c in data.selectedbranchlist)
                {
                    branchid.Add(c.AMB_Id);
                }
                var userid = getuserid(data.username);

                var empcode_check = (from a in _ClgAdmissionContext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(userid))
                                     select new CollegeMultiHoursAttendanceEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count > 0)
                {
                    data.subjectlist = (from a in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                        from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                        from c in _ClgAdmissionContext.AcademicYear
                                        from d in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                                        where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.ISMS_Id == d.ISMS_Id && a.MI_Id == data.MI_Id
                                        && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.ASMAY_Id == data.ASMAY_Id && d.ISMS_ActiveFlag == 1
                                        && b.ACALD_ActiveFlag == true && branchid.Contains(b.AMB_Id))
                                        select new CollegeMultiHoursAttendanceEntryDTO
                                        {
                                            ISMS_Id = d.ISMS_Id,
                                            ISMS_SubjectName = d.ISMS_SubjectName
                                        }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeMultiHoursAttendanceEntryDTO getBatchdata(CollegeMultiHoursAttendanceEntryDTO data)
        {
            try
            {
                List<long> branchid = new List<long>();

                foreach (var c in data.selectedbranchlist)
                {
                    branchid.Add(c.AMB_Id);
                }

                var userid = getuserid(data.username);

                var empcode_check = (from a in _ClgAdmissionContext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(userid))
                                     select new CollegeMultiHoursAttendanceEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                var Coursecount = (from a in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                   from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                   from c in _ClgAdmissionContext.AcademicYear
                                   from d in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                                   where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.ISMS_Id == d.ISMS_Id && a.MI_Id == data.MI_Id
                                   && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.ASMAY_Id == data.ASMAY_Id && d.ISMS_ActiveFlag == 1
                                   && b.ACALD_ActiveFlag == true && d.ISMS_Id == data.ISMS_Id && b.ACALD_ActiveFlag == true && branchid.Contains(b.AMB_Id))
                                   select new CollegeAttendanceEntryNew_DTO
                                   {
                                       AMCO_Id = b.AMCO_Id
                                   }).Distinct().ToArray();

                var branchcount = (from a in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                   from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                   from c in _ClgAdmissionContext.AcademicYear
                                   from d in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                                   where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.ISMS_Id == d.ISMS_Id && a.MI_Id == data.MI_Id
                                   && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.ASMAY_Id == data.ASMAY_Id && d.ISMS_ActiveFlag == 1
                                   && b.ACALD_ActiveFlag == true && d.ISMS_Id == data.ISMS_Id && b.ACALD_ActiveFlag == true && branchid.Contains(b.AMB_Id))
                                   select new CollegeAttendanceEntryNew_DTO
                                   {
                                       AMB_Id = b.AMB_Id,
                                   }).Distinct().ToArray();

                var Semestercount = (from a in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                     from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                     from c in _ClgAdmissionContext.AcademicYear
                                     from d in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                                     where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.ISMS_Id == d.ISMS_Id && a.MI_Id == data.MI_Id
                                     && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.ASMAY_Id == data.ASMAY_Id && d.ISMS_ActiveFlag == 1
                                     && b.ACALD_ActiveFlag == true && d.ISMS_Id == data.ISMS_Id && b.ACALD_ActiveFlag == true && branchid.Contains(b.AMB_Id))
                                     select new CollegeAttendanceEntryNew_DTO
                                     {
                                         AMSE_Id = b.AMSE_Id,
                                     }).Distinct().ToArray();

                List<long> course_ids = new List<long>();
                List<long> bid = new List<long>();
                List<long> sectionid = new List<long>();
                List<long> semes_id = new List<long>();
                if (Coursecount.Count() > 0)
                {
                    foreach (var item in Coursecount)
                    {
                        course_ids.Add(item.AMCO_Id);
                    }
                }

                if (branchcount.Count() > 0)
                {
                    foreach (var item in branchcount)
                    {
                        bid.Add(item.AMB_Id);
                    }
                }
                if (Semestercount.Count() > 0)
                {
                    foreach (var item in Semestercount)
                    {
                        semes_id.Add(item.AMSE_Id);
                    }
                }

                var getbrachdetails = (from a in _ClgAdmissionContext.ClgMasterBranchDMO
                                       from d in _ClgAdmissionContext.AcademicYear
                                       from e in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                       from f in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                       where (a.AMB_Id == e.AMB_Id && f.ASMAY_Id == d.ASMAY_Id && e.ACALU_Id == f.ACALU_Id
                                       && d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && course_ids.Contains(e.AMCO_Id)
                                       && f.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && bid.Contains(e.AMB_Id) && e.ACALD_ActiveFlag == true)
                                       select new CollegeMultiHoursAttendanceEntryDTO
                                       {
                                           AMB_Id = a.AMB_Id,
                                           AMB_BranchName = a.AMB_BranchName,
                                           AMB_BranchCode = a.AMB_BranchCode,
                                           AMB_BranchType = a.AMB_BranchType,
                                           AMB_AidedUnAided = a.AMB_AidedUnAided
                                       }).Distinct().ToList();




                if (data.ACMS_Id > 0)
                {
                    var sectionlist = (from a in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                                       from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                       from f in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                       from g in _ClgAdmissionContext.AcademicYear
                                       where (a.ACMS_Id == b.ACMS_Id && b.AMB_Id == b.AMB_Id && b.AMCO_Id == b.AMCO_Id && b.AMSE_Id == b.AMSE_Id
                                       && f.ASMAY_Id == g.ASMAY_Id && bid.Contains(b.AMB_Id) && course_ids.Contains(b.AMCO_Id) && b.ACALU_Id == f.ACALU_Id
                                       && semes_id.Contains(b.AMSE_Id) && a.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id
                                       && f.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && b.ACMS_Id == data.ACMS_Id && b.ACALD_ActiveFlag == true)
                                       select new CollegeMultiHoursAttendanceEntryDTO
                                       {
                                           ACMS_Id = a.ACMS_Id
                                       }).Distinct().ToArray();


                    if (sectionlist.Count() > 0)
                    {
                        for (int k = 0; k < sectionlist.Count(); k++)
                        {
                            sectionid.Add(sectionlist[k].ACMS_Id);
                        }
                    }
                }
                else
                {
                    var sectionlist = (from a in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                                       from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                       from f in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                       from g in _ClgAdmissionContext.AcademicYear
                                       where (a.ACMS_Id == b.ACMS_Id && b.AMB_Id == b.AMB_Id && b.AMCO_Id == b.AMCO_Id && b.AMSE_Id == b.AMSE_Id
                                       && f.ASMAY_Id == g.ASMAY_Id && bid.Contains(b.AMB_Id) && course_ids.Contains(b.AMCO_Id) && b.ACALU_Id == f.ACALU_Id
                                       && semes_id.Contains(b.AMSE_Id) && a.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id
                                       && f.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && b.ACALD_ActiveFlag == true)
                                       select new CollegeMultiHoursAttendanceEntryDTO
                                       {
                                           ACMS_Id = a.ACMS_Id
                                       }).Distinct().ToList();

                    if (sectionlist.Count() > 0)
                    {
                        for (int k = 0; k < sectionlist.Count(); k++)
                        {
                            sectionid.Add(sectionlist[k].ACMS_Id);
                        }
                    }
                }

                data.getBatch = (from a in _ClgAdmissionContext.Adm_College_Attendance_BatchDMO
                                 from b in _ClgAdmissionContext.Adm_College_Atten_Batch_SubjectsDMO
                                 from c in _ClgAdmissionContext.Adm_College_Atten_Batch_Subject_StudentsDMO
                                 from d in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                                 from e in _ClgAdmissionContext.AcademicYear
                                 from f in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                                 from g in _ClgAdmissionContext.ClgMasterBranchDMO
                                 from h in _ClgAdmissionContext.MasterCourseDMO
                                 from i in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                 where (a.ACAB_Id == b.ACAB_Id && b.ACABS_Id == c.ACABS_Id && b.ISMS_Id == d.ISMS_Id && b.AMB_Id == g.AMB_Id && b.AMCO_Id == h.AMCO_Id
                                 && b.AMSE_Id == i.AMSE_Id && b.ACMS_Id == f.ACMS_Id && b.ASMAY_Id == e.ASMAY_Id && e.ASMAY_Id == data.ASMAY_Id
                                 && bid.Contains(g.AMB_Id) && course_ids.Contains(b.AMCO_Id) && semes_id.Contains(i.AMSE_Id) && sectionid.Contains(f.ACMS_Id)
                                 && d.ISMS_Id == data.ISMS_Id && a.MI_Id == data.MI_Id)
                                 select new CollegeMultiHoursAttendanceEntryDTO
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
        public CollegeMultiHoursAttendanceEntryDTO getStudentdata(CollegeMultiHoursAttendanceEntryDTO data)
        {
            try
            {
                List<long> branchid = new List<long>();

                foreach (var c in data.selectedbranchlist)
                {
                    branchid.Add(c.AMB_Id);
                }

                var attendance_entrytype = _ClgAdmissionContext.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();

                var check_attendance_entrytype = _ClgAdmissionContext.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();

                data.check_attendance_entrytype = check_attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type;

                List<CollegeMultiAttendanceEntryTempDTO> obj = new List<CollegeMultiAttendanceEntryTempDTO>();
                List<CollegeMultiAttendanceEntryTempDTO> studentList1 = new List<CollegeMultiAttendanceEntryTempDTO>();
                List<CollegeMultiAttendanceEntryTempDTO> finalstudentList1 = new List<CollegeMultiAttendanceEntryTempDTO>();
                List<CollegeMultiAttendanceEntryTempDTO> result = new List<CollegeMultiAttendanceEntryTempDTO>();
                List<CollegeMultiHoursAttendanceEntryDTO> check_period = new List<CollegeMultiHoursAttendanceEntryDTO>();

                List<CollegeMultiHoursAttendanceEntryDTO> getbrachdetailsnew = new List<CollegeMultiHoursAttendanceEntryDTO>();
                List<CollegeMultiHoursAttendanceEntryDTO> getsectionlistsnew = new List<CollegeMultiHoursAttendanceEntryDTO>();

                List<long> bid = new List<long>();
                List<long> sectionid = new List<long>();

                var check_rolename = (from a in _ClgAdmissionContext.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new CollegeAttendanceEntryNew_DTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var userid = getuserid(data.username);

                var empcode_check = (from a in _ClgAdmissionContext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(userid))
                                     select new CollegeMultiHoursAttendanceEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();


                var countCC = (from a in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                               from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                               from c in _ClgAdmissionContext.AcademicYear
                               from d in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                               where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.ISMS_Id == d.ISMS_Id && a.MI_Id == data.MI_Id
                               && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.ASMAY_Id == data.ASMAY_Id && d.ISMS_ActiveFlag == 1
                               && b.ACALD_ActiveFlag == true && d.ISMS_Id == data.ISMS_Id && branchid.Contains(b.AMB_Id))
                               select new CollegeAttendanceEntryNew_DTO
                               {
                                   AMCO_Id = b.AMCO_Id,
                                   AMSE_Id = b.AMSE_Id,
                               }).Distinct().ToList();

                getbrachdetailsnew = (from a in _ClgAdmissionContext.ClgMasterBranchDMO
                                      from d in _ClgAdmissionContext.AcademicYear
                                      from e in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                      from f in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                      where (a.AMB_Id == e.AMB_Id && f.ASMAY_Id == d.ASMAY_Id && e.ACALU_Id == f.ACALU_Id
                                      && d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && e.AMCO_Id == countCC.FirstOrDefault().AMCO_Id
                                      && f.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && e.AMSE_Id == countCC.FirstOrDefault().AMSE_Id
                                      && e.ISMS_Id == data.ISMS_Id && e.ACALD_ActiveFlag == true && f.ASMAY_Id == data.ASMAY_Id && branchid.Contains(e.AMB_Id))
                                      select new CollegeMultiHoursAttendanceEntryDTO
                                      {
                                          AMB_Id = a.AMB_Id,
                                          AMB_BranchName = a.AMB_BranchName,
                                          AMB_BranchCode = a.AMB_BranchCode,
                                          AMB_BranchType = a.AMB_BranchType,
                                          AMB_AidedUnAided = a.AMB_AidedUnAided
                                      }).Distinct().ToList();

                for (int k = 0; k < getbrachdetailsnew.Count(); k++)
                {
                    bid.Add(getbrachdetailsnew[k].AMB_Id);
                }


                getsectionlistsnew = (from a in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                                      from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                      from f in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                      from g in _ClgAdmissionContext.AcademicYear
                                      where (a.ACMS_Id == b.ACMS_Id && b.AMB_Id == b.AMB_Id && b.AMCO_Id == b.AMCO_Id && b.AMSE_Id == b.AMSE_Id
                                      && f.ASMAY_Id == g.ASMAY_Id && bid.Contains(b.AMB_Id) && b.AMCO_Id == countCC.FirstOrDefault().AMCO_Id
                                      && b.ACALU_Id == f.ACALU_Id && b.AMSE_Id == countCC.FirstOrDefault().AMSE_Id && a.MI_Id == data.MI_Id
                                      && g.ASMAY_Id == data.ASMAY_Id && f.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && b.ACALD_ActiveFlag == true
                                      && branchid.Contains(b.AMB_Id))
                                      select new CollegeMultiHoursAttendanceEntryDTO
                                      {
                                          ACMS_Id = a.ACMS_Id
                                      }).Distinct().ToList();


                data.AMCO_Id = countCC.FirstOrDefault().AMCO_Id;

                data.AMSE_Id = countCC.FirstOrDefault().AMSE_Id;

                for (int bnch = 0; bnch < getbrachdetailsnew.Count(); bnch++)
                {
                    data.AMB_Id = getbrachdetailsnew[bnch].AMB_Id;

                    // For Loop For Section List
                    for (int sec = 0; sec < getsectionlistsnew.Count(); sec++)
                    {
                        data.ACMS_Id = getsectionlistsnew[sec].ACMS_Id;

                        // For Loop For Period List
                        for (int k = 0; k < data.ClgAttendanceEntryTTPeriodTempDTO.Count(); k++)
                        {
                            data.TTMP_Id = data.ClgAttendanceEntryTTPeriodTempDTO[k].TTMP_Id;
                            DateTime fromdatecon = DateTime.Now;
                            string confromdate = "";
                            if (data.ACSA_AttendanceDate != null)
                            {
                                try
                                {
                                    fromdatecon = Convert.ToDateTime(data.ACSA_AttendanceDate.Value.Date.ToString("yyyy-MM-dd"));
                                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                                }

                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }


                            //CHECKING SELECTED SUBJECT IS ELECTIVE SUBJECT OR NOT FOR SELECTED YEAR , COURSE , BRANCH , SEMESTER ,SECTION 

                            var checksubjectiselectiveornot = _ClgAdmissionContext.Exm_Col_Studentwise_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id
                            && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id
                            && a.ACMS_Id == data.ACMS_Id && a.ISMS_Id == data.ISMS_Id && a.ECSTSU_ElectiveFlag == true && a.ECSTSU_ActiveFlg == true).ToList();


                            //  CHECKING IF SUBJECT IS NOT ELECTVE 

                            if (checksubjectiselectiveornot.Count() == 0)
                            {
                                // CHECKING WHETHER SELECTED DETAILS ATTENDANCE IS ENTERED OR NOT WITHOUT PASSING SUBJECT 

                                List<CollegeMultiHoursAttendanceEntryDTO> obj1 = new List<CollegeMultiHoursAttendanceEntryDTO>();
                                using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "College_Admission_Check_Attendance_Entered_Indi_BatchWise";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMB_Id) });
                                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMSE_Id) });
                                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ACMS_Id) });
                                    cmd.Parameters.Add(new SqlParameter("@TTMP_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.TTMP_Id) });
                                    cmd.Parameters.Add(new SqlParameter("@ACSA_AttendanceDate", SqlDbType.VarChar) { Value = confromdate });
                                    cmd.Parameters.Add(new SqlParameter("@ACAB_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ACAB_Id) });

                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();

                                    var retObject = new List<dynamic>();
                                    try
                                    {
                                        using (var resultc = cmd.ExecuteReader())
                                        {
                                            _logatt.LogInformation("entered in dataReader block");
                                            while (resultc.Read())
                                            {
                                                _logatt.LogInformation("entered in while block");

                                                obj1.Add(new CollegeMultiHoursAttendanceEntryDTO
                                                {
                                                    ACSA_Id = Convert.ToInt64(resultc["ACSA_Id"]),
                                                    TTMP_Id = Convert.ToInt64(resultc["TTMP_Id"]),
                                                });
                                            }
                                            check_period = obj1.ToList();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logatt.LogInformation("error:'" + ex.Message + "'");
                                        Console.Write(ex.Message);
                                    }
                                }

                                // IF ATTENDANCE IS ENTERD FOR THE PERIOD AND SELECTED DETAILS

                                if (check_period.Count() > 0)
                                {
                                    // CHECKING ATTENDANCE IS ENTERD FOR SELECTED DETAILS WITH PASSING SUBJECTE

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
                                                                     && a.AMCO_Id == e.AMCO_Id && a.AMB_Id == f.AMB_Id && a.AMSE_Id == g.AMSE_Id
                                                                     && a.ACMS_Id == h.ACMS_Id && i.TTMP_Id == c.TTMP_Id && a.ASMAY_Id == data.ASMAY_Id
                                                                     && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id
                                                                     && a.ACMS_Id == data.ACMS_Id && c.TTMP_Id == data.TTMP_Id && a.ISMS_Id == data.ISMS_Id
                                                                     && a.ISMS_Id == j.ISMS_Id && a.ISMS_Id == data.ISMS_Id
                                                                     && a.ACSA_AttendanceDate == data.ACSA_AttendanceDate
                                                                     && a.ACSA_Regular_Extra == data.ACSA_Regular_Extra && a.ACSA_ActiveFlag == true)
                                                                select new CollegeMultiHoursAttendanceEntryDTO
                                                                {
                                                                    ACSA_Id = c.ACSA_Id,
                                                                    TTMP_Id = c.TTMP_Id,
                                                                    ISMS_Id = a.ISMS_Id
                                                                }).Distinct().ToList();

                                    // IF ATTENDANCE IS ENTERED FOR SELECTED DETAILS

                                    if (check_period_subject.Count() > 0)
                                    {
                                        studentList1 = new List<CollegeMultiAttendanceEntryTempDTO>();
                                        obj = new List<CollegeMultiAttendanceEntryTempDTO>();

                                        //----- IF BATCH IS NOT SELECTED----------------/
                                        if (data.ACAB_Id == 0)
                                        {
                                            // GETTING SAVED ATTENDANCE DETAILS FOR SELECTED DETAILS 

                                            using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                                            {
                                                cmd.CommandText = "College_Attendance_Get_Student_List_Saved_Data";
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                                                cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMB_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMSE_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ACMS_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@ACSA_Regular_Extra", SqlDbType.VarChar) { Value = data.ACSA_Regular_Extra });
                                                cmd.Parameters.Add(new SqlParameter("@ACSA_AttendanceDate", SqlDbType.VarChar) { Value = confromdate });
                                                cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ISMS_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@TTMP_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.TTMP_Id) });

                                                if (cmd.Connection.State != ConnectionState.Open)
                                                    cmd.Connection.Open();

                                                var retObject = new List<dynamic>();
                                                try
                                                {
                                                    using (var dataReader = cmd.ExecuteReader())
                                                    {
                                                        _logatt.LogInformation("entered in dataReader block");
                                                        while (dataReader.Read())
                                                        {
                                                            _logatt.LogInformation("entered in while block");

                                                            obj.Add(new CollegeMultiAttendanceEntryTempDTO
                                                            {
                                                                AMCST_Id = Convert.ToInt64(dataReader["AMCST_Id"]),
                                                                AMCST_FirstName = dataReader["AMCST_FirstName"].ToString(),
                                                                AMCST_AdmNo = dataReader["AMCST_AdmNo"].ToString(),
                                                                ACYST_RollNo = Convert.ToInt64(dataReader["ACYST_RollNo"]),
                                                                AMCST_RegistrationNo = dataReader["AMCST_RegistrationNo"].ToString(),
                                                                pdays = Convert.ToInt64(dataReader["ASA_Class_Attended"]),
                                                                ACSAS_Id = Convert.ToInt64(dataReader["ACSAS_Id"]),
                                                                ACSA_Id = Convert.ToInt64(dataReader["ACSA_Id"]),
                                                                TTMP_Id = Convert.ToInt64(dataReader["TTMP_Id"]),
                                                                AMCO_Id = data.AMCO_Id,
                                                                AMB_Id = data.AMB_Id,
                                                                AMSE_Id = data.AMSE_Id,
                                                                ACMS_Id = data.ACMS_Id

                                                            });
                                                        }
                                                        studentList1 = obj.ToList();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    _logatt.LogInformation("error:'" + ex.Message + "'");
                                                    Console.Write(ex.Message);
                                                }
                                            }

                                            for (int i = 0; i < result.Count; i++)
                                            {
                                                studentList1.Add(result[i]);
                                            }

                                            for (int k1 = 0; k1 < studentList1.Count(); k1++)
                                            {
                                                finalstudentList1.Add(studentList1[k1]);
                                            }
                                        }
                                        else
                                        {
                                            //****************** IF BATCH IS SELECTED************************//
                                            result = new List<CollegeMultiAttendanceEntryTempDTO>();
                                            using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                                            {
                                                cmd.CommandText = "College_Attendance_Get_Student_List_Batchwise_Saved_Data";
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                                                cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMB_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMSE_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ACMS_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@ACAB_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ACAB_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ISMS_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@TTMP_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.TTMP_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@confromdate", SqlDbType.VarChar) { Value = confromdate });
                                                cmd.Parameters.Add(new SqlParameter("@regularexta", SqlDbType.VarChar) { Value = data.ACSA_Regular_Extra });

                                                if (cmd.Connection.State != ConnectionState.Open)
                                                    cmd.Connection.Open();

                                                var retObject = new List<dynamic>();
                                                try
                                                {
                                                    using (var dataReader = cmd.ExecuteReader())
                                                    {
                                                        _logatt.LogInformation("entered in dataReader block");
                                                        while (dataReader.Read())
                                                        {
                                                            _logatt.LogInformation("entered in while block");

                                                            result.Add(new CollegeMultiAttendanceEntryTempDTO
                                                            {
                                                                AMCST_Id = Convert.ToInt64(dataReader["AMCST_Id"]),
                                                                AMCST_FirstName = dataReader["AMCST_FirstName"].ToString(),
                                                                AMCST_AdmNo = dataReader["AMCST_AdmNo"].ToString(),
                                                                ACYST_RollNo = Convert.ToInt64(dataReader["ACYST_RollNo"]),
                                                                AMCST_RegistrationNo = dataReader["AMCST_RegistrationNo"].ToString(),
                                                                pdays = Convert.ToInt64(dataReader["ASA_Class_Attended"]),
                                                                ACSAS_Id = Convert.ToInt64(dataReader["ACSAS_Id"]),
                                                                ACSA_Id = Convert.ToInt64(dataReader["ACSA_Id"]),
                                                                TTMP_Id = Convert.ToInt64(dataReader["TTMP_Id"]),
                                                                AMCO_Id = data.AMCO_Id,
                                                                AMB_Id = data.AMB_Id,
                                                                AMSE_Id = data.AMSE_Id,
                                                                ACMS_Id = data.ACMS_Id
                                                            });
                                                        }
                                                        result.ToArray();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    _logatt.LogInformation("error:'" + ex.Message + "'");
                                                    Console.Write(ex.Message);
                                                }
                                            }
                                            for (int k2 = 0; k2 < result.Count(); k2++)
                                            {
                                                finalstudentList1.Add(result[k2]);
                                            }
                                        }
                                    }

                                    // IF ATTENDANCE IS ENTERD FOR SELECTED PERIOD BUT NOT FOR SELECTED SUBJECT
                                    else
                                    {
                                        data.message = "Already Attendance Is Enter For " + data.ClgAttendanceEntryTTPeriodTempDTO[k].TTMP_PeriodName + "Period";
                                        return data;
                                    }
                                }

                                // IF ATTENDANCE IS NOT ENTERED FOR SELECTED PREIOD

                                else
                                {
                                    result = new List<CollegeMultiAttendanceEntryTempDTO>();
                                    List<CollegeMultiAttendanceEntryTempDTO> list = new List<CollegeMultiAttendanceEntryTempDTO>();

                                    //  BATCH IS SELECTED
                                    if (data.ACAB_Id != 0)
                                    {
                                        using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                                        {
                                            cmd.CommandText = "College_Get_Student_List_Batchwise";
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                                            cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMB_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMSE_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ACMS_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@ACAB_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ACAB_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ISMS_Id) });
                                            if (cmd.Connection.State != ConnectionState.Open)
                                                cmd.Connection.Open();

                                            var retObject = new List<dynamic>();
                                            try
                                            {
                                                using (var dataReader = cmd.ExecuteReader())
                                                {
                                                    _logatt.LogInformation("entered in dataReader block");
                                                    while (dataReader.Read())
                                                    {
                                                        _logatt.LogInformation("entered in while block");

                                                        result.Add(new CollegeMultiAttendanceEntryTempDTO
                                                        {
                                                            AMCST_Id = Convert.ToInt64(dataReader["AMCST_Id"]),
                                                            AMCST_FirstName = (dataReader["AMCST_FirstName"]).ToString(),
                                                            AMCST_AdmNo = (dataReader["AMCST_AdmNo"]).ToString(),
                                                            ACYST_RollNo = Convert.ToInt64(dataReader["ACYST_RollNo"]),
                                                            AMCST_RegistrationNo = (dataReader["AMCST_RegistrationNo"]).ToString(),
                                                            TTMP_Id = data.ClgAttendanceEntryTTPeriodTempDTO[k].TTMP_Id,
                                                            AMCO_Id = data.AMCO_Id,
                                                            AMB_Id = data.AMB_Id,
                                                            AMSE_Id = data.AMSE_Id,
                                                            ACMS_Id = data.ACMS_Id
                                                        });
                                                    }
                                                    list = result.ToList();
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                _logatt.LogInformation("error:'" + ex.Message + "'");
                                                Console.Write(ex.Message);
                                            }
                                        }

                                        for (int k3 = 0; k3 < list.Count(); k3++)
                                        {
                                            finalstudentList1.Add(list[k3]);
                                        }
                                    }
                                    else
                                    {
                                        // IF BATCH IS NOT SELECTED

                                        result = new List<CollegeMultiAttendanceEntryTempDTO>();

                                        result = (from a in _ClgAdmissionContext.Adm_Master_College_StudentDMO
                                                  from b in _ClgAdmissionContext.Adm_College_Yearly_StudentDMO
                                                  from c in _ClgAdmissionContext.AcademicYear
                                                  from d in _ClgAdmissionContext.ClgMasterBranchDMO
                                                  from e in _ClgAdmissionContext.MasterCourseDMO
                                                  from f in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                                  from g in _ClgAdmissionContext.Adm_College_Master_SectionDMO

                                                  where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMB_Id == d.AMB_Id && b.AMCO_Id == e.AMCO_Id && b.AMSE_Id == f.AMSE_Id && b.ACMS_Id == g.ACMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMB_Id == data.AMB_Id && b.AMCO_Id == data.AMCO_Id && b.AMSE_Id == data.AMSE_Id && b.ACMS_Id == data.ACMS_Id && a.AMCST_SOL == "S" && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1)
                                                  select new CollegeMultiAttendanceEntryTempDTO
                                                  {
                                                      AMCST_Id = a.AMCST_Id,
                                                      AMCST_FirstName = ((a.AMCST_FirstName == null || a.AMCST_FirstName == "" ? "" : " " + a.AMCST_FirstName) + (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "" || a.AMCST_MiddleName == "0" ? "" : " " + a.AMCST_MiddleName) + (a.AMCST_LastName == null || a.AMCST_LastName == "" || a.AMCST_LastName == "0" ? "" : " " + a.AMCST_LastName)).Trim(),
                                                      AMCST_AdmNo = a.AMCST_AdmNo,
                                                      AMCST_RegistrationNo = a.AMCST_RegistrationNo,
                                                      ACYST_RollNo = b.ACYST_RollNo,
                                                      TTMP_Id = data.ClgAttendanceEntryTTPeriodTempDTO[k].TTMP_Id,
                                                      AMCO_Id = data.AMCO_Id,
                                                      AMB_Id = data.AMB_Id,
                                                      AMSE_Id = data.AMSE_Id,
                                                      ACMS_Id = data.ACMS_Id
                                                  }).Distinct().ToList();


                                        for (int k4 = 0; k4 < result.Count(); k4++)
                                        {
                                            finalstudentList1.Add(result[k4]);
                                        }
                                    }
                                }
                            }

                            // CHECKING IF SUBJECT IS ELECTIVE
                            else
                            {
                                // CHECKING WHETHER SELECTED DETAILS ATTENDANCE IS ENTERED OR NOT WITHOUT PASSING SUBJECT 

                                List<CollegeMultiHoursAttendanceEntryDTO> obj1 = new List<CollegeMultiHoursAttendanceEntryDTO>();
                                using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "College_Admission_Check_Attendance_Entered_Indi_BatchWise";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMB_Id) });
                                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMSE_Id) });
                                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ACMS_Id) });
                                    cmd.Parameters.Add(new SqlParameter("@TTMP_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.TTMP_Id) });
                                    cmd.Parameters.Add(new SqlParameter("@ACSA_AttendanceDate", SqlDbType.VarChar) { Value = confromdate });
                                    cmd.Parameters.Add(new SqlParameter("@ACAB_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ACAB_Id) });

                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();

                                    var retObject = new List<dynamic>();
                                    try
                                    {
                                        using (var resultc = cmd.ExecuteReader())
                                        {
                                            _logatt.LogInformation("entered in dataReader block");
                                            while (resultc.Read())
                                            {
                                                _logatt.LogInformation("entered in while block");

                                                obj1.Add(new CollegeMultiHoursAttendanceEntryDTO
                                                {
                                                    ACSA_Id = Convert.ToInt64(resultc["ACSA_Id"]),
                                                    TTMP_Id = Convert.ToInt64(resultc["TTMP_Id"]),
                                                });
                                            }
                                            check_period = obj1.ToList();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logatt.LogInformation("error:'" + ex.Message + "'");
                                        Console.Write(ex.Message);
                                    }
                                }

                                // IF ATTENDANCE IS ENTERD FOR THE PERIOD AND SELECTED DETAILS

                                if (check_period.Count() > 0)
                                {
                                    // CHECKING ATTENDANCE IS ENTERD FOR SELECTED DETAILS WITH PASSING SUBJECTE

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
                                                                     && a.ACSA_Regular_Extra == data.ACSA_Regular_Extra && a.ACSA_ActiveFlag == true)
                                                                select new CollegeMultiHoursAttendanceEntryDTO
                                                                {
                                                                    ACSA_Id = c.ACSA_Id,
                                                                    TTMP_Id = c.TTMP_Id,
                                                                    ISMS_Id = a.ISMS_Id
                                                                }).Distinct().ToList();

                                    // IF ATTENDANCE IS ENTERED FOR SELECTED DETAILS

                                    if (check_period_subject.Count() > 0)
                                    {
                                        studentList1 = new List<CollegeMultiAttendanceEntryTempDTO>();
                                        obj = new List<CollegeMultiAttendanceEntryTempDTO>();

                                        //----- IF BATCH IS NOT SELECTED----------------/
                                        if (data.ACAB_Id == 0)
                                        {
                                            // GETTING SAVED ATTENDANCE DETAILS FOR SELECTED DETAILS 

                                            using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                                            {
                                                cmd.CommandText = "College_Attendance_Get_Student_List_Saved_Data";
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                                                cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMB_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMSE_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ACMS_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@ACSA_Regular_Extra", SqlDbType.VarChar) { Value = data.ACSA_Regular_Extra });
                                                cmd.Parameters.Add(new SqlParameter("@ACSA_AttendanceDate", SqlDbType.VarChar) { Value = confromdate });
                                                cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ISMS_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@TTMP_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.TTMP_Id) });

                                                if (cmd.Connection.State != ConnectionState.Open)
                                                    cmd.Connection.Open();

                                                var retObject = new List<dynamic>();
                                                try
                                                {
                                                    using (var dataReader = cmd.ExecuteReader())
                                                    {
                                                        _logatt.LogInformation("entered in dataReader block");
                                                        while (dataReader.Read())
                                                        {
                                                            _logatt.LogInformation("entered in while block");

                                                            obj.Add(new CollegeMultiAttendanceEntryTempDTO
                                                            {
                                                                AMCST_Id = Convert.ToInt64(dataReader["AMCST_Id"]),
                                                                AMCST_FirstName = dataReader["AMCST_FirstName"].ToString(),
                                                                AMCST_AdmNo = dataReader["AMCST_AdmNo"].ToString(),
                                                                ACYST_RollNo = Convert.ToInt64(dataReader["ACYST_RollNo"]),
                                                                AMCST_RegistrationNo = dataReader["AMCST_RegistrationNo"].ToString(),
                                                                pdays = Convert.ToInt64(dataReader["ASA_Class_Attended"]),
                                                                ACSAS_Id = Convert.ToInt64(dataReader["ACSAS_Id"]),
                                                                ACSA_Id = Convert.ToInt64(dataReader["ACSA_Id"]),
                                                                TTMP_Id = Convert.ToInt64(dataReader["TTMP_Id"]),
                                                                AMCO_Id = data.AMCO_Id,
                                                                AMB_Id = data.AMB_Id,
                                                                AMSE_Id = data.AMSE_Id,
                                                                ACMS_Id = data.ACMS_Id

                                                            });
                                                        }
                                                        studentList1 = obj.ToList();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    _logatt.LogInformation("error:'" + ex.Message + "'");
                                                    Console.Write(ex.Message);
                                                }
                                            }

                                            for (int i = 0; i < result.Count; i++)
                                            {
                                                studentList1.Add(result[i]);
                                            }

                                            for (int k1 = 0; k1 < studentList1.Count(); k1++)
                                            {
                                                finalstudentList1.Add(studentList1[k1]);
                                            }
                                        }
                                        else
                                        {
                                            //****************** IF BATCH IS SELECTED************************//
                                            result = new List<CollegeMultiAttendanceEntryTempDTO>();
                                            using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                                            {
                                                cmd.CommandText = "College_Attendance_Get_Student_List_Batchwise_Saved_Data";
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                                                cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMB_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMSE_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ACMS_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@ACAB_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ACAB_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ISMS_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@TTMP_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.TTMP_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@confromdate", SqlDbType.VarChar) { Value = confromdate });
                                                cmd.Parameters.Add(new SqlParameter("@regularexta", SqlDbType.VarChar) { Value = data.ACSA_Regular_Extra });

                                                if (cmd.Connection.State != ConnectionState.Open)
                                                    cmd.Connection.Open();

                                                var retObject = new List<dynamic>();
                                                try
                                                {
                                                    using (var dataReader = cmd.ExecuteReader())
                                                    {
                                                        _logatt.LogInformation("entered in dataReader block");
                                                        while (dataReader.Read())
                                                        {
                                                            _logatt.LogInformation("entered in while block");

                                                            result.Add(new CollegeMultiAttendanceEntryTempDTO
                                                            {
                                                                AMCST_Id = Convert.ToInt64(dataReader["AMCST_Id"]),
                                                                AMCST_FirstName = dataReader["AMCST_FirstName"].ToString(),
                                                                AMCST_AdmNo = dataReader["AMCST_AdmNo"].ToString(),
                                                                ACYST_RollNo = Convert.ToInt64(dataReader["ACYST_RollNo"]),
                                                                AMCST_RegistrationNo = dataReader["AMCST_RegistrationNo"].ToString(),
                                                                pdays = Convert.ToInt64(dataReader["ASA_Class_Attended"]),
                                                                ACSAS_Id = Convert.ToInt64(dataReader["ACSAS_Id"]),
                                                                ACSA_Id = Convert.ToInt64(dataReader["ACSA_Id"]),
                                                                TTMP_Id = Convert.ToInt64(dataReader["TTMP_Id"]),
                                                                AMCO_Id = data.AMCO_Id,
                                                                AMB_Id = data.AMB_Id,
                                                                AMSE_Id = data.AMSE_Id,
                                                                ACMS_Id = data.ACMS_Id
                                                            });
                                                        }
                                                        result.ToArray();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    _logatt.LogInformation("error:'" + ex.Message + "'");
                                                    Console.Write(ex.Message);
                                                }
                                            }

                                            for (int k2 = 0; k2 < result.Count(); k2++)
                                            {
                                                finalstudentList1.Add(result[k2]);
                                            }
                                        }
                                    }

                                    // IF ATTENDANCE IS ENTERD FOR SELECTED PERIOD BUT NOT FOR SELECTED SUBJECT
                                    else
                                    {
                                        // CHECK THE ENTERED ATTENDANCE IS ELECTIVE SUBJECT OR NOT

                                        var checkenteredsubjectiselectiveornot = (from a in _ClgAdmissionContext.Adm_College_Student_AttendanceDMO
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
                                                                                       && c.TTMP_Id == data.TTMP_Id && a.ISMS_Id == j.ISMS_Id && a.ACSA_AttendanceDate == data.ACSA_AttendanceDate
                                                                                       && a.ACSA_Regular_Extra == data.ACSA_Regular_Extra && a.ACSA_ActiveFlag == true)
                                                                                  select new CollegeMultiHoursAttendanceEntryDTO
                                                                                  {
                                                                                      ISMS_Id = a.ISMS_Id
                                                                                  }).Distinct().ToList();

                                        // CHECKING SUBJECT IS ELECTIVE OR NOT

                                        var checkingsubjectiseleciveornot1 = _ClgAdmissionContext.Exm_Col_Studentwise_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id
                                 && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id
                                 && a.ACMS_Id == data.ACMS_Id && a.ISMS_Id == checkenteredsubjectiselectiveornot.FirstOrDefault().ISMS_Id
                                 && a.ECSTSU_ElectiveFlag == true && a.ECSTSU_ActiveFlg == true).ToList();

                                        //IF ENTERED ATTENDANCE SUBJECT IS ELECTIVE 
                                        if (checkingsubjectiseleciveornot1.Count() > 0)
                                        {
                                            // GET THE STUDENT LIST BASED ON ELECTIVE SUBJECTS

                                            result = new List<CollegeMultiAttendanceEntryTempDTO>();
                                            List<CollegeMultiAttendanceEntryTempDTO> list = new List<CollegeMultiAttendanceEntryTempDTO>();

                                            using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                                            {
                                                cmd.CommandText = "College_Get_Student_List_Batchwise_ElectiveSubjewise";
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                                                cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMB_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMSE_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ACMS_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@ACAB_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ACAB_Id) });
                                                cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ISMS_Id) });
                                                if (cmd.Connection.State != ConnectionState.Open)
                                                    cmd.Connection.Open();

                                                var retObject = new List<dynamic>();
                                                try
                                                {
                                                    using (var dataReader = cmd.ExecuteReader())
                                                    {
                                                        _logatt.LogInformation("entered in dataReader block");
                                                        while (dataReader.Read())
                                                        {
                                                            _logatt.LogInformation("entered in while block");

                                                            result.Add(new CollegeMultiAttendanceEntryTempDTO
                                                            {
                                                                AMCST_Id = Convert.ToInt64(dataReader["AMCST_Id"]),
                                                                AMCST_FirstName = (dataReader["AMCST_FirstName"]).ToString(),
                                                                AMCST_AdmNo = (dataReader["AMCST_AdmNo"]).ToString(),
                                                                ACYST_RollNo = Convert.ToInt64(dataReader["ACYST_RollNo"]),
                                                                AMCST_RegistrationNo = (dataReader["AMCST_RegistrationNo"]).ToString(),
                                                                TTMP_Id = data.ClgAttendanceEntryTTPeriodTempDTO[k].TTMP_Id,
                                                                AMCO_Id = data.AMCO_Id,
                                                                AMB_Id = data.AMB_Id,
                                                                AMSE_Id = data.AMSE_Id,
                                                                ACMS_Id = data.ACMS_Id
                                                            });
                                                        }
                                                        list = result.ToList();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    _logatt.LogInformation("error:'" + ex.Message + "'");
                                                    Console.Write(ex.Message);
                                                }
                                            }

                                            for (int k3 = 0; k3 < list.Count(); k3++)
                                            {
                                                finalstudentList1.Add(list[k3]);
                                            }

                                        }
                                        else
                                        {
                                            data.message = "Already Attendance Is Enter For " + data.ClgAttendanceEntryTTPeriodTempDTO[k].TTMP_PeriodName + "Period";
                                        }

                                    }
                                }

                                // GETTING STUDENT NAMES BASED ON ELECTIVE SUBJECT WISE
                                else
                                {
                                    result = new List<CollegeMultiAttendanceEntryTempDTO>();
                                    List<CollegeMultiAttendanceEntryTempDTO> list = new List<CollegeMultiAttendanceEntryTempDTO>();

                                    using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd.CommandText = "College_Get_Student_List_Batchwise_ElectiveSubjewise";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                                        cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                                        cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMB_Id) });
                                        cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMSE_Id) });
                                        cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ACMS_Id) });
                                        cmd.Parameters.Add(new SqlParameter("@ACAB_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ACAB_Id) });
                                        cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ISMS_Id) });
                                        if (cmd.Connection.State != ConnectionState.Open)
                                            cmd.Connection.Open();

                                        var retObject = new List<dynamic>();
                                        try
                                        {
                                            using (var dataReader = cmd.ExecuteReader())
                                            {
                                                _logatt.LogInformation("entered in dataReader block");
                                                while (dataReader.Read())
                                                {
                                                    _logatt.LogInformation("entered in while block");

                                                    result.Add(new CollegeMultiAttendanceEntryTempDTO
                                                    {
                                                        AMCST_Id = Convert.ToInt64(dataReader["AMCST_Id"]),
                                                        AMCST_FirstName = (dataReader["AMCST_FirstName"]).ToString(),
                                                        AMCST_AdmNo = (dataReader["AMCST_AdmNo"]).ToString(),
                                                        ACYST_RollNo = Convert.ToInt64(dataReader["ACYST_RollNo"]),
                                                        AMCST_RegistrationNo = (dataReader["AMCST_RegistrationNo"]).ToString(),
                                                        TTMP_Id = data.ClgAttendanceEntryTTPeriodTempDTO[k].TTMP_Id,
                                                        AMCO_Id = data.AMCO_Id,
                                                        AMB_Id = data.AMB_Id,
                                                        AMSE_Id = data.AMSE_Id,
                                                        ACMS_Id = data.ACMS_Id
                                                    });
                                                }
                                                list = result.ToList();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            _logatt.LogInformation("error:'" + ex.Message + "'");
                                            Console.Write(ex.Message);
                                        }
                                    }

                                    for (int k3 = 0; k3 < list.Count(); k3++)
                                    {
                                        finalstudentList1.Add(list[k3]);
                                    }
                                }

                            }
                        }
                    }
                }
                data.getStudentdetails = finalstudentList1.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeMultiHoursAttendanceEntryDTO saveatt(CollegeMultiHoursAttendanceEntryDTO data)
        {
            try
            {
                string branchids = "0";
                string sectionids = "0";
                string periodids = "0";

                List<long> branchid = new List<long>();

                foreach (var c in data.selectedbranchlist)
                {
                    branchid.Add(c.AMB_Id);
                }

                List<CollegeMultiHoursAttendanceEntryDTO> getbrachdetailsnew = new List<CollegeMultiHoursAttendanceEntryDTO>();
                List<CollegeMultiHoursAttendanceEntryDTO> getsectionlistsnew = new List<CollegeMultiHoursAttendanceEntryDTO>();
                List<CollegeMultiHoursAttendanceEntryDTO> getsectiondetailssnew = new List<CollegeMultiHoursAttendanceEntryDTO>();

                var check_attendance_entrytype = _ClgAdmissionContext.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                List<long> bid1 = new List<long>();
                List<long> sectionid1 = new List<long>();
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                var MAACAdd = sMacAddress;

                var check_rolename = (from a in _ClgAdmissionContext.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new CollegeAttendanceEntryNew_DTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var userid = getuserid(data.username);

                var empcode_check = (from a in _ClgAdmissionContext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(userid))
                                     select new CollegeMultiHoursAttendanceEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();


                var countCC = (from a in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                               from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                               from c in _ClgAdmissionContext.AcademicYear
                               from d in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                               where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.ISMS_Id == d.ISMS_Id && a.MI_Id == data.MI_Id
                               && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.ASMAY_Id == data.ASMAY_Id && d.ISMS_ActiveFlag == 1
                               && b.ACALD_ActiveFlag == true && d.ISMS_Id == data.ISMS_Id && b.ACALD_ActiveFlag == true && branchid.Contains(b.AMB_Id))
                               select new CollegeAttendanceEntryNew_DTO
                               {
                                   AMCO_Id = b.AMCO_Id,
                                   AMSE_Id = b.AMSE_Id,
                               }).Distinct().ToList();

                getbrachdetailsnew = (from a in _ClgAdmissionContext.ClgMasterBranchDMO
                                      from d in _ClgAdmissionContext.AcademicYear
                                      from e in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                      from f in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                      where (a.AMB_Id == e.AMB_Id && f.ASMAY_Id == d.ASMAY_Id && e.ACALU_Id == f.ACALU_Id
                                      && d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && e.AMCO_Id == countCC.FirstOrDefault().AMCO_Id
                                      && f.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && e.AMSE_Id == countCC.FirstOrDefault().AMSE_Id
                                      && e.ISMS_Id == data.ISMS_Id && e.ACALD_ActiveFlag == true && branchid.Contains(e.AMB_Id))
                                      select new CollegeMultiHoursAttendanceEntryDTO
                                      {
                                          AMB_Id = a.AMB_Id,
                                          AMB_BranchName = a.AMB_BranchName,
                                          AMB_BranchCode = a.AMB_BranchCode,
                                          AMB_BranchType = a.AMB_BranchType,
                                          AMB_AidedUnAided = a.AMB_AidedUnAided
                                      }).Distinct().ToList();

                for (int k = 0; k < getbrachdetailsnew.Count(); k++)
                {
                    bid1.Add(getbrachdetailsnew[k].AMB_Id);
                    branchids = branchids + "," + getbrachdetailsnew[k].AMB_Id;
                }


                getsectionlistsnew = (from a in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                                      from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                      from f in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                      from g in _ClgAdmissionContext.AcademicYear
                                      where (a.ACMS_Id == b.ACMS_Id && b.AMB_Id == b.AMB_Id && b.AMCO_Id == b.AMCO_Id && b.AMSE_Id == b.AMSE_Id
                                      && f.ASMAY_Id == g.ASMAY_Id && bid1.Contains(b.AMB_Id) && b.AMCO_Id == countCC.FirstOrDefault().AMCO_Id
                                      && b.ACALU_Id == f.ACALU_Id && b.AMSE_Id == countCC.FirstOrDefault().AMSE_Id && a.MI_Id == data.MI_Id
                                      && g.ASMAY_Id == data.ASMAY_Id && f.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && b.ACALD_ActiveFlag == true
                                      && branchid.Contains(b.AMB_Id))
                                      select new CollegeMultiHoursAttendanceEntryDTO
                                      {
                                          ACMS_Id = a.ACMS_Id
                                      }).Distinct().ToList();


                data.AMCO_Id = countCC.FirstOrDefault().AMCO_Id;

                data.AMSE_Id = countCC.FirstOrDefault().AMSE_Id;

                if (getsectionlistsnew.Count() > 0)
                {
                    for (int k = 0; k < getsectionlistsnew.Count(); k++)
                    {
                        sectionid1.Add(getsectionlistsnew[k].ACMS_Id);
                        sectionids = sectionids + "," + getsectionlistsnew[k].ACMS_Id;
                    }
                    getsectiondetailssnew = getsectionlistsnew.ToList();
                }
                var emp_att_login_check = (from a in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                           from c in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                           where (a.ACALU_Id == c.ACALU_Id && a.MI_Id == data.MI_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                           && c.AMCO_Id == data.AMCO_Id && bid1.Contains(c.AMB_Id) && a.ASMAY_Id == data.ASMAY_Id && sectionid1.Contains(c.ACMS_Id)
                                           && c.ISMS_Id == data.ISMS_Id && c.AMSE_Id == data.AMSE_Id && branchid.Contains(c.AMB_Id))
                                           select new CollegeMultiHoursAttendanceEntryDTO
                                           {
                                               ACALU_Id = a.ACALU_Id,
                                           }).ToList();


                if (emp_att_login_check.Count == 0)
                {
                    data.message = "For This Staff There Is No Privileges To Enter Attendance.. Please Map The Staff In Attendance Privileges";
                    return data;
                }

                for (int ttmp = 0; ttmp < data.ClgAttendanceEntryTTPeriodTempDTO.Count(); ttmp++)
                {
                    periodids = periodids + "," + data.ClgAttendanceEntryTTPeriodTempDTO[ttmp].TTMP_Id;
                }

                // For Loop For Branch
                for (int amb = 0; amb < getbrachdetailsnew.Count(); amb++)
                {
                    data.AMB_Id = getbrachdetailsnew[amb].AMB_Id;
                    int knew = 0;

                    // For Loop For Section 
                    for (int secid = 0; secid < getsectiondetailssnew.Count(); secid++)
                    {
                        knew = 0;
                        data.ACMS_Id = getsectiondetailssnew[secid].ACMS_Id;

                        // For Loop For Period
                        for (int ttmp = 0; ttmp < data.ClgAttendanceEntryTTPeriodTempDTO.Count(); ttmp++)
                        {
                            knew = 0;

                            data.TTMP_Id = data.ClgAttendanceEntryTTPeriodTempDTO[ttmp].TTMP_Id;

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
                                                       && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACMS_Id == data.ACMS_Id && c.TTMP_Id == data.TTMP_Id && a.ISMS_Id == data.ISMS_Id && a.ACSA_AttendanceDate == data.ACSA_AttendanceDate && a.ACSA_Regular_Extra == data.ACSA_Regular_Extra && a.ACSA_ActiveFlag == true)
                                                       select new CollegeMultiHoursAttendanceEntryDTO
                                                       {
                                                           ACSA_Id = a.ACSA_Id,
                                                       }).ToList();

                                if (check_duplicate.Count > 0)
                                {
                                    for (int kk = 0; kk < data.ClgAttendanceEntryTempDTO.Count(); kk++)
                                    {
                                        // If Loop For Branch Compare
                                        if (data.ClgAttendanceEntryTempDTO[kk].AMB_Id == data.AMB_Id && data.ClgAttendanceEntryTempDTO[kk].ACMS_Id == data.ACMS_Id)
                                        {
                                            // If Loop For Section Compare

                                            for (int kj = 0; kj < data.ClgAttendanceEntryTempDTO[kk].sub_list.Count(); kj++)
                                            {
                                                if (data.TTMP_Id == data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].TTMP_Id)
                                                {
                                                    if (kk == 0)
                                                    {
                                                        if (data.ACAB_Id == 0)
                                                        {
                                                            var result1 = _ClgAdmissionContext.Adm_College_Student_AttendanceDMO.Single(d => d.ACSA_Id == data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].ACSA_Id);
                                                            result1.ACSA_Att_EntryType = "Absent";
                                                            result1.ACALU_Id = emp_att_login_check.FirstOrDefault().ACALU_Id;
                                                            result1.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                                                            result1.UpdatedDate = DateTime.Now;
                                                            _ClgAdmissionContext.Adm_College_Student_AttendanceDMO.Update(result1);

                                                            var resulttt = _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO.Single(d => d.ACSA_Id == data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].ACSA_Id && d.TTMP_Id == data.TTMP_Id);
                                                            resulttt.UpdatedDate = DateTime.Now;
                                                            _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO.Update(resulttt);
                                                        }
                                                        else
                                                        {
                                                            var result2 = _ClgAdmissionContext.Adm_College_Student_AttendanceDMO.Single(d => d.ACSA_Id == check_duplicate.FirstOrDefault().ACSA_Id);
                                                            result2.ACSA_Att_EntryType = "Absent";
                                                            result2.ACALU_Id = emp_att_login_check.FirstOrDefault().ACALU_Id;
                                                            result2.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                                                            result2.UpdatedDate = DateTime.Now;
                                                            _ClgAdmissionContext.Adm_College_Student_AttendanceDMO.Update(result2);

                                                            var resulttt = _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO.Single(d => d.ACSA_Id == check_duplicate.FirstOrDefault().ACSA_Id && d.TTMP_Id == data.TTMP_Id);
                                                            resulttt.UpdatedDate = DateTime.Now;
                                                            _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO.Update(resulttt);
                                                        }
                                                    }

                                                    if (data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].ACSAS_Id != null)
                                                    {
                                                        var result3 = _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO.Single(a => a.ACSA_Id == data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].ACSA_Id && a.AMCST_Id == data.ClgAttendanceEntryTempDTO[kk].AMCST_Id
                                                        && a.ACSAS_Id == data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].ACSAS_Id);

                                                        if (data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].Selected == false)
                                                        {
                                                            result3.ACSAS_AttendanceFlag = "Present";
                                                            result3.ACSAS_ClassAttended = 1;

                                                        }
                                                        else if (data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].Selected == true)
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
                                                        stdperiod.AMCST_Id = Convert.ToInt64(data.ClgAttendanceEntryTempDTO[kk].AMCST_Id);

                                                        if (data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].Selected == true)
                                                        {
                                                            stdperiod.ACSAS_AttendanceFlag = "Absent";
                                                            stdperiod.ACSAS_ClassAttended = 0;
                                                        }
                                                        else if (data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].Selected == false)
                                                        {
                                                            stdperiod.ACSAS_AttendanceFlag = "Present";
                                                            stdperiod.ACSAS_ClassAttended = 1;
                                                        }

                                                        stdperiod.CreatedDate = DateTime.Now;
                                                        stdperiod.UpdatedDate = DateTime.Now;
                                                        _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO.Add(stdperiod);
                                                    }
                                                }
                                            }

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
                                    int savedata = 0;
                                    for (int kk = 0; kk < data.ClgAttendanceEntryTempDTO.Count(); kk++)
                                    {
                                        if (data.AMB_Id == data.ClgAttendanceEntryTempDTO[kk].AMB_Id && data.ACMS_Id == data.ClgAttendanceEntryTempDTO[kk].ACMS_Id)
                                        {
                                            savedata = savedata + 1;

                                            if (knew == 0)
                                            {
                                                knew = knew + 1;
                                                enq.MI_Id = data.MI_Id;
                                                enq.ASMAY_Id = data.ASMAY_Id;
                                                enq.ACSA_Att_EntryType = "Absent";
                                                enq.AMCO_Id = data.AMCO_Id;
                                                enq.AMB_Id = data.AMB_Id;
                                                enq.AMSE_Id = data.AMSE_Id;
                                                enq.ACMS_Id = data.ACMS_Id;
                                                enq.ISMS_Id = data.ISMS_Id;
                                                enq.ACSA_Entry = DateTime.Now;
                                                enq.ACSA_AttendanceDate = Convert.ToDateTime(data.ACSA_AttendanceDate);
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
                                            }

                                            if (data.ClgAttendanceEntryTempDTO != null && data.ClgAttendanceEntryTempDTO.Count() > 0)
                                            {
                                                // If Loop For Branch Compare

                                                for (int i = 0; i < data.ClgAttendanceEntryTempDTO[kk].sub_list.Count(); i++)
                                                {
                                                    Adm_College_Student_Attendance_StudentsDMO std = new Adm_College_Student_Attendance_StudentsDMO();
                                                    std.ACSA_Id = enq.ACSA_Id;
                                                    if (data.TTMP_Id == data.ClgAttendanceEntryTempDTO[kk].sub_list[i].TTMP_Id)
                                                    {
                                                        if (data.ClgAttendanceEntryTempDTO[kk].sub_list[i].Selected == false)
                                                        {
                                                            std.ACSAS_AttendanceFlag = "Present";
                                                            std.ACSAS_ClassAttended = 1;
                                                        }
                                                        else if (data.ClgAttendanceEntryTempDTO[kk].sub_list[i].Selected == true)
                                                        {
                                                            std.ACSAS_AttendanceFlag = "Absent";
                                                            std.ACSAS_ClassAttended = 0;
                                                        }
                                                        std.AMCST_Id = Convert.ToInt64(data.ClgAttendanceEntryTempDTO[kk].AMCST_Id);
                                                        std.CreatedDate = DateTime.Now;
                                                        std.UpdatedDate = DateTime.Now;
                                                        _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO.Add(std);
                                                    }
                                                }

                                            }

                                        }
                                    }
                                    if (savedata > 0)
                                    {
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
                                                       && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACMS_Id == data.ACMS_Id && c.TTMP_Id == data.TTMP_Id && a.ISMS_Id == data.ISMS_Id && a.ACSA_AttendanceDate == data.ACSA_AttendanceDate && a.ACSA_Regular_Extra == data.ACSA_Regular_Extra && a.ACSA_ActiveFlag == true)
                                                       select new CollegeMultiHoursAttendanceEntryDTO
                                                       {
                                                           ACSA_Id = a.ACSA_Id,
                                                       }).ToList();

                                if (check_duplicate.Count > 0)
                                {
                                    for (int kk = 0; kk < data.ClgAttendanceEntryTempDTO.Count(); kk++)
                                    {

                                        // If Loop For Branch Compare
                                        if (data.ClgAttendanceEntryTempDTO[kk].AMB_Id == data.AMB_Id)
                                        {
                                            // If Loop For Section Compate
                                            if (data.ClgAttendanceEntryTempDTO[kk].ACMS_Id == data.ACMS_Id)
                                            {
                                                for (int kj = 0; kj < data.ClgAttendanceEntryTempDTO[kk].sub_list.Count(); kj++)
                                                {
                                                    if (data.TTMP_Id == data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].TTMP_Id)
                                                    {
                                                        if (kk == 0)
                                                        {
                                                            if (data.ACAB_Id == 0)
                                                            {
                                                                var result3 = _ClgAdmissionContext.Adm_College_Student_AttendanceDMO.Single(d => d.ACSA_Id == data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].ACSA_Id);
                                                                result3.ACSA_Att_EntryType = "Present";
                                                                result3.ACALU_Id = emp_att_login_check.FirstOrDefault().ACALU_Id;
                                                                result3.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                                                                result3.UpdatedDate = DateTime.Now;
                                                                _ClgAdmissionContext.Adm_College_Student_AttendanceDMO.Update(result3);

                                                                var resulttt = _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO.Single(d => d.ACSA_Id == data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].ACSA_Id && d.TTMP_Id == data.TTMP_Id);
                                                                resulttt.UpdatedDate = DateTime.Now;
                                                                _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO.Update(resulttt);
                                                            }
                                                            else
                                                            {
                                                                var result4 = _ClgAdmissionContext.Adm_College_Student_AttendanceDMO.Single(d => d.ACSA_Id == check_duplicate.FirstOrDefault().ACSA_Id);
                                                                result4.ACSA_Att_EntryType = "Present";
                                                                result4.ACALU_Id = emp_att_login_check.FirstOrDefault().ACALU_Id;
                                                                result4.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                                                                result4.UpdatedDate = DateTime.Now;
                                                                _ClgAdmissionContext.Adm_College_Student_AttendanceDMO.Update(result4);

                                                                var resulttt = _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO.Single(d => d.ACSA_Id == check_duplicate.FirstOrDefault().ACSA_Id && d.TTMP_Id == data.TTMP_Id);
                                                                resulttt.UpdatedDate = DateTime.Now;
                                                                _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO.Update(resulttt);
                                                            }
                                                        }

                                                        if (data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].ACSAS_Id != null)
                                                        {
                                                            var result3 = _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO.Single(a => a.ACSA_Id == data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].ACSA_Id && a.AMCST_Id == data.ClgAttendanceEntryTempDTO[kk].AMCST_Id
                                                            && a.ACSAS_Id == data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].ACSAS_Id);

                                                            if (data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].Selected == true)
                                                            {
                                                                result3.ACSAS_AttendanceFlag = "Present";
                                                                result3.ACSAS_ClassAttended = 1;

                                                            }
                                                            else if (data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].Selected == false)
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
                                                            stdperiod.AMCST_Id = Convert.ToInt64(data.ClgAttendanceEntryTempDTO[kk].AMCST_Id);

                                                            if (data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].Selected == false)
                                                            {
                                                                stdperiod.ACSAS_AttendanceFlag = "Absent";
                                                                stdperiod.ACSAS_ClassAttended = 0;
                                                            }
                                                            else if (data.ClgAttendanceEntryTempDTO[kk].sub_list[kj].Selected == true)
                                                            {
                                                                stdperiod.ACSAS_AttendanceFlag = "Present";
                                                                stdperiod.ACSAS_ClassAttended = 1;
                                                            }

                                                            stdperiod.CreatedDate = DateTime.Now;
                                                            stdperiod.UpdatedDate = DateTime.Now;
                                                            _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO.Add(stdperiod);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //for (int i = 0; i < data.CollegeMultiAttendanceEntryTempDTO[ttmp].sub_list.Count(); i++)
                                    //{
                                    //    if (i == 0)
                                    //    {
                                    //        var result = _ClgAdmissionContext.Adm_College_Student_AttendanceDMO.Single(d => d.ACSA_Id == data.CollegeMultiAttendanceEntryTempDTO[i].sub_list[i].ACSA_Id);
                                    //        result.ACSA_Att_EntryType = "Absent";
                                    //        result.ACALU_Id = emp_att_login_check.FirstOrDefault().ACALU_Id;
                                    //        result.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                                    //        result.UpdatedDate = DateTime.Now;
                                    //        _ClgAdmissionContext.Adm_College_Student_AttendanceDMO.Update(result);

                                    //        var resulttt = _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO.Single(d => d.ACSA_Id == data.CollegeMultiAttendanceEntryTempDTO[i].ACSA_Id && d.TTMP_Id == data.TTMP_Id);
                                    //        resulttt.UpdatedDate = DateTime.Now;
                                    //        _ClgAdmissionContext.Adm_College_Student_Attendance_PeriodwiseDMO.Update(resulttt);

                                    //    }
                                    //    if (data.CollegeMultiAttendanceEntryTempDTO[i].ACSAS_Id != null)
                                    //    {
                                    //        var result3 = _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO.Single(a => a.ACSA_Id == data.CollegeMultiAttendanceEntryTempDTO[i].ACSA_Id && a.AMCST_Id == data.CollegeMultiAttendanceEntryTempDTO[i].AMCST_Id);

                                    //        //Adm_College_Student_Attendance_StudentsDMO std = Mapper.Map<Adm_College_Student_Attendance_StudentsDMO>(data.CollegeMultiAttendanceEntryTempDTO[i]);


                                    //        if (data.CollegeMultiAttendanceEntryTempDTO[i].Selected == false)
                                    //        {
                                    //            result3.ACSAS_AttendanceFlag = "Present";
                                    //            result3.ACSAS_ClassAttended = 1;

                                    //        }
                                    //        else if (data.CollegeMultiAttendanceEntryTempDTO[i].Selected == true)
                                    //        {
                                    //            result3.ACSAS_AttendanceFlag = "Absent";
                                    //            result3.ACSAS_ClassAttended = 0;
                                    //        }

                                    //        result3.UpdatedDate = DateTime.Now;
                                    //        _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO.Update(result3);
                                    //    }
                                    //    else
                                    //    {
                                    //        Adm_College_Student_Attendance_StudentsDMO stdperiod = new Adm_College_Student_Attendance_StudentsDMO();

                                    //        stdperiod.ACSA_Id = check_duplicate.FirstOrDefault().ACSA_Id;
                                    //        stdperiod.AMCST_Id = Convert.ToInt64(data.CollegeMultiAttendanceEntryTempDTO[i].AMCST_Id);

                                    //        if (data.CollegeMultiAttendanceEntryTempDTO[i].Selected == true)
                                    //        {
                                    //            stdperiod.ACSAS_AttendanceFlag = "Absent";
                                    //            stdperiod.ACSAS_ClassAttended = 0;
                                    //        }
                                    //        else if (data.CollegeMultiAttendanceEntryTempDTO[i].Selected == false)
                                    //        {
                                    //            stdperiod.ACSAS_AttendanceFlag = "Present";
                                    //            stdperiod.ACSAS_ClassAttended = 1;
                                    //        }

                                    //        stdperiod.CreatedDate = DateTime.Now;
                                    //        stdperiod.UpdatedDate = DateTime.Now;
                                    //        _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO.Add(stdperiod);
                                    //    }
                                    //}
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
                                    int savedata = 0;
                                    for (int kk = 0; kk < data.ClgAttendanceEntryTempDTO.Count(); kk++)
                                    {
                                        if (data.AMB_Id == data.ClgAttendanceEntryTempDTO[kk].AMB_Id && data.ACMS_Id == data.ClgAttendanceEntryTempDTO[kk].ACMS_Id)
                                        {
                                            savedata = savedata + 1;

                                            if (knew == 0)
                                            {
                                                knew = knew + 1;

                                                enq.MI_Id = data.MI_Id;
                                                enq.ASMAY_Id = data.ASMAY_Id;
                                                enq.ACSA_Att_EntryType = "Present";
                                                enq.AMCO_Id = data.AMCO_Id;
                                                enq.AMB_Id = data.AMB_Id;
                                                enq.AMSE_Id = data.AMSE_Id;
                                                enq.ACMS_Id = data.ACMS_Id;
                                                enq.ISMS_Id = data.ISMS_Id;
                                                enq.ACSA_Entry = DateTime.Now;
                                                enq.ACSA_AttendanceDate = Convert.ToDateTime(data.ACSA_AttendanceDate);
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
                                            }

                                            for (int i = 0; i < data.ClgAttendanceEntryTempDTO[kk].sub_list.Count(); i++)
                                            {
                                                Adm_College_Student_Attendance_StudentsDMO std = new Adm_College_Student_Attendance_StudentsDMO();
                                                std.ACSA_Id = enq.ACSA_Id;
                                                if (data.TTMP_Id == data.ClgAttendanceEntryTempDTO[kk].sub_list[i].TTMP_Id)
                                                {
                                                    if (data.ClgAttendanceEntryTempDTO[kk].sub_list[i].Selected == true)
                                                    {
                                                        std.ACSAS_AttendanceFlag = "Present";
                                                        std.ACSAS_ClassAttended = 1;
                                                    }
                                                    else if (data.ClgAttendanceEntryTempDTO[kk].sub_list[i].Selected == false)
                                                    {
                                                        std.ACSAS_AttendanceFlag = "Absent";
                                                        std.ACSAS_ClassAttended = 0;
                                                    }
                                                    std.AMCST_Id = Convert.ToInt64(data.ClgAttendanceEntryTempDTO[kk].AMCST_Id);
                                                    std.CreatedDate = DateTime.Now;
                                                    std.UpdatedDate = DateTime.Now;
                                                    _ClgAdmissionContext.Adm_College_Student_Attendance_StudentsDMO.Add(std);
                                                }
                                            }
                                        }
                                    }

                                    if (savedata > 0)
                                    {
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
                    }
                }

                if (DateTime.Now.Date == data.ACSA_AttendanceDate.Value.Date)
                {
                    if (check_attendance_entrytype.FirstOrDefault().ASC_Att_Scheduler_Flag == "Periodwise")
                    {

                        List<CollegeAutoScheduler> obj1 = new List<CollegeAutoScheduler>();
                        List<CollegeAutoScheduler> check_period = new List<CollegeAutoScheduler>();

                        DateTime fromdatecon = Convert.ToDateTime(data.ACSA_AttendanceDate);
                        string confromdate = "";
                        try
                        {
                            fromdatecon = Convert.ToDateTime(fromdatecon.Date.ToString("yyyy-MM-dd"));
                            confromdate = fromdatecon.ToString("yyyy-MM-dd");
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "College_Attendace_PeriodWise_Scheduler_List";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                            cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMCO_Id) });
                            cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = Convert.ToString(branchids) });
                            cmd.Parameters.Add(new SqlParameter("@ACSA_AttendanceDate", SqlDbType.VarChar) { Value = confromdate });
                            cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = data.AMSE_Id });
                            cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = sectionids });
                            cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@TTMP_Id", SqlDbType.VarChar) { Value = periodids });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var resultc = cmd.ExecuteReader())
                                {
                                    _logatt.LogInformation("entered in dataReader block");
                                    while (resultc.Read())
                                    {
                                        _logatt.LogInformation("entered in while block");

                                        obj1.Add(new CollegeAutoScheduler
                                        {
                                            AMCST_MobileNo = Convert.ToInt64(resultc["AMCST_MobileNo"]),
                                            AMCST_Id = Convert.ToInt64(resultc["AMCST_Id"]),
                                            subjects = resultc["SUBJECTS"].ToString(),
                                            AMCST_FirstName = resultc["NAME"].ToString()
                                        });
                                    }
                                    check_period = obj1.ToList();
                                }
                            }
                            catch (Exception ex)
                            {
                                _logatt.LogInformation("error:'" + ex.Message + "'");
                                Console.Write(ex.Message);
                            }
                            try
                            {
                                if (check_period.Count() > 0)
                                {
                                    for (int i = 0; i < check_period.Count(); i++)
                                    {
                                        long MI_id = data.MI_Id;
                                        long mobileno = check_period[i].AMCST_MobileNo;
                                        long AMCST_Id = check_period[i].AMCST_Id;
                                        string subjectname = check_period[i].subjects;
                                        string name = check_period[i].AMCST_FirstName;
                                        try
                                        {
                                            string results = sendSms(MI_id, mobileno, "Attendance_Auto_Schedular_EOD", AMCST_Id, confromdate, data.ASMAY_Id, subjectname, name).Result;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public long getuserid(string data)
        {
            string username = data.ToString();
            //string id = "";
            var getuseridd = _ClgAdmissionContext.ApplUser.Where(a => a.UserName == data).FirstOrDefault().Id;
            return getuseridd;
        }

        // *************** Displaying The Saved Dates ************************************** //
        public CollegeMultiHoursAttendanceEntryDTO getsaveddatepreview(CollegeMultiHoursAttendanceEntryDTO data)
        {
            try
            {
                List<long> branchid = new List<long>();

                foreach (var c in data.selectedbranchlist)
                {
                    branchid.Add(c.AMB_Id);
                }

                string cours_idss = "0";
                string brnch_idss = "0";
                string semes_idss = "0";
                string scton_idss = "0";
                List<long> course_ids = new List<long>();
                List<long> branch_ids = new List<long>();
                List<long> semest_ids = new List<long>();
                List<long> section_ids = new List<long>();

                var userid = getuserid(data.username);

                var empcode_check = (from a in _ClgAdmissionContext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(userid))
                                     select new CollegeMultiHoursAttendanceEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                var Course_data = (from a in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                   from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                   from c in _ClgAdmissionContext.AcademicYear
                                   from d in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                                   where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.ISMS_Id == d.ISMS_Id && a.MI_Id == data.MI_Id
                                   && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.ASMAY_Id == data.ASMAY_Id && d.ISMS_ActiveFlag == 1
                                   && b.ACALD_ActiveFlag == true && branchid.Contains(b.AMB_Id) /*&& d.ISMS_Id == data.ISMS_Id*/)
                                   select new CollegeAttendanceEntryNew_DTO
                                   {
                                       AMCO_Id = b.AMCO_Id,
                                       //AMSE_Id = b.AMSE_Id,
                                   }).Distinct().ToList();

                var branch_data = (from a in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                   from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                   from c in _ClgAdmissionContext.AcademicYear
                                   from d in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                                   where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.ISMS_Id == d.ISMS_Id && a.MI_Id == data.MI_Id
                                   && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.ASMAY_Id == data.ASMAY_Id && d.ISMS_ActiveFlag == 1
                                   && b.ACALD_ActiveFlag == true && branchid.Contains(b.AMB_Id) /*&& d.ISMS_Id == data.ISMS_Id*/)
                                   select new CollegeAttendanceEntryNew_DTO
                                   {
                                       AMB_Id = b.AMB_Id,
                                       //AMSE_Id = b.AMSE_Id,
                                   }).Distinct().ToList();
                var semester_data = (from a in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                     from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                     from c in _ClgAdmissionContext.AcademicYear
                                     from d in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                                     where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.ISMS_Id == d.ISMS_Id && a.MI_Id == data.MI_Id
                                     && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.ASMAY_Id == data.ASMAY_Id && d.ISMS_ActiveFlag == 1
                                     && b.ACALD_ActiveFlag == true && branchid.Contains(b.AMB_Id) /*&& d.isms_id == data.isms_id*/)
                                     select new CollegeAttendanceEntryNew_DTO
                                     {
                                         //AMB_Id = b.AMB_Id,
                                         AMSE_Id = b.AMSE_Id,
                                     }).Distinct().ToList();
                var section_data = (from a in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                    from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                    from c in _ClgAdmissionContext.AcademicYear
                                    from d in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                                    where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.ISMS_Id == d.ISMS_Id && a.MI_Id == data.MI_Id
                                    && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.ASMAY_Id == data.ASMAY_Id && d.ISMS_ActiveFlag == 1
                                    && b.ACALD_ActiveFlag == true && branchid.Contains(b.AMB_Id) /*&& d.ISMS_Id == data.ISMS_Id*/)
                                    select new CollegeAttendanceEntryNew_DTO
                                    {
                                        ACMS_Id = b.ACMS_Id,
                                    }).Distinct().ToList();

                if (Course_data.Count > 0)
                {
                    foreach (var course in Course_data)
                    {
                        course_ids.Add(course.AMCO_Id);
                    }
                    for (int c = 0; c < course_ids.Count(); c++)
                    {
                        cours_idss = cours_idss + "," + course_ids[c].ToString();
                    }
                }
                if (branch_data.Count > 0)
                {
                    foreach (var branch in branch_data)
                    {
                        branch_ids.Add(branch.AMB_Id);
                    }
                    for (int b = 0; b < branch_ids.Count(); b++)
                    {
                        brnch_idss = brnch_idss + "," + branch_ids[b].ToString();
                    }
                }
                if (semester_data.Count > 0)
                {
                    foreach (var semester in semester_data)
                    {
                        semest_ids.Add(semester.AMSE_Id);
                    }
                    for (int s = 0; s < semest_ids.Count(); s++)
                    {
                        semes_idss = semes_idss + "," + semest_ids[s].ToString();
                    }
                }
                if (section_data.Count > 0)
                {
                    foreach (var section in section_data)
                    {
                        section_ids.Add(section.ACMS_Id);
                    }
                    for (int se = 0; se < section_ids.Count(); se++)
                    {
                        scton_idss = scton_idss + "," + section_ids[se].ToString();
                    }
                }
                List<CollegeMultiHoursAttendanceEntryDTO> obj1 = new List<CollegeMultiHoursAttendanceEntryDTO>();
                List<CollegeMultiHoursAttendanceEntryDTO> check_period = new List<CollegeMultiHoursAttendanceEntryDTO>();

                using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Attendance_preview_save_dates_All";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                    cmd.Parameters.Add(new SqlParameter("@amco_id", SqlDbType.VarChar) { Value = cours_idss });
                    cmd.Parameters.Add(new SqlParameter("@amb_id", SqlDbType.VarChar) { Value = brnch_idss });
                    cmd.Parameters.Add(new SqlParameter("@amse_id", SqlDbType.VarChar) { Value = semes_idss });
                    cmd.Parameters.Add(new SqlParameter("@acms_id", SqlDbType.VarChar) { Value = scton_idss });
                    cmd.Parameters.Add(new SqlParameter("@hrme_id", SqlDbType.VarChar) { Value = empcode_check.FirstOrDefault().Emp_Code });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var resultc = cmd.ExecuteReader())
                        {
                            _logatt.LogInformation("entered in dataReader block");
                            while (resultc.Read())
                            {
                                _logatt.LogInformation("entered in while block");

                                obj1.Add(new CollegeMultiHoursAttendanceEntryDTO
                                {
                                    ISMS_SubjectName = (resultc["ISMS_SubjectName"]).ToString(),
                                    TTMP_PeriodName = resultc["TTMP_PeriodName"].ToString(),
                                    ACSA_AttendanceDate = Convert.ToDateTime(resultc["ACSA_AttendanceDate"]),
                                    TotalPresent = (resultc["TotalPresent"]).ToString(),
                                    totalabsent = resultc["totalabsent"].ToString(),
                                    totalcount = (resultc["totalcount"]).ToString(),
                                    ACSA_Regular_Extra = (resultc["ACSA_Regular_Extra"]).ToString(),
                                    AMCO_CourseName = (resultc["AMCO_CourseName"]).ToString(),
                                    AMB_BranchName = (resultc["AMB_BranchName"]).ToString(),
                                    AMSE_SEMName = (resultc["AMSE_SEMName"]).ToString(),
                                    ACMS_SectionName = (resultc["ACMS_SectionName"]).ToString(),
                                    ACSA_Id = Convert.ToInt64((resultc["ACSA_Id"]).ToString()),
                                    AMCO_Id = Convert.ToInt64((resultc["AMCO_Id"]).ToString()),
                                    AMB_Id = Convert.ToInt64((resultc["AMB_Id"]).ToString()),
                                    AMSE_Id = Convert.ToInt64((resultc["AMSE_Id"]).ToString()),
                                    ACMS_Id = Convert.ToInt64((resultc["ACMS_Id"]).ToString()),
                                    employeename = (resultc["employeename"]).ToString(),
                                    flagdelete = Convert.ToInt64((resultc["flag"]).ToString()),
                                });
                            }
                            check_period = obj1.ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logatt.LogInformation("error:'" + ex.Message + "'");
                        Console.Write(ex.Message);
                    }
                }
                data.getpreviewdata = check_period.ToArray();               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<string> sendSms(long MI_Id, long mobileNo, string Template, long UserID, string date, long ASMAY_Id, string subjectname, string name)
        {
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {
                    result = result.Replace("[NAME]", name);
                    result = result.Replace("[SUBJECTS]", subjectname);
                    result = result.Replace("[DATE]", date);
                    sms = result;
                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entity_id", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("template_id", template.FirstOrDefault().ISES_TemplateId);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
    }
}
