using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Exam;
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
    public class MaldaProgressReportExamImpl : Interfaces.MaldaProgressReportExamInterface
    {
        private readonly ExamContext _PCReportContext;
        public StudentAttendanceReportContext _db;
        ILogger<MaldaProgressReportExamImpl> _acdimpl;
        public MaldaProgressReportExamImpl(ExamContext cpContext, StudentAttendanceReportContext db, ILogger<MaldaProgressReportExamImpl> _acdimp)
        {
            _PCReportContext = cpContext;
            _db = db;
            _acdimpl = _acdimp;
        }
        public async Task<MaldaProgressReportExam_DTO> Getdetails(MaldaProgressReportExam_DTO data)//int IVRMM_Id
        {
            MaldaProgressReportExam_DTO getdata = new MaldaProgressReportExam_DTO();
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = await _PCReportContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToListAsync();
                getdata.yearlist = list.ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = await _PCReportContext.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).OrderBy(t => t.ASMC_Order).ToListAsync();
                getdata.seclist = seclist.ToArray();

                List<AdmissionClass> admlist = new List<AdmissionClass>();
                admlist = await _PCReportContext.AdmissionClass.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).OrderBy(t => t.ASMCL_Order).ToListAsync();
                getdata.classlist = admlist.ToArray();

                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = await _PCReportContext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).OrderBy(t => t.EME_ExamOrder).ToListAsync();

                getdata.exmstdlist = esmp.ToArray();

                getdata.grade_list = _PCReportContext.Exm_Master_GradeDMO.Where(t => t.MI_Id == data.MI_Id && t.EMGR_ActiveFlag == true).ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;

        }
        public async Task<MaldaProgressReportExam_DTO> savedetails(MaldaProgressReportExam_DTO data)
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

                data.instname = _PCReportContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                data.clstchname = (from a in _PCReportContext.ClassTeacherMappingDMO
                                   from b in _PCReportContext.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new MaldaProgressReportExam_DTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                data.countStudent = (from a in _PCReportContext.School_Adm_Y_Student
                                     from b in _PCReportContext.Adm_M_Student
                                     where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id 
                                     && a.ASMS_Id == data.ASMS_Id && ids.Contains(a.AMAY_ActiveFlag) && ids.Contains(b.AMST_ActiveFlag) && sol.Contains(b.AMST_SOL))
                                     select b).Distinct().ToArray();


                List<MaldaProgressReportExam_DTO> result = new List<MaldaProgressReportExam_DTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "get_Malda_Exam_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt32(data.ASMCL_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt32(data.ASMS_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt32(data.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                        SqlDbType.BigInt)
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
                                result.Add(new MaldaProgressReportExam_DTO
                                {
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
                                    EYCES_MarksDisplayFlg = Convert.ToBoolean(dataReader["EYCES_MarksDisplayFlg"].ToString()),
                                    EYCES_GradeDisplayFlg = Convert.ToBoolean(dataReader["EYCES_GradeDisplayFlg"].ToString()),
                                    ESTMP_Result = (dataReader["ESTMP_Result"].ToString() == null || dataReader["ESTMP_Result"].ToString() == "" ? "" : dataReader["ESTMP_Result"].ToString())
                                });

                                data.savelist = result.OrderBy(t => t.EYCES_SubjectOrder).ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                //Grade Deatails 

                var EMCA_Ids = _PCReportContext.Exm_Category_ClassDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).EMCA_Id;

                var EYC_Ids = _PCReportContext.Exm_Yearly_CategoryDMO.Single(t => t.ASMAY_Id == data.ASMAY_Id && t.EMCA_Id == EMCA_Ids
                && t.EYC_ActiveFlg == true).EYC_Id;

                var EMGR_Ids = _PCReportContext.Exm_Yearly_Category_ExamsDMO.Single(t => t.EYC_Id == EYC_Ids && t.EME_Id == data.EME_Id && t.EYCE_ActiveFlg == true).EMGR_Id;

                data.grade_detailslist = _PCReportContext.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == EMGR_Ids && t.EMGD_ActiveFlag == true).Distinct().OrderByDescending(t => t.EMGD_From).ThenByDescending(a => a.EMGD_To).ToArray();

                data.getstudentwiseremarks = _PCReportContext.Exm_ProgressCard_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id 
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_ID == data.EME_Id && a.EMER_ActiveFlag == true ).ToArray();


            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }

            return data;
        }
        public MaldaProgressReportExam_DTO onchangeyear(MaldaProgressReportExam_DTO data)
        {
            try
            {
                data.classlist = (from a in _PCReportContext.AcademicYear
                                  from b in _PCReportContext.Exm_Category_ClassDMO
                                  from c in _PCReportContext.AdmissionClass
                                  where (a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && a.MI_Id == data.MI_Id && a.Is_Active == true && b.ASMAY_Id == data.ASMAY_Id && b.ECAC_ActiveFlag == true && b.MI_Id == data.MI_Id)
                                  select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public MaldaProgressReportExam_DTO onchangeclass(MaldaProgressReportExam_DTO data)
        {
            try
            {
                data.seclist = (from a in _PCReportContext.AcademicYear
                                from b in _PCReportContext.Exm_Category_ClassDMO
                                from c in _PCReportContext.AdmissionClass
                                from d in _PCReportContext.School_M_Section
                                where (a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id
                                && a.MI_Id == data.MI_Id && a.Is_Active == true && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ECAC_ActiveFlag == true && b.MI_Id == data.MI_Id)
                                select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public MaldaProgressReportExam_DTO onchangesection(MaldaProgressReportExam_DTO data)
        {
            try
            {
                data.exmstdlist = (from a in _PCReportContext.Exm_Master_CategoryDMO
                                   from b in _PCReportContext.Exm_Category_ClassDMO
                                   from c in _PCReportContext.Exm_Yearly_CategoryDMO
                                   from d in _PCReportContext.Exm_Yearly_Category_ExamsDMO
                                   from e in _PCReportContext.AcademicYear
                                   from f in _PCReportContext.AdmissionClass
                                   from g in _PCReportContext.School_M_Section
                                   from h in _PCReportContext.masterexam
                                   where (a.EMCA_Id == b.EMCA_Id && c.EMCA_Id == a.EMCA_Id && c.EYC_Id == d.EYC_Id && e.ASMAY_Id == c.ASMAY_Id
                                   && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == f.ASMCL_Id && b.ASMS_Id == g.ASMS_Id && d.EME_Id == h.EME_Id
                                   && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id
                                   && c.ASMAY_Id == data.ASMAY_Id && b.ECAC_ActiveFlag == true && c.EYC_ActiveFlg == true && d.EYCE_ActiveFlg == true
                                   && h.EME_ActiveFlag == true)
                                   select h).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        //**** promotion report ***///
        public MaldaProgressReportExam_DTO getreportpromotion(MaldaProgressReportExam_DTO data)
        {
            try
            {
                data.instname = _PCReportContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                data.clstchname = (from a in _PCReportContext.ClassTeacherMappingDMO
                                   from b in _PCReportContext.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                   && a.IMCT_ActiveFlag == true && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false)
                                   select new BaldwinAllReportDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                // STUDENT LIST
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Promotion_Final_Report_Malda";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    {
                        Value = 1
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
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                // SUBJECT LIST
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Promotion_Final_Report_Malda";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    {
                        Value = 3
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
                        data.getsubjectlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                // EXAM LIST
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Promotion_Final_Report_Malda";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    {
                        Value = 2
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
                        data.getexamlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                // SAVED LIST
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Promotion_Final_Report_Malda";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    {
                        Value = 4
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
                        data.getsavedlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                data.remarks = _PCReportContext.ExamPromotionRemarksDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EPRD_Promotionflag == "PE").ToArray();


                var EMCA_Ids = _PCReportContext.Exm_Category_ClassDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).EMCA_Id;

                var EYC_Ids = _PCReportContext.Exm_Yearly_CategoryDMO.Single(t => t.ASMAY_Id == data.ASMAY_Id && t.EMCA_Id == EMCA_Ids
                && t.EYC_ActiveFlg == true).EYC_Id;

                var EMGR_Ids = _PCReportContext.Exm_M_PromotionDMO.Single(t => t.EYC_Id == EYC_Ids && t.EMP_ActiveFlag == true).EMGR_Id;

                data.grade_detailslist = _PCReportContext.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == EMGR_Ids && t.EMGD_ActiveFlag == true).Distinct().OrderByDescending(t => t.EMGD_From).ThenByDescending(a => a.EMGD_To).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public MaldaProgressReportExam_DTO ixpromotionreport(MaldaProgressReportExam_DTO data)
        {
            try
            {
                data.instname = _PCReportContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                data.clstchname = (from a in _PCReportContext.ClassTeacherMappingDMO
                                   from b in _PCReportContext.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                   && a.IMCT_ActiveFlag == true && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false)
                                   select new MaldaProgressReportExam_DTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                data.remarks = _PCReportContext.ExamPromotionRemarksDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EPRD_Promotionflag == "PE").ToArray();


                var EMCA_Ids = _PCReportContext.Exm_Category_ClassDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).EMCA_Id;

                var EYC_Ids = _PCReportContext.Exm_Yearly_CategoryDMO.Single(t => t.ASMAY_Id == data.ASMAY_Id && t.EMCA_Id == EMCA_Ids
                && t.EYC_ActiveFlg == true).EYC_Id;

                var EMGR_Ids = _PCReportContext.Exm_M_PromotionDMO.Single(t => t.EYC_Id == EYC_Ids && t.EMP_ActiveFlag == true).EMGR_Id;

                data.grade_detailslist = _PCReportContext.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == EMGR_Ids && t.EMGD_ActiveFlag == true).Distinct().OrderByDescending(t => t.EMGD_From).ThenByDescending(a => a.EMGD_To).ToArray();

                // STUDENT LIST
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Malda_Promotion_IX_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    {
                        Value = 1
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
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                // SUBJECT LIST
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Malda_Promotion_IX_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    {
                        Value = 2
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
                        data.getsubjectlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                // GROUP LIST
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Malda_Promotion_IX_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    {
                        Value = 3
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
                        data.getgrouplist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                // SAVED LIST
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Malda_Promotion_IX_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    {
                        Value = 4
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
                        data.getsavedlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                // ATTENDANCE AND RANK LIST
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Malda_Promotion_IX_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    {
                        Value = 5
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
                        data.getattendanceranklist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                data.getoveralldetails = _PCReportContext.Exm_Student_MP_PromotionDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                 && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id).ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
