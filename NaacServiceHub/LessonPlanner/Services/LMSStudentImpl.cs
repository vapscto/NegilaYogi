using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using PreadmissionDTOs.NAAC.LessonPlanner;

namespace NaacServiceHub.LessonPlanner.Services
{
    public class LMSStudentImpl : Interface.LMSStudentInterface
    {
        public LessonplannerContext _context;

        public LMSStudentImpl(LessonplannerContext _dbcontext)
        {
            _context = _dbcontext;
        }

        // College
        public LMSStudentDTO Getdetails(LMSStudentDTO data)
        {
            try
            {
                data.getallyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
                data.getcurrentyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_Id).OrderByDescending(a => a.ASMAY_Order).ToArray();

                var getamcstid = _context.CollegeStudentlogin.Where(a => a.IVRMUL_Id == data.Userid && a.IVRMULSPGC_ActiveFlag == true
                && a.IVRMULSPGC_Flag == "S").ToList();

                if (getamcstid.Count > 0)
                {
                    data.AMCST_Id = getamcstid.FirstOrDefault().AMCST_Id;
                }

                var getstudentdetails = (from a in _context.Adm_College_Yearly_StudentDMO
                                         from b in _context.Adm_Master_College_StudentDMO
                                         from c in _context.AcademicYear
                                         from d in _context.MasterCourseDMO
                                         from e in _context.ClgMasterBranchDMO
                                         from f in _context.CLG_Adm_Master_SemesterDMO
                                         from g in _context.Adm_College_Master_SectionDMO
                                         where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == c.ASMAY_Id && a.AMCO_Id == d.AMCO_Id && a.AMB_Id == e.AMB_Id
                                         && a.AMSE_Id == f.AMSE_Id && a.ACMS_Id == g.ACMS_Id && a.ACYST_ActiveFlag == 1 && b.AMCST_SOL == "S"
                                         && b.AMCST_ActiveFlag == true && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id)
                                         select new LMSStudentDTO
                                         {
                                             AMCST_Id = a.AMCST_Id,
                                             AMCO_Id = a.AMCO_Id,
                                             AMB_Id = a.AMB_Id,
                                             AMSE_Id = a.AMSE_Id,
                                             ACMS_Id = a.ACMS_Id,
                                             ASMAY_Id = a.ASMAY_Id
                                         }).Distinct().ToList();

                data.getstudentdetails = getstudentdetails.ToArray();

                if (getstudentdetails.Count > 0)
                {
                    data.AMSE_Id = getstudentdetails.FirstOrDefault().AMSE_Id;
                    data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;
                    data.AMCO_Id = getstudentdetails.FirstOrDefault().AMCO_Id;
                    data.AMB_Id = getstudentdetails.FirstOrDefault().AMB_Id;


                    data.getcurrentsemesterdetails = _context.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true 
                    && a.AMSE_Id == data.AMSE_Id).OrderBy(a => a.AMSE_SEMOrder).ToArray();

                    var checksubjects = _context.Exm_Col_Studentwise_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id
                    && a.AMSE_Id == data.AMSE_Id && a.ECSTSU_ActiveFlg == true && a.ASMAY_Id == data.ASMAY_Id && a.AMB_Id == data.AMB_Id
                    && a.AMCO_Id == data.AMCO_Id).ToList();

