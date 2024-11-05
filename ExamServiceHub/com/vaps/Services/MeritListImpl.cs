

using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace ExamServiceHub.com.vaps.Services
{
    public class MeritListImpl : Interfaces.MeritListInterface
    {
        private static ConcurrentDictionary<string, MeritListDTO> _login =
         new ConcurrentDictionary<string, MeritListDTO>();

        private readonly ExamContext _examcateReportContext;
        ILogger<MeritListImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public MeritListImpl(ExamContext cpContext, DomainModelMsSqlServerContext db, ILogger<MeritListImpl> acdimpl)
        {
            _examcateReportContext = cpContext;
            _db = db;
            _acdimpl = acdimpl;
        }

        public MeritListDTO getdetails(MeritListDTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _examcateReportContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public MeritListDTO onchangeyear(MeritListDTO data)
        {
            try
            {
                var getemployeeid = _examcateReportContext.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.userid).ToList();
                if (getemployeeid.Count() > 0)
                {
                    var getclassid = _examcateReportContext.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.IMCT_ActiveFlag == true && a.HRME_Id == getemployeeid.FirstOrDefault().Emp_Code).ToList();

                    List<long> classid = new List<long>();
                    foreach (var c in getclassid)
                    {
                        classid.Add(c.ASMCL_Id);
                    }

                    data.classname = (from a in _examcateReportContext.Exm_Category_ClassDMO
                                      from b in _examcateReportContext.AdmissionClass
                                      from c in _examcateReportContext.AcademicYear
                                      where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.ECAC_ActiveFlag == true && b.ASMCL_ActiveFlag == true
                                      && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && classid.Contains(a.ASMCL_Id))
                                      select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }
                else
                {
                    data.classname = (from a in _examcateReportContext.Exm_Category_ClassDMO
                                      from b in _examcateReportContext.AdmissionClass
                                      from c in _examcateReportContext.AcademicYear
                                      where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.ECAC_ActiveFlag == true && b.ASMCL_ActiveFlag == true
                                      && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                                      select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public MeritListDTO onchangeclass(MeritListDTO data)
        {
            try
            {
                data.secname = (from a in _examcateReportContext.Exm_Category_ClassDMO
                                from b in _examcateReportContext.AdmissionClass
                                from c in _examcateReportContext.AcademicYear
                                from d in _examcateReportContext.School_M_Section
                                where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMS_Id == d.ASMS_Id && a.ECAC_ActiveFlag == true
                                && b.ASMCL_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.MI_Id == data.MI_Id)
                                select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public MeritListDTO onchangesection(MeritListDTO data)
        {
            try
            {
                var getemcaid = _examcateReportContext.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToList();

                List<long> emcaid = new List<long>();
                foreach (var c in getemcaid)
                {
                    emcaid.Add(c.EMCA_Id);
                }

                var geteycid = _examcateReportContext.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && emcaid.Contains(a.EMCA_Id) && a.EYC_ActiveFlg == true).Distinct().ToList();

                List<long> eycid = new List<long>();
                foreach (var d in geteycid)
                {
                    eycid.Add(d.EYC_Id);
                }


                var getexamlist = (from a in _examcateReportContext.Exm_Yearly_Category_ExamsDMO
                                   from b in _examcateReportContext.exammasterDMO
                                   where (a.EME_Id == b.EME_Id && a.EYCE_ActiveFlg == true && b.MI_Id == data.MI_Id && eycid.Contains(a.EYC_Id))
                                   select b).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                data.examname = getexamlist;

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public MeritListDTO getAttendetails(MeritListDTO data)
        {
            try
            {
                data.studentAttendanceList = (from a in _examcateReportContext.Adm_M_Student
                                              from b in _examcateReportContext.ExmStudentMarksProcessDMO
                                              from c in _examcateReportContext.School_Adm_Y_Student
                                              where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id
                                              && b.ASMS_Id == data.ASMS_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.EME_Id == data.EME_Id
                                              && c.ASMS_Id == data.ASMS_Id && c.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id && c.AMAY_ActiveFlag == 1
                                              && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S")
                                              select new MeritListDTO
                                              {
                                                  AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) +
                                                  (a.AMST_MiddleName == null ? "" : " " + a.AMST_MiddleName) +
                                                  (a.AMST_LastName == null ? "" : " " + a.AMST_LastName) +
                                                  (a.AMST_AdmNo == null ? "" : " : " + a.AMST_AdmNo)).Trim(),
                                                  AMAY_RollNo = c.AMAY_RollNo,
                                                  ESTMP_TotalMaxMarks = b.ESTMP_TotalMaxMarks,
                                                  ESTMP_TotalObtMarks = b.ESTMP_TotalObtMarks,
                                                  ESTMP_Percentage = b.ESTMP_Percentage,
                                                  ESTMP_SectionRank = b.ESTMP_SectionRank,
                                                  ESTMP_ClassRank = b.ESTMP_ClassRank
                                              }).ToArray();

                data.masterinstitution = _examcateReportContext.Institution_master.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public MeritListDTO getreport(MeritListDTO data)
        {
            try
            {
                data.studentAttendanceList = (from a in _examcateReportContext.Adm_M_Student
                                              from b in _examcateReportContext.ExmStudentMarksProcessDMO
                                              from c in _examcateReportContext.School_Adm_Y_Student
                                              where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id
                                              && b.ASMS_Id == data.ASMS_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.EME_Id == data.EME_Id
                                              && c.ASMS_Id == data.ASMS_Id && c.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id && c.AMAY_ActiveFlag == 1
                                              && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S" && b.ESTMP_SectionRank != null)
                                              select new MeritListDTO
                                              {
                                                  AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) +
                                                  (a.AMST_MiddleName == null ? "" : " " + a.AMST_MiddleName) +
                                                  (a.AMST_LastName == null ? "" : " " + a.AMST_LastName) +
                                                  (a.AMST_AdmNo == null ? "" : " : " + a.AMST_AdmNo)).Trim(),
                                                  AMAY_RollNo = c.AMAY_RollNo,
                                                  ESTMP_TotalMaxMarks = b.ESTMP_TotalMaxMarks,
                                                  ESTMP_TotalObtMarks = b.ESTMP_TotalObtMarks,
                                                  ESTMP_Percentage = b.ESTMP_Percentage,
                                                  ESTMP_SectionRank = b.ESTMP_SectionRank,
                                                  ESTMP_ClassRank = b.ESTMP_ClassRank
                                              }).OrderBy(a => a.ESTMP_SectionRank).ToArray();

                List<ExamLoginPrivilegesReportDTO> result = new List<ExamLoginPrivilegesReportDTO>();
                using (var cmd = _examcateReportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Wise_Promotion_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 1 });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getsubjecttopers = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                using (var cmd = _examcateReportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Wise_Promotion_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 2 });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getgradewisetotal = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


                data.masterinstitution = _examcateReportContext.Institution_master.Where(a => a.MI_Id == data.MI_Id).ToArray();
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
