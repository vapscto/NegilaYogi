﻿using DataAccessMsSqlServerProvider;
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

namespace ExamServiceHub.com.vaps.Services
{
    public class VikasaProgressReportExamImpl : Interfaces.VikasaProgressReportExamInterface
    {
        private static ConcurrentDictionary<string, VikasaSubjectwiseCumulativeReportDTO> _login =
      new ConcurrentDictionary<string, VikasaSubjectwiseCumulativeReportDTO>();

        private readonly ExamContext _PCReportContext;
        public StudentAttendanceReportContext _db;
        ILogger<VikasaProgressReportExamImpl> _acdimpl;
        public VikasaProgressReportExamImpl(ExamContext cpContext, StudentAttendanceReportContext db)
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
                data.BasicListYear = (from a in _PCReportContext.AcademicYear
                                      where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                      select new VikasaSubjectwiseCumulativeReportDTO
                                      {
                                          yearname = a.ASMAY_Year
                                      }
                                      ).Distinct().ToArray();


                data.BasiListclass = (from a in _db.admissionyearstudent
                                      from b in _db.admissionClass
                                      where (a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && b.MI_Id == data.MI_Id
                                      && b.ASMCL_Id == data.ASMCL_Id)
                                      select new VikasaSubjectwiseCumulativeReportDTO
                                      {
                                          ClassName = b.ASMCL_ClassName
                                      }
                                 ).Distinct().ToArray();

                data.BasiListsectiont = (from a in _db.admissionyearstudent
                                         from b in _db.masterSection

                                         where (a.ASMAY_Id == data.ASMAY_Id && a.ASMS_Id == b.ASMS_Id && b.ASMC_ActiveFlag == 1 && b.MI_Id == data.MI_Id
                                         && a.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id)
                                         select new VikasaSubjectwiseCumulativeReportDTO
                                         {
                                             sectionname = b.ASMC_SectionName
                                         }
                                         ).Distinct().ToArray();

                data.clstchname = (from a in _PCReportContext.ClassTeacherMappingDMO
                                   from e in _PCReportContext.HR_Master_Employee_DMO
                                   where (a.HRME_Id == e.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new BaldwinAllReportDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + " " + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                data.studentlist = (from a in _PCReportContext.Adm_M_Student
                                    from b in _PCReportContext.ExmStudentMarksProcessSubjectwiseDMO
                                    where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.EME_Id == data.EME_Id)
                                    select new ProgressCardReportDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                        AMST_DOB = a.AMST_DOB,
                                        AMST_AdmNo = a.AMST_AdmNo,
                                    }).Distinct().ToArray();

                data.savelisttot = _PCReportContext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id).Distinct().ToArray();


