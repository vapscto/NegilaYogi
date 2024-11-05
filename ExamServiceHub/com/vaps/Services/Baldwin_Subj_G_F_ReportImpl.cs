using ExamServiceHub.com.vaps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using Microsoft.Extensions.Logging;
using System.Dynamic;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using DomainModel.Model.com.vapstech.TT;

namespace ExamServiceHub.com.vaps.Services
{
    public class Baldwin_Subj_G_F_ReportImpl : Baldwin_Subj_G_F_ReportInterface
    {
        private readonly ExamContext _examcontext;
        ILogger<Baldwin_Final_P_ReportImpl> _acdimpl;
        public Baldwin_Subj_G_F_ReportImpl(ExamContext exm, ILogger<Baldwin_Final_P_ReportImpl> _acdim)
        {
            _examcontext = exm;
            _acdimpl = _acdim;
        }
        public Baldwin_Subj_G_F_ReportDTO Getdetails(Baldwin_Subj_G_F_ReportDTO data)
        {
            try
            {
                data.yearlist = _examcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray(); 
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }

        public Baldwin_Subj_G_F_ReportDTO get_classes(Baldwin_Subj_G_F_ReportDTO data)
        {
            try
            {
                data.classlist = (from c in _examcontext.AdmissionClass
                                  from d in _examcontext.Exm_Category_ClassDMO
                                  where (d.ASMCL_Id == c.ASMCL_Id && d.ECAC_ActiveFlag == true && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id)
                                  select c).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }

        public Baldwin_Subj_G_F_ReportDTO get_sections(Baldwin_Subj_G_F_ReportDTO data)
        {
            try
            {
                data.sectionlist = (from b in _examcontext.AdmissionClass
                                    from c in _examcontext.School_M_Section
                                    from d in _examcontext.Exm_Category_ClassDMO
                                    where (b.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == d.ASMS_Id && d.ECAC_ActiveFlag == true && d.ASMCL_Id == data.ASMCL_Id
                                    && c.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id)
                                    select c).Distinct().OrderBy(t => t.ASMC_Order).ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }

        public Baldwin_Subj_G_F_ReportDTO get_students(Baldwin_Subj_G_F_ReportDTO data)
        {
            try
            {
                List<int?> ids = new List<int?>();
                ids.Add(0);
                ids.Add(1);

                List<string> sol = new List<string>();
                sol.Add("S");
                sol.Add("L");
                sol.Add("D");

                var EMCA_Id = _examcontext.Exm_Category_ClassDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id 
                && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).EMCA_Id;

                var EYC_Id = _examcontext.Exm_Yearly_CategoryDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EMCA_Id == EMCA_Id 
                && t.EYC_ActiveFlg == true).EYC_Id;                

                var EME_Id_Final = (from a in _examcontext.masterexam
                                    from b in _examcontext.Exm_Yearly_Category_ExamsDMO
                                    where (a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true && a.EME_Id == b.EME_Id && b.EYC_Id == EYC_Id && b.EYCE_ActiveFlg == true && a.EME_FinalExamFlag == true)
                                    select a.EME_Id).ToList();

                data.studentlist = (from a in _examcontext.Adm_M_Student
                                    from b in _examcontext.School_Adm_Y_StudentDMO
                                    from c in _examcontext.ExmStudentMarksProcessDMO
                                    where (a.MI_Id == data.MI_Id && sol.Contains(a.AMST_SOL) && ids.Contains(a.AMST_ActiveFlag) && b.AMST_Id == a.AMST_Id 
                                    && b.ASMAY_Id == data.ASMAY_Id && ids.Contains(b.AMAY_ActiveFlag) && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id 
                                    && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id && data.ASMS_Id == data.ASMS_Id 
                                    && c.ESTMP_Result == "Fail" && c.AMST_Id == a.AMST_Id && EME_Id_Final.Contains(c.EME_Id))
                                    select new Baldwin_Subj_G_F_ReportDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                        AMST_AdmNo = a.AMST_AdmNo == null ? "" : a.AMST_AdmNo,
                                        AMAY_RollNo = b.AMAY_RollNo,
                                        AMST_DOB = a.AMST_DOB,
                                        AMST_Photoname = a.AMST_Photoname
                                    }).Distinct().OrderBy(t => t.AMAY_RollNo).ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }

        public Baldwin_Subj_G_F_ReportDTO get_report(Baldwin_Subj_G_F_ReportDTO data)
        {
            try
            {
                var EMCA_Id = _examcontext.Exm_Category_ClassDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).EMCA_Id;

                var EYC_Id = _examcontext.Exm_Yearly_CategoryDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EMCA_Id == EMCA_Id && t.EYC_ActiveFlg == true).EYC_Id;

                var examlist = (from a in _examcontext.masterexam
                                from b in _examcontext.Exm_Yearly_Category_ExamsDMO
                                where (a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true && a.EME_Id == b.EME_Id && b.EYC_Id == EYC_Id && b.EYCE_ActiveFlg == true)
                                select new Baldwin_Subj_G_F_ReportDTO
                                {
                                    EME_Id = a.EME_Id,
                                    EME_ExamName = a.EME_ExamName,
                                    EME_ExamCode = a.EME_ExamCode,
                                    EME_ExamOrder = a.EME_ExamOrder,
                                    EME_FinalExamFlag = a.EME_FinalExamFlag,
                                    EYCE_Id = b.EYCE_Id,
                                    EMGR_Id = b.EMGR_Id
                                }).Distinct().OrderBy(t => t.EME_ExamOrder).ToList();
                data.examlist = examlist.Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();

                var EYCE_Ids = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == EYC_Id && t.EYCE_ActiveFlg == true).Select(t => t.EYCE_Id).Distinct().ToList();

