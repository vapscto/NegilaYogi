

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
    public class BaldwinAllReportImpl : Interfaces.BaldwinAllReportInterface
    {
        private static ConcurrentDictionary<string, BaldwinAllReportDTO> _login =
         new ConcurrentDictionary<string, BaldwinAllReportDTO>();

        private readonly ExamContext _BaldwinAllReportContext;
        ILogger<BaldwinAllReportImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public BaldwinAllReportImpl(ExamContext cpContext, DomainModelMsSqlServerContext db, ILogger<BaldwinAllReportImpl> acdimpl)
        {
            _BaldwinAllReportContext = cpContext;
            _db = db;
            _acdimpl = acdimpl;
        }
        public async Task<BaldwinAllReportDTO> Getdetails(BaldwinAllReportDTO data)//int IVRMM_Id
        {
            BaldwinAllReportDTO getdata = new BaldwinAllReportDTO();
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = await _BaldwinAllReportContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToListAsync();
                getdata.yearlist = list.ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = await _BaldwinAllReportContext.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).OrderBy(t => t.ASMC_Order).ToListAsync();
                getdata.seclist = seclist.ToArray();

                List<AdmissionClass> admlist = new List<AdmissionClass>();
                admlist = await _BaldwinAllReportContext.AdmissionClass.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).OrderBy(t => t.ASMCL_Order).ToListAsync();
                getdata.classlist = admlist.ToArray();

                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = await _BaldwinAllReportContext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).OrderBy(t => t.EME_ExamOrder).ToListAsync();

                getdata.exmstdlist = esmp.ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;

        }
        public async Task<BaldwinAllReportDTO> savedetails(BaldwinAllReportDTO data)
        {
            try
            {
                data.instname = _BaldwinAllReportContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                data.clstchname = (from a in _BaldwinAllReportContext.ClassTeacherMappingDMO
                                   from b in _BaldwinAllReportContext.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new BaldwinAllReportDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                List<BaldwinAllReportDTO> result = new List<BaldwinAllReportDTO>();
                string AMST_Id = "0";
                if (data.selectedclass !=null  && data.selectedclass.Length > 0)
                {
                    foreach(var d  in data.selectedclass)
                    {

                        AMST_Id = AMST_Id + ',' + d.yearid;
                    }
                }
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_get_BB_Exam_Details_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.BigInt){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.BigInt){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",SqlDbType.BigInt){Value = data.EME_Id});
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = AMST_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                result.Add(new BaldwinAllReportDTO
                                {
                                    ESTMPS_ObtainedMarks = dataReader["ESTMPS_ObtainedMarks"].ToString() == null || dataReader["ESTMPS_ObtainedMarks"].ToString() == "" ? (decimal?)null : Convert.ToDecimal(dataReader["ESTMPS_ObtainedMarks"].ToString()),
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
                                    ESTMPSSS_ObtainedMarks = Convert.ToDecimal(dataReader["stu_grandmin_marks"].ToString() == null || dataReader["stu_grandmin_marks"].ToString() == "0" ? "" : dataReader["stu_grandmin_marks"].ToString()),
                                    EYCES_SubjectOrder = Convert.ToInt16(dataReader["EYCES_SubjectOrder"].ToString() == null || dataReader["EYCES_SubjectOrder"].ToString() == "" ? "" : dataReader["EYCES_SubjectOrder"].ToString()),
                                    EYCES_MarksDisplayFlg = Convert.ToBoolean(dataReader["EYCES_MarksDisplayFlg"].ToString()),
                                    EYCES_GradeDisplayFlg = Convert.ToBoolean(dataReader["EYCES_GradeDisplayFlg"].ToString()),
                                    ESTMP_Result = (dataReader["ESTMP_Result"].ToString() == null || dataReader["ESTMP_Result"].ToString() == "" ? "" : dataReader["ESTMP_Result"].ToString()),
                                    UserName = (dataReader["SPCCMH_HouseName"].ToString() == null || dataReader["SPCCMH_HouseName"].ToString() == "" ? "" : dataReader["SPCCMH_HouseName"].ToString())

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

                var from_date = (from a in _BaldwinAllReportContext.Exm_Category_ClassDMO
                                 from b in _BaldwinAllReportContext.Exm_Yearly_CategoryDMO
                                 from c in _BaldwinAllReportContext.Exm_Yearly_Category_ExamsDMO
                                 where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id && b.ASMAY_Id == data.ASMAY_Id)
                                 select c.EYCE_AttendanceFromDate).FirstOrDefault();

                var to_date = (from a in _BaldwinAllReportContext.Exm_Category_ClassDMO
                               from b in _BaldwinAllReportContext.Exm_Yearly_CategoryDMO
                               from c in _BaldwinAllReportContext.Exm_Yearly_Category_ExamsDMO
                               where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id && b.ASMAY_Id == data.ASMAY_Id)
                               select c.EYCE_AttendanceToDate).Max();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "StudentAttendance_W";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.BigInt){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.BigInt){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@from",SqlDbType.Date){Value = from_date});
                    cmd.Parameters.Add(new SqlParameter("@to",SqlDbType.Date){Value = to_date});
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
                                    dataRow1.Add(dataReader.GetName(iFiled1),dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
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
                    cmd.CommandTimeout = 450000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.BigInt){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.BigInt){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@from",SqlDbType.Date){Value = from_date});
                    cmd.Parameters.Add(new SqlParameter("@to",SqlDbType.Date){Value = to_date});
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
                                    dataRow1.Add(dataReader.GetName(iFiled1),dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
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

                data.savelisttot = _BaldwinAllReportContext.ExmStudentMarksProcessDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id 
                && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id).Distinct().ToArray();

                data.subjlist = data.savelist.Distinct<BaldwinAllReportDTO>(new progressEqualityComparer()).OrderBy(t => t.EYCES_SubjectOrder).ToArray();

                List<int> grade = new List<int>();
                foreach (BaldwinAllReportDTO x in data.subjlist)
                {
                    grade.Add(x.EMGR_Id);
                }

                data.grade_details = (from a in _BaldwinAllReportContext.Exm_Master_GradeDMO
                                      from b in _BaldwinAllReportContext.Exm_Master_Grade_DetailsDMO
                                      where (a.MI_Id == data.MI_Id && grade.Contains(a.EMGR_Id) && a.EMGR_Id == b.EMGR_Id)
                                      select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
    }

    class progressEqualityComparer : IEqualityComparer<BaldwinAllReportDTO>
    {
        public bool Equals(BaldwinAllReportDTO b1, BaldwinAllReportDTO b2)
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
        public int GetHashCode(BaldwinAllReportDTO bx)
        {
            int hCode = Convert.ToInt32(bx.ISMS_Id);
            return hCode.GetHashCode();
        }
    }
}
