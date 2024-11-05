using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using DomainModel.Model.com.vapstech.College.Exam;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Services
{
    public class CollegeMarksEntryImpl : Interfaces.CollegeMarksEntryInterface
    {
        private static ConcurrentDictionary<string, CollegeMarksEntryDTO> _login = new ConcurrentDictionary<string, CollegeMarksEntryDTO>();
        public ClgExamContext _examcontext;
        public DomainModelMsSqlServerContext _db;
        readonly ILogger<CollegeMarksEntryImpl> _logger;
        public CollegeMarksEntryImpl(ClgExamContext ttcategory, ILogger<CollegeMarksEntryImpl> log, DomainModelMsSqlServerContext db)
        {
            _examcontext = ttcategory;
            _logger = log;
            _db = db;
        }
        public CollegeMarksEntryDTO getdetails(CollegeMarksEntryDTO TTMC)
        {
            try
            {
                TTMC.getyear = _examcontext.AcademicYear.Where(a => a.MI_Id == TTMC.MI_Id && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();


                var check_rolename = (from a in _examcontext.MasterRoleType
                                      where (a.IVRMRT_Id == TTMC.roleId)
                                      select new CollegeMarksEntryDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var empcode_check = (from a in _examcontext.Staff_User_Login
                                     where (a.MI_Id == TTMC.MI_Id && a.IVRMSTAUL_UserName.Equals(TTMC.username.Trim()))
                                     select new CollegeMarksEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count > 0)
                {
                    TTMC.courseslist = (from a in _examcontext.Adm_College_Atten_Login_UserDMO
                                        from b in _examcontext.Adm_College_Atten_Login_DetailsDMO
                                        from c in _examcontext.AcademicYear
                                        from d in _examcontext.MasterCourseDMO
                                        where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMCO_Id == d.AMCO_Id && a.MI_Id == TTMC.MI_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && d.AMCO_ActiveFlag == true && b.ACALD_ActiveFlag == true)
                                        select d).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                }
                else
                {
                    TTMC.message = "For This Staff There Is No Privileges To Enter Attendance.. Please Contact Administrator";
                }
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }
        public CollegeMarksEntryDTO onchangeyear(CollegeMarksEntryDTO TTMC)
        {
            try
            {
                var check_rolename = (from a in _examcontext.MasterRoleType
                                      where (a.IVRMRT_Id == TTMC.roleId)
                                      select new CollegeMarksEntryDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var empcode_check = (from a in _examcontext.Staff_User_Login
                                     where (a.MI_Id == TTMC.MI_Id && a.IVRMSTAUL_UserName.Equals(TTMC.username.Trim()))
                                     select new CollegeMarksEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count > 0)
                {
                    TTMC.courseslist = (from a in _examcontext.Adm_College_Atten_Login_UserDMO
                                        from b in _examcontext.Adm_College_Atten_Login_DetailsDMO
                                        from c in _examcontext.AcademicYear
                                        from d in _examcontext.MasterCourseDMO
                                        where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMCO_Id == d.AMCO_Id
                                        && a.MI_Id == TTMC.MI_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && d.AMCO_ActiveFlag == true
                                        && b.ACALD_ActiveFlag == true && a.ASMAY_Id == TTMC.ASMAY_Id)
                                        select d).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                }
                else
                {
                    TTMC.message = "For This Staff There Is No Privileges To Enter Attendance.. Please Contact Administrator";
                }

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }
        public CollegeMarksEntryDTO onchangecourse(CollegeMarksEntryDTO data)
        {
            try
            {
                var empcode_check = (from a in _examcontext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                                     select new CollegeMarksEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                data.branchlist = (from a in _examcontext.ClgMasterBranchDMO
                                   from d in _examcontext.AcademicYear
                                   from e in _examcontext.Adm_College_Atten_Login_DetailsDMO
                                   from f in _examcontext.Adm_College_Atten_Login_UserDMO
                                   where (a.AMB_Id == e.AMB_Id && f.ASMAY_Id == d.ASMAY_Id && e.ACALU_Id == f.ACALU_Id && a.MI_Id == data.MI_Id && e.AMCO_Id == data.AMCO_Id && f.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && f.ASMAY_Id == data.ASMAY_Id && e.ACALD_ActiveFlag == true)
                                   select a).Distinct().OrderBy(a => a.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeMarksEntryDTO onchangebranch(CollegeMarksEntryDTO data)
        {
            try
            {
                var empcode_check = (from a in _examcontext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                                     select new CollegeMarksEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                data.semisters = (from a in _examcontext.CLG_Adm_Master_SemesterDMO
                                  from c in _examcontext.AcademicYear
                                  from d in _examcontext.Adm_College_Atten_Login_DetailsDMO
                                  from e in _examcontext.Adm_College_Atten_Login_UserDMO
                                  where (d.AMSE_Id == a.AMSE_Id && e.ACALU_Id == d.ACALU_Id && a.MI_Id == data.MI_Id
                                  && e.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && d.AMB_Id == data.AMB_Id && d.AMCO_Id == data.AMCO_Id && e.ASMAY_Id == data.ASMAY_Id && d.ACALD_ActiveFlag == true)
                                  select a).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeMarksEntryDTO get_exams(CollegeMarksEntryDTO data)
        {
            CollegeMarksEntryDTO TTMC = new CollegeMarksEntryDTO();
            try
            {
                var empcode_check = (from a in _examcontext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                                     select new CollegeMarksEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                TTMC.examlist = (from s in _examcontext.col_exammasterDMO
                                 from k in _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO
                                 where (s.EME_Id == k.EME_Id && k.AMCO_Id == data.AMCO_Id && k.AMB_Id == data.AMB_Id && s.MI_Id == data.MI_Id
                                 && k.AMSE_Id == data.AMSE_Id && k.ECYSE_ActiveFlg == true)
                                 select new CollegeMarksEntryDTO
                                 {
                                     EME_Id = s.EME_Id,
                                     EME_ExamName = s.EME_ExamName,
                                     EME_ExamCode = s.EME_ExamCode,
                                     EME_ExamOrder = s.EME_ExamOrder
                                 }).Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();

                TTMC.sectionlist = (from a in _examcontext.Adm_College_Master_SectionDMO
                                    from b in _examcontext.Adm_College_Atten_Login_DetailsDMO
                                    from f in _examcontext.Adm_College_Atten_Login_UserDMO
                                    from g in _examcontext.AcademicYear
                                    where (a.ACMS_Id == b.ACMS_Id && b.AMB_Id == b.AMB_Id && b.AMCO_Id == b.AMCO_Id && b.AMSE_Id == b.AMSE_Id
                                    && f.ASMAY_Id == g.ASMAY_Id && b.AMB_Id == data.AMB_Id && b.AMCO_Id == data.AMCO_Id && b.ACALU_Id == f.ACALU_Id
                                    && b.AMSE_Id == data.AMSE_Id && a.MI_Id == data.MI_Id && b.ACALD_ActiveFlag == true
                                    && f.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && f.ASMAY_Id == data.ASMAY_Id)
                                    select a).Distinct().OrderBy(a => a.ACMS_Order).ToArray();

                TTMC.subjectgrplist = (from a in _examcontext.Exm_Col_Yearly_SchemeDMO
                                       from b in _examcontext.Exm_Col_Yearly_Scheme_GroupDMO
                                       from c in _examcontext.Exm_Master_GroupDMO
                                       where (a.ECYS_Id == b.ECYS_Id && b.EMG_Id == c.EMG_Id && a.AMB_Id == data.AMB_Id && a.AMCO_Id == data.AMCO_Id
                                       && a.AMSE_Id == data.AMSE_Id && a.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && a.ECYS_ActiveFlag == true
                                       && b.ECYSG_ActiveFlag == true && c.EMG_ActiveFlag == true)
                                       select c).Distinct().OrderBy(a => a.EMG_GroupName).ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }
        public CollegeMarksEntryDTO get_subjects(CollegeMarksEntryDTO data)
        {
            try
            {
                var empcode_check = (from a in _examcontext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                                     select new CollegeMarksEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                data.subjectgroups = (from a in _examcontext.IVRM_School_Master_SubjectsDMO
                                      from b in _examcontext.ClgMasterBranchDMO
                                      from c in _examcontext.MasterCourseDMO
                                      from d in _examcontext.CLG_Adm_Master_SemesterDMO
                                      from e in _examcontext.Adm_College_Atten_Login_DetailsDMO
                                      from f in _examcontext.Adm_College_Atten_Login_UserDMO
                                      from g in _examcontext.AcademicYear
                                      from h in _examcontext.Adm_College_Master_SectionDMO
                                      where (a.ISMS_Id == e.ISMS_Id && b.AMB_Id == e.AMB_Id && c.AMCO_Id == e.AMCO_Id && e.ACMS_Id == h.ACMS_Id
                                      && f.ASMAY_Id == g.ASMAY_Id && e.ACALU_Id == f.ACALU_Id && e.AMB_Id == data.AMB_Id && e.AMCO_Id == data.AMCO_Id
                                      && e.ACMS_Id == data.ACMS_Id && e.AMSE_Id == data.AMSE_Id && a.MI_Id == data.MI_Id
                                      && f.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && f.ASMAY_Id == data.ASMAY_Id && e.ACALD_ActiveFlag == true)
                                      select a).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeMarksEntryDTO getsubjectscheme(CollegeMarksEntryDTO data)
        {
            try
            {
                var empcode_check = (from a in _examcontext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                                     select new CollegeMarksEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                data.getsubjectschemetype = (from a in _examcontext.AdmCollegeSubjectSchemeDMO
                                             from b in _examcontext.Exm_Col_Yearly_SchemeDMO
                                             from c in _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO
                                             from d in _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO
                                             from e in _examcontext.IVRM_School_Master_SubjectsDMO
                                             where (a.ACSS_Id == b.ACSS_Id && b.ECYS_Id == c.ECYS_Id && c.ECYSE_Id == d.ECYSE_Id && e.ISMS_Id == d.ISMS_Id
                                             && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id && b.ECYS_ActiveFlag == true
                                             && c.ECYSE_ActiveFlg == true && d.ECYSES_ActiveFlg == true && e.ISMS_ActiveFlag == 1 && d.ISMS_Id == data.ISMS_Id
                                             && c.EME_Id == data.EME_Id)
                                             select a).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeMarksEntryDTO getsubjectschemetype(CollegeMarksEntryDTO data)
        {
            try
            {
                var empcode_check = (from a in _examcontext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                                     select new CollegeMarksEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();
                data.getschemetype = (from a in _examcontext.AdmCollegeSubjectSchemeDMO
                                      from b in _examcontext.Exm_Col_Yearly_SchemeDMO
                                      from c in _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO
                                      from d in _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO
                                      from e in _examcontext.IVRM_School_Master_SubjectsDMO
                                      from f in _examcontext.AdmCollegeSchemeTypeDMO
                                      where (a.ACSS_Id == b.ACSS_Id && b.ACST_Id == f.ACST_Id && b.ECYS_Id == c.ECYS_Id && c.ECYSE_Id == d.ECYSE_Id
                                      && e.ISMS_Id == d.ISMS_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id
                                      && b.ECYS_ActiveFlag == true && c.ECYSE_ActiveFlg == true && d.ECYSES_ActiveFlg == true && e.ISMS_ActiveFlag == 1
                                      && d.ISMS_Id == data.ISMS_Id && c.EME_Id == data.EME_Id && b.ACSS_Id == data.ACSS_Id)
                                      select f).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<CollegeMarksEntryDTO> onsearch(CollegeMarksEntryDTO data)
        {
            CollegeMarksEntryDTO EM = new CollegeMarksEntryDTO();
            try
            {
                string order = "AMCST_FirstName";
                var get_configuration = _examcontext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                data.configuration = get_configuration.ToArray();

                if (get_configuration != null && get_configuration.Count > 0)
                {
                    if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                    {
                        order = "AMCST_FirstName";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "AdmNo")
                    {
                        order = "AMCST_AdmNo";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RollNo")
                    {
                        order = "ACYST_RollNo";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RegNo")
                    {
                        order = "AMCST_RegistrationNo";
                    }
                    else
                    {
                        order = "AMCST_FirstName";
                    }
                }

                var subject_details = (from a in _examcontext.Exm_Col_Yearly_SchemeDMO
                                       from b in _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO
                                       from c in _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO
                                       from d in _examcontext.IVRM_School_Master_SubjectsDMO
                                       where (a.ECYS_Id == b.ECYS_Id && b.ECYSE_Id == c.ECYSE_Id && d.ISMS_Id == c.ISMS_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id
                                       && a.AMSE_Id == data.AMSE_Id && a.ACSS_Id == data.ACSS_Id && a.ACST_Id == data.ACST_Id && a.ECYS_ActiveFlag == true && b.AMCO_Id == data.AMCO_Id
                                       && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id && b.ACSS_Id == data.ACSS_Id && b.ACST_Id == data.ACST_Id && b.ECYSE_ActiveFlg == true
                                       && b.EME_Id == data.EME_Id && c.ISMS_Id == data.ISMS_Id && c.ECYSES_ActiveFlg == true && d.ISMS_ActiveFlag == 1 && d.ISMS_Id == data.ISMS_Id)
                                       select c).Distinct().ToList();

                data.subject_details = subject_details.ToArray();
                data.ECYSES_MarksGradeEntryFlg = subject_details[0].ECYSES_MarksGradeEntryFlg;

                data.ECYSES_SubSubjectFlg = subject_details[0].ECYSES_SubSubjectFlg;
                data.ECYSES_SubExamFlg = subject_details[0].ECYSES_SubExamFlg;

                if (data.ECYSES_SubSubjectFlg && !data.ECYSES_SubExamFlg)
                {
                    if (data.message == "Mobile")
                    {
                        data.subject_subsubjects = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                                    from b in _examcontext.clg_mastersubsubject
                                                    where (a.EMSS_Id == b.EMSS_Id && b.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.EMSS_ActiveFlag == true
                                                    && a.ECYSES_Id == subject_details[0].ECYSES_Id && b.EMSS_Id == a.EMSS_Id && a.EMSS_Id == data.EMSS_Id && b.EMSS_Id == data.EMSS_Id)
                                                    select new ClgSubjectWizardDTO
                                                    {
                                                        ECYSESSS_Id = a.ECYSESSS_Id,
                                                        ECYSES_Id = a.ECYSES_Id,
                                                        EMSE_Id = a.EMSE_Id,
                                                        EMSS_Id = a.EMSS_Id,
                                                        EMGR_Id = a.EMGR_Id,
                                                        ECYSESSS_MaxMarks = a.ECYSESSS_MaxMarks,
                                                        ECYSESSS_MinMarks = a.ECYSESSS_MinMarks,
                                                        ECYSESSS_ExemptedFlg = a.ECYSESSS_ExemptedFlg,
                                                        ECYSESSS_ExemptedPer = a.ECYSESSS_ExemptedPer,
                                                        ECYSESSS_ActiveFlg = a.ECYSESSS_ActiveFlg,
                                                        ECYSESSS_SubSubjectOrder = a.ECYSESSS_SubSubjectOrder,
                                                        EMSS_SubSubjectName = b.EMSS_SubSubjectName,
                                                        EMSS_SubSubjectCode = b.EMSS_SubSubjectCode

                                                    }).Distinct().OrderBy(a => a.ECYSESSS_SubSubjectOrder).ToArray();
                    }
                    else
                    {
                        data.subject_subsubjects = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                                    from b in _examcontext.clg_mastersubsubject
                                                    where (a.EMSS_Id == b.EMSS_Id && b.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.EMSS_ActiveFlag == true
                                                    && a.ECYSES_Id == subject_details[0].ECYSES_Id && b.EMSS_Id == a.EMSS_Id)
                                                    select new ClgSubjectWizardDTO
                                                    {
                                                        ECYSESSS_Id = a.ECYSESSS_Id,
                                                        ECYSES_Id = a.ECYSES_Id,
                                                        EMSE_Id = a.EMSE_Id,
                                                        EMSS_Id = a.EMSS_Id,
                                                        EMGR_Id = a.EMGR_Id,
                                                        ECYSESSS_MaxMarks = a.ECYSESSS_MaxMarks,
                                                        ECYSESSS_MinMarks = a.ECYSESSS_MinMarks,
                                                        ECYSESSS_ExemptedFlg = a.ECYSESSS_ExemptedFlg,
                                                        ECYSESSS_ExemptedPer = a.ECYSESSS_ExemptedPer,
                                                        ECYSESSS_ActiveFlg = a.ECYSESSS_ActiveFlg,
                                                        ECYSESSS_SubSubjectOrder = a.ECYSESSS_SubSubjectOrder,
                                                        EMSS_SubSubjectName = b.EMSS_SubSubjectName,
                                                        EMSS_SubSubjectCode = b.EMSS_SubSubjectCode

                                                    }).Distinct().OrderBy(a => a.ECYSESSS_SubSubjectOrder).ToArray();
                    }

                }
                else if (!data.ECYSES_SubSubjectFlg && data.ECYSES_SubExamFlg)
                {
                    if (data.message == "Mobile")
                    {
                        data.subject_subexams = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                                 from b in _examcontext.clg_mastersubexam
                                                 where (b.EMSE_Id == a.EMSE_Id && b.MI_Id == data.MI_Id && b.EMSE_ActiveFlag == true
                                                 && a.ECYSES_Id == subject_details[0].ECYSES_Id && a.EMSE_Id==data.EMSE_Id &&  b.EMSE_Id== data.EMSE_Id)
                                                 select new ClgSubjectWizardDTO
                                                 {
                                                     ECYSESSS_Id = a.ECYSESSS_Id,
                                                     ECYSES_Id = a.ECYSES_Id,
                                                     EMSE_Id = a.EMSE_Id,
                                                     EMSS_Id = a.EMSS_Id,
                                                     EMGR_Id = a.EMGR_Id,
                                                     ECYSESSS_MaxMarks = a.ECYSESSS_MaxMarks,
                                                     ECYSESSS_MinMarks = a.ECYSESSS_MinMarks,
                                                     ECYSESSS_ExemptedFlg = a.ECYSESSS_ExemptedFlg,
                                                     ECYSESSS_ExemptedPer = a.ECYSESSS_ExemptedPer,
                                                     ECYSESSS_ActiveFlg = a.ECYSESSS_ActiveFlg,
                                                     ECYSESSS_SubSubjectOrder = a.ECYSESSS_SubSubjectOrder,
                                                     EMSE_SubExamName = b.EMSE_SubExamName,
                                                     EMSE_SubExamCode = b.EMSE_SubExamCode

                                                 }).Distinct().OrderBy(a => a.ECYSESSS_SubSubjectOrder).ToArray();
                    }
                    else
                    {
                        data.subject_subexams = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                                 from b in _examcontext.clg_mastersubexam
                                                 where (b.EMSE_Id == a.EMSE_Id && b.MI_Id == data.MI_Id && b.EMSE_ActiveFlag == true
                                                 && a.ECYSES_Id == subject_details[0].ECYSES_Id)
                                                 select new ClgSubjectWizardDTO
                                                 {
                                                     ECYSESSS_Id = a.ECYSESSS_Id,
                                                     ECYSES_Id = a.ECYSES_Id,
                                                     EMSE_Id = a.EMSE_Id,
                                                     EMSS_Id = a.EMSS_Id,
                                                     EMGR_Id = a.EMGR_Id,
                                                     ECYSESSS_MaxMarks = a.ECYSESSS_MaxMarks,
                                                     ECYSESSS_MinMarks = a.ECYSESSS_MinMarks,
                                                     ECYSESSS_ExemptedFlg = a.ECYSESSS_ExemptedFlg,
                                                     ECYSESSS_ExemptedPer = a.ECYSESSS_ExemptedPer,
                                                     ECYSESSS_ActiveFlg = a.ECYSESSS_ActiveFlg,
                                                     ECYSESSS_SubSubjectOrder = a.ECYSESSS_SubSubjectOrder,
                                                     EMSE_SubExamName = b.EMSE_SubExamName,
                                                     EMSE_SubExamCode = b.EMSE_SubExamCode

                                                 }).Distinct().OrderBy(a => a.ECYSESSS_SubSubjectOrder).ToArray();
                    }
                 
                }
                else
                {
                    data.subject_subexams = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                             from b in _examcontext.clg_mastersubsubject
                                             from c in _examcontext.clg_mastersubexam
                                             where (a.EMSS_Id == b.EMSS_Id && a.EMSE_Id == c.EMSE_Id
                                             && b.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.EMSS_ActiveFlag == true && c.EMSE_ActiveFlag == true
                                             && a.ECYSES_Id == subject_details[0].ECYSES_Id && b.EMSS_Id == a.EMSS_Id)
                                             select new ClgSubjectWizardDTO
                                             {
                                                 ECYSESSS_Id = a.ECYSESSS_Id,
                                                 ECYSES_Id = a.ECYSES_Id,
                                                 EMSE_Id = a.EMSE_Id,
                                                 EMSS_Id = a.EMSS_Id,
                                                 EMGR_Id = a.EMGR_Id,
                                                 ECYSESSS_MaxMarks = a.ECYSESSS_MaxMarks,
                                                 ECYSESSS_MinMarks = a.ECYSESSS_MinMarks,
                                                 ECYSESSS_ExemptedFlg = a.ECYSESSS_ExemptedFlg,
                                                 ECYSESSS_ExemptedPer = a.ECYSESSS_ExemptedPer,
                                                 ECYSESSS_ActiveFlg = a.ECYSESSS_ActiveFlg,
                                                 ECYSESSS_SubSubjectOrder = a.ECYSESSS_SubSubjectOrder,
                                                 EMSE_SubExamName = c.EMSE_SubExamName,
                                                 EMSE_SubExamCode = c.EMSE_SubExamCode

                                             }).Distinct().OrderBy(a => a.ECYSESSS_SubSubjectOrder).ToArray();


                    data.subject_subsubjects = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                                from b in _examcontext.clg_mastersubsubject
                                                from c in _examcontext.clg_mastersubexam
                                                where (a.EMSS_Id == b.EMSS_Id && a.EMSE_Id == c.EMSE_Id
                                                && b.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.EMSS_ActiveFlag == true && c.EMSE_ActiveFlag == true
                                                && a.ECYSES_Id == subject_details[0].ECYSES_Id && b.EMSS_Id == a.EMSS_Id)
                                                select new ClgSubjectWizardDTO
                                                {
                                                    ECYSESSS_Id = a.ECYSESSS_Id,
                                                    ECYSES_Id = a.ECYSES_Id,
                                                    EMSE_Id = a.EMSE_Id,
                                                    EMSS_Id = a.EMSS_Id,
                                                    EMGR_Id = a.EMGR_Id,
                                                    ECYSESSS_MaxMarks = a.ECYSESSS_MaxMarks,
                                                    ECYSESSS_MinMarks = a.ECYSESSS_MinMarks,
                                                    ECYSESSS_ExemptedFlg = a.ECYSESSS_ExemptedFlg,
                                                    ECYSESSS_ExemptedPer = a.ECYSESSS_ExemptedPer,
                                                    ECYSESSS_ActiveFlg = a.ECYSESSS_ActiveFlg,
                                                    ECYSESSS_SubSubjectOrder = a.ECYSESSS_SubSubjectOrder,
                                                    EMSE_SubExamName = c.EMSE_SubExamName,
                                                    EMSE_SubExamCode = c.EMSE_SubExamCode

                                                }).Distinct().OrderBy(a => a.ECYSESSS_SubSubjectOrder).ToArray();


                    data.subsubjectlist = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                           from b in _examcontext.clg_mastersubsubject
                                           where (a.EMSS_Id == b.EMSS_Id && b.MI_Id == data.MI_Id && a.ECYSESSS_ActiveFlg == true
                                           && a.ECYSES_Id == subject_details[0].ECYSES_Id && b.EMSS_Id == a.EMSS_Id)
                                           select b).Distinct().OrderBy(a => a.EMSS_Order).ToArray();

                    data.subexamlist = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                        from b in _examcontext.clg_mastersubexam
                                        from c in _examcontext.clg_mastersubsubject
                                        where (a.EMSS_Id == c.EMSS_Id && b.EMSE_Id == a.EMSE_Id && b.MI_Id == data.MI_Id
                                        && b.EMSE_ActiveFlag == true && a.ECYSESSS_ActiveFlg == true && a.ECYSES_Id == subject_details[0].ECYSES_Id)
                                        select b).Distinct().OrderBy(a => a.EMSE_SubExamOrder).ToArray();
                }

                data.EMGR_Id = subject_details[0].EMGR_Id;


                data.EMGR_Id = subject_details[0].EMGR_Id;

                if (data.ECYSES_MarksGradeEntryFlg == "G")
                {
                    data.grade_details = (from a in _examcontext.Exm_Master_GradeDMO
                                          from b in _examcontext.Exm_Master_Grade_DetailsDMO
                                          where (a.MI_Id == data.MI_Id && a.EMGR_Id == data.EMGR_Id && b.EMGR_Id == data.EMGR_Id && b.EMGD_ActiveFlag == true)
                                          select b).Select(t => t.EMGD_Name).Distinct().ToArray();

                    if (data.ECYSES_SubSubjectFlg)
                    {
                        data.subsubject_gradedetails = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                                        from b in _examcontext.Exm_Master_GradeDMO
                                                        from c in _examcontext.Exm_Master_Grade_DetailsDMO
                                                        where (a.ECYSES_Id == subject_details[0].ECYSES_Id && a.EMGR_Id == b.EMGR_Id && b.MI_Id == data.MI_Id
                                                        && b.EMGR_ActiveFlag == true && c.EMGR_Id == b.EMGR_Id && c.EMGD_ActiveFlag == true)
                                                        select c).Distinct().ToArray();
                    }

                    if (data.ECYSES_SubExamFlg)
                    {
                        data.subexam_gradedetails = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                                     from b in _examcontext.Exm_Master_GradeDMO
                                                     from c in _examcontext.Exm_Master_Grade_DetailsDMO
                                                     where (a.ECYSES_Id == subject_details[0].ECYSES_Id && a.EMGR_Id == b.EMGR_Id && b.MI_Id == data.MI_Id
                                                     && b.EMGR_ActiveFlag == true && c.EMGR_Id == b.EMGR_Id && c.EMGD_ActiveFlag == true)
                                                     select c).Distinct().ToArray();
                    }
                }

                var alrdy_stu_count = _examcontext.Exm_Col_Student_MarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                && t.AMCO_ID == data.AMCO_Id && t.AMB_ID == data.AMB_Id && t.AMSE_ID == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id && t.EME_Id == data.EME_Id
                && t.ISMS_Id == data.ISMS_Id).ToList().Count();

                var stu_list_mapped = _examcontext.Exm_Col_Studentwise_SubjectsDMO.Where(k => k.MI_Id == data.MI_Id && k.ASMAY_Id == data.ASMAY_Id && k.AMCO_Id == data.AMCO_Id
                && k.AMB_Id == data.AMB_Id && k.AMSE_Id == data.AMSE_Id && k.ASMAY_Id == data.ASMAY_Id && k.ISMS_Id == data.ISMS_Id && k.ECSTSU_ActiveFlg == true).Select(t => t.AMCST_Id).Distinct().ToList();

                List<College_temp_marks_DTO> studentList = new List<College_temp_marks_DTO>();

                studentList = (from e in _examcontext.Adm_Master_College_StudentDMO
                               from f in _examcontext.Adm_College_Yearly_StudentDMO
                               from g in _examcontext.IVRM_School_Master_SubjectsDMO
                               from h in _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO
                               from i in subject_details
                               where (e.AMCST_Id == f.AMCST_Id && f.AMCO_Id == data.AMCO_Id && f.AMB_Id == data.AMB_Id && f.AMSE_Id == data.AMSE_Id && f.ASMAY_Id == data.ASMAY_Id
                               && e.MI_Id == data.MI_Id && e.AMCST_ActiveFlag == true && e.AMCST_SOL == "S" && f.ACYST_ActiveFlag == 1 && g.ISMS_Id == h.ISMS_Id
                               && g.MI_Id == data.MI_Id && g.ISMS_ActiveFlag == 1 && g.ISMS_ExamFlag == 1 && g.ISMS_Id == data.ISMS_Id && g.ISMS_Id == data.ISMS_Id
                               && h.ECYSE_Id == i.ECYSE_Id)

                               select new College_temp_marks_DTO//MarksEntryHHSDTO
                               {
                                   AMCST_Id = e.AMCST_Id,
                                   AMCST_FirstName = ((e.AMCST_FirstName == null ? "" : e.AMCST_FirstName) +
                                   (e.AMCST_MiddleName == null || e.AMCST_MiddleName == "" ? "" : " " + e.AMCST_MiddleName) +
                                   (e.AMCST_LastName == null || e.AMCST_LastName == "" ? "" : " " + e.AMCST_LastName)).Trim(),
                                   AMCST_AdmNo = e.AMCST_AdmNo == null ? "" : e.AMCST_AdmNo,
                                   AMCST_RegistrationNo = e.AMCST_RegistrationNo == null ? "" : e.AMCST_RegistrationNo,
                                   ACYST_RollNo = f.ACYST_RollNo,
                                   ISMS_Id = g.ISMS_Id,
                                   ISMS_SubjectName = g.ISMS_SubjectName,
                                   ECYSES_MaxMarks = h.ECYSES_MaxMarks,
                                   ECYSES_MinMarks = h.ECYSES_MinMarks,
                                   ECYSES_MarksEntryMax = h.ECYSES_MarksEntryMax,
                               }).Distinct().OrderBy(t => order).ToList();

                var propertyInfo = typeof(College_temp_marks_DTO).GetProperty(order);
                studentList = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();

                data.studentList = studentList.Where(t => stu_list_mapped.Contains(t.AMCST_Id)).Distinct().ToArray();

                if (alrdy_stu_count > 0 && !data.ECYSES_SubSubjectFlg && !data.ECYSES_SubExamFlg)
                {
                    var stu_marks = _examcontext.Exm_Col_Student_MarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                    && t.AMCO_ID == data.AMCO_Id && t.AMB_ID == data.AMB_Id && t.AMSE_ID == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id && t.EME_Id == data.EME_Id
                    && t.ISMS_Id == data.ISMS_Id).Distinct().ToList();

                    List<College_temp_marks_DTO> saved_studentList = new List<College_temp_marks_DTO>();

                    saved_studentList = (from a in studentList
                                         from b in stu_marks
                                         where (a.AMCST_Id == b.AMCST_Id && b.ISMS_Id == a.ISMS_Id && stu_list_mapped.Contains(a.AMCST_Id))
                                         select new College_temp_marks_DTO
                                         {
                                             AMCST_Id = a.AMCST_Id,
                                             AMCST_FirstName = a.AMCST_FirstName,
                                             AMCST_AdmNo = a.AMCST_AdmNo,
                                             AMCST_RegistrationNo = a.AMCST_RegistrationNo == null ? "" : a.AMCST_RegistrationNo,
                                             ACYST_RollNo = a.ACYST_RollNo,
                                             ISMS_Id = a.ISMS_Id,
                                             ISMS_SubjectName = a.ISMS_SubjectName,
                                             ECYSES_MaxMarks = a.ECYSES_MaxMarks,
                                             ECYSES_MarksEntryMax = a.ECYSES_MarksEntryMax,
                                             ECYSES_MinMarks = a.ECYSES_MinMarks,
                                             ECSTM_Marks = Convert.ToDecimal(b.ECSTM_Marks),
                                             ECSTM_Grade = b.ECSTM_Grade,
                                             ECSTM_Flg = b.ECSTM_Flg,
                                             ECSTM_Id = b.ECSTM_Id
                                         }).Distinct().OrderBy(t => order).ToList();

                    var propertyInfo1 = typeof(College_temp_marks_DTO).GetProperty(order);
                    saved_studentList = saved_studentList.OrderBy(x => propertyInfo1.GetValue(x, null)).ToList();

                    data.saved_studentList = saved_studentList.Distinct().ToArray();
                }

                else if (alrdy_stu_count > 0 && data.ECYSES_SubSubjectFlg && data.ECYSES_SubExamFlg)
                {
                    var stu_marks = _examcontext.Exm_Col_Student_MarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                 && t.AMCO_ID == data.AMCO_Id && t.AMB_ID == data.AMB_Id && t.AMSE_ID == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id && t.EME_Id == data.EME_Id
                 && t.ISMS_Id == data.ISMS_Id).Distinct().ToList();

                    List<College_temp_marks_DTO> saved_studentList = new List<College_temp_marks_DTO>();

                    saved_studentList = (from a in studentList
                                         from b in stu_marks
                                         where (a.AMCST_Id == b.AMCST_Id && b.ISMS_Id == a.ISMS_Id && stu_list_mapped.Contains(a.AMCST_Id))
                                         select new College_temp_marks_DTO
                                         {
                                             AMCST_Id = a.AMCST_Id,
                                             AMCST_FirstName = a.AMCST_FirstName,
                                             AMCST_AdmNo = a.AMCST_AdmNo,
                                             AMCST_RegistrationNo = a.AMCST_RegistrationNo == null ? "" : a.AMCST_RegistrationNo,
                                             ACYST_RollNo = a.ACYST_RollNo,
                                             ISMS_Id = a.ISMS_Id,
                                             ISMS_SubjectName = a.ISMS_SubjectName,
                                             ECYSES_MaxMarks = a.ECYSES_MaxMarks,
                                             ECYSES_MarksEntryMax = a.ECYSES_MarksEntryMax,
                                             ECYSES_MinMarks = a.ECYSES_MinMarks,
                                             ECSTM_Marks = Convert.ToDecimal(b.ECSTM_Marks),
                                             ECSTM_Grade = b.ECSTM_Grade,
                                             ECSTM_Flg = b.ECSTM_Flg,
                                             ECSTM_Id = b.ECSTM_Id
                                         }).Distinct().OrderBy(t => order).ToList();

                    var propertyInfo2 = typeof(College_temp_marks_DTO).GetProperty(order);
                    saved_studentList = saved_studentList.OrderBy(x => propertyInfo2.GetValue(x, null)).ToList();

                    data.saved_studentList = saved_studentList.Distinct().ToArray();

                    data.saved_ssse_list = (from a in _examcontext.Exm_Col_Student_Marks_SubSubjectDMO
                                            from b in stu_marks
                                            where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ECSTM_Id == b.ECSTM_Id)
                                            select a).Distinct().ToArray();
                }
                //subsubject
                else if (alrdy_stu_count > 0 && data.ECYSES_SubSubjectFlg && !data.ECYSES_SubExamFlg)
                {
                    //subsubject
                    var stu_marks = _examcontext.Exm_Col_Student_MarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
          && t.AMCO_ID == data.AMCO_Id && t.AMB_ID == data.AMB_Id && t.AMSE_ID == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id && t.EME_Id == data.EME_Id
          && t.ISMS_Id == data.ISMS_Id).Distinct().ToList();

                    List<College_temp_marks_DTO> saved_studentList = new List<College_temp_marks_DTO>();

                    saved_studentList = (from a in studentList
                                         from b in stu_marks
                                         where (a.AMCST_Id == b.AMCST_Id && b.ISMS_Id == a.ISMS_Id && stu_list_mapped.Contains(a.AMCST_Id))
                                         select new College_temp_marks_DTO
                                         {
                                             AMCST_Id = a.AMCST_Id,
                                             AMCST_FirstName = a.AMCST_FirstName,
                                             AMCST_AdmNo = a.AMCST_AdmNo,
                                             AMCST_RegistrationNo = a.AMCST_RegistrationNo == null ? "" : a.AMCST_RegistrationNo,
                                             ACYST_RollNo = a.ACYST_RollNo,
                                             ISMS_Id = a.ISMS_Id,
                                             ISMS_SubjectName = a.ISMS_SubjectName,
                                             ECYSES_MaxMarks = a.ECYSES_MaxMarks,
                                             ECYSES_MarksEntryMax = a.ECYSES_MarksEntryMax,
                                             ECYSES_MinMarks = a.ECYSES_MinMarks,
                                             ECSTM_Marks = Convert.ToDecimal(b.ECSTM_Marks),
                                             ECSTM_Grade = b.ECSTM_Grade,
                                             ECSTM_Flg = b.ECSTM_Flg,
                                             ECSTM_Id = b.ECSTM_Id
                                         }).Distinct().OrderBy(t => order).ToList();

                    var propertyInfo3 = typeof(College_temp_marks_DTO).GetProperty(order);
                    saved_studentList = saved_studentList.OrderBy(x => propertyInfo3.GetValue(x, null)).ToList();

                    data.saved_studentList = saved_studentList.Distinct().ToArray();
                    //sanjevv
                    if (data.message == "Mobile")
                    {
                        data.saved_ss_list = (from a in _examcontext.Exm_Col_Student_Marks_SubSubjectDMO
                                              from b in stu_marks
                                              where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ECSTM_Id == b.ECSTM_Id && a.EMSS_Id==data.EMSS_Id)
                                              select a).Distinct().ToArray();
                    }
                    else
                    {
                        data.saved_ss_list = (from a in _examcontext.Exm_Col_Student_Marks_SubSubjectDMO
                                              from b in stu_marks
                                              where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ECSTM_Id == b.ECSTM_Id)
                                              select a).Distinct().ToArray();
                    }
                      
                }

                else if (alrdy_stu_count > 0 && data.ECYSES_SubExamFlg && !data.ECYSES_SubSubjectFlg)
                {
                    var stu_marks = _examcontext.Exm_Col_Student_MarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCO_ID == data.AMCO_Id
                    && t.AMB_ID == data.AMB_Id && t.AMSE_ID == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id).Distinct().ToList();

                    List<College_temp_marks_DTO> saved_studentList = new List<College_temp_marks_DTO>();

                    saved_studentList = (from a in studentList
                                         from b in stu_marks
                                         where (a.AMCST_Id == b.AMCST_Id && b.ISMS_Id == a.ISMS_Id && stu_list_mapped.Contains(a.AMCST_Id))
                                         select new College_temp_marks_DTO
                                         {
                                             AMCST_Id = a.AMCST_Id,
                                             AMCST_FirstName = a.AMCST_FirstName,
                                             AMCST_AdmNo = a.AMCST_AdmNo,
                                             AMCST_RegistrationNo = a.AMCST_RegistrationNo == null ? "" : a.AMCST_RegistrationNo,
                                             ACYST_RollNo = a.ACYST_RollNo,
                                             ISMS_Id = a.ISMS_Id,
                                             ISMS_SubjectName = a.ISMS_SubjectName,
                                             ECYSES_MaxMarks = a.ECYSES_MaxMarks,
                                             ECYSES_MarksEntryMax = a.ECYSES_MarksEntryMax,
                                             ECYSES_MinMarks = a.ECYSES_MinMarks,
                                             ECSTM_Marks = Convert.ToDecimal(b.ECSTM_Marks),
                                             ECSTM_Grade = b.ECSTM_Grade,
                                             ECSTM_Flg = b.ECSTM_Flg,
                                             ECSTM_Id = b.ECSTM_Id
                                         }).Distinct().OrderBy(t => order).ToList();

                    var propertyInfo3 = typeof(College_temp_marks_DTO).GetProperty(order);
                    saved_studentList = saved_studentList.OrderBy(x => propertyInfo3.GetValue(x, null)).ToList();


                    data.saved_studentList = saved_studentList.Distinct().ToArray();
                    //sanjevv
                    if (data.message == "Mobile")
                    {
                        data.saved_se_list = (from a in _examcontext.Exm_Col_Student_Marks_SubSubjectDMO
                                              from b in stu_marks
                                              where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ECSTM_Id == b.ECSTM_Id &&  a.EMSE_Id==data.EMSE_Id)
                                              select a).Distinct().ToArray();
                    }
                    else
                    {
                        data.saved_se_list = (from a in _examcontext.Exm_Col_Student_Marks_SubSubjectDMO
                                              from b in stu_marks
                                              where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ECSTM_Id == b.ECSTM_Id)
                                              select a).Distinct().ToArray();
                    }
                    
                       
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
                _logger.LogDebug(ee.Message);

            }
            return data;
        }
        public CollegeMarksEntryDTO SaveMarks(CollegeMarksEntryDTO data)
        {
            try
            {
                if (!data.ECYSES_SubSubjectFlg && !data.ECYSES_SubExamFlg)
                {
                    try
                    {
                        for (int i = 0; i < data.main_save_list.Length; i++)
                        {
                            var stu_id = data.main_save_list[i].AMCST_Id;
                            var already_cnt = _examcontext.Exm_Col_Student_MarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                            && t.AMCO_ID == data.AMCO_Id && t.AMB_ID == data.AMB_Id && t.AMSE_ID == data.AMSE_Id && t.ASMAY_Id == data.ASMAY_Id
                            && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.AMCST_Id == stu_id).Count();
                            if (already_cnt == 0)
                            {
                                Exm_Col_Student_MarksDMO obj_M = new Exm_Col_Student_MarksDMO();
                                obj_M.MI_Id = data.MI_Id;
                                obj_M.ASMAY_Id = data.ASMAY_Id;
                                obj_M.AMCO_ID = data.AMCO_Id;
                                obj_M.AMB_ID = data.AMB_Id;
                                obj_M.AMSE_ID = data.AMSE_Id;
                                obj_M.ACMS_Id = data.ACMS_Id;
                                obj_M.EME_Id = data.EME_Id;
                                obj_M.ISMS_Id = data.ISMS_Id;
                                obj_M.AMCST_Id = stu_id;
                                obj_M.ECSTM_Marks = data.main_save_list[i].ECSTM_Marks;
                                obj_M.ECSTM_MarksGradeFlg = data.ECYSES_MarksGradeEntryFlg;
                                obj_M.Login_Id = data.userId;
                                obj_M.ECSTM_CreatedBy = data.userId;
                                obj_M.ECSTM_UpdatedBy = data.userId;
                                obj_M.ECSTM_LoginDate = DateTime.Now;
                                obj_M.ECSTM_IP4Address = data.IP4;
                                obj_M.CreatedDate = DateTime.Now;
                                obj_M.UpdatedDate = DateTime.Now;
                                obj_M.ECSTM_ActiveFlg = true;
                                obj_M.ECSTM_Grade = data.main_save_list[i].ECSTM_Grade;
                                obj_M.ECSTM_Flg = data.main_save_list[i].ECSTM_Flg;
                                _examcontext.Add(obj_M);
                            }
                            else if (already_cnt == 1)
                            {
                                var result_obj = _examcontext.Exm_Col_Student_MarksDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                                && t.AMCO_ID == data.AMCO_Id && t.AMB_ID == data.AMB_Id && t.AMSE_ID == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id
                                && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id
                                && t.AMCST_Id == stu_id);
                                result_obj.ECSTM_Marks = data.main_save_list[i].ECSTM_Marks;
                                result_obj.ECSTM_MarksGradeFlg = data.ECYSES_MarksGradeEntryFlg;
                                result_obj.Login_Id = data.userId;
                                result_obj.ECSTM_UpdatedBy = data.userId;
                                result_obj.ECSTM_LoginDate = DateTime.Now;
                                result_obj.ECSTM_IP4Address = data.IP4;
                                result_obj.UpdatedDate = DateTime.Now;
                                result_obj.ECSTM_ActiveFlg = true;
                                result_obj.ECSTM_Grade = data.main_save_list[i].ECSTM_Grade;
                                result_obj.ECSTM_Flg = data.main_save_list[i].ECSTM_Flg;
                                _examcontext.Update(result_obj);
                            }
                        }
                        var contactExists = _examcontext.SaveChanges();
                        if (contactExists >= 1)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    catch (Exception ee)
                    {
                        _logger.LogError(ee.Message);
                        _logger.LogDebug(ee.Message);
                        Console.WriteLine(ee.Message);
                    }
                }

                else if (data.ECYSES_SubSubjectFlg && data.ECYSES_SubExamFlg)
                {
                    try
                    {
                        for (int i = 0; i < data.main_save_list.Length; i++)
                        {
                            var stu_id = data.main_save_list[i].AMCST_Id;

                            var already_cnt = _examcontext.Exm_Col_Student_MarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                            && t.AMCO_ID == data.AMCO_Id && t.AMB_ID == data.AMB_Id && t.AMSE_ID == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id && t.EME_Id == data.EME_Id
                            && t.ISMS_Id == data.ISMS_Id && t.AMCST_Id == stu_id).Count();

                            if (already_cnt == 0)
                            {
                                Exm_Col_Student_MarksDMO obj_M = new Exm_Col_Student_MarksDMO();
                                obj_M.MI_Id = data.MI_Id;
                                obj_M.ASMAY_Id = data.ASMAY_Id;
                                obj_M.AMCO_ID = data.AMCO_Id;
                                obj_M.AMB_ID = data.AMB_Id;
                                obj_M.AMSE_ID = data.AMSE_Id;
                                obj_M.ACMS_Id = data.ACMS_Id;
                                obj_M.EME_Id = data.EME_Id;
                                obj_M.ISMS_Id = data.ISMS_Id;
                                obj_M.AMCST_Id = stu_id;
                                obj_M.ECSTM_Marks = data.main_save_list[i].ECSTM_Marks;
                                obj_M.ECSTM_MarksGradeFlg = data.ECYSES_MarksGradeEntryFlg;
                                obj_M.Login_Id = data.userId;
                                obj_M.ECSTM_CreatedBy = data.userId;
                                obj_M.ECSTM_UpdatedBy = data.userId;
                                obj_M.ECSTM_LoginDate = DateTime.Now;
                                obj_M.ECSTM_IP4Address = data.IP4;
                                obj_M.CreatedDate = DateTime.Now;
                                obj_M.UpdatedDate = DateTime.Now;
                                obj_M.ECSTM_ActiveFlg = true;
                                obj_M.ECSTM_Grade = data.main_save_list[i].ECSTM_Grade;
                                obj_M.ECSTM_Flg = data.main_save_list[i].ECSTM_Flg;
                                _examcontext.Add(obj_M);

                                for (int j = 0; j < data.Temp_subs_marks_list.Length; j++)
                                {
                                    var sub_id = data.Temp_subs_marks_list[j].ISMS_Id;
                                    if (data.ISMS_Id == sub_id && stu_id == data.Temp_subs_marks_list[j].AMCST_Id)
                                    {
                                        Exm_Col_Student_Marks_SubSubjectDMO obj_S = new Exm_Col_Student_Marks_SubSubjectDMO();
                                        obj_S.MI_Id = data.MI_Id;
                                        obj_S.ECSTM_Id = obj_M.ECSTM_Id;
                                        obj_S.EMSS_Id = data.Temp_subs_marks_list[j].EMSS_Id;
                                        obj_S.ISMS_Id = sub_id;
                                        obj_S.EMSE_Id = data.Temp_subs_marks_list[j].EMSE_Id;
                                        obj_S.ECSTMSS_Marks = data.Temp_subs_marks_list[j].ECSTMSS_Marks;
                                        obj_S.ECSTMSS_Grade = data.Temp_subs_marks_list[j].ECSTMSS_Grade;
                                        obj_S.ECSTMSS_MarksGradeFlg = data.Temp_subs_marks_list[j].ECSTMSS_MarksGradeFlg;
                                        obj_S.Login_Id = data.userId;
                                        obj_S.LoginDateTime = DateTime.Now;
                                        obj_S.IP4 = data.IP4;
                                        obj_S.ECSTMSS_ActiveFlg = true;
                                        obj_S.ECSTMSS_Flg = data.Temp_subs_marks_list[j].ECSTMSS_Flg;
                                        obj_S.CreatedDate = DateTime.Now;
                                        obj_S.UpdatedDate = DateTime.Now;
                                        _examcontext.Add(obj_S);
                                    }
                                }
                            }
                            else if (already_cnt == 1)
                            {
                                decimal? marks = 0;
                                List<long> emssid = new List<long>();
                                List<long> emseid = new List<long>();

                                for (int j = 0; j < data.Temp_subs_marks_list.Length; j++)
                                {
                                    var sub_idnew = data.Temp_subs_marks_list[j].ISMS_Id;

                                    if (data.ISMS_Id == sub_idnew && stu_id == data.Temp_subs_marks_list[j].AMCST_Id)
                                    {
                                        var getsubexammarks = (from a in _examcontext.Exm_Col_Student_Marks_SubSubjectDMO
                                                               from b in _examcontext.Exm_Col_Student_MarksDMO
                                                               where (a.ECSTM_Id == b.ECSTM_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                                               && b.AMCO_ID == data.AMCO_Id && b.AMB_ID == data.AMB_Id && b.AMSE_ID == data.AMSE_Id
                                                               && b.ISMS_Id == data.ISMS_Id && b.ACMS_Id == data.ACMS_Id
                                                               && b.AMCST_Id == stu_id && a.EMSE_Id != data.Temp_subs_marks_list[j].EMSE_Id
                                                               && a.EMSS_Id != data.Temp_subs_marks_list[j].EMSS_Id && b.EME_Id == data.EME_Id)

                                                               select new Exm_Col_Student_Marks_SubSubjectDMO
                                                               {
                                                                   EMSE_Id = a.EMSE_Id

                                                               }).Distinct().ToList();

                                        var getsubsubjectmarks = (from a in _examcontext.Exm_Col_Student_Marks_SubSubjectDMO
                                                                  from b in _examcontext.Exm_Col_Student_MarksDMO
                                                                  where (a.ECSTM_Id == b.ECSTM_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                                                 && b.AMCO_ID == data.AMCO_Id && b.AMB_ID == data.AMB_Id && b.AMSE_ID == data.AMSE_Id
                                                                 && b.ISMS_Id == data.ISMS_Id && b.ACMS_Id == data.ACMS_Id
                                                                  && b.AMCST_Id == stu_id && a.EMSE_Id != data.Temp_subs_marks_list[j].EMSE_Id
                                                                  && a.EMSS_Id != data.Temp_subs_marks_list[j].EMSS_Id && b.EME_Id == data.EME_Id)

                                                                  select new Exm_Col_Student_Marks_SubSubjectDMO
                                                                  {
                                                                      EMSS_Id = a.EMSS_Id

                                                                  }).Distinct().ToList();

                                        foreach (var sd in getsubsubjectmarks)
                                        {
                                            emssid.Add(sd.EMSS_Id);
                                        }

                                        foreach (var s in getsubexammarks)
                                        {
                                            emseid.Add(s.EMSE_Id);
                                        }
                                    }
                                }

                                var getsubsubjectsubexammarks = (from a in _examcontext.Exm_Col_Student_Marks_SubSubjectDMO
                                                                 from b in _examcontext.Exm_Col_Student_MarksDMO
                                                                 where (a.ECSTM_Id == b.ECSTM_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                                                  && b.AMCO_ID == data.AMCO_Id && b.AMB_ID == data.AMB_Id && b.AMSE_ID == data.AMSE_Id
                                                                  && b.ISMS_Id == data.ISMS_Id && b.ACMS_Id == data.ACMS_Id
                                                                 && b.AMCST_Id == stu_id && emseid.Contains(a.EMSE_Id) && emssid.Contains(a.EMSS_Id)
                                                                 && b.EME_Id == data.EME_Id)

                                                                 select new Exm_Col_Student_Marks_SubSubjectDMO
                                                                 {
                                                                     ECSTMSS_Marks = a.ECSTMSS_Marks

                                                                 }).ToList();


                                foreach (var m in getsubsubjectsubexammarks)
                                {
                                    marks = marks + m.ECSTMSS_Marks;
                                }


                                var result_obj = _examcontext.Exm_Col_Student_MarksDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                                && t.AMCO_ID == data.AMCO_Id && t.AMB_ID == data.AMB_Id && t.AMSE_ID == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id
                                && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.AMCST_Id == stu_id);

                                result_obj.ECSTM_Marks = data.main_save_list[i].ECSTM_Marks + Convert.ToDecimal(marks);
                                result_obj.ECSTM_MarksGradeFlg = data.ECYSES_MarksGradeEntryFlg;
                                result_obj.Login_Id = data.userId;
                                result_obj.ECSTM_UpdatedBy = data.userId;
                                result_obj.ECSTM_LoginDate = DateTime.Now;
                                result_obj.ECSTM_IP4Address = data.IP4;
                                result_obj.UpdatedDate = DateTime.Now;
                                result_obj.ECSTM_ActiveFlg = true;
                                result_obj.ECSTM_Grade = data.main_save_list[i].ECSTM_Grade;
                                result_obj.ECSTM_Flg = data.main_save_list[i].ECSTM_Flg;
                                _examcontext.Update(result_obj);

                                for (int j = 0; j < data.Temp_subs_marks_list.Length; j++)
                                {
                                    var sub_id = data.Temp_subs_marks_list[j].ISMS_Id;
                                    if (data.ISMS_Id == sub_id && stu_id == data.Temp_subs_marks_list[j].AMCST_Id)
                                    {
                                        var checkresult = _examcontext.Exm_Col_Student_Marks_SubSubjectDMO.Where(t => t.MI_Id == data.MI_Id
                                        && t.ECSTM_Id == result_obj.ECSTM_Id && t.EMSS_Id == data.Temp_subs_marks_list[j].EMSS_Id
                                        && t.EMSE_Id == data.Temp_subs_marks_list[j].EMSE_Id).ToList();

                                        if (checkresult.Count() > 0)
                                        {
                                            var checkresultnew = _examcontext.Exm_Col_Student_Marks_SubSubjectDMO.Single(t => t.MI_Id == data.MI_Id
                                         && t.ECSTM_Id == result_obj.ECSTM_Id && t.EMSS_Id == data.Temp_subs_marks_list[j].EMSS_Id
                                         && t.EMSE_Id == data.Temp_subs_marks_list[j].EMSE_Id);

                                            checkresultnew.ECSTMSS_Marks = data.Temp_subs_marks_list[j].ECSTMSS_Marks;
                                            checkresultnew.ECSTMSS_MarksGradeFlg = data.Temp_subs_marks_list[j].ECSTMSS_MarksGradeFlg;
                                            checkresultnew.ECSTMSS_Grade = data.Temp_subs_marks_list[j].ECSTMSS_Grade;
                                            checkresultnew.Login_Id = data.userId;
                                            checkresultnew.LoginDateTime = DateTime.Now;
                                            checkresultnew.IP4 = data.IP4;
                                            checkresultnew.ECSTMSS_ActiveFlg = true;
                                            checkresultnew.ECSTMSS_Flg = data.Temp_subs_marks_list[j].ECSTMSS_Flg;
                                            checkresultnew.UpdatedDate = DateTime.UtcNow;
                                            _examcontext.Update(checkresultnew);

                                        }
                                        else
                                        {
                                            Exm_Col_Student_Marks_SubSubjectDMO obj_S = new Exm_Col_Student_Marks_SubSubjectDMO();
                                            obj_S.MI_Id = data.MI_Id;
                                            obj_S.ECSTM_Id = result_obj.ECSTM_Id;
                                            obj_S.EMSS_Id = data.Temp_subs_marks_list[j].EMSS_Id;
                                            obj_S.ISMS_Id = sub_id;
                                            obj_S.EMSE_Id = data.Temp_subs_marks_list[j].EMSE_Id;
                                            obj_S.ECSTMSS_Marks = data.Temp_subs_marks_list[j].ECSTMSS_Marks;
                                            obj_S.ECSTMSS_Grade = data.Temp_subs_marks_list[j].ECSTMSS_Grade;
                                            obj_S.ECSTMSS_MarksGradeFlg = data.Temp_subs_marks_list[j].ECSTMSS_MarksGradeFlg;
                                            obj_S.Login_Id = data.userId;
                                            obj_S.LoginDateTime = DateTime.Now;
                                            obj_S.IP4 = data.IP4;
                                            obj_S.ECSTMSS_ActiveFlg = true;
                                            obj_S.ECSTMSS_Flg = data.Temp_subs_marks_list[j].ECSTMSS_Flg;
                                            obj_S.CreatedDate = DateTime.Now;
                                            obj_S.UpdatedDate = DateTime.Now;
                                            _examcontext.Add(obj_S);
                                        }
                                    }
                                }
                            }
                        }
                        var contactExists = _examcontext.SaveChanges();
                        if (contactExists >= 1)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    catch (Exception ee)
                    {
                        _logger.LogError(ee.Message);
                        _logger.LogDebug(ee.Message);
                        Console.WriteLine(ee.Message);
                    }
                }

                else if (data.ECYSES_SubSubjectFlg && !data.ECYSES_SubExamFlg)
                {
                    try
                    {
                        for (int i = 0; i < data.main_save_list.Length; i++)
                        {
                            var stu_id = data.main_save_list[i].AMCST_Id;
                            var already_cnt = _examcontext.Exm_Col_Student_MarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                               && t.AMCO_ID == data.AMCO_Id && t.AMB_ID == data.AMB_Id && t.AMSE_ID == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id
                               && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.AMCST_Id == stu_id).Count();
                            if (already_cnt == 0)
                            {
                                Exm_Col_Student_MarksDMO obj_M = new Exm_Col_Student_MarksDMO();
                                obj_M.MI_Id = data.MI_Id;
                                obj_M.ASMAY_Id = data.ASMAY_Id;
                                obj_M.AMCO_ID = data.AMCO_Id;
                                obj_M.AMB_ID = data.AMB_Id;
                                obj_M.AMSE_ID = data.AMSE_Id;
                                obj_M.ACMS_Id = data.ACMS_Id;
                                obj_M.EME_Id = data.EME_Id;
                                obj_M.ISMS_Id = data.ISMS_Id;
                                obj_M.AMCST_Id = stu_id;
                                obj_M.ECSTM_Marks = data.main_save_list[i].ECSTM_Marks;
                                obj_M.ECSTM_MarksGradeFlg = data.ECYSES_MarksGradeEntryFlg;
                                obj_M.Login_Id = data.userId;
                                obj_M.ECSTM_CreatedBy = data.userId;
                                obj_M.ECSTM_UpdatedBy = data.userId;
                                obj_M.ECSTM_LoginDate = DateTime.Now;
                                obj_M.ECSTM_IP4Address = data.IP4;
                                obj_M.CreatedDate = DateTime.Now;
                                obj_M.UpdatedDate = DateTime.Now;
                                obj_M.ECSTM_ActiveFlg = true;
                                obj_M.ECSTM_Grade = data.main_save_list[i].ECSTM_Grade;
                                obj_M.ECSTM_Flg = data.main_save_list[i].ECSTM_Flg;
                                _examcontext.Add(obj_M);

                                for (int j = 0; j < data.Temp_subs_marks_list.Length; j++)
                                {
                                    var sub_id = data.Temp_subs_marks_list[j].ISMS_Id;
                                    if (data.ISMS_Id == sub_id && stu_id == data.Temp_subs_marks_list[j].AMCST_Id)
                                    {
                                        Exm_Col_Student_Marks_SubSubjectDMO obj_S = new Exm_Col_Student_Marks_SubSubjectDMO();
                                        obj_S.MI_Id = data.MI_Id;
                                        obj_S.ECSTM_Id = obj_M.ECSTM_Id;
                                        obj_S.EMSS_Id = data.Temp_subs_marks_list[j].EMSS_Id;
                                        obj_S.ISMS_Id = sub_id;
                                        obj_S.EMSE_Id = data.Temp_subs_marks_list[j].EMSE_Id;
                                        obj_S.ECSTMSS_Marks = data.Temp_subs_marks_list[j].ECSTMSS_Marks;
                                        obj_S.ECSTMSS_MarksGradeFlg = data.Temp_subs_marks_list[j].ECSTMSS_MarksGradeFlg;
                                        obj_S.ECSTMSS_Grade = data.Temp_subs_marks_list[j].ECSTMSS_Grade;
                                        obj_S.Login_Id = data.userId;
                                        obj_S.LoginDateTime = DateTime.Now;
                                        obj_S.IP4 = data.IP4;
                                        obj_S.ECSTMSS_ActiveFlg = true;
                                        obj_S.ECSTMSS_Flg = data.Temp_subs_marks_list[j].ECSTMSS_Flg;
                                        obj_S.CreatedDate = DateTime.Now;
                                        obj_S.UpdatedDate = DateTime.Now;
                                        _examcontext.Add(obj_S);
                                    }
                                }
                            }

                            else if (already_cnt == 1)
                            {
                                var result_obj = _examcontext.Exm_Col_Student_MarksDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                                && t.AMCO_ID == data.AMCO_Id && t.AMB_ID == data.AMB_Id && t.AMSE_ID == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id
                                && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.AMCST_Id == stu_id);
                                result_obj.ECSTM_Marks = data.main_save_list[i].ECSTM_Marks;
                                result_obj.ECSTM_MarksGradeFlg = data.ECYSES_MarksGradeEntryFlg;
                                result_obj.Login_Id = data.userId;
                                result_obj.ECSTM_LoginDate = DateTime.Now;
                                result_obj.ECSTM_IP4Address = data.IP4;
                                result_obj.UpdatedDate = DateTime.Now;
                                result_obj.ECSTM_ActiveFlg = true;
                                result_obj.ECSTM_Grade = data.main_save_list[i].ECSTM_Grade;
                                result_obj.ECSTM_Flg = data.main_save_list[i].ECSTM_Flg;
                                _examcontext.Update(result_obj);

                                var child_list = _examcontext.Exm_Col_Student_Marks_SubSubjectDMO.Where(t => t.MI_Id == data.MI_Id && t.ECSTM_Id == result_obj.ECSTM_Id).ToList();
                                if (child_list.Any())
                                {
                                    for (int l = 0; child_list.Count > l; l++)
                                    {
                                        _examcontext.Remove(child_list.ElementAt(l));
                                    }
                                }
                                for (int j = 0; j < data.Temp_subs_marks_list.Length; j++)
                                {
                                    var sub_id = data.Temp_subs_marks_list[j].ISMS_Id;
                                    if (data.ISMS_Id == sub_id && stu_id == data.Temp_subs_marks_list[j].AMCST_Id)
                                    {
                                        Exm_Col_Student_Marks_SubSubjectDMO obj_S = new Exm_Col_Student_Marks_SubSubjectDMO();
                                        obj_S.MI_Id = data.MI_Id;
                                        obj_S.ECSTM_Id = result_obj.ECSTM_Id;
                                        obj_S.EMSS_Id = data.Temp_subs_marks_list[j].EMSS_Id;
                                        obj_S.ISMS_Id = sub_id;
                                        obj_S.EMSE_Id = data.Temp_subs_marks_list[j].EMSE_Id;
                                        obj_S.ECSTMSS_Marks = data.Temp_subs_marks_list[j].ECSTMSS_Marks;
                                        obj_S.ECSTMSS_MarksGradeFlg = data.Temp_subs_marks_list[j].ECSTMSS_MarksGradeFlg;
                                        obj_S.ECSTMSS_Grade = data.Temp_subs_marks_list[j].ECSTMSS_Grade;
                                        obj_S.Login_Id = data.userId;
                                        obj_S.LoginDateTime = DateTime.Now;
                                        obj_S.IP4 = data.IP4;
                                        obj_S.ECSTMSS_ActiveFlg = true;
                                        obj_S.ECSTMSS_Flg = data.Temp_subs_marks_list[j].ECSTMSS_Flg;
                                        obj_S.CreatedDate = DateTime.Now;
                                        obj_S.UpdatedDate = DateTime.Now;
                                        _examcontext.Add(obj_S);
                                    }
                                }
                            }

                        }
                        var contactExists = _examcontext.SaveChanges();
                        if (contactExists >= 1)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    catch (Exception ee)
                    {
                        _logger.LogError(ee.Message);
                        _logger.LogDebug(ee.Message);
                        Console.WriteLine(ee.Message);
                    }
                }

                else if (!data.ECYSES_SubSubjectFlg && data.ECYSES_SubExamFlg)
                {

                    try
                    {
                        for (int i = 0; i < data.main_save_list.Length; i++)
                        {
                            var stu_id = data.main_save_list[i].AMCST_Id;
                            var already_cnt = _examcontext.Exm_Col_Student_MarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                            && t.AMCO_ID == data.AMCO_Id && t.AMB_ID == data.AMB_Id && t.AMSE_ID == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id
                            && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.AMCST_Id == stu_id).Count();
                            if (already_cnt == 0)
                            {
                                Exm_Col_Student_MarksDMO obj_M = new Exm_Col_Student_MarksDMO();
                                obj_M.MI_Id = data.MI_Id;
                                obj_M.ASMAY_Id = data.ASMAY_Id;
                                obj_M.AMCO_ID = data.AMCO_Id;
                                obj_M.AMB_ID = data.AMB_Id;
                                obj_M.AMSE_ID = data.AMSE_Id;
                                obj_M.ACMS_Id = data.ACMS_Id;
                                obj_M.EME_Id = data.EME_Id;
                                obj_M.ISMS_Id = data.ISMS_Id;
                                obj_M.AMCST_Id = stu_id;
                                obj_M.ECSTM_Marks = data.main_save_list[i].ECSTM_Marks;
                                obj_M.ECSTM_MarksGradeFlg = data.ECYSES_MarksGradeEntryFlg;
                                obj_M.Login_Id = data.userId;
                                obj_M.ECSTM_CreatedBy = data.userId;
                                obj_M.ECSTM_UpdatedBy = data.userId;
                                obj_M.ECSTM_LoginDate = DateTime.Now;
                                obj_M.ECSTM_IP4Address = data.IP4;
                                obj_M.CreatedDate = DateTime.Now;
                                obj_M.UpdatedDate = DateTime.Now;
                                obj_M.ECSTM_ActiveFlg = true;
                                obj_M.ECSTM_Grade = data.main_save_list[i].ECSTM_Grade;
                                obj_M.ECSTM_Flg = data.main_save_list[i].ECSTM_Flg;
                                _examcontext.Add(obj_M);

                                for (int j = 0; j < data.Temp_subs_marks_list.Length; j++)
                                {
                                    var sub_id = data.Temp_subs_marks_list[j].ISMS_Id;
                                    if (data.ISMS_Id == sub_id && stu_id == data.Temp_subs_marks_list[j].AMCST_Id)
                                    {
                                        Exm_Col_Student_Marks_SubSubjectDMO obj_S = new Exm_Col_Student_Marks_SubSubjectDMO();
                                        obj_S.MI_Id = data.MI_Id;
                                        obj_S.ECSTM_Id = obj_M.ECSTM_Id;
                                        obj_S.EMSS_Id = data.Temp_subs_marks_list[j].EMSS_Id;
                                        obj_S.ISMS_Id = sub_id;
                                        obj_S.EMSE_Id = data.Temp_subs_marks_list[j].EMSE_Id;
                                        obj_S.ECSTMSS_Marks = data.Temp_subs_marks_list[j].ECSTMSS_Marks;
                                        obj_S.ECSTMSS_MarksGradeFlg = data.Temp_subs_marks_list[j].ECSTMSS_MarksGradeFlg;
                                        obj_S.ECSTMSS_Grade = data.Temp_subs_marks_list[j].ECSTMSS_Grade;
                                        obj_S.Login_Id = data.userId;
                                        obj_S.LoginDateTime = DateTime.Now;
                                        obj_S.IP4 = data.IP4;
                                        obj_S.ECSTMSS_ActiveFlg = true;
                                        obj_S.ECSTMSS_Flg = data.Temp_subs_marks_list[j].ECSTMSS_Flg;
                                        obj_S.CreatedDate = DateTime.Now;
                                        obj_S.UpdatedDate = DateTime.Now;
                                        _examcontext.Add(obj_S);
                                    }
                                }
                            }
                            else if (already_cnt == 1)
                            {
                                var result_obj = _examcontext.Exm_Col_Student_MarksDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                                && t.AMCO_ID == data.AMCO_Id && t.AMB_ID == data.AMB_Id && t.AMSE_ID == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id
                                && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.AMCST_Id == stu_id);

                                result_obj.ECSTM_Marks = data.main_save_list[i].ECSTM_Marks;
                                result_obj.ECSTM_MarksGradeFlg = data.ECYSES_MarksGradeEntryFlg;
                                result_obj.Login_Id = data.userId;
                                result_obj.ECSTM_UpdatedBy = data.userId;
                                result_obj.ECSTM_LoginDate = DateTime.Now;
                                result_obj.ECSTM_IP4Address = data.IP4;
                                result_obj.UpdatedDate = DateTime.Now;
                                result_obj.ECSTM_ActiveFlg = true;
                                result_obj.ECSTM_Grade = data.main_save_list[i].ECSTM_Grade;
                                result_obj.ECSTM_Flg = data.main_save_list[i].ECSTM_Flg;
                                _examcontext.Update(result_obj);

                                var child_list = _examcontext.Exm_Col_Student_Marks_SubSubjectDMO.Where(t => t.MI_Id == data.MI_Id && t.ECSTM_Id == result_obj.ECSTM_Id).ToList();
                                if (child_list.Any())
                                {
                                    for (int l = 0; child_list.Count > l; l++)
                                    {
                                        _examcontext.Remove(child_list.ElementAt(l));
                                    }
                                }
                                for (int j = 0; j < data.Temp_subs_marks_list.Length; j++)
                                {
                                    var sub_id = data.Temp_subs_marks_list[j].ISMS_Id;
                                    if (data.ISMS_Id == sub_id && stu_id == data.Temp_subs_marks_list[j].AMCST_Id)
                                    {
                                        Exm_Col_Student_Marks_SubSubjectDMO obj_S = new Exm_Col_Student_Marks_SubSubjectDMO();
                                        obj_S.MI_Id = data.MI_Id;
                                        obj_S.ECSTM_Id = result_obj.ECSTM_Id;
                                        obj_S.EMSS_Id = data.Temp_subs_marks_list[j].EMSS_Id;
                                        obj_S.ISMS_Id = sub_id;
                                        obj_S.EMSE_Id = data.Temp_subs_marks_list[j].EMSE_Id;
                                        obj_S.ECSTMSS_Marks = data.Temp_subs_marks_list[j].ECSTMSS_Marks;
                                        obj_S.ECSTMSS_MarksGradeFlg = data.Temp_subs_marks_list[j].ECSTMSS_MarksGradeFlg;
                                        obj_S.ECSTMSS_Grade = data.Temp_subs_marks_list[j].ECSTMSS_Grade;
                                        obj_S.Login_Id = data.userId;
                                        obj_S.LoginDateTime = DateTime.Now;
                                        obj_S.IP4 = data.IP4;
                                        obj_S.ECSTMSS_ActiveFlg = true;
                                        obj_S.ECSTMSS_Flg = data.Temp_subs_marks_list[j].ECSTMSS_Flg;
                                        obj_S.CreatedDate = DateTime.Now;
                                        obj_S.UpdatedDate = DateTime.Now;
                                        _examcontext.Add(obj_S);
                                    }
                                }
                            }

                        }
                        var contactExists = _examcontext.SaveChanges();
                        if (contactExists >= 1)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    catch (Exception ee)
                    {
                        _logger.LogError(ee.Message);
                        _logger.LogDebug(ee.Message);
                        Console.WriteLine(ee.Message);
                    }
                }
                //added By sanjeev  
                //message
                if(data.message== "Autocalculate")
                {
                    using (var cmd = _examcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CLG_IndSubjects_SubsExmMarksCalculation";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.BigInt) { Value = data.AMCO_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.BigInt) { Value = data.AMB_Id });
                        cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.BigInt) { Value = data.ACMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.BigInt) { Value = data.AMSE_Id });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.Int) { Value = data.EME_Id });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var a = cmd.ExecuteNonQuery();


                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        //onchangesubsubject
        public CollegeMarksEntryDTO onchangesubsubject(CollegeMarksEntryDTO id)
        {
            try
            {

                var eycid = _examcontext.Exm_Col_Yearly_SchemeDMO.Where(t => t.MI_Id == id.MI_Id
                && t.AMCO_Id == id.AMCO_Id && t.AMB_Id == id.AMB_Id && t.AMSE_Id == id.AMSE_Id && t.ACSS_Id == id.ACSS_Id && t.ACST_Id == id.ACST_Id && t.ECYS_ActiveFlag == true).Select(t => t.ECYS_Id).ToArray();

                var eyceid = _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO.Where(t => eycid.Contains(t.ECYS_Id) && t.EME_Id == id.EME_Id
               && t.AMCO_Id == id.AMCO_Id && t.AMB_Id == id.AMB_Id && t.AMSE_Id == id.AMSE_Id && t.ACSS_Id == id.ACSS_Id && t.ACST_Id == id.ACST_Id && t.ECYSE_ActiveFlg == true).Select(t => t.ECYSE_Id).ToArray();

                var subid = _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.ECYSE_Id) && t.ISMS_Id == id.ISMS_Id
               && t.ECYSES_ActiveFlg == true).ToList();
                if (subid.FirstOrDefault().ECYSES_SubSubjectFlg == true)
                {

                    var sectionexamid = _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO.Where(R => R.ECYSES_Id == subid.FirstOrDefault().ECYSES_Id && R.ECYSESSS_ActiveFlg == true).Select(t => t.EMSS_Id).ToArray();

                    id.subsubjectlist = _examcontext.clg_mastersubsubject.Where(c => c.MI_Id == id.MI_Id && c.EMSS_ActiveFlag == true
                     && sectionexamid.Contains(c.EMSS_Id)).OrderBy(t => t.EMSS_Order).ToArray();

                }
                else if (subid.FirstOrDefault().ECYSES_SubExamFlg == true)
                {
                    var subexamid = _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO.Where(R => R.ECYSES_Id == subid.FirstOrDefault().ECYSES_Id && R.ECYSESSS_ActiveFlg == true).Select(t => t.EMSE_Id).ToArray();

                    id.subexamlist = _examcontext.clg_mastersubexam.Where(c => c.MI_Id == id.MI_Id && c.EMSE_ActiveFlag == true
                    && subexamid.Contains(c.EMSE_Id)).OrderBy(t => t.EMSE_SubExamOrder).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return id;
        }

        //savemarkMobile 

        public async Task<CollegeMarksEntryDTO> onsearchMobile(CollegeMarksEntryDTO data)
        {
            CollegeMarksEntryDTO EM = new CollegeMarksEntryDTO();
            try
            {
                string order = "AMCST_FirstName";
                var get_configuration = _examcontext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                data.configuration = get_configuration.ToArray();

                if (get_configuration != null && get_configuration.Count > 0)
                {
                    if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                    {
                        order = "AMCST_FirstName";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "AdmNo")
                    {
                        order = "AMCST_AdmNo";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RollNo")
                    {
                        order = "ACYST_RollNo";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RegNo")
                    {
                        order = "AMCST_RegistrationNo";
                    }
                    else
                    {
                        order = "AMCST_FirstName";
                    }
                }

                var subject_details = (from a in _examcontext.Exm_Col_Yearly_SchemeDMO
                                       from b in _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO
                                       from c in _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO
                                       from d in _examcontext.IVRM_School_Master_SubjectsDMO
                                       where (a.ECYS_Id == b.ECYS_Id && b.ECYSE_Id == c.ECYSE_Id && d.ISMS_Id == c.ISMS_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id
                                       && a.AMSE_Id == data.AMSE_Id && a.ACSS_Id == data.ACSS_Id && a.ACST_Id == data.ACST_Id && a.ECYS_ActiveFlag == true && b.AMCO_Id == data.AMCO_Id
                                       && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id && b.ACSS_Id == data.ACSS_Id && b.ACST_Id == data.ACST_Id && b.ECYSE_ActiveFlg == true
                                       && b.EME_Id == data.EME_Id && c.ISMS_Id == data.ISMS_Id && c.ECYSES_ActiveFlg == true && d.ISMS_ActiveFlag == 1 && d.ISMS_Id == data.ISMS_Id)
                                       select c).Distinct().ToList();

                data.subject_details = subject_details.ToArray();
                data.ECYSES_MarksGradeEntryFlg = subject_details[0].ECYSES_MarksGradeEntryFlg;

                data.ECYSES_SubSubjectFlg = subject_details[0].ECYSES_SubSubjectFlg;
                data.ECYSES_SubExamFlg = subject_details[0].ECYSES_SubExamFlg;

                if (data.ECYSES_SubSubjectFlg && !data.ECYSES_SubExamFlg)
                {
                    if (data.message == "Mobile")
                    {
                        data.subject_subsubjects = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                                    from b in _examcontext.clg_mastersubsubject
                                                    where (a.EMSS_Id == b.EMSS_Id && b.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.EMSS_ActiveFlag == true
                                                    && a.ECYSES_Id == subject_details[0].ECYSES_Id && b.EMSS_Id == a.EMSS_Id && a.EMSS_Id == data.EMSS_Id && b.EMSS_Id == data.EMSS_Id)
                                                    select new ClgSubjectWizardDTO
                                                    {
                                                        ECYSESSS_Id = a.ECYSESSS_Id,
                                                        ECYSES_Id = a.ECYSES_Id,
                                                        EMSE_Id = a.EMSE_Id,
                                                        EMSS_Id = a.EMSS_Id,
                                                        EMGR_Id = a.EMGR_Id,
                                                        ECYSESSS_MaxMarks = a.ECYSESSS_MaxMarks,
                                                        ECYSESSS_MinMarks = a.ECYSESSS_MinMarks,
                                                        ECYSESSS_ExemptedFlg = a.ECYSESSS_ExemptedFlg,
                                                        ECYSESSS_ExemptedPer = a.ECYSESSS_ExemptedPer,
                                                        ECYSESSS_ActiveFlg = a.ECYSESSS_ActiveFlg,
                                                        ECYSESSS_SubSubjectOrder = a.ECYSESSS_SubSubjectOrder,
                                                        EMSS_SubSubjectName = b.EMSS_SubSubjectName,
                                                        EMSS_SubSubjectCode = b.EMSS_SubSubjectCode

                                                    }).Distinct().OrderBy(a => a.ECYSESSS_SubSubjectOrder).ToArray();
                    }
                    else
                    {
                        data.subject_subsubjects = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                                    from b in _examcontext.clg_mastersubsubject
                                                    where (a.EMSS_Id == b.EMSS_Id && b.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.EMSS_ActiveFlag == true
                                                    && a.ECYSES_Id == subject_details[0].ECYSES_Id && b.EMSS_Id == a.EMSS_Id)
                                                    select new ClgSubjectWizardDTO
                                                    {
                                                        ECYSESSS_Id = a.ECYSESSS_Id,
                                                        ECYSES_Id = a.ECYSES_Id,
                                                        EMSE_Id = a.EMSE_Id,
                                                        EMSS_Id = a.EMSS_Id,
                                                        EMGR_Id = a.EMGR_Id,
                                                        ECYSESSS_MaxMarks = a.ECYSESSS_MaxMarks,
                                                        ECYSESSS_MinMarks = a.ECYSESSS_MinMarks,
                                                        ECYSESSS_ExemptedFlg = a.ECYSESSS_ExemptedFlg,
                                                        ECYSESSS_ExemptedPer = a.ECYSESSS_ExemptedPer,
                                                        ECYSESSS_ActiveFlg = a.ECYSESSS_ActiveFlg,
                                                        ECYSESSS_SubSubjectOrder = a.ECYSESSS_SubSubjectOrder,
                                                        EMSS_SubSubjectName = b.EMSS_SubSubjectName,
                                                        EMSS_SubSubjectCode = b.EMSS_SubSubjectCode

                                                    }).Distinct().OrderBy(a => a.ECYSESSS_SubSubjectOrder).ToArray();
                    }

                }
                else if (!data.ECYSES_SubSubjectFlg && data.ECYSES_SubExamFlg)
                {
                    if (data.message == "Mobile")
                    {
                        data.subject_subexams = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                                 from b in _examcontext.clg_mastersubexam
                                                 where (b.EMSE_Id == a.EMSE_Id && b.MI_Id == data.MI_Id && b.EMSE_ActiveFlag == true
                                                 && a.ECYSES_Id == subject_details[0].ECYSES_Id && a.EMSE_Id == data.EMSE_Id && b.EMSE_Id == data.EMSE_Id)
                                                 select new ClgSubjectWizardDTO
                                                 {
                                                     ECYSESSS_Id = a.ECYSESSS_Id,
                                                     ECYSES_Id = a.ECYSES_Id,
                                                     EMSE_Id = a.EMSE_Id,
                                                     EMSS_Id = a.EMSS_Id,
                                                     EMGR_Id = a.EMGR_Id,
                                                     ECYSESSS_MaxMarks = a.ECYSESSS_MaxMarks,
                                                     ECYSESSS_MinMarks = a.ECYSESSS_MinMarks,
                                                     ECYSESSS_ExemptedFlg = a.ECYSESSS_ExemptedFlg,
                                                     ECYSESSS_ExemptedPer = a.ECYSESSS_ExemptedPer,
                                                     ECYSESSS_ActiveFlg = a.ECYSESSS_ActiveFlg,
                                                     ECYSESSS_SubSubjectOrder = a.ECYSESSS_SubSubjectOrder,
                                                     EMSE_SubExamName = b.EMSE_SubExamName,
                                                     EMSE_SubExamCode = b.EMSE_SubExamCode

                                                 }).Distinct().OrderBy(a => a.ECYSESSS_SubSubjectOrder).ToArray();
                    }
                    else
                    {
                        data.subject_subexams = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                                 from b in _examcontext.clg_mastersubexam
                                                 where (b.EMSE_Id == a.EMSE_Id && b.MI_Id == data.MI_Id && b.EMSE_ActiveFlag == true
                                                 && a.ECYSES_Id == subject_details[0].ECYSES_Id)
                                                 select new ClgSubjectWizardDTO
                                                 {
                                                     ECYSESSS_Id = a.ECYSESSS_Id,
                                                     ECYSES_Id = a.ECYSES_Id,
                                                     EMSE_Id = a.EMSE_Id,
                                                     EMSS_Id = a.EMSS_Id,
                                                     EMGR_Id = a.EMGR_Id,
                                                     ECYSESSS_MaxMarks = a.ECYSESSS_MaxMarks,
                                                     ECYSESSS_MinMarks = a.ECYSESSS_MinMarks,
                                                     ECYSESSS_ExemptedFlg = a.ECYSESSS_ExemptedFlg,
                                                     ECYSESSS_ExemptedPer = a.ECYSESSS_ExemptedPer,
                                                     ECYSESSS_ActiveFlg = a.ECYSESSS_ActiveFlg,
                                                     ECYSESSS_SubSubjectOrder = a.ECYSESSS_SubSubjectOrder,
                                                     EMSE_SubExamName = b.EMSE_SubExamName,
                                                     EMSE_SubExamCode = b.EMSE_SubExamCode

                                                 }).Distinct().OrderBy(a => a.ECYSESSS_SubSubjectOrder).ToArray();
                    }

                }
                else
                {
                    data.subject_subexams = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                             from b in _examcontext.clg_mastersubsubject
                                             from c in _examcontext.clg_mastersubexam
                                             where (a.EMSS_Id == b.EMSS_Id && a.EMSE_Id == c.EMSE_Id
                                             && b.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.EMSS_ActiveFlag == true && c.EMSE_ActiveFlag == true
                                             && a.ECYSES_Id == subject_details[0].ECYSES_Id && b.EMSS_Id == a.EMSS_Id)
                                             select new ClgSubjectWizardDTO
                                             {
                                                 ECYSESSS_Id = a.ECYSESSS_Id,
                                                 ECYSES_Id = a.ECYSES_Id,
                                                 EMSE_Id = a.EMSE_Id,
                                                 EMSS_Id = a.EMSS_Id,
                                                 EMGR_Id = a.EMGR_Id,
                                                 ECYSESSS_MaxMarks = a.ECYSESSS_MaxMarks,
                                                 ECYSESSS_MinMarks = a.ECYSESSS_MinMarks,
                                                 ECYSESSS_ExemptedFlg = a.ECYSESSS_ExemptedFlg,
                                                 ECYSESSS_ExemptedPer = a.ECYSESSS_ExemptedPer,
                                                 ECYSESSS_ActiveFlg = a.ECYSESSS_ActiveFlg,
                                                 ECYSESSS_SubSubjectOrder = a.ECYSESSS_SubSubjectOrder,
                                                 EMSE_SubExamName = c.EMSE_SubExamName,
                                                 EMSE_SubExamCode = c.EMSE_SubExamCode

                                             }).Distinct().OrderBy(a => a.ECYSESSS_SubSubjectOrder).ToArray();


                    data.subject_subsubjects = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                                from b in _examcontext.clg_mastersubsubject
                                                from c in _examcontext.clg_mastersubexam
                                                where (a.EMSS_Id == b.EMSS_Id && a.EMSE_Id == c.EMSE_Id
                                                && b.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.EMSS_ActiveFlag == true && c.EMSE_ActiveFlag == true
                                                && a.ECYSES_Id == subject_details[0].ECYSES_Id && b.EMSS_Id == a.EMSS_Id)
                                                select new ClgSubjectWizardDTO
                                                {
                                                    ECYSESSS_Id = a.ECYSESSS_Id,
                                                    ECYSES_Id = a.ECYSES_Id,
                                                    EMSE_Id = a.EMSE_Id,
                                                    EMSS_Id = a.EMSS_Id,
                                                    EMGR_Id = a.EMGR_Id,
                                                    ECYSESSS_MaxMarks = a.ECYSESSS_MaxMarks,
                                                    ECYSESSS_MinMarks = a.ECYSESSS_MinMarks,
                                                    ECYSESSS_ExemptedFlg = a.ECYSESSS_ExemptedFlg,
                                                    ECYSESSS_ExemptedPer = a.ECYSESSS_ExemptedPer,
                                                    ECYSESSS_ActiveFlg = a.ECYSESSS_ActiveFlg,
                                                    ECYSESSS_SubSubjectOrder = a.ECYSESSS_SubSubjectOrder,
                                                    EMSE_SubExamName = c.EMSE_SubExamName,
                                                    EMSE_SubExamCode = c.EMSE_SubExamCode

                                                }).Distinct().OrderBy(a => a.ECYSESSS_SubSubjectOrder).ToArray();


                    data.subsubjectlist = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                           from b in _examcontext.clg_mastersubsubject
                                           where (a.EMSS_Id == b.EMSS_Id && b.MI_Id == data.MI_Id && a.ECYSESSS_ActiveFlg == true
                                           && a.ECYSES_Id == subject_details[0].ECYSES_Id && b.EMSS_Id == a.EMSS_Id)
                                           select b).Distinct().OrderBy(a => a.EMSS_Order).ToArray();

                    data.subexamlist = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                        from b in _examcontext.clg_mastersubexam
                                        from c in _examcontext.clg_mastersubsubject
                                        where (a.EMSS_Id == c.EMSS_Id && b.EMSE_Id == a.EMSE_Id && b.MI_Id == data.MI_Id
                                        && b.EMSE_ActiveFlag == true && a.ECYSESSS_ActiveFlg == true && a.ECYSES_Id == subject_details[0].ECYSES_Id)
                                        select b).Distinct().OrderBy(a => a.EMSE_SubExamOrder).ToArray();
                }

                data.EMGR_Id = subject_details[0].EMGR_Id;


                data.EMGR_Id = subject_details[0].EMGR_Id;

                if (data.ECYSES_MarksGradeEntryFlg == "G")
                {
                    data.grade_details = (from a in _examcontext.Exm_Master_GradeDMO
                                          from b in _examcontext.Exm_Master_Grade_DetailsDMO
                                          where (a.MI_Id == data.MI_Id && a.EMGR_Id == data.EMGR_Id && b.EMGR_Id == data.EMGR_Id && b.EMGD_ActiveFlag == true)
                                          select b).Select(t => t.EMGD_Name).Distinct().ToArray();

                    if (data.ECYSES_SubSubjectFlg)
                    {
                        data.subsubject_gradedetails = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                                        from b in _examcontext.Exm_Master_GradeDMO
                                                        from c in _examcontext.Exm_Master_Grade_DetailsDMO
                                                        where (a.ECYSES_Id == subject_details[0].ECYSES_Id && a.EMGR_Id == b.EMGR_Id && b.MI_Id == data.MI_Id
                                                        && b.EMGR_ActiveFlag == true && c.EMGR_Id == b.EMGR_Id && c.EMGD_ActiveFlag == true)
                                                        select c).Distinct().ToArray();
                    }

                    if (data.ECYSES_SubExamFlg)
                    {
                        data.subexam_gradedetails = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                                     from b in _examcontext.Exm_Master_GradeDMO
                                                     from c in _examcontext.Exm_Master_Grade_DetailsDMO
                                                     where (a.ECYSES_Id == subject_details[0].ECYSES_Id && a.EMGR_Id == b.EMGR_Id && b.MI_Id == data.MI_Id
                                                     && b.EMGR_ActiveFlag == true && c.EMGR_Id == b.EMGR_Id && c.EMGD_ActiveFlag == true)
                                                     select c).Distinct().ToArray();
                    }
                }

                var alrdy_stu_count = _examcontext.Exm_Col_Student_MarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                && t.AMCO_ID == data.AMCO_Id && t.AMB_ID == data.AMB_Id && t.AMSE_ID == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id && t.EME_Id == data.EME_Id
                && t.ISMS_Id == data.ISMS_Id).ToList().Count();

                var stu_list_mapped = _examcontext.Exm_Col_Studentwise_SubjectsDMO.Where(k => k.MI_Id == data.MI_Id && k.ASMAY_Id == data.ASMAY_Id && k.AMCO_Id == data.AMCO_Id
                && k.AMB_Id == data.AMB_Id && k.AMSE_Id == data.AMSE_Id && k.ASMAY_Id == data.ASMAY_Id && k.ISMS_Id == data.ISMS_Id && k.ECSTSU_ActiveFlg == true).Select(t => t.AMCST_Id).Distinct().ToList();

                List<College_temp_marks_DTO> studentList = new List<College_temp_marks_DTO>();
                List<College_temp_marks_DTO> result = new List<College_temp_marks_DTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {                  
                    cmd.CommandText = "CLG_Exam_Get_Students_Subjects_Marks_Entry";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = data.AMB_Id });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACSS_Id", SqlDbType.VarChar) { Value = data.ACSS_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACST_Id", SqlDbType.VarChar) { Value = data.ACST_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EMSS_Id", SqlDbType.VarChar) { Value = data.EMSS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EMSE_Id", SqlDbType.VarChar) { Value = data.EMSE_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = data.AMSE_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = data.ACMS_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                result.Add(new College_temp_marks_DTO
                                {
                                    AMCST_Id = Convert.ToInt64(dataReader["AMCST_Id"].ToString()),
                                    ECSTM_Id = Convert.ToInt64(dataReader["ECSTM_Id"].ToString()),
                                    AMCST_FirstName = ((dataReader["AMCST_FirstName"].ToString() == null ? " " : dataReader["AMCST_FirstName"].ToString()) + " " + (dataReader["AMCST_MiddleName"].ToString() == null ? " " : dataReader["AMCST_MiddleName"].ToString()) + " " + (dataReader["AMCST_LastName"].ToString() == null ? " " : dataReader["AMCST_LastName"].ToString())).Trim(),
                                    AMCST_AdmNo = dataReader["AMCST_AdmNo"].ToString() == null ? "" : dataReader["AMCST_AdmNo"].ToString(),
                                    AMCST_RegistrationNo = dataReader["AMCST_RegistrationNo"].ToString() == null ? "" : dataReader["AMCST_RegistrationNo"].ToString(),
                                    ECSTM_Grade = dataReader["ECSTM_Grade"].ToString() == null ? "" : dataReader["ECSTM_Grade"].ToString(),
                                    ACYST_RollNo = Convert.ToInt32(dataReader["ACYST_RollNo"].ToString()),
                                    ECSTM_Flg = dataReader["ECSTM_Flg"].ToString() == null ? "" : dataReader["ECSTM_Flg"].ToString(),
                                    ISMS_SubjectName = dataReader["ISMS_SubjectName"].ToString(),
                                    ECYSES_MaxMarks = Convert.ToDecimal(dataReader["ECYSES_MaxMarks"].ToString()),
                                    ECYSES_MarksEntryMax = Convert.ToDecimal(dataReader["ECYSES_MarksEntryMax"].ToString()),
                                    ECYSES_MinMarks = Convert.ToDecimal(dataReader["ECYSES_MinMarks"].ToString()),
                                    //ECSTM_Grade = (dataReader["ECSTM_Grade"].ToString() == "" ? dataReader["ECSTM_Grade"].ToString() : dataReader["ECSTM_Grade"].ToString()),

                                    ECSTM_Marks = Convert.ToDecimal(dataReader["ECSTM_Marks"].ToString()),


                                });
                                studentList = result.Distinct().ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }



                //studentList = (from e in _examcontext.Adm_Master_College_StudentDMO
                //               from f in _examcontext.Adm_College_Yearly_StudentDMO
                //               from g in _examcontext.IVRM_School_Master_SubjectsDMO
                //               from h in _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO
                //               from i in subject_details
                //               where (e.AMCST_Id == f.AMCST_Id && f.AMCO_Id == data.AMCO_Id && f.AMB_Id == data.AMB_Id && f.AMSE_Id == data.AMSE_Id && f.ASMAY_Id == data.ASMAY_Id
                //               && e.MI_Id == data.MI_Id && e.AMCST_ActiveFlag == true && e.AMCST_SOL == "S" && f.ACYST_ActiveFlag == 1 && g.ISMS_Id == h.ISMS_Id
                //               && g.MI_Id == data.MI_Id && g.ISMS_ActiveFlag == 1 && g.ISMS_ExamFlag == 1 && g.ISMS_Id == data.ISMS_Id && g.ISMS_Id == data.ISMS_Id
                //               && h.ECYSE_Id == i.ECYSE_Id)

                //               select new College_temp_marks_DTO
                //               {
                //                   AMCST_Id = e.AMCST_Id,
                //                   AMCST_FirstName = ((e.AMCST_FirstName == null ? "" : e.AMCST_FirstName) +
                //                   (e.AMCST_MiddleName == null || e.AMCST_MiddleName == "" ? "" : " " + e.AMCST_MiddleName) +
                //                   (e.AMCST_LastName == null || e.AMCST_LastName == "" ? "" : " " + e.AMCST_LastName)).Trim(),
                //                   AMCST_AdmNo = e.AMCST_AdmNo == null ? "" : e.AMCST_AdmNo,
                //                   AMCST_RegistrationNo = e.AMCST_RegistrationNo == null ? "" : e.AMCST_RegistrationNo,
                //                   ACYST_RollNo = f.ACYST_RollNo,
                //                   ISMS_Id = g.ISMS_Id,
                //                   ISMS_SubjectName = g.ISMS_SubjectName,
                //                   ECYSES_MaxMarks = h.ECYSES_MaxMarks,
                //                   ECYSES_MinMarks = h.ECYSES_MinMarks,
                //                   ECYSES_MarksEntryMax = h.ECYSES_MarksEntryMax,
                //               }).Distinct().OrderBy(t => order).ToList();

                //adedd



                var propertyInfo = typeof(College_temp_marks_DTO).GetProperty(order);
                studentList = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();

                data.studentList = studentList.Where(t => stu_list_mapped.Contains(t.AMCST_Id)).Distinct().ToArray();

                if (alrdy_stu_count > 0 && !data.ECYSES_SubSubjectFlg && !data.ECYSES_SubExamFlg)
                {
                    var stu_marks = _examcontext.Exm_Col_Student_MarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                    && t.AMCO_ID == data.AMCO_Id && t.AMB_ID == data.AMB_Id && t.AMSE_ID == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id && t.EME_Id == data.EME_Id
                    && t.ISMS_Id == data.ISMS_Id).Distinct().ToList();

                    List<College_temp_marks_DTO> saved_studentList = new List<College_temp_marks_DTO>();

                    saved_studentList = (from a in studentList
                                         from b in stu_marks
                                         where (a.AMCST_Id == b.AMCST_Id && b.ISMS_Id == a.ISMS_Id && stu_list_mapped.Contains(a.AMCST_Id))
                                         select new College_temp_marks_DTO
                                         {
                                             AMCST_Id = a.AMCST_Id,
                                             AMCST_FirstName = a.AMCST_FirstName,
                                             AMCST_AdmNo = a.AMCST_AdmNo,
                                             AMCST_RegistrationNo = a.AMCST_RegistrationNo == null ? "" : a.AMCST_RegistrationNo,
                                             ACYST_RollNo = a.ACYST_RollNo,
                                             ISMS_Id = a.ISMS_Id,
                                             ISMS_SubjectName = a.ISMS_SubjectName,
                                             ECYSES_MaxMarks = a.ECYSES_MaxMarks,
                                             ECYSES_MarksEntryMax = a.ECYSES_MarksEntryMax,
                                             ECYSES_MinMarks = a.ECYSES_MinMarks,
                                             ECSTM_Marks = Convert.ToDecimal(b.ECSTM_Marks),
                                             ECSTM_Grade = b.ECSTM_Grade,
                                             ECSTM_Flg = b.ECSTM_Flg,
                                             ECSTM_Id = b.ECSTM_Id
                                         }).Distinct().OrderBy(t => order).ToList();

                    var propertyInfo1 = typeof(College_temp_marks_DTO).GetProperty(order);
                    saved_studentList = saved_studentList.OrderBy(x => propertyInfo1.GetValue(x, null)).ToList();

                    data.saved_studentList = saved_studentList.Distinct().ToArray();
                }

                else if (alrdy_stu_count > 0 && data.ECYSES_SubSubjectFlg && data.ECYSES_SubExamFlg)
                {
                    var stu_marks = _examcontext.Exm_Col_Student_MarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                 && t.AMCO_ID == data.AMCO_Id && t.AMB_ID == data.AMB_Id && t.AMSE_ID == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id && t.EME_Id == data.EME_Id
                 && t.ISMS_Id == data.ISMS_Id).Distinct().ToList();

                    List<College_temp_marks_DTO> saved_studentList = new List<College_temp_marks_DTO>();

                    saved_studentList = (from a in studentList
                                         from b in stu_marks
                                         where (a.AMCST_Id == b.AMCST_Id && b.ISMS_Id == a.ISMS_Id && stu_list_mapped.Contains(a.AMCST_Id))
                                         select new College_temp_marks_DTO
                                         {
                                             AMCST_Id = a.AMCST_Id,
                                             AMCST_FirstName = a.AMCST_FirstName,
                                             AMCST_AdmNo = a.AMCST_AdmNo,
                                             AMCST_RegistrationNo = a.AMCST_RegistrationNo == null ? "" : a.AMCST_RegistrationNo,
                                             ACYST_RollNo = a.ACYST_RollNo,
                                             ISMS_Id = a.ISMS_Id,
                                             ISMS_SubjectName = a.ISMS_SubjectName,
                                             ECYSES_MaxMarks = a.ECYSES_MaxMarks,
                                             ECYSES_MarksEntryMax = a.ECYSES_MarksEntryMax,
                                             ECYSES_MinMarks = a.ECYSES_MinMarks,
                                             ECSTM_Marks = Convert.ToDecimal(b.ECSTM_Marks),
                                             ECSTM_Grade = b.ECSTM_Grade,
                                             ECSTM_Flg = b.ECSTM_Flg,
                                             ECSTM_Id = b.ECSTM_Id
                                         }).Distinct().OrderBy(t => order).ToList();

                    var propertyInfo2 = typeof(College_temp_marks_DTO).GetProperty(order);
                    saved_studentList = saved_studentList.OrderBy(x => propertyInfo2.GetValue(x, null)).ToList();

                    data.saved_studentList = saved_studentList.Distinct().ToArray();

                    data.saved_ssse_list = (from a in _examcontext.Exm_Col_Student_Marks_SubSubjectDMO
                                            from b in stu_marks
                                            where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ECSTM_Id == b.ECSTM_Id)
                                            select a).Distinct().ToArray();
                }

                else if (alrdy_stu_count > 0 && data.ECYSES_SubSubjectFlg && !data.ECYSES_SubExamFlg)
                {
                    var stu_marks = _examcontext.Exm_Col_Student_MarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
          && t.AMCO_ID == data.AMCO_Id && t.AMB_ID == data.AMB_Id && t.AMSE_ID == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id && t.EME_Id == data.EME_Id
          && t.ISMS_Id == data.ISMS_Id).Distinct().ToList();

                    List<College_temp_marks_DTO> saved_studentList = new List<College_temp_marks_DTO>();

                    saved_studentList = (from a in studentList
                                         from b in stu_marks
                                         where (a.AMCST_Id == b.AMCST_Id && b.ISMS_Id == a.ISMS_Id && stu_list_mapped.Contains(a.AMCST_Id))
                                         select new College_temp_marks_DTO
                                         {
                                             AMCST_Id = a.AMCST_Id,
                                             AMCST_FirstName = a.AMCST_FirstName,
                                             AMCST_AdmNo = a.AMCST_AdmNo,
                                             AMCST_RegistrationNo = a.AMCST_RegistrationNo == null ? "" : a.AMCST_RegistrationNo,
                                             ACYST_RollNo = a.ACYST_RollNo,
                                             ISMS_Id = a.ISMS_Id,
                                             ISMS_SubjectName = a.ISMS_SubjectName,
                                             ECYSES_MaxMarks = a.ECYSES_MaxMarks,
                                             ECYSES_MarksEntryMax = a.ECYSES_MarksEntryMax,
                                             ECYSES_MinMarks = a.ECYSES_MinMarks,
                                             ECSTM_Marks = Convert.ToDecimal(b.ECSTM_Marks),
                                             ECSTM_Grade = b.ECSTM_Grade,
                                             ECSTM_Flg = b.ECSTM_Flg,
                                             ECSTM_Id = b.ECSTM_Id
                                         }).Distinct().OrderBy(t => order).ToList();

                    var propertyInfo3 = typeof(College_temp_marks_DTO).GetProperty(order);
                    saved_studentList = saved_studentList.OrderBy(x => propertyInfo3.GetValue(x, null)).ToList();

                    data.saved_studentList = saved_studentList.Distinct().ToArray();
                    //sanjevv
                    if (data.message == "Mobile")
                    {
                        data.saved_ss_list = (from a in _examcontext.Exm_Col_Student_Marks_SubSubjectDMO
                                              from b in stu_marks
                                              where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ECSTM_Id == b.ECSTM_Id && a.EMSS_Id == data.EMSS_Id)
                                              select a).Distinct().ToArray();
                    }
                    else
                    {
                        data.saved_ss_list = (from a in _examcontext.Exm_Col_Student_Marks_SubSubjectDMO
                                              from b in stu_marks
                                              where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ECSTM_Id == b.ECSTM_Id)
                                              select a).Distinct().ToArray();
                    }

                }

                else if (alrdy_stu_count > 0 && data.ECYSES_SubExamFlg && !data.ECYSES_SubSubjectFlg)
                {
                    var stu_marks = _examcontext.Exm_Col_Student_MarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCO_ID == data.AMCO_Id
                    && t.AMB_ID == data.AMB_Id && t.AMSE_ID == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id).Distinct().ToList();

                    List<College_temp_marks_DTO> saved_studentList = new List<College_temp_marks_DTO>();

                    saved_studentList = (from a in studentList
                                         from b in stu_marks
                                         where (a.AMCST_Id == b.AMCST_Id && b.ISMS_Id == a.ISMS_Id && stu_list_mapped.Contains(a.AMCST_Id))
                                         select new College_temp_marks_DTO
                                         {
                                             AMCST_Id = a.AMCST_Id,
                                             AMCST_FirstName = a.AMCST_FirstName,
                                             AMCST_AdmNo = a.AMCST_AdmNo,
                                             AMCST_RegistrationNo = a.AMCST_RegistrationNo == null ? "" : a.AMCST_RegistrationNo,
                                             ACYST_RollNo = a.ACYST_RollNo,
                                             ISMS_Id = a.ISMS_Id,
                                             ISMS_SubjectName = a.ISMS_SubjectName,
                                             ECYSES_MaxMarks = a.ECYSES_MaxMarks,
                                             ECYSES_MarksEntryMax = a.ECYSES_MarksEntryMax,
                                             ECYSES_MinMarks = a.ECYSES_MinMarks,
                                             ECSTM_Marks = Convert.ToDecimal(b.ECSTM_Marks),
                                             ECSTM_Grade = b.ECSTM_Grade,
                                             ECSTM_Flg = b.ECSTM_Flg,
                                             ECSTM_Id = b.ECSTM_Id
                                         }).Distinct().OrderBy(t => order).ToList();

                    var propertyInfo3 = typeof(College_temp_marks_DTO).GetProperty(order);
                    saved_studentList = saved_studentList.OrderBy(x => propertyInfo3.GetValue(x, null)).ToList();


                    data.saved_studentList = saved_studentList.Distinct().ToArray();
                    //sanjevv
                    if (data.message == "Mobile")
                    {
                        data.saved_se_list = (from a in _examcontext.Exm_Col_Student_Marks_SubSubjectDMO
                                              from b in stu_marks
                                              where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ECSTM_Id == b.ECSTM_Id && a.EMSE_Id == data.EMSE_Id)
                                              select a).Distinct().ToArray();
                    }
                    else
                    {
                        data.saved_se_list = (from a in _examcontext.Exm_Col_Student_Marks_SubSubjectDMO
                                              from b in stu_marks
                                              where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ECSTM_Id == b.ECSTM_Id)
                                              select a).Distinct().ToArray();
                    }


                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
                _logger.LogDebug(ee.Message);

            }
            return data;
        }

        //onsearchMobile
    }
}