                var stu_subjects = _examcontext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id 
                && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).Select(t => t.ISMS_Id).Distinct().ToList();

                var subjectlist = (from a in _examcontext.IVRM_School_Master_SubjectsDMO
                                   from b in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                   where (a.MI_Id == data.MI_Id && a.ISMS_ExamFlag == 1 && a.ISMS_ActiveFlag == 1 && a.ISMS_Id == b.ISMS_Id 
                                   && EYCE_Ids.Contains(b.EYCE_Id) && b.EYCES_ActiveFlg == true && stu_subjects.Contains(b.ISMS_Id))
                                   select a).Distinct().OrderBy(t => t.ISMS_OrderFlag).ToList();

                data.subjectlist = subjectlist.Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();

                var subject_orders = (from a in examlist
                                      from b in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                      from c in subjectlist
                                      where (b.EYCE_Id == a.EYCE_Id && b.EYCES_ActiveFlg == true && c.ISMS_Id == b.ISMS_Id && a.EME_FinalExamFlag == true)
                                      select new IVRM_School_Master_SubjectsDMO
                                      {
                                          ISMS_Id = c.ISMS_Id,
                                          MI_Id = c.MI_Id,
                                          ISMS_SubjectName = c.ISMS_SubjectName,
                                          ISMS_SubjectCode = c.ISMS_SubjectCode,
                                          ISMS_Max_Marks = c.ISMS_Max_Marks,
                                          ISMS_Min_Marks = c.ISMS_Min_Marks,
                                          ISMS_ExamFlag = c.ISMS_ExamFlag,
                                          ISMS_PreadmFlag = c.ISMS_PreadmFlag,
                                          ISMS_SubjectFlag = c.ISMS_SubjectFlag,
                                          ISMS_BatchAppl = c.ISMS_BatchAppl,
                                          ISMS_ActiveFlag = c.ISMS_ActiveFlag,
                                          ISMS_OrderFlag = b.EYCES_SubjectOrder,
                                          CreatedDate = c.CreatedDate,
                                          UpdatedDate = c.UpdatedDate,
                                          ISMS_AttendanceFlag = c.ISMS_AttendanceFlag,
                                          ISMS_AtExtraFeeFlg = c.ISMS_AtExtraFeeFlg,
                                          ISMS_LanguageFlg = c.ISMS_LanguageFlg,
                                          ISMS_TTFlag = c.ISMS_TTFlag
                                      }).Distinct().OrderBy(t => t.ISMS_OrderFlag).ToList();

