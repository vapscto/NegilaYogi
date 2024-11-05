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
    public class VikasaAssessment2ReportImpl : Interfaces.VikasaAssessment2ReportInterface
    {
        private static ConcurrentDictionary<string, VikasaSubjectwiseCumulativeReportDTO> _login =
       new ConcurrentDictionary<string, VikasaSubjectwiseCumulativeReportDTO>();

        private readonly ExamContext _PCReportContext;
        public StudentAttendanceReportContext _db;
        ILogger<VikasaAssessment2ReportImpl> _acdimpl;
        public VikasaAssessment2ReportImpl(ExamContext cpContext, StudentAttendanceReportContext db)
        {
            _PCReportContext = cpContext;
            _db = db;
        }
        public VikasaSubjectwiseCumulativeReportDTO Getdetails(VikasaSubjectwiseCumulativeReportDTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _PCReportContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();
                data.getgradenames = _PCReportContext.Exm_Master_GradeDMO.Where(a => a.MI_Id == data.MI_Id && a.EMGR_ActiveFlag == true).ToArray();

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
                data.BasicListYear = (from a in _PCReportContext.AcademicYear
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

                data.clstchname = (from a in _PCReportContext.ClassTeacherMappingDMO
                                   from e in _PCReportContext.HR_Master_Employee_DMO
                                   where (a.HRME_Id == e.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                   && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new BaldwinAllReportDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + " " + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                data.studentlist = (from a in _PCReportContext.Adm_M_Student
                                    from b in _PCReportContext.ExmStudentMarksProcessSubjectwiseDMO
                                    where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id
                                    && a.MI_Id == data.MI_Id && b.EME_Id == data.EME_Id)
                                    select new ProgressCardReportDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                        AMST_DOB = a.AMST_DOB,
                                        AMST_AdmNo = a.AMST_AdmNo,
                                    }).Distinct().ToArray();

                data.savelisttot = _PCReportContext.ExmStudentMarksProcessDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id).Distinct().ToArray();

                data.subjlist = (from a in _PCReportContext.Exm_Category_ClassDMO
                                 from b in _PCReportContext.Exm_Yearly_CategoryDMO
                                 from c in _PCReportContext.Exm_Yearly_Category_ExamsDMO
                                 from d in _PCReportContext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                 from e in _PCReportContext.IVRM_School_Master_SubjectsDMO
                                 where (a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.EYC_Id == b.EYC_Id && c.EME_Id == data.EME_Id && d.EYCE_Id == c.EYCE_Id && d.EYCES_ActiveFlg == true && d.EYCES_AplResultFlg == true && e.MI_Id == data.MI_Id && e.ISMS_Id == d.ISMS_Id)
                                 select new ProgressCardReportDTO
                                 {
                                     ISMS_Id = d.ISMS_Id,
                                     ISMS_SubjectName = e.ISMS_SubjectName,
                                     ISMS_SubjectCode = e.ISMS_SubjectCode,
                                     EYCES_AplResultFlg = d.EYCES_AplResultFlg,
                                     EYCES_MaxMarks = d.EYCES_MaxMarks,
                                     EYCES_MinMarks = d.EYCES_MinMarks,
                                     EMGR_Id = d.EMGR_Id,


                                 }
                               ).Distinct().ToArray();

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
                    cmd.CommandText = "Exam_get_BB_Exam_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.BigInt)
                    {
                        Value = data.EME_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

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
        public VikasaSubjectwiseCumulativeReportDTO get_section(VikasaSubjectwiseCumulativeReportDTO data)
        {
            try
            {
                data.sectionList = (from b in _db.admissionClass
                                    from c in _db.masterSection
                                    from d in _db.Exm_Category_ClassDMO
                                    where (b.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == d.ASMS_Id && d.ECAC_ActiveFlag == true && d.ASMCL_Id == data.ASMCL_Id
                                    && c.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id)
                                    select c).Distinct().OrderBy(t => t.ASMC_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public VikasaSubjectwiseCumulativeReportDTO get_exam(VikasaSubjectwiseCumulativeReportDTO dto)
        {
            try
            {
                var EQuery = _PCReportContext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == dto.MI_Id && t.ASMAY_Id == dto.ASMAY_Id && t.ASMCL_Id == dto.ASMCL_Id && t.ASMS_Id == dto.ASMS_Id).Select(d => d.EME_Id).ToList();

                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = _PCReportContext.exammasterDMO.Where(t => t.MI_Id == dto.MI_Id && t.EME_ActiveFlag == true && EQuery.Contains(t.EME_Id)).ToList();
                dto.ExamList = esmp.OrderBy(a => a.EME_ExamOrder).ToArray();


                dto.studentlist = (from a in _PCReportContext.Adm_M_Student
                                   from b in _PCReportContext.School_Adm_Y_Student
                                   where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == dto.ASMCL_Id && b.ASMS_Id == dto.ASMS_Id && b.ASMAY_Id == dto.ASMAY_Id
                                   && a.MI_Id == dto.MI_Id && a.AMST_SOL == dto.radiotype)
                                   select new ProgressCardReportDTO
                                   {
                                       AMST_Id = a.AMST_Id,
                                       AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) +
                                       (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                       (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName) +
                                       (a.AMST_AdmNo == null || a.AMST_AdmNo == "" ? "" : " : " + a.AMST_AdmNo)).Trim()
                                   }).Distinct().OrderBy(a => a.AMST_FirstName).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public VikasaSubjectwiseCumulativeReportDTO getcategory(VikasaSubjectwiseCumulativeReportDTO dto)
        {
            try
            {
                dto.categoryList = (from a in _PCReportContext.Exm_Yearly_CategoryDMO
                                    from b in _PCReportContext.Exm_Master_CategoryDMO
                                    from c in _PCReportContext.Exm_Category_ClassDMO
                                    from d in _PCReportContext.AcademicYear
                                    where (a.EMCA_Id == b.EMCA_Id && b.EMCA_Id == c.EMCA_Id && a.ASMAY_Id == d.ASMAY_Id && a.EYC_ActiveFlg == true
                                    && b.EMCA_ActiveFlag == true && c.ECAC_ActiveFlag == true && a.ASMAY_Id == dto.ASMAY_Id && c.ASMAY_Id == dto.ASMAY_Id
                                    && c.ASMCL_Id == dto.ASMCL_Id && c.ASMS_Id == dto.ASMS_Id && a.MI_Id == dto.MI_Id && b.MI_Id == dto.MI_Id)
                                    select b).Distinct().ToArray();

                dto.studentlist = (from a in _PCReportContext.Adm_M_Student
                                   from b in _PCReportContext.School_Adm_Y_Student
                                   where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == dto.ASMCL_Id && b.ASMS_Id == dto.ASMS_Id && b.ASMAY_Id == dto.ASMAY_Id
                                   && a.MI_Id == dto.MI_Id && a.AMST_SOL == dto.radiotype)
                                   select new ProgressCardReportDTO
                                   {
                                       AMST_Id = a.AMST_Id,
                                       AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) +
                                       (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                       (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName) +
                                       (a.AMST_AdmNo == null || a.AMST_AdmNo == "" ? "" : " : " + a.AMST_AdmNo)).Trim()
                                   }).Distinct().OrderBy(a => a.AMST_FirstName).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

    }
}

