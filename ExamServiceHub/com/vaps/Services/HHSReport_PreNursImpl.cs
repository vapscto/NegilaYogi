
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
    public class HHSReport_PreNursImpl : Interfaces.HHSReport_PreNursInterface
    {
        //: Interfaces.HHSAllReportInterface
        private static ConcurrentDictionary<string, HHSReport_PreNursDTO> _login =
         new ConcurrentDictionary<string, HHSReport_PreNursDTO>();

        private readonly ExamContext _HHSReport_5to7Context;
        ILogger<HHSReport_PreNursImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public HHSReport_PreNursImpl(ExamContext cpContext, DomainModelMsSqlServerContext db, ILogger<HHSReport_PreNursImpl> _acdim)
        {
            _HHSReport_5to7Context = cpContext;
            _db = db;
            _acdimpl = _acdim;
        }
        public async Task<HHSReport_PreNursDTO> Getdetails(HHSReport_PreNursDTO data)
        {
            HHSReport_PreNursDTO getdata = new HHSReport_PreNursDTO();
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

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;

        }
        public async Task<HHSReport_PreNursDTO> savedetails(HHSReport_PreNursDTO data)
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
                                    from IVS in _HHSReport_5to7Context.State
                                    from IVC in _HHSReport_5to7Context.Country

                                    where (h.AMST_Id == f.AMST_Id && ids.Contains(f.AMST_ActiveFlag) && sol.Contains(f.AMST_SOL) && ids.Contains(h.AMAY_ActiveFlag)
                                    && h.ASMAY_Id == y.ASMAY_Id && IVS.IVRMMS_Id == f.AMST_PerState
                                    && h.ASMAY_Id == y.ASMAY_Id && IVC.IVRMMC_Id == f.AMST_PerCountry
                                    && h.ASMCL_Id == c.ASMCL_Id && h.ASMS_Id == s.ASMS_Id && h.ASMAY_Id == y.ASMAY_Id && h.AMST_Id == data.AMST_Id
                                    && h.ASMAY_Id == data.ASMAY_Id && h.ASMCL_Id == data.ASMCL_Id && h.ASMS_Id == data.ASMS_Id)
                                    select new HHSReport_PreNursDTO
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
                                        IVRMMS_Name = IVS.IVRMMS_Name,
                                        IVRMMC_CountryName = IVC.IVRMMC_CountryName,
                                        AMST_PerPincode = f.AMST_PerPincode,
                                        AMST_EmailId = f.AMST_emailId,
                                        AMST_Mobile = f.AMST_MobileNo
                                    }).Distinct().ToArray();

                var getcategoryid = _HHSReport_5to7Context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id 
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                data.fillterms = _HHSReport_5to7Context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EMCA_Id == getcategoryid).ToArray();

                data.fillskills = _HHSReport_5to7Context.CCE_Master_Life_SkillsDMO.Where(t => t.MI_Id == data.MI_Id && t.ECS_ActiveFlag == true).ToArray();

                data.fillskillarea = (from a in _HHSReport_5to7Context.CCE_Master_Life_Skill_Areas_MappingDMO
                                      from b in _HHSReport_5to7Context.CCE_Master_Life_Skill_AreasDMO
                                      where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ECSAM_ActiveFlag == true && b.ECSA_ActiveFlag == true && a.ECSA_Id == b.ECSA_Id)
                                      select new HHSReport_PreNursDTO
                                      {
                                          ECSA_Id = b.ECSA_Id,
                                          ECS_Id = a.ECS_Id,
                                          ECSA_SkillArea = b.ECSA_SkillArea
                                      }).ToArray();

                data.filltermmarks = _HHSReport_5to7Context.Exm_CCE_SKILLS_TransactionDMO.Where(t => t.MI_Id == data.MI_Id && t.ECST_ActiveFlag == true && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.AMST_Id == data.AMST_Id).ToArray();


                //--------------------Personality and Social Tralts

                data.personality_status = (from a in _HHSReport_5to7Context.Exm_PersonalityDMO
                                           from b in _HHSReport_5to7Context.Exm_Student_PersonalityDMO
                                           from c in _HHSReport_5to7Context.exammasterDMO
                                           from d in _HHSReport_5to7Context.Exm_ProgressCard_RemarksDMO
                                           from e in _HHSReport_5to7Context.School_Adm_Y_Student


                                           where (a.MI_Id == b.MI_Id && b.EP_Id == a.EP_Id && b.EME_Id == c.EME_Id //&& d.EPCR_Id == b.EPCR_Id
                                                   && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == e.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && b.AMST_Id == e.AMST_Id
                                                   && b.AMST_Id == data.AMST_Id)
                                           select new HHSReport_PreNursDTO
                                           {
                                               EP_Id = a.EP_Id,
                                               EP_PersonlaityName = a.EP_PersonlaityName,
                                               month_Id = b.EME_Id,
                                               IVRM_Month_Name = c.EME_ExamName,
                                               EPCR_RemarksName = b.ESPM_Remarks
                                           }).Distinct().ToArray();

                //  --------------------CO - CURRICULAR ACTIVITIES

                data.co_curricular_activity = (from a in _HHSReport_5to7Context.exammasterCoCulrricularDMO
                                               from b in _HHSReport_5to7Context.Exm_Student_CoCurricularDMO
                                               from c in _HHSReport_5to7Context.exammasterDMO
                                               from d in _HHSReport_5to7Context.Exm_ProgressCard_RemarksDMO
                                               from e in _HHSReport_5to7Context.School_Adm_Y_Student

                                               where (a.MI_Id == b.MI_Id && b.ECC_Id == a.ECC_Id && b.EME_Id == c.EME_Id //&& d.EPCR_Id == b.EPCR_Id 
                                               && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == e.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && b.AMST_Id == e.AMST_Id && b.AMST_Id == data.AMST_Id)
                                               select new HHSReport_PreNursDTO
                                               {
                                                   ECC_Id = a.ECC_Id,
                                                   ECC_CoCurricularName = a.ECC_CoCurricularName,
                                                   EME_Id = b.EME_Id,
                                                   EME_ExamName = c.EME_ExamName,
                                                   EPCR_RemarksName = b.ESCOM_Remarks
                                               }).Distinct().ToArray();

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
        public HHSReport_PreNursDTO Getsection(HHSReport_PreNursDTO data)
        {
            try
            {
                try
                {
                    List<School_M_Section> seclist = new List<School_M_Section>();
                    seclist = _HHSReport_5to7Context.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToList();
                    data.fillsection = seclist.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return data;
            }
            catch (Exception ex)
            {

            }
            return data;

        }
        public HHSReport_PreNursDTO getclass(HHSReport_PreNursDTO data)
        {


            try
            {
                data.fillclass = (from a in _db.School_Adm_Y_StudentDMO
                                  from b in _db.admissioncls
                                  where (b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && a.AMAY_ActiveFlag == 1)
                                  select new HHSReport_PreNursDTO
                                  {
                                      ASMCL_ClassName = b.ASMCL_ClassName,
                                      ASMCL_Id = b.ASMCL_Id
                                  }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }
        public HHSReport_PreNursDTO GetAttendence(HHSReport_PreNursDTO data)
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

                data.fillstudents = (from a in _db.School_Adm_Y_StudentDMO
                                     from b in _db.admissioncls
                                     from c in _db.Adm_M_Student
                                     from d in _db.Section
                                     from e in _db.AcademicYear
                                     where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == e.ASMAY_Id && c.MI_Id == data.MI_Id
                                     && a.ASMAY_Id == data.ASMAY_Id && sol.Contains(c.AMST_SOL) && ids.Contains(c.AMST_ActiveFlag)
                                     && ids.Contains(a.AMAY_ActiveFlag) && a.ASMCL_Id == data.ASMCL_Id
                                     && d.ASMS_Id == a.ASMS_Id && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id)
                                     select new HHSReport_PreNursDTO
                                     {
                                         AMST_FirstName = ((c.AMST_FirstName == null || c.AMST_FirstName == "" ? "" : " " + c.AMST_FirstName) + (c.AMST_MiddleName == null || c.AMST_MiddleName == "" || c.AMST_MiddleName == "0" ? "" : " " + c.AMST_MiddleName) + (c.AMST_LastName == null || c.AMST_LastName == "" || c.AMST_LastName == "0" ? "" : " " + c.AMST_LastName)).Trim(),
                                         AMST_Id = a.AMST_Id
                                     }).Distinct().OrderBy(t => t.AMST_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