                data.subjectlist = subject_orders.Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();

                var stu_marks = _examcontext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).Distinct().OrderBy(t => t.ISMS_Id).ThenBy(t => t.EME_Id).ToList();
                data.studentmarks = stu_marks.Distinct().OrderBy(t => t.ISMS_Id).ThenBy(t => t.EME_Id).ToArray();

                data.classteacher = (from a in _examcontext.ClassTeacherMappingDMO
                                     from b in _examcontext.HR_Master_Employee_DMO
                                     where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                     select new Baldwin_Subj_G_F_ReportDTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                     }).Distinct().ToArray();

                var Exam_Subwise_Details = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => EYCE_Ids.Contains(t.EYCE_Id) && t.EYCES_ActiveFlg == true && stu_subjects.Contains(t.ISMS_Id)).Distinct().OrderBy(t => t.EYCE_Id).ThenBy(t => t.EYCES_SubjectOrder).ToList();
                data.examsubjectwise_details = Exam_Subwise_Details.ToArray();

                var Process_Exam_Details = _examcontext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).Distinct().OrderBy(t => t.EME_Id).ToList();//&& t.AMST_Id == data.AMST_Id
                data.process_examdetails = Process_Exam_Details.ToArray();
                                
                var promotion_flag = _examcontext.Exm_ConfigurationDMO.Single(t => t.MI_Id == data.MI_Id).ExmConfig_PromotionFlag;
                data.ExmConfig_PromotionFlag = promotion_flag;

                var Subject_GrpsDetails = _examcontext.ExamsubjectGroupMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EMCA_Id == EMCA_Id && t.ESG_ActiveFlag == true).Distinct().ToList();
                data.subject_grpsdetails = Subject_GrpsDetails.ToArray();

                var Subject_Grps_ExmDetails = (from a in _examcontext.ExamSubjectGroupMappingExamsDMO
                                               from b in Subject_GrpsDetails
                                               where (a.ESG_Id == b.ESG_Id && a.ESGE_ActiveFlag == true)
                                               select a).Distinct().ToList();
                data.subject_grps_exmdetails = Subject_Grps_ExmDetails.ToArray();

                var Subject_Grps_SubjDetails = (from a in _examcontext.ExamSubjectGroupMappingSubjectsDMO
                                                from b in Subject_GrpsDetails
                                                where (a.ESG_Id == b.ESG_Id && a.ESGS_ActiveFlag == true)
                                                select a).Distinct().ToList();
                data.subject_grps_subjdetails = Subject_Grps_SubjDetails.ToArray();

                //for Elective subject groups 
                var Elective_Subj_GrpDetails = (from a in _examcontext.StudentMappingDMO
                                                from b in _examcontext.Exm_Master_GroupDMO
                                                where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id 
                                                && a.ASMS_Id == data.ASMS_Id && b.EMG_Id == a.EMG_Id && b.MI_Id == a.MI_Id && b.EMG_ActiveFlag == true 
                                                && b.EMG_MaxAplSubjects == 1 && b.EMG_MinAplSubjects == 1 && b.EMG_ElectiveFlg == true)
                                                select b).Distinct().ToList();
                data.elective_subj_grpdetails = Elective_Subj_GrpDetails.ToArray();

                var Elective_Subj_GrpSubjects = (from a in Elective_Subj_GrpDetails
                                                 from b in _examcontext.Exm_Master_Group_SubjectsDMO
                                                 from c in subject_orders
                                                 where (a.EMG_Id == b.EMG_Id && b.EMGS_ActiveFlag == true && b.ISMS_Id == c.ISMS_Id)
                                                 select b).Distinct().ToList();
                data.elective_subj_grpsubjects = Elective_Subj_GrpSubjects.Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
    }
}
