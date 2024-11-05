using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CommonLibrary;
using System.Data;
using System.Data.SqlClient;
using DomainModel.Model.com.vapstech.MobileApp;
using System.Dynamic;
using DomainModel.Model.com.vapstech.Exam;

namespace ExamServiceHub.com.vaps.Services
{
    public class MarksEntryImpl : Interfaces.MarksEntryInterface
    {
        private static ConcurrentDictionary<string, MasterSubjectGroupDTO> _login =
       new ConcurrentDictionary<string, MasterSubjectGroupDTO>();

        public ExamContext _examcontext;
        public DomainModelMsSqlServerContext _db;
        ILogger<MarksEntryImpl> _acdimpl;
        public MarksEntryImpl(ExamContext ttcategory, DomainModelMsSqlServerContext db, ILogger<MarksEntryImpl> acdimpl)
        {
            _examcontext = ttcategory;
            _db = db;
            _acdimpl = acdimpl;
        }
        public ExamMarksDTO getdetails(ExamMarksDTO id)
        {
            ExamMarksDTO EM = new ExamMarksDTO();
            try
            {
                EM.ASMAY_Id = id.ASMAY_Id;
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _examcontext.AcademicYear.Where(t => t.MI_Id == id.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToList();
                EM.Acdlist = list.ToArray();

                if (id.stringmobileorportal == "Mobile")
                {
                    List<IVRM_User_MobileApp_Login_Privileges> Staffmobileappprivileges = new List<IVRM_User_MobileApp_Login_Privileges>();
                    Staffmobileappprivileges = _db.IVRM_User_MobileApp_Login_Privileges.Where(t => t.IVRMUL_Id == id.Id && t.MI_Id == id.MI_Id).ToList();

                    if (Staffmobileappprivileges.Count() > 0)
                    {
                        EM.Staffmobileappprivileges = (from Mobilepage in _examcontext.IVRM_MobileApp_Page
                                                       from MobileRolePrivileges in _examcontext.IVRM_Role_MobileApp_Privileges
                                                       from UserRolePrivileges in _examcontext.IVRM_User_MobileApp_Login_Privileges
                                                       where (MobileRolePrivileges.MI_ID == UserRolePrivileges.MI_Id
                                                       && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id
                                                       && Mobilepage.IVRMMAP_Id == UserRolePrivileges.IVRMMAP_Id && MobileRolePrivileges.IVRMRT_Id == id.roleid
                                                       && MobileRolePrivileges.MI_ID == id.MI_Id && UserRolePrivileges.IVRMUL_Id == id.Id)
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

                        EM.mobileprivileges = "true";
                    }
                    else
                    {
                        EM.mobileprivileges = "false";
                    }
                }

                EM.ctlist = onselectAcdYear(id).ctlist;

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return EM;
        }
        public ExamMarksDTO onselectAcdYear(ExamMarksDTO id)
        {
            ExamMarksDTO EM = new ExamMarksDTO();
            try
            {
                var classid = _examcontext.Masterclasscategory.Where(t => t.MI_Id == id.MI_Id && t.Is_Active == true && t.Is_Active == true
                && t.ASMAY_Id == id.ASMAY_Id).Select(t => t.ASMCL_Id).ToArray();

                var classexmid = (from e in _examcontext.Staff_User_Login
                                  from f in _examcontext.Exm_Login_PrivilegeDMO
                                  from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                  where (e.Id == id.Id && id.MI_Id == id.MI_Id &&
                                    f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == id.ASMAY_Id && f.MI_Id == id.MI_Id && classid.Contains(i.ASMCL_Id)
                                    && f.ELP_Id == i.ELP_Id && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true)
                                  select new ExamMarksDTO
                                  {
                                      ASMCL_Id = i.ASMCL_Id
                                  }).Distinct().Select(t => t.ASMCL_Id).ToArray();

                List<AdmissionClass> clist = new List<AdmissionClass>();
                clist = _examcontext.AdmissionClass.Where(t => t.MI_Id == id.MI_Id && t.ASMCL_ActiveFlag == true && classexmid.Contains(t.ASMCL_Id)).ToList();
                EM.ctlist = clist.OrderBy(t => t.ASMCL_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return EM;
        }
        public ExamMarksDTO onselectclass(ExamMarksDTO id)
        {
            ExamMarksDTO EM = new ExamMarksDTO();
            try
            {
                var classid = _examcontext.Masterclasscategory.Where(t => t.MI_Id == id.MI_Id && t.Is_Active == true && t.ASMAY_Id == id.ASMAY_Id && t.ASMCL_Id == id.ASMCL_Id).Select(t => t.ASMCC_Id).ToArray();

                var secid = _examcontext.AdmSchoolMasterClassCatSec.Where(t => classid.Contains(t.ASMCC_Id)).Select(t => t.ASMS_Id).ToArray();

                var sectionexamid = (from e in _examcontext.Staff_User_Login
                                     from f in _examcontext.Exm_Login_PrivilegeDMO
                                     from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                     where (e.Id == id.Id && id.MI_Id == id.MI_Id &&
                                       f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == id.ASMAY_Id && f.MI_Id == id.MI_Id && i.ASMCL_Id == id.ASMCL_Id && secid.Contains(i.ASMS_Id)
                                       && f.ELP_Id == i.ELP_Id && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true)
                                     select new ExamMarksDTO
                                     {
                                         ASMS_Id = i.ASMS_Id
                                     }).Distinct().Select(t => t.ASMS_Id).ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = _examcontext.School_M_Section.Where(t => t.MI_Id == id.MI_Id && t.ASMC_ActiveFlag == 1 && sectionexamid.Contains(t.ASMS_Id)).ToList();
                EM.seclist = seclist.OrderBy(t => t.ASMC_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return EM;
        }
        public ExamMarksDTO onselectSection(ExamMarksDTO id)
        {
            ExamMarksDTO EM = new ExamMarksDTO();
            try
            {
                var catid = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == id.ASMAY_Id && t.ASMCL_Id == id.ASMCL_Id
                && t.ASMS_Id == id.ASMS_Id && t.MI_Id == id.MI_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).ToArray();

                var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id
                && catid.Contains(t.EMCA_Id) && t.EYC_ActiveFlg == true).Select(t => t.EYC_Id).ToArray();

                var emeid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id) && t.EYCE_ActiveFlg == true).Select(t => t.EME_Id).ToArray();

                List<exammasterDMO> examlist = new List<exammasterDMO>();
                examlist = _examcontext.exammasterDMO.Where(t => t.MI_Id == id.MI_Id && t.EME_ActiveFlag == true && emeid.Contains(t.EME_Id)).ToList();
                EM.examlist = examlist.ToArray();

                if (EM.examlist.Length == 1)
                {
                    var eyceid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id)).Select(t => t.EYCE_Id).ToArray();

                    var subid = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id)
                    && t.EYCES_ActiveFlg == true).Select(t => t.ISMS_Id).ToArray();

                    var sectionexamid = (from e in _examcontext.Staff_User_Login
                                         from f in _examcontext.Exm_Login_PrivilegeDMO
                                         from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                         where (e.Id == id.Id && id.MI_Id == id.MI_Id && f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == id.ASMAY_Id
                                         && f.MI_Id == id.MI_Id && i.ASMS_Id == id.ASMS_Id && i.ASMCL_Id == id.ASMCL_Id && f.ELP_Id == i.ELP_Id
                                         && subid.Contains(i.ISMS_Id) && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true)
                                         select new ExamMarksDTO
                                         {
                                             ISMS_Id = i.ISMS_Id
                                         }).Distinct().Select(t => t.ISMS_Id).ToArray();


                    List<IVRM_School_Master_SubjectsDMO> subjects = new List<IVRM_School_Master_SubjectsDMO>();
                    subjects = _examcontext.IVRM_School_Master_SubjectsDMO.Where(c => c.MI_Id == id.MI_Id && c.ISMS_ActiveFlag == 1 && c.ISMS_ExamFlag == 1
                    && sectionexamid.Contains(c.ISMS_Id)).ToList();
                    EM.subjectlist = subjects.OrderBy(t => t.ISMS_OrderFlag).ToArray();
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return EM;
        }
        public ExamMarksDTO onselectExam(ExamMarksDTO id)
        {
            ExamMarksDTO EM = new ExamMarksDTO();
            try
            {
                var catid = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == id.ASMAY_Id && t.ASMCL_Id == id.ASMCL_Id && t.ASMS_Id == id.ASMS_Id && t.MI_Id == id.MI_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).ToArray();

                var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id && t.EYC_ActiveFlg == true
                && catid.Contains(t.EMCA_Id)).Select(t => t.EYC_Id).ToArray();

                var eyceid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id) && t.EYCE_ActiveFlg == true).Select(t => t.EYCE_Id).ToArray();

