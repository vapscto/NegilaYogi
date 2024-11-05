using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class Baldwin_Final_P_C_ReportImpl : Interfaces.Baldwin_Final_P_C_ReportInterface
    {        
        ILogger<Baldwin_Final_P_C_ReportImpl> _acdimpl;
        private readonly ExamContext _examcontext;
        public Baldwin_Final_P_C_ReportImpl(ExamContext exm, ILogger<Baldwin_Final_P_C_ReportImpl> _acdim)
        {
            _examcontext = exm;
            _acdimpl = _acdim;
        }

        public Baldwin_Final_P_C_ReportDTO Getdetails(Baldwin_Final_P_C_ReportDTO data)
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
        public Baldwin_Final_P_C_ReportDTO get_classes(Baldwin_Final_P_C_ReportDTO data)
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

        public Baldwin_Final_P_C_ReportDTO get_sections(Baldwin_Final_P_C_ReportDTO data)
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

        public Baldwin_Final_P_C_ReportDTO get_students(Baldwin_Final_P_C_ReportDTO data)
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

                data.studentlist = (from a in _examcontext.Adm_M_Student
                                    from b in _examcontext.School_Adm_Y_StudentDMO
                                    where (a.MI_Id == data.MI_Id && sol.Contains(a.AMST_SOL) && ids.Contains(a.AMST_ActiveFlag) && b.AMST_Id == a.AMST_Id 
                                    && b.ASMAY_Id == data.ASMAY_Id && ids.Contains(b.AMAY_ActiveFlag) && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id)
                                    select new Baldwin_Final_P_C_ReportDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                        AMST_AdmNo = a.AMST_AdmNo == null ? "" : a.AMST_AdmNo,
                                        AMAY_RollNo = b.AMAY_RollNo,
                                        AMST_DOB = a.AMST_DOB
                                    }).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }

        public Baldwin_Final_P_C_ReportDTO get_report(Baldwin_Final_P_C_ReportDTO data)
        {
            try
            {
                var EMCA_Id = _examcontext.Exm_Category_ClassDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).EMCA_Id;
                var EYC_Id = _examcontext.Exm_Yearly_CategoryDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EMCA_Id == EMCA_Id && t.EYC_ActiveFlg == true).EYC_Id;

                var examlist = (from a in _examcontext.masterexam
                                from b in _examcontext.Exm_Yearly_Category_ExamsDMO
                                where (a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true && a.EME_Id == b.EME_Id && b.EYC_Id == EYC_Id && b.EYCE_ActiveFlg == true)
                                select new Baldwin_Final_P_C_ReportDTO
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

                var stu_subjects = _examcontext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).Select(t => t.ISMS_Id).Distinct().ToList();

                var subjectlist = (from a in _examcontext.IVRM_School_Master_SubjectsDMO
                                   from b in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                   where (a.MI_Id == data.MI_Id && a.ISMS_ExamFlag == 1 && a.ISMS_ActiveFlag == 1 && a.ISMS_Id == b.ISMS_Id && EYCE_Ids.Contains(b.EYCE_Id) && b.EYCES_ActiveFlg == true && stu_subjects.Contains(b.ISMS_Id))
                                   select a).Distinct().OrderBy(t => t.ISMS_OrderFlag).ToList();

                data.subjectlist = subjectlist.Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();

                //for final exam order 
                var subject_orders = (from a in examlist
                                      from b in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                      from c in subjectlist
                                      where (a.EME_FinalExamFlag == true && b.EYCE_Id == a.EYCE_Id && b.EYCES_ActiveFlg == true && c.ISMS_Id == b.ISMS_Id)
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
                                     where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                     && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                     select new Baldwin_Final_P_C_ReportDTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                     }).Distinct().ToArray();

                var Exam_Subwise_Details = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => EYCE_Ids.Contains(t.EYCE_Id) && t.EYCES_ActiveFlg == true && stu_subjects.Contains(t.ISMS_Id)).Distinct().OrderBy(t => t.EYCE_Id).ThenBy(t => t.EYCES_SubjectOrder).ToList();

                data.examsubjectwise_details = Exam_Subwise_Details.ToArray();

                var Process_Exam_Details = _examcontext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).Distinct().OrderBy(t => t.EME_Id).ToList();// && t.AMST_Id == data.AMST_Id
                data.process_examdetails = Process_Exam_Details.ToArray();

                var from_date = _examcontext.AcademicYear.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.Is_Active == true).ASMAY_From_Date;
                var to_date = _examcontext.AcademicYear.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.Is_Active == true).ASMAY_To_Date;

                var promotion_flag = _examcontext.Exm_ConfigurationDMO.Single(t => t.MI_Id == data.MI_Id).ExmConfig_PromotionFlag;
                data.ExmConfig_PromotionFlag = promotion_flag;
                if (promotion_flag)
                {

                    var promotion_type = _examcontext.Exm_M_PromotionDMO.Single(t => t.MI_Id == data.MI_Id && t.EYC_Id == EYC_Id 
                    && t.EMP_ActiveFlag == true).EMP_MarksPerFlg;
                    data.EMP_MarksPerFlg = promotion_type;

                    if (promotion_type != "T" && promotion_type != "F")
                    {
                        var EMP_Id = _examcontext.Exm_M_PromotionDMO.Single(t => t.MI_Id == data.MI_Id && t.EYC_Id == EYC_Id && t.EMP_ActiveFlag == true).EMP_Id;

                        var promotion_subectdetails = _examcontext.Exm_M_Promotion_SubjectsDMO.Where(t => t.EMP_Id == EMP_Id && t.EMPS_ActiveFlag == true).Distinct().ToList();
                        data.promotion_subectdetails = promotion_subectdetails.Distinct().ToArray();

                        var prom_subj_groupdetails = _examcontext.Exm_M_Prom_Subj_GroupDMO.Where(t => t.EMPS_Id == promotion_subectdetails[0].EMPS_Id && t.EMPSG_ActiveFlag == true).Distinct().ToList();
                        data.prom_subj_groupdetails = prom_subj_groupdetails.ToArray();

                        var prom_subj_groupdetails_all = (from a in _examcontext.Exm_M_Prom_Subj_GroupDMO
                                                          from b in promotion_subectdetails
                                                          where (a.EMPS_Id == b.EMPS_Id && a.EMPSG_ActiveFlag == true)
                                                          select a).Distinct().ToList();
                        data.prom_subj_groupdetails_all = prom_subj_groupdetails_all.ToArray();

                        var prom_subj_grp_exms = (from a in _examcontext.Exm_M_Prom_Subj_Group_ExamsDMO
                                                  from b in prom_subj_groupdetails
                                                  where (a.EMPSG_Id == b.EMPSG_Id)
                                                  select a).Distinct().ToList();

                        data.prom_subj_grp_exms = prom_subj_grp_exms.ToArray();                      

                        var promotion_stumarks = _examcontext.Exm_Stu_MP_Promo_SubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).Distinct().ToList();

                        data.promotion_stumarks = promotion_stumarks.ToArray();
                        data.promotion_stumarks_grpwise = (from a in _examcontext.Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO
                                                           from b in promotion_stumarks
                                                           where (a.ESTMPPS_Id == b.ESTMPPS_Id)
                                                           select a).Distinct().ToArray();
                        data.promotion_mainmarks = _examcontext.Exm_Student_MP_PromotionDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id 
                        && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).Distinct().ToArray();

                        if (promotion_type == "P")
                        {
                            using (var cmd = _examcontext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Prom_Cumulative_Report";
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

                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                                    SqlDbType.BigInt)
                                {
                                    Value = data.ASMCL_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                                    SqlDbType.BigInt)
                                {
                                    Value = data.ASMS_Id
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
                                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                            {
                                                dataRow1.Add(
                                                    dataReader.GetName(iFiled1),
                                                    dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                                );
                                            }

                                            retObject.Add((ExpandoObject)dataRow1);
                                        }
                                    }
                                    data.prom_cumulativedetails = retObject.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    _acdimpl.LogError(ex.Message);
                                    _acdimpl.LogDebug(ex.Message);
                                }
                            }
                        }
                    }
                    else if (promotion_type == "F" || promotion_type == "T")
                    {
                        var promotion_stumarks = _examcontext.Exm_Stu_MP_Promo_SubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).Distinct().ToList();
                        data.promotion_stumarks = promotion_stumarks.ToArray();
                        
                        data.promotion_mainmarks = _examcontext.Exm_Student_MP_PromotionDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).Distinct().ToArray();
                    }
                }
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