using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using DomainModel.Model.com.vaps.Exam;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Services
{
    public class ClgCumulativeReportIMPL : Interfaces.ClgCumulativeReportInterface
    {
        private static ConcurrentDictionary<string, ClgCumulativeReportDTO> _login = new ConcurrentDictionary<string, ClgCumulativeReportDTO>();
        readonly ILogger<ClgCumulativeReportIMPL> _acdimpl;
        private readonly ClgExamContext _CRContext;
        public DomainModelMsSqlServerContext _db;
        public ClgCumulativeReportIMPL(ClgExamContext cpContext, DomainModelMsSqlServerContext db, ILogger<ClgCumulativeReportIMPL> acdimpl)
        {
            _CRContext = cpContext;
            _db = db;
            _acdimpl = acdimpl;
        }

        public ClgCumulativeReportDTO Getdetails(ClgCumulativeReportDTO data)
        {
            ClgCumulativeReportDTO getdata = new ClgCumulativeReportDTO();
            try
            {
                //getdata.courseslist = _CRContext.MasterCourseDMO.Where(c => c.MI_Id == data.MI_Id && c.AMCO_ActiveFlag == true).ToList().Distinct().ToArray();
                //getdata.branchlist = _CRContext.ClgMasterBranchDMO.Where(c => c.MI_Id == data.MI_Id && c.AMB_ActiveFlag == true).ToList().Distinct().ToArray();
                //getdata.semisters = _CRContext.CLG_Adm_Master_SemesterDMO.Where(c => c.MI_Id == data.MI_Id && c.AMSE_ActiveFlg == true).ToList().Distinct().ToArray();
                //getdata.exmstdlist = _CRContext.col_exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).OrderBy(t => t.EME_ExamOrder).ToList().ToArray();
                //getdata.subjectshemalist = _CRContext.AdmCollegeSubjectSchemeDMO.Where(c => c.MI_Id == data.MI_Id && c.ACST_ActiveFlg == true).ToList().Distinct().ToArray();
                //getdata.schmetypelist = _CRContext.AdmCollegeSchemeTypeDMO.Where(c => c.MI_Id == data.MI_Id && c.ACST_ActiveFlg == true).ToList().Distinct().ToArray();
                //getdata.sections = _CRContext.Adm_College_Master_SectionDMO.Where(t => t.MI_Id == data.MI_Id && t.ACMS_ActiveFlag == true).ToList().ToArray();

                getdata.yearlist = _CRContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();


            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;

        }
        public ClgCumulativeReportDTO onchangeyear(ClgCumulativeReportDTO data)
        {
            try
            {
                data.courseslist = (from a in _CRContext.MasterCourseDMO
                                    from b in _CRContext.CLG_Adm_College_AY_CourseDMO
                                    from c in _CRContext.AcademicYear
                                    where (a.AMCO_Id == b.AMCO_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id)
                                    select a).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgCumulativeReportDTO onchangecourse(ClgCumulativeReportDTO data)
        {
            try
            {
                data.branchlist = (from a in _CRContext.ClgMasterBranchDMO
                                   from b in _CRContext.CLG_Adm_College_AY_CourseDMO
                                   from c in _CRContext.CLG_Adm_College_AY_Course_BranchDMO
                                   from d in _CRContext.AcademicYear
                                   where (a.AMB_Id == c.AMB_Id && b.ACAYC_Id == c.ACAYC_Id && b.ASMAY_Id == d.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id
                                   && a.MI_Id == data.MI_Id && b.AMCO_Id == data.AMCO_Id)
                                   select a).Distinct().OrderBy(a => a.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgCumulativeReportDTO onchangebranch(ClgCumulativeReportDTO data)
        {
            try
            {
                data.semisters = (from a in _CRContext.CLG_Adm_Master_SemesterDMO
                                  from b in _CRContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                  from c in _CRContext.CLG_Adm_College_AY_CourseDMO
                                  from d in _CRContext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (b.AMSE_Id == a.AMSE_Id && a.MI_Id == data.MI_Id && c.ACAYC_Id == d.ACAYC_Id && b.ACAYCB_Id == d.ACAYCB_Id && c.AMCO_Id == data.AMCO_Id && d.AMB_Id == data.AMB_Id && c.ASMAY_Id == data.ASMAY_Id)
                                  select a).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgCumulativeReportDTO onchangesemester(ClgCumulativeReportDTO data)
        {
            try
            {
                data.sections = _CRContext.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag == true).Distinct().OrderBy(a => a.ACMS_Order).ToArray();

                data.subjectshemalist = (from a in _CRContext.Exm_Col_Yearly_SchemeDMO
                                         from b in _CRContext.AdmCollegeSubjectSchemeDMO
                                         where (a.ACSS_Id == b.ACSS_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id
                                         && a.AMSE_Id == data.AMSE_ID && a.ECYS_ActiveFlag == true && b.ACST_ActiveFlg == true)
                                         select b).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgCumulativeReportDTO onchangesubjectscheme(ClgCumulativeReportDTO data)
        {
            try
            {
                data.schmetypelist = (from a in _CRContext.Exm_Col_Yearly_SchemeDMO
                                      from b in _CRContext.AdmCollegeSubjectSchemeDMO
                                      from c in _CRContext.AdmCollegeSchemeTypeDMO
                                      where (a.ACSS_Id == b.ACSS_Id && a.ACST_Id == c.ACST_Id && a.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id
                                      && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_ID && a.ACSS_Id == data.ACSS_Id)
                                      select c).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgCumulativeReportDTO onchangeschemetype(ClgCumulativeReportDTO data)
        {
            try
            {
                data.exmstdlist = (from a in _CRContext.Exm_Col_Yearly_SchemeDMO
                                   from b in _CRContext.Exm_Col_Yearly_Scheme_ExamsDMO
                                   from c in _CRContext.col_exammasterDMO
                                   where (a.ECYS_Id == b.ECYS_Id && b.EME_Id == c.EME_Id && c.MI_Id == data.MI_Id && a.MI_Id == data.MI_Id
                                   && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id
                                   && a.AMSE_Id == data.AMSE_ID && a.ACSS_Id == data.ACSS_Id && a.ACST_Id == data.ACST_Id && a.ECYS_ActiveFlag == true
                                   && b.ECYSE_ActiveFlg == true && c.EME_ActiveFlag == true)
                                   select c).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();



                data.studentlist = (from a in _CRContext.Adm_College_Yearly_StudentDMO
                                    from b in _CRContext.Adm_Master_College_StudentDMO
                                    from c in _CRContext.AcademicYear
                                    from d in _CRContext.MasterCourseDMO
                                    from e in _CRContext.ClgMasterBranchDMO
                                    from f in _CRContext.CLG_Adm_Master_SemesterDMO
                                    from g in _CRContext.Adm_College_Master_SectionDMO
                                    from h in _CRContext.AdmCollegeSubjectSchemeDMO
                                    from i in _CRContext.AdmCollegeSchemeTypeDMO
                                    where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == c.ASMAY_Id && a.AMCO_Id == d.AMCO_Id && a.AMB_Id == e.AMB_Id
                                    && a.AMSE_Id == f.AMSE_Id && a.ACMS_Id == g.ACMS_Id && b.ACSS_Id == h.ACSS_Id && b.ACST_Id == i.ACST_Id
                                    && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_ID
                                    && a.ACMS_Id == data.ACMS_Id && b.ACST_Id == data.ACST_Id && b.ACSS_Id == data.ACSS_Id)
                                    select new ClgCumulativeReportDTO
                                    {
                                        studentname = ((b.AMCST_FirstName == null && b.AMCST_FirstName == "" ? "" : b.AMCST_FirstName) +
                                        (b.AMCST_MiddleName == null && b.AMCST_MiddleName == "" ? "" : " " + b.AMCST_MiddleName) +
                                        (b.AMCST_LastName == null && b.AMCST_LastName == "" ? "" : " " + b.AMCST_LastName) +
                                        (b.AMCST_AdmNo == null && b.AMCST_AdmNo == "" ? "" : " : " + b.AMCST_AdmNo)).Trim(),
                                        AMCST_AdmNo = b.AMCST_AdmNo,
                                        AMCST_RegistrationNo = b.AMCST_RegistrationNo,
                                        AMCST_Id = b.AMCST_Id,
                                    }).Distinct().OrderBy(a => a.studentname).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ClgCumulativeReportDTO> Getcmreport(ClgCumulativeReportDTO data)
        {
            try
            {
                string order = "";
                var get_configuration = _CRContext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                data.configuration = get_configuration.ToArray();

                if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                {
                    order = "AMST_FirstName";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "AdmNo")
                {
                    order = "AMST_AdmNo";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RollNo")
                {
                    order = "AMAY_RollNo";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RegNo")
                {
                    order = "AMST_RegistrationNo";
                }
                else
                {
                    order = "AMST_FirstName";
                }


                data.instname = _CRContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                List<ClgCumulativeReportDTO> result = new List<ClgCumulativeReportDTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "CLG_Exam_cumulative_BB_Report_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = Convert.ToInt32(data.AMCO_Id) });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = Convert.ToInt32(data.AMB_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ACSS_Id", SqlDbType.VarChar) { Value = Convert.ToInt32(data.ACSS_Id) });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToInt32(data.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_ID", SqlDbType.VarChar) { Value = Convert.ToInt32(data.AMSE_ID) });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = Convert.ToInt32(data.EME_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = Convert.ToInt32(data.ACMS_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToInt32(data.ASMAY_Id) });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                result.Add(new ClgCumulativeReportDTO
                                {
                                    MI_name = (dataReader["MI_name"].ToString().Trim() == null || dataReader["MI_name"].ToString().Trim() == "" ? "" : dataReader["MI_name"].ToString()),

                                    ECSTMPS_ObtainedMarks = Convert.ToDecimal(dataReader["ECSTMPS_ObtainedMarks"].ToString() == null || dataReader["ECSTMPS_ObtainedMarks"].ToString() == "" ? "0" : dataReader["ECSTMPS_ObtainedMarks"].ToString()),
                                    ECSTMPS_ObtainedGrade = (dataReader["ECSTMPS_ObtainedGrade"].ToString() == null || dataReader["ECSTMPS_ObtainedGrade"].ToString() == "" ? "" : dataReader["ECSTMPS_ObtainedGrade"].ToString()),
                                    ECSTMPS_PassFailFlg = (dataReader["ECSTMPS_PassFailFlg"].ToString() == null || dataReader["ECSTMPS_PassFailFlg"].ToString() == "" ? "" : dataReader["ECSTMPS_PassFailFlg"].ToString()),
                                    EME_ExamName = (dataReader["EME_ExamName"].ToString() == null || dataReader["EME_ExamName"].ToString() == "" ? "" : dataReader["EME_ExamName"].ToString()),
                                    AMCO_CourseName = (dataReader["AMCO_CourseName"].ToString() == null || dataReader["AMCO_CourseName"].ToString() == "" ? "" : dataReader["AMCO_CourseName"].ToString()),
                                    AMB_BranchName = (dataReader["AMB_BranchName"].ToString() == null || dataReader["AMB_BranchName"].ToString() == "" ? "" : dataReader["AMB_BranchName"].ToString()),
                                    AMSE_SEMName = (dataReader["AMSE_SEMName"].ToString() == null || dataReader["AMSE_SEMName"].ToString() == "" ? "" : dataReader["AMSE_SEMName"].ToString()),
                                    ACMS_SectionName = (dataReader["ACMS_SectionName"].ToString() == null || dataReader["ACMS_SectionName"].ToString() == "" ? "" : dataReader["ACMS_SectionName"].ToString()),
                                    AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                    AMST_FirstName = ((dataReader["AMST_FirstName"].ToString() == null || dataReader["AMST_FirstName"].ToString() == "" ? "" : dataReader["AMST_FirstName"].ToString()) + (dataReader["AMST_MiddleName"].ToString() == null || dataReader["AMST_MiddleName"].ToString() == "" ? "" : " " + dataReader["AMST_MiddleName"].ToString()) +
                                    (dataReader["AMST_LastName"].ToString() == null || dataReader["AMST_LastName"].ToString() == "" ? "" : " " + dataReader["AMST_LastName"].ToString())).Trim(),
                                    AMST_DOB = Convert.ToDateTime(dataReader["AMST_DOB"].ToString() == null || dataReader["AMST_DOB"].ToString() == "" ? "" : dataReader["AMST_DOB"].ToString()),
                                    AMAY_RollNo = Convert.ToInt32(dataReader["AMAY_RollNo"].ToString() == null || dataReader["AMAY_RollNo"].ToString() == "" ? "" : dataReader["AMAY_RollNo"].ToString()),
                                    AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString() == null || dataReader["AMST_AdmNo"].ToString() == "" ? "" : dataReader["AMST_AdmNo"].ToString()),
                                    ISMS_Id = Convert.ToInt32(dataReader["ISMS_Id"].ToString()),
                                    ISMS_SubjectName = (dataReader["ISMS_SubjectName"].ToString() == null || dataReader["ISMS_SubjectName"].ToString() == "" ? "" : dataReader["ISMS_SubjectName"].ToString()),
                                    ECSTMPSSS_MaxMarks = Convert.ToDecimal(dataReader["ECSTMPSSS_MaxMarks"].ToString() == null || dataReader["ECSTMPSSS_MaxMarks"].ToString() == "" ? "0" : dataReader["ECSTMPSSS_MaxMarks"].ToString()),
                                    ECSTMPS_SemAverage = Convert.ToDecimal(dataReader["ECSTMPS_SemAverage"].ToString() == null || dataReader["ECSTMPS_SemAverage"].ToString() == "" ? "0" : dataReader["ECSTMPS_SemAverage"].ToString()),
                                    ECSTMPS_SectionAverage = Convert.ToDecimal(dataReader["ECSTMPS_SectionAverage"].ToString() == null || dataReader["ECSTMPS_SectionAverage"].ToString() == "" ? "0" : dataReader["ECSTMPS_SectionAverage"].ToString()),
                                    ECSTMPS_SectionHighest = Convert.ToDecimal(dataReader["ECSTMPS_SectionHighest"].ToString() == null || dataReader["ECSTMPS_SectionHighest"].ToString() == "" ? "0" : dataReader["ECSTMPS_SectionHighest"].ToString()),
                                    ISMS_SubjectCode = (dataReader["ISMS_SubjectCode"].ToString() == null || dataReader["ISMS_SubjectCode"].ToString() == "" ? "" : dataReader["ISMS_SubjectCode"].ToString()),
                                    ECYSES_AplResultFlg = Convert.ToBoolean(dataReader["ECYSES_AplResultFlg"].ToString()),
                                    ECYSES_MaxMarks = Convert.ToDecimal(dataReader["ECYSES_MaxMarks"].ToString() == null || dataReader["ECYSES_MaxMarks"].ToString() == "" ? "0" : dataReader["ECYSES_MaxMarks"].ToString()),
                                    ECYSES_MinMarks = Convert.ToDecimal(dataReader["ECYSES_MinMarks"].ToString() == null || dataReader["ECYSES_MinMarks"].ToString() == "" ? "0" : dataReader["ECYSES_MinMarks"].ToString()),
                                    EMGR_Id = Convert.ToInt32(dataReader["EMGR_Id"].ToString()),
                                    classheld = Convert.ToDecimal(dataReader["ASA_ClassHeld"].ToString() == null || dataReader["ASA_ClassHeld"].ToString() == "" ? "0" : dataReader["ASA_ClassHeld"].ToString()),
                                    classatt = Convert.ToDecimal(dataReader["ASA_Class_Attended"].ToString() == null || dataReader["ASA_Class_Attended"].ToString() == "" ? "0" : dataReader["ASA_Class_Attended"].ToString()),
                                    graderemark = (dataReader["EMGD_Remarks"].ToString() == null || dataReader["EMGD_Remarks"].ToString() == "" ? "0" : dataReader["EMGD_Remarks"].ToString()),
                                    ECSTMP_TotalObtMarks = Convert.ToDecimal(dataReader["ECSTMP_TotalObtMarks"].ToString() == null || dataReader["ECSTMP_TotalObtMarks"].ToString() == "" ? "0" : dataReader["ECSTMP_TotalObtMarks"].ToString()),
                                    ECSTMP_Percentage = Convert.ToDecimal(dataReader["ECSTMP_Percentage"].ToString() == null || dataReader["ECSTMP_Percentage"].ToString() == "" ? "0" : dataReader["ECSTMP_Percentage"].ToString()),
                                    ECSTMP_TotalGrade = (dataReader["ECSTMP_TotalGrade"].ToString() == null || dataReader["ECSTMP_TotalGrade"].ToString() == "" ? "" : dataReader["ECSTMP_TotalGrade"].ToString()),
                                    ECSTMP_SemRank = Convert.ToInt16(dataReader["ECSTMP_SemRank"].ToString() == null || dataReader["ECSTMP_SemRank"].ToString() == "" ? "" : dataReader["ECSTMP_SemRank"].ToString()),
                                    ECSTMP_SectionRank = Convert.ToInt16(dataReader["ECSTMP_SectionRank"].ToString() == null || dataReader["ECSTMP_SectionRank"].ToString() == "" ? "" : dataReader["ECSTMP_SectionRank"].ToString()),
                                    ECSTMP_TotalGradeRemark = (dataReader["ECSTMP_TotalGradeRemark"].ToString() == null || dataReader["ECSTMP_TotalGradeRemark"].ToString() == "" ? "" : dataReader["ECSTMP_TotalGradeRemark"].ToString()),
                                    ECSTMP_TotalMaxMarks = Convert.ToDecimal(dataReader["ECSTMP_TotalMaxMarks"].ToString() == null || dataReader["ECSTMP_TotalMaxMarks"].ToString() == "0" ? "" : dataReader["ECSTMP_TotalMaxMarks"].ToString()),
                                    //ECYSES_SubjectOrder = Convert.ToInt16(dataReader["ECYSES_SubjectOrder"].ToString() == null || dataReader["ECYSES_SubjectOrder"].ToString() == "" ? "" : dataReader["ECYSES_SubjectOrder"].ToString()),
                                    ECSTMP_Result = (dataReader["ECSTMP_Result"].ToString() == null || dataReader["ECSTMP_Result"].ToString() == "" ? "" : dataReader["ECSTMP_Result"].ToString()),

                                });

                                var propertyInfo = typeof(ClgCumulativeReportDTO).GetProperty(order);
                                result = result.OrderBy(x => propertyInfo.GetValue(x, null)).ThenBy(x => x.ECYSES_SubjectOrder).ToList();
                                data.savelist = result.Distinct().ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }

                List<long> subs = new List<long>();

                if (data.savelist != null && data.savelist.Count > 0)
                {
                    data.subjlist = data.savelist.Distinct<ClgCumulativeReportDTO>(new CumulativeEqualityComparer()).ToArray();
                }
                List<ClgCumulativeReportDTO> result1 = new List<ClgCumulativeReportDTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "CLG_Exam_cumulative_BB_Report_Elective_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = Convert.ToInt32(data.AMCO_Id) });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = Convert.ToInt32(data.AMB_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ACSS_Id", SqlDbType.VarChar) { Value = Convert.ToInt32(data.ACSS_Id) });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToInt32(data.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_ID", SqlDbType.VarChar) { Value = Convert.ToInt32(data.AMSE_ID) });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = Convert.ToInt32(data.EME_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = Convert.ToInt32(data.ACMS_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToInt32(data.ASMAY_Id) });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                result1.Add(new ClgCumulativeReportDTO
                                {
                                    MI_name = (dataReader["MI_name"].ToString().Trim() == null || dataReader["MI_name"].ToString().Trim() == "" ? "" : dataReader["MI_name"].ToString()),

                                    ECSTMPS_ObtainedMarks = Convert.ToDecimal(dataReader["ECSTMPS_ObtainedMarks"].ToString() == null || dataReader["ECSTMPS_ObtainedMarks"].ToString() == "" ? "0" : dataReader["ECSTMPS_ObtainedMarks"].ToString()),
                                    ECSTMPS_ObtainedGrade = (dataReader["ECSTMPS_ObtainedGrade"].ToString() == null || dataReader["ECSTMPS_ObtainedGrade"].ToString() == "" ? "" : dataReader["ECSTMPS_ObtainedGrade"].ToString()),
                                    ECSTMPS_PassFailFlg = (dataReader["ECSTMPS_PassFailFlg"].ToString() == null || dataReader["ECSTMPS_PassFailFlg"].ToString() == "" ? "" : dataReader["ECSTMPS_PassFailFlg"].ToString()),
                                    EME_ExamName = (dataReader["EME_ExamName"].ToString() == null || dataReader["EME_ExamName"].ToString() == "" ? "" : dataReader["EME_ExamName"].ToString()),
                                    AMCO_CourseName = (dataReader["AMCO_CourseName"].ToString() == null || dataReader["AMCO_CourseName"].ToString() == "" ? "" : dataReader["AMCO_CourseName"].ToString()),
                                    AMB_BranchName = (dataReader["AMB_BranchName"].ToString() == null || dataReader["AMB_BranchName"].ToString() == "" ? "" : dataReader["AMB_BranchName"].ToString()),
                                    AMSE_SEMName = (dataReader["AMSE_SEMName"].ToString() == null || dataReader["AMSE_SEMName"].ToString() == "" ? "" : dataReader["AMSE_SEMName"].ToString()),
                                    ACMS_SectionName = (dataReader["ACMS_SectionName"].ToString() == null || dataReader["ACMS_SectionName"].ToString() == "" ? "" : dataReader["ACMS_SectionName"].ToString()),
                                    AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                    AMST_FirstName = ((dataReader["AMST_FirstName"].ToString() == null ? " " : dataReader["AMST_FirstName"].ToString()) + " " + (dataReader["AMST_MiddleName"].ToString() == null ? " " : dataReader["AMST_MiddleName"].ToString()) + " " + (dataReader["AMST_LastName"].ToString() == null ? " " : dataReader["AMST_LastName"].ToString())).Trim(),
                                    AMST_DOB = Convert.ToDateTime(dataReader["AMST_DOB"].ToString() == null || dataReader["AMST_DOB"].ToString() == "" ? "" : dataReader["AMST_DOB"].ToString()),
                                    AMAY_RollNo = Convert.ToInt32(dataReader["AMAY_RollNo"].ToString() == null || dataReader["AMAY_RollNo"].ToString() == "" ? "" : dataReader["AMAY_RollNo"].ToString()),
                                    AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString() == null || dataReader["AMST_AdmNo"].ToString() == "" ? "" : dataReader["AMST_AdmNo"].ToString()),
                                    ISMS_Id = Convert.ToInt32(dataReader["ISMS_Id"].ToString()),
                                    ISMS_SubjectName = (dataReader["ISMS_SubjectName"].ToString() == null || dataReader["ISMS_SubjectName"].ToString() == "" ? "" : dataReader["ISMS_SubjectName"].ToString()),
                                    ECSTMPSSS_MaxMarks = Convert.ToDecimal(dataReader["ECSTMPSSS_MaxMarks"].ToString() == null || dataReader["ECSTMPSSS_MaxMarks"].ToString() == "" ? "0" : dataReader["ECSTMPSSS_MaxMarks"].ToString()),
                                    ECSTMPS_SemAverage = Convert.ToDecimal(dataReader["ECSTMPS_SemAverage"].ToString() == null || dataReader["ECSTMPS_SemAverage"].ToString() == "" ? "0" : dataReader["ECSTMPS_SemAverage"].ToString()),
                                    ECSTMPS_SectionAverage = Convert.ToDecimal(dataReader["ECSTMPS_SectionAverage"].ToString() == null || dataReader["ECSTMPS_SectionAverage"].ToString() == "" ? "0" : dataReader["ECSTMPS_SectionAverage"].ToString()),
                                    ECSTMPS_SectionHighest = Convert.ToDecimal(dataReader["ECSTMPS_SectionHighest"].ToString() == null || dataReader["ECSTMPS_SectionHighest"].ToString() == "" ? "0" : dataReader["ECSTMPS_SectionHighest"].ToString()),
                                    ISMS_SubjectCode = (dataReader["ISMS_SubjectCode"].ToString() == null || dataReader["ISMS_SubjectCode"].ToString() == "" ? "" : dataReader["ISMS_SubjectCode"].ToString()),
                                    ECYSES_AplResultFlg = Convert.ToBoolean(dataReader["ECYSES_AplResultFlg"].ToString()),
                                    ECYSES_MaxMarks = Convert.ToDecimal(dataReader["ECYSES_MaxMarks"].ToString() == null || dataReader["ECYSES_MaxMarks"].ToString() == "" ? "0" : dataReader["ECYSES_MaxMarks"].ToString()),
                                    ECYSES_MinMarks = Convert.ToDecimal(dataReader["ECYSES_MinMarks"].ToString() == null || dataReader["ECYSES_MinMarks"].ToString() == "" ? "0" : dataReader["ECYSES_MinMarks"].ToString()),
                                    EMGR_Id = Convert.ToInt32(dataReader["EMGR_Id"].ToString()),
                                    classheld = Convert.ToDecimal(dataReader["ASA_ClassHeld"].ToString() == null || dataReader["ASA_ClassHeld"].ToString() == "" ? "0" : dataReader["ASA_ClassHeld"].ToString()),
                                    classatt = Convert.ToDecimal(dataReader["ASA_Class_Attended"].ToString() == null || dataReader["ASA_Class_Attended"].ToString() == "" ? "0" : dataReader["ASA_Class_Attended"].ToString()),
                                    graderemark = (dataReader["EMGD_Remarks"].ToString() == null || dataReader["EMGD_Remarks"].ToString() == "" ? "0" : dataReader["EMGD_Remarks"].ToString()),

                                    ECSTMP_TotalObtMarks = Convert.ToDecimal(dataReader["ECSTMP_TotalObtMarks"].ToString() == null || dataReader["ECSTMP_TotalObtMarks"].ToString() == "" ? "0" : dataReader["ECSTMP_TotalObtMarks"].ToString()),
                                    ECSTMP_Percentage = Convert.ToDecimal(dataReader["ECSTMP_Percentage"].ToString() == null || dataReader["ECSTMP_Percentage"].ToString() == "" ? "0" : dataReader["ECSTMP_Percentage"].ToString()),
                                    ECSTMP_TotalGrade = (dataReader["ECSTMP_TotalGrade"].ToString() == null || dataReader["ECSTMP_TotalGrade"].ToString() == "" ? "" : dataReader["ECSTMP_TotalGrade"].ToString()),
                                    ECSTMP_SemRank = Convert.ToInt16(dataReader["ECSTMP_SemRank"].ToString() == null || dataReader["ECSTMP_SemRank"].ToString() == "" ? "" : dataReader["ECSTMP_SemRank"].ToString()),
                                    ECSTMP_SectionRank = Convert.ToInt16(dataReader["ECSTMP_SectionRank"].ToString() == null || dataReader["ECSTMP_SectionRank"].ToString() == "" ? "" : dataReader["ECSTMP_SectionRank"].ToString()),
                                    ECSTMP_TotalGradeRemark = (dataReader["ECSTMP_TotalGradeRemark"].ToString() == null || dataReader["ECSTMP_TotalGradeRemark"].ToString() == "" ? "" : dataReader["ECSTMP_TotalGradeRemark"].ToString()),
                                    ECSTMP_TotalMaxMarks = Convert.ToDecimal(dataReader["ECSTMP_TotalMaxMarks"].ToString() == null || dataReader["ECSTMP_TotalMaxMarks"].ToString() == "0" ? "" : dataReader["ECSTMP_TotalMaxMarks"].ToString()),
                                    ECYSES_SubjectOrder = Convert.ToInt16(dataReader["ECYSES_SubjectOrder"].ToString() == null || dataReader["ECYSES_SubjectOrder"].ToString() == "" ? "" : dataReader["ECYSES_SubjectOrder"].ToString())

                                });
                                var propertyInfo = typeof(ClgCumulativeReportDTO).GetProperty(order);
                                result1 = result1.OrderBy(x => propertyInfo.GetValue(x, null)).ThenBy(x => x.ECYSES_SubjectOrder).ToList();
                                data.savenonsubjlist = result1.Distinct().ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }

                List<long> nonsubs = new List<long>();
                if (data.savenonsubjlist != null && data.savenonsubjlist.Count > 0)
                {
                    data.nonsubjlist = data.savenonsubjlist.Distinct<ClgCumulativeReportDTO>(new CumulativeEqualityComparer()).ToArray();
                }

                var ECYS_Id = _CRContext.Exm_Col_Yearly_SchemeDMO.Single(t => t.MI_Id == data.MI_Id && t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id && t.ACSS_Id == data.ACSS_Id && t.ACST_Id == data.ACST_Id && t.ECYS_ActiveFlag == true && t.AMSE_Id == data.AMSE_ID).ECYS_Id;

                var ECYSE_Id = _CRContext.Exm_Col_Yearly_Scheme_ExamsDMO.Where(t => t.ECYS_Id == ECYS_Id && t.ECYSE_ActiveFlg == true && t.EME_Id == data.EME_Id).Select(t => t.ECYSE_Id).Distinct().ToList();

                var Exam_Subwise_Details = _CRContext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO.Where(t => ECYSE_Id.Contains(t.ECYSE_Id) && t.ECYSES_ActiveFlg == true).Distinct().OrderBy(t => t.ECYSE_Id).ThenBy(t => t.ECYSES_SubjectOrder).ToList();
                data.examsubjectwise_details = Exam_Subwise_Details.ToArray();

                data.configuration = _CRContext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
                //   data.savelisttot = _CRContext.ExmStudentMarksProcessDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id
                //&& t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id).Distinct().ToArray();
                //GetStudentWiseSubjectMakrs
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_Exam_GetStudentWiseSubject_Total";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = data.AMB_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = data.AMSE_ID });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = data.ACMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACST_Id", SqlDbType.VarChar) { Value = data.ACST_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACSS_Id", SqlDbType.VarChar) { Value = data.ACSS_Id });


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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.GetStudentWiseSubjectMakrs = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //schmetypelist
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_Exam_GetStudentWiseSubject_Analysis";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = data.AMB_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = data.AMSE_ID });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = data.ACMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACST_Id", SqlDbType.VarChar) { Value = data.ACST_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACSS_Id", SqlDbType.VarChar) { Value = data.ACSS_Id });


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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.schmetypelist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Cumulative Report Completed Procedure Final catch : ");
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }

            return data;
        }
        public ClgCumulativeReportDTO GetCumulativeReportFormat2(ClgCumulativeReportDTO data)
        {
            try
            {
                string amcst_ids = "0";

                if (data.Studentlist_temp != null && data.Studentlist_temp.Length > 0)
                {
                    foreach (var c in data.Studentlist_temp)
                    {
                        amcst_ids = amcst_ids + "," + c.AMCST_Id;
                    }
                }

                //Student List
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_Exam_Get_CumulativeReport_Format2";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = data.AMB_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = data.AMSE_ID });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = data.ACMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACST_Id", SqlDbType.VarChar) { Value = data.ACST_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACSS_Id", SqlDbType.VarChar) { Value = data.ACSS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar) { Value = amcst_ids });

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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.GetStudentList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //Subject List
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_Exam_Get_CumulativeReport_Format2";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = data.AMB_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = data.AMSE_ID });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = data.ACMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACST_Id", SqlDbType.VarChar) { Value = data.ACST_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACSS_Id", SqlDbType.VarChar) { Value = data.ACSS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar) { Value = amcst_ids });

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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.GetSubjectList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //Student Wise Subject Marks List
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_Exam_Get_CumulativeReport_Format2";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = data.AMB_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = data.AMSE_ID });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = data.ACMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACST_Id", SqlDbType.VarChar) { Value = data.ACST_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACSS_Id", SqlDbType.VarChar) { Value = data.ACSS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "3" });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar) { Value = amcst_ids });

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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.GetStudentWiseSubjectMakrs = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //Student Wise Total Marks List
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_Exam_Get_CumulativeReport_Format2";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = data.AMB_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = data.AMSE_ID });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = data.ACMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACST_Id", SqlDbType.VarChar) { Value = data.ACST_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACSS_Id", SqlDbType.VarChar) { Value = data.ACSS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "4" });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar) { Value = amcst_ids });

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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.GetStudentWiseMakrs = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgCumulativeReportDTO GetProgresscardReport(ClgCumulativeReportDTO data)
        {
            try
            {
                string amcst_ids = "0";

                if (data.Studentlist_temp != null && data.Studentlist_temp.Length > 0)
                {
                    foreach (var c in data.Studentlist_temp)
                    {
                        amcst_ids = amcst_ids + "," + c.AMCST_Id;
                    }
                }
                var from_date = ""; var to_date = "";
                //Student List


                if (data.graderemark == "Shridevi")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ExamWise_Attendance_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = data.AMB_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = data.AMSE_ID });
                        cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = data.ACMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                        cmd.Parameters.Add(new SqlParameter("@ACSS_Id", SqlDbType.VarChar) { Value = data.ACSS_Id });
                        cmd.Parameters.Add(new SqlParameter("@ACST_Id", SqlDbType.VarChar) { Value = data.ACST_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar) { Value = amcst_ids });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "1" });

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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }

                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.GetProgressCardReportList = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ExamWise_Attendance_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = data.AMB_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = data.AMSE_ID });
                        cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = data.ACMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                        cmd.Parameters.Add(new SqlParameter("@ACSS_Id", SqlDbType.VarChar) { Value = data.ACSS_Id });
                        cmd.Parameters.Add(new SqlParameter("@ACST_Id", SqlDbType.VarChar) { Value = data.ACST_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar) { Value = amcst_ids });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "2" });

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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }

                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.exmstdlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ExamWise_Attendance_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = data.AMB_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = data.AMSE_ID });
                        cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = data.ACMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                        cmd.Parameters.Add(new SqlParameter("@ACSS_Id", SqlDbType.VarChar) { Value = data.ACSS_Id });
                        cmd.Parameters.Add(new SqlParameter("@ACST_Id", SqlDbType.VarChar) { Value = data.ACST_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar) { Value = amcst_ids });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "3" });

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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }

                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.subjectshemalist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ExamWise_Attendance_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = data.AMB_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = data.AMSE_ID });
                        cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = data.ACMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                        cmd.Parameters.Add(new SqlParameter("@ACSS_Id", SqlDbType.VarChar) { Value = data.ACSS_Id });
                        cmd.Parameters.Add(new SqlParameter("@ACST_Id", SqlDbType.VarChar) { Value = data.ACST_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar) { Value = amcst_ids });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "4" });

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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }

                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.nonsubjlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CLG_Exam_Progresscard_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = data.AMB_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = data.AMSE_ID });
                        cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = data.ACMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                        cmd.Parameters.Add(new SqlParameter("@ACST_Id", SqlDbType.VarChar) { Value = data.ACST_Id });
                        cmd.Parameters.Add(new SqlParameter("@ACSS_Id", SqlDbType.VarChar) { Value = data.ACSS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar) { Value = amcst_ids });

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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }

                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.GetProgressCardReportList = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "StudentAttendance_W";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });

                        cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date) { Value = from_date });
                        cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date) { Value = to_date });
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
                                        dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
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

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "StudentAttendance_P";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });

                        cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date) { Value = from_date });
                        cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date) { Value = to_date });
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
                                        dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgCumulativeReportDTO GetJNUProgressCardReport1(ClgCumulativeReportDTO data)
        {
            try
            {
                string amcst_ids = "0";

                if (data.Studentlist_temp != null && data.Studentlist_temp.Length > 0)
                {
                    foreach (var c in data.Studentlist_temp)
                    {
                        amcst_ids = amcst_ids + "," + c.AMCST_Id;
                    }
                }

                //Student List
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Exam_Get_Student_Other_Details_JNU";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = data.AMB_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = data.AMSE_ID });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = data.ACMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACST_Id", SqlDbType.VarChar) { Value = data.ACST_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACSS_Id", SqlDbType.VarChar) { Value = data.ACSS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar) { Value = amcst_ids });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });

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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.GetStudentList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        class CumulativeEqualityComparer : IEqualityComparer<ClgCumulativeReportDTO>
        {
            public bool Equals(ClgCumulativeReportDTO b1, ClgCumulativeReportDTO b2)
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

            public int GetHashCode(ClgCumulativeReportDTO bx)
            {
                int hCode = Convert.ToInt32(bx.ISMS_Id);
                return hCode.GetHashCode();
            }
        }
    }
}
