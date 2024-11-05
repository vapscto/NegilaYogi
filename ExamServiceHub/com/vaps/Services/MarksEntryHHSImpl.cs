using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.TT;
using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DomainModel.Model.com.vapstech.MobileApp;
using System.Dynamic;
using DomainModel.Model.com.vapstech.Exam;

namespace ExamServiceHub.com.vaps.Services
{
    public class MarksEntryHHSImpl : MarksEntryHHSInterface
    {
        public ExamContext _examcontext;
        public DomainModelMsSqlServerContext _db;
        ILogger<MarksEntryImpl> _acdimpl;
        public MarksEntryHHSImpl(ExamContext ttcategory, DomainModelMsSqlServerContext db, ILogger<MarksEntryImpl> _acd)
        {
            _examcontext = ttcategory;
            _db = db;
            _acdimpl = _acd;
        }
        public MarksEntryHHSDTO getdetails(MarksEntryHHSDTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _examcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.Distinct().ToArray();

                if (data.stringmobileorportal == "Mobile")
                {
                    List<IVRM_User_MobileApp_Login_Privileges> Staffmobileappprivileges = new List<IVRM_User_MobileApp_Login_Privileges>();
                    Staffmobileappprivileges = _db.IVRM_User_MobileApp_Login_Privileges.Where(t => t.IVRMUL_Id == data.UserId && t.MI_Id == data.MI_Id).ToList();

                    if (Staffmobileappprivileges.Count() > 0)
                    {
                        data.Staffmobileappprivileges = (from Mobilepage in _examcontext.IVRM_MobileApp_Page
                                                         from MobileRolePrivileges in _examcontext.IVRM_Role_MobileApp_Privileges
                                                         from UserRolePrivileges in _examcontext.IVRM_User_MobileApp_Login_Privileges
                                                         where (MobileRolePrivileges.MI_ID == UserRolePrivileges.MI_Id
                                                         && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id
                                                         && Mobilepage.IVRMMAP_Id == UserRolePrivileges.IVRMMAP_Id && MobileRolePrivileges.IVRMRT_Id == data.roleid
                                                         && MobileRolePrivileges.MI_ID == data.MI_Id && UserRolePrivileges.IVRMUL_Id == data.UserId)
                                                         select new ExamMarksDTO
                                                         {
                                                             Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                             Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                             Pageurl = Mobilepage.IVRMMAP_AppPageURL,
                                                             IVRMRMAP_Id = MobileRolePrivileges.IVRMRMAP_Id,
                                                             IVRMMAP_AddFlg = UserRolePrivileges.IVRMUMALP_AddFlg,
                                                             IVRMMAP_UpdateFlg = UserRolePrivileges.IVRMUMALP_UpdateFlg,
                                                             IVRMMAP_DeleteFlg = UserRolePrivileges.IVRMUMALP_DeleteFlg
                                                         }).OrderBy(d => d.IVRMRMAP_Id).ToArray();

                        data.mobileprivileges = "true";
                    }
                    else
                    {
                        data.mobileprivileges = "false";
                    }
                }


                data.classlist = get_classes(data).classlist;


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public MarksEntryHHSDTO get_classes(MarksEntryHHSDTO data)
        {
            try
            {
                var classid = _examcontext.Masterclasscategory.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.Is_Active == true && t.ASMAY_Id == data.ASMAY_Id).Select(t => t.ASMCL_Id).ToArray();

                var classexmid = (from e in _examcontext.Staff_User_Login
                                  from f in _examcontext.Exm_Login_PrivilegeDMO
                                  from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                  where (e.Id == data.UserId && //data.MI_Id == data.MI_Id &&
                                    f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id && classid.Contains(i.ASMCL_Id)
                                    && f.ELP_Id == i.ELP_Id && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true)
                                  select new MarksEntryHHSDTO
                                  {
                                      ASMCL_Id = i.ASMCL_Id
                                  }).Distinct().Select(t => t.ASMCL_Id).ToArray();

                List<AdmissionClass> clist = new List<AdmissionClass>();
                clist = _examcontext.AdmissionClass.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true && classexmid.Contains(t.ASMCL_Id)).ToList();
                data.classlist = clist.Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public MarksEntryHHSDTO get_sections(MarksEntryHHSDTO data)
        {
            try
            {
                var classid = _examcontext.Masterclasscategory.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id).Select(t => t.ASMCC_Id).ToArray();

                var secid = _examcontext.AdmSchoolMasterClassCatSec.Where(t => classid.Contains(t.ASMCC_Id)).Select(t => t.ASMS_Id).ToArray();

                var sectionexamid = (from e in _examcontext.Staff_User_Login
                                     from f in _examcontext.Exm_Login_PrivilegeDMO
                                     from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                     where (e.Id == data.UserId && //data.MI_Id == data.MI_Id &&
                                       f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id && i.ASMCL_Id == data.ASMCL_Id && secid.Contains(i.ASMS_Id)
                                       && f.ELP_Id == i.ELP_Id && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true)
                                     select new MarksEntryHHSDTO
                                     {
                                         ASMS_Id = i.ASMS_Id
                                     }).Distinct().Select(t => t.ASMS_Id).ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = _examcontext.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1 && sectionexamid.Contains(t.ASMS_Id)).ToList();
                data.sectionlist = seclist.Distinct().OrderBy(t => t.ASMC_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public MarksEntryHHSDTO get_exams(MarksEntryHHSDTO data)
        {
            try
            {
                var catid = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.MI_Id == data.MI_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).ToArray();

                var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && catid.Contains(t.EMCA_Id) && t.EYC_ActiveFlg == true).Select(t => t.EYC_Id).ToArray();

                var emeid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id) && t.EYCE_ActiveFlg == true).Select(t => t.EME_Id).ToArray();

                List<exammasterDMO> examlist = new List<exammasterDMO>();
                examlist = _examcontext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && emeid.Contains(t.EME_Id)).ToList();
                data.examlist = examlist.Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public MarksEntryHHSDTO get_subjects(MarksEntryHHSDTO data)
        {
            try
            {
                var catid = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.MI_Id == data.MI_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).ToArray();

                var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && catid.Contains(t.EMCA_Id) && t.EYC_ActiveFlg == true).Select(t => t.EYC_Id).ToArray();

