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
    public class Baldwin_Final_P_ReportBGHSImpl : Baldwin_Final_P_ReportBGHSInterface
    {
        private readonly ExamContext _examcontext;
        ILogger<Baldwin_Final_P_ReportImpl> _acdimpl;
        public Baldwin_Final_P_ReportBGHSImpl(ExamContext exm, ILogger<Baldwin_Final_P_ReportImpl> _acdim)
        {
            _examcontext = exm;
            _acdimpl = _acdim;
        }
        public Baldwin_Final_P_ReportBGHSDTO Getdetails(Baldwin_Final_P_ReportBGHSDTO data)
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

        public Baldwin_Final_P_ReportBGHSDTO get_classes(Baldwin_Final_P_ReportBGHSDTO data)
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

        public Baldwin_Final_P_ReportBGHSDTO get_sections(Baldwin_Final_P_ReportBGHSDTO data)
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

        public Baldwin_Final_P_ReportBGHSDTO get_students(Baldwin_Final_P_ReportBGHSDTO data)
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
                                    select new Baldwin_Final_P_ReportBGHSDTO
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

        public Baldwin_Final_P_ReportBGHSDTO get_report(Baldwin_Final_P_ReportBGHSDTO data)
        {
            try
            {
                var EMCA_Id = _examcontext.Exm_Category_ClassDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).EMCA_Id;

                var EYC_Id = _examcontext.Exm_Yearly_CategoryDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EMCA_Id == EMCA_Id && t.EYC_ActiveFlg == true).EYC_Id;

                var special_exams = _examcontext.masterexam.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && (t.EME_ExamName == "PROJECT" || t.EME_FinalExamFlag)).Select(t => t.EME_Id).ToList();

                //var final_exam = _examcontext.masterexam.Single(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && t.EME_FinalExamFlag).EME_Id;

                //var EMGR_Id = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EME_Id==final_exam && t.EYC_Id == EYC_Id && t.EYCE_ActiveFlg == true).Select(t => t.EMGR_Id).Distinct().ToList();
                //data.EMGR_Id = EMGR_Id[0];

                var Get_Promotion_Grade = _examcontext.Exm_M_PromotionDMO.Where(a => a.EYC_Id == EYC_Id && a.EMP_ActiveFlag == true).ToList();
                data.EMGR_Id = Get_Promotion_Grade.FirstOrDefault().EMGR_Id;

                //var EYCE_Id = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EME_Id == final_exam && t.EYC_Id == EYC_Id && t.EYCE_ActiveFlg == true).Select(t => t.EYCE_Id).Distinct().ToList();
                //data.EYCE_Id = EYCE_Id[0];

                var grade_details = _examcontext.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == data.EMGR_Id && t.EMGD_ActiveFlag == true).Distinct().ToList();
                data.grade_details = grade_details.ToArray();

                var examlist = (from a in _examcontext.masterexam
                                from b in _examcontext.Exm_Yearly_Category_ExamsDMO
                                where (a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true && a.EME_Id == b.EME_Id && b.EYC_Id == EYC_Id && b.EYCE_ActiveFlg == true && special_exams.Contains(a.EME_Id))
                                select new Baldwin_Final_P_ReportBGHSDTO
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

                var EYCE_Ids = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == EYC_Id && t.EYCE_ActiveFlg == true 
                && special_exams.Contains(t.EME_Id)).Select(t => t.EYCE_Id).Distinct().ToList();

                var stu_subjects = _examcontext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id 
                && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).Select(t => t.ISMS_Id).Distinct().ToList();

                //data.stu_subjects = _examcontext.StudentMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id 
                //&& t.ASMS_Id == data.ASMS_Id && t.ESTSU_ActiveFlg == true).Distinct().ToArray();

                //data.stu_subjects = _examcontext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id 
                //&& t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).Select(a=> new { a.AMST_Id, a.ISMS_Id }).Distinct().ToArray();



                data.stu_subjects = (from t in _examcontext.ExmStudentMarksProcessSubjectwiseDMO
                                     from b in _examcontext.IVRM_School_Master_SubjectsDMO
                                     where (t.ISMS_Id == b.ISMS_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id
                                     && t.ASMS_Id == data.ASMS_Id)
                                     select new Baldwin_Final_P_ReportBGHSDTO
                                     {
                                         AMST_Id = t.AMST_Id,
                                         ISMS_Id = t.ISMS_Id
                                     }).Distinct().ToArray();

                var subjectlist = (from a in _examcontext.IVRM_School_Master_SubjectsDMO
                                   from b in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                   where (a.MI_Id == data.MI_Id && a.ISMS_ExamFlag == 1 && a.ISMS_ActiveFlag == 1 && a.ISMS_Id == b.ISMS_Id && EYCE_Ids.Contains(b.EYCE_Id) && b.EYCES_ActiveFlg == true && stu_subjects.Contains(b.ISMS_Id))
                                   select a).Distinct().OrderBy(t => t.ISMS_OrderFlag).ToList();
                data.subjectlist = subjectlist.Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();


                var subject_orders =(from a in _examcontext.Exm_M_PromotionDMO 
                                     from b in _examcontext.Exm_M_Promotion_SubjectsDMO
                                     from c in _examcontext.IVRM_School_Master_SubjectsDMO
                                     where(a.EMP_Id==b.EMP_Id && b.ISMS_Id==c.ISMS_Id && a.EMP_ActiveFlag==true && b.EMPS_ActiveFlag==true
                                     && a.EYC_Id== EYC_Id)
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
                                         ISMS_OrderFlag = Convert.ToInt64(b.EMPS_SubjOrder),
                                         CreatedDate = c.CreatedDate,
                                         UpdatedDate = c.UpdatedDate,
                                         ISMS_AttendanceFlag = c.ISMS_AttendanceFlag,
                                         ISMS_AtExtraFeeFlg = c.ISMS_AtExtraFeeFlg,
                                         ISMS_LanguageFlg = c.ISMS_LanguageFlg,
                                         ISMS_TTFlag = c.ISMS_TTFlag
                                     }).Distinct().OrderBy(t => t.ISMS_OrderFlag).ToList();

                //var subject_orders = (from a in examlist
                //                      from b in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                //                      from c in subjectlist
                //                      where (b.EYCE_Id == a.EYCE_Id && b.EYCES_ActiveFlg == true && c.ISMS_Id == b.ISMS_Id && a.EYCE_Id == data.EYCE_Id)
                //                      select new IVRM_School_Master_SubjectsDMO
                //                      {
                //                          ISMS_Id = c.ISMS_Id,
                //                          MI_Id = c.MI_Id,
                //                          ISMS_SubjectName = c.ISMS_SubjectName,
                //                          ISMS_SubjectCode = c.ISMS_SubjectCode,
                //                          ISMS_Max_Marks = c.ISMS_Max_Marks,
                //                          ISMS_Min_Marks = c.ISMS_Min_Marks,
                //                          ISMS_ExamFlag = c.ISMS_ExamFlag,
                //                          ISMS_PreadmFlag = c.ISMS_PreadmFlag,
                //                          ISMS_SubjectFlag = c.ISMS_SubjectFlag,
                //                          ISMS_BatchAppl = c.ISMS_BatchAppl,
                //                          ISMS_ActiveFlag = c.ISMS_ActiveFlag,
                //                          ISMS_OrderFlag = b.EYCES_SubjectOrder,
                //                          CreatedDate = c.CreatedDate,
                //                          UpdatedDate = c.UpdatedDate,
                //                          ISMS_AttendanceFlag = c.ISMS_AttendanceFlag,
                //                          ISMS_AtExtraFeeFlg = c.ISMS_AtExtraFeeFlg,
                //                          ISMS_LanguageFlg = c.ISMS_LanguageFlg,
                //                          ISMS_TTFlag = c.ISMS_TTFlag
                //                      }).Distinct().OrderBy(t => t.ISMS_OrderFlag).ToList();

                data.subjectlist = subject_orders.Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();

                var stu_marks = _examcontext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && special_exams.Contains(t.EME_Id)).Distinct().OrderBy(t => t.ISMS_Id).ThenBy(t => t.EME_Id).ToList();
                data.studentmarks = stu_marks.Distinct().OrderBy(t => t.ISMS_Id).ThenBy(t => t.EME_Id).ToArray();

                data.classteacher = (from a in _examcontext.ClassTeacherMappingDMO
                                     from b in _examcontext.HR_Master_Employee_DMO
                                     where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag==true)
                                     select new Baldwin_Final_P_ReportBGHSDTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                     }).Distinct().ToArray();

                var Exam_Subwise_Details = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => EYCE_Ids.Contains(t.EYCE_Id) && t.EYCES_ActiveFlg == true 
                && stu_subjects.Contains(t.ISMS_Id)).Distinct().OrderBy(t => t.EYCE_Id).ThenBy(t => t.EYCES_SubjectOrder).ToList();

                data.examsubjectwise_details = Exam_Subwise_Details.ToArray();
                 
                //for non applicable subjects grade remarks
                var subjgrade_details = (from a in Exam_Subwise_Details
                                         from b in _examcontext.Exm_Master_Grade_DetailsDMO
                                         where (a.EMGR_Id == b.EMGR_Id && b.EMGD_ActiveFlag == true)
                                         select b).Distinct().ToList();
                data.subj_grade_details = subjgrade_details.ToArray();

                var Process_Exam_Details = _examcontext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && special_exams.Contains(t.EME_Id)).Distinct().OrderBy(t => t.EME_Id).ToList();
                data.process_examdetails = Process_Exam_Details.ToArray();

                var from_date = _examcontext.AcademicYear.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.Is_Active == true).ASMAY_From_Date;
                var to_date = _examcontext.AcademicYear.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.Is_Active == true).ASMAY_To_Date;
                using (var cmd = _examcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "StudentAttendance_W";
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
                    cmd.Parameters.Add(new SqlParameter("@from",
                        SqlDbType.Date)
                    {
                        Value = from_date
                    });
                    cmd.Parameters.Add(new SqlParameter("@to",
                        SqlDbType.Date)
                    {
                        Value = to_date
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
                        data.Work_attendence = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }
                using (var cmd = _examcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "StudentAttendance_P";
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
                    cmd.Parameters.Add(new SqlParameter("@from",
                        SqlDbType.Date)
                    {
                        Value = from_date
                    });
                    cmd.Parameters.Add(new SqlParameter("@to",
                        SqlDbType.Date)
                    {
                        Value = to_date
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();

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

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.Present_attendence = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }
                var promotion_flag = _examcontext.Exm_ConfigurationDMO.Single(t => t.MI_Id == data.MI_Id).ExmConfig_PromotionFlag;
                data.ExmConfig_PromotionFlag = promotion_flag;

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