                data.subjlist = (from ECC in _PCReportContext.Exm_Category_ClassDMO
                                 from EYC in _PCReportContext.Exm_Yearly_CategoryDMO
                                 from EYCE in _PCReportContext.Exm_Yearly_Category_ExamsDMO
                                 from EYCES in _PCReportContext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                 from n in _PCReportContext.StudentMappingDMO
                                 from i in _PCReportContext.IVRM_School_Master_SubjectsDMO
                                 where (EYC.MI_Id == data.MI_Id && EYC.ASMAY_Id == data.ASMAY_Id && ECC.EMCA_Id == EYC.EMCA_Id && EYCE.EYC_Id == EYC.EYC_Id
                                 && EYCES.EYCE_Id == EYCE.EYCE_Id && n.ISMS_Id == EYCES.ISMS_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id
                                 && n.ASMCL_Id == data.ASMCL_Id && n.ASMS_Id == data.ASMS_Id && EYC.MI_Id == data.MI_Id && EYC.ASMAY_Id == data.ASMAY_Id
                                 && ECC.ASMCL_Id == data.ASMCL_Id && ECC.ASMS_Id == data.ASMS_Id && EYCES.EYCES_AplResultFlg == true
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
                    cmd.CommandText = "Exam_get_BB_Exam_Details";
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
        public VikasaSubjectwiseCumulativeReportDTO get_exam(VikasaSubjectwiseCumulativeReportDTO dto)
        {
            try
            {
                var EQuery = _PCReportContext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == dto.MI_Id && t.ASMAY_Id == dto.ASMAY_Id && t.ASMCL_Id == dto.ASMCL_Id && t.ASMS_Id == dto.ASMS_Id).Select(d => d.EME_Id).ToList();

                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = _PCReportContext.exammasterDMO.Where(t => t.MI_Id == dto.MI_Id && t.EME_ActiveFlag == true && EQuery.Contains(t.EME_Id)).ToList();
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

                string amst_id = "0";

                List<long> amstid = new List<long>();

                if(data.student_temp != null && data.student_temp.Length > 0)
                {
                    foreach (var d in data.student_temp)
                    {
                        amst_id = amst_id + "," + d.AMST_Id;
                        amstid.Add(d.AMST_Id);
                    }
                }
                else
                {
                    var getamstids = _PCReportContext.School_Adm_Y_Student.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id 
                    && a.ASMS_Id == data.ASMS_Id).ToList();

                    foreach (var d in getamstids)
                    {
                        amst_id = amst_id + "," + d.AMST_Id;
                        amstid.Add(d.AMST_Id);
                    }
                }
               

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


                List<VikasaSubjectwiseCumulativeReportDTO> result = new List<VikasaSubjectwiseCumulativeReportDTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Vikasa_Exam_get_BB_Exam_Details_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;
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

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                    {
                        Value = data.EME_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@amst_id", SqlDbType.VarChar)
                    {
                        Value = amst_id
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
                                 where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id)
                                 select c.EYCE_AttendanceFromDate).FirstOrDefault();

                var to_date = (from a in _PCReportContext.Exm_Category_ClassDMO
                               from b in _PCReportContext.Exm_Yearly_CategoryDMO
                               from c in _PCReportContext.Exm_Yearly_Category_ExamsDMO
                               where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id)
                               select c.EYCE_AttendanceToDate).Max();


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Vikasa_StudentAttendance_W";
                    cmd.CommandType = CommandType.StoredProcedure;

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
                    cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date)
                    {
                        Value = from_date
                    });

                    cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date)
                    {
                        Value = to_date
                    });

                    cmd.Parameters.Add(new SqlParameter("@amst_id", SqlDbType.VarChar)
                    {
                        Value = amst_id
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
                    cmd.CommandText = "Vikasa_StudentAttendance_P";
                    cmd.CommandType = CommandType.StoredProcedure;

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

                    cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date)
                    {
                        Value = from_date
                    });

                    cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date)
                    {
                        Value = to_date
                    });

                    cmd.Parameters.Add(new SqlParameter("@amst_id", SqlDbType.VarChar)
                    {
                        Value = amst_id
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

                data.savelisttot = _PCReportContext.ExmStudentMarksProcessDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && amstid.Contains(t.AMST_Id)).Distinct().ToArray();


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


                data.remarks = _PCReportContext.ExamPromotionRemarksDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.EPRD_Promotionflag == "IE" && amstid.Contains(t.AMST_Id)).ToArray();

                using (var cmd = _PCReportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Vikasa_Exam_Sports_Remarks_Details_New";
                    cmd.CommandType = CommandType.StoredProcedure;

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

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                    {
                        Value = data.EME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@amst_id", SqlDbType.VarChar)
                    {
                        Value = amst_id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReaderAsync().Result)
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getsubjectavg = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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
        public async Task<VikasaSubjectwiseCumulativeReportDTO> savedetailsnew(VikasaSubjectwiseCumulativeReportDTO data)
        {
            try
            {
                string amst_id = "0";

                List<long> amstid = new List<long>();

                if (data.student_temp != null && data.student_temp.Length > 0)
                {
                    foreach (var d in data.student_temp)
                    {
                        amst_id = amst_id + "," + d.AMST_Id;
                        amstid.Add(d.AMST_Id);
                    }
                }
                else
                {
                    var getamstids = _PCReportContext.School_Adm_Y_Student.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                    && a.ASMS_Id == data.ASMS_Id).ToList();

                    foreach (var d in getamstids)
                    {
                        amst_id = amst_id + "," + d.AMST_Id;
                        amstid.Add(d.AMST_Id);
                    }
                }

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


                List<VikasaSubjectwiseCumulativeReportDTO> result = new List<VikasaSubjectwiseCumulativeReportDTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Vikasa_Exam_get_BB_Exam_Details_Semester_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",SqlDbType.VarChar)
                    {
                        Value = data.EME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id_New",SqlDbType.VarChar)
                    {
                        Value = data.EME_Id_New
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                    {
                        Value = amst_id
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
                                    internalgrade = (dataReader["InternalGrade"].ToString() == null || dataReader["InternalGrade"].ToString() == "" ? "" : dataReader["InternalGrade"].ToString()),
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
                                 where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id)
                                 select c.EYCE_AttendanceFromDate).FirstOrDefault();

                var to_date = (from a in _PCReportContext.Exm_Category_ClassDMO
                               from b in _PCReportContext.Exm_Yearly_CategoryDMO
                               from c in _PCReportContext.Exm_Yearly_Category_ExamsDMO
                               where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id)
                               select c.EYCE_AttendanceToDate).Max();


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Vikasa_StudentAttendance_W";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@from",SqlDbType.Date)
                    {
                        Value = from_date
                    });
                    cmd.Parameters.Add(new SqlParameter("@to",SqlDbType.Date)
                    {
                        Value = to_date
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",SqlDbType.VarChar)
                    {
                        Value = amst_id
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
                    cmd.CommandText = "Vikasa_StudentAttendance_P";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@from",SqlDbType.Date)
                    {
                        Value = from_date
                    });
                    cmd.Parameters.Add(new SqlParameter("@to",SqlDbType.Date)
                    {
                        Value = to_date
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                    {
                        Value = amst_id
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

                        }
                        data.Present_attendence = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }

                data.savelisttot = _PCReportContext.ExmStudentMarksProcessDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && amstid.Contains(t.AMST_Id)).Distinct().ToArray();


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


                data.remarks = _PCReportContext.ExamPromotionRemarksDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.EPRD_Promotionflag == "IE" && amstid.Contains(t.AMST_Id)).ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }

            return data;
        }
        public async Task<VikasaSubjectwiseCumulativeReportDTO> cbsesavedetails(VikasaSubjectwiseCumulativeReportDTO data)
        {
            try
            {
                string amst_id = "0";

                List<long> amstid = new List<long>();

                foreach (var d in data.student_temp)
                {
                    amst_id = amst_id + "," + d.AMST_Id;
                    amstid.Add(d.AMST_Id);
                }

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


                List<VikasaSubjectwiseCumulativeReportDTO> result = new List<VikasaSubjectwiseCumulativeReportDTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Vikasa_Get_Exam_Subject_SubSubj_Marks_List_New1";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;

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

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                    {
                        Value = data.EME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                    {
                        Value = amst_id
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
                        data.cbsesavelist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                var from_date = (from a in _PCReportContext.Exm_Category_ClassDMO
                                 from b in _PCReportContext.Exm_Yearly_CategoryDMO
                                 from c in _PCReportContext.Exm_Yearly_Category_ExamsDMO
                                 where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id)
                                 select c.EYCE_AttendanceFromDate).FirstOrDefault();

                var to_date = (from a in _PCReportContext.Exm_Category_ClassDMO
                               from b in _PCReportContext.Exm_Yearly_CategoryDMO
                               from c in _PCReportContext.Exm_Yearly_Category_ExamsDMO
                               where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id)
                               select c.EYCE_AttendanceToDate).Max();


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Vikasa_StudentAttendance_W";
                    cmd.CommandType = CommandType.StoredProcedure;

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
                    cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date)
                    {
                        Value = from_date
                    });
                    cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date)
                    {
                        Value = to_date
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                    {
                        Value = amst_id
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
                    cmd.CommandText = "Vikasa_StudentAttendance_P";
                    cmd.CommandType = CommandType.StoredProcedure;

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
                    cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date)
                    {
                        Value = from_date
                    });
                    cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date)
                    {
                        Value = to_date
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                    {
                        Value = amst_id
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
                        }
                        data.Present_attendence = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }

                data.savelisttot = _PCReportContext.ExmStudentMarksProcessDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && amstid.Contains(t.AMST_Id)).Distinct().ToArray();

                data.remarks = _PCReportContext.ExamPromotionRemarksDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.EPRD_Promotionflag == "IE" && amstid.Contains(t.AMST_Id)).ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Vikasa_Get_Exam_Subject_SubSubjectNamesList_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;
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

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                    {
                        Value = data.EME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = 1
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                    {
                        Value = amst_id
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
                        data.cbsesubexamlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Vikasa_Get_Exam_Subject_SubSubjectNamesList_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id

                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                    {
                        Value = data.EME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = 2
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                    {
                        Value = amst_id
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
                        data.cbsestudentlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Vikasa_Get_Exam_Subject_SubSubjectNamesList_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",SqlDbType.VarChar)
                    {
                        Value = data.EME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",SqlDbType.VarChar)
                    {
                        Value = 3
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                    {
                        Value = amst_id
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
                        data.cbsesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                if (data.EMGR_Id != 0)
                {
                    data.grade_details = (from a in _PCReportContext.Exm_Master_GradeDMO
                                          from b in _PCReportContext.Exm_Master_Grade_DetailsDMO
                                          where (a.EMGR_Id == b.EMGR_Id && a.MI_Id == data.MI_Id && a.EMGR_ActiveFlag == true && b.EMGD_ActiveFlag == true
                                          && a.EMGR_Id == data.EMGR_Id && b.EMGR_Id == data.EMGR_Id)
                                          select b).Distinct().OrderBy(a => a.EMGD_Name).ToArray();
                }
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }

            return data;
        }
        public async Task<VikasaSubjectwiseCumulativeReportDTO> aggregativereport(VikasaSubjectwiseCumulativeReportDTO data)
        {
            try
            {
                string amst_id = "0";

                List<long> amstid = new List<long>();

                foreach (var d in data.student_temp)
                {
                    amst_id = amst_id + "," + d.AMST_Id;
                    amstid.Add(d.AMST_Id);
                }



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


                List<VikasaSubjectwiseCumulativeReportDTO> result = new List<VikasaSubjectwiseCumulativeReportDTO>();

                // STUDENT LIST
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Vikasa_Aggregative_Progresscard_Report_New";
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

                    cmd.Parameters.Add(new SqlParameter("@EMCA_Id", SqlDbType.VarChar)
                    {
                        Value = data.EMCA_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = 1
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMSTId", SqlDbType.VarChar)
                    {
                        Value = amst_id
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
                    cmd.CommandText = "Vikasa_Aggregative_Progresscard_Report_New";
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

                    cmd.Parameters.Add(new SqlParameter("@EMCA_Id", SqlDbType.VarChar)
                    {
                        Value = data.EMCA_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = 2
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMSTId", SqlDbType.VarChar)
                    {
                        Value = amst_id
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
                    cmd.CommandText = "Vikasa_Aggregative_Progresscard_Report_New";
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

                    cmd.Parameters.Add(new SqlParameter("@EMCA_Id", SqlDbType.VarChar)
                    {
                        Value = data.EMCA_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = 3
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMSTId", SqlDbType.VarChar)
                    {
                        Value = amst_id
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
                    cmd.CommandText = "Vikasa_Aggregative_Progresscard_Report_New";
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

                    cmd.Parameters.Add(new SqlParameter("@EMCA_Id", SqlDbType.VarChar)
                    {
                        Value = data.EMCA_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = 4
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMSTId", SqlDbType.VarChar)
                    {
                        Value = amst_id
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

                data.remarks = _PCReportContext.ExamPromotionRemarksDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EPRD_Promotionflag == "PE" && amstid.Contains(t.AMST_Id)).ToArray();
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