
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
    public class CumulativeReportImpl : Interfaces.CumulativeReportInterface
    {
        private static ConcurrentDictionary<string, CumulativeReportDTO> _login = new ConcurrentDictionary<string, CumulativeReportDTO>();
        private readonly ExamContext _CumulativeReportContext;
        ILogger<CumulativeReportImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public CumulativeReportImpl(ExamContext cpContext, DomainModelMsSqlServerContext db, ILogger<CumulativeReportImpl> acdimpl)
        {
            _CumulativeReportContext = cpContext;
            _db = db;
            _acdimpl = acdimpl;
        }
        public CumulativeReportDTO Getdetails(CumulativeReportDTO data)
        {
            CumulativeReportDTO getdata = new CumulativeReportDTO();
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
        public CumulativeReportDTO validateordernumber(CumulativeReportDTO data)
        {
            CumulativeReportDTO getdata = new CumulativeReportDTO();

            return getdata;
        }
        public async Task<CumulativeReportDTO> savedetails(CumulativeReportDTO data)
        {
            try
            {
                string order = "AMST_FirstName";
                var get_configuration = _CumulativeReportContext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                data.configuration = get_configuration.ToArray();

                if(get_configuration!=null && get_configuration.Count > 0)
                {
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
                }
                string amstids = "";
                string amst_id = "0";

                List<long> amstid = new List<long>();

                if (data.AMST_Ids != null && data.AMST_Ids.Length > 0)
                {
                    amstids = string.Join(",", data.AMST_Ids);

                    //amstids = string.Join(",", data.AMST_Ids);
                }
                else
                {
                    var getamstids = _CumulativeReportContext.School_Adm_Y_Student.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                    && a.ASMS_Id == data.ASMS_Id).ToList();

                    foreach (var d in getamstids)
                    {
                        amst_id = amst_id + "," + d.AMST_Id;
                    }

                    amstids = amst_id;
                }

                data.instname = _CumulativeReportContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                if (data.applicableflag == true)
                {
                    List<CumulativeReportDTO> result = new List<CumulativeReportDTO>();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "Exam_cumulative_BB_Report_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader.ReadAsync())
                                {
                                    result.Add(new CumulativeReportDTO
                                    {
                                        MI_name = (dataReader["MI_name"].ToString().Trim() == null || dataReader["MI_name"].ToString().Trim() == "" ? "" : dataReader["MI_name"].ToString()),

                                        ESTMPS_ObtainedMarks = Convert.ToDecimal(dataReader["ESTMPS_ObtainedMarks"].ToString() == null || dataReader["ESTMPS_ObtainedMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_ObtainedMarks"].ToString()),
                                        ESTMPS_ObtainedGrade = (dataReader["ESTMPS_ObtainedGrade"].ToString() == null || dataReader["ESTMPS_ObtainedGrade"].ToString() == "" ? "" : dataReader["ESTMPS_ObtainedGrade"].ToString()),
                                        ESTMPS_PassFailFlg = (dataReader["ESTMPS_PassFailFlg"].ToString() == null || dataReader["ESTMPS_PassFailFlg"].ToString() == "" ? "" : dataReader["ESTMPS_PassFailFlg"].ToString()),
                                        EME_ExamName = (dataReader["EME_ExamName"].ToString() == null || dataReader["EME_ExamName"].ToString() == "" ? "" : dataReader["EME_ExamName"].ToString()),
                                        ASMCL_ClassName = (dataReader["ASMCL_ClassName"].ToString() == null || dataReader["ASMCL_ClassName"].ToString() == "" ? "" : dataReader["ASMCL_ClassName"].ToString()),
                                        ASMC_SectionName = (dataReader["ASMC_SectionName"].ToString() == null || dataReader["ASMC_SectionName"].ToString() == "" ? "" : dataReader["ASMC_SectionName"].ToString()),
                                        AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                        AMST_FirstName = ((dataReader["AMST_FirstName"].ToString() == null || dataReader["AMST_FirstName"].ToString() == "" ? "" : dataReader["AMST_FirstName"].ToString()) + (dataReader["AMST_MiddleName"].ToString() == null || dataReader["AMST_MiddleName"].ToString() == "" ? "" : " " + dataReader["AMST_MiddleName"].ToString()) +
                                        (dataReader["AMST_LastName"].ToString() == null || dataReader["AMST_LastName"].ToString() == "" ? "" : " " + dataReader["AMST_LastName"].ToString())).Trim(),
                                        AMST_DOB = Convert.ToDateTime(dataReader["AMST_DOB"].ToString() == null || dataReader["AMST_DOB"].ToString() == "" ? "" : dataReader["AMST_DOB"].ToString()),
                                        AMAY_RollNo = Convert.ToInt32(dataReader["AMAY_RollNo"].ToString() == null || dataReader["AMAY_RollNo"].ToString() == "" ? "" : dataReader["AMAY_RollNo"].ToString()),
                                        AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString() == null || dataReader["AMST_AdmNo"].ToString() == "" ? "" : dataReader["AMST_AdmNo"].ToString()),
                                        AMST_RegistrationNo = (dataReader["AMST_RegistrationNo"].ToString() == null || dataReader["AMST_RegistrationNo"].ToString() == "" ? "" : dataReader["AMST_RegistrationNo"].ToString()),
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

                                        EMPATY_PaperTypeName = (dataReader["EMPATY_PaperTypeName"].ToString() == null || dataReader["EMPATY_PaperTypeName"].ToString() == "" ? "" : dataReader["EMPATY_PaperTypeName"].ToString()),

                                        EMPATY_Color = (dataReader["EMPATY_Color"].ToString() == null || dataReader["EMPATY_Color"].ToString() == "" ? "" : dataReader["EMPATY_Color"].ToString()),

                                    });
                                    var propertyInfo = typeof(CumulativeReportDTO).GetProperty(order);
                                    result = result.OrderBy(x => x.EYCES_SubjectOrder).ThenBy(x => propertyInfo.GetValue(x, null)).ToList();
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
                        data.subjlist = data.savelist.Distinct<CumulativeReportDTO>(new CumulativeEqualityComparer()).ToArray();
                    }
                }

                if (data.nonapplicableflag == true)
                {
                    List<CumulativeReportDTO> result1 = new List<CumulativeReportDTO>();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "Exam_cumulative_BB_Report_Elective_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader.ReadAsync())
                                {
                                    result1.Add(new CumulativeReportDTO
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
                                        AMST_RegistrationNo = (dataReader["AMST_RegistrationNo"].ToString() == null || dataReader["AMST_RegistrationNo"].ToString() == "" ? "" : dataReader["AMST_RegistrationNo"].ToString()),
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

                                        EMPATY_PaperTypeName = (dataReader["EMPATY_PaperTypeName"].ToString() == null || dataReader["EMPATY_PaperTypeName"].ToString() == "" ? "" : dataReader["EMPATY_PaperTypeName"].ToString()),

                                        EMPATY_Color = (dataReader["EMPATY_Color"].ToString() == null || dataReader["EMPATY_Color"].ToString() == "" ? "" : dataReader["EMPATY_Color"].ToString()),

                                    });
                                    var propertyInfo = typeof(CumulativeReportDTO).GetProperty(order);
                                    result1 = result1.OrderBy(x => x.EYCES_SubjectOrder).ThenBy(x => propertyInfo.GetValue(x, null)).ToList();
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
                        data.nonsubjlist = data.savenonsubjlist.Distinct<CumulativeReportDTO>(new CumulativeEqualityComparer()).ToArray();
                    }
                }
                #region comment
                //else
                //{
                //    List<CumulativeReportDTO> result = new List<CumulativeReportDTO>();                   
                //    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                //    {
                //        cmd.CommandTimeout = 0;
                //        cmd.CommandText = "Exam_cumulative_BB_Report";
                //        cmd.CommandType = CommandType.StoredProcedure;

                //        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar){Value = Convert.ToInt32(data.ASMAY_Id)});
                //        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar){Value = Convert.ToInt32(data.ASMCL_Id)});
                //        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar){Value = Convert.ToInt32(data.ASMS_Id)});
                //        cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar){Value = Convert.ToInt32(data.MI_Id)});
                //        cmd.Parameters.Add(new SqlParameter("@EME_Id",SqlDbType.VarChar){Value = Convert.ToInt32(data.EME_Id)});
                //        if (cmd.Connection.State != ConnectionState.Open)
                //            cmd.Connection.Open();

                //        var retObject = new List<dynamic>();

                //        try
                //        {                           
                //            using (var dataReader = await cmd.ExecuteReaderAsync())
                //            {
                //                while (await dataReader.ReadAsync())
                //                {
                //                    result.Add(new CumulativeReportDTO
                //                    {
                //                        MI_name = (dataReader["MI_name"].ToString().Trim() == null || dataReader["MI_name"].ToString().Trim() == "" ? "" : dataReader["MI_name"].ToString()),

                //                        ESTMPS_ObtainedMarks = Convert.ToDecimal(dataReader["ESTMPS_ObtainedMarks"].ToString() == null || dataReader["ESTMPS_ObtainedMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_ObtainedMarks"].ToString()),
                //                        ESTMPS_ObtainedGrade = (dataReader["ESTMPS_ObtainedGrade"].ToString() == null || dataReader["ESTMPS_ObtainedGrade"].ToString() == "" ? "" : dataReader["ESTMPS_ObtainedGrade"].ToString()),
                //                        ESTMPS_PassFailFlg = (dataReader["ESTMPS_PassFailFlg"].ToString() == null || dataReader["ESTMPS_PassFailFlg"].ToString() == "" ? "" : dataReader["ESTMPS_PassFailFlg"].ToString()),
                //                        EME_ExamName = (dataReader["EME_ExamName"].ToString() == null || dataReader["EME_ExamName"].ToString() == "" ? "" : dataReader["EME_ExamName"].ToString()),
                //                        ASMCL_ClassName = (dataReader["ASMCL_ClassName"].ToString() == null || dataReader["ASMCL_ClassName"].ToString() == "" ? "" : dataReader["ASMCL_ClassName"].ToString()),
                //                        ASMC_SectionName = (dataReader["ASMC_SectionName"].ToString() == null || dataReader["ASMC_SectionName"].ToString() == "" ? "" : dataReader["ASMC_SectionName"].ToString()),
                //                        AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                //                        AMST_FirstName = ((dataReader["AMST_FirstName"].ToString() == null || dataReader["AMST_FirstName"].ToString() == "" ? "" : dataReader["AMST_FirstName"].ToString()) + (dataReader["AMST_MiddleName"].ToString() == null || dataReader["AMST_MiddleName"].ToString() == "" ? "" : " " + dataReader["AMST_MiddleName"].ToString()) +
                //                        (dataReader["AMST_LastName"].ToString() == null || dataReader["AMST_LastName"].ToString() == "" ? "" : " " + dataReader["AMST_LastName"].ToString())).Trim(),
                //                        AMST_DOB = Convert.ToDateTime(dataReader["AMST_DOB"].ToString() == null || dataReader["AMST_DOB"].ToString() == "" ? "" : dataReader["AMST_DOB"].ToString()),
                //                        AMAY_RollNo = Convert.ToInt32(dataReader["AMAY_RollNo"].ToString() == null || dataReader["AMAY_RollNo"].ToString() == "" ? "" : dataReader["AMAY_RollNo"].ToString()),
                //                        AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString() == null || dataReader["AMST_AdmNo"].ToString() == "" ? "" : dataReader["AMST_AdmNo"].ToString()),
                //                        AMST_RegistrationNo = (dataReader["AMST_RegistrationNo"].ToString() == null || dataReader["AMST_RegistrationNo"].ToString() == "" ? "" : dataReader["AMST_RegistrationNo"].ToString()),
                //                        ISMS_Id = Convert.ToInt32(dataReader["ISMS_Id"].ToString()),
                //                        ISMS_SubjectName = (dataReader["ISMS_SubjectName"].ToString() == null || dataReader["ISMS_SubjectName"].ToString() == "" ? "" : dataReader["ISMS_SubjectName"].ToString()),
                //                        ESTMPS_MaxMarks = Convert.ToDecimal(dataReader["ESTMPS_MaxMarks"].ToString() == null || dataReader["ESTMPS_MaxMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_MaxMarks"].ToString()),
                //                        ESTMPS_ClassAverage = Convert.ToDecimal(dataReader["ESTMPS_ClassAverage"].ToString() == null || dataReader["ESTMPS_ClassAverage"].ToString() == "" ? "0" : dataReader["ESTMPS_ClassAverage"].ToString()),
                //                        ESTMPS_SectionAverage = Convert.ToDecimal(dataReader["ESTMPS_SectionAverage"].ToString() == null || dataReader["ESTMPS_SectionAverage"].ToString() == "" ? "0" : dataReader["ESTMPS_SectionAverage"].ToString()),
                //                        ESTMPS_ClassHighest = Convert.ToDecimal(dataReader["ESTMPS_ClassHighest"].ToString() == null || dataReader["ESTMPS_ClassHighest"].ToString() == "" ? "0" : dataReader["ESTMPS_ClassHighest"].ToString()),
                //                        ESTMPS_SectionHighest = Convert.ToDecimal(dataReader["ESTMPS_SectionHighest"].ToString() == null || dataReader["ESTMPS_SectionHighest"].ToString() == "" ? "0" : dataReader["ESTMPS_SectionHighest"].ToString()),
                //                        ISMS_SubjectCode = (dataReader["ISMS_SubjectCode"].ToString() == null || dataReader["ISMS_SubjectCode"].ToString() == "" ? "" : dataReader["ISMS_SubjectCode"].ToString()),
                //                        EYCES_AplResultFlg = Convert.ToBoolean(dataReader["EYCES_AplResultFlg"].ToString()),
                //                        EYCES_MaxMarks = Convert.ToDecimal(dataReader["EYCES_MaxMarks"].ToString() == null || dataReader["EYCES_MaxMarks"].ToString() == "" ? "0" : dataReader["EYCES_MaxMarks"].ToString()),
                //                        EYCES_MinMarks = Convert.ToDecimal(dataReader["EYCES_MinMarks"].ToString() == null || dataReader["EYCES_MinMarks"].ToString() == "" ? "0" : dataReader["EYCES_MinMarks"].ToString()),
                //                        EMGR_Id = Convert.ToInt32(dataReader["EMGR_Id"].ToString()),
                //                        classheld = Convert.ToDecimal(dataReader["ASA_ClassHeld"].ToString() == null || dataReader["ASA_ClassHeld"].ToString() == "" ? "0" : dataReader["ASA_ClassHeld"].ToString()),
                //                        classatt = Convert.ToDecimal(dataReader["ASA_Class_Attended"].ToString() == null || dataReader["ASA_Class_Attended"].ToString() == "" ? "0" : dataReader["ASA_Class_Attended"].ToString()),
                //                        graderemark = (dataReader["EMGD_Remarks"].ToString() == null || dataReader["EMGD_Remarks"].ToString() == "" ? "0" : dataReader["EMGD_Remarks"].ToString()),

                //                        ESTMP_TotalObtMarks = Convert.ToDecimal(dataReader["ESTMP_TotalObtMarks"].ToString() == null || dataReader["ESTMP_TotalObtMarks"].ToString() == "" ? "0" : dataReader["ESTMP_TotalObtMarks"].ToString()),
                //                        ESTMP_Percentage = Convert.ToDecimal(dataReader["ESTMP_Percentage"].ToString() == null || dataReader["ESTMP_Percentage"].ToString() == "" ? "0" : dataReader["ESTMP_Percentage"].ToString()),
                //                        ESTMP_TotalGrade = (dataReader["ESTMP_TotalGrade"].ToString() == null || dataReader["ESTMP_TotalGrade"].ToString() == "" ? "" : dataReader["ESTMP_TotalGrade"].ToString()),
                //                        ESTMP_ClassRank = Convert.ToInt16(dataReader["ESTMP_ClassRank"].ToString() == null || dataReader["ESTMP_ClassRank"].ToString() == "" ? "" : dataReader["ESTMP_ClassRank"].ToString()),
                //                        ESTMP_SectionRank = Convert.ToInt16(dataReader["ESTMP_SectionRank"].ToString() == null || dataReader["ESTMP_SectionRank"].ToString() == "" ? "" : dataReader["ESTMP_SectionRank"].ToString()),
                //                        ESTMP_TotalGradeRemark = (dataReader["ESTMP_TotalGradeRemark"].ToString() == null || dataReader["ESTMP_TotalGradeRemark"].ToString() == "" ? "" : dataReader["ESTMP_TotalGradeRemark"].ToString()),
                //                        ESTMP_TotalMaxMarks = Convert.ToDecimal(dataReader["ESTMP_TotalMaxMarks"].ToString() == null || dataReader["ESTMP_TotalMaxMarks"].ToString() == "0" ? "" : dataReader["ESTMP_TotalMaxMarks"].ToString()),
                //                        EYCES_SubjectOrder = Convert.ToInt16(dataReader["EYCES_SubjectOrder"].ToString() == null || dataReader["EYCES_SubjectOrder"].ToString() == "" ? "" : dataReader["EYCES_SubjectOrder"].ToString()),
                //                        //praveen
                //                        ESTMP_Result = (dataReader["ESTMP_Result"].ToString() == null || dataReader["ESTMP_Result"].ToString() == "" ? "" : dataReader["ESTMP_Result"].ToString()),

                //                    });

                //                    var propertyInfo = typeof(CumulativeReportDTO).GetProperty(order);
                //                    result = result.OrderBy(x => x.EYCES_SubjectOrder).ThenBy(x => propertyInfo.GetValue(x, null)).ToList();
                //                    data.savelist = result.Distinct().ToList();
                //                }
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            _acdimpl.LogInformation("Cumulative Report Procedure out 1 data.savelist.Count()");
                //            _acdimpl.LogError(ex.Message);
                //            _acdimpl.LogDebug(ex.Message);
                //        }
                //    }

                //    List<long> subs = new List<long>();

                //    if (data.savelist != null && data.savelist.Count > 0)
                //    {
                //        data.subjlist = data.savelist.Distinct<CumulativeReportDTO>(new CumulativeEqualityComparer()).ToArray();
                //    }

                //    List<CumulativeReportDTO> result1 = new List<CumulativeReportDTO>();                   

                //    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                //    {
                //        cmd.CommandTimeout = 0;
                //        cmd.CommandText = "Exam_cumulative_BB_Report_Elective";
                //        cmd.CommandType = CommandType.StoredProcedure;

                //        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar){Value = Convert.ToInt32(data.ASMAY_Id)});
                //        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar){Value = Convert.ToInt32(data.ASMCL_Id)});
                //        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar){Value = Convert.ToInt32(data.ASMS_Id)});
                //        cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar){Value = Convert.ToInt32(data.MI_Id)});
                //        cmd.Parameters.Add(new SqlParameter("@EME_Id",SqlDbType.VarChar){Value = Convert.ToInt32(data.EME_Id)});

                //        if (cmd.Connection.State != ConnectionState.Open)
                //            cmd.Connection.Open();

                //        var retObject = new List<dynamic>();

                //        try
                //        {                           
                //            using (var dataReader = await cmd.ExecuteReaderAsync())
                //            {
                //                while (await dataReader.ReadAsync())
                //                {
                //                    result1.Add(new CumulativeReportDTO
                //                    {
                //                        MI_name = (dataReader["MI_name"].ToString().Trim() == null || dataReader["MI_name"].ToString().Trim() == "" ? "" : dataReader["MI_name"].ToString()),

                //                        ESTMPS_ObtainedMarks = Convert.ToDecimal(dataReader["ESTMPS_ObtainedMarks"].ToString() == null || dataReader["ESTMPS_ObtainedMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_ObtainedMarks"].ToString()),
                //                        ESTMPS_ObtainedGrade = (dataReader["ESTMPS_ObtainedGrade"].ToString() == null || dataReader["ESTMPS_ObtainedGrade"].ToString() == "" ? "" : dataReader["ESTMPS_ObtainedGrade"].ToString()),
                //                        ESTMPS_PassFailFlg = (dataReader["ESTMPS_PassFailFlg"].ToString() == null || dataReader["ESTMPS_PassFailFlg"].ToString() == "" ? "" : dataReader["ESTMPS_PassFailFlg"].ToString()),
                //                        EME_ExamName = (dataReader["EME_ExamName"].ToString() == null || dataReader["EME_ExamName"].ToString() == "" ? "" : dataReader["EME_ExamName"].ToString()),
                //                        ASMCL_ClassName = (dataReader["ASMCL_ClassName"].ToString() == null || dataReader["ASMCL_ClassName"].ToString() == "" ? "" : dataReader["ASMCL_ClassName"].ToString()),
                //                        ASMC_SectionName = (dataReader["ASMC_SectionName"].ToString() == null || dataReader["ASMC_SectionName"].ToString() == "" ? "" : dataReader["ASMC_SectionName"].ToString()),
                //                        AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                //                        AMST_FirstName = ((dataReader["AMST_FirstName"].ToString() == null ? " " : dataReader["AMST_FirstName"].ToString()) + " " + (dataReader["AMST_MiddleName"].ToString() == null ? " " : dataReader["AMST_MiddleName"].ToString()) + " " + (dataReader["AMST_LastName"].ToString() == null ? " " : dataReader["AMST_LastName"].ToString())).Trim(),
                //                        AMST_DOB = Convert.ToDateTime(dataReader["AMST_DOB"].ToString() == null || dataReader["AMST_DOB"].ToString() == "" ? "" : dataReader["AMST_DOB"].ToString()),
                //                        AMAY_RollNo = Convert.ToInt32(dataReader["AMAY_RollNo"].ToString() == null || dataReader["AMAY_RollNo"].ToString() == "" ? "" : dataReader["AMAY_RollNo"].ToString()),
                //                        AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString() == null || dataReader["AMST_AdmNo"].ToString() == "" ? "" : dataReader["AMST_AdmNo"].ToString()),
                //                        AMST_RegistrationNo = (dataReader["AMST_RegistrationNo"].ToString() == null || dataReader["AMST_RegistrationNo"].ToString() == "" ? "" : dataReader["AMST_RegistrationNo"].ToString()),
                //                        ISMS_Id = Convert.ToInt32(dataReader["ISMS_Id"].ToString()),
                //                        ISMS_SubjectName = (dataReader["ISMS_SubjectName"].ToString() == null || dataReader["ISMS_SubjectName"].ToString() == "" ? "" : dataReader["ISMS_SubjectName"].ToString()),
                //                        ESTMPS_MaxMarks = Convert.ToDecimal(dataReader["ESTMPS_MaxMarks"].ToString() == null || dataReader["ESTMPS_MaxMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_MaxMarks"].ToString()),
                //                        ESTMPS_ClassAverage = Convert.ToDecimal(dataReader["ESTMPS_ClassAverage"].ToString() == null || dataReader["ESTMPS_ClassAverage"].ToString() == "" ? "0" : dataReader["ESTMPS_ClassAverage"].ToString()),
                //                        ESTMPS_SectionAverage = Convert.ToDecimal(dataReader["ESTMPS_SectionAverage"].ToString() == null || dataReader["ESTMPS_SectionAverage"].ToString() == "" ? "0" : dataReader["ESTMPS_SectionAverage"].ToString()),
                //                        ESTMPS_ClassHighest = Convert.ToDecimal(dataReader["ESTMPS_ClassHighest"].ToString() == null || dataReader["ESTMPS_ClassHighest"].ToString() == "" ? "0" : dataReader["ESTMPS_ClassHighest"].ToString()),
                //                        ESTMPS_SectionHighest = Convert.ToDecimal(dataReader["ESTMPS_SectionHighest"].ToString() == null || dataReader["ESTMPS_SectionHighest"].ToString() == "" ? "0" : dataReader["ESTMPS_SectionHighest"].ToString()),
                //                        ISMS_SubjectCode = (dataReader["ISMS_SubjectCode"].ToString() == null || dataReader["ISMS_SubjectCode"].ToString() == "" ? "" : dataReader["ISMS_SubjectCode"].ToString()),
                //                        EYCES_AplResultFlg = Convert.ToBoolean(dataReader["EYCES_AplResultFlg"].ToString()),
                //                        EYCES_MaxMarks = Convert.ToDecimal(dataReader["EYCES_MaxMarks"].ToString() == null || dataReader["EYCES_MaxMarks"].ToString() == "" ? "0" : dataReader["EYCES_MaxMarks"].ToString()),
                //                        EYCES_MinMarks = Convert.ToDecimal(dataReader["EYCES_MinMarks"].ToString() == null || dataReader["EYCES_MinMarks"].ToString() == "" ? "0" : dataReader["EYCES_MinMarks"].ToString()),
                //                        EMGR_Id = Convert.ToInt32(dataReader["EMGR_Id"].ToString()),
                //                        classheld = Convert.ToDecimal(dataReader["ASA_ClassHeld"].ToString() == null || dataReader["ASA_ClassHeld"].ToString() == "" ? "0" : dataReader["ASA_ClassHeld"].ToString()),
                //                        classatt = Convert.ToDecimal(dataReader["ASA_Class_Attended"].ToString() == null || dataReader["ASA_Class_Attended"].ToString() == "" ? "0" : dataReader["ASA_Class_Attended"].ToString()),
                //                        graderemark = (dataReader["EMGD_Remarks"].ToString() == null || dataReader["EMGD_Remarks"].ToString() == "" ? "0" : dataReader["EMGD_Remarks"].ToString()),

                //                        ESTMP_TotalObtMarks = Convert.ToDecimal(dataReader["ESTMP_TotalObtMarks"].ToString() == null || dataReader["ESTMP_TotalObtMarks"].ToString() == "" ? "0" : dataReader["ESTMP_TotalObtMarks"].ToString()),
                //                        ESTMP_Percentage = Convert.ToDecimal(dataReader["ESTMP_Percentage"].ToString() == null || dataReader["ESTMP_Percentage"].ToString() == "" ? "0" : dataReader["ESTMP_Percentage"].ToString()),
                //                        ESTMP_TotalGrade = (dataReader["ESTMP_TotalGrade"].ToString() == null || dataReader["ESTMP_TotalGrade"].ToString() == "" ? "" : dataReader["ESTMP_TotalGrade"].ToString()),
                //                        ESTMP_ClassRank = Convert.ToInt16(dataReader["ESTMP_ClassRank"].ToString() == null || dataReader["ESTMP_ClassRank"].ToString() == "" ? "" : dataReader["ESTMP_ClassRank"].ToString()),
                //                        ESTMP_SectionRank = Convert.ToInt16(dataReader["ESTMP_SectionRank"].ToString() == null || dataReader["ESTMP_SectionRank"].ToString() == "" ? "" : dataReader["ESTMP_SectionRank"].ToString()),
                //                        ESTMP_TotalGradeRemark = (dataReader["ESTMP_TotalGradeRemark"].ToString() == null || dataReader["ESTMP_TotalGradeRemark"].ToString() == "" ? "" : dataReader["ESTMP_TotalGradeRemark"].ToString()),
                //                        ESTMP_TotalMaxMarks = Convert.ToDecimal(dataReader["ESTMP_TotalMaxMarks"].ToString() == null || dataReader["ESTMP_TotalMaxMarks"].ToString() == "0" ? "" : dataReader["ESTMP_TotalMaxMarks"].ToString()),
                //                        EYCES_SubjectOrder = Convert.ToInt16(dataReader["EYCES_SubjectOrder"].ToString() == null || dataReader["EYCES_SubjectOrder"].ToString() == "" ? "" : dataReader["EYCES_SubjectOrder"].ToString())

                //                    });
                //                    var propertyInfo = typeof(CumulativeReportDTO).GetProperty(order);
                //                    result1 = result1.OrderBy(x => x.EYCES_SubjectOrder).ThenBy(x => propertyInfo.GetValue(x, null)).ToList();                                   
                //                    data.savenonsubjlist = result1.Distinct().ToList();
                //                }
                //            }
                //        }

                //        catch (Exception ex)
                //        {
                //            _acdimpl.LogInformation("Cumulative Report Procedure 2-5 data.savenonsubjlist");
                //            _acdimpl.LogError(ex.Message);
                //            _acdimpl.LogDebug(ex.Message);
                //        }
                //    }

                //    List<long> nonsubs = new List<long>();
                //    if (data.savenonsubjlist != null && data.savenonsubjlist.Count > 0)
                //    {
                //        data.nonsubjlist = data.savenonsubjlist.Distinct<CumulativeReportDTO>(new CumulativeEqualityComparer()).ToArray();
                //    }

                //}    

                #endregion

                var EMCA_Id = _CumulativeReportContext.Exm_Category_ClassDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).EMCA_Id;

                var EYC_Id = _CumulativeReportContext.Exm_Yearly_CategoryDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EMCA_Id == EMCA_Id && t.EYC_ActiveFlg == true).EYC_Id;

                var EYCE_Id = _CumulativeReportContext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == EYC_Id && t.EYCE_ActiveFlg == true && t.EME_Id == data.EME_Id).Select(t => t.EYCE_Id).Distinct().ToList();

                var Exam_Subwise_Details = _CumulativeReportContext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => EYCE_Id.Contains(t.EYCE_Id) && t.EYCES_ActiveFlg == true).Distinct().OrderBy(t => t.EYCE_Id).ThenBy(t => t.EYCES_SubjectOrder).ToList();
                data.examsubjectwise_details = Exam_Subwise_Details.ToArray();                

                data.studenwise_remarks = _CumulativeReportContext.Exm_ProgressCard_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_ID == data.EME_Id && a.EMER_ActiveFlag == true).ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Cumulative Report Completed Procedure Final catch : ");
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }

            return data;
        }
        public CumulativeReportDTO editdetails(int ID)
        {
            CumulativeReportDTO editlt = new CumulativeReportDTO();
            return editlt;
        }
        public CumulativeReportDTO deactivate(CumulativeReportDTO data)
        {
            CumulativeReportDTO deact = new CumulativeReportDTO();
            return deact;
        }
        public CumulativeReportDTO onchangeyear(CumulativeReportDTO data)
        {
            try
            {
                data.classlist = (from a in _CumulativeReportContext.AcademicYear
                                  from b in _CumulativeReportContext.Exm_Category_ClassDMO
                                  from c in _CumulativeReportContext.AdmissionClass
                                  where (a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && a.MI_Id == data.MI_Id && a.Is_Active == true && b.ASMAY_Id == data.ASMAY_Id && b.ECAC_ActiveFlag == true && b.MI_Id == data.MI_Id)
                                  select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public CumulativeReportDTO onchangeclass(CumulativeReportDTO data)
        {
            try
            {
                data.seclist = (from a in _CumulativeReportContext.AcademicYear
                                from b in _CumulativeReportContext.Exm_Category_ClassDMO
                                from c in _CumulativeReportContext.AdmissionClass
                                from d in _CumulativeReportContext.School_M_Section
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
        public CumulativeReportDTO onchangesection(CumulativeReportDTO data)
        {
            try
            {
                data.exmstdlist = (from a in _CumulativeReportContext.Exm_Master_CategoryDMO
                                   from b in _CumulativeReportContext.Exm_Category_ClassDMO
                                   from c in _CumulativeReportContext.Exm_Yearly_CategoryDMO
                                   from d in _CumulativeReportContext.Exm_Yearly_Category_ExamsDMO
                                   from e in _CumulativeReportContext.AcademicYear
                                   from f in _CumulativeReportContext.AdmissionClass
                                   from g in _CumulativeReportContext.School_M_Section
                                   from h in _CumulativeReportContext.masterexam
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
    }
    class CumulativeEqualityComparer : IEqualityComparer<CumulativeReportDTO>
    {
        public bool Equals(CumulativeReportDTO b1, CumulativeReportDTO b2)
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

        public int GetHashCode(CumulativeReportDTO bx)
        {
            int hCode = Convert.ToInt32(bx.ISMS_Id);
            return hCode.GetHashCode();
        }
    }
}