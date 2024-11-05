using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Exam;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class VikasaSchoolExamWiseCumulativeReportImpl : Interfaces.VikasaSchoolExamWiseCumulativeReportInterface
    {

        private static ConcurrentDictionary<string, VikasaSubjectwiseCumulativeReportDTO> _login =
         new ConcurrentDictionary<string, VikasaSubjectwiseCumulativeReportDTO>();

        private readonly ExamContext _ReportContext;
        public StudentAttendanceReportContext _db;
        ILogger<VikasaSchoolExamWiseCumulativeReportImpl> _acdimpl;
        public VikasaSchoolExamWiseCumulativeReportImpl(ExamContext cpContext, StudentAttendanceReportContext db)
        {
            _ReportContext = cpContext;
            _db = db;
        }
        public VikasaSubjectwiseCumulativeReportDTO Getdetails(VikasaSubjectwiseCumulativeReportDTO data)//int IVRMM_Id
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _ReportContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public VikasaSubjectwiseCumulativeReportDTO showdetails(VikasaSubjectwiseCumulativeReportDTO data)
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

                data.BasicListYear = (from a in _ReportContext.AcademicYear
                                      where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                      select new VikasaSubjectwiseCumulativeReportDTO
                                      {
                                          yearname = a.ASMAY_Year
                                      }).Distinct().ToArray();


                data.BasiListclass = (from a in _db.admissionyearstudent
                                      from b in _db.admissionClass
                                      where (a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && b.MI_Id == data.MI_Id
                                      && b.ASMCL_Id == data.ASMCL_Id)
                                      select new VikasaSubjectwiseCumulativeReportDTO
                                      {
                                          ClassName = b.ASMCL_ClassName
                                      }).Distinct().ToArray();

                data.BasiListsectiont = (from a in _db.admissionyearstudent
                                         from b in _db.masterSection

                                         where (a.ASMAY_Id == data.ASMAY_Id && a.ASMS_Id == b.ASMS_Id && b.ASMC_ActiveFlag == 1 && b.MI_Id == data.MI_Id
                                         && a.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id)
                                         select new VikasaSubjectwiseCumulativeReportDTO
                                         {
                                             sectionname = b.ASMC_SectionName
                                         }).Distinct().ToArray();


                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = _ReportContext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && t.EME_Id == data.EME_Id).ToList();
                data.BasiListsubject = esmp.ToArray();

                //for student details
                data.studentList = (from a in _db.admissionyearstudent
                                    from b in _db.admissionStduent
                                    where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && ids.Contains(a.AMAY_ActiveFlag) && a.ASMS_Id == data.ASMS_Id
                                    && b.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && ids.Contains(b.AMST_ActiveFlag)
                                    && sol.Contains(b.AMST_SOL))
                                    select b).Distinct().ToArray();

                //for student Exam Group head

                data.ExamGroupname = (from a in _ReportContext.Exm_Category_ClassDMO
                                      from b in _ReportContext.Exm_Yearly_CategoryDMO
                                      from c in _ReportContext.Exm_Yearly_Category_ExamsDMO
                                      from d in _ReportContext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                      from e in _ReportContext.IVRM_School_Master_SubjectsDMO
                                      where (a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.EYC_Id == b.EYC_Id && c.EME_Id == data.EME_Id && d.EYCE_Id == c.EYCE_Id && d.EYCES_ActiveFlg == true && e.MI_Id == data.MI_Id && e.ISMS_Id == d.ISMS_Id)
                                      select new ProgressCardReportDTO
                                      {
                                          ISMS_Id = d.ISMS_Id,
                                          ISMS_SubjectName = e.ISMS_SubjectName,
                                          ISMS_SubjectCode = e.ISMS_SubjectCode
                                      }).Distinct().ToArray();

                //for student Exam Group Marks

                data.examgroupmarks = (from a in _ReportContext.ExmStudentMarksProcessSubjectwiseDMO
                                       from b in _ReportContext.AdmissionClass
                                       from c in _ReportContext.exammasterDMO

                                       where (a.ASMCL_Id == b.ASMCL_Id && a.ASMCL_Id == data.ASMCL_Id && a.EME_Id == c.EME_Id && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.EME_Id == data.EME_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                       group new { a, b, c }
                                      by new { a.AMST_Id, a.EME_Id } into g
                                       select new ProgressCardReportDTO
                                       {
                                           ESTMPS_ObtainedMarks = g.FirstOrDefault().a.ESTMPS_ObtainedMarks,
                                           ESTMPS_SectionAverage = g.FirstOrDefault().a.ESTMPS_SectionAverage,
                                           ESTMPS_SectionHighest = g.FirstOrDefault().a.ESTMPS_SectionHighest,
                                           ESTMPS_ObtainedGrade = g.FirstOrDefault().a.ESTMPS_ObtainedGrade,
                                           ESTMPS_PassFailFlg = g.FirstOrDefault().a.ESTMPS_PassFailFlg,
                                           AMST_Id = g.FirstOrDefault().a.AMST_Id,
                                           ISMS_Id = g.FirstOrDefault().a.ISMS_Id,
                                           ESTMPS_MaxMarks = g.Sum(d => d.a.ESTMPS_ObtainedMarks),
                                       }).Distinct().ToArray();



                data.classteacher = (from a in _ReportContext.ClassTeacherMappingDMO
                                     from b in _ReportContext.HR_Master_Employee_DMO
                                     where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.IMCT_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id
                                     && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                     select new VikasaSubjectwiseCumulativeReportDTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         empname = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null ? " " : "  " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null ? " " : "  " + b.HRME_EmployeeLastName)).Trim(),
                                     }).ToArray();
            }

            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }

            return data;
        }
        public VikasaSubjectwiseCumulativeReportDTO get_class(VikasaSubjectwiseCumulativeReportDTO dto)
        {
            try
            {
                dto.classlist = (from c in _db.admissionClass
                                 from d in _db.Exm_Category_ClassDMO
                                 where (d.ASMCL_Id == c.ASMCL_Id && d.ECAC_ActiveFlag == true && d.MI_Id == dto.MI_Id && d.ASMAY_Id == dto.ASMAY_Id)
                                 select c).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public VikasaSubjectwiseCumulativeReportDTO get_section(VikasaSubjectwiseCumulativeReportDTO dto)
        {
            try
            {
                dto.sectionList = (from b in _db.admissionClass
                                   from c in _db.masterSection
                                   from d in _db.Exm_Category_ClassDMO
                                   where (b.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == d.ASMS_Id && d.ECAC_ActiveFlag == true && d.ASMCL_Id == dto.ASMCL_Id
                                   && c.MI_Id == dto.MI_Id && d.MI_Id == dto.MI_Id && d.ASMAY_Id == dto.ASMAY_Id)
                                   select c).Distinct().OrderBy(t => t.ASMC_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public VikasaSubjectwiseCumulativeReportDTO get_subject(VikasaSubjectwiseCumulativeReportDTO dto)
        {
            try
            {
                dto.gradeList = _ReportContext.Exm_Master_GradeDMO.Where(t => t.MI_Id == dto.MI_Id && t.EMGR_ActiveFlag == true).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public VikasaSubjectwiseCumulativeReportDTO get_Exam(VikasaSubjectwiseCumulativeReportDTO dto)
        {
            try
            {
                var EQuery = _ReportContext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == dto.MI_Id && t.ASMAY_Id == dto.ASMAY_Id && t.ASMCL_Id == dto.ASMCL_Id && t.ASMS_Id == dto.ASMS_Id).Select(d => d.EME_Id).ToList();

                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = _ReportContext.exammasterDMO.Where(t => t.MI_Id == dto.MI_Id && t.EME_ActiveFlag == true && EQuery.Contains(t.EME_Id)).ToList();
                dto.ExamList = esmp.OrderBy(a => a.EME_ExamOrder).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }

        public async Task<VikasaSubjectwiseCumulativeReportDTO> savedetails(VikasaSubjectwiseCumulativeReportDTO data)
        {
            try
            {
                data.instname = _ReportContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                List<VikasaSubjectwiseCumulativeReportDTO> result = new List<VikasaSubjectwiseCumulativeReportDTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_cumulative_Vikasa_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMCL_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMS_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.EME_Id)
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                result.Add(new VikasaSubjectwiseCumulativeReportDTO
                                {
                                    MI_name = (dataReader["MI_name"].ToString().Trim() == null || dataReader["MI_name"].ToString().Trim() == "" ? "" : dataReader["MI_name"].ToString()),

                                    ESTMPS_ObtainedMarks = Convert.ToDecimal(dataReader["ESTMPS_ObtainedMarks"].ToString() == null || dataReader["ESTMPS_ObtainedMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_ObtainedMarks"].ToString()),
                                    ESTMPS_ObtainedGrade = (dataReader["ESTMPS_ObtainedGrade"].ToString() == null || dataReader["ESTMPS_ObtainedGrade"].ToString() == "" ? "" : dataReader["ESTMPS_ObtainedGrade"].ToString()),
                                    ESTMPS_PassFailFlg = (dataReader["ESTMPS_PassFailFlg"].ToString() == null || dataReader["ESTMPS_PassFailFlg"].ToString() == "" ? "" : dataReader["ESTMPS_PassFailFlg"].ToString()),
                                    EME_ExamName = (dataReader["EME_ExamName"].ToString() == null || dataReader["EME_ExamName"].ToString() == "" ? "" : dataReader["EME_ExamName"].ToString()),
                                    ASMCL_ClassName = (dataReader["ASMCL_ClassName"].ToString() == null || dataReader["ASMCL_ClassName"].ToString() == "" ? "" : dataReader["ASMCL_ClassName"].ToString()),
                                    ASMC_SectionName = (dataReader["ASMC_SectionName"].ToString() == null || dataReader["ASMC_SectionName"].ToString() == "" ? "" : dataReader["ASMC_SectionName"].ToString()),
                                    AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                    AMST_FirstName = ((dataReader["AMST_FirstName"].ToString() == null ? " " : dataReader["AMST_FirstName"].ToString()) + " " + (dataReader["AMST_MiddleName"].ToString() == null ? " " : dataReader["AMST_MiddleName"].ToString()) + " " + (dataReader["AMST_LastName"].ToString() == null ? " " : dataReader["AMST_LastName"].ToString())).Trim(),
                                    AMST_DOB = Convert.ToDateTime(dataReader["AMST_DOB"].ToString() == null || dataReader["AMST_DOB"].ToString() == "" ? "" : dataReader["AMST_DOB"].ToString()),
                                    AMAY_RollNo = Convert.ToInt32(dataReader["AMAY_RollNo"].ToString() == null || dataReader["AMAY_RollNo"].ToString() == "" ? "" : dataReader["AMAY_RollNo"].ToString()),
                                    AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString() == null || dataReader["AMST_AdmNo"].ToString() == "" ? "" : dataReader["AMST_AdmNo"].ToString()),
                                    ISMS_Id = Convert.ToInt32(dataReader["ISMS_Id"].ToString()),
                                    ISMS_SubjectName = (dataReader["ISMS_SubjectName"].ToString() == null || dataReader["ISMS_SubjectName"].ToString() == "" ? "" : dataReader["ISMS_SubjectName"].ToString()),
                                    ESTMPS_MaxMarks = Convert.ToDecimal(dataReader["ESTMPS_MaxMarks"].ToString() == null || dataReader["ESTMPS_MaxMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_MaxMarks"].ToString()),
                                    ESTMPS_ClassAverage = Convert.ToDecimal(dataReader["ESTMPS_ClassAverage"].ToString() == null || dataReader["ESTMPS_ClassAverage"].ToString() == "" ? "0" : dataReader["ESTMPS_ClassAverage"].ToString()),
                                    ESTMPS_SectionAverage = Convert.ToDecimal(dataReader["ESTMPS_SectionAverage"].ToString() == null || dataReader["ESTMPS_SectionAverage"].ToString() == "" ? "0" : dataReader["ESTMPS_SectionAverage"].ToString()),
                                    ESTMPS_ClassHighest = Convert.ToDecimal(dataReader["ESTMPS_ClassHighest"].ToString() == null || dataReader["ESTMPS_ClassHighest"].ToString() == "" ? "0" : dataReader["ESTMPS_ClassHighest"].ToString()),
                                    ESTMPS_SectionHighest = Convert.ToDecimal(dataReader["ESTMPS_SectionHighest"].ToString() == null || dataReader["ESTMPS_SectionHighest"].ToString() == "" ? "0" : dataReader["ESTMPS_SectionHighest"].ToString()),
                                    ISMS_SubjectCode = (dataReader["ISMS_SubjectCode"].ToString() == null || dataReader["ISMS_SubjectCode"].ToString() == "" ? "" : dataReader["ISMS_SubjectCode"].ToString()),
                                    EYCES_AplResultFlg = Convert.ToBoolean(dataReader["EYCES_AplResultFlg"].ToString()),
                                    EYCES_MaxMarks = Convert.ToDecimal(dataReader["EYCES_MaxMarks"].ToString() == null || dataReader["EYCES_MaxMarks"].ToString() == "" ? "0" : dataReader["EYCES_MaxMarks"].ToString()),
                                    EYCES_MinMarks = Convert.ToDecimal(dataReader["EYCES_MinMarks"].ToString() == null || dataReader["EYCES_MinMarks"].ToString() == "" ? "0" : dataReader["EYCES_MinMarks"].ToString()),
                                    EMGR_Id = Convert.ToInt32(dataReader["EMGR_Id"].ToString()),
                                    classheld = Convert.ToDecimal(dataReader["ASA_ClassHeld"].ToString() == null || dataReader["ASA_ClassHeld"].ToString() == "" ? "0" : dataReader["ASA_ClassHeld"].ToString()),
                                    classatt = Convert.ToDecimal(dataReader["ASA_Class_Attended"].ToString() == null || dataReader["ASA_Class_Attended"].ToString() == "" ? "0" : dataReader["ASA_Class_Attended"].ToString()),
                                    graderemark = (dataReader["EMGD_Remarks"].ToString() == null || dataReader["EMGD_Remarks"].ToString() == "" ? "0" : dataReader["EMGD_Remarks"].ToString()),

                                    ESTMP_TotalObtMarks = Convert.ToDecimal(dataReader["ESTMP_TotalObtMarks"].ToString() == null || dataReader["ESTMP_TotalObtMarks"].ToString() == "" ? "0" : dataReader["ESTMP_TotalObtMarks"].ToString()),
                                    ESTMP_Percentage = Convert.ToDecimal(dataReader["ESTMP_Percentage"].ToString() == null || dataReader["ESTMP_Percentage"].ToString() == "" ? "0" : dataReader["ESTMP_Percentage"].ToString()),
                                    ESTMP_TotalGrade = (dataReader["ESTMP_TotalGrade"].ToString() == null || dataReader["ESTMP_TotalGrade"].ToString() == "" ? "" : dataReader["ESTMP_TotalGrade"].ToString()),
                                    ESTMP_ClassRank = Convert.ToInt16(dataReader["ESTMP_ClassRank"].ToString() == null || dataReader["ESTMP_ClassRank"].ToString() == "" ? "" : dataReader["ESTMP_ClassRank"].ToString()),
                                    ESTMP_SectionRank = Convert.ToInt16(dataReader["ESTMP_SectionRank"].ToString() == null || dataReader["ESTMP_SectionRank"].ToString() == "" ? "" : dataReader["ESTMP_SectionRank"].ToString()),
                                    ESTMP_TotalGradeRemark = (dataReader["ESTMP_TotalGradeRemark"].ToString() == null || dataReader["ESTMP_TotalGradeRemark"].ToString() == "" ? "" : dataReader["ESTMP_TotalGradeRemark"].ToString()),
                                    ESTMP_TotalMaxMarks = Convert.ToDecimal(dataReader["ESTMP_TotalMaxMarks"].ToString() == null || dataReader["ESTMP_TotalMaxMarks"].ToString() == "0" ? "" : dataReader["ESTMP_TotalMaxMarks"].ToString()),
                                    EYCES_SubjectOrder = Convert.ToInt16(dataReader["EYCES_SubjectOrder"].ToString() == null || dataReader["EYCES_SubjectOrder"].ToString() == "" ? "" : dataReader["EYCES_SubjectOrder"].ToString()),
                                    ESTMP_Result = (dataReader["ESTMP_Result"].ToString() == null || dataReader["ESTMP_Result"].ToString() == "" ? "" : dataReader["ESTMP_Result"].ToString()),

                                });
                                data.savelist = result.Distinct().OrderBy(t => t.EYCES_SubjectOrder).ToList();
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
                    data.subjlist = data.savelist.Distinct<VikasaSubjectwiseCumulativeReportDTO>(new CumulativeEqualityComparer1()).ToArray();
                }

                List<VikasaSubjectwiseCumulativeReportDTO> result1 = new List<VikasaSubjectwiseCumulativeReportDTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_cumulative_Vikasa_Report_Elective";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMCL_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMS_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.EME_Id)
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                result1.Add(new VikasaSubjectwiseCumulativeReportDTO
                                {
                                    MI_name = (dataReader["MI_name"].ToString().Trim() == null || dataReader["MI_name"].ToString().Trim() == "" ? "" : dataReader["MI_name"].ToString()),

                                    ESTMPS_ObtainedMarks = Convert.ToDecimal(dataReader["ESTMPS_ObtainedMarks"].ToString() == null || dataReader["ESTMPS_ObtainedMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_ObtainedMarks"].ToString()),
                                    ESTMPS_ObtainedGrade = (dataReader["ESTMPS_ObtainedGrade"].ToString() == null || dataReader["ESTMPS_ObtainedGrade"].ToString() == "" ? "" : dataReader["ESTMPS_ObtainedGrade"].ToString()),
                                    ESTMPS_PassFailFlg = (dataReader["ESTMPS_PassFailFlg"].ToString() == null || dataReader["ESTMPS_PassFailFlg"].ToString() == "" ? "" : dataReader["ESTMPS_PassFailFlg"].ToString()),
                                    EME_ExamName = (dataReader["EME_ExamName"].ToString() == null || dataReader["EME_ExamName"].ToString() == "" ? "" : dataReader["EME_ExamName"].ToString()),
                                    ASMCL_ClassName = (dataReader["ASMCL_ClassName"].ToString() == null || dataReader["ASMCL_ClassName"].ToString() == "" ? "" : dataReader["ASMCL_ClassName"].ToString()),
                                    ASMC_SectionName = (dataReader["ASMC_SectionName"].ToString() == null || dataReader["ASMC_SectionName"].ToString() == "" ? "" : dataReader["ASMC_SectionName"].ToString()),
                                    AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                    AMST_FirstName = ((dataReader["AMST_FirstName"].ToString() == null ? " " : dataReader["AMST_FirstName"].ToString()) + " " + (dataReader["AMST_MiddleName"].ToString() == null ? " " : dataReader["AMST_MiddleName"].ToString()) + " " + (dataReader["AMST_LastName"].ToString() == null ? " " : dataReader["AMST_LastName"].ToString())).Trim(),
                                    AMST_DOB = Convert.ToDateTime(dataReader["AMST_DOB"].ToString() == null || dataReader["AMST_DOB"].ToString() == "" ? "" : dataReader["AMST_DOB"].ToString()),
                                    AMAY_RollNo = Convert.ToInt32(dataReader["AMAY_RollNo"].ToString() == null || dataReader["AMAY_RollNo"].ToString() == "" ? "" : dataReader["AMAY_RollNo"].ToString()),
                                    AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString() == null || dataReader["AMST_AdmNo"].ToString() == "" ? "" : dataReader["AMST_AdmNo"].ToString()),
                                    ISMS_Id = Convert.ToInt32(dataReader["ISMS_Id"].ToString()),
                                    ISMS_SubjectName = (dataReader["ISMS_SubjectName"].ToString() == null || dataReader["ISMS_SubjectName"].ToString() == "" ? "" : dataReader["ISMS_SubjectName"].ToString()),
                                    ESTMPS_MaxMarks = Convert.ToDecimal(dataReader["ESTMPS_MaxMarks"].ToString() == null || dataReader["ESTMPS_MaxMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_MaxMarks"].ToString()),
                                    ESTMPS_ClassAverage = Convert.ToDecimal(dataReader["ESTMPS_ClassAverage"].ToString() == null || dataReader["ESTMPS_ClassAverage"].ToString() == "" ? "0" : dataReader["ESTMPS_ClassAverage"].ToString()),
                                    ESTMPS_SectionAverage = Convert.ToDecimal(dataReader["ESTMPS_SectionAverage"].ToString() == null || dataReader["ESTMPS_SectionAverage"].ToString() == "" ? "0" : dataReader["ESTMPS_SectionAverage"].ToString()),
                                    ESTMPS_ClassHighest = Convert.ToDecimal(dataReader["ESTMPS_ClassHighest"].ToString() == null || dataReader["ESTMPS_ClassHighest"].ToString() == "" ? "0" : dataReader["ESTMPS_ClassHighest"].ToString()),
                                    ESTMPS_SectionHighest = Convert.ToDecimal(dataReader["ESTMPS_SectionHighest"].ToString() == null || dataReader["ESTMPS_SectionHighest"].ToString() == "" ? "0" : dataReader["ESTMPS_SectionHighest"].ToString()),
                                    ISMS_SubjectCode = (dataReader["ISMS_SubjectCode"].ToString() == null || dataReader["ISMS_SubjectCode"].ToString() == "" ? "" : dataReader["ISMS_SubjectCode"].ToString()),
                                    EYCES_AplResultFlg = Convert.ToBoolean(dataReader["EYCES_AplResultFlg"].ToString()),
                                    EYCES_MaxMarks = Convert.ToDecimal(dataReader["EYCES_MaxMarks"].ToString() == null || dataReader["EYCES_MaxMarks"].ToString() == "" ? "0" : dataReader["EYCES_MaxMarks"].ToString()),
                                    EYCES_MinMarks = Convert.ToDecimal(dataReader["EYCES_MinMarks"].ToString() == null || dataReader["EYCES_MinMarks"].ToString() == "" ? "0" : dataReader["EYCES_MinMarks"].ToString()),
                                    EMGR_Id = Convert.ToInt32(dataReader["EMGR_Id"].ToString()),
                                    classheld = Convert.ToDecimal(dataReader["ASA_ClassHeld"].ToString() == null || dataReader["ASA_ClassHeld"].ToString() == "" ? "0" : dataReader["ASA_ClassHeld"].ToString()),
                                    classatt = Convert.ToDecimal(dataReader["ASA_Class_Attended"].ToString() == null || dataReader["ASA_Class_Attended"].ToString() == "" ? "0" : dataReader["ASA_Class_Attended"].ToString()),
                                    graderemark = (dataReader["EMGD_Remarks"].ToString() == null || dataReader["EMGD_Remarks"].ToString() == "" ? "0" : dataReader["EMGD_Remarks"].ToString()),

                                    ESTMP_TotalObtMarks = Convert.ToDecimal(dataReader["ESTMP_TotalObtMarks"].ToString() == null || dataReader["ESTMP_TotalObtMarks"].ToString() == "" ? "0" : dataReader["ESTMP_TotalObtMarks"].ToString()),
                                    ESTMP_Percentage = Convert.ToDecimal(dataReader["ESTMP_Percentage"].ToString() == null || dataReader["ESTMP_Percentage"].ToString() == "" ? "0" : dataReader["ESTMP_Percentage"].ToString()),
                                    ESTMP_TotalGrade = (dataReader["ESTMP_TotalGrade"].ToString() == null || dataReader["ESTMP_TotalGrade"].ToString() == "" ? "" : dataReader["ESTMP_TotalGrade"].ToString()),
                                    ESTMP_ClassRank = Convert.ToInt16(dataReader["ESTMP_ClassRank"].ToString() == null || dataReader["ESTMP_ClassRank"].ToString() == "" ? "" : dataReader["ESTMP_ClassRank"].ToString()),
                                    ESTMP_SectionRank = Convert.ToInt16(dataReader["ESTMP_SectionRank"].ToString() == null || dataReader["ESTMP_SectionRank"].ToString() == "" ? "" : dataReader["ESTMP_SectionRank"].ToString()),
                                    ESTMP_TotalGradeRemark = (dataReader["ESTMP_TotalGradeRemark"].ToString() == null || dataReader["ESTMP_TotalGradeRemark"].ToString() == "" ? "" : dataReader["ESTMP_TotalGradeRemark"].ToString()),
                                    ESTMP_TotalMaxMarks = Convert.ToDecimal(dataReader["ESTMP_TotalMaxMarks"].ToString() == null || dataReader["ESTMP_TotalMaxMarks"].ToString() == "0" ? "" : dataReader["ESTMP_TotalMaxMarks"].ToString()),
                                    EYCES_SubjectOrder = Convert.ToInt16(dataReader["EYCES_SubjectOrder"].ToString() == null || dataReader["EYCES_SubjectOrder"].ToString() == "" ? "" : dataReader["EYCES_SubjectOrder"].ToString())

                                });
                                data.savenonsubjlist = result1.Distinct().OrderBy(t => t.EYCES_SubjectOrder).ToList();
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
                    data.nonsubjlist = data.savenonsubjlist.Distinct<VikasaSubjectwiseCumulativeReportDTO>(new CumulativeEqualityComparer1()).ToArray();
                }

                var EMCA_Id = _ReportContext.Exm_Category_ClassDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).EMCA_Id;
                var EYC_Id = _ReportContext.Exm_Yearly_CategoryDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EMCA_Id == EMCA_Id && t.EYC_ActiveFlg == true).EYC_Id;


                var EYCE_Id = _ReportContext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == EYC_Id && t.EYCE_ActiveFlg == true && t.EME_Id == data.EME_Id).Select(t => t.EYCE_Id).Distinct().ToList();

                var Exam_Subwise_Details = _ReportContext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => EYCE_Id.Contains(t.EYCE_Id) && t.EYCES_ActiveFlg == true).Distinct().OrderBy(t => t.EYCE_Id).ThenBy(t => t.EYCES_SubjectOrder).ToList();
                data.examsubjectwise_details = Exam_Subwise_Details.ToArray();


                data.classteacher = (from a in _ReportContext.ClassTeacherMappingDMO
                                     from b in _ReportContext.HR_Master_Employee_DMO
                                     where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.IMCT_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id
                                     && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                     select new VikasaSubjectwiseCumulativeReportDTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         empname = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null ? " " : "  " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null ? " " : "  " + b.HRME_EmployeeLastName)).Trim(),
                                     }).ToArray();

                data.sectionaverage = (from a in _ReportContext.ExmStudentMarksProcessSubjectwiseDMO
                                       from b in _ReportContext.IVRM_School_Master_SubjectsDMO
                                       where (a.ISMS_Id == b.ISMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.MI_Id == data.MI_Id)
                                       select new VikasaSubjectwiseCumulativeReportDTO
                                       {
                                           ISMS_Id = b.ISMS_Id,
                                           ISMS_SubjectName = b.ISMS_SubjectName,
                                           ESTMPS_SectionAverage = a.ESTMPS_SectionAverage,
                                           ISMS_Order = b.ISMS_OrderFlag,
                                       }).Distinct().OrderBy(a => a.ISMS_Order).ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
    }
    class CumulativeEqualityComparer1 : IEqualityComparer<VikasaSubjectwiseCumulativeReportDTO>
    {
        public bool Equals(VikasaSubjectwiseCumulativeReportDTO b1, VikasaSubjectwiseCumulativeReportDTO b2)
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

        public int GetHashCode(VikasaSubjectwiseCumulativeReportDTO bx)
        {
            int hCode = Convert.ToInt32(bx.ISMS_Id);
            return hCode.GetHashCode();
        }
    }
}