                var eyceid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id) && t.EYCE_ActiveFlg == true && t.EME_Id == data.EME_Id).Select(t => t.EYCE_Id).ToArray();

                var subid = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id) && t.EYCES_ActiveFlg == true).Select(t => t.ISMS_Id).ToArray();

                var sectionexamid = (from e in _examcontext.Staff_User_Login
                                     from f in _examcontext.Exm_Login_PrivilegeDMO
                                     from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                     where (e.Id == data.UserId && e.MI_Id == data.MI_Id &&
                                       f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id && i.ASMS_Id == data.ASMS_Id && i.ASMCL_Id == data.ASMCL_Id && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true && f.ELP_Id == i.ELP_Id && subid.Contains(i.ISMS_Id))//subid.Contains(i.ISMS_Id) subid_with_flags.Contains(i.ISMS_Id)
                                     select new MarksEntryHHSDTO
                                     {
                                         ISMS_Id = i.ISMS_Id
                                     }).Distinct().Select(t => t.ISMS_Id).ToArray();


                List<IVRM_School_Master_SubjectsDMO> subjects = new List<IVRM_School_Master_SubjectsDMO>();
                subjects = _examcontext.IVRM_School_Master_SubjectsDMO.Where(c => c.MI_Id == data.MI_Id && c.ISMS_ActiveFlag == 1 && c.ISMS_ExamFlag == 1 && sectionexamid.Contains(c.ISMS_Id)).ToList();
                data.subjectlist = subjects.OrderBy(t => t.ISMS_OrderFlag).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public MarksEntryHHSDTO onsearch(MarksEntryHHSDTO data)
        {
            var catid = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id
              && t.MI_Id == data.MI_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).ToArray();

            var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EYC_ActiveFlg == true
            && catid.Contains(t.EMCA_Id)).Select(t => t.EYC_Id).ToArray();

            var eyceid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id) && t.EYCE_ActiveFlg == true
            && t.EME_Id == data.EME_Id).Select(t => t.EYCE_Id).ToArray();

            var eycid_details = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EYC_ActiveFlg == true
             && catid.Contains(t.EMCA_Id) && t.EYC_BasedOnPaperTypeFlg == true).ToList();

            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())                {
                    //cmd.CommandText = "Exam_get_Marks_Entry_Modify";
                    cmd.CommandText = "Exam_Duplicate_MarksEntry_Delete";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)                                {                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);                                }                                retObject.Add((ExpandoObject)dataRow1);                            }                        }

                    }                    catch (Exception ex)                    {                        _acdimpl.LogError(ex.Message);                        _acdimpl.LogDebug(ex.Message);                    }                }
                string order = "AMST_FirstName";
                var get_configuration = _examcontext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                data.configuration = get_configuration.ToArray();

                if (get_configuration != null && get_configuration.Count > 0)
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

                List<ExmStudentMarksProcessDMO> calculationid = new List<ExmStudentMarksProcessDMO>();
                calculationid = _examcontext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id
                && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id).ToList();

                if (calculationid.Count > 0)
                {
                    data.marksdeleteflag = true;
                }

                else if (calculationid.Count == 0)
                {
                    List<ExmStudentMarksProcessSubjectwiseDMO> calculationSubWiseid = new List<ExmStudentMarksProcessSubjectwiseDMO>();
                    calculationSubWiseid = _examcontext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                    && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id).ToList();

                    if (calculationSubWiseid.Count > 0)
                    {
                        data.marksdeleteflag = true;
                    }
                    else if (calculationSubWiseid.Count == 0)
                    {
                        data.marksdeleteflag = false;
                    }
                }

                var subject_details = (from a in _examcontext.Exm_Category_ClassDMO
                                       from b in _examcontext.Exm_Yearly_CategoryDMO
                                       from c in _examcontext.Exm_Yearly_Category_ExamsDMO
                                       from d in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                       where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                       && a.ECAC_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == a.EMCA_Id
                                       && b.EYC_ActiveFlg == true && c.EYC_Id == b.EYC_Id && c.EME_Id == data.EME_Id && c.EYCE_ActiveFlg == true
                                       && d.EYCE_Id == c.EYCE_Id && d.ISMS_Id == data.ISMS_Id && d.EYCES_ActiveFlg == true)
                                       select d).Distinct().ToList();

                data.subject_details = subject_details.ToArray();
                data.EYCES_MarksGradeEntryFlg = subject_details[0].EYCES_MarksGradeEntryFlg;

                data.EYCES_SubSubjectFlg = subject_details[0].EYCES_SubSubjectFlg;
                data.EYCES_SubExamFlg = subject_details[0].EYCES_SubExamFlg;

                /* SubSubject ==true && SubExam ==false */
                if (data.EYCES_SubSubjectFlg && !data.EYCES_SubExamFlg)
                {
                    data.subject_subsubjects = (from a in _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                                from b in _examcontext.mastersubsubject
                                                from c in _examcontext.Exm_Login_PrivilegeDMO
                                                from d in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                                from e in _examcontext.Exm_Login_Privilege_SubSubjectsDMO
                                                from f in _examcontext.IVRM_School_Master_SubjectsDMO
                                                from g in _examcontext.Staff_User_Login
                                                where (c.ELP_Id == d.ELP_Id && d.ELPS_Id == e.ELPS_Id && e.EMSS_Id == b.EMSS_Id
                                                && f.ISMS_Id == d.ISMS_Id && c.MI_Id == data.MI_Id && c.Login_Id == g.IVRMSTAUL_Id && c.ASMAY_Id == data.ASMAY_Id
                                                && b.MI_Id == data.MI_Id && b.EMSS_ActiveFlag == true && d.ASMCL_Id == data.ASMCL_Id && d.ASMS_Id == data.ASMS_Id
                                                && a.EYCES_Id == subject_details[0].EYCES_Id && b.EMSS_Id == a.EMSS_Id && g.Id == data.UserId
                                                && a.EYCESSS_ActiveFlg == true && e.ELPSS_ActiveFlg == true)
                                                select new ExamSubjectMappingDTO
                                                {
                                                    EYCESSS_Id = a.EYCESSS_Id,
                                                    EYCES_Id = a.EYCES_Id,
                                                    EMSS_Id = a.EMSS_Id,
                                                    EMGR_Id = a.EMGR_Id,
                                                    EYCESSS_MaxMarks = a.EYCESSS_MaxMarks,
                                                    EYCESSS_MinMarks = a.EYCESSS_MinMarks,
                                                    EYCESSS_MarksEntryMax = a.EYCESSS_MarksEntryMax,
                                                    EYCESSS_ExemptedFlg = a.EYCESSS_ExemptedFlg,
                                                    EYCESSS_ExemptedPer = a.EYCESSS_ExemptedPer,
                                                    EYCESSS_ActiveFlg = a.EYCESSS_ActiveFlg,
                                                    EYCESSS_SubSubjectOrder = a.EYCESSS_SubSubjectOrder,
                                                    EMSS_SubSubjectName = b.EMSS_SubSubjectName,
                                                    EMSS_SubSubjectCode = b.EMSS_SubSubjectCode
                                                }).Distinct().OrderBy(a => a.EYCESSS_SubSubjectOrder).ToArray();
                }

                /* SubExam ==true && SubSubject ==false */
                else if (data.EYCES_SubExamFlg && !data.EYCES_SubSubjectFlg)
                {
                    data.subject_subexams = (from a in _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                             from b in _examcontext.mastersubexam
                                             where (b.EMSE_Id == a.EMSE_Id && b.MI_Id == data.MI_Id && b.EMSE_ActiveFlag == true
                                             && a.EYCES_Id == subject_details[0].EYCES_Id && a.EYCESSS_ActiveFlg == true)
                                             select new ExamSubjectMappingDTO
                                             {
                                                 EYCESSE_Id = a.EYCESSS_Id,
                                                 EYCES_Id = a.EYCES_Id,
                                                 EMSE_Id = a.EMSE_Id,
                                                 EMGR_Id = a.EMGR_Id,
                                                 EYCESSE_MaxMarks = a.EYCESSS_MaxMarks,
                                                 EYCESSE_MinMarks = a.EYCESSS_MinMarks,
                                                 EYCESSS_MarksEntryMax = a.EYCESSS_MarksEntryMax,
                                                 EYCESSE_ExemptedFlg = a.EYCESSS_ExemptedFlg,
                                                 EYCESSE_ExemptedPer = a.EYCESSS_ExemptedPer,
                                                 EYCESSE_ActiveFlg = a.EYCESSS_ActiveFlg,
                                                 EYCESSE_SubExamOrder = a.EYCESSS_SubSubjectOrder,
                                                 EMSE_SubExamName = b.EMSE_SubExamName,
                                                 EMSE_SubExamCode = b.EMSE_SubExamCode
                                             }).Distinct().OrderBy(a => a.EYCESSE_SubExamOrder).ToArray();
                }

                /* SubExam ==true && SubSubject ==true */
                else
                {
                    data.subject_subexams = (from a in _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                             from b in _examcontext.mastersubexam
                                             from c in _examcontext.mastersubsubject
                                             where (a.EMSS_Id == c.EMSS_Id && b.EMSE_Id == a.EMSE_Id && b.MI_Id == data.MI_Id
                                             && b.EMSE_ActiveFlag == true && a.EYCESSS_ActiveFlg == true && a.EYCES_Id == subject_details[0].EYCES_Id)
                                             select new ExamSubjectMappingDTO
                                             {
                                                 EYCESSE_Id = a.EYCESSS_Id,
                                                 EYCES_Id = a.EYCES_Id,
                                                 EMSE_Id = a.EMSE_Id,
                                                 EMSS_Id = a.EMSS_Id,
                                                 EMGR_Id = a.EMGR_Id,
                                                 EYCESSE_MaxMarks = a.EYCESSS_MaxMarks,
                                                 EYCESSE_MinMarks = a.EYCESSS_MinMarks,
                                                 EYCESSS_MarksEntryMax = a.EYCESSS_MarksEntryMax,
                                                 EYCESSE_ExemptedFlg = a.EYCESSS_ExemptedFlg,
                                                 EYCESSE_ExemptedPer = a.EYCESSS_ExemptedPer,
                                                 EYCESSE_ActiveFlg = a.EYCESSS_ActiveFlg,
                                                 EYCESSE_SubExamOrder = a.EYCESSS_SubSubjectOrder,
                                                 EMSE_SubExamName = b.EMSE_SubExamName,
                                                 EMSE_SubExamCode = b.EMSE_SubExamCode
                                             }).Distinct().OrderBy(a => a.EYCESSE_SubExamOrder).ToArray();


                    data.subject_subsubjects = (from a in _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                                from b in _examcontext.mastersubexam
                                                from c in _examcontext.mastersubsubject
                                                where (a.EMSS_Id == c.EMSS_Id && b.EMSE_Id == a.EMSE_Id && b.MI_Id == data.MI_Id
                                                && b.EMSE_ActiveFlag == true && a.EYCESSS_ActiveFlg == true && a.EYCES_Id == subject_details[0].EYCES_Id)
                                                select new ExamSubjectMappingDTO
                                                {
                                                    EYCESSE_Id = a.EYCESSS_Id,
                                                    EYCES_Id = a.EYCES_Id,
                                                    EMSE_Id = a.EMSE_Id,
                                                    EMSS_Id = a.EMSS_Id,
                                                    EMGR_Id = a.EMGR_Id,
                                                    EYCESSE_MaxMarks = a.EYCESSS_MaxMarks,
                                                    EYCESSE_MinMarks = a.EYCESSS_MinMarks,
                                                    EYCESSS_MarksEntryMax = a.EYCESSS_MarksEntryMax,
                                                    EYCESSE_ExemptedFlg = a.EYCESSS_ExemptedFlg,
                                                    EYCESSE_ExemptedPer = a.EYCESSS_ExemptedPer,
                                                    EYCESSE_ActiveFlg = a.EYCESSS_ActiveFlg,
                                                    EYCESSE_SubExamOrder = a.EYCESSS_SubSubjectOrder,
                                                    EMSE_SubExamName = b.EMSE_SubExamName,
                                                    EMSE_SubExamCode = b.EMSE_SubExamCode
                                                }).Distinct().OrderBy(a => a.EYCESSE_SubExamOrder).ToArray();


                    data.subsubjectlist = (from a in _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                           from b in _examcontext.mastersubsubject
                                           from c in _examcontext.Exm_Login_PrivilegeDMO
                                           from d in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                           from e in _examcontext.Exm_Login_Privilege_SubSubjectsDMO
                                           from f in _examcontext.IVRM_School_Master_SubjectsDMO
                                           from g in _examcontext.Staff_User_Login
                                           where (c.ELP_Id == d.ELP_Id && d.ELPS_Id == e.ELPS_Id && e.EMSS_Id == b.EMSS_Id && f.ISMS_Id == d.ISMS_Id
                                           && c.MI_Id == data.MI_Id && c.Login_Id == g.IVRMSTAUL_Id && c.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id
                                           && b.EMSS_ActiveFlag == true && d.ASMCL_Id == data.ASMCL_Id && d.ASMS_Id == data.ASMS_Id && a.EYCESSS_ActiveFlg == true
                                           && a.EYCES_Id == subject_details[0].EYCES_Id && b.EMSS_Id == a.EMSS_Id && g.Id == data.UserId
                                           && e.ELPSS_ActiveFlg == true)
                                           select new ExamSubjectMappingDTO
                                           {
                                               EMSS_Id = a.EMSS_Id,
                                               EYCESSE_SubExamOrder = b.EMSS_Order,
                                               EMSS_SubSubjectName = b.EMSS_SubSubjectName,
                                           }).Distinct().OrderBy(a => a.EYCESSE_SubExamOrder).ToArray();

                    data.subexamlist = (from a in _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                        from b in _examcontext.mastersubexam
                                        from c in _examcontext.mastersubsubject
                                        where (a.EMSS_Id == c.EMSS_Id && b.EMSE_Id == a.EMSE_Id && b.MI_Id == data.MI_Id
                                        && b.EMSE_ActiveFlag == true && a.EYCESSS_ActiveFlg == true && a.EYCES_Id == subject_details[0].EYCES_Id)
                                        select new ExamSubjectMappingDTO
                                        {
                                            EMSE_Id = a.EMSE_Id,
                                            EMSE_SubExamName = b.EMSE_SubExamName,
                                            EYCESSE_SubExamOrder = b.EMSE_SubExamOrder
                                        }).Distinct().OrderBy(a => a.EYCESSE_SubExamOrder).ToArray();

                }

                data.EMGR_Id = subject_details[0].EMGR_Id;

                if (data.EYCES_MarksGradeEntryFlg == "G")
                {
                    data.grade_details = (from a in _examcontext.Exm_Master_GradeDMO
                                          from b in _examcontext.Exm_Master_Grade_DetailsDMO
                                          where (a.MI_Id == data.MI_Id && a.EMGR_Id == data.EMGR_Id && b.EMGR_Id == data.EMGR_Id && b.EMGD_ActiveFlag == true)
                                          select b).Select(t => t.EMGD_Name).Distinct().ToArray();

                    if (data.EYCES_SubSubjectFlg)
                    {
                        data.subsubject_gradedetails = (from a in _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                                        from b in _examcontext.Exm_Master_GradeDMO
                                                        from c in _examcontext.Exm_Master_Grade_DetailsDMO
                                                        where (a.EYCES_Id == subject_details[0].EYCES_Id && a.EMGR_Id == b.EMGR_Id && b.MI_Id == data.MI_Id
                                                        && b.EMGR_ActiveFlag == true && c.EMGR_Id == b.EMGR_Id && c.EMGD_ActiveFlag == true
                                                        && a.EYCESSS_ActiveFlg == true)
                                                        select c).Distinct().ToArray();
                    }

                    if (data.EYCES_SubExamFlg)
                    {
                        data.subexam_gradedetails = (from a in _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                                     from b in _examcontext.Exm_Master_GradeDMO
                                                     from c in _examcontext.Exm_Master_Grade_DetailsDMO
                                                     where (a.EYCES_Id == subject_details[0].EYCES_Id && a.EMGR_Id == b.EMGR_Id && b.MI_Id == data.MI_Id
                                                     && b.EMGR_ActiveFlag == true && c.EMGR_Id == b.EMGR_Id && c.EMGD_ActiveFlag == true
                                                     && a.EYCESSS_ActiveFlg == true)
                                                     select c).Distinct().ToArray();
                    }
                }

                var alrdy_stu_count = _examcontext.ExamMarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id
                && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.ESTM_ActiveFlg == true).ToList().Count();

                List<long> stu_list_mapped = new List<long>();

                if (eycid_details.Count > 0)
                {
                    stu_list_mapped = _examcontext.StudentMappingDMO.Where(k => k.MI_Id == data.MI_Id && k.ASMAY_Id == data.ASMAY_Id && k.ASMCL_Id == data.ASMCL_Id
                    && k.ASMS_Id == data.ASMS_Id && k.ISMS_Id == data.ISMS_Id && k.ESTSU_ActiveFlg == true
                    && k.EME_Id == data.EME_Id).Select(t => t.AMST_Id).Distinct().ToList();

                    data.get_student_wise_papertype_list = (from k in _examcontext.Exm_Student_Examwise_PTDMO
                                                            from b in _examcontext.Exm_Master_PaperTypeDMO
                                                            where (k.EMPATY_Id == b.EMPATY_Id && k.MI_Id == data.MI_Id && k.ASMAY_Id == data.ASMAY_Id
                                                            && k.ASMCL_Id == data.ASMCL_Id && k.ASMS_Id == data.ASMS_Id && k.ISMS_Id == data.ISMS_Id
                                                            && k.ESEWPT_ActiveFlg == true && k.EME_Id == data.EME_Id && b.MI_Id == data.MI_Id)
                                                            select new MarksEntryHHSDTO
                                                            {
                                                                EMPATY_PaperTypeName = b.EMPATY_PaperTypeName,
                                                                AMST_Id = k.AMST_Id,
                                                                EMPATY_Id = k.EMPATY_Id,
                                                            }).Distinct().ToArray();

                    data.get_papertype_grade_details = (from a in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                                        from b in _examcontext.Exm_Yrly_Cat_Exams_Subwise_PTDMO
                                                        from c in _examcontext.Exm_Master_PaperTypeDMO
                                                        from d in _examcontext.Exm_Master_GradeDMO
                                                        from f in _examcontext.Exm_Master_Grade_DetailsDMO
                                                        where (a.EYCES_Id == b.EYCES_Id && b.EMPATY_Id == c.EMPATY_Id && b.EMGR_Id == d.EMGR_Id
                                                        && d.EMGR_Id == f.EMGR_Id && eyceid.Contains(a.EYCE_Id) && a.ISMS_Id == data.ISMS_Id
                                                        && b.EYCESPT_ActiveFlg == true)
                                                        select new MarksEntryHHSDTO
                                                        {
                                                            EMPATY_Id = b.EMPATY_Id,
                                                            EMGR_Id = b.EMGR_Id,
                                                            EMGR_GradeName = d.EMGR_GradeName,
                                                            EMGD_Name = f.EMGD_Name,
                                                            EMGD_Id = f.EMGD_Id,
                                                        }).Distinct().ToArray();
                }
                else
                {
                    var Countlist = _examcontext.StudentMappingDMO.Where(k => k.MI_Id == data.MI_Id && k.ASMAY_Id == data.ASMAY_Id && k.ASMCL_Id == data.ASMCL_Id
                     && k.ASMS_Id == data.ASMS_Id && k.ISMS_Id == data.ISMS_Id && k.ESTSU_ActiveFlg == true && k.ESTSU_ElecetiveFlag == true).ToList();
                    if (Countlist.Count > 0)
                    {
                        stu_list_mapped = _examcontext.StudentMappingDMO.Where(k => k.MI_Id == data.MI_Id && k.ASMAY_Id == data.ASMAY_Id && k.ASMCL_Id == data.ASMCL_Id
                   && k.ASMS_Id == data.ASMS_Id && k.ISMS_Id == data.ISMS_Id && k.ESTSU_ActiveFlg == true && k.ESTSU_ElecetiveFlag == true).Select(t => t.AMST_Id).Distinct().ToList();
                    }
                    else
                    {
                        stu_list_mapped = _examcontext.StudentMappingDMO.Where(k => k.MI_Id == data.MI_Id && k.ASMAY_Id == data.ASMAY_Id && k.ASMCL_Id == data.ASMCL_Id
                   && k.ASMS_Id == data.ASMS_Id && k.ISMS_Id == data.ISMS_Id && k.ESTSU_ActiveFlg == true).Select(t => t.AMST_Id).Distinct().ToList();

                    }


                }

                List<temp_marks_DTO> studentList = new List<temp_marks_DTO>();

                studentList = (from e in _examcontext.Adm_M_Student
                               from f in _examcontext.School_Adm_Y_StudentDMO
                               from g in _examcontext.IVRM_School_Master_SubjectsDMO
                               from h in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                               from i in subject_details
                               where (e.AMST_Id == f.AMST_Id && f.ASMCL_Id == data.ASMCL_Id && f.ASMS_Id == data.ASMS_Id && f.ASMAY_Id == data.ASMAY_Id
                               && e.MI_Id == data.MI_Id && e.AMST_ActiveFlag == 1 && e.AMST_SOL == "S" && f.AMAY_ActiveFlag == 1 && g.ISMS_Id == h.ISMS_Id
                               && g.MI_Id == data.MI_Id && g.ISMS_ActiveFlag == 1 && g.ISMS_ExamFlag == 1 && g.ISMS_Id == data.ISMS_Id && g.ISMS_Id == data.ISMS_Id
                               && h.EYCE_Id == i.EYCE_Id)
                               select new temp_marks_DTO
                               {
                                   AMST_Id = e.AMST_Id,
                                   AMST_FirstName = ((e.AMST_FirstName == null ? " " : e.AMST_FirstName) + " " + (e.AMST_MiddleName == null ? " " : e.AMST_MiddleName) + " " + (e.AMST_LastName == null ? " " : e.AMST_LastName)).Trim(),
                                   AMST_AdmNo = e.AMST_AdmNo == null ? "" : e.AMST_AdmNo,
                                   AMST_RegistrationNo = e.AMST_RegistrationNo == null ? "" : e.AMST_RegistrationNo,
                                   AMAY_RollNo = f.AMAY_RollNo,
                                   ISMS_Id = g.ISMS_Id,
                                   ISMS_SubjectName = g.ISMS_SubjectName,
                                   EYCES_MaxMarks = h.EYCES_MaxMarks,
                                   EYCES_MinMarks = h.EYCES_MinMarks,
                                   EYCES_MarksEntryMax = h.EYCES_MarksEntryMax,
                                   //  obtainmarks = "0"
                               }).Distinct().OrderBy(t => order).ToList();


                var propertyInfo = typeof(temp_marks_DTO).GetProperty(order);
                studentList = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();

                data.studentList = studentList.Where(t => stu_list_mapped.Contains(t.AMST_Id)).Distinct().ToArray();

                /* SubExam ==false && SubSubject ==false */
                if (alrdy_stu_count > 0 && !data.EYCES_SubSubjectFlg && !data.EYCES_SubExamFlg)
                {
                    var stu_marks = _examcontext.ExamMarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.ESTM_ActiveFlg == true).Distinct().ToList();

                    List<temp_marks_DTO> saved_studentList = new List<temp_marks_DTO>();

                    saved_studentList = (from a in studentList
                                         from b in stu_marks
                                         where (a.AMST_Id == b.AMST_Id && b.ISMS_Id == a.ISMS_Id && stu_list_mapped.Contains(a.AMST_Id))
                                         select new temp_marks_DTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = a.AMST_FirstName,
                                             AMST_AdmNo = a.AMST_AdmNo,
                                             AMST_RegistrationNo = a.AMST_RegistrationNo == null ? "" : a.AMST_RegistrationNo,
                                             AMAY_RollNo = a.AMAY_RollNo,
                                             ISMS_Id = a.ISMS_Id,
                                             ISMS_SubjectName = a.ISMS_SubjectName,
                                             EYCES_MaxMarks = a.EYCES_MaxMarks,
                                             EYCES_MarksEntryMax = a.EYCES_MarksEntryMax,
                                             EYCES_MinMarks = a.EYCES_MinMarks,
                                             ESTM_Marks = b.ESTM_Marks,
                                             ESTM_Grade = b.ESTM_Grade,
                                             ESTM_Flg = b.ESTM_Flg,
                                             ESTM_Id = b.ESTM_Id
                                         }).Distinct().OrderBy(t => order).ToList();

                    var propertyInfo1 = typeof(temp_marks_DTO).GetProperty(order);
                    saved_studentList = saved_studentList.OrderBy(x => propertyInfo1.GetValue(x, null)).ToList();

                    data.saved_studentList = saved_studentList.Distinct().ToArray();
                }

                /* SubExam ==true && SubSubject ==true */
                else if (alrdy_stu_count > 0 && data.EYCES_SubSubjectFlg && data.EYCES_SubExamFlg)
                {
                    var stu_marks = _examcontext.ExamMarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.ESTM_ActiveFlg == true).Distinct().ToList();

                    List<temp_marks_DTO> saved_studentList = new List<temp_marks_DTO>();

                    saved_studentList = (from a in studentList
                                         from b in stu_marks
                                         where (a.AMST_Id == b.AMST_Id && b.ISMS_Id == a.ISMS_Id && stu_list_mapped.Contains(a.AMST_Id))
                                         select new temp_marks_DTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = a.AMST_FirstName,
                                             AMST_AdmNo = a.AMST_AdmNo,
                                             AMST_RegistrationNo = a.AMST_RegistrationNo == null ? "" : a.AMST_RegistrationNo,
                                             AMAY_RollNo = a.AMAY_RollNo,
                                             ISMS_Id = a.ISMS_Id,
                                             ISMS_SubjectName = a.ISMS_SubjectName,
                                             EYCES_MaxMarks = a.EYCES_MaxMarks,
                                             EYCES_MarksEntryMax = a.EYCES_MarksEntryMax,
                                             EYCES_MinMarks = a.EYCES_MinMarks,
                                             ESTM_Marks = b.ESTM_Marks,
                                             ESTM_Grade = b.ESTM_Grade,
                                             ESTM_Flg = b.ESTM_Flg,
                                             ESTM_Id = b.ESTM_Id
                                         }).Distinct().OrderBy(t => order).ToList();

                    var propertyInfo2 = typeof(temp_marks_DTO).GetProperty(order);
                    saved_studentList = saved_studentList.OrderBy(x => propertyInfo2.GetValue(x, null)).ToList();

                    data.saved_studentList = saved_studentList.Distinct().ToArray();

                    data.saved_ssse_list = (from a in _examcontext.Exm_Student_Marks_SubSubjectDMO
                                            from b in stu_marks
                                            where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ESTM_Id == b.ESTM_Id)
                                            select a).Distinct().ToArray();
                }

                /* SubExam ==false && SubSubject ==true */
                else if (alrdy_stu_count > 0 && data.EYCES_SubSubjectFlg && !data.EYCES_SubExamFlg)
                {
                    var stu_marks = _examcontext.ExamMarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.ESTM_ActiveFlg == true).Distinct().ToList();

                    List<temp_marks_DTO> saved_studentList = new List<temp_marks_DTO>();

                    saved_studentList = (from a in studentList
                                         from b in stu_marks
                                         where (a.AMST_Id == b.AMST_Id && b.ISMS_Id == a.ISMS_Id && stu_list_mapped.Contains(a.AMST_Id))
                                         select new temp_marks_DTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = a.AMST_FirstName,
                                             AMST_AdmNo = a.AMST_AdmNo,
                                             AMST_RegistrationNo = a.AMST_RegistrationNo == null ? "" : a.AMST_RegistrationNo,
                                             AMAY_RollNo = a.AMAY_RollNo,
                                             ISMS_Id = a.ISMS_Id,
                                             ISMS_SubjectName = a.ISMS_SubjectName,
                                             EYCES_MaxMarks = a.EYCES_MaxMarks,
                                             EYCES_MarksEntryMax = a.EYCES_MarksEntryMax,
                                             EYCES_MinMarks = a.EYCES_MinMarks,
                                             ESTM_Marks = b.ESTM_Marks,
                                             ESTM_Grade = b.ESTM_Grade,
                                             ESTM_Flg = b.ESTM_Flg,
                                             ESTM_Id = b.ESTM_Id
                                         }).Distinct().OrderBy(t => order).ToList();

                    var propertyInfo3 = typeof(temp_marks_DTO).GetProperty(order);
                    saved_studentList = saved_studentList.OrderBy(x => propertyInfo3.GetValue(x, null)).ToList();

                    data.saved_studentList = saved_studentList.Distinct().ToArray();

                    data.saved_ss_list = (from a in _examcontext.Exm_Student_Marks_SubSubjectDMO
                                          from b in stu_marks
                                          where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ESTM_Id == b.ESTM_Id)
                                          select a).Distinct().ToArray();
                }

                /* SubExam ==true */
                else if (alrdy_stu_count > 0 && data.EYCES_SubExamFlg)
                {
                    var stu_marks = _examcontext.ExamMarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.ESTM_ActiveFlg == true).Distinct().ToList();

                    List<temp_marks_DTO> saved_studentList = new List<temp_marks_DTO>();

                    saved_studentList = (from a in studentList
                                         from b in stu_marks
                                         where (a.AMST_Id == b.AMST_Id && b.ISMS_Id == a.ISMS_Id && stu_list_mapped.Contains(a.AMST_Id))
                                         select new temp_marks_DTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = a.AMST_FirstName,
                                             AMST_AdmNo = a.AMST_AdmNo,
                                             AMST_RegistrationNo = a.AMST_RegistrationNo == null ? "" : a.AMST_RegistrationNo,
                                             AMAY_RollNo = a.AMAY_RollNo,
                                             ISMS_Id = a.ISMS_Id,
                                             ISMS_SubjectName = a.ISMS_SubjectName,
                                             EYCES_MaxMarks = a.EYCES_MaxMarks,
                                             EYCES_MarksEntryMax = a.EYCES_MarksEntryMax,
                                             EYCES_MinMarks = a.EYCES_MinMarks,
                                             ESTM_Marks = b.ESTM_Marks,
                                             ESTM_Grade = b.ESTM_Grade,
                                             ESTM_Flg = b.ESTM_Flg,
                                             ESTM_Id = b.ESTM_Id
                                         }).Distinct().OrderBy(t => order).ToList();

                    var propertyInfo3 = typeof(temp_marks_DTO).GetProperty(order);
                    saved_studentList = saved_studentList.OrderBy(x => propertyInfo3.GetValue(x, null)).ToList();


                    data.saved_studentList = saved_studentList.Distinct().ToArray();

                    data.saved_se_list = (from a in _examcontext.Exm_Student_Marks_SubSubjectDMO
                                          from b in stu_marks
                                          where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ESTM_Id == b.ESTM_Id)
                                          select a).Distinct().ToArray();
                }

                //
              
            }
            catch (Exception ee)
            {
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
                Console.WriteLine(ee.Message);
            }

            try
            {
                if (data.EYCES_SubExamFlg == false && data.EYCES_SubSubjectFlg == false)
                {
                    var checkmarkssaved = _examcontext.ExamMarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                    && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.ISMS_Id == data.ISMS_Id && a.ESTM_ActiveFlg == true).ToList();
                    try
                    {
                        data.saveupdatecount = checkmarkssaved.Count();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else if (data.EYCES_SubExamFlg == true)
                {
                    List<long> getsubsubjectid = new List<long>();

                    var getemssid = (from a in _examcontext.Exm_Login_PrivilegeDMO
                                     from b in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                     from c in _examcontext.Exm_Login_Privilege_SubSubjectsDMO
                                     from g in _examcontext.Staff_User_Login
                                     where (a.ELP_Id == b.ELP_Id && g.IVRMSTAUL_Id == a.Login_Id && b.ELPS_Id == c.ELPS_Id && a.ELP_ActiveFlg == true
                                     && b.ELPs_ActiveFlg == true && c.ELPSS_ActiveFlg == true && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                                     && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && b.ISMS_Id == data.ISMS_Id && g.Id == data.UserId)
                                     select new MarksEntryHHSDTO
                                     {
                                         EMSS_Id = c.EMSS_Id

                                     }).Distinct().ToList();

                    foreach (var c in getemssid)
                    {
                        getsubsubjectid.Add(c.EMSS_Id);
                    }

                    var checkmarks = (from a in _examcontext.ExamMarksDMO
                                      from b in _examcontext.Exm_Student_Marks_SubSubjectDMO
                                      where (a.ESTM_Id == b.ESTM_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                      && a.EME_Id == data.EME_Id && a.ISMS_Id == data.ISMS_Id && b.ISMS_Id == data.ISMS_Id && a.MI_Id == data.MI_Id
                                      && getsubsubjectid.Contains(b.EMSS_Id) && a.ESTM_ActiveFlg == true)
                                      select a).Distinct().ToList();

                    data.saveupdatecount = checkmarks.Count();
                }
                else
                {
                    var checkmarkssaved = _examcontext.ExamMarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                  && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.ISMS_Id == data.ISMS_Id && a.ESTM_ActiveFlg == true).ToList();
                    try
                    {
                        data.saveupdatecount = checkmarkssaved.Count();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                var checklastdateisnullornot = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYCE_ActiveFlg == true && eyceid.Contains(t.EYCE_Id)).ToList();

                if (checklastdateisnullornot.FirstOrDefault().EYCE_MarksEntryLastDate != null)
                {
                    var eyceidlastdateentry = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYCE_ActiveFlg == true && eyceid.Contains(t.EYCE_Id)
                    && Convert.ToDateTime(System.DateTime.Today.Date) <= Convert.ToDateTime(t.EYCE_MarksEntryLastDate)).Distinct().ToList();

                    data.lastdateentry = eyceidlastdateentry.Count();

                    if (eyceidlastdateentry.Count() == 0)
                    {
                        // Checking The Login User Having Authority To Enter The Marks

                        var getstaffid = _examcontext.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();

                        var checkspecialuser = _examcontext.UserPromotion_DMO.Where(a => eyceid.Contains(a.EYCE_Id) && a.IVRMUL_Id == data.UserId).ToList();

                        if (checkspecialuser.Count() > 0)
                        {
                            var eyceidlastdateentryuser = _examcontext.UserPromotion_DMO.Where(t => t.EYCESU_ActiveFlg == true
                            && eyceid.Contains(t.EYCE_Id) && t.IVRMUL_Id == data.UserId
                            && Convert.ToDateTime(System.DateTime.Today.Date) >= Convert.ToDateTime(t.EYCESU_MarksEntryFromDate)
                            && Convert.ToDateTime(System.DateTime.Today.Date) <= Convert.ToDateTime(t.EYCESU_MarksEntryToDate)).Distinct().ToList();

                            data.lastdateentry = eyceidlastdateentryuser.Count();


                            if (eyceidlastdateentryuser.Count() > 0)
                            {
                                data.lastdateentry = 1000;
                                data.lastdateentryflag = true;
                            }
                            else
                            {
                                data.lastdateentry = 0;
                                data.lastdateentryflag = false;
                            }
                        }
                        else
                        {
                            data.lastdateentry = 0;
                            data.lastdateentryflag = false;
                        }
                    }
                    else
                    {
                        data.lastdateentry = 1000;
                        data.lastdateentryflag = true;
                    }
                }
                else
                {
                    data.lastdateentry = 1000;
                    data.lastdateentryflag = true;
                }

                if (checklastdateisnullornot.FirstOrDefault().EYCE_ExamEndDate != null)
                {
                    var eyceidexamlastdateentry = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYCE_ActiveFlg == true
                    && eyceid.Contains(t.EYCE_Id) && Convert.ToDateTime(System.DateTime.Today.Date) >= Convert.ToDateTime(t.EYCE_ExamEndDate)).Distinct().ToList();

                    if (eyceidexamlastdateentry.Count() > 0)
                    {
                        data.lastdateexam = eyceidexamlastdateentry.Count();
                        data.lastdateexamflag = true;
                    }
                    else
                    {
                        var examlastdateentry = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYCE_ActiveFlg == true
                        && eyceid.Contains(t.EYCE_Id)).Distinct().ToList();

                        data.lastdateexam = 0;
                        data.lastdateexamflag = false;
                        data.marksentrystatedate = examlastdateentry.FirstOrDefault().EYCE_ExamEndDate;
                    }
                }
                else
                {
                    data.lastdateexam = 2000;
                    data.lastdateexamflag = true;
                }
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())                {
                    //cmd.CommandText = "Exam_get_Marks_Entry_Modify";
                    cmd.CommandText = "Exam_Duplicate_MarksEntry_Delete";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)                                {                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);                                }                                retObject.Add((ExpandoObject)dataRow1);                            }                        }

                    }                    catch (Exception ex)                    {                        _acdimpl.LogError(ex.Message);                        _acdimpl.LogDebug(ex.Message);                    }                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public MarksEntryHHSDTO SaveMarks(MarksEntryHHSDTO data)
        {
            if (!data.EYCES_SubSubjectFlg && !data.EYCES_SubExamFlg)
            {
                try
                {
                    for (int i = 0; i < data.main_save_list.Length; i++)
                    {
                        var stu_id = data.main_save_list[i].AMST_Id;
                        var already_cnt = _examcontext.ExamMarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.AMST_Id == stu_id && t.ESTM_ActiveFlg == true).Count();
                        if (already_cnt == 0)
                        {
                            ExamMarksDMO obj_M = new ExamMarksDMO();
                            obj_M.MI_Id = data.MI_Id;
                            obj_M.ASMAY_Id = data.ASMAY_Id;
                            obj_M.ASMCL_Id = data.ASMCL_Id;
                            obj_M.ASMS_Id = data.ASMS_Id;
                            obj_M.EME_Id = data.EME_Id;
                            obj_M.ISMS_Id = data.ISMS_Id;
                            obj_M.AMST_Id = stu_id;
                            obj_M.ESTM_Marks = data.main_save_list[i].ESTM_Marks;
                            obj_M.ESTM_MarksGradeFlg = data.EYCES_MarksGradeEntryFlg;
                            obj_M.Id = data.UserId;
                            obj_M.LoginDateTime = DateTime.Now;
                            obj_M.IP4 = data.IP4;
                            obj_M.CreatedDate = DateTime.Now;
                            obj_M.UpdatedDate = DateTime.Now;
                            obj_M.ESTM_ActiveFlg = true;
                            obj_M.ESTM_OnlineExamFlag = false;
                            obj_M.ESTM_Grade = data.main_save_list[i].ESTM_Grade;
                            obj_M.ESTM_Flg = data.main_save_list[i].ESTM_Flg;
                            obj_M.ESTM_CreatedBy = data.UserId;
                            obj_M.ESTM_UpdatedBy = data.UserId;
                            _examcontext.Add(obj_M);
                        }
                        else if (already_cnt == 1)
                        {
                            var result_obj = _examcontext.ExamMarksDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.AMST_Id == stu_id && t.ESTM_ActiveFlg == true);
                            result_obj.ESTM_Marks = data.main_save_list[i].ESTM_Marks;
                            result_obj.ESTM_MarksGradeFlg = data.EYCES_MarksGradeEntryFlg;
                            result_obj.Id = data.UserId;
                            result_obj.LoginDateTime = DateTime.Now;
                            result_obj.IP4 = data.IP4;
                            result_obj.ESTM_UpdatedBy = data.UserId;
                            result_obj.UpdatedDate = DateTime.Now;
                            result_obj.ESTM_ActiveFlg = true;
                            result_obj.ESTM_Grade = data.main_save_list[i].ESTM_Grade;
                            result_obj.ESTM_Flg = data.main_save_list[i].ESTM_Flg;
                            _examcontext.Update(result_obj);
                        }

                    }
                    var contactExists = _examcontext.SaveChanges();
                    if (contactExists >= 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }


                }
                catch (Exception ee)
                {
                    _acdimpl.LogError(ee.Message);
                    _acdimpl.LogDebug(ee.Message);
                    Console.WriteLine(ee.Message);
                }
            }

            else if (data.EYCES_SubSubjectFlg && data.EYCES_SubExamFlg)
            {
                try
                {
                    for (int i = 0; i < data.main_save_list.Length; i++)
                    {
                        var stu_id = data.main_save_list[i].AMST_Id;

                        var already_cnt = _examcontext.ExamMarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.AMST_Id == stu_id && t.ESTM_ActiveFlg == true).Count();

                        if (already_cnt == 0)
                        {
                            ExamMarksDMO obj_M = new ExamMarksDMO();
                            obj_M.MI_Id = data.MI_Id;
                            obj_M.ASMAY_Id = data.ASMAY_Id;
                            obj_M.ASMCL_Id = data.ASMCL_Id;
                            obj_M.ASMS_Id = data.ASMS_Id;
                            obj_M.EME_Id = data.EME_Id;
                            obj_M.ISMS_Id = data.ISMS_Id;
                            obj_M.AMST_Id = stu_id;
                            obj_M.ESTM_Marks = data.main_save_list[i].ESTM_Marks;
                            obj_M.ESTM_MarksGradeFlg = data.EYCES_MarksGradeEntryFlg;
                            obj_M.Id = data.UserId;
                            obj_M.ESTM_CreatedBy = data.UserId;
                            obj_M.ESTM_UpdatedBy = data.UserId;
                            obj_M.LoginDateTime = DateTime.Now;
                            obj_M.IP4 = data.IP4;
                            obj_M.CreatedDate = DateTime.Now;
                            obj_M.UpdatedDate = DateTime.Now;
                            obj_M.ESTM_ActiveFlg = true;
                            obj_M.ESTM_OnlineExamFlag = false;
                            obj_M.ESTM_Grade = data.main_save_list[i].ESTM_Grade;
                            obj_M.ESTM_Flg = data.main_save_list[i].ESTM_Flg;
                            _examcontext.Add(obj_M);

                            for (int j = 0; j < data.Temp_subs_marks_list.Length; j++)
                            {
                                var sub_id = data.Temp_subs_marks_list[j].ISMS_Id;
                                if (data.ISMS_Id == sub_id && stu_id == data.Temp_subs_marks_list[j].AMST_Id)
                                {
                                    Exm_Student_Marks_SubSubjectDMO obj_S = new Exm_Student_Marks_SubSubjectDMO();
                                    obj_S.MI_Id = data.MI_Id;
                                    obj_S.ESTM_Id = obj_M.ESTM_Id;
                                    obj_S.EMSS_Id = data.Temp_subs_marks_list[j].EMSS_Id;
                                    obj_S.ISMS_Id = sub_id;
                                    obj_S.EMSE_Id = data.Temp_subs_marks_list[j].EMSE_Id;
                                    obj_S.ESTMSS_Marks = data.Temp_subs_marks_list[j].ESTMSS_Marks;
                                    obj_S.ESTMSS_MarksGradeFlg = data.Temp_subs_marks_list[j].ESTMSS_MarksGradeFlg;
                                    obj_S.ESTMSS_Grade = data.Temp_subs_marks_list[j].ESTMSS_Grade;
                                    obj_S.Login_Id = data.UserId;
                                    obj_S.ESTMSS_CreatedBy = data.UserId;
                                    obj_S.ESTMSS_UpdatedBy = data.UserId;
                                    obj_S.LoginDateTime = DateTime.Now;
                                    obj_S.IP4 = data.IP4;
                                    obj_S.ESTMSS_ActiveFlg = true;
                                    obj_S.ESTMSS_Flg = data.Temp_subs_marks_list[j].ESTMSS_Flg;
                                    obj_S.CreatedDate = DateTime.Now;
                                    obj_S.UpdatedDate = DateTime.Now;
                                    _examcontext.Add(obj_S);
                                }
                            }
                        }
                        else if (already_cnt == 1)
                        {
                            decimal? marks = 0;
                            List<long> emssid = new List<long>();
                            List<long> emseid = new List<long>();

                            List<long> emssidnew = new List<long>();
                            List<long> emseidnew = new List<long>();

                            for (int j1 = 0; j1 < data.Temp_subs_marks_list.Length; j1++)
                            {
                                var sub_idnew = data.Temp_subs_marks_list[j1].ISMS_Id;

                                if (data.ISMS_Id == sub_idnew && stu_id == data.Temp_subs_marks_list[j1].AMST_Id)
                                {
                                    if (emssidnew.Count > 0)
                                    {
                                        int count = 0;
                                        for (int k = 0; k < emssidnew.Count; k++)
                                        {
                                            if (emssidnew[k] == data.Temp_subs_marks_list[j1].EMSS_Id)
                                            {
                                                count += 1;
                                            }
                                        }
                                        if (count == 0)
                                        {
                                            emssidnew.Add(data.Temp_subs_marks_list[j1].EMSS_Id);
                                        }
                                    }
                                    else
                                    {
                                        emssidnew.Add(data.Temp_subs_marks_list[j1].EMSS_Id);
                                    }

                                }
                                if (data.ISMS_Id == sub_idnew && stu_id == data.Temp_subs_marks_list[j1].AMST_Id)
                                {
                                    if (emseidnew.Count > 0)
                                    {
                                        int count = 0;
                                        for (int k = 0; k < emseidnew.Count; k++)
                                        {
                                            if (emseidnew[k] == data.Temp_subs_marks_list[j1].EMSE_Id)
                                            {
                                                count += 1;
                                            }
                                        }
                                        if (count == 0)
                                        {
                                            emseidnew.Add(data.Temp_subs_marks_list[j1].EMSE_Id);
                                        }
                                    }
                                    else
                                    {
                                        emseidnew.Add(data.Temp_subs_marks_list[j1].EMSE_Id);
                                    }
                                }
                            }

                            var getsubsubjectsubexammarks = (from a in _examcontext.Exm_Student_Marks_SubSubjectDMO
                                                             from b in _examcontext.ExamMarksDMO
                                                             where (a.ESTM_Id == b.ESTM_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                                             && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && b.ISMS_Id == data.ISMS_Id
                                                             && b.AMST_Id == stu_id && (!emseidnew.Contains(a.EMSE_Id) || !emssidnew.Contains(a.EMSS_Id))
                                                             && b.EME_Id == data.EME_Id && b.ESTM_ActiveFlg == true)

                                                             select new Exm_Student_Marks_SubSubjectDMO
                                                             {
                                                                 ESTMSS_Marks = a.ESTMSS_Marks

                                                             }).ToList();


                            foreach (var m in getsubsubjectsubexammarks)
                            {
                                marks = marks + m.ESTMSS_Marks;
                            }


                            var result_obj = _examcontext.ExamMarksDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id
                            && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.AMST_Id == stu_id && t.ESTM_ActiveFlg == true);

                            result_obj.ESTM_Marks = data.main_save_list[i].ESTM_Marks + Convert.ToDecimal(marks);
                            result_obj.ESTM_MarksGradeFlg = data.EYCES_MarksGradeEntryFlg;
                            result_obj.Id = data.UserId;
                            result_obj.ESTM_UpdatedBy = data.UserId;
                            result_obj.LoginDateTime = DateTime.Now;
                            result_obj.IP4 = data.IP4;
                            result_obj.UpdatedDate = DateTime.Now;
                            result_obj.ESTM_ActiveFlg = true;
                            result_obj.ESTM_Grade = data.main_save_list[i].ESTM_Grade;
                            result_obj.ESTM_Flg = data.main_save_list[i].ESTM_Flg;
                            _examcontext.Update(result_obj);

                            for (int j = 0; j < data.Temp_subs_marks_list.Length; j++)
                            {
                                var sub_id = data.Temp_subs_marks_list[j].ISMS_Id;
                                if (data.ISMS_Id == sub_id && stu_id == data.Temp_subs_marks_list[j].AMST_Id)
                                {
                                    var checkresult = _examcontext.Exm_Student_Marks_SubSubjectDMO.Where(t => t.MI_Id == data.MI_Id
                                    && t.ESTM_Id == result_obj.ESTM_Id && t.EMSS_Id == data.Temp_subs_marks_list[j].EMSS_Id
                                    && t.EMSE_Id == data.Temp_subs_marks_list[j].EMSE_Id).ToList();

                                    if (checkresult.Count() > 0)
                                    {
                                        var checkresultnew = _examcontext.Exm_Student_Marks_SubSubjectDMO.Single(t => t.MI_Id == data.MI_Id
                                     && t.ESTM_Id == result_obj.ESTM_Id && t.EMSS_Id == data.Temp_subs_marks_list[j].EMSS_Id
                                     && t.EMSE_Id == data.Temp_subs_marks_list[j].EMSE_Id);

                                        checkresultnew.ESTMSS_Marks = data.Temp_subs_marks_list[j].ESTMSS_Marks;
                                        checkresultnew.ESTMSS_MarksGradeFlg = data.Temp_subs_marks_list[j].ESTMSS_MarksGradeFlg;
                                        checkresultnew.ESTMSS_Grade = data.Temp_subs_marks_list[j].ESTMSS_Grade;
                                        checkresultnew.Login_Id = data.UserId;
                                        checkresultnew.ESTMSS_UpdatedBy = data.UserId;
                                        checkresultnew.LoginDateTime = DateTime.Now;
                                        checkresultnew.IP4 = data.IP4;
                                        checkresultnew.ESTMSS_ActiveFlg = true;
                                        checkresultnew.ESTMSS_Flg = data.Temp_subs_marks_list[j].ESTMSS_Flg;
                                        checkresultnew.UpdatedDate = DateTime.UtcNow;
                                        _examcontext.Update(checkresultnew);

                                    }
                                    else
                                    {
                                        Exm_Student_Marks_SubSubjectDMO obj_S = new Exm_Student_Marks_SubSubjectDMO();
                                        obj_S.MI_Id = data.MI_Id;
                                        obj_S.ESTM_Id = result_obj.ESTM_Id;
                                        obj_S.EMSS_Id = data.Temp_subs_marks_list[j].EMSS_Id;
                                        obj_S.ISMS_Id = sub_id;
                                        obj_S.EMSE_Id = data.Temp_subs_marks_list[j].EMSE_Id;
                                        obj_S.ESTMSS_Marks = data.Temp_subs_marks_list[j].ESTMSS_Marks;
                                        obj_S.ESTMSS_MarksGradeFlg = data.Temp_subs_marks_list[j].ESTMSS_MarksGradeFlg;
                                        obj_S.ESTMSS_Grade = data.Temp_subs_marks_list[j].ESTMSS_Grade;
                                        obj_S.Login_Id = data.UserId;
                                        obj_S.ESTMSS_CreatedBy = data.UserId;
                                        obj_S.ESTMSS_UpdatedBy = data.UserId;
                                        obj_S.LoginDateTime = DateTime.Now;
                                        obj_S.IP4 = data.IP4;
                                        obj_S.ESTMSS_ActiveFlg = true;
                                        obj_S.ESTMSS_Flg = data.Temp_subs_marks_list[j].ESTMSS_Flg;
                                        obj_S.CreatedDate = result_obj.CreatedDate;
                                        obj_S.UpdatedDate = DateTime.Now;
                                        _examcontext.Add(obj_S);
                                    }
                                }
                            }
                        }
                    }

                    var contactExists = _examcontext.SaveChanges();
                    if (contactExists >= 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                catch (Exception ee)
                {
                    _acdimpl.LogError(ee.Message);
                    _acdimpl.LogDebug(ee.Message);
                    Console.WriteLine(ee.Message);
                }
            }

            else if (data.EYCES_SubSubjectFlg && !data.EYCES_SubExamFlg)
            {
                try
                {
                    for (int i = 0; i < data.main_save_list.Length; i++)
                    {
                        var stu_id = data.main_save_list[i].AMST_Id;
                        var already_cnt = _examcontext.ExamMarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.AMST_Id == stu_id && t.ESTM_ActiveFlg == true).Count();
                        if (already_cnt == 0)
                        {
                            ExamMarksDMO obj_M = new ExamMarksDMO();
                            obj_M.MI_Id = data.MI_Id;
                            obj_M.ASMAY_Id = data.ASMAY_Id;
                            obj_M.ASMCL_Id = data.ASMCL_Id;
                            obj_M.ASMS_Id = data.ASMS_Id;
                            obj_M.EME_Id = data.EME_Id;
                            obj_M.ISMS_Id = data.ISMS_Id;
                            obj_M.AMST_Id = stu_id;
                            obj_M.ESTM_Marks = data.main_save_list[i].ESTM_Marks;
                            obj_M.ESTM_MarksGradeFlg = data.EYCES_MarksGradeEntryFlg;
                            obj_M.Id = data.UserId;
                            obj_M.ESTM_CreatedBy = data.UserId;
                            obj_M.ESTM_UpdatedBy = data.UserId;
                            obj_M.LoginDateTime = DateTime.Now;
                            obj_M.IP4 = data.IP4;
                            obj_M.CreatedDate = DateTime.Now;
                            obj_M.UpdatedDate = DateTime.Now;
                            obj_M.ESTM_ActiveFlg = true;
                            obj_M.ESTM_OnlineExamFlag = false;
                            obj_M.ESTM_Grade = data.main_save_list[i].ESTM_Grade;
                            obj_M.ESTM_Flg = data.main_save_list[i].ESTM_Flg;
                            _examcontext.Add(obj_M);
                            for (int j = 0; j < data.Temp_subs_marks_list.Length; j++)
                            {
                                var sub_id = data.Temp_subs_marks_list[j].ISMS_Id;
                                if (data.ISMS_Id == sub_id && stu_id == data.Temp_subs_marks_list[j].AMST_Id)
                                {
                                    Exm_Student_Marks_SubSubjectDMO obj_S = new Exm_Student_Marks_SubSubjectDMO();
                                    obj_S.MI_Id = data.MI_Id;
                                    obj_S.ESTM_Id = obj_M.ESTM_Id;
                                    obj_S.EMSS_Id = data.Temp_subs_marks_list[j].EMSS_Id;
                                    obj_S.ISMS_Id = sub_id;
                                    obj_S.EMSE_Id = data.Temp_subs_marks_list[j].EMSE_Id;
                                    obj_S.ESTMSS_Marks = data.Temp_subs_marks_list[j].ESTMSS_Marks;
                                    obj_S.ESTMSS_MarksGradeFlg = data.Temp_subs_marks_list[j].ESTMSS_MarksGradeFlg;
                                    obj_S.ESTMSS_Grade = data.Temp_subs_marks_list[j].ESTMSS_Grade;
                                    obj_S.Login_Id = data.UserId;
                                    obj_S.ESTMSS_CreatedBy = data.UserId;
                                    obj_S.ESTMSS_UpdatedBy = data.UserId;
                                    obj_S.LoginDateTime = DateTime.Now;
                                    obj_S.IP4 = data.IP4;
                                    obj_S.ESTMSS_ActiveFlg = true;
                                    obj_S.ESTMSS_Flg = data.Temp_subs_marks_list[j].ESTMSS_Flg;
                                    obj_S.CreatedDate = DateTime.Now;
                                    obj_S.UpdatedDate = DateTime.Now;
                                    _examcontext.Add(obj_S);
                                }
                            }
                        }

                        else if (already_cnt == 1)
                        {
                            //var child_list = _examcontext.Exm_Student_Marks_SubSubjectDMO.Where(t => t.MI_Id == data.MI_Id && t.ESTM_Id == result_obj.ESTM_Id).ToList();
                            //if (child_list.Any())
                            //{
                            //    for (int l = 0; child_list.Count > l; l++)
                            //    {
                            //        _examcontext.Remove(child_list.ElementAt(l));
                            //    }
                            //}

                            decimal? marks = 0;
                            List<long> emssid = new List<long>();
                            List<long> emseid = new List<long>();

                            List<long> emssidnew = new List<long>();
                            List<long> emseidnew = new List<long>();

                            for (int j1 = 0; j1 < data.Temp_subs_marks_list.Length; j1++)
                            {
                                var sub_idnew = data.Temp_subs_marks_list[j1].ISMS_Id;

                                if (data.ISMS_Id == sub_idnew && stu_id == data.Temp_subs_marks_list[j1].AMST_Id)
                                {
                                    if (emssidnew.Count > 0)
                                    {
                                        int count = 0;
                                        for (int k = 0; k < emssidnew.Count; k++)
                                        {
                                            if (emssidnew[k] == data.Temp_subs_marks_list[j1].EMSS_Id)
                                            {
                                                count += 1;
                                            }
                                        }
                                        if (count == 0)
                                        {
                                            emssidnew.Add(data.Temp_subs_marks_list[j1].EMSS_Id);
                                        }
                                    }
                                    else
                                    {
                                        emssidnew.Add(data.Temp_subs_marks_list[j1].EMSS_Id);
                                    }
                                }
                            }

                            var getsubsubjectsubexammarks = (from a in _examcontext.Exm_Student_Marks_SubSubjectDMO
                                                             from b in _examcontext.ExamMarksDMO
                                                             where (a.ESTM_Id == b.ESTM_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                                             && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && b.ISMS_Id == data.ISMS_Id
                                                             && b.AMST_Id == stu_id && (!emssidnew.Contains(a.EMSS_Id))
                                                             && b.EME_Id == data.EME_Id && b.ESTM_ActiveFlg == true)

                                                             select new Exm_Student_Marks_SubSubjectDMO
                                                             {
                                                                 ESTMSS_Marks = a.ESTMSS_Marks
                                                             }).ToList();


                            foreach (var m in getsubsubjectsubexammarks)
                            {
                                marks = marks + m.ESTMSS_Marks;
                            }

                            var result_obj = _examcontext.ExamMarksDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.AMST_Id == stu_id && t.ESTM_ActiveFlg == true);

                            result_obj.ESTM_Marks = data.main_save_list[i].ESTM_Marks + Convert.ToDecimal(marks);
                            result_obj.ESTM_MarksGradeFlg = data.EYCES_MarksGradeEntryFlg;
                            result_obj.Id = data.UserId;
                            result_obj.ESTM_UpdatedBy = data.UserId;
                            result_obj.LoginDateTime = DateTime.Now;
                            result_obj.IP4 = data.IP4;
                            result_obj.UpdatedDate = DateTime.Now;
                            result_obj.ESTM_ActiveFlg = true;
                            result_obj.ESTM_Grade = data.main_save_list[i].ESTM_Grade;
                            result_obj.ESTM_Flg = data.main_save_list[i].ESTM_Flg;
                            _examcontext.Update(result_obj);


                            for (int j = 0; j < data.Temp_subs_marks_list.Length; j++)
                            {
                                var sub_id = data.Temp_subs_marks_list[j].ISMS_Id;
                                if (data.ISMS_Id == sub_id && stu_id == data.Temp_subs_marks_list[j].AMST_Id)
                                {

                                    var checkresult = _examcontext.Exm_Student_Marks_SubSubjectDMO.Where(t => t.MI_Id == data.MI_Id
                                    && t.ESTM_Id == result_obj.ESTM_Id && t.EMSS_Id == data.Temp_subs_marks_list[j].EMSS_Id).ToList();

                                    if (checkresult.Count() > 0)
                                    {
                                        var checkresultnew = _examcontext.Exm_Student_Marks_SubSubjectDMO.Single(t => t.MI_Id == data.MI_Id
                                     && t.ESTM_Id == result_obj.ESTM_Id && t.EMSS_Id == data.Temp_subs_marks_list[j].EMSS_Id);

                                        checkresultnew.ESTMSS_Marks = data.Temp_subs_marks_list[j].ESTMSS_Marks;
                                        checkresultnew.ESTMSS_MarksGradeFlg = data.Temp_subs_marks_list[j].ESTMSS_MarksGradeFlg;
                                        checkresultnew.ESTMSS_Grade = data.Temp_subs_marks_list[j].ESTMSS_Grade;
                                        checkresultnew.Login_Id = data.UserId;
                                        checkresultnew.ESTMSS_UpdatedBy = data.UserId;
                                        checkresultnew.LoginDateTime = DateTime.Now;
                                        checkresultnew.IP4 = data.IP4;
                                        checkresultnew.ESTMSS_ActiveFlg = true;
                                        checkresultnew.ESTMSS_Flg = data.Temp_subs_marks_list[j].ESTMSS_Flg;
                                        checkresultnew.UpdatedDate = DateTime.UtcNow;
                                        _examcontext.Update(checkresultnew);

                                    }
                                    else
                                    {
                                        Exm_Student_Marks_SubSubjectDMO obj_S = new Exm_Student_Marks_SubSubjectDMO();
                                        obj_S.MI_Id = data.MI_Id;
                                        obj_S.ESTM_Id = result_obj.ESTM_Id;
                                        obj_S.EMSS_Id = data.Temp_subs_marks_list[j].EMSS_Id;
                                        obj_S.ISMS_Id = sub_id;
                                        obj_S.EMSE_Id = data.Temp_subs_marks_list[j].EMSE_Id;
                                        obj_S.ESTMSS_Marks = data.Temp_subs_marks_list[j].ESTMSS_Marks;
                                        obj_S.ESTMSS_MarksGradeFlg = data.Temp_subs_marks_list[j].ESTMSS_MarksGradeFlg;
                                        obj_S.ESTMSS_Grade = data.Temp_subs_marks_list[j].ESTMSS_Grade;
                                        obj_S.Login_Id = data.UserId;
                                        obj_S.ESTMSS_CreatedBy = data.UserId;
                                        obj_S.ESTMSS_UpdatedBy = data.UserId;
                                        obj_S.LoginDateTime = DateTime.Now;
                                        obj_S.IP4 = data.IP4;
                                        obj_S.ESTMSS_ActiveFlg = true;
                                        obj_S.ESTMSS_Flg = data.Temp_subs_marks_list[j].ESTMSS_Flg;
                                        obj_S.CreatedDate = result_obj.CreatedDate;
                                        obj_S.UpdatedDate = DateTime.Now;
                                        _examcontext.Add(obj_S);
                                    }
                                }
                            }
                        }
                    }
                    var contactExists = _examcontext.SaveChanges();
                    if (contactExists >= 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                catch (Exception ee)
                {
                    _acdimpl.LogError(ee.Message);
                    _acdimpl.LogDebug(ee.Message);
                    Console.WriteLine(ee.Message);
                }
            }

            else if (!data.EYCES_SubSubjectFlg && data.EYCES_SubExamFlg)
            {
                try
                {
                    for (int i = 0; i < data.main_save_list.Length; i++)
                    {
                        var stu_id = data.main_save_list[i].AMST_Id;
                        var already_cnt = _examcontext.ExamMarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.AMST_Id == stu_id && t.ESTM_ActiveFlg == true).Count();
                        if (already_cnt == 0)
                        {
                            ExamMarksDMO obj_M = new ExamMarksDMO();
                            obj_M.MI_Id = data.MI_Id;
                            obj_M.ASMAY_Id = data.ASMAY_Id;
                            obj_M.ASMCL_Id = data.ASMCL_Id;
                            obj_M.ASMS_Id = data.ASMS_Id;
                            obj_M.EME_Id = data.EME_Id;
                            obj_M.ISMS_Id = data.ISMS_Id;
                            obj_M.AMST_Id = stu_id;
                            obj_M.ESTM_Marks = data.main_save_list[i].ESTM_Marks;
                            obj_M.ESTM_MarksGradeFlg = data.EYCES_MarksGradeEntryFlg;
                            obj_M.Id = data.UserId;
                            obj_M.ESTM_CreatedBy = data.UserId;
                            obj_M.ESTM_UpdatedBy = data.UserId;
                            obj_M.LoginDateTime = DateTime.Now;
                            obj_M.IP4 = data.IP4;
                            obj_M.CreatedDate = DateTime.Now;
                            obj_M.UpdatedDate = DateTime.Now;
                            obj_M.ESTM_ActiveFlg = true;
                            obj_M.ESTM_OnlineExamFlag = false;
                            obj_M.ESTM_Grade = data.main_save_list[i].ESTM_Grade;
                            obj_M.ESTM_Flg = data.main_save_list[i].ESTM_Flg;
                            _examcontext.Add(obj_M);
                            for (int j = 0; j < data.Temp_subs_marks_list.Length; j++)
                            {
                                var sub_id = data.Temp_subs_marks_list[j].ISMS_Id;
                                if (data.ISMS_Id == sub_id && stu_id == data.Temp_subs_marks_list[j].AMST_Id)
                                {
                                    Exm_Student_Marks_SubSubjectDMO obj_S = new Exm_Student_Marks_SubSubjectDMO();
                                    obj_S.MI_Id = data.MI_Id;
                                    obj_S.ESTM_Id = obj_M.ESTM_Id;
                                    obj_S.EMSS_Id = data.Temp_subs_marks_list[j].EMSS_Id;
                                    obj_S.ISMS_Id = sub_id;
                                    obj_S.EMSE_Id = data.Temp_subs_marks_list[j].EMSE_Id;
                                    obj_S.ESTMSS_Marks = data.Temp_subs_marks_list[j].ESTMSS_Marks;
                                    obj_S.ESTMSS_MarksGradeFlg = data.Temp_subs_marks_list[j].ESTMSS_MarksGradeFlg;
                                    obj_S.ESTMSS_Grade = data.Temp_subs_marks_list[j].ESTMSS_Grade;
                                    obj_S.Login_Id = data.UserId;
                                    obj_S.ESTMSS_CreatedBy = data.UserId;
                                    obj_S.ESTMSS_UpdatedBy = data.UserId;
                                    obj_S.Login_Id = data.UserId;
                                    obj_S.LoginDateTime = DateTime.Now;
                                    obj_S.IP4 = data.IP4;
                                    obj_S.ESTMSS_ActiveFlg = true;
                                    obj_S.ESTMSS_Flg = data.Temp_subs_marks_list[j].ESTMSS_Flg;
                                    obj_S.CreatedDate = DateTime.Now;
                                    obj_S.UpdatedDate = DateTime.Now;
                                    _examcontext.Add(obj_S);
                                }
                            }
                        }

                        else if (already_cnt == 1)
                        {
                            //var child_list = _examcontext.Exm_Student_Marks_SubSubjectDMO.Where(t => t.MI_Id == data.MI_Id && t.ESTM_Id == result_obj.ESTM_Id).ToList();
                            //if (child_list.Any())
                            //{
                            //    for (int l = 0; child_list.Count > l; l++)
                            //    {
                            //        _examcontext.Remove(child_list.ElementAt(l));
                            //    }
                            //}

                            decimal? marks = 0;
                            List<long> emssid = new List<long>();
                            List<long> emseid = new List<long>();

                            List<long> emssidnew = new List<long>();
                            List<long> emseidnew = new List<long>();

                            for (int j1 = 0; j1 < data.Temp_subs_marks_list.Length; j1++)
                            {
                                var sub_idnew = data.Temp_subs_marks_list[j1].ISMS_Id;

                                if (data.ISMS_Id == sub_idnew && stu_id == data.Temp_subs_marks_list[j1].AMST_Id)
                                {
                                    if (emseidnew.Count > 0)
                                    {
                                        int count = 0;
                                        for (int k = 0; k < emseidnew.Count; k++)
                                        {
                                            if (emseidnew[k] == data.Temp_subs_marks_list[j1].EMSE_Id)
                                            {
                                                count += 1;
                                            }
                                        }
                                        if (count == 0)
                                        {
                                            emseidnew.Add(data.Temp_subs_marks_list[j1].EMSE_Id);
                                        }
                                    }
                                    else
                                    {
                                        emseidnew.Add(data.Temp_subs_marks_list[j1].EMSE_Id);
                                    }
                                }
                            }

                            var getsubsubjectsubexammarks = (from a in _examcontext.Exm_Student_Marks_SubSubjectDMO
                                                             from b in _examcontext.ExamMarksDMO
                                                             where (a.ESTM_Id == b.ESTM_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                                             && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && b.ISMS_Id == data.ISMS_Id
                                                             && b.AMST_Id == stu_id && (!emseidnew.Contains(a.EMSE_Id))
                                                             && b.EME_Id == data.EME_Id && b.ESTM_ActiveFlg == true)

                                                             select new Exm_Student_Marks_SubSubjectDMO
                                                             {
                                                                 ESTMSS_Marks = a.ESTMSS_Marks
                                                             }).ToList();


                            foreach (var m in getsubsubjectsubexammarks)
                            {
                                marks = marks + m.ESTMSS_Marks;
                            }

                            var result_obj = _examcontext.ExamMarksDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id && t.AMST_Id == stu_id && t.ESTM_ActiveFlg == true);

                            result_obj.ESTM_Marks = data.main_save_list[i].ESTM_Marks + Convert.ToDecimal(marks);
                            result_obj.ESTM_MarksGradeFlg = data.EYCES_MarksGradeEntryFlg;
                            result_obj.Id = data.UserId;
                            result_obj.ESTM_UpdatedBy = data.UserId;
                            result_obj.LoginDateTime = DateTime.Now;
                            result_obj.IP4 = data.IP4;
                            result_obj.UpdatedDate = DateTime.Now;
                            result_obj.ESTM_ActiveFlg = true;
                            result_obj.ESTM_Grade = data.main_save_list[i].ESTM_Grade;
                            result_obj.ESTM_Flg = data.main_save_list[i].ESTM_Flg;
                            _examcontext.Update(result_obj);

                            for (int j = 0; j < data.Temp_subs_marks_list.Length; j++)
                            {
                                var sub_id = data.Temp_subs_marks_list[j].ISMS_Id;
                                if (data.ISMS_Id == sub_id && stu_id == data.Temp_subs_marks_list[j].AMST_Id)
                                {
                                    var checkresult = _examcontext.Exm_Student_Marks_SubSubjectDMO.Where(t => t.MI_Id == data.MI_Id && t.ESTM_Id == result_obj.ESTM_Id
                                    && t.EMSE_Id == data.Temp_subs_marks_list[j].EMSE_Id).ToList();

                                    if (checkresult.Count() > 0)
                                    {
                                        var checkresultnew = _examcontext.Exm_Student_Marks_SubSubjectDMO.Single(t => t.MI_Id == data.MI_Id
                                        && t.ESTM_Id == result_obj.ESTM_Id && t.EMSE_Id == data.Temp_subs_marks_list[j].EMSE_Id);

                                        checkresultnew.ESTMSS_Marks = data.Temp_subs_marks_list[j].ESTMSS_Marks;
                                        checkresultnew.ESTMSS_MarksGradeFlg = data.Temp_subs_marks_list[j].ESTMSS_MarksGradeFlg;
                                        checkresultnew.ESTMSS_Grade = data.Temp_subs_marks_list[j].ESTMSS_Grade;
                                        checkresultnew.Login_Id = data.UserId;
                                        checkresultnew.ESTMSS_UpdatedBy = data.UserId;
                                        checkresultnew.LoginDateTime = DateTime.Now;
                                        checkresultnew.IP4 = data.IP4;
                                        checkresultnew.ESTMSS_ActiveFlg = true;
                                        checkresultnew.ESTMSS_Flg = data.Temp_subs_marks_list[j].ESTMSS_Flg;
                                        checkresultnew.UpdatedDate = DateTime.UtcNow;
                                        _examcontext.Update(checkresultnew);
                                    }
                                    else
                                    {
                                        Exm_Student_Marks_SubSubjectDMO obj_S = new Exm_Student_Marks_SubSubjectDMO();
                                        obj_S.MI_Id = data.MI_Id;
                                        obj_S.ESTM_Id = result_obj.ESTM_Id;
                                        obj_S.EMSS_Id = data.Temp_subs_marks_list[j].EMSS_Id;
                                        obj_S.ISMS_Id = sub_id;
                                        obj_S.EMSE_Id = data.Temp_subs_marks_list[j].EMSE_Id;
                                        obj_S.ESTMSS_Marks = data.Temp_subs_marks_list[j].ESTMSS_Marks;
                                        obj_S.ESTMSS_MarksGradeFlg = data.Temp_subs_marks_list[j].ESTMSS_MarksGradeFlg;
                                        obj_S.ESTMSS_Grade = data.Temp_subs_marks_list[j].ESTMSS_Grade;
                                        obj_S.Login_Id = data.UserId;
                                        obj_S.ESTMSS_CreatedBy = data.UserId;
                                        obj_S.ESTMSS_UpdatedBy = data.UserId;
                                        obj_S.LoginDateTime = DateTime.Now;
                                        obj_S.IP4 = data.IP4;
                                        obj_S.ESTMSS_ActiveFlg = true;
                                        obj_S.ESTMSS_Flg = data.Temp_subs_marks_list[j].ESTMSS_Flg;
                                        obj_S.CreatedDate = result_obj.CreatedDate;
                                        obj_S.UpdatedDate = DateTime.Now;
                                        _examcontext.Add(obj_S);
                                    }
                                }
                            }
                        }
                    }
                    var contactExists = _examcontext.SaveChanges();
                    if (contactExists >= 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                catch (Exception ee)
                {
                    _acdimpl.LogError(ee.Message);
                    _acdimpl.LogDebug(ee.Message);
                    Console.WriteLine(ee.Message);
                }
            }

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                //cmd.CommandText = "Exam_get_Marks_Entry_Modify";
                cmd.CommandText = "Exam_Duplicate_MarksEntry_Delete";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });
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
                                dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                            }

                            retObject.Add((ExpandoObject)dataRow1);
                        }
                    }

                }
                catch (Exception ex)
                {
                    _acdimpl.LogError(ex.Message);
                    _acdimpl.LogDebug(ex.Message);
                }

            }
            if (data.IVRMMAP_AddFlg == true)
            {
                 Calculation(data);
                var catid = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id
                  && t.MI_Id == data.MI_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).ToArray();

                var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EYC_ActiveFlg == true
                && catid.Contains(t.EMCA_Id)).Distinct().ToList();
                List<long> EYC_Id = new List<long>();
                if (eycid.Count > 0)
                {
                    
                    foreach(var d in eycid)
                    {
                        EYC_Id.Add(d.EYC_Id);
                    }

                }
                var pramotion = _examcontext.Exm_M_PromotionDMO.Where(R => R.MI_Id == data.MI_Id && EYC_Id.Contains(R.EYC_Id)).Distinct().ToList();
                if(pramotion.Count > 0)
                {
                    if (data.Pagename == "HHS")
                    {
                        promotionsaveddata(data);
                    }
                    else
                    {
                        data.IVRMMAP_UpdateFlg = Promotion_Calculation(data);
                    }
                    
                }
            }
            return data;
           
            //sanjeev
        }
        public MarksEntryHHSDTO Calculation(MarksEntryHHSDTO exm)
        {
            try
            {
                using (var cmd = _examcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 80000000;

                    if (exm.Pagename== "HHS")
                    {
                        cmd.CommandText = "IndSubjects_SubsExmMarksCalculation";
                    }
                    else
                    {
                        cmd.CommandText = "IndExamMarksCalculation";
                    }
                   
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = exm.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = exm.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                      SqlDbType.BigInt)
                    {
                        Value = exm.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                      SqlDbType.BigInt)
                    {
                        Value = exm.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                  SqlDbType.Int)
                    {
                        Value = exm.EME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ESTMP_PublishToStudentFlg",
                 SqlDbType.Int)
                    {
                        Value =0
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var a = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("Exam Calculation old 1:" + exm.MI_Id + " " + ex.Message + "");
                    }

                  //  exm.returnval = true;
                }

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                Exm_Calculation_LogDMO dmo = new Exm_Calculation_LogDMO();

                dmo.MI_Id = exm.MI_Id;
                dmo.ASMAY_Id = exm.ASMAY_Id;
                dmo.ASMCL_Id = exm.ASMCL_Id;
                dmo.ASMS_Id = exm.ASMS_Id;
                dmo.EME_Id = exm.EME_Id;
                dmo.IVRMUL_Id = exm.UserId;
                dmo.ECL_PublishFlag = 0;
                dmo.CreatedDate = indiantime0;
                dmo.UpdatedDate = indiantime0;

                _examcontext.Add(dmo);
                try
                {
                    var i = _examcontext.SaveChanges();
                    if (i > 0)
                    {
                        _acdimpl.LogInformation("Exam Calculation Old Insert Into Log Success");
                        //exm.returnval = true;
                    }
                    else
                    {
                        _acdimpl.LogInformation("Exam Calculation Old Insert Into Log Failed");
                       // exm.returnval = true;
                    }
                }
                catch (Exception ex)
                {
                    _acdimpl.LogInformation("Exam Calculation Old Insert :" + exm.MI_Id + " " + ex.Message + "");
                    Console.WriteLine(ex.Message);
                   // exm.returnval = true;
                }
            }
            catch (Exception ee)
            {
                _acdimpl.LogInformation("Exam Calculation old 2:" + exm.MI_Id + " " + ee.Message + "");
                Console.WriteLine(ee.Message);
               // exm.returnval = false;
            }

            return exm;
        }
        //Pramotion
        public MarksEntryHHSDTO promotionsaveddata(MarksEntryHHSDTO exm)
        {
            try
            {
                using (var cmd = _examcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EXAM_WITHOUT_RULES_PROMOTION_Calculation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 80000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = exm.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = exm.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = exm.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = exm.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@ESTMPP_PublishToStudentFlg", SqlDbType.Int) { Value = 0 });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var a = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("Exam  promotion Calculation New 1:" + exm.MI_Id + " " + ex.Message + "");
                        Console.WriteLine(ex.Message);
                    }


                    //exm.returnval = true;
                }
            }
            catch (Exception ee)
            {
                _acdimpl.LogInformation("Exam Calculation New 2:" + exm.MI_Id + " " + ee.Message + "");
                Console.WriteLine(ee.Message);
                //exm.returnval = false;
            }

            return exm;
        }
        public bool Promotion_Calculation(MarksEntryHHSDTO data)
        {
            var go_stop = false;
            try
            {
                var getconfing = _examcontext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();


                var EMCA_Id = _examcontext.Exm_Category_ClassDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).EMCA_Id;
                var EYC_Id = _examcontext.Exm_Yearly_CategoryDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EMCA_Id == EMCA_Id
                && t.EYC_ActiveFlg == true).EYC_Id;

                var promotion_type = _examcontext.Exm_M_PromotionDMO.Single(t => t.MI_Id == data.MI_Id && t.EYC_Id == EYC_Id
                && t.EMP_ActiveFlag == true).EMP_MarksPerFlg;
                //data.EMP_MarksPerFlg = promotion_type;

                if (promotion_type == "T" || promotion_type == "F")
                {
                    if (promotion_type == "F")
                    {
                        var EME_Id_Final = _examcontext.masterexam.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && t.EME_FinalExamFlag == true).FirstOrDefault().EME_Id;

                        var Cat_exms = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == EYC_Id && t.EYCE_ActiveFlg == true).Select(t => t.EME_Id).Distinct().ToList();
                        if (Cat_exms.Contains(EME_Id_Final))
                        {
                            var stu_process_marks = _examcontext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == EME_Id_Final).OrderBy(t => t.AMST_Id).Distinct().ToList();

                            var stu_subj_process_marks = _examcontext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == EME_Id_Final).OrderBy(t => t.AMST_Id).Distinct().ToList();

                            if (stu_process_marks.Count > 0 && stu_subj_process_marks.Count > 0)
                            {
                                var EMGR_Id = _examcontext.Exm_Yearly_Category_ExamsDMO.Single(t => t.EYC_Id == EYC_Id && t.EYCE_ActiveFlg == true && t.EME_Id == EME_Id_Final).EMGR_Id;

                                var gradedetails = _examcontext.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == EMGR_Id && t.EMGD_ActiveFlag == true).Distinct().ToList();



                                var outputval = _examcontext.Database.ExecuteSqlCommand("Exm_PromotionDetails_Delete @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, data.ASMCL_Id, data.ASMS_Id);

                                foreach (var subj_mks in stu_subj_process_marks)
                                {
                                    Exm_Stu_MP_Promo_SubjectwiseDMO obj_s = new Exm_Stu_MP_Promo_SubjectwiseDMO();
                                    obj_s.MI_Id = data.MI_Id;
                                    obj_s.ASMAY_Id = data.ASMAY_Id;
                                    obj_s.ASMCL_Id = data.ASMCL_Id;
                                    obj_s.ASMS_Id = data.ASMS_Id;
                                    obj_s.AMST_Id = subj_mks.AMST_Id;
                                    obj_s.ISMS_Id = subj_mks.ISMS_Id;
                                    obj_s.ESTMPPS_MaxMarks = subj_mks.ESTMPS_Medical_MaxMarks;
                                    obj_s.ESTMPPS_ObtainedMarks = subj_mks.ESTMPS_ObtainedMarks;
                                    obj_s.ESTMPPS_ObtainedGrade = subj_mks.ESTMPS_ObtainedGrade;
                                    obj_s.ESTMPPS_GradePoints = subj_mks.ESTMPS_GradePoints;
                                    obj_s.ESTMPPS_ClassAverage = subj_mks.ESTMPS_ClassAverage;
                                    obj_s.ESTMPPS_SectionAverage = subj_mks.ESTMPS_SectionAverage;
                                    obj_s.ESTMPPS_ClassHighest = subj_mks.ESTMPS_ClassHighest;
                                    obj_s.ESTMPPS_SectionHighest = subj_mks.ESTMPS_SectionHighest;
                                    obj_s.ESTMPPS_PassFailFlg = subj_mks.ESTMPS_PassFailFlg;

                                    obj_s.EMGD_Id = (obj_s.ESTMPPS_ObtainedGrade != null && obj_s.ESTMPPS_ObtainedGrade != "") ?
                                        gradedetails.Where(t => t.EMGD_Name == obj_s.ESTMPPS_ObtainedGrade).FirstOrDefault().EMGD_Id : obj_s.EMGD_Id;

                                    obj_s.ESTMPPS_Remarks = (obj_s.ESTMPPS_ObtainedGrade != null && obj_s.ESTMPPS_ObtainedGrade != "") ?
                                        gradedetails.Where(t => t.EMGD_Name == obj_s.ESTMPPS_ObtainedGrade).FirstOrDefault().EMGD_Remarks : obj_s.ESTMPPS_ObtainedGrade;

                                    obj_s.CreatedDate = DateTime.Now;
                                    obj_s.UpdatedDate = DateTime.Now;
                                    _examcontext.Add(obj_s);
                                }

                                foreach (var mks in stu_process_marks)
                                {
                                    Exm_Student_MP_PromotionDMO obj_m = new Exm_Student_MP_PromotionDMO();
                                    obj_m.MI_Id = data.MI_Id;
                                    obj_m.ASMAY_Id = data.ASMAY_Id;
                                    obj_m.ASMCL_Id = data.ASMCL_Id;
                                    obj_m.ASMS_Id = data.ASMS_Id;
                                    obj_m.AMST_Id = mks.AMST_Id;
                                    obj_m.ESTMPP_TotalMaxMarks = mks.ESTMP_TotalMaxMarks;
                                    obj_m.ESTMPP_TotalObtMarks = mks.ESTMP_TotalObtMarks;
                                    obj_m.ESTMPP_GraceMarks = 0;
                                    obj_m.ESTMPP_BonusMarks = 0;
                                    obj_m.ESTMPP_TotalMarks = (obj_m.ESTMPP_TotalObtMarks + obj_m.ESTMPP_GraceMarks + obj_m.ESTMPP_BonusMarks);
                                    obj_m.ESTMPP_Percentage = mks.ESTMP_Percentage;
                                    obj_m.ESTMPP_TotalGrade = mks.ESTMP_TotalGrade;
                                    obj_m.ESTMPP_ClassRank = mks.ESTMP_ClassRank;
                                    obj_m.ESTMPP_SectionRank = mks.ESTMP_SectionRank;
                                    obj_m.ESTMPP_Result = mks.ESTMP_Result;
                                    obj_m.EMGD_Id = mks.EMGD_Id;
                                    obj_m.CreatedDate = DateTime.Now;
                                    obj_m.UpdatedDate = DateTime.Now;
                                    _examcontext.Add(obj_m);
                                }

                                var contactExists = _examcontext.SaveChanges();
                                if (contactExists >= 1)
                                {
                                    go_stop = true;
                                }
                                else
                                {
                                    go_stop = false;
                                }
                            }
                        }

                    }

                    else if (promotion_type == "T")
                    {
                        var already_details1 = _examcontext.Exm_Stu_MP_Promo_SubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).Distinct().ToList();


                        var outputval = _examcontext.Database.ExecuteSqlCommand("Exm_PromotionDetails_Delete @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, data.ASMCL_Id, data.ASMS_Id);

                        var student_list = (from a in _examcontext.ExmStudentMarksProcessDMO
                                            from b in _examcontext.ExmStudentMarksProcessSubjectwiseDMO
                                            where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == b.ASMS_Id
                                            && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                            select a).Select(t => t.AMST_Id).Distinct().ToList();

                        var EMGR_Id = _examcontext.Exm_M_PromotionDMO.Single(t => t.MI_Id == data.MI_Id && t.EYC_Id == EYC_Id
                        && t.EMP_ActiveFlag == true).EMGR_Id;

                        var EMGR_MarksPerFlag = _examcontext.Exm_Master_GradeDMO.Single(t => t.MI_Id == data.MI_Id && t.EMGR_Id == EMGR_Id
                        && t.EMGR_ActiveFlag == true).EMGR_MarksPerFlag;

                        var Grade_Details = _examcontext.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == EMGR_Id
                        && t.EMGD_ActiveFlag == true).Distinct().ToList();

                        foreach (var stu_id in student_list)
                        {
                            data.AMST_Id = stu_id;

                            var student_subjects = (from a in _examcontext.StudentMappingDMO
                                                    from b in _examcontext.ExmStudentMarksProcessSubjectwiseDMO
                                                    where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                                    && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.AMST_Id && a.ESTSU_ActiveFlg == true && b.MI_Id == a.MI_Id
                                                    && b.ASMAY_Id == a.ASMAY_Id && b.ASMCL_Id == a.ASMCL_Id && b.ASMS_Id == a.ASMS_Id && b.AMST_Id == a.AMST_Id
                                                    && b.ISMS_Id == a.ISMS_Id)
                                                    select a.ISMS_Id).Distinct().ToList();

                            foreach (var subj_id in student_subjects)
                            {
                                var stu_subj_process_marks = _examcontext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id
                                && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.AMST_Id == data.AMST_Id
                                && t.ISMS_Id == subj_id).OrderBy(t => t.EME_Id).Distinct().ToList();

                                decimal? ESTMPPS_MaxMarks = 0;
                                decimal? ESTMPPS_ObtainedMarks = 0;
                                var flag = "";

                                foreach (var marks in stu_subj_process_marks)
                                {
                                    var subj_flg_exm_wise = marks.ESTMPS_PassFailFlg;
                                    if (subj_flg_exm_wise == "AB" || subj_flg_exm_wise == "M" || subj_flg_exm_wise == "OD" || subj_flg_exm_wise == "L")
                                    {
                                        flag = subj_flg_exm_wise;
                                    }
                                    ESTMPPS_MaxMarks += marks.ESTMPS_Medical_MaxMarks;
                                    ESTMPPS_ObtainedMarks += marks.ESTMPS_ObtainedMarks;
                                }
                                var result_grade = new List<Exm_Master_Grade_DetailsDMO>();
                                if (EMGR_MarksPerFlag == "M")
                                {
                                    result_grade = Grade_Details.Where(t => (ESTMPPS_ObtainedMarks >= t.EMGD_From && ESTMPPS_ObtainedMarks <= t.EMGD_To) || (ESTMPPS_ObtainedMarks <= t.EMGD_From && ESTMPPS_ObtainedMarks >= t.EMGD_To)).Distinct().ToList();
                                }

                                else if (EMGR_MarksPerFlag == "P")
                                {
                                    decimal? per = 0;
                                    per = (ESTMPPS_ObtainedMarks / ESTMPPS_MaxMarks) * 100;
                                    result_grade = Grade_Details.Where(t => (per >= t.EMGD_From && per <= t.EMGD_To) || (per <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();
                                }

                                decimal? ESTMPPS_MinMarks = 0;
                                ESTMPPS_MinMarks = (from a in _examcontext.Exm_Yearly_Category_ExamsDMO
                                                    from b in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                                    where (a.EYC_Id == EYC_Id && a.EYCE_ActiveFlg == true && b.EYCE_Id == a.EYCE_Id && b.EYCES_ActiveFlg == true
                                                    && b.ISMS_Id == subj_id)
                                                    select b.EYCES_MinMarks).Sum();

                                Exm_Stu_MP_Promo_SubjectwiseDMO obj_p = new Exm_Stu_MP_Promo_SubjectwiseDMO();
                                obj_p.MI_Id = data.MI_Id;
                                obj_p.ASMAY_Id = data.ASMAY_Id;
                                obj_p.ASMCL_Id = data.ASMCL_Id;
                                obj_p.ASMS_Id = data.ASMS_Id;
                                obj_p.AMST_Id = data.AMST_Id;
                                obj_p.ISMS_Id = subj_id;
                                obj_p.ESTMPPS_MaxMarks = ESTMPPS_MaxMarks;
                                obj_p.ESTMPPS_ObtainedMarks = ESTMPPS_ObtainedMarks;
                                obj_p.ESTMPPS_ObtainedGrade = result_grade.Count > 0 ? result_grade[0].EMGD_Name : null;
                                obj_p.ESTMPPS_GradePoints = result_grade.Count > 0 ? result_grade[0].EMGD_GradePoints : null;
                                if (result_grade.Count > 0)
                                {
                                    obj_p.EMGD_Id = result_grade[0].EMGD_Id;
                                }

                                obj_p.ESTMPPS_PassFailFlg = flag == "" ? ((ESTMPPS_ObtainedMarks) < ESTMPPS_MinMarks ? "Fail" : "Pass") : flag;
                                obj_p.ESTMPPS_Remarks = result_grade.Count > 0 ? result_grade[0].EMGD_Remarks : null;
                                obj_p.CreatedDate = DateTime.Now;
                                obj_p.UpdatedDate = DateTime.Now;
                                _examcontext.Add(obj_p);
                            }
                        }

                        var contactExists = _examcontext.SaveChanges();
                        if (contactExists >= 1)
                        {
                            go_stop = true;

                            using (var cmd = _examcontext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandTimeout = 80000000;
                                cmd.CommandText = "Promotion_StudentCS_CACH_Total";
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
                                    Value = data.ASMCL_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                                    SqlDbType.BigInt)
                                {
                                    Value = data.ASMS_Id
                                });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();
                                try
                                {
                                    var a = cmd.ExecuteNonQuery();
                                    if (a >= 1)
                                    {
                                        go_stop = true;
                                    }
                                    else
                                    {
                                        go_stop = false;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    go_stop = false;
                                    _acdimpl.LogError(ex.Message);
                                    _acdimpl.LogDebug(ex.Message);
                                }
                            }

                        }
                        else
                        {
                            go_stop = false;
                        }
                    }
                }

                else if (promotion_type != "T" && promotion_type != "F")
                {
                    if (promotion_type == "P")
                    {
                        //var already_details1 = _examcontext.Exm_Stu_MP_Promo_SubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).Distinct().ToList();

                        //var already_details2 = (from a in _examcontext.Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO
                        //                        from b in already_details1
                        //                        where (a.ESTMPPS_Id == b.ESTMPPS_Id)
                        //                        select a).Distinct().ToList();

                        //var already_details3 = (from a in _examcontext.Exm_Stu_MP_Promo_Subject_Groupwise_ExamDMO
                        //                        from b in already_details2
                        //                        where (a.ESTMPPSG_Id == b.ESTMPPSG_Id)
                        //                        select a).Distinct().ToList();


                        //if (already_details3.Any())
                        //{
                        //    for (int i = 0; already_details3.Count > i; i++)
                        //    {
                        //        _examcontext.Remove(already_details3.ElementAt(i));
                        //    }
                        //}

                        //if (already_details2.Any())
                        //{
                        //    for (int i = 0; already_details2.Count > i; i++)
                        //    {
                        //        _examcontext.Remove(already_details2.ElementAt(i));
                        //    }
                        //}

                        //if (already_details1.Any())
                        //{
                        //    for (int i = 0; already_details1.Count > i; i++)
                        //    {
                        //        _examcontext.Remove(already_details1.ElementAt(i));
                        //    }
                        //}

                        var outputval = _examcontext.Database.ExecuteSqlCommand("Exm_PromotionDetails_Delete @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, data.ASMCL_Id, data.ASMS_Id);

                        var EMP_Id = _examcontext.Exm_M_PromotionDMO.Single(t => t.MI_Id == data.MI_Id && t.EYC_Id == EYC_Id && t.EMP_ActiveFlag == true).EMP_Id;

                        //var promotion_subectdetails_new = _examcontext.Exm_M_Promotion_SubjectsDMO.Where(t => t.EMP_Id == EMP_Id).Distinct().ToList();
                        // var Amstid=4611,4733,4885,  4749,4776,5601,4870,4887,4524,4918

                        //var amst_id = (0, 4611, 4733, 4885, 4749, 4776, 5601, 4870, 4887, 4524, 4918);
                        //List<int> startperiod = new List<int>();
                        //sanjeev
                        //List<long> amst_ids = new List<long>(14) { 4611, 4733, 4885, 4749, 4776, 5601, 4870, 4887, 4524, 4918, 30719599, 4665, 4925, 4855 };

                        // amst_ids.Add(Convert.ToInt64(amst_id));
                        // List<long> amst_ids = new List<0, 4611, 4733, 4885, 4749, 4776, 5601, 4870, 4887, 4524, 4918>();

                        var student_list = (from a in _examcontext.ExmStudentMarksProcessDMO
                                            from b in _examcontext.ExmStudentMarksProcessSubjectwiseDMO
                                            where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == b.ASMS_Id
                                            && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                            select a).Select(t => t.AMST_Id).Distinct().ToList();

                        foreach (var stu_id in student_list)
                        {
                            data.AMST_Id = stu_id;

                            var promotion_subectdetails = (from a in _examcontext.Exm_M_Promotion_SubjectsDMO
                                                           from b in _examcontext.ExmStudentMarksProcessSubjectwiseDMO
                                                           where (a.ISMS_Id == b.ISMS_Id && b.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id
                                                           && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && a.EMP_Id == EMP_Id
                                                           && a.EMPS_ActiveFlag == true && b.MI_Id == data.MI_Id)
                                                           select a).Distinct().ToList();

                            List<TEmp_GroupWise_Exam_Marks> TEmp_GroupWise_Exam_Marks = new List<TEmp_GroupWise_Exam_Marks>();
                            List<TEmp_GroupWise_Marks> Groupwise_Details = new List<TEmp_GroupWise_Marks>();

                            foreach (var s in promotion_subectdetails)
                            {
                                var EMGR_MarksPerFlag = _examcontext.Exm_Master_GradeDMO.Single(t => t.MI_Id == data.MI_Id && t.EMGR_Id == s.EMGR_Id && t.EMGR_ActiveFlag == true).EMGR_MarksPerFlag;

                                var grade_details = _examcontext.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == s.EMGR_Id && t.EMGD_ActiveFlag == true).Distinct().ToList();

                                var stu_marks_details = _examcontext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id
                                && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.AMST_Id == data.AMST_Id
                                && t.ISMS_Id == s.ISMS_Id).Distinct().ToList();

                                if (stu_marks_details.Count > 0)
                                {
                                    var prom_subj_groupdetails = _examcontext.Exm_M_Prom_Subj_GroupDMO.Where(t => t.EMPS_Id == s.EMPS_Id).Distinct().ToList();
                                    var flag = "";
                                    //data.prom_subj_groupdetails = prom_subj_groupdetails.ToArray();
                                    foreach (var w in prom_subj_groupdetails)
                                    {
                                        decimal? group_marks = 0;
                                        decimal? ESTMPPSG_GroupMaxMarks = 0;

                                        var prom_subj_grp_exms = _examcontext.Exm_M_Prom_Subj_Group_ExamsDMO.Where(t => t.EMPSGE_ActiveFlg == true
                                        && t.EMPSG_Id == w.EMPSG_Id).Distinct().ToList();

                                        List<decimal?> exms_marks_grpwise = new List<decimal?>();
                                        List<decimal?> exms_max_marks_grpwise = new List<decimal?>();

                                        List<decimal?> exms_marks_grpwise_Temp = new List<decimal?>();
                                        List<decimal?> exms_max_marks_grpwise_Temp = new List<decimal?>();

                                        int subexamgrp = 0;

                                        //foreach (var z in prom_subj_grp_exms)
                                        //{
                                        //    var result = stu_marks_details.Where(t => t.EME_Id == z.EME_Id && t.ESTMPS_PassFailFlg != "OD").Distinct().ToList();
                                        //    if (result.Count > 0)
                                        //    {
                                        //        var subj_max_exm_wise = stu_marks_details.Single(t => t.EME_Id == z.EME_Id).ESTMPS_Medical_MaxMarks;
                                        //        var subj_flg_exm_wise = stu_marks_details.Single(t => t.EME_Id == z.EME_Id).ESTMPS_PassFailFlg;
                                        //        var subj_obt_exm_wise = stu_marks_details.Single(t => t.EME_Id == z.EME_Id).ESTMPS_ObtainedMarks;

                                        //        var ratio = s.EMPS_MaxMarks / subj_max_exm_wise;
                                        //        decimal? groupwise_marks = 0;

                                        //        if (subj_flg_exm_wise != "AB" && subj_flg_exm_wise != "OD" && subj_flg_exm_wise != "M" && subj_flg_exm_wise != "L")
                                        //        {
                                        //            groupwise_marks = subj_obt_exm_wise * ratio;
                                        //        }
                                        //        else
                                        //        {
                                        //            flag = subj_flg_exm_wise;
                                        //        }
                                        //        exms_marks_grpwise.Add(groupwise_marks);
                                        //    }
                                        //}

                                        foreach (var z in prom_subj_grp_exms)
                                        {
                                            decimal? raitoexammarks = 0.00m;
                                            decimal? convertedexammarks = 0.00m;
                                            decimal? convertedexam_max_marks = 0.00m;

                                            decimal? raitoexammarks_temp = 0.00m;
                                            decimal? convertedexammarks_temp = 0.00m;
                                            decimal? convertedexam_max_marks_temp = 0.00m;

                                            var examForMaxMarkrs = z.EMPSGE_ForMaxMarkrs;

                                            var result = stu_marks_details.Where(t => t.EME_Id == z.EME_Id && t.ESTMPS_PassFailFlg != "OD").Distinct().ToList();
                                            if (result.Count > 0)
                                            {
                                                subexamgrp = 1;
                                                var subj_max_exm_wise = stu_marks_details.Single(t => t.EME_Id == z.EME_Id).ESTMPS_Medical_MaxMarks;
                                                var subj_flg_exm_wise = stu_marks_details.Single(t => t.EME_Id == z.EME_Id).ESTMPS_PassFailFlg;
                                                var subj_obt_exm_wise = stu_marks_details.Single(t => t.EME_Id == z.EME_Id).ESTMPS_ObtainedMarks;
                                                var subj_max_exm_wise_temp = subj_max_exm_wise;

                                                decimal? groupwise_marks = 0;
                                                decimal? groupwise_maxmarks = 0;

                                                if (subj_flg_exm_wise != "OD")
                                                {
                                                    groupwise_marks = subj_obt_exm_wise;
                                                }
                                                flag = subj_flg_exm_wise;

                                                if (z.EMPSGE_ConvertionReqOrNot == true)
                                                {
                                                    // 30 > 10
                                                    if (subj_max_exm_wise > z.EMPSGE_ForMaxMarkrs)
                                                    {
                                                        //raitoexammarks = z.EMPSGE_ForMaxMarkrs / subj_max_exm_wise;
                                                        raitoexammarks = z.EMPSGE_ForMaxMarkrs / subj_max_exm_wise;
                                                        convertedexammarks = subj_obt_exm_wise * raitoexammarks;
                                                        convertedexam_max_marks = subj_max_exm_wise * raitoexammarks;

                                                    }
                                                    //10 < 30
                                                    else if (subj_max_exm_wise < z.EMPSGE_ForMaxMarkrs)
                                                    {
                                                        raitoexammarks = z.EMPSGE_ForMaxMarkrs / subj_max_exm_wise;
                                                        convertedexammarks = subj_obt_exm_wise * raitoexammarks;
                                                        convertedexam_max_marks = subj_max_exm_wise * raitoexammarks;
                                                    }
                                                    // 10 = 10
                                                    else if (subj_max_exm_wise == z.EMPSGE_ForMaxMarkrs)
                                                    {
                                                        raitoexammarks = z.EMPSGE_ForMaxMarkrs / subj_max_exm_wise;
                                                        convertedexammarks = subj_obt_exm_wise * raitoexammarks;
                                                        convertedexam_max_marks = subj_max_exm_wise * raitoexammarks;
                                                    }
                                                    subj_max_exm_wise_temp = convertedexam_max_marks;
                                                }

                                                else
                                                {
                                                    convertedexammarks = subj_obt_exm_wise;
                                                }


                                                // Convertion To Group Marks For Best Marks 

                                                if (subj_max_exm_wise == 100)
                                                {
                                                    convertedexammarks_temp = subj_obt_exm_wise;
                                                    convertedexam_max_marks_temp = subj_max_exm_wise;
                                                }

                                                else if (subj_max_exm_wise > 100)
                                                {
                                                    raitoexammarks_temp = 100 / subj_max_exm_wise;
                                                    convertedexammarks_temp = subj_obt_exm_wise * raitoexammarks_temp;
                                                    convertedexam_max_marks_temp = subj_max_exm_wise * raitoexammarks_temp;
                                                }
                                                else if (subj_max_exm_wise < 100)
                                                {
                                                    raitoexammarks_temp = subj_max_exm_wise / 100;
                                                    convertedexammarks_temp = subj_obt_exm_wise * raitoexammarks_temp;
                                                    convertedexam_max_marks_temp = subj_max_exm_wise * raitoexammarks_temp;
                                                }

                                                groupwise_maxmarks = z.EMPSGE_ForMaxMarkrs;
                                                exms_marks_grpwise.Add(convertedexammarks);
                                                exms_max_marks_grpwise.Add(subj_max_exm_wise_temp);


                                                exms_marks_grpwise_Temp.Add(convertedexammarks_temp);
                                                exms_max_marks_grpwise_Temp.Add(convertedexam_max_marks_temp);

                                                var resultexam_grade = new List<Exm_Master_Grade_DetailsDMO>();
                                                if (EMGR_MarksPerFlag == "M")
                                                {
                                                    resultexam_grade = grade_details.Where(t => (convertedexammarks >= t.EMGD_From
                                                    && convertedexammarks <= t.EMGD_To) || (convertedexammarks <= t.EMGD_From
                                                    && convertedexammarks >= t.EMGD_To)).Distinct().ToList();
                                                }
                                                else if (EMGR_MarksPerFlag == "P")
                                                {
                                                    decimal? per = 0;
                                                    per = (convertedexammarks / examForMaxMarkrs) * 100;
                                                    resultexam_grade = grade_details.Where(t => (per >= t.EMGD_From && per <= t.EMGD_To)
                                                    || (per <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();
                                                }

                                                int? EMGD_Id = null;
                                                if (resultexam_grade.Count > 0)
                                                {
                                                    EMGD_Id = resultexam_grade[0].EMGD_Id;
                                                }

                                                TEmp_GroupWise_Exam_Marks.Add(new TEmp_GroupWise_Exam_Marks
                                                {
                                                    MI_Id = data.MI_Id,
                                                    ASMAY_Id = data.ASMAY_Id,
                                                    ASMCL_Id = data.ASMCL_Id,
                                                    ASMS_Id = data.ASMS_Id,
                                                    AMST_Id = data.AMST_Id,
                                                    ISMS_Id = s.ISMS_Id,
                                                    EMPSG_Id = w.EMPSG_Id,
                                                    EME_Id = z.EME_Id,
                                                    ESTMPPSGE_ExamActualMarks = subj_obt_exm_wise,
                                                    ESTMPPSGE_ExamActualMaxMarks = subj_max_exm_wise,
                                                    ESTMPPSGE_ExamConvertedMarks = convertedexammarks,
                                                    ESTMPPSGE_ExamConvertedMaxMarks = subj_max_exm_wise_temp,
                                                    ESTMPPSGE_ExamPassFailFlag = subj_flg_exm_wise,
                                                    ESTMPPSGE_ExamConvertedGrade = resultexam_grade.Count > 0 ?
                                                    resultexam_grade[0].EMGD_Name : null,
                                                    ESTMPPSGE_ExamConvertedPoints = resultexam_grade.Count > 0 ?
                                                    resultexam_grade[0].EMGD_GradePoints : null,
                                                    EMGD_Id = EMGD_Id

                                                });
                                            }
                                        }

                                        var Best_off = w.EMPSG_BestOff;

                                        var best_marks_GroupTotal = exms_marks_grpwise.OrderByDescending(t => t).Take(Best_off).ToList();
                                        var best_max_marks_GroupTotal = exms_max_marks_grpwise.OrderByDescending(t => t).Take(Best_off).ToList();

                                        var best_marks = exms_marks_grpwise_Temp.OrderByDescending(t => t).Take(Best_off).ToList();
                                        var best_max_marks = exms_max_marks_grpwise_Temp.OrderByDescending(t => t).Take(Best_off).ToList();


                                        //var avg_marks = best_marks.Average();
                                        var avg_marks = best_marks.Sum();
                                        var avg_Max_marks = best_max_marks.Sum();

                                        var avg_marks_GroupTotal = best_marks_GroupTotal.Sum();
                                        var avg_Max_marks_GroupTotal = best_max_marks_GroupTotal.Sum();
                                        var avg_percentage_GroupTotal = (avg_marks_GroupTotal * s.EMPS_MaxMarks / avg_Max_marks_GroupTotal);

                                        if (promotion_type == "P")
                                        {
                                            if (avg_Max_marks > 0)
                                            {
                                                group_marks = (avg_marks * 100 / avg_Max_marks) * (w.EMPSG_PercentValue) / 100;
                                            }
                                            else
                                            {
                                                group_marks = 0;
                                            }

                                            ESTMPPSG_GroupMaxMarks = ((w.EMPSG_PercentValue) / (s.EMPS_MaxMarks)) * 100;
                                        }

                                        if (getconfing.FirstOrDefault().ExmConfig_RoundoffFlag == true)
                                        {
                                            group_marks = Math.Round(Convert.ToDecimal(group_marks), 0, MidpointRounding.AwayFromZero);
                                        }

                                        if (w.EMPSG_RoundOffFlag == true)
                                        {
                                            group_marks = Math.Round(Convert.ToDecimal(group_marks), 0, MidpointRounding.AwayFromZero);
                                        }

                                        Groupwise_Details.Add(new TEmp_GroupWise_Marks
                                        {
                                            MI_Id = data.MI_Id,
                                            ASMAY_Id = data.ASMAY_Id,
                                            ASMCL_Id = data.ASMCL_Id,
                                            ASMS_Id = data.ASMS_Id,
                                            AMST_Id = data.AMST_Id,
                                            ISMS_Id = s.ISMS_Id,
                                            EMPSG_Id = w.EMPSG_Id,
                                            ESTMPPSG_GroupMaxMarks = ESTMPPSG_GroupMaxMarks,
                                            ESTMPPSG_GroupObtMarks = group_marks,
                                            ESTMPPSG_GroupTotalMarks = avg_Max_marks_GroupTotal,
                                            ESTMPPSG_GroupObtMarksOutOfGroupTotal = avg_marks_GroupTotal,
                                            ESTMPPSG_ObtMarksOutOfSubjectMaxMarks = avg_percentage_GroupTotal,
                                        });
                                    }

                                    var sub_marks_total = Groupwise_Details.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.AMST_Id == data.AMST_Id && t.ISMS_Id == s.ISMS_Id).GroupBy(t => t.ISMS_Id).Select(a => new { Sum_obtain = a.Sum(t => t.ESTMPPSG_GroupObtMarks), Sum_max = a.Sum(t => t.ESTMPPSG_GroupMaxMarks) }).ToList();

                                    //var EMGR_MarksPerFlag = _examcontext.Exm_Master_GradeDMO.Single(t => t.MI_Id == data.MI_Id && t.EMGR_Id == s.EMGR_Id && t.EMGR_ActiveFlag == true).EMGR_MarksPerFlag;

                                    //var grade_details = _examcontext.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == s.EMGR_Id && t.EMGD_ActiveFlag == true).Distinct().ToList();

                                    if (sub_marks_total.Count > 0)
                                    {
                                        Exm_Stu_MP_Promo_SubjectwiseDMO obj_p = new Exm_Stu_MP_Promo_SubjectwiseDMO();
                                        obj_p.MI_Id = data.MI_Id;
                                        obj_p.ASMAY_Id = data.ASMAY_Id;
                                        obj_p.ASMCL_Id = data.ASMCL_Id;
                                        obj_p.ASMS_Id = data.ASMS_Id;
                                        obj_p.AMST_Id = data.AMST_Id;
                                        obj_p.ISMS_Id = s.ISMS_Id;

                                        if (s.EMPS_MaxMarks == s.EMPS_ConvertForMarks)
                                        {
                                            var result_grade = new List<Exm_Master_Grade_DetailsDMO>();
                                            if (EMGR_MarksPerFlag == "M")
                                            {
                                                result_grade = grade_details.Where(t => (sub_marks_total[0].Sum_obtain >= t.EMGD_From
                                                && sub_marks_total[0].Sum_obtain <= t.EMGD_To) || (sub_marks_total[0].Sum_obtain <= t.EMGD_From
                                                && sub_marks_total[0].Sum_obtain >= t.EMGD_To)).Distinct().ToList();
                                            }

                                            else if (EMGR_MarksPerFlag == "P")
                                            {
                                                decimal? per = 0;
                                                per = (sub_marks_total[0].Sum_obtain / sub_marks_total[0].Sum_max) * 100;
                                                result_grade = grade_details.Where(t => (per >= t.EMGD_From && per <= t.EMGD_To)
                                                || (per <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();
                                            }

                                            obj_p.ESTMPPS_MaxMarks = sub_marks_total[0].Sum_max;
                                            obj_p.ESTMPPS_ObtainedMarks = sub_marks_total[0].Sum_obtain;
                                            obj_p.ESTMPPS_ObtainedGrade = result_grade.Count > 0 ? result_grade[0].EMGD_Name : null;
                                            obj_p.ESTMPPS_PassFailFlg = flag == "" ? ((s.EMPS_MinMarks) > obj_p.ESTMPPS_ObtainedMarks ? "Fail" : "Pass") : flag;
                                            obj_p.ESTMPPS_Remarks = result_grade.Count > 0 ? result_grade[0].EMGD_Remarks : null;
                                            obj_p.ESTMPPS_GradePoints = result_grade.Count > 0 ? result_grade[0].EMGD_GradePoints : null;
                                            if (result_grade.Count > 0)
                                            {
                                                obj_p.EMGD_Id = result_grade[0].EMGD_Id;
                                            }
                                        }
                                        else
                                        {
                                            var convert_ratio = s.EMPS_ConvertForMarks / s.EMPS_MaxMarks;
                                            obj_p.ESTMPPS_MaxMarks = (sub_marks_total[0].Sum_max * convert_ratio);
                                            obj_p.ESTMPPS_ObtainedMarks = (sub_marks_total[0].Sum_obtain * convert_ratio);
                                            if (getconfing.FirstOrDefault().ExmConfig_RoundoffFlag == true)
                                            {
                                                obj_p.ESTMPPS_ObtainedMarks = Math.Round(Convert.ToDecimal(obj_p.ESTMPPS_ObtainedMarks), 0,
                                                    MidpointRounding.AwayFromZero);
                                            }

                                            var result_grade = new List<Exm_Master_Grade_DetailsDMO>();
                                            if (EMGR_MarksPerFlag == "M")
                                            {
                                                result_grade = grade_details.Where(t => (obj_p.ESTMPPS_ObtainedMarks >= t.EMGD_From
                                                && obj_p.ESTMPPS_ObtainedMarks <= t.EMGD_To) || (obj_p.ESTMPPS_ObtainedMarks <= t.EMGD_From
                                                && obj_p.ESTMPPS_ObtainedMarks >= t.EMGD_To)).Distinct().ToList();
                                            }

                                            else if (EMGR_MarksPerFlag == "P")
                                            {
                                                decimal? per = 0.00m;
                                                per = (obj_p.ESTMPPS_ObtainedMarks / obj_p.ESTMPPS_MaxMarks) * 100;
                                                result_grade = grade_details.Where(t => (per >= t.EMGD_From && per <= t.EMGD_To)
                                                || (per <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();
                                            }

                                            obj_p.ESTMPPS_ObtainedGrade = result_grade.Count > 0 ? result_grade[0].EMGD_Name : null;
                                            obj_p.ESTMPPS_PassFailFlg = flag == "" ? ((s.EMPS_MinMarks) > obj_p.ESTMPPS_ObtainedMarks ? "Fail" : "Pass") : flag;
                                            obj_p.ESTMPPS_Remarks = result_grade.Count > 0 ? result_grade[0].EMGD_Remarks : null;
                                            obj_p.ESTMPPS_GradePoints = result_grade.Count > 0 ? result_grade[0].EMGD_GradePoints : null;
                                            if (result_grade.Count > 0)
                                            {
                                                obj_p.EMGD_Id = result_grade[0].EMGD_Id;
                                            }
                                        }

                                        obj_p.CreatedDate = DateTime.Now;
                                        obj_p.UpdatedDate = DateTime.Now;
                                        _examcontext.Add(obj_p);

                                        foreach (var q in Groupwise_Details)
                                        {
                                            decimal? per1 = 0;

                                            if (q.MI_Id == obj_p.MI_Id && q.ASMAY_Id == obj_p.ASMAY_Id && q.ASMCL_Id == obj_p.ASMCL_Id && q.ASMS_Id == obj_p.ASMS_Id
                                                && q.AMST_Id == obj_p.AMST_Id && q.ISMS_Id == obj_p.ISMS_Id)
                                            {
                                                var result1_grade = new List<Exm_Master_Grade_DetailsDMO>();
                                                var result2_grade = new List<Exm_Master_Grade_DetailsDMO>();

                                                if (EMGR_MarksPerFlag == "M")
                                                {
                                                    result1_grade = grade_details.Where(t => (q.ESTMPPSG_GroupObtMarks >= t.EMGD_From
                                                    && q.ESTMPPSG_GroupObtMarks <= t.EMGD_To) || (q.ESTMPPSG_GroupObtMarks <= t.EMGD_From
                                                    && q.ESTMPPSG_GroupObtMarks >= t.EMGD_To)).Distinct().ToList();

                                                    result2_grade = grade_details.Where(t => (q.ESTMPPSG_GroupObtMarksOutOfGroupTotal >= t.EMGD_From
                                                    && q.ESTMPPSG_GroupObtMarksOutOfGroupTotal <= t.EMGD_To) || (q.ESTMPPSG_GroupObtMarksOutOfGroupTotal <= t.EMGD_From
                                                    && q.ESTMPPSG_GroupObtMarksOutOfGroupTotal >= t.EMGD_To)).Distinct().ToList();
                                                }
                                                else if (EMGR_MarksPerFlag == "P")
                                                {
                                                    decimal? per = 0;
                                                    per = (q.ESTMPPSG_GroupObtMarks / q.ESTMPPSG_GroupMaxMarks) * 100;
                                                    result1_grade = grade_details.Where(t => (per >= t.EMGD_From && per <= t.EMGD_To)
                                                    || (per <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();

                                                    per1 = (q.ESTMPPSG_GroupObtMarksOutOfGroupTotal / q.ESTMPPSG_GroupTotalMarks) * 100;

                                                    result2_grade = grade_details.Where(t => (per1 >= t.EMGD_From && per1 <= t.EMGD_To)
                                                    || (per1 <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();
                                                }


                                                Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO obj_c = new Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO();
                                                obj_c.ESTMPPS_Id = obj_p.ESTMPPS_Id;
                                                obj_c.EMPSG_Id = q.EMPSG_Id;
                                                obj_c.ESTMPPSG_GroupMaxMarks = q.ESTMPPSG_GroupMaxMarks;
                                                obj_c.ESTMPPSG_GroupObtMarks = q.ESTMPPSG_GroupObtMarks;
                                                obj_c.ESTMPPSG_GroupObtGrade = result1_grade.Count > 0 ? result1_grade[0].EMGD_Name : null;
                                                obj_c.ESTMPPSG_GradePoints = result1_grade.Count > 0 ? result1_grade[0].EMGD_GradePoints : null;
                                                if (result1_grade.Count > 0)
                                                {
                                                    obj_c.EMGD_Id = result1_grade[0].EMGD_Id;
                                                }

                                                obj_c.ESTMPPSG_GroupTotalMarks = q.ESTMPPSG_GroupTotalMarks;
                                                obj_c.ESTMPPSG_GroupObtMarksOutOfGroupTotal = q.ESTMPPSG_GroupObtMarksOutOfGroupTotal;
                                                obj_c.ESTMPPSG_ObtMarksOutOfSubjectMaxMarks = q.ESTMPPSG_ObtMarksOutOfSubjectMaxMarks;
                                                obj_c.ESTMPPSG_GroupMarksGrade = result1_grade.Count > 0 ? result1_grade[0].EMGD_Name : null;
                                                obj_c.ESTMPPSG_GroupPercentage = per1;

                                                if (result2_grade.Count > 0)
                                                {
                                                    obj_c.EMGD_Id_GroupTotalMarks = result2_grade[0].EMGD_Id;
                                                }

                                                obj_c.CreatedDate = DateTime.Now;
                                                obj_c.UpdatedDate = DateTime.Now;
                                                _examcontext.Add(obj_c);

                                                foreach (var r in TEmp_GroupWise_Exam_Marks)
                                                {
                                                    if (r.MI_Id == obj_p.MI_Id && r.ASMAY_Id == obj_p.ASMAY_Id && r.ASMCL_Id == obj_p.ASMCL_Id
                                                        && r.ASMS_Id == obj_p.ASMS_Id && r.AMST_Id == obj_p.AMST_Id && r.ISMS_Id == obj_p.ISMS_Id
                                                        && q.EMPSG_Id == r.EMPSG_Id)
                                                    {
                                                        Exm_Stu_MP_Promo_Subject_Groupwise_ExamDMO obj_groupexam = new Exm_Stu_MP_Promo_Subject_Groupwise_ExamDMO();

                                                        obj_groupexam.ESTMPPSG_Id = obj_c.ESTMPPSG_Id;
                                                        obj_groupexam.EME_Id = r.EME_Id;
                                                        obj_groupexam.ESTMPPSGE_ExamActualMarks = r.ESTMPPSGE_ExamActualMarks;
                                                        // obj_groupexam.ESTMPPSGE_ExamActualMaxMarks = r.ESTMPPSGE_ExamActualMaxMarks;
                                                        obj_groupexam.ESTMPPSGE_ExamConvertedMarks = r.ESTMPPSGE_ExamConvertedMarks;
                                                        //obj_groupexam.ESTMPPSGE_ExamConvertedMaxMarks = r.ESTMPPSGE_ExamConvertedMaxMarks;
                                                        obj_groupexam.ESTMPPSGE_ExamConvertedGrade = r.ESTMPPSGE_ExamConvertedGrade;
                                                        obj_groupexam.ESTMPPSGE_ExamConvertedPoints = r.ESTMPPSGE_ExamConvertedPoints;
                                                        obj_groupexam.ESTMPPSGE_ActiveFlg = true;
                                                        obj_groupexam.ESTMPPSGE_ExamPassFailFlag = r.ESTMPPSGE_ExamPassFailFlag;
                                                        obj_groupexam.EMGD_Id = r.EMGD_Id;
                                                        obj_groupexam.CreatedDate = DateTime.Now;
                                                        obj_groupexam.UpdatedDate = DateTime.Now;
                                                        _examcontext.Add(obj_groupexam);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        var contactExists = _examcontext.SaveChanges();
                        if (contactExists >= 1)
                        {
                            go_stop = true;

                            using (var cmd = _examcontext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandTimeout = 80000000;
                                cmd.CommandText = "Promotion_StudentCS_CACH";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();
                                try
                                {
                                    var a = cmd.ExecuteNonQuery();
                                    if (a >= 1)
                                    {
                                        go_stop = true;
                                    }
                                    else
                                    {
                                        go_stop = false;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    go_stop = false;
                                    _acdimpl.LogError(ex.Message);
                                    _acdimpl.LogDebug(ex.Message);
                                }
                            }
                        }
                        else
                        {
                            go_stop = false;
                        }
                    }

                    else if (promotion_type == "M")
                    {
                        //var already_details1 = _examcontext.Exm_Stu_MP_Promo_SubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).Distinct().ToList();//&& t.AMST_Id == data.AMST_Id

                        //var already_details2 = (from a in _examcontext.Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO
                        //                        from b in already_details1
                        //                        where (a.ESTMPPS_Id == b.ESTMPPS_Id)
                        //                        select a).Distinct().ToList();

                        //var already_details3 = (from a in _examcontext.Exm_Stu_MP_Promo_Subject_Groupwise_ExamDMO
                        //                        from b in already_details2
                        //                        where (a.ESTMPPSG_Id == b.ESTMPPSG_Id)
                        //                        select a).Distinct().ToList();


                        //if (already_details3.Any())
                        //{
                        //    for (int i = 0; already_details3.Count > i; i++)
                        //    {
                        //        _examcontext.Remove(already_details3.ElementAt(i));
                        //    }
                        //}

                        //if (already_details2.Any())
                        //{
                        //    for (int i = 0; already_details2.Count > i; i++)
                        //    {
                        //        _examcontext.Remove(already_details2.ElementAt(i));
                        //    }
                        //}

                        //if (already_details1.Any())
                        //{
                        //    for (int i = 0; already_details1.Count > i; i++)
                        //    {
                        //        _examcontext.Remove(already_details1.ElementAt(i));
                        //    }
                        //}

                        var outputval = _examcontext.Database.ExecuteSqlCommand("Exm_PromotionDetails_Delete @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, data.ASMCL_Id, data.ASMS_Id);

                        var EMP_Id = _examcontext.Exm_M_PromotionDMO.Single(t => t.MI_Id == data.MI_Id && t.EYC_Id == EYC_Id && t.EMP_ActiveFlag == true).EMP_Id;

                        //var promotion_subectdetails_new = _examcontext.Exm_M_Promotion_SubjectsDMO.Where(t => t.EMP_Id == EMP_Id).Distinct().ToList();

                        var student_list = (from a in _examcontext.ExmStudentMarksProcessDMO
                                            from b in _examcontext.ExmStudentMarksProcessSubjectwiseDMO
                                            where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id
                                            && a.ASMS_Id == b.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                            && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                            select a).Select(t => t.AMST_Id).Distinct().ToList();

                        foreach (var stu_id in student_list)
                        {
                            data.AMST_Id = stu_id;


                            List<TEmp_GroupWise_Exam_Marks> TEmp_GroupWise_Exam_Marks = new List<TEmp_GroupWise_Exam_Marks>();

                            List<TEmp_GroupWise_Marks> Groupwise_Details = new List<TEmp_GroupWise_Marks>();

                            var promotion_subectdetails = (from a in _examcontext.Exm_M_Promotion_SubjectsDMO
                                                           from b in _examcontext.ExmStudentMarksProcessSubjectwiseDMO
                                                           where (a.ISMS_Id == b.ISMS_Id && b.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id
                                                           && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && a.EMP_Id == EMP_Id
                                                           && a.EMPS_ActiveFlag == true && b.MI_Id == data.MI_Id)
                                                           select a).Distinct().ToList();

                            foreach (var s in promotion_subectdetails)
                            {
                                var EMGR_MarksPerFlag = _examcontext.Exm_Master_GradeDMO.Single(t => t.MI_Id == data.MI_Id && t.EMGR_Id == s.EMGR_Id && t.EMGR_ActiveFlag == true).EMGR_MarksPerFlag;

                                var grade_details = _examcontext.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == s.EMGR_Id && t.EMGD_ActiveFlag == true).Distinct().ToList();

                                var stu_marks_details = _examcontext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id
                                && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.AMST_Id == data.AMST_Id
                                && t.ISMS_Id == s.ISMS_Id).Distinct().ToList();

                                if (stu_marks_details.Count > 0)
                                {
                                    var prom_subj_groupdetails = _examcontext.Exm_M_Prom_Subj_GroupDMO.Where(t => t.EMPS_Id == s.EMPS_Id).Distinct().ToList();
                                    var flag = "";

                                    foreach (var w in prom_subj_groupdetails)
                                    {
                                        decimal? group_marks = 0;
                                        decimal? ESTMPPSG_GroupMaxMarks = 0;

                                        var prom_subj_grp_exms = _examcontext.Exm_M_Prom_Subj_Group_ExamsDMO.Where(t => t.EMPSGE_ActiveFlg == true && t.EMPSG_Id == w.EMPSG_Id).Distinct().ToList();

                                        List<decimal?> exms_marks_grpwise = new List<decimal?>();
                                        List<decimal?> exms_max_marks_grpwise = new List<decimal?>();

                                        List<decimal?> exms_marks_grpwise_Temp = new List<decimal?>();
                                        List<decimal?> exms_max_marks_grpwise_Temp = new List<decimal?>();


                                        int subexamgrp = 0;

                                        foreach (var z in prom_subj_grp_exms)
                                        {
                                            decimal? raitoexammarks = 0.00m;
                                            decimal? convertedexammarks = 0.00m;
                                            decimal? convertedexam_max_marks = 0.00m;

                                            decimal? raitoexammarks_temp = 0.00m;
                                            decimal? convertedexammarks_temp = 0.00m;
                                            decimal? convertedexam_max_marks_temp = 0.00m;

                                            var examForMaxMarkrs = z.EMPSGE_ForMaxMarkrs;

                                            var result = stu_marks_details.Where(t => t.EME_Id == z.EME_Id && t.ESTMPS_PassFailFlg != "OD").Distinct().ToList();
                                            if (result.Count > 0)
                                            {
                                                subexamgrp = 1;

                                                var subj_max_exm_wise = stu_marks_details.Where(t => t.EME_Id == z.EME_Id).FirstOrDefault().ESTMPS_Medical_MaxMarks;
                                                var subj_flg_exm_wise = stu_marks_details.Where(t => t.EME_Id == z.EME_Id).FirstOrDefault().ESTMPS_PassFailFlg;
                                                var subj_obt_exm_wise = stu_marks_details.Where(t => t.EME_Id == z.EME_Id).FirstOrDefault().ESTMPS_ObtainedMarks;
                                                //var subj_max_exm_wise = stu_marks_details.Single(t => t.EME_Id == z.EME_Id).ESTMPS_Medical_MaxMarks;
                                                //var subj_flg_exm_wise = stu_marks_details.Single(t => t.EME_Id == z.EME_Id).ESTMPS_PassFailFlg;
                                                //var subj_obt_exm_wise = stu_marks_details.Single(t => t.EME_Id == z.EME_Id).ESTMPS_ObtainedMarks;
                                                var subj_max_exm_wise_temp = subj_max_exm_wise;

                                                decimal? groupwise_marks = 0;
                                                decimal? groupwise_maxmarks = 0;

                                                if (subj_flg_exm_wise != "OD")
                                                {
                                                    groupwise_marks = subj_obt_exm_wise;
                                                }
                                                flag = subj_flg_exm_wise;

                                                //Convertion To Exam Wise
                                                if (z.EMPSGE_ConvertionReqOrNot == true)
                                                {
                                                    // 30 > 10
                                                    if (subj_max_exm_wise > z.EMPSGE_ForMaxMarkrs)
                                                    {
                                                        if (subj_max_exm_wise > 0)
                                                        {
                                                            raitoexammarks = z.EMPSGE_ForMaxMarkrs / subj_max_exm_wise;
                                                            convertedexammarks = subj_obt_exm_wise * raitoexammarks;
                                                            convertedexam_max_marks = subj_max_exm_wise * raitoexammarks;
                                                        }
                                                    }
                                                    //10 < 30
                                                    else if (subj_max_exm_wise < z.EMPSGE_ForMaxMarkrs)
                                                    {
                                                        if (subj_max_exm_wise > 0)
                                                        {
                                                            raitoexammarks = z.EMPSGE_ForMaxMarkrs / subj_max_exm_wise;
                                                            convertedexammarks = subj_obt_exm_wise * raitoexammarks;
                                                            convertedexam_max_marks = subj_max_exm_wise * raitoexammarks;
                                                        }
                                                    }
                                                    // 10 = 10
                                                    else if (subj_max_exm_wise == z.EMPSGE_ForMaxMarkrs)
                                                    {
                                                        if (subj_max_exm_wise > 0)
                                                        {
                                                            raitoexammarks = z.EMPSGE_ForMaxMarkrs / subj_max_exm_wise;
                                                            convertedexammarks = subj_obt_exm_wise * raitoexammarks;
                                                            convertedexam_max_marks = subj_max_exm_wise * raitoexammarks;
                                                        }
                                                    }
                                                    subj_max_exm_wise_temp = convertedexam_max_marks;
                                                }
                                                else
                                                {
                                                    convertedexammarks = subj_obt_exm_wise;
                                                }

                                                // Convertion To Group Marks For Best Marks 

                                                if (w.EMPSG_MarksValue == subj_max_exm_wise)
                                                {
                                                    convertedexammarks_temp = subj_obt_exm_wise;
                                                    convertedexam_max_marks_temp = subj_max_exm_wise;
                                                }

                                                else if (w.EMPSG_MarksValue > subj_max_exm_wise)
                                                {
                                                    if (subj_max_exm_wise > 0)
                                                    {
                                                        raitoexammarks_temp = w.EMPSG_MarksValue / subj_max_exm_wise;
                                                        convertedexammarks_temp = subj_obt_exm_wise * raitoexammarks_temp;
                                                        convertedexam_max_marks_temp = subj_max_exm_wise * raitoexammarks_temp;
                                                    }
                                                }

                                                else if (w.EMPSG_MarksValue < subj_max_exm_wise)
                                                {
                                                    if (w.EMPSG_MarksValue > 0)
                                                    {
                                                        raitoexammarks_temp = subj_max_exm_wise / w.EMPSG_MarksValue;
                                                        convertedexammarks_temp = subj_obt_exm_wise * raitoexammarks_temp;
                                                        convertedexam_max_marks_temp = subj_max_exm_wise * raitoexammarks_temp;
                                                    }
                                                }


                                                if (getconfing.FirstOrDefault().ExmConfig_RoundoffFlag == true)
                                                {
                                                    convertedexammarks = Math.Round(Convert.ToDecimal(convertedexammarks), 0, MidpointRounding.AwayFromZero);
                                                }

                                                if (w.EMPSG_RoundOffFlag == true)
                                                {
                                                    convertedexammarks = Math.Round(Convert.ToDecimal(convertedexammarks), 0, MidpointRounding.AwayFromZero);
                                                }

                                                groupwise_maxmarks = z.EMPSGE_ForMaxMarkrs;

                                                exms_marks_grpwise.Add(convertedexammarks);
                                                exms_max_marks_grpwise.Add(subj_max_exm_wise_temp);


                                                //exms_marks_grpwise_Temp.Add(convertedexammarks_temp);
                                                //exms_max_marks_grpwise_Temp.Add(convertedexam_max_marks_temp);


                                                exms_marks_grpwise_Temp.Add(convertedexammarks);
                                                exms_max_marks_grpwise_Temp.Add(subj_max_exm_wise_temp);


                                                var resultexam_grade = new List<Exm_Master_Grade_DetailsDMO>();
                                                if (EMGR_MarksPerFlag == "M")
                                                {
                                                    resultexam_grade = grade_details.Where(t => (convertedexammarks >= t.EMGD_From
                                                    && convertedexammarks <= t.EMGD_To) || (convertedexammarks <= t.EMGD_From
                                                    && convertedexammarks >= t.EMGD_To)).Distinct().ToList();
                                                }
                                                else if (EMGR_MarksPerFlag == "P")
                                                {
                                                    decimal? per = 0;
                                                    if (examForMaxMarkrs > 0)
                                                    {
                                                        per = (convertedexammarks / examForMaxMarkrs) * 100;
                                                    }
                                                    resultexam_grade = grade_details.Where(t => (per >= t.EMGD_From && per <= t.EMGD_To)
                                                    || (per <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();
                                                }
                                                int? EMGD_Id = null;
                                                if (resultexam_grade.Count > 0)
                                                {
                                                    EMGD_Id = resultexam_grade[0].EMGD_Id;
                                                }

                                                TEmp_GroupWise_Exam_Marks.Add(new TEmp_GroupWise_Exam_Marks
                                                {
                                                    MI_Id = data.MI_Id,
                                                    ASMAY_Id = data.ASMAY_Id,
                                                    ASMCL_Id = data.ASMCL_Id,
                                                    ASMS_Id = data.ASMS_Id,
                                                    AMST_Id = data.AMST_Id,
                                                    ISMS_Id = s.ISMS_Id,
                                                    EMPSG_Id = w.EMPSG_Id,
                                                    EME_Id = z.EME_Id,
                                                    ESTMPPSGE_ExamActualMarks = subj_obt_exm_wise,
                                                    ESTMPPSGE_ExamActualMaxMarks = subj_max_exm_wise,
                                                    ESTMPPSGE_ExamConvertedMarks = convertedexammarks,
                                                    ESTMPPSGE_ExamConvertedMaxMarks = subj_max_exm_wise_temp,
                                                    ESTMPPSGE_ExamPassFailFlag = subj_flg_exm_wise,
                                                    ESTMPPSGE_ExamConvertedGrade = resultexam_grade.Count > 0 ?
                                                    resultexam_grade[0].EMGD_Name : null,
                                                    ESTMPPSGE_ExamConvertedPoints = resultexam_grade.Count > 0 ?
                                                    resultexam_grade[0].EMGD_GradePoints : null,
                                                    EMGD_Id = EMGD_Id
                                                });
                                            }
                                        }

                                        var Best_off = w.EMPSG_BestOff;

                                        var best_marks_GroupTotal = exms_marks_grpwise.OrderByDescending(t => t).Take(Best_off).ToList();
                                        var best_maxmarks_GroupTotal = exms_max_marks_grpwise.OrderByDescending(t => t).Take(Best_off).ToList();
                                        var avg_marks_GroupTotal = best_marks_GroupTotal.Sum();
                                        var avg_maxmarks_GroupTotal = best_maxmarks_GroupTotal.Sum();

                                        decimal? avg_percentage_GroupTotal = 0.00m;
                                        if (avg_maxmarks_GroupTotal > 0)
                                        {
                                            avg_percentage_GroupTotal = (avg_marks_GroupTotal * s.EMPS_MaxMarks / avg_maxmarks_GroupTotal);
                                        }

                                        var best_marks = exms_marks_grpwise_Temp.OrderByDescending(t => t).Take(Best_off).ToList();
                                        var best_maxmarks = exms_max_marks_grpwise_Temp.OrderByDescending(t => t).Take(Best_off).ToList();
                                        var avg_marks = best_marks.Average();
                                        var avg_maxmarks = best_maxmarks.Average();

                                        if (promotion_type == "P")
                                        {
                                            group_marks = avg_marks * (w.EMPSG_PercentValue) / 100;
                                            ESTMPPSG_GroupMaxMarks = ((w.EMPSG_PercentValue) / (s.EMPS_MaxMarks)) * 100;
                                        }
                                        else
                                        {
                                            if (avg_maxmarks > 0)
                                            {
                                                var ratio = w.EMPSG_MarksValue / avg_maxmarks;
                                                group_marks = avg_marks * ratio;
                                                ESTMPPSG_GroupMaxMarks = w.EMPSG_MarksValue;
                                            }
                                        }

                                        if (getconfing.FirstOrDefault().ExmConfig_RoundoffFlag == true)
                                        {
                                            group_marks = Math.Round(Convert.ToDecimal(group_marks), 0, MidpointRounding.AwayFromZero);
                                        }

                                        if (w.EMPSG_RoundOffFlag == true)
                                        {
                                            group_marks = Math.Round(Convert.ToDecimal(group_marks), 0, MidpointRounding.AwayFromZero);
                                        }

                                        if (subexamgrp == 1)
                                        {
                                            Groupwise_Details.Add(new TEmp_GroupWise_Marks
                                            {
                                                MI_Id = data.MI_Id,
                                                ASMAY_Id = data.ASMAY_Id,
                                                ASMCL_Id = data.ASMCL_Id,
                                                ASMS_Id = data.ASMS_Id,
                                                AMST_Id = data.AMST_Id,
                                                ISMS_Id = s.ISMS_Id,
                                                EMPSG_Id = w.EMPSG_Id,
                                                ESTMPPSG_GroupMaxMarks = ESTMPPSG_GroupMaxMarks,
                                                ESTMPPSG_GroupObtMarks = group_marks,
                                                ESTMPPSG_GroupObtMarksOutOfGroupTotal = avg_marks_GroupTotal,
                                                ESTMPPSG_GroupTotalMarks = avg_marks_GroupTotal,
                                                ESTMPPSG_ObtMarksOutOfSubjectMaxMarks = avg_percentage_GroupTotal,
                                            });
                                        }
                                    }

                                    var sub_marks_total = Groupwise_Details.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.AMST_Id == data.AMST_Id && t.ISMS_Id == s.ISMS_Id).GroupBy(t => t.ISMS_Id).Select(a => new { Sum_obtain = a.Sum(t => t.ESTMPPSG_GroupObtMarks), Sum_max = a.Sum(t => t.ESTMPPSG_GroupMaxMarks) }).ToList();

                                    if (sub_marks_total.Count > 0)
                                    {
                                        Exm_Stu_MP_Promo_SubjectwiseDMO obj_p = new Exm_Stu_MP_Promo_SubjectwiseDMO();
                                        obj_p.MI_Id = data.MI_Id;
                                        obj_p.ASMAY_Id = data.ASMAY_Id;
                                        obj_p.ASMCL_Id = data.ASMCL_Id;
                                        obj_p.ASMS_Id = data.ASMS_Id;
                                        obj_p.AMST_Id = data.AMST_Id;
                                        obj_p.ISMS_Id = s.ISMS_Id;
                                        //for conversion of marks ........
                                        if (s.EMPS_MaxMarks == s.EMPS_ConvertForMarks)
                                        {
                                            var result_grade = new List<Exm_Master_Grade_DetailsDMO>();
                                            if (EMGR_MarksPerFlag == "M")
                                            {
                                                result_grade = grade_details.Where(t => (sub_marks_total[0].Sum_obtain >= t.EMGD_From
                                                && sub_marks_total[0].Sum_obtain <= t.EMGD_To) || (sub_marks_total[0].Sum_obtain <= t.EMGD_From
                                                && sub_marks_total[0].Sum_obtain >= t.EMGD_To)).Distinct().ToList();
                                            }
                                            else if (EMGR_MarksPerFlag == "P")
                                            {
                                                decimal? per = 0;
                                                if (sub_marks_total[0].Sum_max > 0)
                                                {
                                                    per = (sub_marks_total[0].Sum_obtain / sub_marks_total[0].Sum_max) * 100;
                                                }

                                                result_grade = grade_details.Where(t => (per >= t.EMGD_From && per <= t.EMGD_To)
                                                || (per <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();
                                            }

                                            obj_p.ESTMPPS_MaxMarks = sub_marks_total[0].Sum_max;
                                            obj_p.ESTMPPS_ObtainedMarks = sub_marks_total[0].Sum_obtain;
                                            obj_p.ESTMPPS_ObtainedGrade = result_grade.Count > 0 ? result_grade[0].EMGD_Name : null;
                                            obj_p.ESTMPPS_PassFailFlg = flag == "" ? ((s.EMPS_MinMarks) > obj_p.ESTMPPS_ObtainedMarks ? "Fail" : "Pass") : flag;
                                            obj_p.ESTMPPS_Remarks = result_grade.Count > 0 ? result_grade[0].EMGD_Remarks : null;
                                            obj_p.ESTMPPS_GradePoints = result_grade.Count > 0 ? result_grade[0].EMGD_GradePoints : null;
                                            if (result_grade.Count > 0)
                                            {
                                                obj_p.EMGD_Id = result_grade[0].EMGD_Id;
                                            }
                                        }
                                        else
                                        {
                                            decimal? convert_ratio = 0.00m;
                                            if (s.EMPS_MaxMarks > 0)
                                            {
                                                convert_ratio = s.EMPS_ConvertForMarks / s.EMPS_MaxMarks;
                                            }
                                            obj_p.ESTMPPS_MaxMarks = (sub_marks_total[0].Sum_max * convert_ratio);
                                            obj_p.ESTMPPS_ObtainedMarks = (sub_marks_total[0].Sum_obtain * convert_ratio);
                                            if (getconfing.FirstOrDefault().ExmConfig_RoundoffFlag == true)
                                            {
                                                obj_p.ESTMPPS_ObtainedMarks = Math.Round(Convert.ToDecimal(obj_p.ESTMPPS_ObtainedMarks),
                                                    0, MidpointRounding.AwayFromZero);
                                            }

                                            var result_grade = new List<Exm_Master_Grade_DetailsDMO>();
                                            if (EMGR_MarksPerFlag == "M")

                                            {
                                                result_grade = grade_details.Where(t => (obj_p.ESTMPPS_ObtainedMarks >= t.EMGD_From
                                                && obj_p.ESTMPPS_ObtainedMarks <= t.EMGD_To) || (obj_p.ESTMPPS_ObtainedMarks <= t.EMGD_From
                                                && obj_p.ESTMPPS_ObtainedMarks >= t.EMGD_To)).Distinct().ToList();
                                            }
                                            else if (EMGR_MarksPerFlag == "P")
                                            {
                                                decimal? per = 0;
                                                if (obj_p.ESTMPPS_MaxMarks > 0)
                                                {
                                                    per = (obj_p.ESTMPPS_ObtainedMarks / obj_p.ESTMPPS_MaxMarks) * 100;
                                                }
                                                result_grade = grade_details.Where(t => (per >= t.EMGD_From && per <= t.EMGD_To)
                                                || (per <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();
                                            }

                                            obj_p.ESTMPPS_ObtainedGrade = result_grade.Count > 0 ? result_grade[0].EMGD_Name : null;
                                            obj_p.ESTMPPS_PassFailFlg = flag == "" ? ((s.EMPS_MinMarks) > obj_p.ESTMPPS_ObtainedMarks ? "Fail" : "Pass") : flag;
                                            obj_p.ESTMPPS_Remarks = result_grade.Count > 0 ? result_grade[0].EMGD_Remarks : null;
                                            obj_p.ESTMPPS_GradePoints = result_grade.Count > 0 ? result_grade[0].EMGD_GradePoints : null;
                                            if (result_grade.Count > 0)
                                            {
                                                obj_p.EMGD_Id = result_grade[0].EMGD_Id;
                                            }
                                        }
                                        obj_p.CreatedDate = DateTime.Now;
                                        obj_p.UpdatedDate = DateTime.Now;
                                        _examcontext.Add(obj_p);

                                        foreach (var q in Groupwise_Details)
                                        {
                                            decimal? per1 = 0;

                                            if (q.MI_Id == obj_p.MI_Id && q.ASMAY_Id == obj_p.ASMAY_Id && q.ASMCL_Id == obj_p.ASMCL_Id && q.ASMS_Id == obj_p.ASMS_Id
                                                && q.AMST_Id == obj_p.AMST_Id && q.ISMS_Id == obj_p.ISMS_Id)
                                            {
                                                var result1_grade = new List<Exm_Master_Grade_DetailsDMO>();
                                                var result2_grade = new List<Exm_Master_Grade_DetailsDMO>();

                                                if (EMGR_MarksPerFlag == "M")
                                                {
                                                    result1_grade = grade_details.Where(t => (q.ESTMPPSG_GroupObtMarks >= t.EMGD_From
                                                    && q.ESTMPPSG_GroupObtMarks <= t.EMGD_To) || (q.ESTMPPSG_GroupObtMarks <= t.EMGD_From
                                                    && q.ESTMPPSG_GroupObtMarks >= t.EMGD_To)).Distinct().ToList();

                                                    result2_grade = grade_details.Where(t => (q.ESTMPPSG_GroupObtMarksOutOfGroupTotal >= t.EMGD_From
                                                 && q.ESTMPPSG_GroupObtMarksOutOfGroupTotal <= t.EMGD_To) || (q.ESTMPPSG_GroupObtMarksOutOfGroupTotal <= t.EMGD_From
                                                 && q.ESTMPPSG_GroupObtMarksOutOfGroupTotal >= t.EMGD_To)).Distinct().ToList();
                                                }
                                                else if (EMGR_MarksPerFlag == "P")
                                                {
                                                    decimal? per = 0;
                                                    if (q.ESTMPPSG_GroupMaxMarks > 0)
                                                    {
                                                        per = (q.ESTMPPSG_GroupObtMarks / q.ESTMPPSG_GroupMaxMarks) * 100;
                                                    }
                                                    result1_grade = grade_details.Where(t => (per >= t.EMGD_From && per <= t.EMGD_To)
                                                    || (per <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();

                                                    if (q.ESTMPPSG_GroupTotalMarks > 0)
                                                    {
                                                        per1 = (q.ESTMPPSG_GroupObtMarksOutOfGroupTotal / q.ESTMPPSG_GroupTotalMarks) * 100;
                                                    }
                                                    else
                                                    {
                                                        per1 = 0;
                                                    }

                                                    result2_grade = grade_details.Where(t => (per1 >= t.EMGD_From && per1 <= t.EMGD_To)
                                                    || (per1 <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();
                                                }


                                                Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO obj_c = new Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO();
                                                obj_c.ESTMPPS_Id = obj_p.ESTMPPS_Id;
                                                obj_c.EMPSG_Id = q.EMPSG_Id;
                                                obj_c.ESTMPPSG_GroupMaxMarks = q.ESTMPPSG_GroupMaxMarks;
                                                obj_c.ESTMPPSG_GroupObtMarks = q.ESTMPPSG_GroupObtMarks;
                                                obj_c.ESTMPPSG_GroupObtGrade = result1_grade.Count > 0 ? result1_grade[0].EMGD_Name : null;
                                                obj_c.ESTMPPSG_GradePoints = result1_grade.Count > 0 ? result1_grade[0].EMGD_GradePoints : null;
                                                if (result1_grade.Count > 0)
                                                {
                                                    obj_c.EMGD_Id = result1_grade[0].EMGD_Id;
                                                }

                                                obj_c.ESTMPPSG_GroupMarksGrade = result2_grade.Count > 0 ? result2_grade[0].EMGD_Name : null;
                                                obj_c.ESTMPPSG_GroupTotalMarks = q.ESTMPPSG_GroupTotalMarks;
                                                obj_c.ESTMPPSG_GroupObtMarksOutOfGroupTotal = q.ESTMPPSG_GroupObtMarksOutOfGroupTotal;
                                                obj_c.ESTMPPSG_ObtMarksOutOfSubjectMaxMarks = q.ESTMPPSG_ObtMarksOutOfSubjectMaxMarks;
                                                obj_c.ESTMPPSG_GroupPercentage = per1;

                                                if (result2_grade.Count > 0)
                                                {
                                                    obj_c.EMGD_Id_GroupTotalMarks = result2_grade[0].EMGD_Id;
                                                }
                                                obj_c.CreatedDate = DateTime.Now;
                                                obj_c.UpdatedDate = DateTime.Now;
                                                _examcontext.Add(obj_c);

                                                foreach (var r in TEmp_GroupWise_Exam_Marks)
                                                {
                                                    if (r.MI_Id == obj_p.MI_Id && r.ASMAY_Id == obj_p.ASMAY_Id && r.ASMCL_Id == obj_p.ASMCL_Id
                                                        && r.ASMS_Id == obj_p.ASMS_Id && r.AMST_Id == obj_p.AMST_Id && r.ISMS_Id == obj_p.ISMS_Id
                                                        && q.EMPSG_Id == r.EMPSG_Id)
                                                    {
                                                        Exm_Stu_MP_Promo_Subject_Groupwise_ExamDMO obj_groupexam = new Exm_Stu_MP_Promo_Subject_Groupwise_ExamDMO();

                                                        obj_groupexam.ESTMPPSG_Id = obj_c.ESTMPPSG_Id;
                                                        obj_groupexam.EME_Id = r.EME_Id;
                                                        obj_groupexam.ESTMPPSGE_ExamActualMarks = r.ESTMPPSGE_ExamActualMarks;
                                                        //obj_groupexam.ESTMPPSGE_ExamActualMaxMarks = r.ESTMPPSGE_ExamActualMaxMarks;
                                                        obj_groupexam.ESTMPPSGE_ExamConvertedMarks = r.ESTMPPSGE_ExamConvertedMarks;
                                                        // obj_groupexam.ESTMPPSGE_ExamConvertedMaxMarks = r.ESTMPPSGE_ExamConvertedMaxMarks;
                                                        obj_groupexam.ESTMPPSGE_ExamConvertedGrade = r.ESTMPPSGE_ExamConvertedGrade;
                                                        obj_groupexam.ESTMPPSGE_ExamConvertedPoints = r.ESTMPPSGE_ExamConvertedPoints;
                                                        obj_groupexam.ESTMPPSGE_ActiveFlg = true;
                                                        obj_groupexam.ESTMPPSGE_ExamPassFailFlag = r.ESTMPPSGE_ExamPassFailFlag;
                                                        obj_groupexam.EMGD_Id = r.EMGD_Id;
                                                        obj_groupexam.CreatedDate = DateTime.Now;
                                                        obj_groupexam.UpdatedDate = DateTime.Now;
                                                        _examcontext.Add(obj_groupexam);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        var contactExists = _examcontext.SaveChanges();
                        if (contactExists >= 1)
                        {
                            go_stop = true;

                            using (var cmd = _examcontext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandTimeout = 80000000;
                                cmd.CommandText = "Promotion_StudentCS_CACH";
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
                                    Value = data.ASMCL_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                                    SqlDbType.BigInt)
                                {
                                    Value = data.ASMS_Id
                                });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();
                                try
                                {
                                    var a = cmd.ExecuteNonQuery();
                                    if (a >= 1)
                                    {
                                        go_stop = true;
                                    }
                                    else
                                    {
                                        go_stop = false;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    go_stop = false;
                                    _acdimpl.LogError(ex.Message);
                                    _acdimpl.LogDebug(ex.Message);
                                }
                            }

                        }
                        else
                        {
                            go_stop = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return go_stop;
        }
        public MarksEntryHHSDTO onsearchworking(MarksEntryHHSDTO data)
        {
            try
            {
                string order = "";
                var get_configuration = _examcontext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
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



                List<ExmStudentMarksProcessDMO> calculationid = new List<ExmStudentMarksProcessDMO>();
                calculationid = _examcontext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id).ToList();
                if (calculationid.Count > 0)
                {
                    data.marksdeleteflag = true;
                }
                else if (calculationid.Count == 0)
                {
                    List<ExmStudentMarksProcessSubjectwiseDMO> calculationSubWiseid = new List<ExmStudentMarksProcessSubjectwiseDMO>();
                    calculationSubWiseid = _examcontext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id).ToList();
                    if (calculationSubWiseid.Count > 0)
                    {
                        data.marksdeleteflag = true;
                    }
                    else if (calculationSubWiseid.Count == 0)
                    {
                        data.marksdeleteflag = false;
                    }
                }

                var subject_details = (from a in _examcontext.Exm_Category_ClassDMO
                                       from b in _examcontext.Exm_Yearly_CategoryDMO
                                       from c in _examcontext.Exm_Yearly_Category_ExamsDMO
                                       from d in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                       where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == a.EMCA_Id && b.EYC_ActiveFlg == true && c.EYC_Id == b.EYC_Id && c.EME_Id == data.EME_Id && c.EYCE_ActiveFlg == true && d.EYCE_Id == c.EYCE_Id && d.ISMS_Id == data.ISMS_Id && d.EYCES_ActiveFlg == true)
                                       select d).Distinct().ToList();

                data.subject_details = subject_details.ToArray();
                data.EYCES_MarksGradeEntryFlg = subject_details[0].EYCES_MarksGradeEntryFlg;

                data.EYCES_SubSubjectFlg = subject_details[0].EYCES_SubSubjectFlg;
                data.EYCES_SubExamFlg = subject_details[0].EYCES_SubExamFlg;

                if (data.EYCES_SubSubjectFlg)
                {
                    data.subject_subsubjects = (from a in _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                                from b in _examcontext.mastersubsubject
                                                where (b.MI_Id == data.MI_Id && b.EMSS_ActiveFlag == true && a.EYCES_Id == subject_details[0].EYCES_Id && b.EMSS_Id == a.EMSS_Id)
                                                select new ExamSubjectMappingDTO
                                                {
                                                    EYCESSS_Id = a.EYCESSS_Id,
                                                    EYCES_Id = a.EYCES_Id,
                                                    EMSS_Id = a.EMSS_Id,
                                                    EMGR_Id = a.EMGR_Id,
                                                    EYCESSS_MaxMarks = a.EYCESSS_MaxMarks,
                                                    EYCESSS_MinMarks = a.EYCESSS_MinMarks,
                                                    EYCESSS_ExemptedFlg = a.EYCESSS_ExemptedFlg,
                                                    EYCESSS_ExemptedPer = a.EYCESSS_ExemptedPer,
                                                    EYCESSS_ActiveFlg = a.EYCESSS_ActiveFlg,
                                                    EYCESSS_SubSubjectOrder = a.EYCESSS_SubSubjectOrder,
                                                    EMSS_SubSubjectName = b.EMSS_SubSubjectName,
                                                    EMSS_SubSubjectCode = b.EMSS_SubSubjectCode
                                                }).Distinct().OrderBy(a => a.EYCESSS_SubSubjectOrder).ToArray();
                }
                if (data.EYCES_SubExamFlg)
                {
                    data.subject_subexams = (from a in _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO
                                             from b in _examcontext.mastersubexam
                                             where (b.MI_Id == data.MI_Id && b.EMSE_ActiveFlag == true && a.EYCES_Id == subject_details[0].EYCES_Id && b.EMSE_Id == a.EMSE_Id)
                                             select new ExamSubjectMappingDTO
                                             {
                                                 EYCESSE_Id = a.EYCESSE_Id,
                                                 EYCES_Id = a.EYCES_Id,
                                                 EMSE_Id = a.EMSE_Id,
                                                 EMGR_Id = a.EMGR_Id,
                                                 EYCESSE_MaxMarks = a.EYCESSE_MaxMarks,
                                                 EYCESSE_MinMarks = a.EYCESSE_MinMarks,
                                                 EYCESSE_ExemptedFlg = a.EYCESSE_ExemptedFlg,
                                                 EYCESSE_ExemptedPer = a.EYCESSE_ExemptedPer,
                                                 EYCESSE_ActiveFlg = a.EYCESSE_ActiveFlg,
                                                 EYCESSE_SubExamOrder = a.EYCESSE_SubExamOrder,
                                                 EMSE_SubExamName = b.EMSE_SubExamName,
                                                 EMSE_SubExamCode = b.EMSE_SubExamCode
                                             }).Distinct().OrderBy(a => a.EYCESSE_SubExamOrder).ToArray();
                }

                data.EMGR_Id = subject_details[0].EMGR_Id;

                if (data.EYCES_MarksGradeEntryFlg == "G")
                {
                    data.grade_details = (from a in _examcontext.Exm_Master_GradeDMO
                                          from b in _examcontext.Exm_Master_Grade_DetailsDMO
                                          where (a.MI_Id == data.MI_Id && a.EMGR_Id == data.EMGR_Id && b.EMGR_Id == data.EMGR_Id && b.EMGD_ActiveFlag == true)
                                          select b).Select(t => t.EMGD_Name).Distinct().ToArray();
                    if (data.EYCES_SubSubjectFlg)
                    {
                        data.subsubject_gradedetails = (from a in _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                                        from b in _examcontext.Exm_Master_GradeDMO
                                                        from c in _examcontext.Exm_Master_Grade_DetailsDMO
                                                        where (a.EYCES_Id == subject_details[0].EYCES_Id && a.EMGR_Id == b.EMGR_Id && b.MI_Id == data.MI_Id && b.EMGR_ActiveFlag == true && c.EMGR_Id == b.EMGR_Id && c.EMGD_ActiveFlag == true)
                                                        select c).Distinct().ToArray();
                    }
                    if (data.EYCES_SubExamFlg)
                    {
                        data.subexam_gradedetails = (from a in _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO
                                                     from b in _examcontext.Exm_Master_GradeDMO
                                                     from c in _examcontext.Exm_Master_Grade_DetailsDMO
                                                     where (a.EYCES_Id == subject_details[0].EYCES_Id && a.EMGR_Id == b.EMGR_Id && b.MI_Id == data.MI_Id && b.EMGR_ActiveFlag == true && c.EMGR_Id == b.EMGR_Id && c.EMGD_ActiveFlag == true)
                                                     select c).Distinct().ToArray();
                    }
                }

                var alrdy_stu_count = _examcontext.ExamMarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id).ToList().Count();

                var stu_list_mapped = _examcontext.StudentMappingDMO.Where(k => k.MI_Id == data.MI_Id && k.ASMAY_Id == data.ASMAY_Id && k.ASMCL_Id == data.ASMCL_Id && k.ASMS_Id == data.ASMS_Id && k.ISMS_Id == data.ISMS_Id && k.ESTSU_ActiveFlg == true).Select(t => t.AMST_Id).Distinct().ToList();

                List<temp_marks_DTO> studentList = new List<temp_marks_DTO>();

                studentList = (from e in _examcontext.Adm_M_Student
                               from f in _examcontext.School_Adm_Y_StudentDMO
                               from g in _examcontext.IVRM_School_Master_SubjectsDMO
                               from h in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                               from i in subject_details
                               where (e.AMST_Id == f.AMST_Id && f.ASMCL_Id == data.ASMCL_Id && f.ASMS_Id == data.ASMS_Id && f.ASMAY_Id == data.ASMAY_Id && e.MI_Id == data.MI_Id && e.AMST_ActiveFlag == 1 && e.AMST_SOL == "S" && f.AMAY_ActiveFlag == 1 && g.ISMS_Id == h.ISMS_Id && g.MI_Id == data.MI_Id && g.ISMS_ActiveFlag == 1 && g.ISMS_ExamFlag == 1 && g.ISMS_Id == data.ISMS_Id && g.ISMS_Id == data.ISMS_Id && h.EYCE_Id == i.EYCE_Id)

                               select new temp_marks_DTO//MarksEntryHHSDTO
                               {
                                   AMST_Id = e.AMST_Id,
                                   AMST_FirstName = ((e.AMST_FirstName == null ? " " : e.AMST_FirstName) + " " + (e.AMST_MiddleName == null ? " " : e.AMST_MiddleName) + " " + (e.AMST_LastName == null ? " " : e.AMST_LastName)).Trim(),
                                   AMST_AdmNo = e.AMST_AdmNo == null ? "" : e.AMST_AdmNo,
                                   AMST_RegistrationNo = e.AMST_RegistrationNo == null ? "" : e.AMST_RegistrationNo,
                                   AMAY_RollNo = f.AMAY_RollNo,
                                   ISMS_Id = g.ISMS_Id,
                                   ISMS_SubjectName = g.ISMS_SubjectName,
                                   EYCES_MaxMarks = h.EYCES_MaxMarks,
                                   EYCES_MinMarks = h.EYCES_MinMarks,
                                   EYCES_MarksEntryMax = h.EYCES_MarksEntryMax,
                               }).Distinct().OrderBy(t => order).ToList();


                var propertyInfo = typeof(temp_marks_DTO).GetProperty(order);
                studentList = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();

                data.studentList = studentList.Where(t => stu_list_mapped.Contains(t.AMST_Id)).Distinct().ToArray();

                if (alrdy_stu_count > 0 && !data.EYCES_SubSubjectFlg && !data.EYCES_SubExamFlg)
                {
                    var stu_marks = _examcontext.ExamMarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id).Distinct().ToList();

                    List<temp_marks_DTO> saved_studentList = new List<temp_marks_DTO>();

                    saved_studentList = (from a in studentList
                                         from b in stu_marks
                                         where (a.AMST_Id == b.AMST_Id && b.ISMS_Id == a.ISMS_Id && stu_list_mapped.Contains(a.AMST_Id))
                                         select new temp_marks_DTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = a.AMST_FirstName,
                                             AMST_AdmNo = a.AMST_AdmNo,
                                             AMST_RegistrationNo = a.AMST_RegistrationNo == null ? "" : a.AMST_RegistrationNo,
                                             AMAY_RollNo = a.AMAY_RollNo,
                                             ISMS_Id = a.ISMS_Id,
                                             ISMS_SubjectName = a.ISMS_SubjectName,
                                             EYCES_MaxMarks = a.EYCES_MaxMarks,
                                             EYCES_MarksEntryMax = a.EYCES_MarksEntryMax,
                                             EYCES_MinMarks = a.EYCES_MinMarks,
                                             ESTM_Marks = b.ESTM_Marks,
                                             ESTM_Grade = b.ESTM_Grade,
                                             ESTM_Flg = b.ESTM_Flg,
                                             ESTM_Id = b.ESTM_Id
                                         }).Distinct().OrderBy(t => order).ToList();

                    var propertyInfo1 = typeof(temp_marks_DTO).GetProperty(order);
                    saved_studentList = saved_studentList.OrderBy(x => propertyInfo1.GetValue(x, null)).ToList();

                    data.saved_studentList = saved_studentList.Distinct().ToArray();
                }

                else if (alrdy_stu_count > 0 && data.EYCES_SubSubjectFlg && data.EYCES_SubExamFlg)
                {
                    var stu_marks = _examcontext.ExamMarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id).Distinct().ToList();

                    List<temp_marks_DTO> saved_studentList = new List<temp_marks_DTO>();

                    saved_studentList = (from a in studentList
                                         from b in stu_marks
                                         where (a.AMST_Id == b.AMST_Id && b.ISMS_Id == a.ISMS_Id && stu_list_mapped.Contains(a.AMST_Id))
                                         select new temp_marks_DTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = a.AMST_FirstName,
                                             AMST_AdmNo = a.AMST_AdmNo,
                                             AMST_RegistrationNo = a.AMST_RegistrationNo == null ? "" : a.AMST_RegistrationNo,
                                             AMAY_RollNo = a.AMAY_RollNo,
                                             ISMS_Id = a.ISMS_Id,
                                             ISMS_SubjectName = a.ISMS_SubjectName,
                                             EYCES_MaxMarks = a.EYCES_MaxMarks,
                                             EYCES_MarksEntryMax = a.EYCES_MarksEntryMax,
                                             EYCES_MinMarks = a.EYCES_MinMarks,
                                             ESTM_Marks = b.ESTM_Marks,
                                             ESTM_Grade = b.ESTM_Grade,
                                             ESTM_Flg = b.ESTM_Flg,
                                             ESTM_Id = b.ESTM_Id
                                         }).Distinct().OrderBy(t => order).ToList();

                    var propertyInfo2 = typeof(temp_marks_DTO).GetProperty(order);
                    saved_studentList = saved_studentList.OrderBy(x => propertyInfo2.GetValue(x, null)).ToList();

                    data.saved_studentList = saved_studentList.Distinct().ToArray();

                    data.saved_ssse_list = (from a in _examcontext.Exm_Student_Marks_SubSubjectDMO
                                            from b in stu_marks
                                            where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ESTM_Id == b.ESTM_Id)
                                            select a).Distinct().ToArray();
                }
                else if (alrdy_stu_count > 0 && data.EYCES_SubSubjectFlg && !data.EYCES_SubExamFlg)
                {
                    var stu_marks = _examcontext.ExamMarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id).Distinct().ToList();

                    List<temp_marks_DTO> saved_studentList = new List<temp_marks_DTO>();

                    saved_studentList = (from a in studentList
                                         from b in stu_marks
                                         where (a.AMST_Id == b.AMST_Id && b.ISMS_Id == a.ISMS_Id && stu_list_mapped.Contains(a.AMST_Id))
                                         select new temp_marks_DTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = a.AMST_FirstName,
                                             AMST_AdmNo = a.AMST_AdmNo,
                                             AMST_RegistrationNo = a.AMST_RegistrationNo == null ? "" : a.AMST_RegistrationNo,
                                             AMAY_RollNo = a.AMAY_RollNo,
                                             ISMS_Id = a.ISMS_Id,
                                             ISMS_SubjectName = a.ISMS_SubjectName,
                                             EYCES_MaxMarks = a.EYCES_MaxMarks,
                                             EYCES_MarksEntryMax = a.EYCES_MarksEntryMax,
                                             EYCES_MinMarks = a.EYCES_MinMarks,
                                             ESTM_Marks = b.ESTM_Marks,
                                             ESTM_Grade = b.ESTM_Grade,
                                             ESTM_Flg = b.ESTM_Flg,
                                             ESTM_Id = b.ESTM_Id
                                         }).Distinct().OrderBy(t => order).ToList();

                    var propertyInfo3 = typeof(temp_marks_DTO).GetProperty(order);
                    saved_studentList = saved_studentList.OrderBy(x => propertyInfo3.GetValue(x, null)).ToList();

                    data.saved_studentList = saved_studentList.Distinct().ToArray();

                    data.saved_ss_list = (from a in _examcontext.Exm_Student_Marks_SubSubjectDMO
                                          from b in stu_marks
                                          where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ESTM_Id == b.ESTM_Id)
                                          select a).Distinct().ToArray();
                }
                else if (alrdy_stu_count > 0 && data.EYCES_SubExamFlg)
                {
                    var stu_marks = _examcontext.ExamMarksDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.ISMS_Id == data.ISMS_Id).Distinct().ToList();

                    List<temp_marks_DTO> saved_studentList = new List<temp_marks_DTO>();

                    saved_studentList = (from a in studentList
                                         from b in stu_marks
                                         where (a.AMST_Id == b.AMST_Id && b.ISMS_Id == a.ISMS_Id && stu_list_mapped.Contains(a.AMST_Id))
                                         select new temp_marks_DTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = a.AMST_FirstName,
                                             AMST_AdmNo = a.AMST_AdmNo,
                                             AMST_RegistrationNo = a.AMST_RegistrationNo == null ? "" : a.AMST_RegistrationNo,
                                             AMAY_RollNo = a.AMAY_RollNo,
                                             ISMS_Id = a.ISMS_Id,
                                             ISMS_SubjectName = a.ISMS_SubjectName,
                                             EYCES_MaxMarks = a.EYCES_MaxMarks,
                                             EYCES_MarksEntryMax = a.EYCES_MarksEntryMax,
                                             EYCES_MinMarks = a.EYCES_MinMarks,
                                             ESTM_Marks = b.ESTM_Marks,
                                             ESTM_Grade = b.ESTM_Grade,
                                             ESTM_Flg = b.ESTM_Flg,
                                             ESTM_Id = b.ESTM_Id
                                         }).Distinct().OrderBy(t => order).ToList();

                    var propertyInfo3 = typeof(temp_marks_DTO).GetProperty(order);
                    saved_studentList = saved_studentList.OrderBy(x => propertyInfo3.GetValue(x, null)).ToList();


                    data.saved_studentList = saved_studentList.Distinct().ToArray();

                    data.saved_se_list = (from a in _examcontext.Exm_Student_Marks_SubSubjectDMO
                                          from b in stu_marks
                                          where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ESTM_Id == b.ESTM_Id)
                                          select a).Distinct().ToArray();
                }

            }
            catch (Exception ee)
            {
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        //CalculationHHS
    }
}