                var subid = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id) && t.EYCES_ActiveFlg == true).Select(t => t.ISMS_Id).ToArray();

                var sectionexamid = (from e in _examcontext.Staff_User_Login
                                     from f in _examcontext.Exm_Login_PrivilegeDMO
                                     from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                     where (e.Id == id.Id && e.MI_Id == id.MI_Id && f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == id.ASMAY_Id && f.MI_Id == id.MI_Id
                                     && i.ASMS_Id == id.ASMS_Id && i.ASMCL_Id == id.ASMCL_Id && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true
                                     && f.ELP_Id == i.ELP_Id && subid.Contains(i.ISMS_Id))
                                     select new ExamMarksDTO
                                     {
                                         ISMS_Id = i.ISMS_Id
                                     }).Distinct().Select(t => t.ISMS_Id).ToArray();


                List<IVRM_School_Master_SubjectsDMO> subjects = new List<IVRM_School_Master_SubjectsDMO>();
                subjects = _examcontext.IVRM_School_Master_SubjectsDMO.Where(c => c.MI_Id == id.MI_Id && c.ISMS_ActiveFlag == 1 && c.ISMS_ExamFlag == 1
                && sectionexamid.Contains(c.ISMS_Id)).ToList();

                EM.subjectlist = subjects.OrderBy(t => t.ISMS_OrderFlag).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return EM;
        }
        public ExamMarksDTO onselectSubject(ExamMarksDTO id)
        {
            try
            {
                var catid = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == id.ASMAY_Id && t.ASMCL_Id == id.ASMCL_Id && t.ASMS_Id == id.ASMS_Id
                && t.MI_Id == id.MI_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).ToArray();

                var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id && t.EYC_ActiveFlg == true
                && catid.Contains(t.EMCA_Id)).Select(t => t.EYC_Id).ToArray();

                var eyceid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id) && t.EME_Id == id.EME_Id
                && t.EYCE_ActiveFlg == true).Select(t => t.EYCE_Id).ToArray();

                var subid = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id) && t.ISMS_Id == id.ISMS_Id
                && t.EYCES_ActiveFlg == true).ToList();

                var subidnew = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id) && t.EYCES_ActiveFlg == true
                && t.ISMS_Id == id.ISMS_Id).Select(t => t.EYCES_Id).ToArray();

                var subsubjectid = _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO.Where(t => subidnew.Contains(t.EYCES_Id)
                && t.EYCESSS_ActiveFlg == true).Select(t => t.EMSS_Id).ToArray();

                var subexamid = _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO.Where(t => subidnew.Contains(t.EYCES_Id)
                && t.EYCESSS_ActiveFlg == true).Select(t => t.EMSE_Id).ToArray();

                if (subid.FirstOrDefault().EYCES_SubSubjectFlg == true)
                {
                    var sectionexamid = (from e in _examcontext.Staff_User_Login
                                         from f in _examcontext.Exm_Login_PrivilegeDMO
                                         from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                         from j in _examcontext.Exm_Login_Privilege_SubSubjectsDMO
                                         from g in _examcontext.mastersubsubject
                                         where (e.IVRMSTAUL_Id == f.Login_Id && f.ELP_Id == i.ELP_Id && i.ELPS_Id == j.ELPS_Id && j.EMSS_Id == g.EMSS_Id
                                         && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true && j.ELPSS_ActiveFlg == true && f.ASMAY_Id == id.ASMAY_Id
                                         && i.ASMCL_Id == id.ASMCL_Id && i.ASMS_Id == id.ASMS_Id && i.ISMS_Id == id.ISMS_Id && e.Id == id.Id
                                         && subsubjectid.Contains(j.EMSS_Id) && f.MI_Id == id.MI_Id)
                                         select new ExamMarksDTO
                                         {
                                             EMSS_Id = j.EMSS_Id
                                         }).Distinct().Select(t => t.EMSS_Id).ToArray();

                    List<mastersubsubjectDMO> subsubjects = new List<mastersubsubjectDMO>();
                    subsubjects = _examcontext.mastersubsubject.Where(c => c.MI_Id == id.MI_Id && c.EMSS_ActiveFlag == true
                    && sectionexamid.Contains(c.EMSS_Id)).ToList();

                    id.subsubjectlist = subsubjects.OrderBy(t => t.EMSS_Order).ToArray();

                }
                else if (subid.FirstOrDefault().EYCES_SubExamFlg == true)
                {
                    List<mastersubexamDMO> mastersubexamDMO = new List<mastersubexamDMO>();
                    mastersubexamDMO = _examcontext.mastersubexam.Where(c => c.MI_Id == id.MI_Id && c.EMSE_ActiveFlag == true
                    && subexamid.Contains(c.EMSE_Id)).ToList();

                    id.subexamlist = mastersubexamDMO.OrderBy(t => t.EMSE_SubExamOrder).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return id;
        }
        public ExamMarksDTO onchangesubsubject(ExamMarksDTO id)
        {
            try
            {
                var catid = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == id.ASMAY_Id && t.ASMCL_Id == id.ASMCL_Id && t.ASMS_Id == id.ASMS_Id
                && t.MI_Id == id.MI_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).ToArray();

                var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id && t.EYC_ActiveFlg == true
                && catid.Contains(t.EMCA_Id)).Select(t => t.EYC_Id).ToArray();

                var eyceid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id) && t.EME_Id == id.EME_Id
                && t.EYCE_ActiveFlg == true).Select(t => t.EYCE_Id).ToArray();

                var subid = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id) && t.ISMS_Id == id.ISMS_Id
                && t.EYCES_ActiveFlg == true).ToList();

                var subidnew = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id) && t.EYCES_ActiveFlg == true
                && t.ISMS_Id == id.ISMS_Id).Select(t => t.EYCES_Id).ToArray();

                var subexamid = _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO.Where(t => subidnew.Contains(t.EYCES_Id)
                && t.EYCESSS_ActiveFlg == true && t.EMSS_Id == id.EMSS_Id).Select(t => t.EMSE_Id).ToArray();

                List<mastersubexamDMO> mastersubexamDMO = new List<mastersubexamDMO>();
                mastersubexamDMO = _examcontext.mastersubexam.Where(c => c.MI_Id == id.MI_Id && c.EMSE_ActiveFlag == true
                && subexamid.Contains(c.EMSE_Id)).ToList();

                id.subexamlist = mastersubexamDMO.OrderBy(t => t.EMSE_SubExamOrder).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return id;
        }
        public async Task<ExamMarksDTO> onsearch(ExamMarksDTO id)
        {
            ExamMarksDTO EM = new ExamMarksDTO();
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())                {
                    //cmd.CommandText = "Exam_get_Marks_Entry_Modify";
                    cmd.CommandText = "Exam_Duplicate_MarksEntry_Delete";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = id.ASMAY_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = id.ASMCL_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = id.ASMS_Id });                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = id.MI_Id });                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = id.EME_Id });                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = id.ISMS_Id });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)                                {                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);                                }                                retObject.Add((ExpandoObject)dataRow1);                            }                        }

                    }                    catch (Exception ex)                    {                        _acdimpl.LogError(ex.Message);                        _acdimpl.LogDebug(ex.Message);                    }                }
                MarksCalcReset MarksCalcReset = new MarksCalcReset(_examcontext);
                EM.marksdeleteflag = MarksCalcReset.MarksCalculationResetFlag(id.ASMAY_Id, id.ASMCL_Id, id.ASMS_Id, id.MI_Id, id.EME_Id);

                var get_configuration = _examcontext.Exm_ConfigurationDMO.Where(a => a.MI_Id == id.MI_Id).ToList();
                EM.configuration = get_configuration.ToArray();

                var catid = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == id.ASMAY_Id && t.ASMCL_Id == id.ASMCL_Id && t.ASMS_Id == id.ASMS_Id
                && t.MI_Id == id.MI_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).ToArray();

                var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id
                && catid.Contains(t.EMCA_Id) && t.EYC_ActiveFlg == true).Select(t => t.EYC_Id).ToArray();

                var eyceid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id) && t.EME_Id == id.EME_Id
                && t.EYCE_ActiveFlg == true).Select(t => t.EYCE_Id).ToArray();

                var subid = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id) && t.ISMS_Id == id.ISMS_Id
                && t.EYCES_ActiveFlg == true).Select(t => t.ISMS_Id).ToArray();

                var subsubjectid = _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO.Where(t => subid.Contains(t.EYCES_Id)
               && t.EYCESSS_ActiveFlg == true).Select(t => t.EMSS_Id).ToArray();

                var eycid_details = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id && t.EYC_ActiveFlg == true
            && catid.Contains(t.EMCA_Id) && t.EYC_BasedOnPaperTypeFlg == true).ToList();

                var subMorGFlag = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id) && t.ISMS_Id == id.ISMS_Id).ToArray();

                EM.subMorGFlag = subMorGFlag[0].EYCES_MarksGradeEntryFlg;

                EM.EMGR_Id = subMorGFlag[0].EMGR_Id;

                if (EM.subMorGFlag == "G")
                {
                    EM.gradname = (from a in _examcontext.Exm_Master_GradeDMO
                                   from b in _examcontext.Exm_Master_Grade_DetailsDMO
                                   where (a.MI_Id == id.MI_Id && a.EMGR_Id == EM.EMGR_Id && b.EMGR_Id == EM.EMGR_Id)
                                   select new ExamMarksDTO
                                   {
                                       grade = b.EMGD_Name
                                   }).Select(b => b.grade).ToArray();
                }

                List<ExamMarksDTO> result = new List<ExamMarksDTO>();

                if (eycid_details.Count > 0)
                {
                    EM.get_student_wise_papertype_list = (from k in _examcontext.Exm_Student_Examwise_PTDMO
                                                          from b in _examcontext.Exm_Master_PaperTypeDMO
                                                          where (k.EMPATY_Id == b.EMPATY_Id && k.MI_Id == id.MI_Id && k.ASMAY_Id == id.ASMAY_Id
                                                          && k.ASMCL_Id == id.ASMCL_Id && k.ASMS_Id == id.ASMS_Id && k.ISMS_Id == id.ISMS_Id
                                                          && k.ESEWPT_ActiveFlg == true && k.EME_Id == id.EME_Id && b.MI_Id == id.MI_Id)
                                                          select new MarksEntryHHSDTO
                                                          {
                                                              EMPATY_PaperTypeName = b.EMPATY_PaperTypeName,
                                                              AMST_Id = k.AMST_Id,
                                                              EMPATY_Id = k.EMPATY_Id,
                                                          }).Distinct().ToArray();

                    EM.get_papertype_grade_details = (from a in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                                      from b in _examcontext.Exm_Yrly_Cat_Exams_Subwise_PTDMO
                                                      from c in _examcontext.Exm_Master_PaperTypeDMO
                                                      from d in _examcontext.Exm_Master_GradeDMO
                                                      from f in _examcontext.Exm_Master_Grade_DetailsDMO
                                                      where (a.EYCES_Id == b.EYCES_Id && b.EMPATY_Id == c.EMPATY_Id && b.EMGR_Id == d.EMGR_Id
                                                      && d.EMGR_Id == f.EMGR_Id && eyceid.Contains(a.EYCE_Id) && a.ISMS_Id == id.ISMS_Id
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

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "Exam_get_Marks_Entry_Modify";
                    cmd.CommandText = "Exam_Get_Students_Subjects_Marks_Entry";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = id.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = id.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = id.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = id.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = id.EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = id.ISMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EMSS_Id", SqlDbType.VarChar) { Value = id.EMSS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EMSE_Id", SqlDbType.VarChar) { Value = id.EMSE_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                result.Add(new ExamMarksDTO
                                {
                                    AMST_Id = Convert.ToInt64(dataReader["AMST_Id"].ToString()),
                                    ESTM_Id = Convert.ToInt32(dataReader["ESTM_Id"].ToString()),
                                    studentname = ((dataReader["AMST_FirstName"].ToString() == null ? " " : dataReader["AMST_FirstName"].ToString()) + " " + (dataReader["AMST_MiddleName"].ToString() == null ? " " : dataReader["AMST_MiddleName"].ToString()) + " " + (dataReader["AMST_LastName"].ToString() == null ? " " : dataReader["AMST_LastName"].ToString())).Trim(),
                                    amsT_AdmNo = dataReader["AMST_AdmNo"].ToString() == null ? "" : dataReader["AMST_AdmNo"].ToString(),
                                    amsT_RegistrationNo = dataReader["AMST_RegistrationNo"].ToString() == null ? "" : dataReader["AMST_RegistrationNo"].ToString(),
                                    amaY_RollNo = Convert.ToInt32(dataReader["AMAY_RollNo"].ToString()),
                                    SubjectName = dataReader["ISMS_SubjectName"].ToString(),
                                    TotalMarks = Convert.ToDecimal(dataReader["EYCES_MaxMarks"].ToString()),
                                    MarksEnterFor = Convert.ToDecimal(dataReader["EYCES_MarksEntryMax"].ToString()),
                                    MinMarks = Convert.ToDecimal(dataReader["EYCES_MinMarks"].ToString()),
                                    ESTM_Grade = (dataReader["ESTM_Flg"].ToString() == "" ? dataReader["ESTM_Grade"].ToString() : dataReader["ESTM_Flg"].ToString()),
                                    obtainmarks = (dataReader["ESTM_Flg"].ToString() == "" ? dataReader["ESTM_Marks"].ToString() : dataReader["ESTM_Flg"].ToString())
                                });

                                //EM.studentList = result.ToArray();
                                EM.studentList = result.Distinct().ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                id.messagesub = "";
                if (subMorGFlag[0].EYCES_SubSubjectFlg == false && subMorGFlag[0].EYCES_SubExamFlg == false)
                {
                    var checkmarkssaved = _examcontext.ExamMarksDMO.Where(a => a.MI_Id == id.MI_Id && a.ASMAY_Id == id.ASMAY_Id && a.ASMCL_Id == id.ASMCL_Id
                    && a.ASMS_Id == id.ASMS_Id && a.EME_Id == id.EME_Id && a.ISMS_Id == id.ISMS_Id && a.ESTM_ActiveFlg == true).ToList();
                    try
                    {
                        EM.saveupdatecount = checkmarkssaved.Count();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    EM.messagesub = "can";
                }

                else if (subMorGFlag[0].EYCES_SubSubjectFlg == true && subMorGFlag[0].EYCES_SubExamFlg == false)
                {
                    List<long> getsubsubjectid = new List<long>();

                    var checkmarks = (from a in _examcontext.ExamMarksDMO
                                      from b in _examcontext.Exm_Student_Marks_SubSubjectDMO
                                      where (a.ESTM_Id == b.ESTM_Id && a.ASMAY_Id == id.ASMAY_Id && a.ASMCL_Id == id.ASMCL_Id && a.ASMS_Id == id.ASMS_Id
                                      && a.EME_Id == id.EME_Id && a.ISMS_Id == id.ISMS_Id && b.ISMS_Id == id.ISMS_Id && a.MI_Id == id.MI_Id
                                      && b.EMSS_Id == id.EMSS_Id && a.ESTM_ActiveFlg == true)
                                      select a).Distinct().ToList();

                    EM.saveupdatecount = checkmarks.Count();
                }

                else if (subMorGFlag[0].EYCES_SubSubjectFlg == false && subMorGFlag[0].EYCES_SubExamFlg == true)
                {

                    var checkmarks = (from a in _examcontext.ExamMarksDMO
                                      from b in _examcontext.Exm_Student_Marks_SubSubjectDMO
                                      where (a.ESTM_Id == b.ESTM_Id && a.ASMAY_Id == id.ASMAY_Id && a.ASMCL_Id == id.ASMCL_Id && a.ASMS_Id == id.ASMS_Id
                                      && a.EME_Id == id.EME_Id && a.ISMS_Id == id.ISMS_Id && b.ISMS_Id == id.ISMS_Id && a.MI_Id == id.MI_Id
                                      && b.EMSE_Id == id.EMSE_Id && a.ESTM_ActiveFlg == true)
                                      select a).Distinct().ToList();

                    EM.saveupdatecount = checkmarks.Count();
                }

                else if (subMorGFlag[0].EYCES_SubSubjectFlg == true && subMorGFlag[0].EYCES_SubExamFlg == true)
                {
                    List<long> getsubsubjectid = new List<long>();

                    var getemssid = (from a in _examcontext.Exm_Login_PrivilegeDMO
                                     from b in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                     from c in _examcontext.Exm_Login_Privilege_SubSubjectsDMO
                                     from g in _examcontext.Staff_User_Login
                                     where (a.ELP_Id == b.ELP_Id && g.IVRMSTAUL_Id == a.Login_Id && b.ELPS_Id == c.ELPS_Id && a.ELP_ActiveFlg == true
                                     && b.ELPs_ActiveFlg == true && c.ELPSS_ActiveFlg == true && a.ASMAY_Id == id.ASMAY_Id && a.MI_Id == id.MI_Id
                                     && b.ASMCL_Id == id.ASMCL_Id && b.ASMS_Id == id.ASMS_Id && b.ISMS_Id == id.ISMS_Id && g.Id == id.Id)
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
                                      where (a.ESTM_Id == b.ESTM_Id && a.ASMAY_Id == id.ASMAY_Id && a.ASMCL_Id == id.ASMCL_Id && a.ASMS_Id == id.ASMS_Id
                                      && a.EME_Id == id.EME_Id && a.ISMS_Id == id.ISMS_Id && b.ISMS_Id == id.ISMS_Id && a.MI_Id == id.MI_Id
                                      && getsubsubjectid.Contains(b.EMSS_Id) && b.EMSS_Id == id.EMSS_Id && b.EMSE_Id == id.EMSE_Id && a.ESTM_ActiveFlg == true)
                                      select a).Distinct().ToList();

                    EM.saveupdatecount = checkmarks.Count();
                }

                // MARKS ENTRY SCHDULER

                var checklastdateisnullornot = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYCE_ActiveFlg == true && eyceid.Contains(t.EYCE_Id)).ToList();

                if (checklastdateisnullornot.FirstOrDefault().EYCE_MarksEntryLastDate != null)
                {
                    var eyceidlastdateentry = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYCE_ActiveFlg == true && eyceid.Contains(t.EYCE_Id)
                    && Convert.ToDateTime(System.DateTime.Today.Date) <= Convert.ToDateTime(t.EYCE_MarksEntryLastDate)).Distinct().ToList();

                    EM.lastdateentry = eyceidlastdateentry.Count();

                    if (eyceidlastdateentry.Count() == 0)
                    {
                        // Checking The Login User Having Authority To Enter The Marks

                        var getstaffid = _examcontext.Staff_User_Login.Where(a => a.Id == id.Id).ToList();

                        var checkspecialuser = _examcontext.UserPromotion_DMO.Where(a => eyceid.Contains(a.EYCE_Id) && a.IVRMUL_Id == id.Id).ToList();

                        if (checkspecialuser.Count() > 0)
                        {
                            var eyceidlastdateentryuser = _examcontext.UserPromotion_DMO.Where(t => t.EYCESU_ActiveFlg == true
                            && eyceid.Contains(t.EYCE_Id) && t.IVRMUL_Id == id.Id
                            && Convert.ToDateTime(System.DateTime.Today.Date) >= Convert.ToDateTime(t.EYCESU_MarksEntryFromDate)
                            && Convert.ToDateTime(System.DateTime.Today.Date) <= Convert.ToDateTime(t.EYCESU_MarksEntryToDate)).Distinct().ToList();

                            EM.lastdateentry = eyceidlastdateentryuser.Count();

                            if (eyceidlastdateentryuser.Count() > 0)
                            {
                                EM.lastdateentry = 1000;
                                EM.lastdateentryflag = true;
                            }
                            else
                            {
                                EM.lastdateentry = 0;
                                EM.lastdateentryflag = false;
                            }

                        }
                        else
                        {
                            EM.lastdateentry = 0;
                            EM.lastdateentryflag = false;
                        }
                    }
                    else
                    {
                        EM.lastdateentry = 1000;
                        EM.lastdateentryflag = true;
                    }
                }
                else
                {
                    EM.lastdateentry = 1000;
                    EM.lastdateentryflag = true;
                }

                if (checklastdateisnullornot.FirstOrDefault().EYCE_ExamEndDate != null)
                {
                    var eyceidexamlastdateentry = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYCE_ActiveFlg == true
                    && eyceid.Contains(t.EYCE_Id) && Convert.ToDateTime(System.DateTime.Today.Date) >= Convert.ToDateTime(t.EYCE_ExamEndDate)).Distinct().ToList();

                    if (eyceidexamlastdateentry.Count() > 0)
                    {
                        EM.lastdateexam = eyceidexamlastdateentry.Count();
                        EM.lastdateexamflag = true;
                    }
                    else
                    {
                        var examlastdateentry = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYCE_ActiveFlg == true
                        && eyceid.Contains(t.EYCE_Id)).Distinct().ToList();

                        EM.lastdateexam = 0;
                        EM.lastdateexamflag = false;
                        EM.marksentrystatedate = examlastdateentry.FirstOrDefault().EYCE_ExamEndDate;
                    }
                }
                else
                {
                    EM.lastdateexam = 2000;
                    EM.lastdateexamflag = true;
                }

                //deleted Marks
               
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return EM;
        }
        public ExamMarksDTO SaveMarks(ExamMarksDTO id)
        {
            try
            {
                //MarksCalcReset MarksCalcReset = new MarksCalcReset(_examcontext);
                //id.marksdeleteflag = MarksCalcReset.MarksCalculationReset(id.ASMAY_Id, id.ASMCL_Id, id.ASMS_Id, id.MI_Id, id.EME_Id);

                var catid = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == id.ASMAY_Id && t.ASMCL_Id == id.ASMCL_Id && t.ASMS_Id == id.ASMS_Id && t.MI_Id == id.MI_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).ToArray();

                var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id && catid.Contains(t.EMCA_Id)
                && t.EYC_ActiveFlg == true).Select(t => t.EYC_Id).ToArray();

                var eycidlist = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id && catid.Contains(t.EMCA_Id)
            && t.EYC_ActiveFlg == true).ToList();

                var eyceid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id) && t.EME_Id == id.EME_Id
                && t.EYCE_ActiveFlg == true).Select(t => t.EYCE_Id).ToArray();

                var subid = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id) && t.EYCES_ActiveFlg == true).ToArray();

                var subMorGFlag = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id) && t.EYCES_ActiveFlg == true
                && t.ISMS_Id == id.ISMS_Id).ToArray();

                id.subMorGFlag = subMorGFlag[0].EYCES_MarksGradeEntryFlg;

                id.EMGR_Id = subMorGFlag[0].EMGR_Id;

                string msg = "";

                // WHEN SUB SUBJECT AND SUB EXAM IS NOT THERE
                if (subMorGFlag[0].EYCES_SubSubjectFlg == false && subMorGFlag[0].EYCES_SubExamFlg == false)
                {
                    if (id.detailsList.Length > 0)
                    {
                        for (int i = 0; i < id.detailsList.Length; i++)
                        {
                            if (id.detailsList[i].estM_Id > 0)
                            {
                                var result = _examcontext.ExamMarksDMO.Single(d => d.AMST_Id == id.detailsList[i].amsT_Id
                                && d.MI_Id == id.MI_Id && d.ESTM_Id == id.detailsList[i].estM_Id);

                                result.MI_Id = id.MI_Id;
                                result.ASMCL_Id = id.ASMCL_Id;
                                result.ASMAY_Id = id.ASMAY_Id;
                                result.ASMS_Id = id.ASMS_Id;
                                result.EME_Id = id.EME_Id;
                                result.ISMS_Id = id.ISMS_Id;
                                result.AMST_Id = id.detailsList[i].amsT_Id;

                                if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                                    || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m"
                                    || id.detailsList[i].obtainmarks == "OD" || id.detailsList[i].obtainmarks == "od")
                                {
                                    result.ESTM_Flg = id.detailsList[i].obtainmarks;
                                    result.ESTM_Marks = Convert.ToDecimal("0");
                                    result.ESTM_Grade = "null";
                                    if (id.subMorGFlag == "M")
                                    {
                                        result.ESTM_MarksGradeFlg = "M";
                                    }
                                    else if (id.subMorGFlag == "G")
                                    {
                                        result.ESTM_MarksGradeFlg = "G";
                                    }
                                }
                                else
                                {
                                    result.ESTM_Flg = "";

                                    if (id.subMorGFlag == "M")
                                    {
                                        result.ESTM_MarksGradeFlg = "M";
                                        result.ESTM_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
                                        result.ESTM_Grade = "null";
                                    }
                                    else if (id.subMorGFlag == "G")
                                    {
                                        result.ESTM_MarksGradeFlg = "G";
                                        result.ESTM_Marks = Convert.ToDecimal("0");
                                        result.ESTM_Grade = id.detailsList[i].obtainmarks; //id.ESTM_Grade;
                                    }
                                }

                                result.UpdatedDate = DateTime.Now;
                                result.Id = id.Id;
                                result.ESTM_UpdatedBy = id.Id;
                                result.IP4 = id.IP4;
                                _examcontext.Update(result);
                            }
                            else
                            {
                                var duplicate = _examcontext.ExamMarksDMO.Where(R => R.MI_Id == id.MI_Id
                                && R.ASMAY_Id == id.ASMAY_Id && R.ASMCL_Id == id.ASMCL_Id && R.ASMS_Id == id.ASMS_Id && R.EME_Id == id.EME_Id
                                && R.ISMS_Id == id.ISMS_Id && R.AMST_Id == id.detailsList[i].amsT_Id && R.ESTM_ActiveFlg==true).ToList();
                                if (duplicate.Count > 0)
                                {

                                }
                                else
                                {

                                    ExamMarksDMO MM = new ExamMarksDMO();
                                   //ExamMarksDMO MM = Mapper.Map<ExamMarksDMO>(id);
                                    MM.MI_Id = id.MI_Id;
                                    MM.ASMCL_Id = id.ASMCL_Id;
                                    MM.ASMAY_Id = id.ASMAY_Id;
                                    MM.ASMS_Id = id.ASMS_Id;
                                    MM.EME_Id = id.EME_Id;
                                    MM.ISMS_Id = id.ISMS_Id;
                                    MM.AMST_Id = id.detailsList[i].amsT_Id;
                                    MM.ESTM_OnlineExamFlag = false;
                                    if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                                        || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m"
                                        || id.detailsList[i].obtainmarks == "OD" || id.detailsList[i].obtainmarks == "od")
                                    {
                                        MM.ESTM_Flg = id.detailsList[i].obtainmarks;
                                        MM.ESTM_Marks = Convert.ToDecimal("0");
                                        MM.ESTM_Grade = "null";
                                        if (id.subMorGFlag == "M")
                                        {
                                            MM.ESTM_MarksGradeFlg = "M";
                                        }
                                        else if (id.subMorGFlag == "G")
                                        {
                                            MM.ESTM_MarksGradeFlg = "G";
                                        }
                                    }
                                    else
                                    {
                                        MM.ESTM_Flg = "";

                                        if (id.subMorGFlag == "M")
                                        {
                                            MM.ESTM_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
                                            MM.ESTM_MarksGradeFlg = "M";
                                            MM.ESTM_Grade = "null";
                                        }
                                        else if (id.subMorGFlag == "G")
                                        {
                                            MM.ESTM_Marks = Convert.ToDecimal("0");
                                            MM.ESTM_MarksGradeFlg = "G";
                                            MM.ESTM_Grade = id.detailsList[i].obtainmarks; //id.ESTM_Grade;
                                        }
                                    }

                                    MM.CreatedDate = DateTime.Now;
                                    MM.UpdatedDate = DateTime.Now;
                                    MM.Id = id.Id;
                                    MM.ESTM_UpdatedBy = id.Id;
                                    MM.ESTM_CreatedBy = id.Id;
                                    MM.IP4 = id.IP4;
                                    MM.ESTM_ActiveFlg = true;
                                    _examcontext.Add(MM);
                                }
                            }
                        }

                        int flag = _examcontext.SaveChanges();
                        if (flag > 0)
                        {
                            id.messagesaveupdate = "true";
                        }
                        else
                        {
                            id.messagesaveupdate = "false";
                        }
                    }
                }

                // WHEN SUB SUBJECT IS THERE AND SUB EXAM NOT THERE
                else if (subMorGFlag[0].EYCES_SubSubjectFlg == true && subMorGFlag[0].EYCES_SubExamFlg == false)
                {
                    if (id.detailsList.Length > 0)
                    {
                        for (int i = 0; i < id.detailsList.Length; i++)
                        {
                            decimal? marks = 0;
                            
                            var  stu_id = id.detailsList[i].amsT_Id;

                            var checkdetails = _examcontext.ExamMarksDMO.Where(a => a.MI_Id == id.MI_Id && a.ASMAY_Id == id.ASMAY_Id && a.ASMCL_Id == id.ASMCL_Id
                            && a.ASMS_Id == id.ASMS_Id && a.EME_Id == id.EME_Id && a.ISMS_Id == id.ISMS_Id && a.AMST_Id == stu_id).ToList();

                            if (id.detailsList[i].estM_Id > 0)
                            {
                                var getsubsubjectsubexammarks = (from a in _examcontext.Exm_Student_Marks_SubSubjectDMO
                                                                 from b in _examcontext.ExamMarksDMO
                                                                 where (a.ESTM_Id == b.ESTM_Id && b.MI_Id == id.MI_Id && b.ASMAY_Id == id.ASMAY_Id
                                                                 && b.ASMCL_Id == id.ASMCL_Id && b.ASMS_Id == id.ASMS_Id && b.ISMS_Id == id.ISMS_Id
                                                                 && b.AMST_Id == stu_id && a.EMSS_Id != id.EMSS_Id && b.EME_Id == id.EME_Id && a.ISMS_Id == id.ISMS_Id)
                                                                 select new Exm_Student_Marks_SubSubjectDMO
                                                                 {
                                                                     ESTMSS_Marks = a.ESTMSS_Marks,
                                                                     ESTMSS_Flg = a.ESTMSS_Flg
                                                                 }).ToList();
                                foreach (var m in getsubsubjectsubexammarks)
                                {
                                    marks = marks + m.ESTMSS_Marks;
                                }
                                var resultflag = "";
                                var result_obj = _examcontext.ExamMarksDMO.Single(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id && t.ASMCL_Id == id.ASMCL_Id
                                && t.ASMS_Id == id.ASMS_Id && t.EME_Id == id.EME_Id && t.ISMS_Id == id.ISMS_Id && t.AMST_Id == stu_id);
                                result_obj.ESTM_MarksGradeFlg = id.subMorGFlag;
                                result_obj.Id = id.Id;
                                result_obj.ESTM_UpdatedBy = id.Id;
                                result_obj.LoginDateTime = DateTime.Now;
                                result_obj.IP4 = id.IP4;
                                result_obj.UpdatedDate = DateTime.Now;
                                if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                                 || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m"
                                 || id.detailsList[i].obtainmarks == "OD" || id.detailsList[i].obtainmarks == "od")
                                {
                                    result_obj.ESTM_Flg = id.detailsList[i].obtainmarks;
                                    result_obj.ESTM_Marks = Convert.ToDecimal("0") + Convert.ToDecimal(marks);
                                    result_obj.ESTM_Grade = "null";
                                }
                                else
                                {
                                    result_obj.ESTM_Flg = "";
                                    foreach (var m in getsubsubjectsubexammarks)
                                    {
                                        resultflag = m.ESTMSS_Flg;
                                        if (resultflag == "AB" || resultflag == "ab" || resultflag == "L" || resultflag == "l" || resultflag == "M"
                                            || resultflag == "m" || resultflag == "OD" || resultflag == "od")
                                        {
                                            result_obj.ESTM_Flg = resultflag;
                                            break;
                                        }
                                    }

                                    if (id.subMorGFlag == "M")
                                    {
                                        result_obj.ESTM_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks) + Convert.ToDecimal(marks);
                                        result_obj.ESTM_MarksGradeFlg = "M";
                                        result_obj.ESTM_Grade = "null";
                                    }
                                    else if (id.subMorGFlag == "G")
                                    {
                                        result_obj.ESTM_Marks = Convert.ToDecimal("0") + Convert.ToDecimal(marks);
                                        result_obj.ESTM_MarksGradeFlg = "G";
                                        result_obj.ESTM_Grade = id.detailsList[i].obtainmarks; //id.ESTM_Grade;
                                    }
                                }
                                _examcontext.Update(result_obj);

                                var checksubsubjectmarks = _examcontext.Exm_Student_Marks_SubSubjectDMO.Where(a => a.MI_Id == id.MI_Id
                                && a.ESTM_Id == result_obj.ESTM_Id && a.EMSS_Id == id.EMSS_Id && a.ISMS_Id == id.ISMS_Id).ToList();

                                if (checksubsubjectmarks.Count > 0)
                                {
                                    var resultsubsubjectmarks = _examcontext.Exm_Student_Marks_SubSubjectDMO.Single(a => a.MI_Id == id.MI_Id
                                    && a.ESTM_Id == result_obj.ESTM_Id && a.EMSS_Id == id.EMSS_Id && a.ISMS_Id == id.ISMS_Id);
                                    resultsubsubjectmarks.MI_Id = id.MI_Id;
                                    resultsubsubjectmarks.ESTM_Id = result_obj.ESTM_Id;
                                    resultsubsubjectmarks.EMSS_Id = id.EMSS_Id;
                                    resultsubsubjectmarks.ISMS_Id = id.ISMS_Id;
                                    resultsubsubjectmarks.EMSE_Id = 0;
                                    resultsubsubjectmarks.Login_Id = id.Id;
                                    resultsubsubjectmarks.ESTMSS_UpdatedBy = id.Id;
                                    resultsubsubjectmarks.LoginDateTime = DateTime.Now;
                                    resultsubsubjectmarks.IP4 = id.IP4;
                                    resultsubsubjectmarks.ESTMSS_ActiveFlg = true;
                                    resultsubsubjectmarks.CreatedDate = DateTime.Now;
                                    resultsubsubjectmarks.UpdatedDate = DateTime.Now;
                                    resultsubsubjectmarks.ESTMSS_MarksGradeFlg = id.subMorGFlag;
                                    if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                                     || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m"
                                     || id.detailsList[i].obtainmarks == "OD" || id.detailsList[i].obtainmarks == "od")
                                    {
                                        resultsubsubjectmarks.ESTMSS_Grade = "null";
                                        resultsubsubjectmarks.ESTMSS_Marks = 0;
                                        resultsubsubjectmarks.ESTMSS_Flg = id.detailsList[i].obtainmarks;
                                    }
                                    else
                                    {
                                        resultsubsubjectmarks.ESTMSS_Flg = "";
                                        if (id.subMorGFlag == "M")
                                        {
                                            resultsubsubjectmarks.ESTMSS_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
                                            resultsubsubjectmarks.ESTMSS_Grade = "null";
                                        }
                                        else if (id.subMorGFlag == "G")
                                        {
                                            resultsubsubjectmarks.ESTMSS_Marks = Convert.ToDecimal("0");
                                            resultsubsubjectmarks.ESTMSS_Grade = id.detailsList[i].obtainmarks;
                                        }
                                    }
                                    _examcontext.Update(resultsubsubjectmarks);
                                }
                                else
                                {
                                    
                                    Exm_Student_Marks_SubSubjectDMO obj_S = new Exm_Student_Marks_SubSubjectDMO();
                                    obj_S.MI_Id = id.MI_Id;
                                    obj_S.ESTM_Id = result_obj.ESTM_Id;
                                    obj_S.EMSS_Id = id.EMSS_Id;
                                    obj_S.ISMS_Id = id.ISMS_Id;
                                    obj_S.EMSE_Id = 0;
                                    obj_S.Login_Id = id.Id;
                                    obj_S.ESTMSS_CreatedBy = id.Id;
                                    obj_S.ESTMSS_UpdatedBy = id.Id;
                                    obj_S.LoginDateTime = DateTime.Now;
                                    obj_S.IP4 = id.IP4;
                                    obj_S.ESTMSS_ActiveFlg = true;
                                    obj_S.CreatedDate = DateTime.Now;
                                    obj_S.UpdatedDate = DateTime.Now;
                                    obj_S.ESTMSS_MarksGradeFlg = id.subMorGFlag;
                                    if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                                      || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m"
                                      || id.detailsList[i].obtainmarks == "OD" || id.detailsList[i].obtainmarks == "od")
                                    {
                                        obj_S.ESTMSS_Grade = "null";
                                        obj_S.ESTMSS_Marks = 0;
                                        obj_S.ESTMSS_Flg = id.detailsList[i].obtainmarks;
                                    }
                                    else
                                    {
                                        obj_S.ESTMSS_Flg = "";

                                        if (id.subMorGFlag == "M")
                                        {
                                            obj_S.ESTMSS_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
                                            obj_S.ESTMSS_Grade = "null";
                                        }
                                        else if (id.subMorGFlag == "G")
                                        {
                                            obj_S.ESTMSS_Marks = Convert.ToDecimal("0");
                                            obj_S.ESTMSS_Grade = id.detailsList[i].obtainmarks;
                                        }
                                    }
                                    _examcontext.Add(obj_S);
                                }
                            }

                            else
                            {
                                var duplicate = _examcontext.ExamMarksDMO.Where(R => R.MI_Id == id.MI_Id
                               && R.ASMAY_Id == id.ASMAY_Id && R.ASMCL_Id == id.ASMCL_Id && R.ASMS_Id == id.ASMS_Id && R.EME_Id == id.EME_Id
                               && R.ISMS_Id == id.ISMS_Id && R.AMST_Id == stu_id && R.ESTM_ActiveFlg == true).ToList();
                                if (duplicate.Count > 0)
                                {

                                }
                                else
                                {


                                    ExamMarksDMO obj_M = new ExamMarksDMO();
                                    obj_M.MI_Id = id.MI_Id;
                                    obj_M.ASMAY_Id = id.ASMAY_Id;
                                    obj_M.ASMCL_Id = id.ASMCL_Id;
                                    obj_M.ASMS_Id = id.ASMS_Id;
                                    obj_M.EME_Id = id.EME_Id;
                                    obj_M.ISMS_Id = id.ISMS_Id;
                                    obj_M.AMST_Id = stu_id;
                                    obj_M.ESTM_MarksGradeFlg = id.subMorGFlag;
                                    obj_M.Id = id.Id;
                                    obj_M.ESTM_UpdatedBy = id.Id;
                                    obj_M.ESTM_CreatedBy = id.Id;
                                    obj_M.LoginDateTime = DateTime.Now;
                                    obj_M.IP4 = id.IP4;
                                    obj_M.CreatedDate = DateTime.Now;
                                    obj_M.UpdatedDate = DateTime.Now;
                                    obj_M.ESTM_ActiveFlg = true;
                                    obj_M.ESTM_OnlineExamFlag = false;

                                    if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                                      || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m"
                                      || id.detailsList[i].obtainmarks == "OD" || id.detailsList[i].obtainmarks == "od")
                                    {
                                        obj_M.ESTM_Grade = "null";
                                        obj_M.ESTM_Marks = 0;
                                        obj_M.ESTM_Flg = id.detailsList[i].obtainmarks;
                                    }
                                    else
                                    {
                                        obj_M.ESTM_Flg = "";

                                        if (id.subMorGFlag == "M")
                                        {
                                            obj_M.ESTM_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
                                            obj_M.ESTM_Grade = "null";
                                        }
                                        else if (id.subMorGFlag == "G")
                                        {
                                            obj_M.ESTM_Marks = Convert.ToDecimal("0");
                                            obj_M.ESTM_Grade = id.detailsList[i].obtainmarks;
                                        }
                                    }
                                    _examcontext.Add(obj_M);                                
                                    Exm_Student_Marks_SubSubjectDMO obj_S = new Exm_Student_Marks_SubSubjectDMO();
                                    obj_S.MI_Id = id.MI_Id;
                                    obj_S.ESTM_Id = obj_M.ESTM_Id;
                                    obj_S.EMSS_Id = id.EMSS_Id;
                                    obj_S.ISMS_Id = id.ISMS_Id;
                                    obj_S.EMSE_Id = 0;
                                    obj_S.Login_Id = id.Id;
                                    obj_S.ESTMSS_UpdatedBy = id.Id;
                                    obj_S.ESTMSS_CreatedBy = id.Id;
                                    obj_S.ESTMSS_MarksGradeFlg = id.subMorGFlag;
                                    obj_S.LoginDateTime = DateTime.Now;
                                    obj_S.IP4 = id.IP4;
                                    obj_S.ESTMSS_ActiveFlg = true;
                                    obj_S.CreatedDate = DateTime.Now;
                                    obj_S.UpdatedDate = DateTime.Now;

                                    if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                                      || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m"
                                      || id.detailsList[i].obtainmarks == "OD" || id.detailsList[i].obtainmarks == "od")
                                    {
                                        obj_S.ESTMSS_Grade = "null";
                                        obj_S.ESTMSS_Marks = 0;
                                        obj_S.ESTMSS_Flg = id.detailsList[i].obtainmarks;
                                    }
                                    else
                                    {
                                        obj_S.ESTMSS_Flg = "";

                                        if (id.subMorGFlag == "M")
                                        {
                                            obj_S.ESTMSS_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
                                            obj_S.ESTMSS_Grade = "null";
                                        }
                                        else if (id.subMorGFlag == "G")
                                        {
                                            obj_S.ESTMSS_Marks = Convert.ToDecimal("0");
                                            obj_S.ESTMSS_Grade = id.detailsList[i].obtainmarks;
                                        }
                                    }
                                    _examcontext.Add(obj_S);
                                }
                            }
                        }

                        int flag = _examcontext.SaveChanges();
                        if (flag > 0)
                        {
                            id.messagesaveupdate = "true";
                        }
                        else
                        {
                            id.messagesaveupdate = "false";
                        }
                    }
                }

                // WHEN SUB SUBJECT IS NOT THERE AND SUB EXAM IS THERE
                else if (subMorGFlag[0].EYCES_SubSubjectFlg == false && subMorGFlag[0].EYCES_SubExamFlg == true)
                {
                    if (id.detailsList.Length > 0)
                    {
                        for (int i = 0; i < id.detailsList.Length; i++)
                        {
                            decimal? marks = 0;

                            var stu_id = id.detailsList[i].amsT_Id;

                            var checkdetails = _examcontext.ExamMarksDMO.Where(a => a.MI_Id == id.MI_Id && a.ASMAY_Id == id.ASMAY_Id && a.ASMCL_Id == id.ASMCL_Id
                            && a.ASMS_Id == id.ASMS_Id && a.EME_Id == id.EME_Id && a.ISMS_Id == id.ISMS_Id && a.AMST_Id == stu_id).ToList();

                            if (id.detailsList[i].estM_Id > 0)
                            {
                                var getsubsubjectsubexammarks = (from a in _examcontext.Exm_Student_Marks_SubSubjectDMO
                                                                 from b in _examcontext.ExamMarksDMO
                                                                 where (a.ESTM_Id == b.ESTM_Id && b.MI_Id == id.MI_Id && b.ASMAY_Id == id.ASMAY_Id
                                                                 && b.ASMCL_Id == id.ASMCL_Id && b.ASMS_Id == id.ASMS_Id && b.ISMS_Id == id.ISMS_Id
                                                                 && b.AMST_Id == stu_id && a.EMSE_Id != id.EMSE_Id && b.EME_Id == id.EME_Id && a.ISMS_Id == id.ISMS_Id)
                                                                 select new Exm_Student_Marks_SubSubjectDMO
                                                                 {
                                                                     ESTMSS_Marks = a.ESTMSS_Marks,
                                                                     ESTMSS_Flg = a.ESTMSS_Flg

                                                                 }).ToList();


                                foreach (var m in getsubsubjectsubexammarks)
                                {
                                    marks = marks + m.ESTMSS_Marks;
                                }
                                var resultflag = "";

                                var result_obj = _examcontext.ExamMarksDMO.Single(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id && t.ASMCL_Id == id.ASMCL_Id
                                && t.ASMS_Id == id.ASMS_Id && t.EME_Id == id.EME_Id && t.ISMS_Id == id.ISMS_Id && t.AMST_Id == stu_id);

                                resultflag = result_obj.ESTM_Flg;

                                result_obj.ESTM_MarksGradeFlg = id.subMorGFlag;
                                result_obj.Id = id.Id;
                                result_obj.ESTM_UpdatedBy = id.Id;
                                result_obj.LoginDateTime = DateTime.Now;
                                result_obj.IP4 = id.IP4;
                                result_obj.UpdatedDate = DateTime.Now;

                                if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                                 || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m"
                                 || id.detailsList[i].obtainmarks == "OD" || id.detailsList[i].obtainmarks == "od")
                                {
                                    result_obj.ESTM_Flg = id.detailsList[i].obtainmarks;
                                    result_obj.ESTM_Marks = Convert.ToDecimal("0") + Convert.ToDecimal(marks);
                                    result_obj.ESTM_Grade = "null";
                                }
                                else
                                {
                                    result_obj.ESTM_Flg = "";

                                    foreach (var m in getsubsubjectsubexammarks)
                                    {
                                        resultflag = m.ESTMSS_Flg;
                                        if (resultflag == "AB" || resultflag == "ab" || resultflag == "L" || resultflag == "l" || resultflag == "M"
                                            || resultflag == "m" || resultflag == "OD" || resultflag == "od")
                                        {
                                            result_obj.ESTM_Flg = resultflag;
                                            break;
                                        }
                                    }

                                    if (id.subMorGFlag == "M")
                                    {
                                        result_obj.ESTM_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks) + Convert.ToDecimal(marks);
                                        result_obj.ESTM_MarksGradeFlg = "M";
                                        result_obj.ESTM_Grade = "null";
                                    }
                                    else if (id.subMorGFlag == "G")
                                    {
                                        result_obj.ESTM_Marks = Convert.ToDecimal("0") + Convert.ToDecimal(marks);
                                        result_obj.ESTM_MarksGradeFlg = "G";
                                        result_obj.ESTM_Grade = id.detailsList[i].obtainmarks; //id.ESTM_Grade;
                                    }
                                }
                                _examcontext.Update(result_obj);

                                var checksubsubjectmarks = _examcontext.Exm_Student_Marks_SubSubjectDMO.Where(a => a.MI_Id == id.MI_Id
                                && a.ESTM_Id == result_obj.ESTM_Id && a.EMSE_Id == id.EMSE_Id && a.ISMS_Id == id.ISMS_Id).ToList();

                                if (checksubsubjectmarks.Count > 0)
                                {
                                    var resultsubsubjectmarks = _examcontext.Exm_Student_Marks_SubSubjectDMO.Single(a => a.MI_Id == id.MI_Id
                                    && a.ESTM_Id == result_obj.ESTM_Id && a.EMSE_Id == id.EMSE_Id && a.ISMS_Id == id.ISMS_Id);

                                    resultsubsubjectmarks.MI_Id = id.MI_Id;
                                    resultsubsubjectmarks.ESTM_Id = result_obj.ESTM_Id;
                                    resultsubsubjectmarks.EMSS_Id = 0;
                                    resultsubsubjectmarks.ISMS_Id = id.ISMS_Id;
                                    resultsubsubjectmarks.EMSE_Id = id.EMSE_Id;
                                    resultsubsubjectmarks.Login_Id = id.Id;
                                    resultsubsubjectmarks.ESTMSS_UpdatedBy = id.Id;
                                    resultsubsubjectmarks.LoginDateTime = DateTime.Now;
                                    resultsubsubjectmarks.IP4 = id.IP4;
                                    resultsubsubjectmarks.ESTMSS_ActiveFlg = true;
                                    resultsubsubjectmarks.CreatedDate = DateTime.Now;
                                    resultsubsubjectmarks.UpdatedDate = DateTime.Now;
                                    resultsubsubjectmarks.ESTMSS_MarksGradeFlg = id.subMorGFlag;

                                    if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                                     || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m"
                                     || id.detailsList[i].obtainmarks == "OD" || id.detailsList[i].obtainmarks == "od")
                                    {
                                        resultsubsubjectmarks.ESTMSS_Grade = "null";
                                        resultsubsubjectmarks.ESTMSS_Marks = 0;
                                        resultsubsubjectmarks.ESTMSS_Flg = id.detailsList[i].obtainmarks;
                                    }
                                    else
                                    {
                                        resultsubsubjectmarks.ESTMSS_Flg = "";

                                        if (id.subMorGFlag == "M")
                                        {
                                            resultsubsubjectmarks.ESTMSS_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
                                            resultsubsubjectmarks.ESTMSS_Grade = "null";
                                        }
                                        else if (id.subMorGFlag == "G")
                                        {
                                            resultsubsubjectmarks.ESTMSS_Marks = Convert.ToDecimal("0");
                                            resultsubsubjectmarks.ESTMSS_Grade = id.detailsList[i].obtainmarks;
                                        }
                                    }
                                    _examcontext.Update(resultsubsubjectmarks);
                                }
                                else
                                {
                                    Exm_Student_Marks_SubSubjectDMO obj_S = new Exm_Student_Marks_SubSubjectDMO();

                                    obj_S.MI_Id = id.MI_Id;
                                    obj_S.ESTM_Id = result_obj.ESTM_Id;
                                    obj_S.EMSS_Id = 0;
                                    obj_S.ISMS_Id = id.ISMS_Id;
                                    obj_S.EMSE_Id = id.EMSE_Id;
                                    obj_S.Login_Id = id.Id;
                                    obj_S.ESTMSS_CreatedBy = id.Id;
                                    obj_S.ESTMSS_UpdatedBy = id.Id;
                                    obj_S.LoginDateTime = DateTime.Now;
                                    obj_S.IP4 = id.IP4;
                                    obj_S.ESTMSS_ActiveFlg = true;
                                    obj_S.CreatedDate = DateTime.Now;
                                    obj_S.UpdatedDate = DateTime.Now;
                                    obj_S.ESTMSS_MarksGradeFlg = id.subMorGFlag;

                                    if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                                      || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m"
                                      || id.detailsList[i].obtainmarks == "OD" || id.detailsList[i].obtainmarks == "od")
                                    {
                                        obj_S.ESTMSS_Grade = "null";
                                        obj_S.ESTMSS_Marks = 0;
                                        obj_S.ESTMSS_Flg = id.detailsList[i].obtainmarks;
                                    }
                                    else
                                    {
                                        obj_S.ESTMSS_Flg = "";

                                        if (id.subMorGFlag == "M")
                                        {
                                            obj_S.ESTMSS_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
                                            obj_S.ESTMSS_Grade = "null";
                                        }
                                        else if (id.subMorGFlag == "G")
                                        {
                                            obj_S.ESTMSS_Marks = Convert.ToDecimal("0");
                                            obj_S.ESTMSS_Grade = id.detailsList[i].obtainmarks;
                                        }
                                    }
                                    _examcontext.Add(obj_S);
                                }
                            }

                            else
                            {
                                var duplicate = _examcontext.ExamMarksDMO.Where(R => R.MI_Id == id.MI_Id
                               && R.ASMAY_Id == id.ASMAY_Id && R.ASMCL_Id == id.ASMCL_Id && R.ASMS_Id == id.ASMS_Id && R.EME_Id == id.EME_Id
                               && R.ISMS_Id == id.ISMS_Id && R.AMST_Id == stu_id && R.ESTM_ActiveFlg == true).ToList();
                                if (duplicate.Count > 0)
                                {

                                }
                                else
                                {


                                    ExamMarksDMO obj_M = new ExamMarksDMO();
                                    obj_M.MI_Id = id.MI_Id;
                                    obj_M.ASMAY_Id = id.ASMAY_Id;
                                    obj_M.ASMCL_Id = id.ASMCL_Id;
                                    obj_M.ASMS_Id = id.ASMS_Id;
                                    obj_M.EME_Id = id.EME_Id;
                                    obj_M.ISMS_Id = id.ISMS_Id;
                                    obj_M.AMST_Id = stu_id;
                                    obj_M.ESTM_MarksGradeFlg = id.subMorGFlag;
                                    obj_M.Id = id.Id;
                                    obj_M.ESTM_UpdatedBy = id.Id;
                                    obj_M.ESTM_CreatedBy = id.Id;
                                    obj_M.LoginDateTime = DateTime.Now;
                                    obj_M.IP4 = id.IP4;
                                    obj_M.CreatedDate = DateTime.Now;
                                    obj_M.UpdatedDate = DateTime.Now;
                                    obj_M.ESTM_ActiveFlg = true;
                                    obj_M.ESTM_OnlineExamFlag = false;

                                    if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                                      || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m"
                                      || id.detailsList[i].obtainmarks == "OD" || id.detailsList[i].obtainmarks == "od")
                                    {
                                        obj_M.ESTM_Grade = "null";
                                        obj_M.ESTM_Marks = 0;
                                        obj_M.ESTM_Flg = id.detailsList[i].obtainmarks;
                                    }
                                    else
                                    {
                                        obj_M.ESTM_Flg = "";

                                        if (id.subMorGFlag == "M")
                                        {
                                            obj_M.ESTM_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
                                            obj_M.ESTM_Grade = "null";
                                        }
                                        else if (id.subMorGFlag == "G")
                                        {
                                            obj_M.ESTM_Marks = Convert.ToDecimal("0");
                                            obj_M.ESTM_Grade = id.detailsList[i].obtainmarks;
                                        }
                                    }
                                    _examcontext.Add(obj_M);

                                    Exm_Student_Marks_SubSubjectDMO obj_S = new Exm_Student_Marks_SubSubjectDMO();

                                    obj_S.MI_Id = id.MI_Id;
                                    obj_S.ESTM_Id = obj_M.ESTM_Id;
                                    obj_S.EMSS_Id = 0;
                                    obj_S.ISMS_Id = id.ISMS_Id;
                                    obj_S.EMSE_Id = id.EMSE_Id;
                                    obj_S.Login_Id = id.Id;
                                    obj_S.ESTMSS_CreatedBy = id.Id;
                                    obj_S.ESTMSS_UpdatedBy = id.Id;

                                    obj_S.ESTMSS_MarksGradeFlg = id.subMorGFlag;
                                    obj_S.LoginDateTime = DateTime.Now;
                                    obj_S.IP4 = id.IP4;
                                    obj_S.ESTMSS_ActiveFlg = true;
                                    obj_S.CreatedDate = DateTime.Now;
                                    obj_S.UpdatedDate = DateTime.Now;

                                    if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                                      || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m"
                                      || id.detailsList[i].obtainmarks == "OD" || id.detailsList[i].obtainmarks == "od")
                                    {
                                        obj_S.ESTMSS_Grade = "null";
                                        obj_S.ESTMSS_Marks = 0;
                                        obj_S.ESTMSS_Flg = id.detailsList[i].obtainmarks;
                                    }
                                    else
                                    {
                                        obj_S.ESTMSS_Flg = "";

                                        if (id.subMorGFlag == "M")
                                        {
                                            obj_S.ESTMSS_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
                                            obj_S.ESTMSS_Grade = "null";
                                        }
                                        else if (id.subMorGFlag == "G")
                                        {
                                            obj_S.ESTMSS_Marks = Convert.ToDecimal("0");
                                            obj_S.ESTMSS_Grade = id.detailsList[i].obtainmarks;
                                        }
                                    }
                                    _examcontext.Add(obj_S);
                                }
                            }
                        }

                        int flag = _examcontext.SaveChanges();
                        if (flag > 0)
                        {
                            id.messagesaveupdate = "true";
                        }
                        else
                        {
                            id.messagesaveupdate = "false";
                        }
                    }
                }

                // WHEN SUB SUBJECT AND SUB EXAM IS THERE 
                else if (subMorGFlag[0].EYCES_SubSubjectFlg == true && subMorGFlag[0].EYCES_SubExamFlg == true)
                {
                    if (id.detailsList.Length > 0)
                    {
                        for (int i = 0; i < id.detailsList.Length; i++)
                        {
                            var stu_id = id.detailsList[i].amsT_Id;

                            decimal? marks = 0;

                            var already_cnt = _examcontext.ExamMarksDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id && t.ASMCL_Id == id.ASMCL_Id
                            && t.ASMS_Id == id.ASMS_Id && t.EME_Id == id.EME_Id && t.ISMS_Id == id.ISMS_Id && t.AMST_Id == stu_id).Count();

                            if (already_cnt == 0)
                            {
                                var duplicate = _examcontext.ExamMarksDMO.Where(R => R.MI_Id == id.MI_Id
                               && R.ASMAY_Id == id.ASMAY_Id && R.ASMCL_Id == id.ASMCL_Id && R.ASMS_Id == id.ASMS_Id && R.EME_Id == id.EME_Id
                               && R.ISMS_Id == id.ISMS_Id && R.AMST_Id == stu_id && R.ESTM_ActiveFlg == true).ToList();
                                if (duplicate.Count > 0)
                                {

                                }
                                else
                                {


                                    ExamMarksDMO obj_M = new ExamMarksDMO();
                                    obj_M.MI_Id = id.MI_Id;
                                    obj_M.ASMAY_Id = id.ASMAY_Id;
                                    obj_M.ASMCL_Id = id.ASMCL_Id;
                                    obj_M.ASMS_Id = id.ASMS_Id;
                                    obj_M.EME_Id = id.EME_Id;
                                    obj_M.ISMS_Id = id.ISMS_Id;
                                    obj_M.AMST_Id = stu_id;
                                    obj_M.ESTM_MarksGradeFlg = id.subMorGFlag;
                                    obj_M.Id = id.Id;
                                    obj_M.ESTM_UpdatedBy = id.Id;
                                    obj_M.ESTM_CreatedBy = id.Id;
                                    obj_M.LoginDateTime = DateTime.Now;
                                    obj_M.IP4 = id.IP4;
                                    obj_M.CreatedDate = DateTime.Now;
                                    obj_M.UpdatedDate = DateTime.Now;
                                    obj_M.ESTM_ActiveFlg = true;
                                    obj_M.ESTM_OnlineExamFlag = false;

                                    if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                                      || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m"
                                      || id.detailsList[i].obtainmarks == "OD" || id.detailsList[i].obtainmarks == "od")
                                    {
                                        obj_M.ESTM_Grade = "null";
                                        obj_M.ESTM_Marks = 0;
                                        obj_M.ESTM_Flg = id.detailsList[i].obtainmarks;
                                    }
                                    else
                                    {
                                        obj_M.ESTM_Flg = "";

                                        if (id.subMorGFlag == "M")
                                        {
                                            obj_M.ESTM_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
                                            obj_M.ESTM_Grade = "null";
                                        }
                                        else if (id.subMorGFlag == "G")
                                        {
                                            obj_M.ESTM_Marks = Convert.ToDecimal("0");
                                            obj_M.ESTM_Grade = id.detailsList[i].obtainmarks;
                                        }
                                    }

                                    _examcontext.Add(obj_M);

                                    Exm_Student_Marks_SubSubjectDMO obj_S = new Exm_Student_Marks_SubSubjectDMO();

                                    obj_S.MI_Id = id.MI_Id;
                                    obj_S.ESTM_Id = obj_M.ESTM_Id;
                                    obj_S.EMSS_Id = id.EMSS_Id;
                                    obj_S.ISMS_Id = id.ISMS_Id;
                                    obj_S.EMSE_Id = id.EMSE_Id;
                                    obj_S.Login_Id = id.Id;
                                    obj_S.ESTMSS_CreatedBy = id.Id;
                                    obj_S.ESTMSS_UpdatedBy = id.Id;
                                    obj_S.ESTMSS_MarksGradeFlg = id.subMorGFlag;
                                    obj_S.LoginDateTime = DateTime.Now;
                                    obj_S.IP4 = id.IP4;
                                    obj_S.ESTMSS_ActiveFlg = true;
                                    obj_S.CreatedDate = DateTime.Now;
                                    obj_S.UpdatedDate = DateTime.Now;

                                    if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                                      || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m"
                                      || id.detailsList[i].obtainmarks == "OD" || id.detailsList[i].obtainmarks == "od")
                                    {
                                        obj_S.ESTMSS_Grade = "null";
                                        obj_S.ESTMSS_Marks = 0;
                                        obj_S.ESTMSS_Flg = id.detailsList[i].obtainmarks;
                                    }
                                    else
                                    {
                                        obj_S.ESTMSS_Flg = "";

                                        if (id.subMorGFlag == "M")
                                        {
                                            obj_S.ESTMSS_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
                                            obj_S.ESTMSS_Grade = "null";
                                        }
                                        else if (id.subMorGFlag == "G")
                                        {
                                            obj_S.ESTMSS_Marks = Convert.ToDecimal("0");
                                            obj_S.ESTMSS_Grade = id.detailsList[i].obtainmarks;
                                        }
                                    }
                                    _examcontext.Add(obj_S);
                                }
                            }
                            else
                            {
                                var getsubsubjectsubexammarks = (from a in _examcontext.Exm_Student_Marks_SubSubjectDMO
                                                                 from b in _examcontext.ExamMarksDMO
                                                                 where (a.ESTM_Id == b.ESTM_Id && b.MI_Id == id.MI_Id && b.ASMAY_Id == id.ASMAY_Id
                                                                 && b.ASMCL_Id == id.ASMCL_Id && b.ASMS_Id == id.ASMS_Id && b.ISMS_Id == id.ISMS_Id
                                                                 && b.AMST_Id == stu_id && (a.EMSE_Id != id.EMSE_Id || a.EMSS_Id != id.EMSS_Id)
                                                                 && b.EME_Id == id.EME_Id)

                                                                 select new Exm_Student_Marks_SubSubjectDMO
                                                                 {
                                                                     ESTMSS_Marks = a.ESTMSS_Marks,
                                                                     ESTMSS_Flg = a.ESTMSS_Flg

                                                                 }).ToList();


                                foreach (var m in getsubsubjectsubexammarks)
                                {
                                    marks = marks + m.ESTMSS_Marks;
                                }
                                var resultflag = "";

                                var result_obj = _examcontext.ExamMarksDMO.Single(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id && t.ASMCL_Id == id.ASMCL_Id
                                && t.ASMS_Id == id.ASMS_Id && t.EME_Id == id.EME_Id && t.ISMS_Id == id.ISMS_Id && t.AMST_Id == stu_id);

                                resultflag = result_obj.ESTM_Flg;

                                result_obj.ESTM_MarksGradeFlg = id.subMorGFlag;
                                result_obj.Id = id.Id;
                                result_obj.ESTM_UpdatedBy = id.Id;
                                result_obj.LoginDateTime = DateTime.Now;
                                result_obj.IP4 = id.IP4;
                                result_obj.UpdatedDate = DateTime.Now;

                                if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                                 || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m"
                                 || id.detailsList[i].obtainmarks == "OD" || id.detailsList[i].obtainmarks == "od")
                                {
                                    result_obj.ESTM_Flg = id.detailsList[i].obtainmarks;
                                    result_obj.ESTM_Marks = Convert.ToDecimal("0") + Convert.ToDecimal(marks);
                                    result_obj.ESTM_Grade = "null";
                                }
                                else
                                {
                                    result_obj.ESTM_Flg = "";

                                    foreach (var m in getsubsubjectsubexammarks)
                                    {
                                        resultflag = m.ESTMSS_Flg;
                                        if (resultflag == "AB" || resultflag == "ab" || resultflag == "L" || resultflag == "l" || resultflag == "M"
                                            || resultflag == "m" || resultflag == "OD" || resultflag == "od")
                                        {
                                            result_obj.ESTM_Flg = resultflag;
                                            break;
                                        }
                                    }

                                    if (id.subMorGFlag == "M")
                                    {
                                        result_obj.ESTM_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks) + Convert.ToDecimal(marks);
                                        result_obj.ESTM_MarksGradeFlg = "M";
                                        result_obj.ESTM_Grade = "null";
                                    }
                                    else if (id.subMorGFlag == "G")
                                    {
                                        result_obj.ESTM_Marks = Convert.ToDecimal("0") + Convert.ToDecimal(marks);
                                        result_obj.ESTM_MarksGradeFlg = "G";
                                        result_obj.ESTM_Grade = id.detailsList[i].obtainmarks; //id.ESTM_Grade;
                                    }
                                }
                                _examcontext.Update(result_obj);


                                var checksubsubjectmarks = _examcontext.Exm_Student_Marks_SubSubjectDMO.Where(a => a.MI_Id == id.MI_Id
                                && a.ESTM_Id == result_obj.ESTM_Id && a.EMSE_Id == id.EMSE_Id && a.EMSS_Id == id.EMSS_Id && a.ISMS_Id == id.ISMS_Id).ToList();

                                if (checksubsubjectmarks.Count > 0)
                                {
                                    var resultsubsubjectmarks = _examcontext.Exm_Student_Marks_SubSubjectDMO.Single(a => a.MI_Id == id.MI_Id
                                    && a.ESTM_Id == result_obj.ESTM_Id && a.EMSE_Id == id.EMSE_Id && a.EMSS_Id == id.EMSS_Id && a.ISMS_Id == id.ISMS_Id);

                                    resultsubsubjectmarks.MI_Id = id.MI_Id;
                                    resultsubsubjectmarks.ESTM_Id = result_obj.ESTM_Id;
                                    resultsubsubjectmarks.EMSS_Id = id.EMSS_Id;
                                    resultsubsubjectmarks.ISMS_Id = id.ISMS_Id;
                                    resultsubsubjectmarks.EMSE_Id = id.EMSE_Id;
                                    resultsubsubjectmarks.Login_Id = id.Id;
                                    resultsubsubjectmarks.ESTMSS_UpdatedBy = id.Id;
                                    resultsubsubjectmarks.LoginDateTime = DateTime.Now;
                                    resultsubsubjectmarks.IP4 = id.IP4;
                                    resultsubsubjectmarks.ESTMSS_ActiveFlg = true;
                                    resultsubsubjectmarks.CreatedDate = DateTime.Now;
                                    resultsubsubjectmarks.UpdatedDate = DateTime.Now;
                                    resultsubsubjectmarks.ESTMSS_MarksGradeFlg = id.subMorGFlag;

                                    if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                                     || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m"
                                     || id.detailsList[i].obtainmarks == "OD" || id.detailsList[i].obtainmarks == "od")
                                    {
                                        resultsubsubjectmarks.ESTMSS_Grade = "null";
                                        resultsubsubjectmarks.ESTMSS_Marks = 0;
                                        resultsubsubjectmarks.ESTMSS_Flg = id.detailsList[i].obtainmarks;
                                    }
                                    else
                                    {
                                        resultsubsubjectmarks.ESTMSS_Flg = "";

                                        if (id.subMorGFlag == "M")
                                        {
                                            resultsubsubjectmarks.ESTMSS_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
                                            resultsubsubjectmarks.ESTMSS_Grade = "null";
                                        }
                                        else if (id.subMorGFlag == "G")
                                        {
                                            resultsubsubjectmarks.ESTMSS_Marks = Convert.ToDecimal("0");
                                            resultsubsubjectmarks.ESTMSS_Grade = id.detailsList[i].obtainmarks;
                                        }
                                    }
                                    _examcontext.Update(resultsubsubjectmarks);
                                }
                                else
                                {
                                    Exm_Student_Marks_SubSubjectDMO obj_S = new Exm_Student_Marks_SubSubjectDMO();

                                    obj_S.MI_Id = id.MI_Id;
                                    obj_S.ESTM_Id = result_obj.ESTM_Id;
                                    obj_S.EMSS_Id = id.EMSS_Id;
                                    obj_S.ISMS_Id = id.ISMS_Id;
                                    obj_S.EMSE_Id = id.EMSE_Id;
                                    obj_S.Login_Id = id.Id;
                                    obj_S.ESTMSS_CreatedBy = id.Id;
                                    obj_S.ESTMSS_UpdatedBy = id.Id;
                                    obj_S.LoginDateTime = DateTime.Now;
                                    obj_S.IP4 = id.IP4;
                                    obj_S.ESTMSS_ActiveFlg = true;
                                    obj_S.CreatedDate = DateTime.Now;
                                    obj_S.UpdatedDate = DateTime.Now;
                                    obj_S.ESTMSS_MarksGradeFlg = id.subMorGFlag;

                                    if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                                      || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m"
                                      || id.detailsList[i].obtainmarks == "OD" || id.detailsList[i].obtainmarks == "od")
                                    {
                                        obj_S.ESTMSS_Grade = "null";
                                        obj_S.ESTMSS_Marks = 0;
                                        obj_S.ESTMSS_Flg = id.detailsList[i].obtainmarks;
                                    }
                                    else
                                    {
                                        obj_S.ESTMSS_Flg = "";

                                        if (id.subMorGFlag == "M")
                                        {
                                            obj_S.ESTMSS_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
                                            obj_S.ESTMSS_Grade = "null";
                                        }
                                        else if (id.subMorGFlag == "G")
                                        {
                                            obj_S.ESTMSS_Marks = Convert.ToDecimal("0");
                                            obj_S.ESTMSS_Grade = id.detailsList[i].obtainmarks;
                                        }
                                    }
                                    _examcontext.Add(obj_S);
                                }
                            }
                        }

                        int flag = _examcontext.SaveChanges();
                        if (flag > 0)
                        {
                            id.messagesaveupdate = "true";
                        }
                        else
                        {
                            id.messagesaveupdate = "false";
                        }
                    }
                }

                //save duplicate

                //Delete Dublicate Record

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())                {
                    //cmd.CommandText = "Exam_get_Marks_Entry_Modify";
                    cmd.CommandText = "Exam_Duplicate_MarksEntry_Delete";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = id.ASMAY_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = id.ASMCL_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = id.ASMS_Id });                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = id.MI_Id });                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = id.EME_Id });                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = id.ISMS_Id });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)                                {                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);                                }                                retObject.Add((ExpandoObject)dataRow1);                            }                        }

                    }                    catch (Exception ex)                    {                        _acdimpl.LogError(ex.Message);                        _acdimpl.LogDebug(ex.Message);                    }                }

                if (id.IVRMMAP_AddFlg == true)
                {
                    Calculation(id);
            
                    List<long> EYC_Id = new List<long>();
                    if (eycidlist.Count > 0)
                    {

                        foreach (var d in eycidlist)
                        {
                            EYC_Id.Add(d.EYC_Id);
                        }

                    }
                    var pramotion = _examcontext.Exm_M_PromotionDMO.Where(R => R.MI_Id == id.MI_Id && EYC_Id.Contains(R.EYC_Id)).Distinct().ToList();
                    if (pramotion.Count > 0)
                    {
                        if (id.Pagename == "HHS")
                        {
                            promotionsaveddata(id);
                        }
                        else
                        {
                            id.IVRMMAP_UpdateFlg = Promotion_Calculation(id);
                        }

                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return id;
        }
        public ExamMarksDTO DeleteMarks(ExamMarksDTO data)
        {
            try
            {
                var result = _examcontext.ExamMarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.ISMS_Id == data.ISMS_Id).ToList();

                if (result.Count > 0)
                {
                    var flag = _examcontext.Database.ExecuteSqlCommand("Exam_Student_Marks_Deactivate @p0,@p1,@p2,@p3,@p4,@p5,@p6",
                        data.MI_Id, data.ASMAY_Id, data.ASMCL_Id, data.ASMS_Id, data.EME_Id, data.ISMS_Id, data.Id);

                    if (flag > 0)
                    {
                        data.messagesaveupdate = "true";
                    }
                    else
                    {
                        data.messagesaveupdate = "false";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamMarksDTO SaveMarksworking(ExamMarksDTO id)
        {
            try
            {
               
                var catid = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == id.ASMAY_Id && t.ASMCL_Id == id.ASMCL_Id && t.ASMS_Id == id.ASMS_Id && t.MI_Id == id.MI_Id).Select(t => t.EMCA_Id).ToArray();

                var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id && catid.Contains(t.EMCA_Id)).Select(t => t.EYC_Id).ToArray();

                var eycidlist = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id && catid.Contains(t.EMCA_Id)).ToList();

                var eyceid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id)).Select(t => t.EYCE_Id).ToArray();

                var subid = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id)).ToArray();

                var subMorGFlag = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id)).ToArray();

                id.subMorGFlag = subMorGFlag[0].EYCES_MarksGradeEntryFlg;

                id.EMGR_Id = subMorGFlag[0].EMGR_Id;

                string msg = "";

                if (id.detailsList.Length > 0)
                {
                    for (int i = 0; i < id.detailsList.Length; i++)
                    {
                        var checkduplicates = _examcontext.ExamMarksDMO.Where(d => d.ASMAY_Id == id.ASMAY_Id && d.ASMCL_Id == id.ASMCL_Id && d.ASMS_Id == id.ASMS_Id && d.AMST_Id == id.detailsList[i].amsT_Id && d.EME_Id == id.EME_Id && d.ISMS_Id == id.ISMS_Id && d.ESTM_ActiveFlg == true && d.MI_Id == id.MI_Id).ToList();

                        if (checkduplicates.Count > 0)
                        {
                            var result = _examcontext.ExamMarksDMO.Single(d => d.ASMAY_Id == id.ASMAY_Id && d.ASMCL_Id == id.ASMCL_Id && d.ASMS_Id == id.ASMS_Id
                            && d.AMST_Id == id.detailsList[i].amsT_Id && d.EME_Id == id.EME_Id && d.ISMS_Id == id.ISMS_Id && d.ESTM_ActiveFlg == true
                            && d.MI_Id == id.MI_Id);

                            result.MI_Id = id.MI_Id;
                            result.ASMCL_Id = id.ASMCL_Id;
                            result.ASMAY_Id = id.ASMAY_Id;
                            result.ASMS_Id = id.ASMS_Id;
                            result.EME_Id = id.EME_Id;
                            result.ISMS_Id = id.ISMS_Id;
                            result.AMST_Id = id.detailsList[i].amsT_Id;

                            if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                                || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m"
                                || id.detailsList[i].obtainmarks == "OD" || id.detailsList[i].obtainmarks == "od")
                            {
                                result.ESTM_Flg = id.detailsList[i].obtainmarks;
                                result.ESTM_Marks = Convert.ToDecimal("0");
                                result.ESTM_Grade = "null";
                                if (id.subMorGFlag == "M")
                                {
                                    result.ESTM_MarksGradeFlg = "M";
                                }
                                else if (id.subMorGFlag == "G")
                                {
                                    result.ESTM_MarksGradeFlg = "G";
                                }
                            }
                            else
                            {
                                result.ESTM_Flg = "";

                                if (id.subMorGFlag == "M")
                                {
                                    result.ESTM_MarksGradeFlg = "M";
                                    result.ESTM_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
                                    result.ESTM_Grade = "null";
                                }
                                else if (id.subMorGFlag == "G")
                                {
                                    result.ESTM_MarksGradeFlg = "G";
                                    result.ESTM_Marks = Convert.ToDecimal("0");
                                    result.ESTM_Grade = id.detailsList[i].obtainmarks; //id.ESTM_Grade;
                                }
                            }

                            result.UpdatedDate = DateTime.Now;
                            result.Id = id.Id;
                            result.IP4 = id.IP4;
                            _examcontext.Update(result);
                        }
                        else
                        {
                            var duplicate = _examcontext.ExamMarksDMO.Where(R => R.MI_Id == id.MI_Id
                               && R.ASMAY_Id == id.ASMAY_Id && R.ASMCL_Id == id.ASMCL_Id && R.ASMS_Id == id.ASMS_Id && R.EME_Id == id.EME_Id
                               && R.ISMS_Id == id.ISMS_Id && R.AMST_Id == id.detailsList[i].amsT_Id && R.ESTM_ActiveFlg == true).ToList();
                            if (duplicate.Count > 0)
                            {

                            }
                            else
                            {

                                ExamMarksDMO MM = new ExamMarksDMO();
                                //ExamMarksDMO MM = Mapper.Map<ExamMarksDMO>(id);
                                MM.MI_Id = id.MI_Id;
                                MM.ASMCL_Id = id.ASMCL_Id;
                                MM.ASMAY_Id = id.ASMAY_Id;
                                MM.ASMS_Id = id.ASMS_Id;
                                MM.EME_Id = id.EME_Id;
                                MM.ISMS_Id = id.ISMS_Id;
                                MM.AMST_Id = id.detailsList[i].amsT_Id;

                                if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                                    || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m"
                                    || id.detailsList[i].obtainmarks == "OD" || id.detailsList[i].obtainmarks == "od")
                                {
                                    MM.ESTM_Flg = id.detailsList[i].obtainmarks;
                                    MM.ESTM_Marks = Convert.ToDecimal("0");
                                    MM.ESTM_Grade = "null";
                                    if (id.subMorGFlag == "M")
                                    {
                                        MM.ESTM_MarksGradeFlg = "M";
                                    }
                                    else if (id.subMorGFlag == "G")
                                    {
                                        MM.ESTM_MarksGradeFlg = "G";
                                    }
                                }
                                else
                                {
                                    MM.ESTM_Flg = "";

                                    if (id.subMorGFlag == "M")
                                    {
                                        MM.ESTM_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
                                        MM.ESTM_MarksGradeFlg = "M";
                                        MM.ESTM_Grade = "null";
                                    }
                                    else if (id.subMorGFlag == "G")
                                    {
                                        MM.ESTM_Marks = Convert.ToDecimal("0");
                                        MM.ESTM_MarksGradeFlg = "G";
                                        MM.ESTM_Grade = id.detailsList[i].obtainmarks; //id.ESTM_Grade;
                                    }
                                }

                                MM.CreatedDate = DateTime.Now;
                                MM.UpdatedDate = DateTime.Now;
                                MM.Id = id.Id;
                                MM.IP4 = id.IP4;
                                MM.ESTM_ActiveFlg = true;
                                _examcontext.Add(MM);
                            }
                        }
                    }

                    int flag = _examcontext.SaveChanges();
                    if (flag > 0)
                    {
                        id.messagesaveupdate = "true";
                    }
                    else
                    {
                        id.messagesaveupdate = "false";
                    }
                    //deleted Marks
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        //cmd.CommandText = "Exam_get_Marks_Entry_Modify";
                        cmd.CommandText = "Exam_Duplicate_MarksEntry_Delete";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = id.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = id.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = id.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = id.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = id.EME_Id });
                        cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = id.ISMS_Id });
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
                }

                if (id.IVRMMAP_AddFlg == true)
                {
                    Calculation(id);

                    List<long> EYC_Id = new List<long>();
                    if (eycidlist.Count > 0)
                    {

                        foreach (var d in eycidlist)
                        {
                            EYC_Id.Add(d.EYC_Id);
                        }

                    }
                    var pramotion = _examcontext.Exm_M_PromotionDMO.Where(R => R.MI_Id == id.MI_Id && EYC_Id.Contains(R.EYC_Id)).Distinct().ToList();
                    if (pramotion.Count > 0)
                    {
                        if (id.Pagename == "HHS")
                        {
                            promotionsaveddata(id);
                        }
                        else
                        {
                            id.IVRMMAP_UpdateFlg = Promotion_Calculation(id);
                        }

                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return id;
        }
        //sanjeev
        public ExamMarksDTO Calculation(ExamMarksDTO exm)
        {
            try
            {
                using (var cmd = _examcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 80000000;

                    if (exm.Pagename == "HHS")
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
                        Value = 0
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
                dmo.IVRMUL_Id = exm.Id;
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
        //login
        public ExamMarksDTO promotionsaveddata(ExamMarksDTO exm)
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
        public bool Promotion_Calculation(ExamMarksDTO data)
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
                        
                        var outputval = _examcontext.Database.ExecuteSqlCommand("Exm_PromotionDetails_Delete @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, data.ASMCL_Id, data.ASMS_Id);

                        var EMP_Id = _examcontext.Exm_M_PromotionDMO.Single(t => t.MI_Id == data.MI_Id && t.EYC_Id == EYC_Id && t.EMP_ActiveFlag == true).EMP_Id;

                       

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
    }
}
