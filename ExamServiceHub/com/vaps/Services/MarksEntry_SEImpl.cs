using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.TT;
using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace ExamServiceHub.com.vaps.Services
{
    public class MarksEntry_SEImpl : MarksEntry_SEInterface
    {
        public ExamContext _examcontext;
        public DomainModelMsSqlServerContext _db;
        ILogger<MarksEntryImpl> _acdimpl;
        public MarksEntry_SEImpl(ExamContext ttcategory, DomainModelMsSqlServerContext db)
        {
            _examcontext = ttcategory;
            _db = db;
        }
        public MarksEntry_SEDTO getdetails(MarksEntry_SEDTO data)
        {
            //MarksEntry_SEDTO data = new MarksEntry_SEDTO();
            try
            {

                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _examcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.yearlist = list.Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public MarksEntry_SEDTO get_classes(MarksEntry_SEDTO data)
        {

            try
            {


                var classid = _examcontext.Masterclasscategory.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.Is_Active == true && t.ASMAY_Id == data.ASMAY_Id).Select(t => t.ASMCL_Id).ToArray();

                var classexmid = (from e in _examcontext.Staff_User_Login
                                  from f in _examcontext.Exm_Login_PrivilegeDMO
                                  from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                  where (e.Id == data.UserId && //data.MI_Id == data.MI_Id &&
                                    f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id && classid.Contains(i.ASMCL_Id)
                                    && f.ELP_Id == i.ELP_Id && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true)
                                  select new MarksEntry_SEDTO
                                  {
                                      ASMCL_Id = i.ASMCL_Id
                                  }).Distinct().Select(t => t.ASMCL_Id).ToArray();

                List<AdmissionClass> clist = new List<AdmissionClass>();
                clist = _examcontext.AdmissionClass.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true && classexmid.Contains(t.ASMCL_Id)).ToList();
                data.classlist = clist.Distinct().OrderBy(t=>t.ASMCL_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public MarksEntry_SEDTO get_sections(MarksEntry_SEDTO data)
        {
            // ExamMarksDTO data = new ExamMarksDTO();
            try
            {
                var classid = _examcontext.Masterclasscategory.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id).Select(t => t.ASMCC_Id).ToArray();

                var secid = _examcontext.AdmSchoolMasterClassCatSec.Where(t => classid.Contains(t.ASMCC_Id)).Select(t => t.ASMS_Id).ToArray();

                var sectionexamid = (from e in _examcontext.Staff_User_Login
                                     from f in _examcontext.Exm_Login_PrivilegeDMO
                                     from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                     where (e.Id == data.UserId && //data.MI_Id == data.MI_Id &&
                                       f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id && i.ASMCL_Id == data.ASMCL_Id && secid.Contains(i.ASMS_Id)
                                       && f.ELP_Id == i.ELP_Id && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true)
                                     select new MarksEntry_SEDTO
                                     {
                                         ASMS_Id = i.ASMS_Id
                                     }).Distinct().Select(t => t.ASMS_Id).ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = _examcontext.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1 && sectionexamid.Contains(t.ASMS_Id)).ToList();
                data.sectionlist = seclist.Distinct().OrderBy(t=>t.ASMC_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public MarksEntry_SEDTO get_exams(MarksEntry_SEDTO data)
        {
            //ExamMarksDTO data = new ExamMarksDTO();
            try
            {
                var catid = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.MI_Id == data.MI_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).ToArray();

                var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && catid.Contains(t.EMCA_Id) && t.EYC_ActiveFlg == true).Select(t => t.EYC_Id).ToArray();

                var emeid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id) && t.EYCE_ActiveFlg == true).Select(t => t.EME_Id).ToArray();

                List<exammasterDMO> examlist = new List<exammasterDMO>();
                examlist = _examcontext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && emeid.Contains(t.EME_Id)).ToList();
                data.examlist = examlist.Distinct().OrderBy(t=>t.EME_ExamOrder).ToArray();

                //if (data.examlist.Length == 1)
                //{
                //    var eyceid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id)).Select(t => t.EYCE_Id).ToArray();

                //    var subid = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id)).Select(t => t.ISMS_Id).ToArray();

                //    var sectionexamid = (from e in _examcontext.Staff_User_Login
                //                         from f in _examcontext.Exm_Login_PrivilegeDMO
                //                         from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                //                         where (e.Id == data.UserId && data.MI_Id == data.MI_Id &&
                //                           f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id && i.ASMS_Id == data.ASMS_Id && i.ASMCL_Id == data.ASMCL_Id && f.ELP_Id == i.ELP_Id && subid.Contains(i.ISMS_Id))
                //                         select new MarksEntry_SEDTO
                //                         {
                //                             ISMS_Id = i.ISMS_Id
                //                         }).Distinct().Select(t => t.ISMS_Id).ToArray();


                //    List<IVRM_School_Master_SubjectsDMO> subjects = new List<IVRM_School_Master_SubjectsDMO>();
                //    subjects = _examcontext.IVRM_School_Master_SubjectsDMO.Where(c => c.MI_Id == data.MI_Id && c.ISMS_ActiveFlag == 1 && c.ISMS_ExamFlag == 1 && sectionexamid.Contains(c.ISMS_Id)).ToList();
                //    data.subjectlist = subjects.ToArray();
                //}
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public MarksEntry_SEDTO get_subjects(MarksEntry_SEDTO data)
        {
            //ExamMarksDTO data = new ExamMarksDTO();
            try
            {
                var catid = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.MI_Id == data.MI_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).ToArray();

                var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && catid.Contains(t.EMCA_Id) && t.EYC_ActiveFlg == true).Select(t => t.EYC_Id).ToArray();

                var eyceid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id) && t.EYCE_ActiveFlg == true && t.EME_Id == data.EME_Id).Select(t => t.EYCE_Id).ToArray();

                var subid = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id) && t.EYCES_ActiveFlg == true).Select(t => t.ISMS_Id).ToArray();
                //Before Search ,For Getting Subjects Based Type 
                var subid_with_flags = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id) && t.EYCES_ActiveFlg == true && !t.EYCES_SubSubjectFlg && t.EYCES_SubExamFlg).Select(t => t.ISMS_Id).ToArray();

                var sectionexamid = (from e in _examcontext.Staff_User_Login
                                     from f in _examcontext.Exm_Login_PrivilegeDMO
                                     from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                     where (e.Id == data.UserId && e.MI_Id == data.MI_Id &&
                                       f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id && i.ASMS_Id == data.ASMS_Id && i.ASMCL_Id == data.ASMCL_Id && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true && f.ELP_Id == i.ELP_Id && subid_with_flags.Contains(i.ISMS_Id))//subid.Contains(i.ISMS_Id)
                                     select new MarksEntry_SEDTO
                                     {
                                         ISMS_Id = i.ISMS_Id
                                     }).Distinct().Select(t => t.ISMS_Id).ToArray();


                List<IVRM_School_Master_SubjectsDMO> subjects = new List<IVRM_School_Master_SubjectsDMO>();
                subjects = _examcontext.IVRM_School_Master_SubjectsDMO.Where(c => c.MI_Id == data.MI_Id && c.ISMS_ActiveFlag == 1 && c.ISMS_ExamFlag == 1 && sectionexamid.Contains(c.ISMS_Id)).ToList();
                data.subjectlist = subjects.OrderBy(t=>t.ISMS_OrderFlag).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public MarksEntry_SEDTO onsearch(MarksEntry_SEDTO data)
        {
            try
            {
                List<ExmStudentMarksProcessDMO> calculationid = new List<ExmStudentMarksProcessDMO>();
                calculationid = _examcontext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id).ToList();
                if (calculationid.Count > 0)
                {
                    data.marksdeleteflag = true;
                }
                else if (calculationid.Count == 0)
                {
                    List<ExmStudentMarksProcessSubjectwiseDMO> calculationSubWiseid = new List<ExmStudentMarksProcessSubjectwiseDMO>();
                    calculationSubWiseid = _examcontext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id).ToList();
                    if (calculationSubWiseid.Count > 0)
                    {
                        data.marksdeleteflag = true;
                    }
                    else if (calculationSubWiseid.Count == 0)
                    {
                        data.marksdeleteflag = false;
                    }

                }

                var subject_details = (from a in _examcontext.Exm_Category_ClassDMO
                                       from b in _examcontext.Exm_Yearly_CategoryDMO
                                       from c in _examcontext.Exm_Yearly_Category_ExamsDMO
                                       from d in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                       where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == a.EMCA_Id && b.EYC_ActiveFlg == true && c.EYC_Id == b.EYC_Id && c.EME_Id == data.EME_Id && c.EYCE_ActiveFlg == true && d.EYCE_Id == c.EYCE_Id && d.ISMS_Id == data.ISMS_Id && d.EYCES_ActiveFlg == true)
                                       select d).Distinct().ToList();
                data.subject_details = subject_details.ToArray();
                data.EYCES_MarksGradeEntryFlg = subject_details[0].EYCES_MarksGradeEntryFlg;

                data.EYCES_SubSubjectFlg = subject_details[0].EYCES_SubSubjectFlg;
                data.EYCES_SubExamFlg = subject_details[0].EYCES_SubExamFlg;

                if (data.EYCES_SubSubjectFlg)
                {
                    data.subject_subsubjects = (from a in _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                                from b in _examcontext.mastersubsubject
                                                where (b.MI_Id == data.MI_Id && b.EMSS_ActiveFlag == true && a.EYCES_Id == subject_details[0].EYCES_Id && b.EMSS_Id == a.EMSS_Id)
                                                select new ExamSubjectMappingDTO
                                                {
                                                    EYCESSS_Id = a.EYCESSS_Id,
                                                    EYCES_Id = a.EYCES_Id,
                                                    EMSS_Id = a.EMSS_Id,
                                                    EMGR_Id = a.EMGR_Id,
                                                    EYCESSS_MaxMarks = a.EYCESSS_MaxMarks,
                                                    EYCESSS_MinMarks = a.EYCESSS_MinMarks,
                                                    EYCESSS_ExemptedFlg = a.EYCESSS_ExemptedFlg,
                                                    EYCESSS_ExemptedPer = a.EYCESSS_ExemptedPer,
                                                    EYCESSS_ActiveFlg = a.EYCESSS_ActiveFlg,
                                                    EYCESSS_SubSubjectOrder = a.EYCESSS_SubSubjectOrder,
                                                    EMSS_SubSubjectName = b.EMSS_SubSubjectName,
                                                    EMSS_SubSubjectCode = b.EMSS_SubSubjectCode
                                                }).Distinct().ToArray();
                }
                if (data.EYCES_SubExamFlg)
                {
                    data.subject_subexams = (from a in _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO
                                             from b in _examcontext.mastersubexam
                                             where (b.MI_Id == data.MI_Id && b.EMSE_ActiveFlag == true && a.EYCES_Id == subject_details[0].EYCES_Id && b.EMSE_Id == a.EMSE_Id)
                                             select new ExamSubjectMappingDTO
                                             {
                                                 EYCESSE_Id = a.EYCESSE_Id,
                                                 EYCES_Id = a.EYCES_Id,
                                                 EMSE_Id = a.EMSE_Id,
                                                 EMGR_Id = a.EMGR_Id,
                                                 EYCESSE_MaxMarks = a.EYCESSE_MaxMarks,
                                                 EYCESSE_MinMarks = a.EYCESSE_MinMarks,
                                                 EYCESSE_ExemptedFlg = a.EYCESSE_ExemptedFlg,
                                                 EYCESSE_ExemptedPer = a.EYCESSE_ExemptedPer,
                                                 EYCESSE_ActiveFlg = a.EYCESSE_ActiveFlg,
                                                 EYCESSE_SubExamOrder = a.EYCESSE_SubExamOrder,
                                                 EMSE_SubExamName = b.EMSE_SubExamName,
                                                 EMSE_SubExamCode = b.EMSE_SubExamCode
                                             }).Distinct().ToArray();
                }

                data.EMGR_Id = subject_details[0].EMGR_Id;

                if (data.EYCES_MarksGradeEntryFlg == "G")
                {
                    data.grade_details = (from a in _examcontext.Exm_Master_GradeDMO
                                          from b in _examcontext.Exm_Master_Grade_DetailsDMO
                                          where (a.MI_Id == data.MI_Id && a.EMGR_Id == data.EMGR_Id && b.EMGR_Id == data.EMGR_Id && b.EMGD_ActiveFlag == true)
                                          select b).Select(t => t.EMGD_Name).Distinct().ToArray();
                    if (data.EYCES_SubSubjectFlg)
                    {
                        data.subsubject_gradedetails = (from a in _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                                        from b in _examcontext.Exm_Master_GradeDMO
                                                        from c in _examcontext.Exm_Master_Grade_DetailsDMO
                                                        where (a.EYCES_Id == subject_details[0].EYCES_Id && a.EMGR_Id == b.EMGR_Id && b.MI_Id == data.MI_Id && b.EMGR_ActiveFlag == true && c.EMGR_Id == b.EMGR_Id && c.EMGD_ActiveFlag == true)
                                                        select c).Distinct().ToArray();
                    }
                    if (data.EYCES_SubExamFlg)
                    {
                        data.subexam_gradedetails = (from a in _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO
                                                     from b in _examcontext.Exm_Master_GradeDMO
                                                     from c in _examcontext.Exm_Master_Grade_DetailsDMO
                                                     where (a.EYCES_Id == subject_details[0].EYCES_Id && a.EMGR_Id == b.EMGR_Id && b.MI_Id == data.MI_Id && b.EMGR_ActiveFlag == true && c.EMGR_Id == b.EMGR_Id && c.EMGD_ActiveFlag == true)
                                                     select c).Distinct().ToArray();
                    }
                }

                var alrdy_stu_count = _examcontext.ExamMarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id).ToList().Count();

                var stu_list_mapped = _examcontext.StudentMappingDMO.Where(k => k.MI_Id == data.MI_Id && k.ASMAY_Id == data.ASMAY_Id && k.ASMCL_Id == data.ASMCL_Id && k.ASMS_Id == data.ASMS_Id && k.ISMS_Id == data.ISMS_Id).Select(t => t.AMST_Id).Distinct().ToList();
                var studentList=new  List<temp_marks_DTO>();
                studentList = (from e in _examcontext.Adm_M_Student
                               from f in _examcontext.School_Adm_Y_StudentDMO
                               from g in _examcontext.IVRM_School_Master_SubjectsDMO
                               from h in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                               from i in subject_details
                                   //  from k in _examcontext.StudentMappingDMO
                               where (e.AMST_Id == f.AMST_Id && f.ASMCL_Id == data.ASMCL_Id && f.ASMS_Id == data.ASMS_Id && f.ASMAY_Id == data.ASMAY_Id && e.MI_Id == data.MI_Id && e.AMST_ActiveFlag == 1 && e.AMST_SOL == "S" && f.AMAY_ActiveFlag == 1 && g.ISMS_Id == h.ISMS_Id && g.MI_Id == data.MI_Id && g.ISMS_ActiveFlag == 1 && g.ISMS_ExamFlag == 1 && g.ISMS_Id == data.ISMS_Id && g.ISMS_Id == data.ISMS_Id && h.EYCE_Id == i.EYCE_Id)//&& k.MI_Id==data.MI_Id && k.ASMAY_Id==data.ASMAY_Id && k.ASMCL_Id ==data.ASMCL_Id && k.ASMS_Id==data.ASMS_Id && k.ISMS_Id==data.ISMS_Id && k.AMST_Id==f.AMST_Id && stu_list_mapped.Contains(f.AMST_Id)
                               select new temp_marks_DTO//MarksEntry_SEDTO
                               {
                                   AMST_Id = e.AMST_Id,
                                   AMST_FirstName = ((e.AMST_FirstName == null ? " " : e.AMST_FirstName) + " " + (e.AMST_MiddleName == null ? " " : e.AMST_MiddleName) + " " + (e.AMST_LastName == null ? " " : e.AMST_LastName)).Trim(),
                                   AMST_AdmNo = e.AMST_AdmNo == null ? "" : e.AMST_AdmNo,
                                   AMAY_RollNo = f.AMAY_RollNo,
                                   ISMS_Id = g.ISMS_Id,
                                   ISMS_SubjectName = g.ISMS_SubjectName,
                                   EYCES_MaxMarks = h.EYCES_MaxMarks,
                                   EYCES_MinMarks = h.EYCES_MinMarks,
                                   EYCES_MarksEntryMax = h.EYCES_MarksEntryMax,
                                   //  obtainmarks = "0"
                               }).Distinct().OrderBy(t => t.AMAY_RollNo).ToList();
                data.studentList = studentList.Where(t => stu_list_mapped.Contains(t.AMST_Id)).Distinct().OrderBy(t => t.AMAY_RollNo).ToArray();
               
                 if (alrdy_stu_count > 0 && data.EYCES_SubExamFlg)
                {
                    var stu_marks = _examcontext.ExamMarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id).Distinct().ToList();
                  
                    var  saved_studentList = (from a in studentList
                                        from b in stu_marks
                                        where (a.AMST_Id == b.AMST_Id && b.ISMS_Id == a.ISMS_Id && stu_list_mapped.Contains(a.AMST_Id))
                                        select new temp_marks_DTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName,
                                            AMST_AdmNo = a.AMST_AdmNo,
                                            AMAY_RollNo = a.AMAY_RollNo,
                                            ISMS_Id = a.ISMS_Id,
                                            ISMS_SubjectName = a.ISMS_SubjectName,
                                            EYCES_MaxMarks = a.EYCES_MaxMarks,
                                            EYCES_MarksEntryMax = a.EYCES_MarksEntryMax,
                                            EYCES_MinMarks = a.EYCES_MinMarks,
                                            ESTM_Marks = b.ESTM_Marks,
                                            ESTM_Grade = b.ESTM_Grade,
                                            ESTM_Flg = b.ESTM_Flg,
                                            ESTM_Id=b.ESTM_Id
                                        }).Distinct().OrderBy(t => t.AMAY_RollNo).ToList();
                    data.saved_studentList = saved_studentList.Distinct().OrderBy(t => t.AMAY_RollNo).ToArray();

                    data.saved_se_list = (from a in _examcontext.Exm_Student_Marks_SubSubjectDMO
                                            from b in stu_marks
                                            where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ESTM_Id == b.ESTM_Id)
                                            select a).Distinct().ToArray();
                }               
            }
            catch (Exception ee)
            {
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
                Console.WriteLine(ee.Message);
            }           
            return data;
        }

        public MarksEntry_SEDTO SaveMarks(MarksEntry_SEDTO data)
        {
            try
            {
                for(int i =0;i<data.main_save_list.Length;i++)
                {
                    var stu_id = data.main_save_list[i].AMST_Id;
                    var already_cnt = _examcontext.ExamMarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.AMST_Id == stu_id).Count();
                    if(already_cnt==0)
                    {
                        ExamMarksDMO obj_M = new ExamMarksDMO();
                        obj_M.MI_Id = data.MI_Id;
                        obj_M.ASMAY_Id = data.ASMAY_Id;
                        obj_M.ASMCL_Id = data.ASMCL_Id;
                        obj_M.ASMS_Id = data.ASMS_Id;
                        obj_M.EME_Id = data.EME_Id;
                        obj_M.ISMS_Id = data.ISMS_Id;
                        obj_M.AMST_Id = stu_id;
                        obj_M.ESTM_Marks = data.main_save_list[i].ESTM_Marks;
                        obj_M.ESTM_MarksGradeFlg = data.EYCES_MarksGradeEntryFlg;
                        obj_M.Id = data.UserId;
                        obj_M.LoginDateTime = DateTime.Now;
                        obj_M.IP4 = data.IP4;
                        obj_M.CreatedDate = DateTime.Now;
                        obj_M.UpdatedDate = DateTime.Now;
                        obj_M.ESTM_ActiveFlg = true;
                        obj_M.ESTM_Grade = data.main_save_list[i].ESTM_Grade;
                        obj_M.ESTM_Flg = data.main_save_list[i].ESTM_Flg;
                        _examcontext.Add(obj_M);
                        for(int j=0;j<data.Temp_subs_marks_list.Length;j++)
                        {
                            var sub_id = data.Temp_subs_marks_list[j].ISMS_Id;
                            if (data.ISMS_Id == sub_id && stu_id==data.Temp_subs_marks_list[j].AMST_Id)
                            {
                                Exm_Student_Marks_SubSubjectDMO obj_S = new Exm_Student_Marks_SubSubjectDMO();
                                obj_S.MI_Id = data.MI_Id;
                                obj_S.ESTM_Id = obj_M.ESTM_Id;
                                obj_S.EMSS_Id = data.Temp_subs_marks_list[j].EMSS_Id;
                                obj_S.ISMS_Id = sub_id;
                                obj_S.EMSE_Id = data.Temp_subs_marks_list[j].EMSE_Id;
                                obj_S.ESTMSS_Marks = data.Temp_subs_marks_list[j].ESTMSS_Marks;
                                obj_S.ESTMSS_MarksGradeFlg = data.Temp_subs_marks_list[j].ESTMSS_MarksGradeFlg;
                                obj_S.ESTMSS_Grade = data.Temp_subs_marks_list[j].ESTMSS_Grade;
                                obj_S.Login_Id = data.UserId;
                                obj_S.LoginDateTime = DateTime.Now;
                                obj_S.IP4 = data.IP4;
                                obj_S.ESTMSS_ActiveFlg = true;
                                obj_S.ESTMSS_Flg = data.Temp_subs_marks_list[j].ESTMSS_Flg;
                                obj_S.CreatedDate = DateTime.Now;
                                obj_S.UpdatedDate = DateTime.Now;
                                _examcontext.Add(obj_S);
                            }
                        }
                    }
                    else if(already_cnt == 1)
                    {
                        var result_obj = _examcontext.ExamMarksDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.AMST_Id == stu_id);
                        result_obj.ESTM_Marks = data.main_save_list[i].ESTM_Marks;
                        result_obj.ESTM_MarksGradeFlg = data.EYCES_MarksGradeEntryFlg;
                        result_obj.Id = data.UserId;
                        result_obj.LoginDateTime = DateTime.Now;
                        result_obj.IP4 = data.IP4;
                        // result_onj.CreatedDate = DateTime.Now;
                        result_obj.UpdatedDate = DateTime.Now;
                        result_obj.ESTM_ActiveFlg = true;
                        result_obj.ESTM_Grade = data.main_save_list[i].ESTM_Grade;
                        result_obj.ESTM_Flg = data.main_save_list[i].ESTM_Flg;
                        _examcontext.Update(result_obj);

                        var child_list = _examcontext.Exm_Student_Marks_SubSubjectDMO.Where(t => t.MI_Id == data.MI_Id && t.ESTM_Id == result_obj.ESTM_Id).ToList();
                        if (child_list.Any())
                        {
                            for (int l = 0; child_list.Count > l; l++)
                            {
                                _examcontext.Remove(child_list.ElementAt(l));
                                //var contactExists = _examcontext.SaveChanges();
                                //if (contactExists == 1)
                                //{
                                //    pagert.returnval = true;
                                //}
                                //else
                                //{
                                //    pagert.returnval = false;
                                //}
                            }
                        }
                        for (int j = 0; j < data.Temp_subs_marks_list.Length; j++)
                        {
                            var sub_id = data.Temp_subs_marks_list[j].ISMS_Id;
                            if (data.ISMS_Id == sub_id && stu_id == data.Temp_subs_marks_list[j].AMST_Id)
                            {
                                Exm_Student_Marks_SubSubjectDMO obj_S = new Exm_Student_Marks_SubSubjectDMO();
                                obj_S.MI_Id = data.MI_Id;
                                obj_S.ESTM_Id = result_obj.ESTM_Id;
                                obj_S.EMSS_Id = data.Temp_subs_marks_list[j].EMSS_Id;
                                obj_S.ISMS_Id = sub_id;
                                obj_S.EMSE_Id = data.Temp_subs_marks_list[j].EMSE_Id;
                                obj_S.ESTMSS_Marks = data.Temp_subs_marks_list[j].ESTMSS_Marks;
                                obj_S.ESTMSS_MarksGradeFlg = data.Temp_subs_marks_list[j].ESTMSS_MarksGradeFlg;
                                obj_S.ESTMSS_Grade = data.Temp_subs_marks_list[j].ESTMSS_Grade;
                                obj_S.Login_Id = data.UserId;
                                obj_S.LoginDateTime = DateTime.Now;
                                obj_S.IP4 = data.IP4;
                                obj_S.ESTMSS_ActiveFlg = true;
                                obj_S.ESTMSS_Flg = data.Temp_subs_marks_list[j].ESTMSS_Flg;
                                obj_S.CreatedDate = result_obj.CreatedDate;
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
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
                Console.WriteLine(ee.Message);
            }

           // try
          //  {
                //MarksCalcReset MarksCalcReset = new MarksCalcReset(_examcontext);
                //id.marksdeleteflag = MarksCalcReset.MarksCalculationReset(id.ASMAY_Id, id.ASMCL_Id, id.ASMS_Id, id.MI_Id, id.EME_Id);

                //var catid = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == id.ASMAY_Id && t.ASMCL_Id == id.ASMCL_Id && t.ASMS_Id == id.ASMS_Id && t.MI_Id == id.MI_Id).Select(t => t.EMCA_Id).ToArray();

                //var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id && catid.Contains(t.EMCA_Id)).Select(t => t.EYC_Id).ToArray();

                //var eyceid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id)).Select(t => t.EYCE_Id).ToArray();

                //var subid = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id)).ToArray();

                //var subMorGFlag = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id)).ToArray();

                //id.subMorGFlag = subMorGFlag[0].EYCES_MarksGradeEntryFlg;

                //id.EMGR_Id = subMorGFlag[0].EMGR_Id;

                //string msg = "";

            //    for (int i = 0; i < id.detailsList.Length; i++)
            //    {
            //        var checkduplicates = _examcontext.ExamMarksDMO.Where(d => d.ASMAY_Id == id.ASMAY_Id && d.ASMCL_Id == id.ASMCL_Id && d.ASMS_Id == id.ASMS_Id && d.AMST_Id == id.detailsList[i].amsT_Id && d.EME_Id == id.EME_Id && d.ISMS_Id == id.ISMS_Id && d.ESTM_ActiveFlg == true && d.MI_Id == id.MI_Id).ToList();

            //        if (checkduplicates.Count > 0)
            //        {

            //            var result = _examcontext.ExamMarksDMO.Single(d => d.ASMAY_Id == id.ASMAY_Id && d.ASMCL_Id == id.ASMCL_Id && d.ASMS_Id == id.ASMS_Id && d.AMST_Id == id.detailsList[i].amsT_Id && d.EME_Id == id.EME_Id && d.ISMS_Id == id.ISMS_Id && d.ESTM_ActiveFlg == true && d.MI_Id == id.MI_Id);


            //            result.MI_Id = id.MI_Id;
            //            result.ASMCL_Id = id.ASMCL_Id;
            //            result.ASMAY_Id = id.ASMAY_Id;
            //            result.ASMS_Id = id.ASMS_Id;
            //            result.EME_Id = id.EME_Id;
            //            result.ISMS_Id = id.ISMS_Id;
            //            result.AMST_Id = id.detailsList[i].amsT_Id;

            //            if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L" || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m")
            //            {
            //                result.ESTM_Flg = id.detailsList[i].obtainmarks;
            //                result.ESTM_Marks = Convert.ToDecimal("0");
            //                result.ESTM_Grade = "null";
            //                if (id.subMorGFlag == "M")
            //                {
            //                    result.ESTM_MarksGradeFlg = "M";
            //                }
            //                else if (id.subMorGFlag == "G")
            //                {
            //                    result.ESTM_MarksGradeFlg = "G";
            //                }
            //            }
            //            else
            //            {
            //                result.ESTM_Flg = "";

            //                if (id.subMorGFlag == "M")
            //                {
            //                    result.ESTM_MarksGradeFlg = "M";
            //                    result.ESTM_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
            //                    result.ESTM_Grade = "null";
            //                }
            //                else if (id.subMorGFlag == "G")
            //                {
            //                    result.ESTM_MarksGradeFlg = "G";
            //                    result.ESTM_Marks = Convert.ToDecimal("0");
            //                    result.ESTM_Grade = id.detailsList[i].obtainmarks; //id.ESTM_Grade;
            //                }
            //            }

            //            result.UpdatedDate = DateTime.Now;
            //            result.Id = id.Id;
            //            result.IP4 = id.IP4;




            //            _examcontext.Update(result);
            //            int flag = _examcontext.SaveChanges();
            //            if (flag == 1)
            //            {
            //                id.messagesaveupdate = "true";
            //            }
            //            else
            //            {
            //                id.messagesaveupdate = "true";
            //            }
            //        }
            //        else
            //        {
            //            ExamMarksDMO MM = Mapper.Map<ExamMarksDMO>(id);
            //            MM.MI_Id = id.MI_Id;
            //            MM.ASMCL_Id = id.ASMCL_Id;
            //            MM.ASMAY_Id = id.ASMAY_Id;
            //            MM.ASMS_Id = id.ASMS_Id;
            //            MM.EME_Id = id.EME_Id;
            //            MM.ISMS_Id = id.ISMS_Id;
            //            MM.AMST_Id = id.detailsList[i].amsT_Id;

            //            if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L" || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m")
            //            {
            //                MM.ESTM_Flg = id.detailsList[i].obtainmarks;
            //                MM.ESTM_Marks = Convert.ToDecimal("0");
            //                MM.ESTM_Grade = "null";
            //                if (id.subMorGFlag == "M")
            //                {
            //                    MM.ESTM_MarksGradeFlg = "M";
            //                }
            //                else if (id.subMorGFlag == "G")
            //                {
            //                    MM.ESTM_MarksGradeFlg = "G";
            //                }
            //            }
            //            else
            //            {
            //                MM.ESTM_Flg = "";

            //                if (id.subMorGFlag == "M")
            //                {
            //                    MM.ESTM_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
            //                    MM.ESTM_MarksGradeFlg = "M";
            //                    MM.ESTM_Grade = "null";
            //                }
            //                else if (id.subMorGFlag == "G")
            //                {
            //                    MM.ESTM_Marks = Convert.ToDecimal("0");
            //                    MM.ESTM_MarksGradeFlg = "G";
            //                    MM.ESTM_Grade = id.detailsList[i].obtainmarks; //id.ESTM_Grade;
            //                }
            //            }




            //            MM.CreatedDate = DateTime.Now;
            //            MM.UpdatedDate = DateTime.Now;
            //            MM.Id = id.Id;
            //            MM.IP4 = id.IP4;
            //            //MM.ESTM_Grade = id.ESTM_Grade;
            //            MM.ESTM_ActiveFlg = true;

            //            _examcontext.Add(MM);
            //            int flag = _examcontext.SaveChanges();
            //            if (flag == 1)
            //            {
            //                id.messagesaveupdate = "true";
            //            }
            //            else
            //            {
            //                id.messagesaveupdate = "true";
            //            }
            //        }

            //    }

            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            return data;
        }



    }
}