                    if (checksubjects.Count > 0)
                    {
                        List<long> subjid = new List<long>();

                        foreach (var c in checksubjects)
                        {
                            subjid.Add(c.ISMS_Id);
                        }

                        data.getsubjectdetails = _context.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && subjid.Contains(a.ISMS_Id)
                        && a.ISMS_ActiveFlag == 1).OrderBy(a => a.ISMS_OrderFlag).ToArray();
                    }
                    else
                    {
                        data.getsubjectdetails = checksubjects.ToArray();
                    }
                }

                var getstudentdetails_all = (from a in _context.Adm_College_Yearly_StudentDMO
                                             from b in _context.Adm_Master_College_StudentDMO
                                             from c in _context.AcademicYear
                                             from d in _context.MasterCourseDMO
                                             from e in _context.ClgMasterBranchDMO
                                             from f in _context.CLG_Adm_Master_SemesterDMO
                                             from g in _context.Adm_College_Master_SectionDMO
                                             where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == c.ASMAY_Id && a.AMCO_Id == d.AMCO_Id && a.AMB_Id == e.AMB_Id
                                             && a.AMSE_Id == f.AMSE_Id && a.ACMS_Id == g.ACMS_Id && b.AMCST_SOL == "S"
                                             && b.AMCST_ActiveFlag == true && b.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id)
                                             select new LMSStudentDTO
                                             {
                                                 AMSE_Id = a.AMSE_Id,
                                             }).Distinct().ToList();


                List<long> semid = new List<long>();
                foreach (var c in getstudentdetails_all)
                {
                    semid.Add(c.AMSE_Id);
                }

                data.getsemesterdetails = _context.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true
                && semid.Contains(a.AMSE_Id)).OrderBy(a => a.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LMSStudentDTO onchangesemester(LMSStudentDTO data)
        {
            try
            {
                var getamcstid = _context.CollegeStudentlogin.Where(a => a.IVRMUL_Id == data.Userid && a.IVRMULSPGC_ActiveFlag == true
                 && a.IVRMULSPGC_Flag == "S").ToList();

                if (getamcstid.Count > 0)
                {
                    data.AMCST_Id = getamcstid.FirstOrDefault().AMCST_Id;
                }

                var getstudentdetails = (from a in _context.Adm_College_Yearly_StudentDMO
                                         from b in _context.Adm_Master_College_StudentDMO
                                         from c in _context.AcademicYear
                                         from d in _context.MasterCourseDMO
                                         from e in _context.ClgMasterBranchDMO
                                         from f in _context.CLG_Adm_Master_SemesterDMO
                                         from g in _context.Adm_College_Master_SectionDMO
                                         where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == c.ASMAY_Id && a.AMCO_Id == d.AMCO_Id && a.AMB_Id == e.AMB_Id
                                         && a.AMSE_Id == f.AMSE_Id && a.ACMS_Id == g.ACMS_Id && b.AMCST_SOL == "S"
                                         && b.AMCST_ActiveFlag == true && b.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && a.AMSE_Id == data.AMSE_Id)
                                         select new LMSStudentDTO
                                         {
                                             AMCST_Id = a.AMCST_Id,
                                             AMCO_Id = a.AMCO_Id,
                                             AMB_Id = a.AMB_Id,
                                             AMSE_Id = a.AMSE_Id,
                                             ACMS_Id = a.ACMS_Id,
                                             ASMAY_Id = a.ASMAY_Id
                                         }).Distinct().ToList();

                data.getstudentdetails = getstudentdetails.ToArray();

                if (getstudentdetails.Count > 0)
                {
                    data.AMSE_Id = getstudentdetails.FirstOrDefault().AMSE_Id;
                    data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;
                    data.AMCO_Id = getstudentdetails.FirstOrDefault().AMCO_Id;
                    data.AMB_Id = getstudentdetails.FirstOrDefault().AMB_Id;

                    var checksubjects = _context.Exm_Col_Studentwise_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id
                    && a.AMSE_Id == data.AMSE_Id && a.ECSTSU_ActiveFlg == true && a.ASMAY_Id == data.ASMAY_Id && a.AMB_Id == data.AMB_Id
                    && a.AMCO_Id == data.AMCO_Id).ToList();

                    if (checksubjects.Count > 0)
                    {
                        List<long> subjid = new List<long>();

                        foreach (var c in checksubjects)
                        {
                            subjid.Add(c.ISMS_Id);
                        }

                        data.getsubjectdetails = _context.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && subjid.Contains(a.ISMS_Id)
                        && a.ISMS_ActiveFlag == 1).OrderBy(a => a.ISMS_OrderFlag).ToArray();
                    }
                    else
                    {
                        data.getsubjectdetails = checksubjects.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LMSStudentDTO getcollegetopics(LMSStudentDTO data)
        {
            try
            {
                var getamcstid = _context.CollegeStudentlogin.Where(a => a.IVRMUL_Id == data.Userid && a.IVRMULSPGC_ActiveFlag == true
                 && a.IVRMULSPGC_Flag == "S").ToList();

                if (getamcstid.Count > 0)
                {
                    data.AMCST_Id = getamcstid.FirstOrDefault().AMCST_Id;
                }

                var getstudentdetails = (from a in _context.Adm_College_Yearly_StudentDMO
                                         from b in _context.Adm_Master_College_StudentDMO
                                         from c in _context.AcademicYear
                                         from d in _context.MasterCourseDMO
                                         from e in _context.ClgMasterBranchDMO
                                         from f in _context.CLG_Adm_Master_SemesterDMO
                                         from g in _context.Adm_College_Master_SectionDMO
                                         where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == c.ASMAY_Id && a.AMCO_Id == d.AMCO_Id && a.AMB_Id == e.AMB_Id
                                         && a.AMSE_Id == f.AMSE_Id && a.ACMS_Id == g.ACMS_Id && b.AMCST_SOL == "S"
                                         && b.AMCST_ActiveFlag == true && b.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && a.AMSE_Id == data.AMSE_Id)
                                         select new LMSStudentDTO
                                         {
                                             AMCST_Id = a.AMCST_Id,
                                             AMCO_Id = a.AMCO_Id,
                                             AMB_Id = a.AMB_Id,
                                             AMSE_Id = a.AMSE_Id,
                                             ACMS_Id = a.ACMS_Id,
                                             ASMAY_Id = a.ASMAY_Id
                                         }).Distinct().ToList();

                data.getstudentdetails = getstudentdetails.ToArray();

                if (getstudentdetails.Count > 0)
                {
                    data.AMSE_Id = getstudentdetails.FirstOrDefault().AMSE_Id;
                    data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;
                    data.AMCO_Id = getstudentdetails.FirstOrDefault().AMCO_Id;
                    data.AMB_Id = getstudentdetails.FirstOrDefault().AMB_Id;

                    data.getunitdetails = (from a in _context.SchoolMasterUnitDMO
                                           from b in _context.LP_Master_MainTopic_CollegeDMO
                                           from c in _context.IVRM_School_Master_SubjectsDMO
                                           where (a.LPMU_Id == b.LPMU_Id && b.ISMS_Id == c.ISMS_Id && a.LPMU_ActiveFlag == true && b.LPMMTC_ActiveFlg == true
                                           && c.ISMS_ActiveFlag == 1 && b.ISMS_Id == data.ISMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                           && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id && b.ASMAY_Id == data.ASMAY_Id)
                                           select new LMSStudentDTO
                                           {
                                               LPMU_Id = a.LPMU_Id,
                                               LPMU_UnitName = a.LPMU_UnitName,
                                               LPMU_UnitDescription = a.LPMU_UnitDescription,
                                               LPMU_Order = a.LPMU_Order,
                                               ISMS_Id = b.ISMS_Id

                                           }).Distinct().OrderBy(a => a.LPMU_Order).ToArray();

                    data.gettopiclist = (from a in _context.SchoolMasterUnitDMO
                                         from b in _context.LP_Master_MainTopic_CollegeDMO
                                         from c in _context.IVRM_School_Master_SubjectsDMO
                                         where (a.LPMU_Id == b.LPMU_Id && b.ISMS_Id == c.ISMS_Id && a.LPMU_ActiveFlag == true && b.LPMMTC_ActiveFlg == true
                                         && c.ISMS_ActiveFlag == 1 && b.ISMS_Id == data.ISMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                         && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id && b.ASMAY_Id == data.ASMAY_Id)
                                         select new LMSStudentDTO
                                         {
                                             LPMU_Id = a.LPMU_Id,
                                             LPMMT_Id = b.LPMMTC_Id,
                                             LPMMT_TopicName = b.LPMMTC_TopicName,
                                             LPMMT_TopicDescription = b.LPMMTC_TopicDescription,
                                             LPMMT_Order = b.LPMMTC_Order,
                                             ISMS_Id = b.ISMS_Id

                                         }).Distinct().OrderBy(a => a.LPMMT_Order).ToArray();

                    data.getsubtopicdetails = (from a in _context.SchoolMasterUnitDMO
                                               from b in _context.LP_Master_MainTopic_CollegeDMO
                                               from c in _context.IVRM_School_Master_SubjectsDMO
                                               from d in _context.LP_Master_Topic_CollegeDMO
                                               where (a.LPMU_Id == b.LPMU_Id && b.ISMS_Id == c.ISMS_Id && d.LPMMTC_Id == b.LPMMTC_Id && a.LPMU_ActiveFlag == true
                                               && b.LPMMTC_ActiveFlg == true && d.LPMTC_Activefalg == true && d.MI_Id == data.MI_Id && c.ISMS_ActiveFlag == 1
                                               && b.ISMS_Id == data.ISMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                               && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id && b.ASMAY_Id == data.ASMAY_Id)
                                               select new LMSStudentDTO
                                               {
                                                   LPMU_Id = a.LPMU_Id,
                                                   LPMMT_Id = b.LPMMTC_Id,
                                                   LPMT_Id = d.LPMTC_Id,
                                                   LPMT_TopicName = d.LPMTC_TopicName,
                                                   LPMT_LessonPlan = d.LPMTC_LessonPlan,
                                                   LPMT_TopicOrder = d.LPMTC_TopicOrder,
                                                   ISMS_Id = b.ISMS_Id
                                               }).Distinct().OrderBy(a => a.LPMT_TopicOrder).ToArray();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LMSStudentDTO getcollegedocuments(LMSStudentDTO data)
        {
            try
            {
                var getamcstid = _context.CollegeStudentlogin.Where(a => a.IVRMUL_Id == data.Userid && a.IVRMULSPGC_ActiveFlag == true
                 && a.IVRMULSPGC_Flag == "S").ToList();

                if (getamcstid.Count > 0)
                {
                    data.AMCST_Id = getamcstid.FirstOrDefault().AMCST_Id;
                }

                var getstudentdetails = (from a in _context.Adm_College_Yearly_StudentDMO
                                         from b in _context.Adm_Master_College_StudentDMO
                                         from c in _context.AcademicYear
                                         from d in _context.MasterCourseDMO
                                         from e in _context.ClgMasterBranchDMO
                                         from f in _context.CLG_Adm_Master_SemesterDMO
                                         from g in _context.Adm_College_Master_SectionDMO
                                         where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == c.ASMAY_Id && a.AMCO_Id == d.AMCO_Id && a.AMB_Id == e.AMB_Id
                                         && a.AMSE_Id == f.AMSE_Id && a.ACMS_Id == g.ACMS_Id && b.AMCST_SOL == "S"
                                         && b.AMCST_ActiveFlag == true && b.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && a.AMSE_Id == data.AMSE_Id)
                                         select new LMSStudentDTO
                                         {
                                             AMCST_Id = a.AMCST_Id,
                                             AMCO_Id = a.AMCO_Id,
                                             AMB_Id = a.AMB_Id,
                                             AMSE_Id = a.AMSE_Id,
                                             ACMS_Id = a.ACMS_Id,
                                             ASMAY_Id = a.ASMAY_Id
                                         }).Distinct().ToList();

                data.getstudentdetails = getstudentdetails.ToArray();

                if (getstudentdetails.Count > 0)
                {
                    data.AMSE_Id = getstudentdetails.FirstOrDefault().AMSE_Id;
                    data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;
                    data.AMCO_Id = getstudentdetails.FirstOrDefault().AMCO_Id;
                    data.AMB_Id = getstudentdetails.FirstOrDefault().AMB_Id;

                    data.getdocumentlist = (from a in _context.SchoolMasterUnitDMO
                                            from b in _context.LP_Master_MainTopic_CollegeDMO
                                            from c in _context.IVRM_School_Master_SubjectsDMO
                                            from d in _context.LP_Master_Topic_CollegeDMO
                                            from e in _context.LP_Master_Topic_Resources_CollegeDMO
                                            where (a.LPMU_Id == b.LPMU_Id && b.ISMS_Id == c.ISMS_Id && d.LPMMTC_Id == b.LPMMTC_Id && e.LPMTC_Id == d.LPMTC_Id
                                            && a.LPMU_ActiveFlag == true && b.LPMMTC_ActiveFlg == true && d.LPMTC_Activefalg == true && d.MI_Id == data.MI_Id
                                            && c.ISMS_ActiveFlag == 1 && b.ISMS_Id == data.ISMS_Id && b.ASMAY_Id == data.ASMAY_Id
                                            && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id && b.ASMAY_Id == data.ASMAY_Id
                                            && a.MI_Id == data.MI_Id && d.LPMTC_Id == data.LPMT_Id & e.LPMTRC_ResourceType == "Student Guide")
                                            select new LMSStudentDTO
                                            {
                                                LPMTR_Resources = e.LPMTRC_Resources,
                                                LPMTR_FileName = e.LPMTRC_FileName,
                                                LPMTR_ResourceType = e.LPMTRC_ResourceType
                                            }).Distinct().ToArray();

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // School  
        public LMSStudentDTO Getdetailsschool(LMSStudentDTO data)
        {
            try
            {
                data.getallyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.getcurrentyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_Id).OrderByDescending(a => a.ASMAY_Order).ToArray();

                var getamcstid = _context.StudentAppUserLoginDMO.Where(a => a.STD_APP_ID == data.Userid).ToList();

                if (getamcstid.Count > 0)
                {
                    data.AMST_Id = getamcstid.FirstOrDefault().AMST_ID;
                }

                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         from c in _context.AcademicYear
                                         from d in _context.AdmissionClass
                                         from e in _context.School_M_Section
                                         where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                         && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                         && a.AMST_Id == data.AMST_Id)
                                         select new LMSStudentDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                             ASMAY_Id = a.ASMAY_Id
                                         }).Distinct().ToList();

                data.getstudentdetails = getstudentdetails.ToArray();

                if (getstudentdetails.Count > 0)
                {
                    data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                    data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;

                    data.getcurrentclass = _context.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true
                    && a.ASMCL_Id == data.ASMCL_Id).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();

                    var checksubjects = _context.StudentMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id
                    && a.ASMAY_Id == data.ASMAY_Id && a.ESTSU_ActiveFlg == true && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id).ToList();

                    if (checksubjects.Count > 0)
                    {
                        List<long> subjid = new List<long>();

                        foreach (var c in checksubjects)
                        {
                            subjid.Add(c.ISMS_Id);
                        }

                        data.getsubjectdetails = _context.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && subjid.Contains(a.ISMS_Id)
                        && a.ISMS_ActiveFlag == 1).OrderBy(a => a.ISMS_OrderFlag).ToArray();
                    }
                    else
                    {
                        data.getsubjectdetails = checksubjects.ToArray();
                    }
                }

                var getstudentdetails_all = (from a in _context.School_Adm_Y_StudentDMO
                                             from b in _context.Adm_M_Student
                                             from c in _context.AcademicYear
                                             from d in _context.AdmissionClass
                                             from e in _context.School_M_Section
                                             where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                             && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && b.MI_Id == data.MI_Id
                                             && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id)
                                             select new LMSStudentDTO
                                             {
                                                 ASMCL_Id = a.ASMCL_Id
                                             }).Distinct().ToList();


                List<long> clsid = new List<long>();
                foreach (var c in getstudentdetails_all)
                {
                    clsid.Add(c.ASMCL_Id);
                }

                data.getclass = _context.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true && clsid.Contains(a.ASMCL_Id)).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LMSStudentDTO onchangeyear(LMSStudentDTO data)
        {
            try
            {
                var getamcstid = _context.StudentAppUserLoginDMO.Where(a => a.STD_APP_ID == data.Userid).ToList();

                if (getamcstid.Count > 0)
                {
                    data.AMST_Id = getamcstid.FirstOrDefault().AMST_ID;
                }

                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         from c in _context.AcademicYear
                                         from d in _context.AdmissionClass
                                         from e in _context.School_M_Section
                                         where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                         && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                         && a.AMST_Id == data.AMST_Id)
                                         select new LMSStudentDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                             ASMAY_Id = a.ASMAY_Id
                                         }).Distinct().ToList();

                data.getstudentdetails = getstudentdetails.ToArray();

                if (getstudentdetails.Count > 0)
                {
                    data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                    data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;

                    var checksubjects = _context.StudentMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ESTSU_ActiveFlg == true && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id).ToList();

                    if (checksubjects.Count > 0)
                    {
                        List<long> subjid = new List<long>();

                        foreach (var c in checksubjects)
                        {
                            subjid.Add(c.ISMS_Id);
                        }

                        data.getsubjectdetails = _context.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && subjid.Contains(a.ISMS_Id)
                        && a.ISMS_ActiveFlag == 1).OrderBy(a => a.ISMS_OrderFlag).ToArray();
                    }
                    else
                    {
                        data.getsubjectdetails = checksubjects.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LMSStudentDTO onchangeclass(LMSStudentDTO data)
        {
            try
            {
                var getamcstid = _context.StudentAppUserLoginDMO.Where(a => a.STD_APP_ID == data.Userid).ToList();

                if (getamcstid.Count > 0)
                {
                    data.AMST_Id = getamcstid.FirstOrDefault().AMST_ID;
                }

                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         from c in _context.AcademicYear
                                         from d in _context.AdmissionClass
                                         from e in _context.School_M_Section
                                         where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                         && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && b.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.AMST_Id == data.AMST_Id)
                                         select new LMSStudentDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                             ASMAY_Id = a.ASMAY_Id,
                                             ASMAY_Order = c.ASMAY_Order
                                         }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToList();

                data.getstudentdetails = getstudentdetails.ToArray();

                if (getstudentdetails.Count > 0)
                {
                    data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                    data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;
                    data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;

                    var checksubjects = _context.StudentMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ESTSU_ActiveFlg == true && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id).ToList();

                    if (checksubjects.Count > 0)
                    {
                        List<long> subjid = new List<long>();

                        foreach (var c in checksubjects)
                        {
                            subjid.Add(c.ISMS_Id);
                        }

                        data.getsubjectdetails = _context.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && subjid.Contains(a.ISMS_Id)
                        && a.ISMS_ActiveFlag == 1).OrderBy(a => a.ISMS_OrderFlag).ToArray();
                    }
                    else
                    {
                        data.getsubjectdetails = checksubjects.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LMSStudentDTO getschooltopics(LMSStudentDTO data)
        {
            try
            {
                var getamcstid = _context.StudentAppUserLoginDMO.Where(a => a.STD_APP_ID == data.Userid).ToList();

                if (getamcstid.Count > 0)
                {
                    data.AMST_Id = getamcstid.FirstOrDefault().AMST_ID;
                }

                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         from c in _context.AcademicYear
                                         from d in _context.AdmissionClass
                                         from e in _context.School_M_Section
                                         where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                         && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && b.MI_Id == data.MI_Id
                                         && a.ASMCL_Id == data.ASMCL_Id && a.AMST_Id == data.AMST_Id)
                                         select new LMSStudentDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                             ASMAY_Id = a.ASMAY_Id,
                                             ASMAY_Order = c.ASMAY_Order
                                         }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToList();

                data.getstudentdetails = getstudentdetails.ToArray();

                if (getstudentdetails.Count > 0)
                {
                    data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                    data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;
                    data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;

                    data.getunitdetails = (from a in _context.SchoolMasterUnitDMO
                                           from b in _context.MasterSchoolTopicDMO
                                           from c in _context.IVRM_School_Master_SubjectsDMO
                                           where (a.LPMU_Id == b.LPMU_Id && b.ISMS_Id == c.ISMS_Id && a.LPMU_ActiveFlag == true && b.LPMMT_ActiveFlag == true
                                           && c.ISMS_ActiveFlag == 1 && b.ISMS_Id == data.ISMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                           && b.ASMCL_Id == data.ASMCL_Id)
                                           select new LMSStudentDTO
                                           {
                                               LPMU_Id = a.LPMU_Id,
                                               LPMU_UnitName = a.LPMU_UnitName,
                                               LPMU_UnitDescription = a.LPMU_UnitDescription,
                                               LPMU_Order = a.LPMU_Order,
                                               ISMS_Id = b.ISMS_Id

                                           }).Distinct().OrderBy(a => a.LPMU_Order).ToArray();

                    data.gettopiclist = (from a in _context.SchoolMasterUnitDMO
                                         from b in _context.MasterSchoolTopicDMO
                                         from c in _context.IVRM_School_Master_SubjectsDMO
                                         where (a.LPMU_Id == b.LPMU_Id && b.ISMS_Id == c.ISMS_Id && a.LPMU_ActiveFlag == true && b.LPMMT_ActiveFlag == true
                                         && c.ISMS_ActiveFlag == 1 && b.ISMS_Id == data.ISMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                           && b.ASMCL_Id == data.ASMCL_Id)
                                         select new LMSStudentDTO
                                         {
                                             LPMU_Id = a.LPMU_Id,
                                             LPMMT_Id = b.LPMMT_Id,
                                             LPMMT_TopicName = b.LPMMT_TopicName,
                                             LPMMT_TopicDescription = b.LPMMT_TopicDescription,
                                             LPMMT_Order = b.LPMMT_Order,
                                             ISMS_Id = b.ISMS_Id

                                         }).Distinct().OrderBy(a => a.LPMMT_Order).ToArray();

                    data.getsubtopicdetails = (from a in _context.SchoolMasterUnitDMO
                                               from b in _context.MasterSchoolTopicDMO
                                               from c in _context.IVRM_School_Master_SubjectsDMO
                                               from d in _context.SchoolSubjectWithMasterTopicMapping
                                               where (a.LPMU_Id == b.LPMU_Id && b.ISMS_Id == c.ISMS_Id && d.LPMMT_Id == b.LPMMT_Id && a.LPMU_ActiveFlag == true
                                               && b.LPMMT_ActiveFlag == true && d.LPMT_ActiveFlag == true && d.MI_Id == data.MI_Id && c.ISMS_ActiveFlag == 1
                                               && b.ISMS_Id == data.ISMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id)
                                               select new LMSStudentDTO
                                               {
                                                   LPMU_Id = a.LPMU_Id,
                                                   LPMMT_Id = b.LPMMT_Id,
                                                   LPMT_Id = d.LPMT_Id,
                                                   LPMT_TopicName = d.LPMT_TopicName,
                                                   LPMT_LessonPlan = d.LPMT_LessonPlan,
                                                   LPMT_TopicOrder = d.LPMT_TopicOrder,
                                                   ISMS_Id = b.ISMS_Id
                                               }).Distinct().OrderBy(a => a.LPMT_TopicOrder).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LMSStudentDTO getschooldocuments(LMSStudentDTO data)
        {
            try
            {
                var getamcstid = _context.StudentAppUserLoginDMO.Where(a => a.STD_APP_ID == data.Userid).ToList();

                if (getamcstid.Count > 0)
                {
                    data.AMST_Id = getamcstid.FirstOrDefault().AMST_ID;
                }

                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         from c in _context.AcademicYear
                                         from d in _context.AdmissionClass
                                         from e in _context.School_M_Section
                                         where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                         && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && b.MI_Id == data.MI_Id
                                         && a.ASMCL_Id == data.ASMCL_Id && a.AMST_Id == data.AMST_Id)
                                         select new LMSStudentDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                             ASMAY_Id = a.ASMAY_Id,
                                             ASMAY_Order = c.ASMAY_Order
                                         }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToList();

                data.getstudentdetails = getstudentdetails.ToArray();

                if (getstudentdetails.Count > 0)
                {
                    data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                    data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;
                    data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;

                    data.getdocumentlist = (from a in _context.SchoolMasterUnitDMO
                                            from b in _context.MasterSchoolTopicDMO
                                            from c in _context.IVRM_School_Master_SubjectsDMO
                                            from d in _context.SchoolSubjectWithMasterTopicMapping
                                            from e in _context.School_Topic_Resource_MappingDMO
                                            where (a.LPMU_Id == b.LPMU_Id && b.ISMS_Id == c.ISMS_Id && d.LPMMT_Id == b.LPMMT_Id && e.LPMT_Id == d.LPMT_Id
                                            && a.LPMU_ActiveFlag == true && b.LPMMT_ActiveFlag == true && d.LPMT_ActiveFlag == true && d.MI_Id == data.MI_Id
                                            && c.ISMS_ActiveFlag == 1 && b.ISMS_Id == data.ISMS_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                                            && a.MI_Id == data.MI_Id && d.LPMT_Id == data.LPMT_Id & e.LPMTR_ResourceType == "Student Guide")
                                            select new LMSStudentDTO
                                            {
                                                LPMTR_Resources = e.LPMTR_Resources,
                                                LPMTR_FileName = e.LPMTR_FileName,
                                                LPMTR_ResourceType = e.LPMTR_ResourceType
                                            }).Distinct().ToArray();
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
