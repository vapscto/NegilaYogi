
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
    public class HHSReport_5to7Impl : Interfaces.HHSReport_5to7Interface
    {
        //: Interfaces.HHSAllReportInterface
        private static ConcurrentDictionary<string, HHSReport_5to7DTO> _login =
         new ConcurrentDictionary<string, HHSReport_5to7DTO>();

        private readonly ExamContext _HHSReport_5to7Context;
        ILogger<BaldwinAllReportImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public HHSReport_5to7Impl(ExamContext cpContext, DomainModelMsSqlServerContext db, ILogger<BaldwinAllReportImpl> _acd)
        {
            _HHSReport_5to7Context = cpContext;
            _db = db;
            _acdimpl = _acd;
        }
        public async Task<HHSReport_5to7DTO> Getdetails(HHSReport_5to7DTO data)//int IVRMM_Id
        {
            HHSReport_5to7DTO getdata = new HHSReport_5to7DTO();
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = await _HHSReport_5to7Context.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToListAsync();
                getdata.yearlist = list.ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = await _HHSReport_5to7Context.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).OrderBy(t => t.ASMC_Order).ToListAsync();
                getdata.seclist = seclist.ToArray();

                List<AdmissionClass> admlist = new List<AdmissionClass>();
                admlist = await _HHSReport_5to7Context.AdmissionClass.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).OrderBy(t => t.ASMCL_Order).ToListAsync();
                getdata.classlist = admlist.ToArray();

                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = await _HHSReport_5to7Context.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).OrderBy(t => t.EME_ExamOrder).ToListAsync();
                getdata.exmstdlist = esmp.ToArray();

                getdata.hhstudlist = (from a in _HHSReport_5to7Context.StudentMappingDMO
                                      from b in _HHSReport_5to7Context.AdmissionClass
                                      from c in _HHSReport_5to7Context.School_M_Section
                                      from d in _HHSReport_5to7Context.Adm_M_Student
                                      from e in _HHSReport_5to7Context.IVRM_School_Master_SubjectsDMO
                                      where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == b.MI_Id && a.ASMS_Id == c.ASMS_Id && a.MI_Id == c.MI_Id && a.AMST_Id == d.AMST_Id && a.MI_Id == d.MI_Id && a.ISMS_Id == e.ISMS_Id && a.MI_Id == e.MI_Id && a.MI_Id == data.MI_Id && a.ESTSU_ElecetiveFlag == true)
                                      select new StudentMappingDTO
                                      {
                                          ASMCL_ClassName = b.ASMCL_ClassName,
                                          ASMC_SectionName = c.ASMC_SectionName,
                                          AMST_FirstName = ((d.AMST_FirstName.Trim() == null || d.AMST_FirstName.Trim() == "" ? "" : d.AMST_FirstName.Trim()) + (d.AMST_MiddleName.Trim() == null || d.AMST_MiddleName.Trim() == "" || d.AMST_MiddleName.Trim() == "0" ? "" : " " + d.AMST_MiddleName.Trim()) + (d.AMST_LastName.Trim() == null || d.AMST_LastName.Trim() == "" || d.AMST_LastName.Trim() == "0" ? "" : " " + d.AMST_LastName.Trim())).Trim(),
                                          AMST_Id = d.AMST_Id,
                                          AMST_DOB = d.AMST_DOB,
                                          ESTSU_ActiveFlg = a.ESTSU_ActiveFlg,
                                          ESTSU_ElecetiveFlag = a.ESTSU_ElecetiveFlag,
                                      }).Distinct().ToArray();


                //04-09-2018
                getdata.grade_list = _HHSReport_5to7Context.Exm_Master_GradeDMO.Where(t => t.MI_Id == data.MI_Id && t.EMGR_ActiveFlag == true).ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;

        }
        public async Task<HHSReport_5to7DTO> savedetails(HHSReport_5to7DTO data)
        {
            try
            {
                data.stu_details = (from f in _HHSReport_5to7Context.Adm_M_Student
                                    from h in _HHSReport_5to7Context.School_Adm_Y_Student
                                    from c in _HHSReport_5to7Context.AdmissionClass
                                    from s in _HHSReport_5to7Context.School_M_Section
                                    from y in _HHSReport_5to7Context.AcademicYear
                                    where (h.AMST_Id == f.AMST_Id && h.ASMAY_Id == y.ASMAY_Id && h.ASMAY_Id == y.ASMAY_Id && h.ASMCL_Id == c.ASMCL_Id
                                    && h.ASMS_Id == s.ASMS_Id && h.ASMAY_Id == y.ASMAY_Id && h.AMST_Id == data.AMST_Id && h.ASMAY_Id == data.ASMAY_Id
                                    && h.ASMCL_Id == data.ASMCL_Id && h.ASMS_Id == data.ASMS_Id)
                                    select new HHSReport_5to7DTO
                                    {
                                        AMST_Id = h.AMST_Id,
                                        Stuname = f.AMST_FirstName + ' ' + f.AMST_MiddleName + ' ' + f.AMST_LastName,
                                        AMST_AdmNo = f.AMST_AdmNo,
                                        ASMCL_ClassName = c.ASMCL_ClassName,
                                        ASMC_SectionName = s.ASMC_SectionName,
                                        AMST_FatherName = ((f.AMST_FatherName != null && f.AMST_FatherName != "" ? f.AMST_FatherName : "") +
                                        (f.AMST_FatherSurname != null && f.AMST_FatherSurname != "" ? "" : " " + f.AMST_FatherSurname)).Trim(),
                                        AMST_MotherName = ((f.AMST_MotherName != null && f.AMST_MotherName != "" ? f.AMST_MotherName : "") +
                                        (f.AMST_MotherSurname != null && f.AMST_MotherSurname != "" ? "" : " " + f.AMST_MotherSurname)).Trim(),
                                        AMAY_RollNo = h.AMAY_RollNo,
                                        AMST_RegistrationNo = f.AMST_RegistrationNo,
                                        AMST_PerStreet = f.AMST_PerStreet,
                                        AMST_PerArea = f.AMST_PerArea,
                                        AMST_PerCity = f.AMST_PerCity,
                                        AMST_DOB = f.AMST_DOB,
                                        ASMAY_Year = y.ASMAY_Year,
                                        IVRMMS_Name = f.AMST_PerState != null && f.AMST_PerState > 0 ? _HHSReport_5to7Context.State.Where(a => a.IVRMMS_Id == f.AMST_PerState).Distinct().FirstOrDefault().IVRMMS_Name : "",

                                        IVRMMC_CountryName = f.AMST_PerCountry != null && f.AMST_PerCountry > 0 ? _HHSReport_5to7Context.Country.Where(a => a.IVRMMC_Id == f.AMST_PerCountry).Distinct().FirstOrDefault().IVRMMC_CountryName : "",
                                        AMST_PerPincode = f.AMST_PerPincode,
                                        AMST_EmailId = f.AMST_emailId,
                                        AMST_Mobile = f.AMST_MobileNo,
                                        AMST_Photoname = f.AMST_Photoname

                                    }).Distinct().ToArray();


                //Eaxm Subject Sub-Subject Marks
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HHS_Get_Exam_Subject_SubSubj_Marks_List_1";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",SqlDbType.VarChar){Value = data.AMST_Id});
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
                        data.eam_sub_mrks_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }


                var EMCA_Id = _HHSReport_5to7Context.Exm_Category_ClassDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).EMCA_Id;
                var EYC_Id = _HHSReport_5to7Context.Exm_Yearly_CategoryDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EMCA_Id == EMCA_Id && t.EYC_ActiveFlg == true).EYC_Id;

                //Total Percentage and Rank              

                var exmrank = (from t in _HHSReport_5to7Context.ExmStudentMarksProcessDMO
                               from b in _HHSReport_5to7Context.masterexam
                               where (t.EME_Id == b.EME_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.AMST_Id == data.AMST_Id && b.EME_ActiveFlag == true)
                               select new HHSReport_5to7DTO
                               {
                                   EME_FinalExamFlag = b.EME_FinalExamFlag,
                                   ESTMP_SectionRank = Convert.ToInt32(t.ESTMP_SectionRank),
                                   ESTMP_Percentage = Convert.ToDecimal(t.ESTMP_Percentage),
                                   EME_Id = t.EME_Id,
                                   ESTMP_TotalObtMarks = Convert.ToDecimal(t.ESTMP_TotalObtMarks),
                                   ESTMP_TotalGrade = t.ESTMP_TotalGrade

                               }).Distinct().ToList();

                data.exmTPR = exmrank.ToArray();

                data.promotionrank = _HHSReport_5to7Context.Exm_Student_MP_PromotionDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id).ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Get_InternalAssesment_marks_Record";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.BigInt){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.BigInt){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",SqlDbType.BigInt){Value = data.AMST_Id});
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
                        data.marksprocess_Pro_markscal = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }


                //Internal Assessments //-----------Exam + Sub Exam
                var exam_subexam = (from a in _HHSReport_5to7Context.Exm_Yearly_Category_ExamsDMO
                                    from b in _HHSReport_5to7Context.Exm_Yrly_Cat_Exams_SubwiseDMO
                                    from c in _HHSReport_5to7Context.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                    from d in _HHSReport_5to7Context.masterexam
                                    from e in _HHSReport_5to7Context.mastersubexam
                                    from f in _HHSReport_5to7Context.ExmStudentMarksProcessSubjectwiseDMO
                                    where (a.EYC_Id == EYC_Id && a.EYCE_ActiveFlg == true && a.EYCE_SubExamFlg == true && b.EYCE_Id == a.EYCE_Id
                                    && b.EYCES_ActiveFlg == true && b.EYCES_SubExamFlg == true && c.EYCES_Id == b.EYCES_Id && c.EYCESSS_ActiveFlg == true
                                    && d.MI_Id == data.MI_Id && d.EME_Id == a.EME_Id && d.EME_ActiveFlag == true && e.MI_Id == data.MI_Id && e.EMSE_Id == c.EMSE_Id
                                    && e.EMSE_ActiveFlag == true && f.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id && f.ASMCL_Id == data.ASMCL_Id
                                    && f.ASMS_Id == data.ASMS_Id && f.EME_Id == a.EME_Id && f.ISMS_Id == b.ISMS_Id && f.AMST_Id == data.AMST_Id)
                                    select new HHSReport_5to7DTO
                                    {
                                        EYCE_Id = a.EYCE_Id,
                                        EME_Id = a.EME_Id,
                                        EME_ExamName = d.EME_ExamName,
                                        EYCESSE_Id = c.EYCESSS_Id,
                                        EMSE_Id = e.EMSE_Id,
                                        EMSE_SubExamName = e.EMSE_SubExamName,
                                        EYCES_Id = b.EYCES_Id,
                                        ISMS_Id = b.ISMS_Id,
                                        ESTMPS_Id = f.ESTMPS_Id
                                    }).Distinct().ToList();


                var exam_subexam_W = (from a in _HHSReport_5to7Context.Exm_Yearly_Category_ExamsDMO
                                      from b in _HHSReport_5to7Context.Exm_Yrly_Cat_Exams_SubwiseDMO
                                      from c in _HHSReport_5to7Context.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                      from d in _HHSReport_5to7Context.masterexam
                                      from e in _HHSReport_5to7Context.mastersubexam
                                      where (a.EYC_Id == EYC_Id && a.EYCE_ActiveFlg == true && a.EYCE_SubExamFlg == true && b.EYCE_Id == a.EYCE_Id
                                      && b.EYCES_ActiveFlg == true && b.EYCES_SubExamFlg == true && c.EYCES_Id == b.EYCES_Id && c.EYCESSS_ActiveFlg == true
                                      && d.MI_Id == data.MI_Id && d.EME_Id == a.EME_Id && d.EME_ActiveFlag == true && e.MI_Id == data.MI_Id && e.EMSE_Id == c.EMSE_Id
                                      && e.EMSE_ActiveFlag == true)
                                      select new HHSReport_5to7DTO
                                      {
                                          EYCE_Id = a.EYCE_Id,
                                          EME_Id = a.EME_Id,
                                          EME_ExamName = d.EME_ExamName,
                                          EMSE_Id = e.EMSE_Id,
                                          EMSE_SubExamName = e.EMSE_SubExamName,
                                          EYCESSS_MaxMarks = c.EYCESSS_MaxMarks
                                      }).Distinct().ToList();
                data.exam_subexam_W = exam_subexam_W.OrderBy(t => t.EME_Id).ToArray();
                data.exam_subexam = exam_subexam.ToArray();


                //--------------------Personality and Social Tralts

                data.remarkslist = _HHSReport_5to7Context.exammasterRemarkDMO.Where(a => a.MI_Id == data.MI_Id && a.EPCR_ActiveFlag == true).OrderBy(a => a.EPCR_RemarksOrder).ToArray();

                data.personality_status = (from a in _HHSReport_5to7Context.Exm_PersonalityDMO
                                           from b in _HHSReport_5to7Context.Exm_Student_PersonalityDMO
                                           from c in _HHSReport_5to7Context.exammasterDMO
                                           from e in _HHSReport_5to7Context.School_Adm_Y_Student
                                           from f in _HHSReport_5to7Context.Adm_M_Student
                                           where (f.AMST_Id == e.AMST_Id && a.MI_Id == b.MI_Id && b.EP_Id == a.EP_Id && b.EME_Id == c.EME_Id
                                                   && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == e.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && b.AMST_Id == e.AMST_Id
                                                   && b.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                                                   && b.ASMS_Id == data.ASMS_Id)
                                           select new HHSReport_5to7DTO
                                           {
                                               EP_Id = a.EP_Id,
                                               EP_PersonlaityName = a.EP_PersonlaityName,
                                               EPCR_RemarksName = b.ESPM_Remarks
                                           }).Distinct().ToArray();


                data.personalitymonth = (from a in _HHSReport_5to7Context.Exm_PersonalityDMO
                                         from b in _HHSReport_5to7Context.Exm_Student_PersonalityDMO
                                         from c in _HHSReport_5to7Context.exammasterDMO
                                         from e in _HHSReport_5to7Context.School_Adm_Y_Student
                                         from f in _HHSReport_5to7Context.Adm_M_Student
                                         where (f.AMST_Id == e.AMST_Id && a.MI_Id == b.MI_Id && b.EP_Id == a.EP_Id && b.EME_Id == c.EME_Id
                                                 && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == e.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && b.AMST_Id == e.AMST_Id
                                                 && b.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                                                 && b.ASMS_Id == data.ASMS_Id)
                                         select new HHSReport_5to7DTO
                                         {
                                             EME_Id = b.EME_Id,
                                             EME_ExamName = c.EME_ExamName,
                                             EME_ExamOrder = c.EME_ExamOrder
                                         }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                //  --------------------CO - CURRICULAR ACTIVITIES

                data.co_curricular_activity = (from a in _HHSReport_5to7Context.exammasterCoCulrricularDMO
                                               from b in _HHSReport_5to7Context.Exm_Student_CoCurricularDMO
                                               from c in _HHSReport_5to7Context.exammasterDMO
                                               from e in _HHSReport_5to7Context.School_Adm_Y_Student
                                               from f in _HHSReport_5to7Context.Adm_M_Student
                                               where (f.AMST_Id == e.AMST_Id && a.MI_Id == b.MI_Id && b.ECC_Id == a.ECC_Id && b.EME_Id == c.EME_Id
                                               && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == e.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && b.AMST_Id == e.AMST_Id
                                               && b.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id)
                                               select new HHSReport_5to7DTO
                                               {
                                                   ECC_Id = a.ECC_Id,
                                                   ECC_CoCurricularName = a.ECC_CoCurricularName,
                                                   EME_Id = b.EME_Id,
                                                   EME_ExamName = c.EME_ExamName,
                                                   EPCR_RemarksName = b.ESCOM_Remarks
                                               }).Distinct().ToArray();


                data.cocurillarymonth = (from a in _HHSReport_5to7Context.exammasterCoCulrricularDMO
                                         from b in _HHSReport_5to7Context.Exm_Student_CoCurricularDMO
                                         from c in _HHSReport_5to7Context.exammasterDMO
                                         from e in _HHSReport_5to7Context.School_Adm_Y_Student
                                         from f in _HHSReport_5to7Context.Adm_M_Student
                                         where (f.AMST_Id == e.AMST_Id && a.MI_Id == b.MI_Id && b.ECC_Id == a.ECC_Id && b.EME_Id == c.EME_Id
                                         && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == e.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && b.AMST_Id == e.AMST_Id
                                         && b.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id)
                                         select new HHSReport_5to7DTO
                                         {
                                             EME_Id = b.EME_Id,
                                             EME_ExamName = c.EME_ExamName,
                                             EME_ExamOrder = c.EME_ExamOrder
                                         }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                //Atteadence
                var list1 = await _HHSReport_5to7Context.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id).ToListAsync();
                data.yearlistdate = list1.ToArray();

                data.strtdate = Convert.ToDateTime(list1[0].ASMAY_From_Date.ToString());
                data.enddate = Convert.ToDateTime(list1[0].ASMAY_To_Date.ToString());


                //find the middle date
                double totaldays = 0;
                double days = 0;
                totaldays = (data.enddate - data.strtdate).TotalDays;
                if (totaldays > 1)
                {
                    days = (totaldays / 2);
                }
                data.middledate = data.strtdate.AddDays(days);


                ///Final attendance
                List<HHSReport_5to7DTO> result = new List<HHSReport_5to7DTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "StudentAttendance_HHS_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@from",SqlDbType.Date){Value = data.strtdate});
                    cmd.Parameters.Add(new SqlParameter("@to",SqlDbType.Date){Value = data.enddate});
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",SqlDbType.VarChar){Value = data.AMST_Id});
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                result.Add(new HHSReport_5to7DTO
                                {
                                    totalpresentday = Convert.ToDecimal(dataReader["TotalPresentDays"].ToString()),
                                    totalworkingday = Convert.ToDecimal(dataReader["TotalSchoolDays"].ToString()),
                                });
                            }
                        }
                        data.fullyearatt = result.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }


                //half att
                List<HHSReport_5to7DTO> result1 = new List<HHSReport_5to7DTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "StudentAttendance_HHS_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@from",SqlDbType.Date){Value = data.strtdate});
                    cmd.Parameters.Add(new SqlParameter("@to",SqlDbType.Date){Value = data.middledate});
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",SqlDbType.VarChar){Value = data.AMST_Id});
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                result1.Add(new HHSReport_5to7DTO
                                {
                                    totalpresentday = Convert.ToDecimal(dataReader["TotalPresentDays"].ToString()),
                                    totalworkingday = Convert.ToDecimal(dataReader["TotalSchoolDays"].ToString()),

                                });
                            }
                        }
                        data.halfyearatt = result1.ToArray();

                        //Grade Deatails 

                        data.grade_detailslist = _HHSReport_5to7Context.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == data.EMGR_Id && t.EMGD_ActiveFlag == true).OrderByDescending(a => a.EMGD_From).ToArray();


                        // Remarks 

                        data.examwiseremarks = (from a in _HHSReport_5to7Context.Exm_ProgressCard_RemarksDMO
                                                from b in _HHSReport_5to7Context.AcademicYear
                                                from c in _HHSReport_5to7Context.AdmissionClass
                                                from d in _HHSReport_5to7Context.School_M_Section
                                                from e in _HHSReport_5to7Context.School_Adm_Y_StudentDMO
                                                from f in _HHSReport_5to7Context.Adm_M_Student
                                                from g in _HHSReport_5to7Context.masterexam
                                                where (a.AMST_Id == e.AMST_Id && e.AMST_Id == f.AMST_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id
                                                 && a.ASMS_Id == d.ASMS_Id && e.ASMAY_Id == b.ASMAY_Id && e.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == d.ASMS_Id
                                                 && a.EME_ID == g.EME_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                                 && a.AMST_Id == data.AMST_Id && e.ASMAY_Id == data.ASMAY_Id && e.ASMCL_Id == data.ASMCL_Id
                                                 && e.ASMS_Id == data.ASMS_Id && e.AMST_Id == data.AMST_Id && a.EMER_ActiveFlag == true)
                                                select new HHSReport_5to7DTO
                                                {
                                                    EME_ExamName = g.EME_ExamName,
                                                    EMER_Remarks = a.EMER_Remarks,
                                                    EME_ExamOrder = g.EME_ExamOrder,
                                                    AMST_Id = a.AMST_Id
                                                }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }

                data.promotionstatus = _HHSReport_5to7Context.ExamPromotionRemarksDMO.Where(p => p.MI_Id == data.MI_Id
                && p.AMST_Id == data.AMST_Id && p.EPRD_Promotionflag == "PE" && p.ASMAY_Id == data.ASMAY_Id
                && p.ASMCL_Id == data.ASMCL_Id && p.ASMS_Id == data.ASMS_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public HHSReport_5to7DTO Getsection(HHSReport_5to7DTO data)
        {
            try
            {
                data.fillsection = (from a in _db.Exm_Category_ClassDMO
                                    from b in _db.School_M_Class
                                    from c in _db.School_M_Section
                                    where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                                    && a.ASMCL_Id == data.ASMCL_Id && a.ECAC_ActiveFlag == true)
                                    select c).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;


        }
        public HHSReport_5to7DTO getclass(HHSReport_5to7DTO data)
        {
            try
            {
                data.fillclass = (from a in _db.Exm_Category_ClassDMO
                                  from b in _db.School_M_Class
                                  where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true)
                                  select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }
        public HHSReport_5to7DTO GetAttendence(HHSReport_5to7DTO data)
        {
            try
            {
                data.fillstudents = (from a in _db.School_Adm_Y_StudentDMO
                                     from b in _db.admissioncls
                                     from c in _db.Adm_M_Student
                                     from d in _db.Section
                                     from e in _db.AcademicYear
                                     where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == e.ASMAY_Id && c.MI_Id == data.MI_Id
                                     && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && c.AMST_SOL != "WD"
                                     && d.ASMS_Id == a.ASMS_Id && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id)
                                     select new HHSReport_5to7DTO
                                     {
                                         AMST_FirstName = ((c.AMST_FirstName == null || c.AMST_FirstName == "" ? "" : " " + c.AMST_FirstName) +
                                         (c.AMST_MiddleName == null || c.AMST_MiddleName == "" || c.AMST_MiddleName == "0" ? "" : " " + c.AMST_MiddleName) +
                                         (c.AMST_LastName == null || c.AMST_LastName == "" || c.AMST_LastName == "0" ? "" : " " + c.AMST_LastName) +
                                         (c.AMST_AdmNo == null || c.AMST_AdmNo == "" ? "" : " : " + c.AMST_AdmNo)).Trim(),
                                         AMST_Id = a.AMST_Id
                                     }).Distinct().OrderBy(t => t.AMST_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HHSReport_5to7DTO Get_primary_savedetails(HHSReport_5to7DTO data)
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

                data.stu_details = (from f in _HHSReport_5to7Context.Adm_M_Student
                                    from h in _HHSReport_5to7Context.School_Adm_Y_Student
                                    from c in _HHSReport_5to7Context.AdmissionClass
                                    from s in _HHSReport_5to7Context.School_M_Section
                                    from y in _HHSReport_5to7Context.AcademicYear
                                    where (h.AMST_Id == f.AMST_Id && ids.Contains(f.AMST_ActiveFlag) && sol.Contains(f.AMST_SOL) && ids.Contains(h.AMAY_ActiveFlag)
                                    && h.ASMAY_Id == y.ASMAY_Id && h.ASMAY_Id == y.ASMAY_Id && h.ASMCL_Id == c.ASMCL_Id && h.ASMS_Id == s.ASMS_Id
                                    && h.ASMAY_Id == y.ASMAY_Id && h.AMST_Id == data.AMST_Id && h.ASMAY_Id == data.ASMAY_Id && h.ASMCL_Id == data.ASMCL_Id
                                    && h.ASMS_Id == data.ASMS_Id)
                                    select new HHSReport_5to7DTO
                                    {
                                        AMST_Id = h.AMST_Id,
                                        Stuname = f.AMST_FirstName + ' ' + f.AMST_MiddleName + ' ' + f.AMST_LastName,
                                        AMST_AdmNo = f.AMST_AdmNo,
                                        ASMCL_ClassName = c.ASMCL_ClassName,
                                        ASMC_SectionName = s.ASMC_SectionName,
                                        AMST_FatherName = ((f.AMST_FatherName != null && f.AMST_FatherName != "" ? f.AMST_FatherName : "") +
                                        (f.AMST_FatherSurname != null && f.AMST_FatherSurname != "" ? "" : " " + f.AMST_FatherSurname)).Trim(),
                                        AMST_MotherName = ((f.AMST_MotherName != null && f.AMST_MotherName != "" ? f.AMST_MotherName : "") +
                                        (f.AMST_MotherSurname != null && f.AMST_MotherSurname != "" ? "" : " " + f.AMST_MotherSurname)).Trim(),
                                        AMAY_RollNo = h.AMAY_RollNo,
                                        AMST_RegistrationNo = f.AMST_RegistrationNo,
                                        AMST_PerStreet = f.AMST_PerStreet,
                                        AMST_PerArea = f.AMST_PerArea,
                                        AMST_PerCity = f.AMST_PerCity,
                                        AMST_DOB = f.AMST_DOB,
                                        ASMAY_Year = y.ASMAY_Year,
                                        IVRMMS_Name = f.AMST_PerState != null && f.AMST_PerState > 0 ? _HHSReport_5to7Context.State.Where(a => a.IVRMMS_Id == f.AMST_PerState).Distinct().FirstOrDefault().IVRMMS_Name : "",
                                        IVRMMC_CountryName = f.AMST_PerCountry != null && f.AMST_PerCountry > 0 ? _HHSReport_5to7Context.Country.Where(a => a.IVRMMC_Id == f.AMST_PerCountry).Distinct().FirstOrDefault().IVRMMC_CountryName : "",
                                        AMST_PerPincode = f.AMST_PerPincode,
                                        AMST_EmailId = f.AMST_emailId,
                                        AMST_Mobile = f.AMST_MobileNo,
                                        AMST_Photoname = f.AMST_Photoname
                                    }).Distinct().ToArray();


                //Eaxm Subject Sub-Subject Marks
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HHS_Get_Exam_Subject_SubSubj_Marks_List_1";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",SqlDbType.VarChar){Value = data.AMST_Id});
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
                        data.eam_sub_mrks_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }


                var EMCA_Id = _HHSReport_5to7Context.Exm_Category_ClassDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).EMCA_Id;

                var EYC_Id = _HHSReport_5to7Context.Exm_Yearly_CategoryDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                && t.EMCA_Id == EMCA_Id && t.EYC_ActiveFlg == true).EYC_Id;

                var exmrank = (from t in _HHSReport_5to7Context.ExmStudentMarksProcessDMO
                               from b in _HHSReport_5to7Context.masterexam
                               where (t.EME_Id == b.EME_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id
                               && t.ASMS_Id == data.ASMS_Id && t.AMST_Id == data.AMST_Id && b.EME_ActiveFlag == true)
                               select new HHSReport_5to7DTO
                               {
                                   EME_FinalExamFlag = b.EME_FinalExamFlag,
                                   ESTMP_SectionRank = Convert.ToInt32(t.ESTMP_SectionRank),
                                   ESTMP_Percentage = Convert.ToDecimal(t.ESTMP_Percentage),
                                   EME_Id = t.EME_Id,
                                   ESTMP_TotalObtMarks = Convert.ToDecimal(t.ESTMP_TotalObtMarks),
                                   ESTMP_TotalGrade = t.ESTMP_TotalGrade

                               }).Distinct().ToList();

                data.exmTPR = exmrank.ToArray();

                data.promotionrank = _HHSReport_5to7Context.Exm_Student_MP_PromotionDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id).ToArray();


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HHS_Get_Exam_Attendance";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar){Value = data.AMST_Id});
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
                        data.Present_attendence = retObject.ToArray();

                        data.grade_detailslist = _HHSReport_5to7Context.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == data.EMGR_Id
                        && t.EMGD_ActiveFlag == true).OrderByDescending(a => a.EMGD_From).ToArray();

                        data.examwiseremarks = (from a in _HHSReport_5to7Context.Exm_ProgressCard_RemarksDMO
                                                from b in _HHSReport_5to7Context.AcademicYear
                                                from c in _HHSReport_5to7Context.AdmissionClass
                                                from d in _HHSReport_5to7Context.School_M_Section
                                                from e in _HHSReport_5to7Context.School_Adm_Y_StudentDMO
                                                from f in _HHSReport_5to7Context.Adm_M_Student
                                                from g in _HHSReport_5to7Context.masterexam
                                                where (a.AMST_Id == e.AMST_Id && e.AMST_Id == f.AMST_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id
                                                 && a.ASMS_Id == d.ASMS_Id && e.ASMAY_Id == b.ASMAY_Id && e.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == d.ASMS_Id
                                                 && a.EME_ID == g.EME_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                                 && a.AMST_Id == data.AMST_Id && e.ASMAY_Id == data.ASMAY_Id && e.ASMCL_Id == data.ASMCL_Id
                                                 && e.ASMS_Id == data.ASMS_Id && e.AMST_Id == data.AMST_Id
                                                 && sol.Contains(f.AMST_SOL) && ids.Contains(f.AMST_ActiveFlag) && ids.Contains(f.AMST_ActiveFlag)
                                                 && a.EMER_ActiveFlag == true)
                                                select new HHSReport_5to7DTO
                                                {
                                                    EME_ExamName = g.EME_ExamName,
                                                    EMER_Remarks = a.EMER_Remarks,
                                                    EME_ExamOrder = g.EME_ExamOrder,
                                                    AMST_Id = a.AMST_Id
                                                }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }

                data.promotionstatus = _HHSReport_5to7Context.ExamPromotionRemarksDMO.Where(p => p.MI_Id == data.MI_Id
                && p.AMST_Id == data.AMST_Id && p.EPRD_Promotionflag == "PE" && p.ASMAY_Id == data.ASMAY_Id
                && p.ASMCL_Id == data.ASMCL_Id && p.ASMS_Id == data.ASMS_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}