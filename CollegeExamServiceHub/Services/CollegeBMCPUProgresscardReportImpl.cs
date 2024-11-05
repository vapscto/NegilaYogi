using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Services
{
    public class CollegeBMCPUProgresscardReportImpl : Interfaces.CollegeBMCPUProgresscardReportInterface
    {
        public ClgExamContext _examcontext;
        ILogger<CollegeBMCPUProgresscardReportImpl> _acdimpl;

        public CollegeBMCPUProgresscardReportImpl(ClgExamContext _context, ILogger<CollegeBMCPUProgresscardReportImpl> _log)
        {
            _examcontext = _context;
            _acdimpl = _log;
        }

        public CollegeBMCPUProgresscardReportDTO Getdetails(CollegeBMCPUProgresscardReportDTO data)
        {
            try
            {
                data.getyear = _examcontext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeBMCPUProgresscardReportDTO OnAcdyear(CollegeBMCPUProgresscardReportDTO data)
        {
            try
            {
                data.getcourse = (from a in _examcontext.MasterCourseDMO
                                  from b in _examcontext.CLG_Adm_College_AY_CourseDMO
                                  from c in _examcontext.AcademicYear
                                  where (a.AMCO_Id == b.AMCO_Id && b.ASMAY_Id == c.ASMAY_Id && a.AMCO_ActiveFlag == true && b.ACAYC_ActiveFlag == true
                                  && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id)
                                  select a).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeBMCPUProgresscardReportDTO onchangecourse(CollegeBMCPUProgresscardReportDTO data)
        {
            try
            {
                data.getbranch = (from a in _examcontext.MasterCourseDMO
                                  from b in _examcontext.CLG_Adm_College_AY_CourseDMO
                                  from c in _examcontext.AcademicYear
                                  from d in _examcontext.ClgMasterBranchDMO
                                  from e in _examcontext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.AMCO_Id == b.AMCO_Id && b.ASMAY_Id == c.ASMAY_Id && a.AMCO_ActiveFlag == true && b.ACAYC_ActiveFlag == true
                                  && b.ACAYC_Id == e.ACAYC_Id && d.AMB_Id == e.AMB_Id && e.ACAYCB_ActiveFlag == true && d.AMB_ActiveFlag == true
                                  && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id)
                                  select d).Distinct().OrderBy(a => a.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeBMCPUProgresscardReportDTO onchangebranch(CollegeBMCPUProgresscardReportDTO data)
        {
            try
            {
                data.getsemester = (from a in _examcontext.MasterCourseDMO
                                    from b in _examcontext.CLG_Adm_College_AY_CourseDMO
                                    from c in _examcontext.AcademicYear
                                    from d in _examcontext.ClgMasterBranchDMO
                                    from e in _examcontext.CLG_Adm_College_AY_Course_BranchDMO
                                    from f in _examcontext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    from g in _examcontext.CLG_Adm_Master_SemesterDMO
                                    where (a.AMCO_Id == b.AMCO_Id && b.ASMAY_Id == c.ASMAY_Id && a.AMCO_ActiveFlag == true && b.ACAYC_ActiveFlag == true
                                    && b.ACAYC_Id == e.ACAYC_Id && d.AMB_Id == e.AMB_Id && e.ACAYCB_ActiveFlag == true && d.AMB_ActiveFlag == true
                                    && e.ACAYCB_Id == f.ACAYCB_Id && f.AMSE_Id == g.AMSE_Id && f.ACAYCBS_ActiveFlag == true && g.AMSE_ActiveFlg == true
                                    && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && e.AMB_Id == data.AMB_Id)
                                    select g).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeBMCPUProgresscardReportDTO onchangesemester(CollegeBMCPUProgresscardReportDTO data)
        {
            try
            {
                data.getsection = (from a in _examcontext.Adm_College_Yearly_StudentDMO
                                   from d in _examcontext.Adm_College_Master_SectionDMO
                                   where (a.ACMS_Id == d.ACMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id
                                   && a.AMSE_Id == data.AMSE_Id)
                                   select d).Distinct().OrderBy(a => a.ACMS_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeBMCPUProgresscardReportDTO onchangesection(CollegeBMCPUProgresscardReportDTO data)
        {
            try
            {
                data.getsubjectscheme = (from a in _examcontext.AdmCollegeSubjectSchemeDMO
                                         from b in _examcontext.Exm_Col_Yearly_SchemeDMO
                                         where (a.ACSS_Id == b.ACSS_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id
                                         && b.ECYS_ActiveFlag == true)
                                         select a).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeBMCPUProgresscardReportDTO onchangesubjectscheme(CollegeBMCPUProgresscardReportDTO data)
        {
            try
            {
                data.getschemetype = (from a in _examcontext.AdmCollegeSubjectSchemeDMO
                                      from b in _examcontext.Exm_Col_Yearly_SchemeDMO                                      
                                      from f in _examcontext.AdmCollegeSchemeTypeDMO
                                      where (a.ACSS_Id == b.ACSS_Id && b.ACST_Id == f.ACST_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id 
                                      && b.AMSE_Id == data.AMSE_Id && b.ECYS_ActiveFlag == true && b.ACSS_Id == data.ACSS_Id)
                                      select f).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeBMCPUProgresscardReportDTO onchangeschemetype(CollegeBMCPUProgresscardReportDTO data)
        {
            try
            {
                data.getexam = (from c in _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO
                                from d in _examcontext.col_exammasterDMO
                                where (c.EME_Id == d.EME_Id && c.AMCO_Id == data.AMCO_Id && c.AMB_Id == data.AMB_Id && c.AMSE_Id == data.AMSE_Id
                                && c.ECYSE_ActiveFlg == true && d.EME_ActiveFlag == true && c.ACSS_Id==data.ACSS_Id && c.ACST_Id==data.ACST_Id)
                                select d).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeBMCPUProgresscardReportDTO getreport(CollegeBMCPUProgresscardReportDTO data)
        {
            try
            {
                data.instname = _examcontext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                List<CollegeBMCPUProgresscardReportDTO> result = new List<CollegeBMCPUProgresscardReportDTO>();
                using (var cmd = _examcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_Exam_get_BB_Exam_Details_PU";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.AMCO_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.AMB_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ACSS_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.ACSS_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.AMSE_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.EME_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.ACMS_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACST_Id",
                       SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.ACST_Id)
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
                                result.Add(new CollegeBMCPUProgresscardReportDTO
                                {
                                    ECSTMPS_ObtainedMarks = Convert.ToDecimal(dataReader["ECSTMPS_ObtainedMarks"].ToString() == null || dataReader["ECSTMPS_ObtainedMarks"].ToString() == "" ? "0" : dataReader["ECSTMPS_ObtainedMarks"].ToString()),

                                    ECSTMPS_ObtainedGrade = (dataReader["ECSTMPS_ObtainedGrade"].ToString() == null || dataReader["ECSTMPS_ObtainedGrade"].ToString() == "" ? "" : dataReader["ECSTMPS_ObtainedGrade"].ToString()),

                                    ECSTMPS_PassFailFlg = (dataReader["ECSTMPS_PassFailFlg"].ToString() == null || dataReader["ECSTMPS_PassFailFlg"].ToString() == "" ? "" : dataReader["ECSTMPS_PassFailFlg"].ToString()),

                                    EME_ExamName = (dataReader["EME_ExamName"].ToString() == null || dataReader["EME_ExamName"].ToString() == "" ? "" : dataReader["EME_ExamName"].ToString()),

                                    AMCO_CourseName = (dataReader["AMCO_CourseName"].ToString() == null || dataReader["AMCO_CourseName"].ToString() == "" ? "" : dataReader["AMCO_CourseName"].ToString()),

                                    AMB_BranchName = (dataReader["AMB_BranchName"].ToString() == null || dataReader["AMB_BranchName"].ToString() == "" ? "" : dataReader["AMB_BranchName"].ToString()),

                                    AMSE_SEMName = (dataReader["AMSE_SEMName"].ToString() == null || dataReader["AMSE_SEMName"].ToString() == "" ? "" : dataReader["AMSE_SEMName"].ToString()),

                                    ACMS_SectionName = (dataReader["ACMS_SectionName"].ToString() == null || dataReader["ACMS_SectionName"].ToString() == "" ? "" : dataReader["ACMS_SectionName"].ToString()),

                                    AMCST_Id = Convert.ToInt32(dataReader["AMCST_Id"].ToString()),

                                    AMCST_FirstName = ((dataReader["AMCST_FirstName"].ToString() == null ? " " : dataReader["AMCST_FirstName"].ToString()) + " " + (dataReader["AMCST_MiddleName"].ToString() == null ? " " : dataReader["AMCST_MiddleName"].ToString()) + " " + (dataReader["AMCST_LastName"].ToString() == null ? " " : dataReader["AMCST_LastName"].ToString())).Trim(),

                                    AMCST_DOB = Convert.ToDateTime(dataReader["AMCST_DOB"].ToString() == null || dataReader["AMCST_DOB"].ToString() == "" ? "" : dataReader["AMCST_DOB"].ToString()),

                                    ACYST_RollNo = Convert.ToInt64(dataReader["ACYST_RollNo"].ToString() == null || dataReader["ACYST_RollNo"].ToString() == "" ? "" : dataReader["ACYST_RollNo"].ToString()),

                                    AMCST_AdmNo = (dataReader["AMCST_AdmNo"].ToString() == null || dataReader["AMCST_AdmNo"].ToString() == "" ? "" : dataReader["AMCST_AdmNo"].ToString()),

                                    AMCST_RegistrationNo = (dataReader["AMCST_RegistrationNo"].ToString() == null || dataReader["AMCST_RegistrationNo"].ToString() == "" ? "" : dataReader["AMCST_RegistrationNo"].ToString()),

                                    ISMS_Id = Convert.ToInt32(dataReader["ISMS_Id"].ToString()),

                                    ISMS_SubjectName = (dataReader["ISMS_SubjectName"].ToString() == null || dataReader["ISMS_SubjectName"].ToString() == "" ? "" : dataReader["ISMS_SubjectName"].ToString()),

                                    ECSTMPSSS_MaxMarks = Convert.ToDecimal(dataReader["ECSTMPSSS_MaxMarks"].ToString() == null || dataReader["ECSTMPSSS_MaxMarks"].ToString() == "" ? "0" : dataReader["ECSTMPSSS_MaxMarks"].ToString()),

                                    ECSTMPS_SectionAverage = Convert.ToDecimal(dataReader["ECSTMPS_SectionAverage"].ToString() == null || dataReader["ECSTMPS_SectionAverage"].ToString() == "" ? "0" : dataReader["ECSTMPS_SectionAverage"].ToString()),

                                    ECSTMPS_SemAverage = Convert.ToDecimal(dataReader["ECSTMPS_SemAverage"].ToString() == null || dataReader["ECSTMPS_SemAverage"].ToString() == "" ? "0" : dataReader["ECSTMPS_SemAverage"].ToString()),

                                    ECSTMPS_SemHighest = Convert.ToDecimal(dataReader["ECSTMPS_SemHighest"].ToString() == null || dataReader["ECSTMPS_SemHighest"].ToString() == "" ? "0" : dataReader["ECSTMPS_SemHighest"].ToString()),

                                    ECSTMPS_SectionHighest = Convert.ToDecimal(dataReader["ECSTMPS_SectionHighest"].ToString() == null || dataReader["ECSTMPS_SectionHighest"].ToString() == "" ? "0" : dataReader["ECSTMPS_SectionHighest"].ToString()),

                                    ISMS_SubjectCode = (dataReader["ISMS_SubjectCode"].ToString() == null || dataReader["ISMS_SubjectCode"].ToString() == "" ? "" : dataReader["ISMS_SubjectCode"].ToString()),

                                    ECYSES_AplResultFlg = Convert.ToBoolean(dataReader["ECYSES_AplResultFlg"].ToString()),

                                    ECYSES_MaxMarks = Convert.ToDecimal(dataReader["ECYSES_MaxMarks"].ToString() == null || dataReader["ECYSES_MaxMarks"].ToString() == "" ? "0" : dataReader["ECYSES_MaxMarks"].ToString()),

                                    ECYSES_MinMarks = Convert.ToDecimal(dataReader["ECYSES_MinMarks"].ToString() == null || dataReader["ECYSES_MinMarks"].ToString() == "" ? "0" : dataReader["ECYSES_MinMarks"].ToString()),

                                    EMGR_Id = Convert.ToInt32(dataReader["EMGR_Id"].ToString()),

                                    ASA_ClassHeld = Convert.ToDecimal(dataReader["ASA_ClassHeld"].ToString() == null || dataReader["ASA_ClassHeld"].ToString() == "" ? "0" : dataReader["ASA_ClassHeld"].ToString()),

                                    ASA_Class_Attended = Convert.ToDecimal(dataReader["ASA_Class_Attended"].ToString() == null || dataReader["ASA_Class_Attended"].ToString() == "" ? "0" : dataReader["ASA_Class_Attended"].ToString()),

                                    EMGD_Remarks = (dataReader["EMGD_Remarks"].ToString() == null || dataReader["EMGD_Remarks"].ToString() == "" ? "0" : dataReader["EMGD_Remarks"].ToString()),

                                    ECSTMP_TotalObtMarks = Convert.ToDecimal(dataReader["ECSTMP_TotalObtMarks"].ToString() == null || dataReader["ECSTMP_TotalObtMarks"].ToString() == "" ? "0" : dataReader["ECSTMP_TotalObtMarks"].ToString()),

                                    ECSTMP_Percentage = Convert.ToDecimal(dataReader["ECSTMP_Percentage"].ToString() == null || dataReader["ECSTMP_Percentage"].ToString() == "" ? "0" : dataReader["ECSTMP_Percentage"].ToString()),

                                    ECSTMP_TotalGrade = (dataReader["ECSTMP_TotalGrade"].ToString() == null || dataReader["ECSTMP_TotalGrade"].ToString() == "" ? "" : dataReader["ECSTMP_TotalGrade"].ToString()),

                                    ECSTMP_SemRank = Convert.ToInt16(dataReader["ECSTMP_SemRank"].ToString() == null || dataReader["ECSTMP_SemRank"].ToString() == "" ? "" : dataReader["ECSTMP_SemRank"].ToString()),

                                    ECSTMP_SectionRank = Convert.ToInt16(dataReader["ECSTMP_SectionRank"].ToString() == null || dataReader["ECSTMP_SectionRank"].ToString() == "" ? "" : dataReader["ECSTMP_SectionRank"].ToString()),

                                    TotalGrade = (dataReader["TotalGrade"].ToString() == null || dataReader["TotalGrade"].ToString() == "" ? "" : dataReader["TotalGrade"].ToString()),

                                    ECSTMP_TotalMaxMarks = Convert.ToDecimal(dataReader["ECSTMP_TotalMaxMarks"].ToString() == null || dataReader["ECSTMP_TotalMaxMarks"].ToString() == "0" ? "" : dataReader["ECSTMP_TotalMaxMarks"].ToString()),

                                    ECYSES_SubjectOrder = Convert.ToInt16(dataReader["ECYSES_SubjectOrder"].ToString() == null || dataReader["ECYSES_SubjectOrder"].ToString() == "" ? "" : dataReader["ECYSES_SubjectOrder"].ToString()),

                                    ECYSES_MarksDisplayFlg = Convert.ToBoolean(dataReader["ECYSES_MarksDisplayFlg"].ToString()),

                                    ECYSES_GradeDisplayFlg = Convert.ToBoolean(dataReader["ECYSES_GradeDisplayFlg"].ToString()),

                                    ECSTMP_Result = (dataReader["ECSTMP_Result"].ToString() == null || dataReader["ECSTMP_Result"].ToString() == "" ? "" : dataReader["ECSTMP_Result"].ToString()),

                                    EMSE_SubExamName = (dataReader["EMSE_SubExamName"].ToString() == null || dataReader["EMSE_SubExamName"].ToString() == "" ? "" : dataReader["EMSE_SubExamName"].ToString()),

                                    ECSTMPSSS_ObtainedMarks = Convert.ToDecimal(dataReader["ECSTMPSSS_ObtainedMarks"].ToString() == null || dataReader["ECSTMPSSS_ObtainedMarks"].ToString() == "" ? "0" : dataReader["ECSTMPSSS_ObtainedMarks"].ToString())

                                });

                                data.savelist = result.OrderBy(t => t.ECYSES_SubjectOrder).ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                //using (var cmd = _examcontext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "StudentAttendance_W";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //      SqlDbType.BigInt)
                //    {
                //        Value = data.MI_Id
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                //                 SqlDbType.BigInt)
                //    {
                //        Value = data.ASMAY_Id
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                //        SqlDbType.BigInt)
                //    {
                //        Value = data.ASMCL_Id
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                //        SqlDbType.BigInt)
                //    {
                //        Value = data.ASMS_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@from",
                //        SqlDbType.Date)
                //    {
                //        Value = from_date
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@to",
                //        SqlDbType.Date)
                //    {
                //        Value = to_date
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
                //                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                //                {
                //                    dataRow1.Add(
                //                        dataReader.GetName(iFiled1),
                //                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                //                    );
                //                }

                //                retObject.Add((ExpandoObject)dataRow1);
                //            }
                //        }
                //        data.Work_attendence = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        _acdimpl.LogError(ex.Message);
                //        _acdimpl.LogDebug(ex.Message);
                //    }
                //}

                //using (var cmd = _examcontext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "StudentAttendance_P";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //      SqlDbType.BigInt)
                //    {
                //        Value = data.MI_Id
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                //                 SqlDbType.BigInt)
                //    {
                //        Value = data.ASMAY_Id
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                //        SqlDbType.BigInt)
                //    {
                //        Value = data.ASMCL_Id
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                //        SqlDbType.BigInt)
                //    {
                //        Value = data.ASMS_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@from",
                //        SqlDbType.Date)
                //    {
                //        Value = from_date
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@to",
                //        SqlDbType.Date)
                //    {
                //        Value = to_date
                //    });


                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject1 = new List<dynamic>();

                //    try
                //    {

                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                //                {
                //                    dataRow1.Add(
                //                        dataReader.GetName(iFiled1),
                //                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                //                    );
                //                }

                //                retObject1.Add((ExpandoObject)dataRow1);
                //            }

                //        }
                //        data.Present_attendence = retObject1.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        _acdimpl.LogError(ex.Message);
                //        _acdimpl.LogDebug(ex.Message);
                //    }
                //}

                data.savelisttot = _examcontext.CLG_Exm_Col_Student_Marks_ProcessDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.AMCO_Id == data.AMCO_Id && t.MI_Id == data.MI_Id && t.AMB_Id == data.AMB_Id && t.EME_Id == data.EME_Id && t.AMSE_Id == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id).Distinct().ToArray();

                data.subjlist = data.savelist.Distinct<CollegeBMCPUProgresscardReportDTO>(new progressEqualityComparerPUC()).OrderBy(t => t.ECYSES_SubjectOrder).ToArray();

                List<int> grade = new List<int>();
                foreach (CollegeBMCPUProgresscardReportDTO x in data.subjlist)
                {
                    grade.Add(x.EMGR_Id);
                }

                data.grade_details = (from a in _examcontext.Exm_Master_GradeDMO
                                      from b in _examcontext.Exm_Master_Grade_DetailsDMO
                                      where (a.MI_Id == data.MI_Id && grade.Contains(a.EMGR_Id) && a.EMGR_Id == b.EMGR_Id)
                                      select b
                                     ).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }

    }

    class progressEqualityComparerPUC : IEqualityComparer<CollegeBMCPUProgresscardReportDTO>
    {
        public bool Equals(CollegeBMCPUProgresscardReportDTO b1, CollegeBMCPUProgresscardReportDTO b2)
        {
            if (b2 == null && b1 == null)
                return true;
            else if (b1 == null | b2 == null)
                return false;
            else if (b1.ISMS_Id == b2.ISMS_Id)
                return true;
            else
                return false;
        }

        public int GetHashCode(CollegeBMCPUProgresscardReportDTO bx)
        {
            int hCode = Convert.ToInt32(bx.ISMS_Id);
            return hCode.GetHashCode();
        }
    }
}
