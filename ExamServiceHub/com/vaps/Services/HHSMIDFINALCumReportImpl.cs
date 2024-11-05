
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
    public class HHSMIDFINALCumReportImpl : Interfaces.HHSMIDFINALCumReportInterface
    {
        private readonly ExamContext _CumulativeReportContext;
        ILogger<CumulativeReportImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public HHSMIDFINALCumReportImpl(ExamContext cpContext, DomainModelMsSqlServerContext db, ILogger<CumulativeReportImpl> _log)
        {
            _CumulativeReportContext = cpContext;
            _db = db;
            _acdimpl = _log;
        }
        public HHSMIDFINALCumReportDTO Getdetails(HHSMIDFINALCumReportDTO data)
        {
            HHSMIDFINALCumReportDTO getdata = new HHSMIDFINALCumReportDTO();
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _CumulativeReportContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                getdata.yearlist = list.ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = _CumulativeReportContext.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).OrderBy(t => t.ASMC_Order).ToList();
                getdata.seclist = seclist.ToArray();

                List<AdmissionClass> admlist = new List<AdmissionClass>();
                admlist = _CumulativeReportContext.AdmissionClass.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).OrderBy(t => t.ASMCL_Order).ToList();
                getdata.classlist = admlist.ToArray();

                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = _CumulativeReportContext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).OrderBy(t => t.EME_ExamOrder).ToList();
                getdata.exmstdlist = esmp.ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;

        }
        public HHSMIDFINALCumReportDTO validateordernumber(HHSMIDFINALCumReportDTO data)
        {
            HHSMIDFINALCumReportDTO getdata = new HHSMIDFINALCumReportDTO();

            return getdata;
        }
        public async Task<HHSMIDFINALCumReportDTO> savedetails(HHSMIDFINALCumReportDTO data)
        {
            try
            {
                data.configuration = _CumulativeReportContext.Exm_ConfigurationDMO.Where(t => t.MI_Id == data.MI_Id).ToArray();

                data.instname = _CumulativeReportContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                string order = "";
                var get_configuration = _CumulativeReportContext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
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

                List<long> amstid = new List<long>();

                if (data.AMST_Ids != null && data.AMST_Ids.Length > 0)
                {
                    foreach (var d in data.AMST_Ids)
                    {
                        amstid.Add(d);
                    }
                }
                else
                {
                    var getamstids = _CumulativeReportContext.School_Adm_Y_Student.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                    && a.ASMS_Id == data.ASMS_Id).ToList();

                    foreach (var d in getamstids)
                    {
                        amstid.Add(d.AMST_Id);
                    }
                }

                List<HHSAllReportDTO> studentlist = new List<HHSAllReportDTO>();

                studentlist = (from f in _CumulativeReportContext.Adm_M_Student
                               from h in _CumulativeReportContext.School_Adm_Y_Student
                               from c in _CumulativeReportContext.AdmissionClass
                               from s in _CumulativeReportContext.School_M_Section
                               from y in _CumulativeReportContext.AcademicYear
                               where (h.AMST_Id == f.AMST_Id && h.ASMAY_Id == y.ASMAY_Id && h.ASMAY_Id == y.ASMAY_Id
                               && h.ASMCL_Id == c.ASMCL_Id && h.ASMS_Id == s.ASMS_Id && h.ASMAY_Id == y.ASMAY_Id
                               && h.ASMAY_Id == data.ASMAY_Id && h.ASMCL_Id == data.ASMCL_Id && h.ASMS_Id == data.ASMS_Id
                               && amstid.Contains(h.AMST_Id))
                               select new HHSAllReportDTO
                               {
                                   AMST_Id = h.AMST_Id,
                                   AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "" ? "" : f.AMST_FirstName) + 
                                   (f.AMST_MiddleName == null || f.AMST_MiddleName == "" || f.AMST_MiddleName == "0" ? "" : " " + f.AMST_MiddleName) + 
                                   (f.AMST_LastName == null || f.AMST_LastName == "" || f.AMST_LastName == "0" ? "" : " " + f.AMST_LastName)).Trim(),
                                   AMST_AdmNo = f.AMST_AdmNo,
                                   AMAY_RollNo = h.AMAY_RollNo,
                                   AMST_RegistrationNo = f.AMST_RegistrationNo

                               }).Distinct().ToList();

                var propertyInfo = typeof(HHSAllReportDTO).GetProperty(order);
                data.studlist = studentlist.Distinct().OrderBy(x => propertyInfo.GetValue(x, null)).ToArray();

                List<HHSMIDFINALCumReportDTO> result = new List<HHSMIDFINALCumReportDTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_HHS_MID_FINAL_CUMULATIVE_REPORT";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.BigInt){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.BigInt){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",SqlDbType.BigInt){Value = data.EME_Id});

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                result.Add(new HHSMIDFINALCumReportDTO
                                {
                                    AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                    AMST_FirstName = (dataReader["stname"].ToString() == null ? " " : dataReader["stname"].ToString()).Trim(),
                                    AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString() == null || dataReader["AMST_AdmNo"].ToString() == "" ? "" : dataReader["AMST_AdmNo"].ToString()),
                                    AMAY_RollNo = Convert.ToInt32(dataReader["AMAY_RollNo"].ToString() == null || dataReader["AMAY_RollNo"].ToString() == "" ? "" : dataReader["AMAY_RollNo"].ToString()),
                                    AMST_RegistrationNo = Convert.ToString(dataReader["AMST_RegistrationNo"].ToString() == null || dataReader["AMST_RegistrationNo"].ToString() == "" ? "" : dataReader["AMST_RegistrationNo"].ToString()),
                                    ISMS_Id = Convert.ToInt32(dataReader["ISMS_Id"].ToString()),
                                    EMSE_Id = Convert.ToInt32(dataReader["EMSE_Id"].ToString()),
                                    EMSE_SubExamName = (dataReader["EMSE_SubExamName"].ToString() == null || dataReader["EMSE_SubExamName"].ToString() == "" ? "" : dataReader["EMSE_SubExamName"].ToString()),

                                    ESTMPS_ObtainedMarks = Convert.ToDecimal(dataReader["ESTMPS_ObtainedMarks"].ToString() == null || dataReader["ESTMPS_ObtainedMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_ObtainedMarks"].ToString()),
                                    ESTMPS_MaxMarks = Convert.ToDecimal(dataReader["ESTMPS_MaxMarks"].ToString() == null || dataReader["ESTMPS_MaxMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_MaxMarks"].ToString()),
                                    ESTMPS_PassFailFlg = (dataReader["ESTMPS_PassFailFlg"].ToString() == null || dataReader["ESTMPS_PassFailFlg"].ToString() == "" ? "" : dataReader["ESTMPS_PassFailFlg"].ToString()),
                                    ESTMPS_ObtainedGrade = (dataReader["ESTMPS_ObtainedGrade"].ToString() == null || dataReader["ESTMPS_ObtainedGrade"].ToString() == "" ? "" : dataReader["ESTMPS_ObtainedGrade"].ToString()),
                                });
                                data.savelist = result.Distinct().OrderBy(t => t.ISMS_Id).ThenBy(t => t.EMSE_Id).ToList();                               
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }

                List<HHSMIDFINALCumReportDTO> result1 = new List<HHSMIDFINALCumReportDTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_HHS_MID_FINAL_SUB_SUBEXAM";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.BigInt){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.BigInt){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",SqlDbType.BigInt){Value = data.EME_Id});
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                result1.Add(new HHSMIDFINALCumReportDTO
                                {
                                    ISMS_Id = Convert.ToInt32(dataReader["ISMS_Id"].ToString()),
                                    ISMS_SubjectName = (dataReader["ISMS_SubjectName"].ToString() == null || dataReader["ISMS_SubjectName"].ToString() == "" ? "" : dataReader["ISMS_SubjectName"].ToString()),

                                    EMSE_Id = Convert.ToInt32(dataReader["EMSE_Id"].ToString()),
                                    EMSE_SubExamName = (dataReader["EMSE_SubExamName"].ToString() == null || dataReader["EMSE_SubExamName"].ToString() == "" ? "" : dataReader["EMSE_SubExamName"].ToString()),

                                    EYCES_AplResultFlg = Convert.ToBoolean(dataReader["EYCES_AplResultFlg"].ToString()),
                                    EYCES_MarksDisplayFlg = Convert.ToBoolean(dataReader["EYCES_MarksDisplayFlg"].ToString()),
                                    EYCES_GradeDisplayFlg = Convert.ToBoolean(dataReader["EYCES_GradeDisplayFlg"].ToString()),
                                    EYCESSS_MarksFlg = Convert.ToBoolean(dataReader["EYCESSS_MarksFlg"].ToString()),
                                    EYCESSS_GradesFlg = Convert.ToBoolean(dataReader["EYCESSS_GradesFlg"].ToString()),
                                });
                                data.subwithsubexm = result1.Distinct().ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }

                var exmrank = _CumulativeReportContext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id 
                && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && amstid.Contains(t.AMST_Id)).ToList();
                data.exmrank = exmrank.ToArray();

                var electivemarks1 = (from a in _CumulativeReportContext.Exm_Yearly_CategoryDMO
                                      from b in _CumulativeReportContext.Exm_Category_ClassDMO
                                      from c in _CumulativeReportContext.Exm_Yearly_Category_ExamsDMO
                                      from d in _CumulativeReportContext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                      from e in _CumulativeReportContext.ExmStudentMarksProcessSubjectwiseDMO
                                      from f in _CumulativeReportContext.IVRM_School_Master_SubjectsDMO
                                      where (a.MI_Id == b.MI_Id && a.MI_Id == e.MI_Id && f.ISMS_Id == e.ISMS_Id && a.MI_Id == data.MI_Id &&
                                      a.ASMAY_Id == b.ASMAY_Id && a.ASMAY_Id == e.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id &&
                                      a.EMCA_Id == b.EMCA_Id && a.EYC_Id == c.EYC_Id && d.EYCE_Id == c.EYCE_Id && d.EYCES_AplResultFlg == false 
                                      && d.ISMS_Id == e.ISMS_Id && b.ASMCL_Id == e.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && b.ASMCL_Id == data.ASMCL_Id 
                                      && b.ASMS_Id == data.ASMS_Id && e.EME_Id == data.EME_Id && c.EYCE_ActiveFlg == true && amstid.Contains(e.AMST_Id))
                                      select new HHSMIDFINALCumReportDTO
                                      {
                                          ISMS_SubjectName = f.ISMS_SubjectName,
                                          ESTMPS_ObtainedMarks = e.ESTMPS_ObtainedMarks,
                                          ESTMPS_ClassAverage = e.ESTMPS_ClassAverage,
                                          ESTMPS_ClassHighest = e.ESTMPS_ClassHighest,
                                          ESTMPS_SectionAverage = e.ESTMPS_SectionAverage,
                                          ESTMPS_SectionHighest = e.ESTMPS_SectionHighest,
                                          ESTMPS_PassFailFlg = e.ESTMPS_PassFailFlg,
                                          ESTMPS_MaxMarks = e.ESTMPS_MaxMarks,
                                          ESTMPS_ObtainedGrade = e.ESTMPS_ObtainedGrade,
                                          ESTMPS_Id = e.ESTMPS_Id,
                                          ISMS_Id = e.ISMS_Id,
                                          EME_Id = e.EME_Id,
                                          ASMAY_Id = e.ASMAY_Id,
                                          ASMCL_Id = e.ASMCL_Id,
                                          ASMS_Id = e.ASMS_Id,
                                          AMST_Id = e.AMST_Id,
                                          ISMS_OrderFlag = f.ISMS_OrderFlag
                                      }).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToList();
                data.electivemarks = electivemarks1.ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public async Task<HHSMIDFINALCumReportDTO> savedetailsnew(HHSMIDFINALCumReportDTO data)
        {
            try
            {
                data.configuration = _CumulativeReportContext.Exm_ConfigurationDMO.Where(t => t.MI_Id == data.MI_Id).ToArray();

                data.instname = _CumulativeReportContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                string order = "";
                var get_configuration = _CumulativeReportContext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
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

                List<long> amstid = new List<long>();

                if (data.AMST_Ids != null && data.AMST_Ids.Length > 0)
                {
                    foreach (var d in data.AMST_Ids)
                    {
                        amstid.Add(d);
                    }
                }
                else
                {
                    var getamstids = _CumulativeReportContext.School_Adm_Y_Student.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                    && a.ASMS_Id == data.ASMS_Id).ToList();

                    foreach (var d in getamstids)
                    {
                        amstid.Add(d.AMST_Id);
                    }
                }

                List<HHSAllReportDTO> studentlist = new List<HHSAllReportDTO>();

                studentlist = (from f in _CumulativeReportContext.Adm_M_Student
                               from h in _CumulativeReportContext.School_Adm_Y_Student
                               from c in _CumulativeReportContext.AdmissionClass
                               from s in _CumulativeReportContext.School_M_Section
                               from y in _CumulativeReportContext.AcademicYear
                               where (h.AMST_Id == f.AMST_Id && h.ASMAY_Id == y.ASMAY_Id && h.ASMAY_Id == y.ASMAY_Id
                               && h.ASMCL_Id == c.ASMCL_Id && h.ASMS_Id == s.ASMS_Id && h.ASMAY_Id == y.ASMAY_Id
                               && h.ASMAY_Id == data.ASMAY_Id && h.ASMCL_Id == data.ASMCL_Id && h.ASMS_Id == data.ASMS_Id
                               && amstid.Contains(h.AMST_Id))
                               select new HHSAllReportDTO
                               {
                                   AMST_Id = h.AMST_Id,
                                   AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "" ? "" : f.AMST_FirstName) + 
                                   (f.AMST_MiddleName == null || f.AMST_MiddleName == "" || f.AMST_MiddleName == "0" ? "" : " " + f.AMST_MiddleName) + 
                                   (f.AMST_LastName == null || f.AMST_LastName == "" || f.AMST_LastName == "0" ? "" : " " + f.AMST_LastName)).Trim(),
                                   AMST_AdmNo = f.AMST_AdmNo,
                                   AMAY_RollNo = h.AMAY_RollNo,
                                   AMST_RegistrationNo = f.AMST_RegistrationNo

                               }).Distinct().ToList();

                var propertyInfo = typeof(HHSAllReportDTO).GetProperty(order);
                data.studlist = studentlist.Distinct().OrderBy(x => propertyInfo.GetValue(x, null)).ToArray();

                List<HHSMIDFINALCumReportDTO> result = new List<HHSMIDFINALCumReportDTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Subject_Subsubject_Subexam_Report";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",SqlDbType.VarChar){Value = data.EME_Id});
                    cmd.Parameters.Add(new SqlParameter("@Flag",SqlDbType.VarChar){Value = 1});
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.savelistnew = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Subject_Subsubject_Subexam_Report";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",SqlDbType.VarChar){Value = data.EME_Id});
                    cmd.Parameters.Add(new SqlParameter("@Flag",SqlDbType.VarChar){Value = 2});
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.subjectlistnew = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Subject_Subsubject_Subexam_Report";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",SqlDbType.VarChar){Value = data.EME_Id});
                    cmd.Parameters.Add(new SqlParameter("@Flag",SqlDbType.VarChar){Value = 3});

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.subjectlistwithdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }

                var exmrank = _CumulativeReportContext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id 
                && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && amstid.Contains(t.AMST_Id)).ToList();
                data.exmrank = exmrank.ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }

            return data;
        }
        public HHSMIDFINALCumReportDTO cumulativereport(HHSMIDFINALCumReportDTO data)
        {
            try
            {               
                data.configuration = _CumulativeReportContext.Exm_ConfigurationDMO.Where(t => t.MI_Id == data.MI_Id).ToArray();

                data.instname = _CumulativeReportContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                string order = "";
                var get_configuration = _CumulativeReportContext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
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

                List<long> amstid = new List<long>();

                if (data.AMST_Ids != null && data.AMST_Ids.Length > 0)
                {
                    foreach (var d in data.AMST_Ids)
                    {
                        amstid.Add(d);
                    }
                }
                else
                {
                    var getamstids = _CumulativeReportContext.School_Adm_Y_Student.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                    && a.ASMS_Id == data.ASMS_Id).ToList();

                    foreach (var d in getamstids)
                    {
                        amstid.Add(d.AMST_Id);
                    }
                }

                List<HHSAllReportDTO> studentlist = new List<HHSAllReportDTO>();

                studentlist = (from f in _CumulativeReportContext.Adm_M_Student
                               from h in _CumulativeReportContext.School_Adm_Y_Student
                               from c in _CumulativeReportContext.AdmissionClass
                               from s in _CumulativeReportContext.School_M_Section
                               from y in _CumulativeReportContext.AcademicYear
                               where (h.AMST_Id == f.AMST_Id && h.ASMAY_Id == y.ASMAY_Id && h.ASMAY_Id == y.ASMAY_Id
                               && h.ASMCL_Id == c.ASMCL_Id && h.ASMS_Id == s.ASMS_Id && h.ASMAY_Id == y.ASMAY_Id
                               && h.ASMAY_Id == data.ASMAY_Id && h.ASMCL_Id == data.ASMCL_Id && h.ASMS_Id == data.ASMS_Id
                               && amstid.Contains(h.AMST_Id))
                               select new HHSAllReportDTO
                               {
                                   AMST_Id = h.AMST_Id,
                                   AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "" ? "" : f.AMST_FirstName) + 
                                   (f.AMST_MiddleName == null || f.AMST_MiddleName == "" || f.AMST_MiddleName == "0" ? "" : " " + f.AMST_MiddleName) + 
                                   (f.AMST_LastName == null || f.AMST_LastName == "" || f.AMST_LastName == "0" ? "" : " " + f.AMST_LastName)).Trim(),
                                   AMST_AdmNo = f.AMST_AdmNo,
                                   AMAY_RollNo = h.AMAY_RollNo,
                                   AMST_RegistrationNo = f.AMST_RegistrationNo

                               }).Distinct().ToList();

                var propertyInfo = typeof(HHSAllReportDTO).GetProperty(order);
                data.studlist = studentlist.Distinct().OrderBy(x => propertyInfo.GetValue(x, null)).ToArray();

                var exmrank = _CumulativeReportContext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id 
                && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && amstid.Contains(t.AMST_Id)).ToList();
                data.exmrank = exmrank.ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Subject_Subsubject_Subexam_Report";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",SqlDbType.VarChar){Value = data.EME_Id});
                    cmd.Parameters.Add(new SqlParameter("@Flag",SqlDbType.VarChar){Value = 1});

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
                                    dataRow.Add(dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.savelistnew = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Subject_Subsubject_Subexam_Report";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",SqlDbType.VarChar){Value = data.EME_Id});
                    cmd.Parameters.Add(new SqlParameter("@Flag",SqlDbType.VarChar){Value = 2});
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
                                    dataRow.Add(dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.subjectlistnew = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Subject_Subsubject_Subexam_Report";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",SqlDbType.VarChar){Value = data.EME_Id});
                    cmd.Parameters.Add(new SqlParameter("@Flag",SqlDbType.VarChar){Value = 3});

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
                                    dataRow.Add(dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.subjectlistwithdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HHSMIDFINALCumReportDTO ExamSubExamCumulativeReport(HHSMIDFINALCumReportDTO data)
        {
            try
            {
                data.configuration = _CumulativeReportContext.Exm_ConfigurationDMO.Where(t => t.MI_Id == data.MI_Id).ToArray();

                data.instname = _CumulativeReportContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                string order = "";
                var get_configuration = _CumulativeReportContext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
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

                List<long> amstid = new List<long>();

                if (data.AMST_Ids != null && data.AMST_Ids.Length > 0)
                {
                    foreach (var d in data.AMST_Ids)
                    {
                        amstid.Add(d);
                    }
                }
                else
                {
                    var getamstids = _CumulativeReportContext.School_Adm_Y_Student.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                    && a.ASMS_Id == data.ASMS_Id).ToList();

                    foreach (var d in getamstids)
                    {
                        amstid.Add(d.AMST_Id);
                    }
                }

                List<HHSAllReportDTO> studentlist = new List<HHSAllReportDTO>();

                studentlist = (from f in _CumulativeReportContext.Adm_M_Student
                               from h in _CumulativeReportContext.School_Adm_Y_Student
                               from c in _CumulativeReportContext.AdmissionClass
                               from s in _CumulativeReportContext.School_M_Section
                               from y in _CumulativeReportContext.AcademicYear

                               where (h.AMST_Id == f.AMST_Id && h.ASMAY_Id == y.ASMAY_Id && h.ASMAY_Id == y.ASMAY_Id
                               && h.ASMCL_Id == c.ASMCL_Id && h.ASMS_Id == s.ASMS_Id && h.ASMAY_Id == y.ASMAY_Id
                               && h.ASMAY_Id == data.ASMAY_Id && h.ASMCL_Id == data.ASMCL_Id && h.ASMS_Id == data.ASMS_Id
                               && amstid.Contains(h.AMST_Id))
                               select new HHSAllReportDTO
                               {
                                   AMST_Id = h.AMST_Id,
                                   AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "" ? "" : f.AMST_FirstName) + 
                                   (f.AMST_MiddleName == null || f.AMST_MiddleName == "" || f.AMST_MiddleName == "0" ? "" : " " + f.AMST_MiddleName) + 
                                   (f.AMST_LastName == null || f.AMST_LastName == "" || f.AMST_LastName == "0" ? "" : " " + f.AMST_LastName)).Trim(),
                                   AMST_AdmNo = f.AMST_AdmNo,
                                   AMAY_RollNo = h.AMAY_RollNo,
                                   AMST_RegistrationNo = f.AMST_RegistrationNo

                               }).Distinct().ToList();

                var propertyInfo = typeof(HHSAllReportDTO).GetProperty(order);
                data.studlist = studentlist.Distinct().OrderBy(x => propertyInfo.GetValue(x, null)).ToArray();

                List<HHSMIDFINALCumReportDTO> result = new List<HHSMIDFINALCumReportDTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_SubExam_Cumulative_Report";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.BigInt){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.BigInt){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",SqlDbType.BigInt){Value = data.EME_Id});
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new HHSMIDFINALCumReportDTO
                                {
                                    AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                    AMST_FirstName = ((dataReader["stname"].ToString() == null ? " " : dataReader["stname"].ToString())).Trim(),
                                    AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString() == null || dataReader["AMST_AdmNo"].ToString() == "" ? "" : dataReader["AMST_AdmNo"].ToString()),
                                    AMAY_RollNo = Convert.ToInt32(dataReader["AMAY_RollNo"].ToString() == null || dataReader["AMAY_RollNo"].ToString() == "" ? "" : dataReader["AMAY_RollNo"].ToString()),
                                    AMST_RegistrationNo = Convert.ToString(dataReader["AMST_RegistrationNo"].ToString() == null || dataReader["AMST_RegistrationNo"].ToString() == "" ? "" : dataReader["AMST_RegistrationNo"].ToString()),
                                    ISMS_Id = Convert.ToInt32(dataReader["ISMS_Id"].ToString()),
                                    EMSE_Id = Convert.ToInt32(dataReader["EMSE_Id"].ToString()),
                                    EMSE_SubExamName = (dataReader["EMSE_SubExamName"].ToString() == null || dataReader["EMSE_SubExamName"].ToString() == "" ? "" : dataReader["EMSE_SubExamName"].ToString()),

                                    ESTMPS_ObtainedMarks = Convert.ToDecimal(dataReader["ESTMPS_ObtainedMarks"].ToString() == null || dataReader["ESTMPS_ObtainedMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_ObtainedMarks"].ToString()),
                                    ESTMPS_MaxMarks = Convert.ToDecimal(dataReader["ESTMPS_MaxMarks"].ToString() == null || dataReader["ESTMPS_MaxMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_MaxMarks"].ToString()),
                                    ESTMPS_PassFailFlg = (dataReader["ESTMPS_PassFailFlg"].ToString() == null || dataReader["ESTMPS_PassFailFlg"].ToString() == "" ? "" : dataReader["ESTMPS_PassFailFlg"].ToString()),
                                    ESTMPS_ObtainedGrade = (dataReader["ESTMPS_ObtainedGrade"].ToString() == null || dataReader["ESTMPS_ObtainedGrade"].ToString() == "" ? "" : dataReader["ESTMPS_ObtainedGrade"].ToString()),
                                });
                                data.savelist = result.Distinct().OrderBy(t => t.ISMS_Id).ThenBy(t => t.EMSE_Id).ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }
                
                List<HHSMIDFINALCumReportDTO> result1 = new List<HHSMIDFINALCumReportDTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_SubExam_HHS_MID_FINAL_SUB_SUBEXAM";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.BigInt){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.BigInt){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",SqlDbType.BigInt){Value = data.EME_Id});
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result1.Add(new HHSMIDFINALCumReportDTO
                                {
                                    ISMS_Id = Convert.ToInt32(dataReader["ISMS_Id"].ToString()),
                                    ISMS_SubjectName = (dataReader["ISMS_SubjectName"].ToString() == null || dataReader["ISMS_SubjectName"].ToString() == "" ? "" : dataReader["ISMS_SubjectName"].ToString()),

                                    EMSE_Id = Convert.ToInt32(dataReader["EMSE_Id"].ToString()),
                                    EMSE_SubExamName = (dataReader["EMSE_SubExamName"].ToString() == null || dataReader["EMSE_SubExamName"].ToString() == "" ? "" : dataReader["EMSE_SubExamName"].ToString()),

                                    EYCES_AplResultFlg = Convert.ToBoolean(dataReader["EYCES_AplResultFlg"].ToString()),
                                    EYCES_MarksDisplayFlg = Convert.ToBoolean(dataReader["EYCES_MarksDisplayFlg"].ToString()),
                                    EYCES_GradeDisplayFlg = Convert.ToBoolean(dataReader["EYCES_GradeDisplayFlg"].ToString()),
                                    EYCESSS_MarksFlg = Convert.ToBoolean(dataReader["EYCESSS_MarksFlg"].ToString()),
                                    EYCESSS_GradesFlg = Convert.ToBoolean(dataReader["EYCESSS_GradesFlg"].ToString()),
                                });
                                data.subwithsubexm = result1.Distinct().ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }

                var exmrank = _CumulativeReportContext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id 
                && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && amstid.Contains(t.AMST_Id)).ToList();
                data.exmrank = exmrank.ToArray();

                var electivemarks1 = (from a in _CumulativeReportContext.Exm_Yearly_CategoryDMO
                                      from b in _CumulativeReportContext.Exm_Category_ClassDMO
                                      from c in _CumulativeReportContext.Exm_Yearly_Category_ExamsDMO
                                      from d in _CumulativeReportContext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                      from e in _CumulativeReportContext.ExmStudentMarksProcessSubjectwiseDMO
                                      from f in _CumulativeReportContext.IVRM_School_Master_SubjectsDMO
                                      where (a.MI_Id == b.MI_Id && a.MI_Id == e.MI_Id && f.ISMS_Id == e.ISMS_Id && a.MI_Id == data.MI_Id &&
                                      a.ASMAY_Id == b.ASMAY_Id && a.ASMAY_Id == e.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id &&
                                      a.EMCA_Id == b.EMCA_Id && a.EYC_Id == c.EYC_Id && d.EYCE_Id == c.EYCE_Id && d.EYCES_AplResultFlg == false 
                                      && d.ISMS_Id == e.ISMS_Id && b.ASMCL_Id == e.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && b.ASMCL_Id == data.ASMCL_Id 
                                      && b.ASMS_Id == data.ASMS_Id && e.EME_Id == data.EME_Id && c.EYCE_ActiveFlg == true
                                      && amstid.Contains(e.AMST_Id))
                                      select new HHSMIDFINALCumReportDTO
                                      {
                                          ISMS_SubjectName = f.ISMS_SubjectName,
                                          ESTMPS_ObtainedMarks = e.ESTMPS_ObtainedMarks,
                                          ESTMPS_ClassAverage = e.ESTMPS_ClassAverage,
                                          ESTMPS_ClassHighest = e.ESTMPS_ClassHighest,
                                          ESTMPS_SectionAverage = e.ESTMPS_SectionAverage,
                                          ESTMPS_SectionHighest = e.ESTMPS_SectionHighest,
                                          ESTMPS_PassFailFlg = e.ESTMPS_PassFailFlg,
                                          ESTMPS_MaxMarks = e.ESTMPS_MaxMarks,
                                          ESTMPS_ObtainedGrade = e.ESTMPS_ObtainedGrade,
                                          ESTMPS_Id = e.ESTMPS_Id,
                                          ISMS_Id = e.ISMS_Id,
                                          EME_Id = e.EME_Id,
                                          ASMAY_Id = e.ASMAY_Id,
                                          ASMCL_Id = e.ASMCL_Id,
                                          ASMS_Id = e.ASMS_Id,
                                          AMST_Id = e.AMST_Id,
                                          ISMS_OrderFlag = f.ISMS_OrderFlag
                                      }).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToList();
                data.electivemarks = electivemarks1.ToArray();
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