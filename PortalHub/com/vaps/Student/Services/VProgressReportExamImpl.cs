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
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Services
{
    public class VProgressReportExamImpl : Interfaces.VProgressReportExamInterface
    {
        private static ConcurrentDictionary<string, VikasaSubjectwiseCumulativeReportDTO> _login =
      new ConcurrentDictionary<string, VikasaSubjectwiseCumulativeReportDTO>();

        private readonly ExamContext _PCReportContext;
        public StudentAttendanceReportContext _db;
        ILogger<VProgressReportExamImpl> _acdimpl;
        public VProgressReportExamImpl(ExamContext cpContext, StudentAttendanceReportContext db)
        {
            _PCReportContext = cpContext;
            _db = db;
        }
        public VikasaSubjectwiseCumulativeReportDTO Getdetails(VikasaSubjectwiseCumulativeReportDTO data)//int IVRMM_Id
        {
            try
            {
                List<MasterAcademic> list = new List<DomainModel.Model.MasterAcademic>();
                list = _PCReportContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public VikasaSubjectwiseCumulativeReportDTO showdetails1(VikasaSubjectwiseCumulativeReportDTO data)
        {


            try
            {
                var clssec = (from a in _PCReportContext.Adm_M_Student
                              from b in _PCReportContext.School_Adm_Y_Student
                              from c in _PCReportContext.AdmissionClass
                              from s in _PCReportContext.School_M_Section
                              where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                              select new VikasaSubjectwiseCumulativeReportDTO
                              {
                                  ASMCL_Id = c.ASMCL_Id,
                                  ASMCL_ClassName = c.ASMCL_ClassName,
                                  ASMS_Id = s.ASMS_Id,
                                  ASMC_SectionName = s.ASMC_SectionName
                              }).Distinct().ToList();

                data.BasicListYear = (from a in _PCReportContext.AcademicYear
                                      where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                      select new VikasaSubjectwiseCumulativeReportDTO
                                      {
                                          yearname = a.ASMAY_Year
                                      }
                                      ).Distinct().ToArray();


                data.BasiListclass = (from a in _db.admissionyearstudent
                                      from b in _db.admissionClass
                                      where (a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMCL_Id == clssec.FirstOrDefault().ASMCL_Id)
                                      select new VikasaSubjectwiseCumulativeReportDTO
                                      {
                                          ClassName = b.ASMCL_ClassName
                                      }
                                 ).Distinct().ToArray();

                data.BasiListsectiont = (from a in _db.admissionyearstudent
                                         from b in _db.masterSection

                                         where (a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMS_Id == b.ASMS_Id && b.ASMC_ActiveFlag == 1 && b.MI_Id == data.MI_Id && a.ASMCL_Id == clssec.FirstOrDefault().ASMCL_Id && b.ASMS_Id == clssec.FirstOrDefault().ASMS_Id)
                                         select new VikasaSubjectwiseCumulativeReportDTO
                                         {
                                             sectionname = b.ASMC_SectionName
                                         }
                                         ).Distinct().ToArray();

                data.clstchname = (from a in _PCReportContext.ClassTeacherMappingDMO
                                   from e in _PCReportContext.HR_Master_Employee_DMO
                                   where (a.HRME_Id == e.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == clssec.FirstOrDefault().ASMCL_Id && a.ASMS_Id == clssec.FirstOrDefault().ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new BaldwinAllReportDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       //  HRME_EmployeeFirstName = b.HRME_EmployeeFirstName,
                                       HRME_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + " " + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                data.studentlist = (from a in _PCReportContext.Adm_M_Student
                                    from b in _PCReportContext.ExmStudentMarksProcessSubjectwiseDMO
                                    where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == clssec.FirstOrDefault().ASMCL_Id && b.ASMS_Id == clssec.FirstOrDefault().ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.EME_Id == data.EME_Id)
                                    select new ProgressCardReportDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                        AMST_DOB = a.AMST_DOB,
                                        AMST_AdmNo = a.AMST_AdmNo,

                                    }).Distinct().ToArray();


                data.savelisttot = _PCReportContext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == clssec.FirstOrDefault().ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == clssec.FirstOrDefault().ASMS_Id && t.EME_Id == data.EME_Id).Distinct().ToArray();


                data.subjlist = (from ECC in _PCReportContext.Exm_Category_ClassDMO
                                 from EYC in _PCReportContext.Exm_Yearly_CategoryDMO
                                 from EYCE in _PCReportContext.Exm_Yearly_Category_ExamsDMO
                                 from EYCES in _PCReportContext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                 from n in _PCReportContext.StudentMappingDMO
                                 from i in _PCReportContext.IVRM_School_Master_SubjectsDMO
                                 where (EYC.MI_Id == data.MI_Id && EYC.ASMAY_Id == data.ASMAY_Id && ECC.EMCA_Id == EYC.EMCA_Id && EYCE.EYC_Id == EYC.EYC_Id
                                 && EYCES.EYCE_Id == EYCE.EYCE_Id && n.ISMS_Id == EYCES.ISMS_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id
                                 && n.ASMCL_Id == clssec.FirstOrDefault().ASMCL_Id && n.ASMS_Id == clssec.FirstOrDefault().ASMS_Id && EYC.MI_Id == data.MI_Id && EYC.ASMAY_Id == data.ASMAY_Id
                                 && ECC.ASMCL_Id == clssec.FirstOrDefault().ASMCL_Id && ECC.ASMS_Id == clssec.FirstOrDefault().ASMS_Id && EYCES.EYCES_AplResultFlg == true
                                 && EYCE.EME_Id == data.EME_Id && i.ISMS_Id == EYCES.ISMS_Id && i.MI_Id == data.MI_Id)
                                 select new ProgressCardReportDTO
                                 {
                                     ISMS_Id = EYCES.ISMS_Id,
                                     ISMS_SubjectName = i.ISMS_SubjectName,
                                     ISMS_SubjectCode = i.ISMS_SubjectCode,
                                     EYCES_AplResultFlg = EYCES.EYCES_AplResultFlg,
                                     EYCES_MaxMarks = EYCES.EYCES_MaxMarks,
                                     EYCES_MinMarks = EYCES.EYCES_MinMarks,
                                     EMGR_Id = EYCES.EMGR_Id,
                                     ISMS_Order = Convert.ToInt32(i.ISMS_OrderFlag)

                                 }
                               ).Distinct().OrderBy(a => a.ISMS_Order).ToArray();

                List<int> grade = new List<int>();
                foreach (ProgressCardReportDTO x in data.subjlist)
                {
                    grade.Add(x.EMGR_Id);
                }

                data.grade_details = (from a in _PCReportContext.Exm_Master_GradeDMO
                                      from b in _PCReportContext.Exm_Master_Grade_DetailsDMO
                                      where (a.MI_Id == data.MI_Id && grade.Contains(a.EMGR_Id) && a.EMGR_Id == b.EMGR_Id)
                                      select b
                                     ).Distinct().ToArray();


                List<VikasaSubjectwiseCumulativeReportDTO> result = new List<VikasaSubjectwiseCumulativeReportDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_Exam_get_BB_Exam_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(clssec.FirstOrDefault().ASMCL_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(clssec.FirstOrDefault().ASMS_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                  SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.AMST_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.EME_Id)
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        using (var dataReader = cmd.ExecuteReaderAsync().Result)
                        {
                            while (dataReader.ReadAsync().Result)
                            {
                                result.Add(new VikasaSubjectwiseCumulativeReportDTO
                                {

                                    classheld = Convert.ToDecimal(dataReader["ASA_ClassHeld"].ToString() == null || dataReader["ASA_ClassHeld"].ToString() == "" ? "0" : dataReader["ASA_ClassHeld"].ToString()),
                                    classatt = Convert.ToDecimal(dataReader["ASA_Class_Attended"].ToString() == null || dataReader["ASA_Class_Attended"].ToString() == "" ? "0" : dataReader["ASA_Class_Attended"].ToString()),
                                });

                                data.empAttendence = result.OrderBy(t => t.EYCES_SubjectOrder).ToArray();


                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public VikasaSubjectwiseCumulativeReportDTO get_exam(VikasaSubjectwiseCumulativeReportDTO dto)
        {
            try
            {
                var clssec = (from a in _PCReportContext.Adm_M_Student
                              from b in _PCReportContext.School_Adm_Y_Student
                              from c in _PCReportContext.AdmissionClass
                              from s in _PCReportContext.School_M_Section
                              where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.AMST_Id == b.AMST_Id && a.MI_Id == c.MI_Id && a.MI_Id == dto.MI_Id
                              && b.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id && b.AMST_Id == dto.AMST_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1
                              && b.AMAY_ActiveFlag == 1)
                              select new VikasaSubjectwiseCumulativeReportDTO
                              {
                                  ASMCL_Id = c.ASMCL_Id,
                                  ASMCL_ClassName = c.ASMCL_ClassName,
                                  ASMS_Id = s.ASMS_Id,
                                  ASMC_SectionName = s.ASMC_SectionName
                              }).Distinct().ToList();

                var EQuery = _PCReportContext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == dto.MI_Id && t.ASMAY_Id == dto.ASMAY_Id && t.ASMCL_Id == clssec.FirstOrDefault().ASMCL_Id && t.ASMS_Id == clssec.FirstOrDefault().ASMS_Id).Select(d => d.EME_Id).ToList();


                List<long> emeidnew = new List<long>();

                var getexamlist = (from a in _PCReportContext.Exm_Category_ClassDMO
                                   from b in _PCReportContext.Exm_Master_CategoryDMO
                                   from c in _PCReportContext.Exm_Yearly_CategoryDMO
                                   from d in _PCReportContext.Exm_Yearly_Category_ExamsDMO
                                   from e in _PCReportContext.ExmStudentMarksProcessDMO
                                   where (a.EMCA_Id == b.EMCA_Id && b.EMCA_Id == c.EMCA_Id && c.EYC_Id == d.EYC_Id && d.EME_Id == e.EME_Id && a.ECAC_ActiveFlag == true
                                   && b.EMCA_ActiveFlag == true && c.EYC_ActiveFlg == true && d.EYCE_ActiveFlg == true && a.ASMCL_Id == clssec.FirstOrDefault().ASMCL_Id
                                   && a.ASMS_Id == clssec.FirstOrDefault().ASMS_Id && a.ASMAY_Id == dto.ASMAY_Id && e.ASMCL_Id == clssec.FirstOrDefault().ASMCL_Id
                                   && e.ASMS_Id == clssec.FirstOrDefault().ASMS_Id && e.ASMAY_Id == dto.ASMAY_Id && e.AMST_Id == dto.AMST_Id
                                   && ((d.EYCE_MarksPublishDate == null && e.ESTMP_PublishToStudentFlg == true)
                                   || (d.EYCE_MarksPublishDate != null && e.ESTMP_PublishToStudentFlg == false
                                   && Convert.ToDateTime(System.DateTime.Today.Date) >= Convert.ToDateTime(d.EYCE_MarksPublishDate))
                                   || (d.EYCE_MarksPublishDate != null && e.ESTMP_PublishToStudentFlg == true
                                   && Convert.ToDateTime(System.DateTime.Today.Date) >= Convert.ToDateTime(d.EYCE_MarksPublishDate))))
                                   select new exammasterDMO
                                   {
                                       EME_Id = d.EME_Id
                                   }).Distinct().ToList();

                foreach (var e in getexamlist)
                {
                    emeidnew.Add(e.EME_Id);
                }


                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = _PCReportContext.exammasterDMO.Where(t => t.MI_Id == dto.MI_Id && t.EME_ActiveFlag == true && emeidnew.Contains(t.EME_Id)).ToList();
                dto.ExamList = esmp.OrderBy(a => a.EME_ExamOrder).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public async Task<VikasaSubjectwiseCumulativeReportDTO> showdetails(VikasaSubjectwiseCumulativeReportDTO data)
        {
            try
            {
                data.instname = _PCReportContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();
                var clssec = (from a in _PCReportContext.Adm_M_Student
                              from b in _PCReportContext.School_Adm_Y_Student
                              from c in _PCReportContext.AdmissionClass
                              from s in _PCReportContext.School_M_Section
                              where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                              select new VikasaSubjectwiseCumulativeReportDTO
                              {
                                  ASMCL_Id = c.ASMCL_Id,
                                  ASMCL_ClassName = c.ASMCL_ClassName,
                                  ASMS_Id = s.ASMS_Id,
                                  ASMC_SectionName = s.ASMC_SectionName
                              }).Distinct().ToList();

                data.clstchname = (from a in _PCReportContext.ClassTeacherMappingDMO
                                   from b in _PCReportContext.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == clssec.FirstOrDefault().ASMCL_Id && a.ASMS_Id == clssec.FirstOrDefault().ASMS_Id
                                   && a.IMCT_ActiveFlag == true && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false)
                                   select new BaldwinAllReportDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();


                List<VikasaSubjectwiseCumulativeReportDTO> result = new List<VikasaSubjectwiseCumulativeReportDTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_Vikasa_Exam_get_BB_Exam_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(clssec.FirstOrDefault().ASMCL_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(clssec.FirstOrDefault().ASMS_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                 SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                        SqlDbType.TinyInt)
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

                var from_date = (from a in _PCReportContext.Exm_Category_ClassDMO
                                 from b in _PCReportContext.Exm_Yearly_CategoryDMO
                                 from c in _PCReportContext.Exm_Yearly_Category_ExamsDMO
                                 where (a.MI_Id == data.MI_Id && a.ASMCL_Id == clssec.FirstOrDefault().ASMCL_Id && a.ASMS_Id == clssec.FirstOrDefault().ASMS_Id && a.EMCA_Id == b.EMCA_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id)
                                 select c.EYCE_AttendanceFromDate).FirstOrDefault();

                var to_date = (from a in _PCReportContext.Exm_Category_ClassDMO
                               from b in _PCReportContext.Exm_Yearly_CategoryDMO
                               from c in _PCReportContext.Exm_Yearly_Category_ExamsDMO
                               where (a.MI_Id == data.MI_Id && a.ASMCL_Id == clssec.FirstOrDefault().ASMCL_Id && a.ASMS_Id == clssec.FirstOrDefault().ASMS_Id && a.EMCA_Id == b.EMCA_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id)
                               select c.EYCE_AttendanceToDate).Max();


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_StudentAttendance_W";
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
                        Value = clssec.FirstOrDefault().ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.BigInt)
                    {
                        Value = clssec.FirstOrDefault().ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                 SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
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

                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                            //data.savelist = retObject.ToArray();

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
                    cmd.CommandText = "Portal_StudentAttendance_P";
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
                        Value = clssec.FirstOrDefault().ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.BigInt)
                    {
                        Value = clssec.FirstOrDefault().ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                 SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
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

                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                            //data.savelist = retObject.ToArray();

                        }
                        data.Present_attendence = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }

                data.savelisttot = _PCReportContext.ExmStudentMarksProcessDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == clssec.FirstOrDefault().ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == clssec.FirstOrDefault().ASMS_Id && t.EME_Id == data.EME_Id).Distinct().ToArray();


                data.subjlist = data.savelist.Distinct<VikasaSubjectwiseCumulativeReportDTO>(new progressEqualityComparer1323()).OrderBy(t => t.EYCES_SubjectOrder).ToArray();

                List<int> grade = new List<int>();
                foreach (VikasaSubjectwiseCumulativeReportDTO x in data.subjlist)
                {
                    grade.Add(x.EMGR_Id);
                }

                data.grade_details = (from a in _PCReportContext.Exm_Master_GradeDMO
                                      from b in _PCReportContext.Exm_Master_Grade_DetailsDMO
                                      where (a.MI_Id == data.MI_Id && grade.Contains(a.EMGR_Id) && a.EMGR_Id == b.EMGR_Id)
                                      select b
                                     ).Distinct().ToArray();


                data.remarks = _PCReportContext.ExamPromotionRemarksDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == clssec.FirstOrDefault().ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == clssec.FirstOrDefault().ASMS_Id && t.EME_Id == data.EME_Id && t.EPRD_Promotionflag == "IE").ToArray();




            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }

            return data;
        }
        public VikasaSubjectwiseCumulativeReportDTO get_Category(VikasaSubjectwiseCumulativeReportDTO dto)
        {
            try
            {
                var clssec = (from a in _PCReportContext.Adm_M_Student
                              from b in _PCReportContext.School_Adm_Y_Student
                              from c in _PCReportContext.AdmissionClass
                              from s in _PCReportContext.School_M_Section
                              where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == dto.MI_Id && b.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id && b.AMST_Id == dto.AMST_Id)
                              select new VikasaSubjectwiseCumulativeReportDTO
                              {
                                  ASMCL_Id = c.ASMCL_Id,
                                  ASMCL_ClassName = c.ASMCL_ClassName,
                                  ASMS_Id = s.ASMS_Id,
                                  ASMC_SectionName = s.ASMC_SectionName
                              }).Distinct().ToList();
                var asmclid = clssec.FirstOrDefault().ASMCL_Id;
                var asmsid = clssec.FirstOrDefault().ASMS_Id;

                dto.categoryList = (from a in _PCReportContext.Exm_Yearly_CategoryDMO
                                    from b in _PCReportContext.Exm_Master_CategoryDMO
                                    from c in _PCReportContext.Exm_Category_ClassDMO
                                    from d in _PCReportContext.AcademicYear
                                    where (a.EMCA_Id == b.EMCA_Id && b.EMCA_Id == c.EMCA_Id && a.ASMAY_Id == d.ASMAY_Id && a.EYC_ActiveFlg == true
                                    && b.EMCA_ActiveFlag == true && c.ECAC_ActiveFlag == true && a.ASMAY_Id == dto.ASMAY_Id && c.ASMAY_Id == dto.ASMAY_Id
                                    && c.ASMCL_Id == asmclid && c.ASMS_Id == asmsid && a.MI_Id == dto.MI_Id && b.MI_Id == dto.MI_Id)
                                    select b).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public async Task<VikasaSubjectwiseCumulativeReportDTO> aggregativereport(VikasaSubjectwiseCumulativeReportDTO data)
        {
            try
            {
                data.instname = _PCReportContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                var clssec = (from a in _PCReportContext.Adm_M_Student
                              from b in _PCReportContext.School_Adm_Y_Student
                              from c in _PCReportContext.AdmissionClass
                              from s in _PCReportContext.School_M_Section
                              where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                              select new VikasaSubjectwiseCumulativeReportDTO
                              {
                                  ASMCL_Id = c.ASMCL_Id,
                                  ASMCL_ClassName = c.ASMCL_ClassName,
                                  ASMS_Id = s.ASMS_Id,
                                  ASMC_SectionName = s.ASMC_SectionName
                              }).Distinct().ToList();
                var asmclid = clssec.FirstOrDefault().ASMCL_Id;
                var asmsid = clssec.FirstOrDefault().ASMS_Id;

                data.clstchname = (from a in _PCReportContext.ClassTeacherMappingDMO
                                   from b in _PCReportContext.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == asmclid && a.ASMS_Id == asmsid
                                   && a.IMCT_ActiveFlag == true && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false)
                                   select new BaldwinAllReportDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();


                List<VikasaSubjectwiseCumulativeReportDTO> result = new List<VikasaSubjectwiseCumulativeReportDTO>();

                // STUDENT LIST
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_V_Aggregative_Progresscard_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = asmclid
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = asmsid
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@EMCA_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.EMCA_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",
                   SqlDbType.VarChar)
                    {
                        Value = 1
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
                    cmd.CommandText = "Portal_V_Aggregative_Progresscard_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = asmclid
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = asmsid
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@EMCA_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.EMCA_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",
                   SqlDbType.VarChar)
                    {
                        Value = 2
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
                    cmd.CommandText = "Portal_V_Aggregative_Progresscard_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = asmclid
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = asmsid
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EMCA_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.EMCA_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",
                   SqlDbType.VarChar)
                    {
                        Value = 3
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
                    cmd.CommandText = "Portal_V_Aggregative_Progresscard_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = asmclid
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = asmsid
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EMCA_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.EMCA_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",
                   SqlDbType.VarChar)
                    {
                        Value = 4
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

                data.remarks = _PCReportContext.ExamPromotionRemarksDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == asmclid && t.MI_Id == data.MI_Id && t.ASMS_Id == asmsid && t.EPRD_Promotionflag == "PE").ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }

            return data;
        }
    }
    class progressEqualityComparer1323 : IEqualityComparer<VikasaSubjectwiseCumulativeReportDTO>
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
