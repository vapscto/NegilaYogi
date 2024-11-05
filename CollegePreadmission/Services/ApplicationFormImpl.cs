using AutoMapper;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DataAccessMsSqlServerProvider.com.vapstech.College.Preadmission;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Fees;
using DomainModel.Model.com.vapstech.College.Preadmission;
using DomainModel.Model.com.vapstech.Fee;

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Admission;
using PreadmissionDTOs.com.vaps.College.Fees;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using paytm.security;
using paytm.util;
using Org.BouncyCastle.Crypto;
//using Razorpay.Api;
using Payment = CommonLibrary.Payment;
using DomainModel.Model.com.vaps.admission;

namespace CollegePreadmission.Services
{
    public class ApplicationFormImpl : Interfaces.ApplicationFormInterface
    {
        ClgAdmissionContext _context;
        CollegepreadmissionContext _precontext;
        private readonly DomainModelMsSqlServerContext _db;
        CollFeeGroupContext _feecontext;
        public ApplicationFormImpl(ClgAdmissionContext context, CollegepreadmissionContext precontext, DomainModelMsSqlServerContext db, CollFeeGroupContext feecontext)
        {
            _context = context;
            _precontext = precontext;
            _db = db;
            _feecontext = feecontext;
        }
        public CollegePreadmissionstudnetDto Getdetails(CollegePreadmissionstudnetDto obj)
      {

            try
            {

                obj.mediumlist = _context.IVRM_MediumOfInstructionDMO.Where(e => e.IVRMOI_ActiveFlag == true)
                   .Distinct().ToArray();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);


                var Acdemic_preadmission = _precontext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == obj.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                DateTime startdate = Convert.ToDateTime(_precontext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == obj.MI_Id).Select(d => d.ASMAY_PreAdm_F_Date).FirstOrDefault());


                obj.ASMAY_Id = Acdemic_preadmission;
                obj.ASMAY_PreAdm_F_Date = startdate;
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                List<MasterAcademic> allyearget = new List<MasterAcademic>();
                string rolename = _precontext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == obj.roleId).IVRMRT_Role;
                if (rolename == "OnlinePreadmissionUser")
                {
                    allyearget = (from a in _precontext.AcademicYear
                                  where (a.MI_Id == obj.MI_Id && a.ASMAY_Pre_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == obj.MI_Id)
                                  select new MasterAcademic
                                  {
                                      ASMAY_Id = a.ASMAY_Id,
                                      ASMAY_Year = a.ASMAY_Year
                                  }
                     ).ToList();

                    allyear = (from a in _precontext.AcademicYear
                               where (a.MI_Id == obj.MI_Id && a.ASMAY_PreAdm_F_Date <= indianTime && a.ASMAY_PreAdm_T_Date >= indianTime && a.ASMAY_Id == allyearget.FirstOrDefault().ASMAY_Id)
                               select new MasterAcademic
                               {
                                   ASMAY_Id = a.ASMAY_Id,
                                   ASMAY_Year = a.ASMAY_Year
                               }
                   ).ToList();


                    if (allyear.Count == 0)
                    {
                        List<PA_College_Application> alllocation = new List<PA_College_Application>();
                        alllocation = _precontext.PA_College_Application.Where(t => t.ID == obj.ID).ToList();
                        if (alllocation.Count > 0)
                        {
                            allyear = (from a in _precontext.AcademicYear
                                       where (a.MI_Id == obj.MI_Id && a.ASMAY_Pre_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == obj.MI_Id)
                                       select new MasterAcademic
                                       {
                                           ASMAY_Id = a.ASMAY_Id,
                                           ASMAY_Year = a.ASMAY_Year
                                       }
                ).ToList();
                        }
                    }


                    obj.countrole = false;


                    if (obj.status_type != null && obj.status_type != "")
                    {
                        var Students = (from adm_stu in _precontext.PA_College_Application
                                        from crse in _precontext.MasterCourseDMO
                                        from branch in _precontext.ClgMasterBranchDMO
                                        from o in _context.ClgMasterCourseCategorycategoryMap
                                        from p in _context.mastercategory
                                        where (adm_stu.AMCO_Id == crse.AMCO_Id && adm_stu.AMB_Id == branch.AMB_Id && adm_stu.MI_Id == obj.MI_Id && adm_stu.PACA_ActiveFlag == true && adm_stu.ID == obj.ID && adm_stu.ASMAY_Id == obj.ASMAY_Id && o.AMCOCM_ActiveFlg == true && p.ACMC_ActiveFlag == true && o.AMCOC_Id == p.AMCOC_Id && o.AMCOC_Id == adm_stu.AMCOC_Id && adm_stu.MI_Id == o.MI_Id && o.MI_Id == p.MI_Id && o.AMCO_Id == adm_stu.AMCO_Id && p.AMCOC_Name.Trim().ToLower() == obj.status_type.Trim().ToLower())
                                        select new CollegePreadmissionstudnetDto
                                        {
                                            PACA_FirstName = adm_stu.PACA_FirstName,
                                            PACA_MiddleName = adm_stu.PACA_MiddleName,
                                            PACA_LastName = adm_stu.PACA_LastName,
                                            PACA_Date = adm_stu.PACA_Date,
                                            PACA_Sex = adm_stu.PACA_Sex,
                                            PACA_RegistrationNo = adm_stu.PACA_RegistrationNo,
                                            PACA_emailId = adm_stu.PACA_emailId,
                                            PACA_MobileNo = adm_stu.PACA_MobileNo,
                                            PACA_Id = adm_stu.PACA_Id,
                                            PACA_DOB = adm_stu.PACA_DOB,
                                            courseName = crse.AMCO_CourseName,
                                            branchName = branch.AMB_BranchName,
                                            ID = adm_stu.ID,
                                            PACA_CompleteFillflag = adm_stu.PACA_CompleteFillflag
                                        }).OrderByDescending(d => d.PACA_Id).ToList();

                        obj.StudentList = Students.ToArray();
                    }
                    else
                    {
                        var Students = (from adm_stu in _precontext.PA_College_Application
                                        from crse in _precontext.MasterCourseDMO
                                        from branch in _precontext.ClgMasterBranchDMO
                                        where (adm_stu.AMCO_Id == crse.AMCO_Id && adm_stu.AMB_Id == branch.AMB_Id && adm_stu.MI_Id == obj.MI_Id && adm_stu.PACA_ActiveFlag == true && adm_stu.ID == obj.ID && adm_stu.ASMAY_Id == obj.ASMAY_Id)
                                        select new CollegePreadmissionstudnetDto
                                        {
                                            PACA_FirstName = adm_stu.PACA_FirstName,
                                            PACA_MiddleName = adm_stu.PACA_MiddleName,
                                            PACA_LastName = adm_stu.PACA_LastName,
                                            PACA_Date = adm_stu.PACA_Date,
                                            PACA_Sex = adm_stu.PACA_Sex,
                                            PACA_RegistrationNo = adm_stu.PACA_RegistrationNo,
                                            PACA_emailId = adm_stu.PACA_emailId,
                                            PACA_MobileNo = adm_stu.PACA_MobileNo,
                                            PACA_Id = adm_stu.PACA_Id,
                                            PACA_DOB = adm_stu.PACA_DOB,
                                            courseName = crse.AMCO_CourseName,
                                            branchName = branch.AMB_BranchName,
                                            ID = adm_stu.ID,
                                            PACA_CompleteFillflag = adm_stu.PACA_CompleteFillflag
                                        }).OrderByDescending(d => d.PACA_Id).ToList();

                        obj.StudentList = Students.ToArray();
                    }



                }
                else
                {

                    allyear = (from a in _precontext.AcademicYear
                               where (a.MI_Id == obj.MI_Id && a.ASMAY_Pre_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == obj.MI_Id)
                               select new MasterAcademic
                               {
                                   ASMAY_Id = a.ASMAY_Id,
                                   ASMAY_Year = a.ASMAY_Year
                               }
                   ).ToList();

                    obj.countrole = true;


                    if (obj.status_type != null && obj.status_type != "")
                    {
                        var Students = (from adm_stu in _precontext.PA_College_Application
                                        from crse in _precontext.MasterCourseDMO
                                        from branch in _precontext.ClgMasterBranchDMO
                                        from o in _context.ClgMasterCourseCategorycategoryMap
                                        from p in _context.mastercategory
                                        where (adm_stu.AMCO_Id == crse.AMCO_Id && adm_stu.AMB_Id == branch.AMB_Id && adm_stu.MI_Id == obj.MI_Id && adm_stu.PACA_ActiveFlag == true && crse.AMCO_RegFeeFlg == true && adm_stu.ASMAY_Id == obj.ASMAY_Id && o.AMCOCM_ActiveFlg == true && p.ACMC_ActiveFlag == true && o.AMCOC_Id == p.AMCOC_Id && o.AMCOC_Id == adm_stu.AMCOC_Id && adm_stu.MI_Id == o.MI_Id && o.MI_Id == p.MI_Id && o.AMCO_Id == adm_stu.AMCO_Id && p.AMCOC_Name.Trim().ToLower() == obj.status_type.Trim().ToLower())
                                        select new CollegePreadmissionstudnetDto
                                        {
                                            PACA_FirstName = adm_stu.PACA_FirstName,
                                            PACA_MiddleName = adm_stu.PACA_MiddleName,
                                            PACA_LastName = adm_stu.PACA_LastName,
                                            PACA_Date = adm_stu.PACA_Date,
                                            PACA_Sex = adm_stu.PACA_Sex,
                                            PACA_RegistrationNo = adm_stu.PACA_RegistrationNo,
                                            PACA_emailId = adm_stu.PACA_emailId,
                                            PACA_MobileNo = adm_stu.PACA_MobileNo,
                                            PACA_Id = adm_stu.PACA_Id,
                                            courseName = crse.AMCO_CourseName,
                                            branchName = branch.AMB_BranchName,
                                            PACA_DOB = adm_stu.PACA_DOB,
                                            ID = adm_stu.ID,
                                            PACA_CompleteFillflag = adm_stu.PACA_CompleteFillflag
                                        }).OrderByDescending(d => d.PACA_Id).ToList();

                        obj.StudentList = Students.ToArray();
                    }
                    else
                    {
                        var Students = (from adm_stu in _precontext.PA_College_Application
                                        from crse in _precontext.MasterCourseDMO
                                        from branch in _precontext.ClgMasterBranchDMO
                                        where (adm_stu.AMCO_Id == crse.AMCO_Id && adm_stu.AMB_Id == branch.AMB_Id && adm_stu.MI_Id == obj.MI_Id && adm_stu.PACA_ActiveFlag == true && crse.AMCO_RegFeeFlg == true && adm_stu.ASMAY_Id == obj.ASMAY_Id)
                                        select new CollegePreadmissionstudnetDto
                                        {
                                            PACA_FirstName = adm_stu.PACA_FirstName,
                                            PACA_MiddleName = adm_stu.PACA_MiddleName,
                                            PACA_LastName = adm_stu.PACA_LastName,
                                            PACA_Date = adm_stu.PACA_Date,
                                            PACA_Sex = adm_stu.PACA_Sex,
                                            PACA_RegistrationNo = adm_stu.PACA_RegistrationNo,
                                            PACA_emailId = adm_stu.PACA_emailId,
                                            PACA_MobileNo = adm_stu.PACA_MobileNo,
                                            PACA_Id = adm_stu.PACA_Id,
                                            PACA_DOB = adm_stu.PACA_DOB,
                                            courseName = crse.AMCO_CourseName,
                                            branchName = branch.AMB_BranchName,
                                            ID = adm_stu.ID,
                                            PACA_CompleteFillflag = adm_stu.PACA_CompleteFillflag
                                        }).OrderByDescending(d => d.PACA_Id).ToList();

                        obj.StudentList = Students.ToArray();
                    }


                }
                obj.academicdrp = allyear.ToArray();


                var allyearonload = _context.AcademicYear.Where(d => d.MI_Id == obj.MI_Id && d.Is_Active == true).
                    Select(d => new MasterAcademic { ASMAY_Id = d.ASMAY_Id, ASMAY_Year = d.ASMAY_Year, ASMAY_Order = d.ASMAY_Order }).OrderByDescending(a => a.ASMAY_Order).ToList();

                if (allyearonload.Count > 0)
                {
                   // obj.academicYearOnLoad = allyearonload.ToArray();
                    obj.academicYearOnLoad = (from a in _context.AcademicYear
                                              where (a.MI_Id == obj.MI_Id && a.ASMAY_Id != obj.ASMAY_Id && a.Is_Active == true)
                                              select new MasterAcademic
                                              {
                                                  ASMAY_Id = a.ASMAY_Id,
                                                  ASMAY_Year = a.ASMAY_Year
                                              }).ToArray();
                    obj.AllAcademicYear = _context.AcademicYear.Where(d => d.MI_Id == obj.MI_Id && d.Is_Active == true && d.ASMAY_Pre_ActiveFlag == 1 && d.ASMAY_Id == obj.ASMAY_Id).Select(d => new MasterAcademic { ASMAY_Id = d.ASMAY_Id, ASMAY_Year = d.ASMAY_Year }).ToArray();
                }

                var branches = _context.ClgMasterBranchDMO.Where(d => d.MI_Id == obj.MI_Id && d.AMB_ActiveFlag == true).Select(d => new AdmMasterCollegeStudentDTO { AMB_Id = d.AMB_Id, branchName = d.AMB_BranchName }).ToList();
                if (branches.Count > 0)
                {
                    obj.branches = branches.ToArray();
                }
                var semester = _context.CLG_Adm_Master_SemesterDMO.Where(d => d.MI_Id == obj.MI_Id && d.AMSE_ActiveFlg == true).Select(d => new AdmMasterCollegeStudentDTO { AMSE_Id = d.AMSE_Id, semesterName = d.AMSE_SEMName }).ToList();
                if (semester.Count > 0)
                {
                    obj.semesters = semester.ToArray();
                }
                var Allcountry = _context.Country.Where(c => c.IVRMMC_CountryName != null).ToList();
                if (Allcountry.Count > 0)
                {
                    obj.AllCountry = Allcountry.ToArray();
                }

                //country code
                var Allcountrycode = _context.Country.Where(c => c.IVRMMC_CountryPhCode != null).ToList();
                if (Allcountrycode.Count > 0)
                {
                    obj.AllCountrycode = Allcountrycode.ToArray();
                }

                var AllReligion = _context.Religion.Where(r => r.Is_Active == true && r.IVRMMR_Name != null).ToList();
                if (AllReligion.Count > 0)
                {
                    obj.AllReligion = AllReligion.ToArray();
                }
                var AllCaste = _context.Caste.Where(c => c.MI_Id == obj.MI_Id && c.IMC_CasteName != null).ToList();
                if (AllCaste.Count > 0)
                {
                    obj.AllCaste = AllCaste.ToArray();
                }
                var AllcasteCategory = _context.CasteCategory.Where(c => c.IMCC_CategoryName != null).ToList();
                if (AllcasteCategory.Count > 0)
                {
                    obj.AllcasteCategory = AllcasteCategory.ToArray();
                }
                var AllRefrence = _context.MasterReference.Where(m => m.PAMR_ReferenceName != null).ToList();
                if (AllRefrence.Count > 0)
                {
                    obj.AllRefrence = AllRefrence.ToArray();
                }
                var AllSources = _context.MasterSource.Where(m => m.PAMS_SourceName != null).ToList();
                if (AllSources.Count > 0)
                {
                    obj.AllSources = AllSources.ToArray();
                }
                var batch = _context.AdmCollegeMasterBatchDMO.Where(d => d.MI_Id == obj.MI_Id && d.ACMSN_ActiveFlag == true).ToList();
                if (batch.Count > 0)
                {
                    obj.batches = batch.ToArray();
                }
                var quota = _context.Clg_Adm_College_QuotaDMO.Where(d => d.MI_Id == obj.MI_Id && d.ACQ_ActiveFlg == true).ToList();
                if (quota.Count > 0)
                {
                    obj.quotas = quota.ToArray();
                }

                var subScheme = _context.AdmCollegeSubjectSchemeDMO.Where(d => d.MI_Id == obj.MI_Id && d.ACST_ActiveFlg == true).ToList();
                if (subScheme.Count > 0)
                {
                    obj.subjectScheme = subScheme.ToArray();
                }

                var schemeType = _context.AdmCollegeSchemeTypeDMO.Where(d => d.MI_Id == obj.MI_Id && d.ACST_ActiveFlg == true).ToList();
                if (schemeType.Count > 0)
                {
                    obj.schemeType = schemeType.ToArray();
                }

                var quotaCategory = (from m in _context.Clg_Adm_College_Quota_CategoryDMO
                                     from n in _context.Clg_Adm_College_Quota_Category_MappingDMO
                                     where m.ACQC_Id == n.ACQC_Id && m.MI_Id == obj.MI_Id && m.ACQC_ActiveFlg == true && n.ACQCM_ActiveFlg == true
                                     group new { m, n } by n.ACQC_Id into g
                                     select new Clg_Adm_College_Quota_CategoryDMO
                                     {
                                         ACQC_Id = g.FirstOrDefault().n.ACQC_Id,
                                         ACQC_CategoryName = g.FirstOrDefault().m.ACQC_CategoryName
                                     }).ToList();
                if (quotaCategory.Count > 0)
                {
                    obj.quotaCategory = quotaCategory.ToArray();
                }
                var course = (from m in _context.CLG_Adm_College_AY_CourseDMO
                              from n in _context.MasterCourseDMO
                              where m.AMCO_Id == n.AMCO_Id && m.MI_Id == obj.MI_Id && m.ASMAY_Id == obj.ASMAY_Id && m.ACAYC_ActiveFlag == true && n.AMCO_ActiveFlag == true && n.AMCO_RegFeeFlg == true
                              group new { m, n } by m.AMCO_Id into g
                              select new AdmMasterCollegeStudentDTO
                              {
                                  AMCO_Id = g.FirstOrDefault().m.AMCO_Id,
                                  ACAYC_Id = g.FirstOrDefault().m.ACAYC_Id,
                                  courseName = g.FirstOrDefault().n.AMCO_CourseName,
                                  ASMAY_Id = g.FirstOrDefault().m.ASMAY_Id
                              }).Distinct().ToList();
                if (course.Count > 0)
                {
                    obj.courses = course.ToArray();
                }
                var studentCategory = (from m in _context.ClgMasterCourseCategorycategoryMap
                                       from n in _context.mastercategory
                                       where m.AMCOC_Id == n.AMCOC_Id && m.MI_Id == obj.MI_Id && m.AMCOCM_ActiveFlg == true
                                       select new AdmMasterCollegeStudentDTO
                                       {
                                           AMCOC_Id = m.AMCOC_Id,
                                           AMCOC_Name = n.AMCOC_Name
                                       }).ToArray();
                if (studentCategory.Length > 0)
                {
                    obj.studentCategory = studentCategory;
                }
                obj.admTransNumSetting = _context.Master_Numbering.Where(t => t.MI_Id == obj.MI_Id).ToArray();

                //Master Board.
                obj.boardList = _context.MasterBorad.Where(d => d.MI_Id == obj.MI_Id && d.Is_Active == true).ToArray();
                //School Type
                obj.Schooltypelist = _context.MasterSchoolType.Where(d => d.Is_Active == true).ToArray();

                //Master Documnets.
                var MasterDocumentDMO = _context.MasterDocumentDMO.Where(t => t.MI_Id == obj.MI_Id).ToList();
                obj.DocumentList = MasterDocumentDMO.ToArray();

                var combination = _context.Adm_Prv_Sch_CombinationDMO.Where(a => a.MI_Id == obj.MI_Id && a.ADMCB_Activeflag == true).ToList();
                obj.combinationlist = combination.ToArray();

                obj.prospectusPaymentlist = _feecontext.Fee_Y_Payment_PA_Application.Where(t => t.FYPPA_Type == "R").ToArray();

                //master subject

                obj.subjectlist = _context.IVRM_School_Master_SubjectsDMO.Where(w => w.MI_Id == obj.MI_Id && w.ISMS_ActiveFlag == 1 && w.ISMS_LanguageFlg == 0).Distinct().OrderBy(e => e.ISMS_SubjectName).ToArray();
                obj.subjectlistlag = _context.IVRM_School_Master_SubjectsDMO.Where(w => w.MI_Id == obj.MI_Id && w.ISMS_ActiveFlag == 1 && w.ISMS_LanguageFlg == 1).Distinct().OrderBy(e => e.ISMS_SubjectName).ToArray();
                //Master competitive Exam 

                obj.compExamarray = _precontext.Master_Competitive_ExamsClgDMO.Where(c => c.MI_Id == obj.MI_Id && c.PAMCEXM_ActiveFlg == true).Distinct().OrderBy(d => d.PAMCEXM_CompetitiveExams).ToArray();

                obj.compSubarray = _precontext.Master_CompetitiveExamsSubjectsClgDMO.Where(s => s.PAMCEXMSUB_ActiveFlg == true).Distinct().OrderBy(m => m.PAMCEXMSUB_SubjectName).ToArray();
              //payment gateway
                if (!rolename.Equals("Student", StringComparison.OrdinalIgnoreCase) && !rolename.Equals("Alumni", StringComparison.OrdinalIgnoreCase))
                {
                    obj.fillpaymentgateway = (from a in _db.PAYUDETAILS
                                              from b in _db.Fee_PaymentGateway_Details
                                              where (a.IMPG_ActiveFlg == true && a.IMPG_Id == b.IMPG_Id && b.MI_Id == obj.MI_Id && b.FPGD_PGActiveFlag == "1")
                                              select new CountryDTO
                                              {
                                                  FPGD_Id = a.IMPG_Id,
                                                  FPGD_PGName = a.IMPG_PGFlag,
                                                  FPGD_Image = b.FPGD_Image
                                              }
                                             ).Distinct().ToArray();
                }
                //


                List<AdmissionStandardDMO> admissionconfigurationsettings = new List<AdmissionStandardDMO>();
                admissionconfigurationsettings = _db.AdmissionStandardDMO.Where(t => t.MI_Id == obj.MI_Id).ToList();
                obj.studentcurrenrtbranch = admissionconfigurationsettings.ToArray();
                //obj.admissioncongigurationList = admissionconfigurationsettings.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }


        public CollegePreadmissionstudnetDto Dashboarddetails(CollegePreadmissionstudnetDto Edata)
        {
            try
            {

                var id = _precontext.PA_College_Application.Where(t => t.ID == Edata.ID).ToList();
                Edata.PACA_Id = id.FirstOrDefault().PACA_Id;

                var StudentPrevSchoolDMO = _precontext.PA_College_Student_PrevSchool.Where(t => t.PACA_Id == Edata.PACA_Id).ToList();
                Edata.PrevSchoolDetails = StudentPrevSchoolDMO.ToArray();


                var StudentGuardianDMO = _precontext.PA_College_Student_Guardian.Where(t => t.PACA_Id == Edata.PACA_Id).ToList();
                Edata.StudentGuardianDetails = StudentGuardianDMO.ToArray();

                var Studentsubjectmarks = _context.PA_College_Student_SubjectMarks.Where(t => t.PACA_Id == Edata.PACA_Id).ToList();
                Edata.Studentsubjectmarksarry = Studentsubjectmarks.ToArray();



                var adm_m_student = _precontext.PA_College_Application.Where(t => t.PACA_Id == Edata.PACA_Id).ToList();
                Edata.StudentList = adm_m_student.ToArray();

                Edata.AllCaste = (from m in _context.CasteCategory
                                  from n in _context.Caste
                                  where m.IMCC_Id == n.IMCC_Id && n.IMC_Id == adm_m_student.FirstOrDefault().IMC_Id && n.MI_Id == adm_m_student.FirstOrDefault().MI_Id
                                  select new CollegePreadmissionstudnetDto
                                  {
                                      IMC_Id = n.IMC_Id,
                                      IMC_CasteName = n.IMC_CasteName
                                  }).ToArray();

                Edata.studentcourse = (from a in _precontext.PA_College_Application
                                       from b in _precontext.MasterCourseDMO
                                       where (a.AMCO_Id == b.AMCO_Id && a.PACA_Id == Edata.PACA_Id)
                                       select new CollegePreadmissionstudnetDto
                                       {
                                           AMCO_CourseName = b.AMCO_CourseName,
                                           PACA_Id = a.PACA_Id,
                                           AMCO_Id = b.AMCO_Id
                                       }).ToArray();

                Edata.studentReligion = (from a in _precontext.PA_College_Application
                                         from c in _precontext.religion
                                         where (a.IVRMMR_Id == c.IVRMMR_Id && a.PACA_Id == Edata.PACA_Id)
                                         select new CollegePreadmissionstudnetDto
                                         {
                                             IVRMMR_Name = c.IVRMMR_Name
                                         }).ToArray();


                Edata.studentcastecate = (from a in _precontext.PA_College_Application
                                          from e in _precontext.caste
                                          where (a.IMC_Id == e.IMC_Id && a.PACA_Id == Edata.PACA_Id)
                                          select new CollegePreadmissionstudnetDto
                                          {
                                              IMC_CasteName = e.IMC_CasteName
                                          }).ToArray();


                Edata.studentpreviousstate = (from a in _precontext.PA_College_Student_PrevSchool
                                              from b in _precontext.state
                                              where (Convert.ToInt64(a.PACSTPS_PreSchoolState) == b.IVRMMS_Id && a.PACA_Id == Edata.PACA_Id)

                                              select new CollegePreadmissionstudnetDto
                                              {
                                                  studpreviousstate = b.IVRMMS_Name,
                                                  statecode = b.IVRMMS_Code
                                              }).ToArray();



                Edata.studentperstate = (from a in _precontext.PA_College_Application
                                         from b in _precontext.state
                                         where (a.PACA_PerState == b.IVRMMS_Id && a.PACA_Id == Edata.PACA_Id)
                                         select new CollegePreadmissionstudnetDto
                                         {
                                             studperstate = b.IVRMMS_Name,
                                             statecode = b.IVRMMS_Code
                                         }).ToArray();


                Edata.studentconstate = (from a in _precontext.PA_College_Application
                                         from b in _precontext.state
                                         where (a.PACA_ConState == b.IVRMMS_Id && a.PACA_Id == Edata.PACA_Id)
                                         select new CollegePreadmissionstudnetDto
                                         {
                                             studconstate = b.IVRMMS_Name
                                         }).ToArray();

                Edata.studentconcountry = (from a in _precontext.PA_College_Application
                                           from b in _precontext.country
                                           where (a.PACA_ConCountryId == (b.IVRMMC_Id) && a.PACA_Id == Edata.PACA_Id)
                                           select new CollegePreadmissionstudnetDto
                                           {
                                               studconcountry = b.IVRMMC_CountryName
                                           }).ToArray();

                Edata.studentpercountry = (from a in _precontext.PA_College_Application
                                           from b in _precontext.country
                                           where (a.PACA_Nationality == Convert.ToString(b.IVRMMC_Id) && a.PACA_Id == Edata.PACA_Id)
                                           select new CollegePreadmissionstudnetDto
                                           {
                                               studpercountry = b.IVRMMC_Nationality,
                                               countrycode = b.IVRMMC_CountryCode
                                           }).ToArray();

                Edata.CasteCategoryName = (from a in _precontext.PA_College_Application
                                           from b in _precontext.castecategory
                                           where (a.IMCC_Id == b.IMCC_Id && a.PACA_Id == Edata.PACA_Id)
                                           select new CollegePreadmissionstudnetDto
                                           {
                                               CategoryName = b.IMCC_CategoryName
                                           }).ToArray();

                Edata.studentpreffredbranch = (from a in _precontext.PA_College_Application
                                               from b in _precontext.PA_College_Student_CBPreference
                                               from c in _precontext.ClgMasterBranchDMO
                                               where (a.PACA_Id == b.PACA_Id && b.AMB_Id == c.AMB_Id && a.PACA_Id == Edata.PACA_Id && b.PACA_Id == Edata.PACA_Id)
                                               select new CollegePreadmissionstudnetDto
                                               {
                                                   branchname = c.AMB_BranchName
                                               }).ToArray();

                Edata.studentcurrenrtbranch = (from a in _precontext.PA_College_Application
                                               from c in _precontext.ClgMasterBranchDMO
                                               where (a.AMB_Id == c.AMB_Id && a.PACA_Id == Edata.PACA_Id)
                                               select new CollegePreadmissionstudnetDto
                                               {
                                                   studentbranchname = c.AMB_BranchName,
                                                   PACA_Id = a.PACA_Id,
                                                   AMB_Id = c.AMB_Id
                                               }).ToArray();

                var asmccid = _precontext.PA_College_Application.Where(t => t.PACA_Id == Edata.PACA_Id).ToList();
                Edata.studentCategory = (from m in _precontext.ClgMasterCourseCategorycategoryMap
                                         from n in _precontext.mastercategory
                                         where m.AMCOC_Id == n.AMCOC_Id && m.AMCOC_Id == asmccid.FirstOrDefault().PACA_Id
                                         select new CollegePreadmissionstudnetDto
                                         {
                                             AMCOC_Id = m.AMCOC_Id,
                                             AMCOC_Name = n.AMCOC_Name
                                         }).ToArray();

                List<AdmissionStatus> status = new List<AdmissionStatus>();
                status = _db.status.Where(t => t.MI_Id == Edata.MI_Id).ToList();
                Edata.statuslist = status.ToArray();

                Edata.prospectusPaymentlist = _feecontext.Fee_Y_Payment_PA_Application.Where(t => t.FYPPA_Type == "R" && t.PACA_Id == Edata.PACA_Id).ToArray();

                List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                mstConfig = _db.mstConfig.Where(d => d.MI_Id.Equals(Edata.MI_Id) && d.ASMAY_Id.Equals(id.FirstOrDefault().ASMAY_Id)).ToList();
                Edata.mstConfig = mstConfig.ToArray();
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var allyear = (from a in _db.AcademicYear
                               where (a.MI_Id == Edata.MI_Id && TimeZoneInfo.ConvertTime(a.ASMAY_PreAdm_F_Date.Value, INDIAN_ZONE) <= indianTime && TimeZoneInfo.ConvertTime(a.ASMAY_PreAdm_T_Date.Value, INDIAN_ZONE) >= indianTime && a.ASMAY_Id == id.FirstOrDefault().ASMAY_Id)
                               select new MasterAcademic
                               {
                                   ASMAY_Id = a.ASMAY_Id,
                                   ASMAY_Year = a.ASMAY_Year
                               }
                      ).ToList();

                if (allyear.Count > 0)
                {
                    Edata.precutdate = "True";
                }
                else
                {
                    Edata.precutdate = "false";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Edata;
        }

        public CollegePreadmissionstudnetDto paynow(CollegePreadmissionstudnetDto dt)
        {

            try
            {


                var alreadyExistEmailId = _precontext.PA_College_Application.Where(d => d.PACA_Id == dt.PACA_Id).ToList();
                var stuid = _precontext.PA_College_Application.Single(d => d.PACA_Id == dt.PACA_Id).ASMAY_Id;

                dt.AMCO_Id = alreadyExistEmailId.FirstOrDefault().AMCO_Id;
                dt.ASMAY_Id = alreadyExistEmailId.FirstOrDefault().ASMAY_Id;
                dt.PACA_FirstName = alreadyExistEmailId.FirstOrDefault().PACA_FirstName;
                dt.PACA_FirstName = ((alreadyExistEmailId.FirstOrDefault().PACA_FirstName == null || alreadyExistEmailId.FirstOrDefault().PACA_FirstName == "0" ? "" : alreadyExistEmailId.FirstOrDefault().PACA_FirstName) + " " + (alreadyExistEmailId.FirstOrDefault().PACA_MiddleName == null || alreadyExistEmailId.FirstOrDefault().PACA_MiddleName == "0" ? "" : alreadyExistEmailId.FirstOrDefault().PACA_MiddleName) + " " + (alreadyExistEmailId.FirstOrDefault().PACA_LastName == null || alreadyExistEmailId.FirstOrDefault().PACA_LastName == "0" ? "" : alreadyExistEmailId.FirstOrDefault().PACA_LastName)).Trim();
                dt.PACA_emailId = alreadyExistEmailId.FirstOrDefault().PACA_emailId;
                dt.PACA_MobileNo = alreadyExistEmailId.FirstOrDefault().PACA_MobileNo;
                dt.AMB_Id = alreadyExistEmailId.FirstOrDefault().AMB_Id;
                dt.PACA_RegistrationNo = alreadyExistEmailId.FirstOrDefault().PACA_RegistrationNo;
                dt.PACA_PerStreet = alreadyExistEmailId.FirstOrDefault().PACA_PerStreet;

                if (dt.configurationsettings.ISPAC_ApplFeeFlag == 1)
                {
                    dt.payementcheck = _feecontext.Fee_Y_Payment_PA_Application.Where(t => t.FYPPA_Type == "R" && t.PACA_Id == dt.PACA_Id).Count();

                    if (dt.payementcheck == 0)
                    {
                        dt.paydet = paymentPart(dt);
                    }
                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return dt;
        }

        public Array paymentPart(CollegePreadmissionstudnetDto enq)
        {
            Payment pay = new Payment(_db);
            ProspectusDTO data = new ProspectusDTO();
            List<Prospepaymentamount> paymentdetails = new List<Prospepaymentamount>();
            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            int autoinc = 1, totpayableamount = 0;
            string orderId = "";
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
            //enq.ASMAY_Id = 7;
            try
            {
                paymentdetails = _precontext.Prospepaymentamount.Where(t => t.IVRMOP_MIID == enq.MI_Id).ToList();
                // ProspectusDTO ProspectusDTO = new ProspectusDTO();
                var FeeAmountresult = (from a in _feecontext.Clg_Fee_AmountEntry_DMO
                                       from b in _feecontext.FeeHeadClgDMO
                                       from c in _feecontext.CLG_Fee_College_Master_Amount_Semesterwise
                                       where (a.FMH_Id == b.FMH_Id && a.FCMA_Id == c.FCMA_Id && a.MI_Id == enq.MI_Id && a.ASMAY_Id == enq.ASMAY_Id && a.AMCO_Id == enq.AMCO_Id && a.AMB_Id == enq.AMB_Id)
                                       select new CollegePreadmissionstudnetDto
                                       {
                                           FCMAS_Id = c.FCMAS_Id,
                                           FCMAS_Amount = c.FCMAS_Amount
                                       }
            ).FirstOrDefault();

                var groupwisefmaids = (from a in _feecontext.Clg_Fee_AmountEntry_DMO
                                       from b in _feecontext.FeeHeadClgDMO
                                       from c in _feecontext.CLG_Fee_College_Master_Amount_Semesterwise
                                       where (a.FMH_Id == b.FMH_Id && a.FCMA_Id == c.FCMA_Id && a.MI_Id == enq.MI_Id && a.ASMAY_Id == enq.ASMAY_Id && a.AMCO_Id == enq.AMCO_Id && a.AMB_Id == enq.AMB_Id && b.FMH_Flag == "R")
                                       select new CollegePreadmissionstudnetDto
                                       {
                                           FCMAS_Id = c.FCMAS_Id,
                                           FCMAS_Amount = c.FCMAS_Amount
                                       }
          ).ToArray();

                try
                {
                    // string ids = enq.ftiidss;

                    using (var cmd1 = _precontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd1.CommandText = "Preadmission_Split_Payment_Registration_College";
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                         SqlDbType.BigInt)
                        {
                            Value = enq.MI_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@Asmay_Id",
                        SqlDbType.BigInt)
                        {
                            Value = enq.ASMAY_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@Amst_Id",
                        SqlDbType.VarChar)
                        {
                            Value = enq.PACA_Id
                        });

                        //cmd1.Parameters.Add(new SqlParameter("@fmt_id",
                        // SqlDbType.VarChar)
                        //{
                        //    Value = enq.multiplegroups
                        //});

                        if (cmd1.Connection.State != ConnectionState.Open)
                            cmd1.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd1.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new FeeSlplitOnlinePayment
                                    {
                                        name = "splitId" + autoinc.ToString(),
                                        merchantId = dataReader["FPGD_MerchantId"].ToString(),
                                        value = dataReader["balance"].ToString(),
                                        commission = "0",
                                        description = "Online Payment",
                                    });

                                    autoinc = autoinc + 1;
                                }
                            }
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


                if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    enq.transnumbconfigurationsettingsss.MI_Id = enq.MI_Id;
                    enq.transnumbconfigurationsettingsss.ASMAY_Id = enq.ASMAY_Id;
                    PaymentDetailsDto.trans_id = a.GenerateNumber(enq.transnumbconfigurationsettingsss);
                }

                if (FeeAmountresult != null)
                {
                    PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

                    foreach (FeeSlplitOnlinePayment x in result)
                    {
                        totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                    }

                    var item = new
                    {
                        paymentParts = result
                    };

                    string payinfo = JsonConvert.SerializeObject(item);
                    if (enq.onlinepaygteway == "PAYU")
                    {
                        PaymentDetailsDto.productinfo = payinfo;
                        PaymentDetailsDto.amount = Convert.ToDecimal(totpayableamount);
                        PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().IVRMOP_MERCHANT_KEY;
                        PaymentDetailsDto.firstname = enq.PACA_FirstName;


                        PaymentDetailsDto.email = enq.PACA_emailId;

                        PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().IVRMOP_SALT;
                        PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().IVRMOP_BASE_URL;
                        PaymentDetailsDto.phone = enq.PACA_MobileNo;
                        PaymentDetailsDto.udf1 = Convert.ToString(enq.ASMAY_Id);
                        PaymentDetailsDto.udf2 = Convert.ToString(enq.PACA_Id);
                        PaymentDetailsDto.udf3 = enq.MI_Id.ToString();
                        PaymentDetailsDto.udf4 = enq.AMCOC_Id.ToString();
                        PaymentDetailsDto.udf5 = enq.ASMAY_Id.ToString();
                        PaymentDetailsDto.udf6 = enq.AMCO_Id.ToString();
                        // PaymentDetailsDto.transaction_response_url = "";
                        PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/ApplicationForm/paymentresponse/";
                        PaymentDetailsDto.status = "success";
                        PaymentDetailsDto.service_provider = "payu_paisa";

                        PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);
                    }
                    else if (enq.onlinepaygteway == "PAYTM")
                    {
                        PaymentDetailsDto.trans_id = "CUST" + enq.MI_Id + DateTime.Now.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (DateTime.Now.Millisecond)).Trim();

                        List<FeeStudentTransactionDTO> paymentdet = new List<FeeStudentTransactionDTO>();
                        paymentdet = (from a in _feecontext.Fee_PaymentGateway_Details
                                      where (a.MI_Id == enq.MI_Id && a.FPGD_PGName == enq.onlinepaygteway)
                                      select new FeeStudentTransactionDTO
                                      {
                                          merchantid = a.FPGD_MerchantId,
                                          merchantkey = a.FPGD_AuthorisationKey,
                                          merchanturl = a.FPGD_URL
                                      }
                   ).ToList();

                        List<FeeStudentTransactionDTO> PAYMENTPARAMDETAILS = new List<FeeStudentTransactionDTO>();
                        PAYMENTPARAMDETAILS = (from a in _feecontext.PAYUDETAILS
                                               where (a.IMPG_PGFlag == enq.onlinepaygteway)
                                               select new FeeStudentTransactionDTO
                                               {
                                                   IMPG_IndustryType = a.IMPG_IndustryType,
                                                   IMPG_Website = a.IMPG_Website
                                               }
                        ).ToList();

                        // For Testing Purpose
                        totpayableamount = Convert.ToInt32(totpayableamount);
                        // totpayableamount = 5000;

                        PaymentDetails.PAYTM aa = new PaymentDetails.PAYTM();

                        Dictionary<string, string> parameters = new Dictionary<string, string>();

                        parameters.Add("MID", paymentdet.FirstOrDefault().merchantid);
                        parameters.Add("ORDER_ID", PaymentDetailsDto.trans_id);
                        parameters.Add("CUST_ID", enq.MI_Id.ToString());
                        parameters.Add("TXN_AMOUNT", totpayableamount.ToString());
                        parameters.Add("CHANNEL_ID", "WEB");
                        parameters.Add("INDUSTRY_TYPE_ID", "PrivateEducation");
                        parameters.Add("WEBSITE", "Teresi");
                        parameters.Add("MOBILE_NO", enq.PACA_MobileNo.ToString());
                        parameters.Add("EMAIL", enq.PACA_emailId.Trim());
                        parameters.Add("MERC_UNQ_REF", enq.ASMAY_Id.ToString().Trim() + "_" + enq.AMCO_Id.ToString() + "_" + enq.AMCO_Id.ToString().Trim() + "_" + enq.ASMAY_Id.ToString() + "_" + Convert.ToString(enq.PACA_Id) + "_" + PaymentDetailsDto.trans_id + "_" + enq.MI_Id.ToString() + "_" + enq.PACA_emailId.Trim() + "_" + enq.PACA_MobileNo.ToString() + "_" + totpayableamount.ToString());
                        string url = paymentdet.FirstOrDefault().merchanturl;
                        //parameters.Add("REQUEST_TYPE", "DEFAULT");
                        parameters.Add("CALLBACK_URL", "http://localhost:57606/api/ApplicationForm/paymentresponsepaytm/");
                        //parameters.Add("CALLBACK_URL", "https://secure.paytm.in/oltp-web/invoiceResponse");
                        // parameters.Add("CALLBACK_URL", "https://securegw-stage.paytm.in/theia/processTransaction");

                        string checksum = generateCheckSum(paymentdet.FirstOrDefault().merchantkey, parameters);

                        aa.MID = paymentdet.FirstOrDefault().merchantid;
                        aa.ORDER_ID = PaymentDetailsDto.trans_id;
                        aa.CUST_ID = enq.MI_Id.ToString();
                        aa.TXN_AMOUNT = Convert.ToDecimal(totpayableamount);
                        aa.CHANNEL_ID = "WEB";
                        aa.INDUSTRY_TYPE_ID = "PrivateEducation";
                        aa.WEBSITE = "Teresi";
                        aa.MOBILE_NO = Convert.ToInt64(enq.PACA_MobileNo);
                        aa.EMAIL = enq.PACA_emailId;
                        aa.payu_URL = url;
                        aa.CHECKSUMHASH = checksum;
                        aa.MERC_UNQ_REF = enq.ASMAY_Id.ToString().Trim() + "_" + enq.AMCO_Id.ToString() + "_" + enq.AMCO_Id.ToString().Trim() + "_" + enq.ASMAY_Id.ToString() + "_" + Convert.ToString(enq.PACA_Id) + "_" + PaymentDetailsDto.trans_id + "_" + enq.MI_Id.ToString() + "_" + enq.PACA_emailId.Trim() + "_" + enq.PACA_MobileNo + "_" + totpayableamount.ToString();

                        List<PaymentDetails.PAYTM> paydet = new List<PaymentDetails.PAYTM>();
                        paydet.Add(aa);

                        PaymentDetailsDto.PaymentDetailsList = paydet.ToArray();
                    }

                    else if (enq.onlinepaygteway == "RAZORPAY")
                    {

                        List<MOBILE_INSTITUTION> instidet = new List<MOBILE_INSTITUTION>();
                        instidet = _feecontext.MOBILE_INSTITUTION.Where(t => t.MI_ID == enq.MI_Id).ToList();


                        List<PaymentDetails> paydet = new List<PaymentDetails>();

                        List<Fee_PaymentGateway_DetailsDMO> paymentdetailsrazorpay = new List<Fee_PaymentGateway_DetailsDMO>();

                        paymentdetailsrazorpay = _feecontext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();

                        List<FeeStudentTransactionDTO> PAYMENTPARAMDETAILS = new List<FeeStudentTransactionDTO>();
                        PAYMENTPARAMDETAILS = (from a in _feecontext.PAYUDETAILS
                                               where (a.IMPG_PGFlag == enq.onlinepaygteway)
                                               select new FeeStudentTransactionDTO
                                               {
                                                   IMPG_IndustryType = a.IMPG_IndustryType,
                                                   IMPG_Website = a.IMPG_Website
                                               }
                        ).ToList();

                        // For Testing Purpose
                        totpayableamount = Convert.ToInt32(totpayableamount);



                        Dictionary<string, object> input = new Dictionary<string, object>();
                        //input.Add("amount", 1 * 100);
                        input.Add("amount", totpayableamount * 100); // this amount should be same as transaction amount
                        input.Add("currency", "INR");
                        input.Add("receipt", "");
                        input.Add("payment_capture", 1);

                        string key = paymentdetailsrazorpay.FirstOrDefault().FPGD_SaltKey;
                        string secret = paymentdetailsrazorpay.FirstOrDefault().FPGD_AuthorisationKey;

                        //RazorpayClient client = new RazorpayClient(key, secret);

                        //Razorpay.Api.Order order = client.Order.Create(input);
                        //orderId = order["id"].ToString();

                        PaymentDetails aa = new PaymentDetails();

                        //added for change in receiptno
                        PaymentDetailsDto.trans_id = orderId;

                        aa.trans_id = orderId;
                        aa.IVRMOP_MERCHANT_KEY = paymentdetailsrazorpay.FirstOrDefault().FPGD_SaltKey;
                        aa.FMA_Amount = totpayableamount;
                        aa.splitpayinformation = payinfo;

                        aa.firstname = enq.PACA_FirstName;
                        aa.mobile = enq.PACA_MobileNo.ToString();
                        aa.email = enq.PACA_emailId.ToString();
                        aa.PACA_RegistrationNo = enq.PACA_RegistrationNo.ToString();
                        if (enq.PACA_PerCity != null && enq.PACA_PerCity != "")
                        {
                            aa.stuaddress = enq.PACA_PerCity.ToString();
                        }
                        else
                        {
                            aa.stuaddress = enq.PACA_PerStreet.ToString();
                        }

                        aa.institutioname = instidet[0].INSTITUTION_NAME;
                        aa.institulogo = instidet[0].INSTITUTION_LOGO;
                        aa.PACA_ID = enq.PACA_Id;
                        paydet.Add(aa);
                        PaymentDetailsDto.PaymentDetailsList = paydet.ToArray();
                    }

                    Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

                    onlinemtrans.FMOT_Trans_Id = PaymentDetailsDto.trans_id;
                    //onlinemtrans.FMOT_Amount = data.topayamount;
                    onlinemtrans.FMOT_Amount = Convert.ToDecimal(totpayableamount);
                    onlinemtrans.FMOT_Date = indianTime;
                    onlinemtrans.FMOT_Flag = "P";
                    onlinemtrans.PASR_Id = 0;
                    onlinemtrans.AMST_Id = enq.PACA_Id;
                    onlinemtrans.FMOT_Receipt_no = PaymentDetailsDto.trans_id;

                    onlinemtrans.MI_Id = enq.MI_Id;
                    onlinemtrans.ASMAY_ID = enq.ASMAY_Id;

                    _feecontext.Fee_M_Online_TransactionDMO.Add(onlinemtrans);

                    for (int s = 0; s < groupwisefmaids.Count(); s++)
                    {
                        Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                        onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                        onlinettrans.FMA_Id = groupwisefmaids[s].FCMAS_Id;
                        onlinettrans.FTOT_Amount = groupwisefmaids[s].FCMAS_Amount;
                        onlinettrans.FTOT_Created_date = indianTime;
                        onlinettrans.FTOT_Updated_date = indianTime;
                        onlinettrans.FTOT_Concession = 0;
                        onlinettrans.FTOT_Fine = 0;
                        _feecontext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                    }

                    _feecontext.SaveChanges();

                    PaymentDetailsDto.paymentdetails = "True";

                }
                else
                {
                    PaymentDetailsDto.paymentdetails = "false";
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return PaymentDetailsDto.PaymentDetailsList;

        }

        public static String generateCheckSum(String masterKey, Dictionary<String, String> parameters)
        {
            // Validate Input
            validateGenerateCheckSumInput(masterKey);
            Dictionary<string, string> parameter = new Dictionary<string, string>();
            try
            {
                foreach (string key in parameters.Keys)
                {
                    // below code snippet is mandatory, so that no one can use your checksumgeneration url for other purpose .
                    if (parameters[key].Trim().ToUpper().Contains("REFUND") || parameters[key].Trim().Contains("|"))
                    {
                        parameter.Add(key.Trim(), "");
                    }

                    //if (parameters.ContainsKey(key.Trim()))
                    //{
                    //    parameters[key.Trim()] = parameters[key].Trim();
                    //}
                    else
                    {
                        parameter.Add(key.Trim(), parameters[key].Trim());
                    }
                }

                String checkSumParams = SecurityUtils.createCheckSumString(parameter);
                String salt = StringUtils.generateRandomString(Constants.SALT_LENGTH);

                checkSumParams += salt;

                MessageConsole.WriteLine(); MessageConsole.WriteLine("Final CheckSum String:::: " + checkSumParams);
                String hashedCheckSum = SecurityUtils.getHashedString(checkSumParams);

                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedCheckSum String:::: " + hashedCheckSum);
                hashedCheckSum += salt;

                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedCheckSum String with Salt:::: " + hashedCheckSum);

                String checkSum = Crypto.Encrypt(hashedCheckSum, masterKey);

                return checkSum;
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while generating CheckSum. " + e.Message);
            }
        }

        public static String generateCheckSumForRefund(String masterKey, Dictionary<String, String> parameters)
        {
            // Validate Input
            validateGenerateCheckSumInput(masterKey);

            try
            {
                String checkSumParams = SecurityUtils.createCheckSumString(parameters);
                String salt = StringUtils.generateRandomString(Constants.SALT_LENGTH);
                checkSumParams += salt;

                MessageConsole.WriteLine(); MessageConsole.WriteLine("Final CheckSum String:::: " + checkSumParams);
                String hashedCheckSum = SecurityUtils.getHashedString(checkSumParams);

                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedCheckSum String:::: " + hashedCheckSum);
                hashedCheckSum += salt;

                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedCheckSum String with Salt:::: " + hashedCheckSum);

                String checkSum = Crypto.Encrypt(hashedCheckSum, masterKey);
                return checkSum;
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while generating CheckSum. " + e.Message);
            }

        }

        public static String generateCheckSumByJson(String masterKey, string json)
        {
            validateGenerateCheckSumInput(masterKey);

            try
            {
                String checkSumParams = json;
                String salt = StringUtils.generateRandomString(Constants.SALT_LENGTH);
                checkSumParams += Constants.VALUE_SEPARATOR_TOKEN;
                checkSumParams += salt;

                String hashedCheckSum = SecurityUtils.getHashedString(checkSumParams);

                hashedCheckSum += salt;

                String checkSum = Crypto.Encrypt(hashedCheckSum, masterKey);
                return checkSum;
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while generating CheckSum. " + e.Message);
            }

        }

        public static Boolean verifyCheckSumByjson(String masterKey, string json, String checkSum)
        {
            // Validate Input
            validateVerifyCheckSumInput(masterKey, checkSum);

            try
            {
                String hashedCheckSum = Crypto.Decrypt(checkSum, masterKey);

                if (hashedCheckSum == null || hashedCheckSum.Length < Constants.SALT_LENGTH)
                {
                    return false;
                }

                String salt = hashedCheckSum.Substring(hashedCheckSum.Length - Constants.SALT_LENGTH, Constants.SALT_LENGTH);
                MessageConsole.WriteLine("Salt:::: " + salt);


                MessageConsole.WriteLine(); MessageConsole.WriteLine("Input CheckSum:::: " + checkSum);

                // Now creating hashed checkSum string from given checkSum string
                String checkSumParams = json;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("GeneratedCheckSum String:::: " + checkSumParams);
                checkSumParams += Constants.VALUE_SEPARATOR_TOKEN;
                checkSumParams += salt;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("GeneratedCheckSum String with Salt:::: " + checkSumParams);

                String hashedInputCheckSum = SecurityUtils.getHashedString(checkSumParams);
                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedGeneratedCheckSum String:::: " + hashedInputCheckSum);
                hashedInputCheckSum += salt;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedGeneratedCheckSum String with Salt:::: " + hashedInputCheckSum);

                return hashedInputCheckSum.Equals(hashedCheckSum);
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while verifying CheckSum. " + e.Message);
            }

        }

        public static Boolean verifyCheckSum(String masterKey, Dictionary<String, String> parameters, String checkSum)
        {
            // Validate Input
            validateVerifyCheckSumInput(masterKey, checkSum);

            try
            {
                String hashedCheckSum = Crypto.Decrypt(checkSum, masterKey);

                if (hashedCheckSum == null || hashedCheckSum.Length < Constants.SALT_LENGTH)
                {
                    return false;
                }

                String salt = hashedCheckSum.Substring(hashedCheckSum.Length - Constants.SALT_LENGTH, Constants.SALT_LENGTH);
                MessageConsole.WriteLine("Salt:::: " + salt);


                MessageConsole.WriteLine(); MessageConsole.WriteLine("Input CheckSum:::: " + checkSum);

                // Now creating hashed checkSum string from given checkSum string
                String checkSumParams = SecurityUtils.createCheckSumString(parameters);
                MessageConsole.WriteLine(); MessageConsole.WriteLine("GeneratedCheckSum String:::: " + checkSumParams);
                checkSumParams += salt;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("GeneratedCheckSum String with Salt:::: " + checkSumParams);

                String hashedInputCheckSum = SecurityUtils.getHashedString(checkSumParams);
                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedGeneratedCheckSum String:::: " + hashedInputCheckSum);
                hashedInputCheckSum += salt;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedGeneratedCheckSum String with Salt:::: " + hashedInputCheckSum);

                return hashedInputCheckSum.Equals(hashedCheckSum);
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while verifying CheckSum. " + e.Message);
            }

        }

        private static void validateGenerateCheckSumInput(String masterKey)
        {
            if (masterKey == null)
            {
                //throw new ArgumentNullException("masterKey");
                throw new ArgumentNullException("Parameter cannot be null", "masterKey");
            }

        }

        private static void validateVerifyCheckSumInput(String masterKey, String checkSum)
        {
            if (masterKey == null)
            {
                //throw new ArgumentNullException("masterKey");
                throw new ArgumentNullException("Parameter cannot be null", "masterKey");
            }

            if (checkSum == null)
            {
                throw new ArgumentNullException("Parameter cannot be null", "checkSum");
            }
        }

        public static string Encrypt(String CardDetails, String masterKey)
        {
            return Crypto.Encrypt(CardDetails, masterKey);
        }

        public static String Decrypt(String carddetails, String masterKey)
        {
            return Crypto.Decrypt(carddetails, masterKey);
        }

        public PaymentDetails payuresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            FeeStudentTransactionDTO data = new FeeStudentTransactionDTO();
            //   FeePaymentDetailsDMO feeypayment = Mapper.Map<FeePaymentDetailsDMO>(response);

            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            if (response.status == "success")
            {
                stu.MI_Id = Convert.ToInt64(response.udf3);
                stu.PASR_MobileNo = response.phone;
                stu.pasR_Id = Convert.ToInt64(response.udf2);
                stu.PASR_emailId = response.email;
                stu.ASMAY_Id = Convert.ToInt64(response.udf5);

                data.MI_Id = Convert.ToInt64(response.udf3);
                data.ASMCL_ID = Convert.ToInt64(response.udf4);
                data.ASMAY_Id = Convert.ToInt64(response.udf5);

                //string recno = get_grp_reptno(data);

                var confirmstatus = insertdatainfeetables(response.udf3, response.udf4, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid.ToString(), response.udf5, indianTime, "0");

                if (Convert.ToInt32(confirmstatus) > 0)
                {
                    List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                    mstConfig = _db.mstConfig.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.ASMAY_Id.Equals(stu.ASMAY_Id)).ToList();

                    if (mstConfig.FirstOrDefault().ISPAC_ApplMailFlag == 1)
                    {
                        Email Email = new Email(_db);

                        Email.sendmail(stu.MI_Id, stu.PASR_emailId, "CLG_STUDENT_REGISTRATION", stu.pasR_Id);
                    }

                    if (mstConfig.FirstOrDefault().ISPAC_ApplSMSFlag == 1)
                    {
                        SMS sms = new SMS(_db);
                        sms.sendSms(stu.MI_Id, stu.PASR_MobileNo, "CLG_STUDENT_REGISTRATION", stu.pasR_Id);

                    }
                }



            }
            else
            {
                dto.status = response.status;



            }

            return response;
        }

        public PaymentDetails.PAYTM paytmresponse(PaymentDetails.PAYTM response)
        {
            PaymentDetails dto = new PaymentDetails();

            StudentApplicationDTO stu = new StudentApplicationDTO();
            FeeStudentTransactionDTO data = new FeeStudentTransactionDTO();
            //   FeePaymentDetailsDMO feeypayment = Mapper.Map<FeePaymentDetailsDMO>(response);
            if (response.status == "TXN_SUCCESS")
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                string[] tokens = response.MERC_UNQ_REF.Split('_');

                dto.udf3 = tokens[6];
                dto.udf2 = tokens[4];
                dto.udf5 = tokens[0];
                dto.amount = Convert.ToDecimal(tokens[9]);
                dto.udf4 = tokens[2];
                dto.txnid = tokens[5];
                dto.email = tokens[7];
                dto.mobile = tokens[8];
                data.PASR_Id = Convert.ToInt64(tokens[4]);
                string paytmtranid = response.txnid.ToString();
                data.MI_Id = Convert.ToInt64(dto.udf3);
                data.ASMCL_ID = Convert.ToInt64(dto.udf4);
                data.ASMAY_Id = Convert.ToInt64(dto.udf5);

                var confirmstatus = insertdatainfeetables(dto.udf3, "0", dto.udf2, "0", dto.amount, dto.txnid, paytmtranid.ToString(), dto.udf5, indianTime, "0");
                //public string insertdatainfeetables(string miid, string groupidss, string studentid, string headid, decimal amount, string transid, string refid, string yearid, DateTime indianTime, string transactionid)

                if (Convert.ToInt32(confirmstatus) > 0)
                {
                    List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                    mstConfig = _db.mstConfig.Where(d => d.MI_Id.Equals(dto.udf3) && d.ASMAY_Id.Equals(dto.udf5)).ToList();

                    //if (mstConfig.FirstOrDefault().ISPAC_ApplMailFlag == 1)
                    //{
                    Email Email = new Email(_db);

                    Email.sendmail(Convert.ToInt64(dto.udf3), dto.email, "CLG_STUDENT_REGISTRATION", Convert.ToInt64(dto.udf2));
                    //}

                    //if (mstConfig.FirstOrDefault().ISPAC_ApplSMSFlag == 1)
                    //{
                    SMS sms = new SMS(_db);
                    sms.sendSms(Convert.ToInt64(dto.udf3), Convert.ToInt64(dto.mobile), "CLG_STUDENT_REGISTRATION", Convert.ToInt64(dto.udf2));
                    //}
                }

            }
            else
            {
                dto.status = response.status;
            }

            return response;
        }

        public string get_grp_reptno(Fee_StudentTransactionClgDTO data)
        {
            try
            {

                List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
                feemasnum = _db.FeeMasterConfigurationDMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.feeconfiglist = feemasnum.ToArray();

                List<long> temparr = new List<long>();
                for (int i = 0; i < feemasnum.Count; i++)
                {
                    data.auto_receipt_flag = feemasnum[i].FMC_AutoReceiptFeeGroupFlag;
                }

                if (data.auto_receipt_flag == 1)
                {

                    var FeeAmountresult = (from a in _precontext.feeYCC
                                           from c in _precontext.feeYCCC
                                           from d in _precontext.FeeAmountEntryDMO


                                           from g in _precontext.FeeHeadDMO
                                           where (d.FMH_Id == g.FMH_Id && d.FMCC_Id == a.FMCC_Id && a.ASMAY_Id == d.ASMAY_Id && a.FYCC_Id == c.FYCC_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && g.FMH_Flag == "R" && c.ASMCL_Id == data.ASMCL_ID)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FMH_Id = d.FMH_Id,
                                           }
           ).ToList();

                    List<long> HeadId = new List<long>();
                    foreach (var item in FeeAmountresult)
                    {
                        HeadId.Add(item.FMH_Id);
                    }

                    List<FeeStudentTransactionDTO> grps = new List<FeeStudentTransactionDTO>();
                    grps = (from b in _precontext.FeeYearlygroupHeadMappingDMO

                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id))

                            select new FeeStudentTransactionDTO
                            {
                                FMG_Id = b.FMG_Id
                            }
                           ).Distinct().ToList();

                    List<long> grpid = new List<long>();
                    string groupidss = "0";
                    foreach (var item in grps)
                    {
                        grpid.Add(item.FMG_Id);
                    }

                    for (int r = 0; r < grpid.Count(); r++)
                    {
                        groupidss = groupidss + ',' + grpid[r];
                    }

                    var final_rept_no = "";
                    List<FeeStudentTransactionDTO> list_all = new List<FeeStudentTransactionDTO>();
                    List<FeeStudentTransactionDTO> list_repts = new List<FeeStudentTransactionDTO>();

                    list_all = (from b in _feecontext.Fee_Groupwise_AutoReceiptDMO
                                from c in _feecontext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                                select new FeeStudentTransactionDTO
                                {
                                    FGAR_PrefixName = b.FGAR_PrefixName,
                                    FGAR_SuffixName = b.FGAR_SuffixName,
                                    //FGAR_Name = b.FGAR_Name,
                                    //FMG_Id = c.FMG_Id
                                }
                         ).Distinct().ToList();

                    data.grp_count = list_all.Count();

                    if (data.grp_count == 1)
                    {


                        using (var cmd = _feecontext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "receiptnogeneration";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@mi_id",
                                SqlDbType.VarChar, 100)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@asmayid",
                               SqlDbType.NVarChar, 100)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@fmgid",
                           SqlDbType.NVarChar, 100)
                            {
                                Value = groupidss
                            });

                            cmd.Parameters.Add(new SqlParameter("@receiptno",
                SqlDbType.NVarChar, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            data.FYP_Receipt_No = cmd.Parameters["@receiptno"].Value.ToString();

                        }

                        //data.auto_FYP_Receipt_No = final_rept_no;

                        //data.FYP_Receipt_No = final_rept_no;
                    }
                }
                else
                {
                    data.FYP_Receipt_No = "0";
                }

                //else if (data.automanualreceiptno == "Auto")
                //{
                //    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                //    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                //    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                //    data.FYP_Receipt_No = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                //}

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data.FYP_Receipt_No;
        }

        public CollegePreadmissionstudnetDto getCourse(CollegePreadmissionstudnetDto data)
        {
            try
            {


                if (data.status_type != null && data.status_type != "")
                {

                    var course = (from m in _context.CLG_Adm_College_AY_CourseDMO
                                  from n in _context.MasterCourseDMO
                                  from o in _context.ClgMasterCourseCategorycategoryMap
                                  from p in _context.mastercategory
                                  where m.AMCO_Id == n.AMCO_Id && m.MI_Id == data.MI_Id && m.ASMAY_Id == data.ASMAY_Id && m.ACAYC_ActiveFlag == true && n.AMCO_ActiveFlag == true && m.AMCO_Id == o.AMCO_Id && o.AMCOCM_ActiveFlg == true && o.AMCOC_Id == p.AMCOC_Id && p.ACMC_ActiveFlag == true && p.MI_Id == o.MI_Id && p.MI_Id == m.MI_Id && p.AMCOC_Name.Trim().ToLower() == data.status_type.Trim().ToLower()
                                  group new { m, n } by m.AMCO_Id into g
                                  select new CollegePreadmissionstudnetDto
                                  {
                                      AMCO_Id = g.FirstOrDefault().m.AMCO_Id,
                                      ACAYC_Id = g.FirstOrDefault().m.ACAYC_Id,
                                      courseName = g.FirstOrDefault().n.AMCO_CourseName,
                                      ASMAY_Id = g.FirstOrDefault().m.ASMAY_Id
                                  }).Distinct().ToList();
                    if (course.Count > 0)
                    {
                        data.courses = course.ToArray();
                    }
                }
                else
                {

                    var course = (from m in _context.CLG_Adm_College_AY_CourseDMO
                                  from n in _context.MasterCourseDMO
                                  where m.AMCO_Id == n.AMCO_Id && m.MI_Id == data.MI_Id && m.ASMAY_Id == data.ASMAY_Id && m.ACAYC_ActiveFlag == true && n.AMCO_ActiveFlag == true
                                  group new { m, n } by m.AMCO_Id into g
                                  select new CollegePreadmissionstudnetDto
                                  {
                                      AMCO_Id = g.FirstOrDefault().m.AMCO_Id,
                                      ACAYC_Id = g.FirstOrDefault().m.ACAYC_Id,
                                      courseName = g.FirstOrDefault().n.AMCO_CourseName,
                                      ASMAY_Id = g.FirstOrDefault().m.ASMAY_Id
                                  }).Distinct().ToList();
                    if (course.Count > 0)
                    {
                        data.courses = course.ToArray();
                    }
                }





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegePreadmissionstudnetDto getBranch(CollegePreadmissionstudnetDto data)
        {
            try
            {
                var branch = (from m in _context.CLG_Adm_College_AY_Course_BranchDMO
                              from a in _context.CLG_Adm_College_AY_CourseDMO
                              from n in _context.ClgMasterBranchDMO
                              from b in _context.MasterCourseDMO
                              where a.ACAYC_Id == m.ACAYC_Id && a.AMCO_Id == b.AMCO_Id && m.AMB_Id == n.AMB_Id && m.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id
                              && m.ACAYCB_ActiveFlag == true && n.AMB_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id
                              select new AdmMasterCollegeStudentDTO
                              {
                                  AMB_Id = m.AMB_Id,
                                  branchName = n.AMB_BranchName,
                                  ACAYCB_Id = m.ACAYCB_Id,
                                  ACAYC_Id = m.ACAYC_Id
                              }).Distinct().ToArray();
                if (branch.Length > 0)
                {
                    data.branches = branch;
                }
                var studentCategory = (from m in _context.ClgMasterCourseCategorycategoryMap
                                       from n in _context.mastercategory
                                       where m.AMCOC_Id == n.AMCOC_Id && m.MI_Id == data.MI_Id && m.AMCO_Id == data.AMCO_Id && m.AMCOCM_ActiveFlg == true
                                       select new AdmMasterCollegeStudentDTO
                                       {
                                           AMCOC_Id = m.AMCOC_Id,
                                           AMCOC_Name = n.AMCOC_Name
                                       }).ToArray();
                if (studentCategory.Length > 0)
                {
                    data.studentCategory = studentCategory;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CollegePreadmissionstudnetDto getSemester(CollegePreadmissionstudnetDto dt)
        {
            try
            {
                var MaxCapacity = _context.ClgMasterBranchDMO.Where(d => d.MI_Id == dt.MI_Id && d.AMB_ActiveFlag == true && d.AMB_Id == dt.AMB_Id).Select(d => d.AMB_StudentCapacity).ToList();

                var StudCount = _context.Adm_Master_College_StudentDMO.Where(d => d.MI_Id == dt.MI_Id && d.ASMAY_Id == dt.ASMAY_Id && d.AMCST_SOL != "D" && d.AMB_Id == dt.AMB_Id).Count();

                //if (StudCount >= MaxCapacity.FirstOrDefault())
                //{
                //    dt.message = "MaxCapacity";
                //    return dt;
                //}
                //else
                //{
                var semester = (from m in _context.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                from n in _context.CLG_Adm_Master_SemesterDMO
                                where m.AMSE_Id == n.AMSE_Id && m.MI_Id == dt.MI_Id && m.ACAYCB_Id == dt.ACAYCB_Id && m.ACAYCBS_ActiveFlag == true && n.AMSE_ActiveFlg == true
                                select new AdmMasterCollegeStudentDTO
                                {
                                    AMSE_Id = m.AMSE_Id,
                                    semesterName = n.AMSE_SEMName,
                                    ACAYCBS_Id = m.ACAYCBS_Id,
                                    ACAYCB_Id = m.ACAYCB_Id
                                }
                              ).Distinct().ToArray();
                if (semester.Length > 0)
                {
                    dt.semesters = semester;
                }
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dt;
        }

        public CollegePreadmissionstudnetDto getcaste(CollegePreadmissionstudnetDto dto)
        {
            try
            {
                dto.AllCaste = (from m in _context.CasteCategory
                                from n in _context.Caste
                                where m.IMCC_Id == n.IMCC_Id && n.IMCC_Id == dto.IMCC_Id && n.MI_Id == dto.MI_Id
                                select new CollegePreadmissionstudnetDto
                                {
                                    IMC_Id = n.IMC_Id,
                                    IMC_CasteName = n.IMC_CasteName
                                }).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public AdmMasterCollegeStudentDTO getQuotaCategory(AdmMasterCollegeStudentDTO dts)
        {
            try
            {
                var quotaCategory = (from m in _context.Clg_Adm_College_Quota_CategoryDMO
                                     from n in _context.Clg_Adm_College_Quota_Category_MappingDMO
                                     where m.ACQC_Id == n.ACQC_Id && m.MI_Id == dts.MI_Id && m.ACQC_ActiveFlg == true && n.ACQCM_ActiveFlg == true && n.ACQ_Id == dts.ACQ_Id
                                     group new { m, n } by n.ACQC_Id into g
                                     select new Clg_Adm_College_Quota_CategoryDMO
                                     {
                                         ACQC_Id = g.FirstOrDefault().n.ACQC_Id,
                                         ACQC_CategoryName = g.FirstOrDefault().m.ACQC_CategoryName
                                     }).ToList();
                if (quotaCategory.Count > 0)
                {
                    dts.quotaCategory = quotaCategory.ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dts;
        }


        public CollegePreadmissionstudnetDto saveStudentDetails(CollegePreadmissionstudnetDto data)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            try
            {
                var AdmissionStatus = _precontext.AdmissionStatus.FirstOrDefault(d => d.MI_Id == data.MI_Id && d.PAMST_StatusFlag.Contains("INP"));
                if (AdmissionStatus != null)
                {
                    data.PACA_AdmStatus = AdmissionStatus.PAMST_Id;
                }
                if (data.PACA_Id > 0)
                {
                    var result = _precontext.PA_College_Application.Single(a => a.MI_Id == data.MI_Id && a.PACA_Id == data.PACA_Id);
                    result.AMB_Id = data.AMB_Id;
                    result.AMCO_Id = data.AMCO_Id;
                    result.AMSE_Id = data.AMSE_Id;
                    result.PACA_AadharNo = data.PACA_AadharNo;
                    result.PACA_Age = data.PACA_Age;
                    result.PACA_BirthPlace = data.PACA_BirthPlace;
                    result.PACA_BloodGroup = data.PACA_BloodGroup;
                    result.PACA_BPLCardFlag = data.PACA_BPLCardFlag;
                    result.PACA_DOB = data.PACA_DOB;
                    result.PACA_DOB_inwords = data.PACA_DOB_inwords;
                    result.PACA_FirstName = data.PACA_FirstName.ToUpper();
                    result.PACA_GymReqdFlag = data.PACA_GymReqdFlag;
                    result.PACA_MotherTongue = data.PACA_MotherTongue;
                    result.PACA_HostelReqdFlag = data.PACA_HostelReqdFlag;
                    result.PACA_AlterNativeEmialId = data.PACA_AlterNativeEmialId;
                    result.PACA_Urban_Rural = data.PACA_Urban_Rural;
                    result.PACA_CoutryCode = data.PACA_CoutryCode;

                    result.ACQC_Id = data.ACQC_Id;
                    result.ACQ_Id = data.ACQ_Id;

                    if (data.PACA_LastName != null && data.PACA_LastName != "")
                    {
                        result.PACA_LastName = data.PACA_LastName.ToUpper();
                    }
                    else
                    {
                        result.PACA_LastName = "";
                    }
                    if (data.PACA_MiddleName != null && data.PACA_MiddleName != "")
                    {
                        result.PACA_MiddleName = data.PACA_MiddleName.ToUpper();
                    }
                    else
                    {
                        result.PACA_MiddleName = "";
                    }
                   
                    result.PACA_Nationality = data.PACA_Nationality;
                    result.PACA_Sex = data.PACA_Sex;
                    result.PACA_StuBankAccNo = data.PACA_StuBankAccNo;
                    result.PACA_StuBankIFSCCode = data.PACA_StuBankIFSCCode;
                    result.PACA_StuCasteCertiNo = data.PACA_StuCasteCertiNo;
                    result.PACA_StudentPhoto = data.PACA_StudentPhoto;
                    result.PACA_StudentSubCaste = data.PACA_StudentSubCaste;
                    result.PACA_TransportReqdFlag = data.PACA_TransportReqdFlag;
                    result.PACA_MedOfInstruction = data.PACA_MedOfInstruction;
                    result.ASMAY_Id = data.ASMAY_Id;
                    result.IMCC_Id = data.IMCC_Id;
                    result.IMC_Id = data.IMC_Id;
                    result.IVRMMR_Id = data.IVRMMR_Id;
                    result.ACST_Id = data.ACST_Id;
                    result.ACSS_Id = data.ACSS_Id;
                    result.PACA_Taluk = data.PACA_Taluk;
                    result.PACA_Village = data.PACA_Village;
                    result.PACA_District = data.PACA_District;
                    result.UpdatedDate = indianTime;
                    result.PACA_MobileNo = data.PACA_MobileNo;
                    result.PACA_emailId = data.PACA_emailId;
                    result.PACA_AdmStatus = data.PACA_AdmStatus;
                    result.PACA_ApplStatus = data.PACA_ApplStatus;
                    result.PACA_PhysicallyChallenged = data.PACA_PhysicallyChallenged;
                    result.PACA_MotherCountryCode = data.PACA_MotherCountryCode;
                    result.PACA_FatherCountryCode = data.PACA_FatherCountryCode;
                    var clgtypelist = _context.ClgMasterCourseCategorycategoryMap.Where(e => e.MI_Id == data.MI_Id && e.AMCOCM_ActiveFlg == true && e.AMCO_Id == data.AMCO_Id).Distinct().ToList();

                    if (clgtypelist.Count > 0)
                    {
                        result.AMCOC_Id = clgtypelist.FirstOrDefault().AMCOC_Id;
                    }


                    savesubjects(data);
                    student_mobile_no(data);
                    _precontext.Update(result);
                    int n = _precontext.SaveChanges();
                    if (n > 0)
                    {
                        data.message = "Update";
                    }
                    else
                    {
                        data.message = "";
                    }
                }
                else
                {

                    PA_College_Application Clgstudent = Mapper.Map<PA_College_Application>(data);

                    if (data.PACA_MiddleName == "" || data.PACA_MiddleName == null)
                    {
                        data.PACA_MiddleName = "";
                    }

                    var clgtypelist = _context.ClgMasterCourseCategorycategoryMap.Where(e => e.MI_Id == data.MI_Id && e.AMCOCM_ActiveFlg == true && e.AMCO_Id == data.AMCO_Id).Distinct().ToList();

                    if (clgtypelist.Count > 0)
                    {
                        Clgstudent.AMCOC_Id = clgtypelist.FirstOrDefault().AMCOC_Id;
                    }

                    if (data.PACA_FirstName != null && data.PACA_FirstName != "")
                    {
                        Clgstudent.PACA_FirstName = data.PACA_FirstName.ToUpper();
                    }
                    if (data.PACA_LastName != null && data.PACA_LastName != "")
                    {
                        Clgstudent.PACA_LastName = data.PACA_LastName.ToUpper();
                    }
                    if (data.PACA_MiddleName != null && data.PACA_MiddleName != "")
                    {
                        Clgstudent.PACA_MiddleName = data.PACA_MiddleName.ToUpper();
                    }

                    Clgstudent.ID = data.ID;
                    Clgstudent.PACA_ActiveFlag = true;
                    Clgstudent.PACA_Date = indianTime;
                    Clgstudent.CreatedDate = indianTime;
                    Clgstudent.UpdatedDate = indianTime;
                    Clgstudent.PACA_CompleteFillflag = 1;

                    if (data.transnumconfigsettings != null)
                    {
                        if (data.transnumconfigsettings.IMN_AutoManualFlag == "Auto")
                        {
                            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                            data.transnumconfigsettings.MI_Id = data.MI_Id;
                            data.transnumconfigsettings.ASMAY_Id = data.ASMAY_Id;
                            Clgstudent.PACA_RegistrationNo = a.GenerateNumber(data.transnumconfigsettings);

                        }
                        else if (data.transnumconfigsettings.IMN_AutoManualFlag == "serial")
                        {
                            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                            data.transnumconfigsettings.MI_Id = data.MI_Id;
                            data.transnumconfigsettings.ASMAY_Id = data.ASMAY_Id;
                            Clgstudent.PACA_RegistrationNo = a.GenerateNumber(data.transnumconfigsettings);

                        }

                        else
                        {
                            Clgstudent.PACA_RegistrationNo = data.PACA_RegistrationNo;
                        }

                    }
                    else
                    {
                        data.message = "Registration Number is not Generated. kindly contact Admin";
                    }

                    //if (data.manualAdmFlag.Equals("1"))
                    //{
                    //    var isduplicateAdmNo = _precontext.PA_College_Application.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.PACA_Id != data.PACA_Id && d.PACA_ApplicationNo.Equals(data.ApplicationNo.Trim())).ToList();
                    //    if (isduplicateAdmNo.Count > 0)
                    //    {
                    //        data.message = "Application no. already exist.Please enter different one";
                    //        return data;
                    //    }
                    //    Clgstudent.PACA_ApplicationNo = data.ApplicationNo.Trim();
                    //}
                    //else
                    //{
                    //    Clgstudent.PACA_ApplicationNo = Clgstudent.PACA_RegistrationNo;
                    //}


                    _precontext.Add(Clgstudent);
                    int n = _precontext.SaveChanges();
                    if (n > 0)
                    {
                        data.PACA_Id = Clgstudent.PACA_Id;
                        savesubjects(data);
                        if( data.PA_College_Student_CBPreference[0].AMB_Id != 0)
                        {
                            student_mobile_no(data);
                        }
                     
                        data.message = "Add";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        //saving and updating student mobile
        public CollegePreadmissionstudnetDto student_mobile_no(CollegePreadmissionstudnetDto datastdmobile)
        {
            try
            {
                if (datastdmobile.PA_College_Student_CBPreference.Count() > 0)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();
                    //getting all mobilenumbers
                    foreach (PA_College_Student_CBPreferenceDTO ph in datastdmobile.PA_College_Student_CBPreference)
                    {
                        temparr.Add(ph.AMB_Id);
                    }

                    //removing mobile number 
                    Array Phone_Noresultremove = _precontext.PA_College_Student_CBPreference.Where(t => !temparr.Contains(t.AMB_Id) && t.PACA_Id == datastdmobile.PACA_Id).ToArray();
                    foreach (PA_College_Student_CBPreference ph1 in Phone_Noresultremove)
                    {
                        _precontext.Remove(ph1);
                    }

                    //updating and saving 

                    foreach (PA_College_Student_CBPreferenceDTO ph in datastdmobile.PA_College_Student_CBPreference)
                    {
                        ph.PACA_Id = datastdmobile.PACA_Id;
                        PA_College_Student_CBPreference phone = Mapper.Map<PA_College_Student_CBPreference>(ph);
                        if (phone.PACSTCBO_Id > 0)
                        {
                            var Phone_Noresult = _precontext.PA_College_Student_CBPreference.Single(t => t.PACSTCBO_Id == ph.PACSTCBO_Id);
                            Phone_Noresult.UpdatedDate = DateTime.Now;
                            Phone_Noresult.CreatedDate = Phone_Noresult.CreatedDate;
                            Phone_Noresult.AMB_Id = Convert.ToInt64(ph.AMB_Id);
                            Mapper.Map(ph, Phone_Noresult);
                            Phone_Noresult.AMCO_Id = Convert.ToInt64(datastdmobile.AMCO_Id);
                            _precontext.Update(Phone_Noresult);
                        }
                        else
                        {
                            phone.CreatedDate = DateTime.Now;
                            phone.UpdatedDate = DateTime.Now;
                            phone.AMB_Id = Convert.ToInt64(ph.AMB_Id);
                            phone.AMCO_Id = Convert.ToInt64(datastdmobile.AMCO_Id);
                            _precontext.Add(phone);
                        }
                        _precontext.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _context.Database.RollbackTransaction();
            }
            return datastdmobile;
        }

        //saving and updating emailids
        public AdmMasterCollegeStudentDTO student_email_id(AdmMasterCollegeStudentDTO datastdemail)
        {
            try
            {
                if (datastdemail.Adm_College_Student_EmailIdDTO.Count() > 0)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();
                    //getting all emails
                    foreach (Adm_College_Student_EmailIdDTO ph in datastdemail.Adm_College_Student_EmailIdDTO)
                    {
                        temparr.Add(ph.ACSTE_Id);
                    }

                    //removing email number 
                    Array Phone_Noresultremove = _context.AdmCollegeStudentEmailIdDMO.Where(t => !temparr.Contains(t.ACSTE_Id) && t.AMCST_Id == datastdemail.AMCST_Id).ToArray();
                    foreach (AdmCollegeStudentEmailIdDMO ph1 in Phone_Noresultremove)
                    {
                        _context.Remove(ph1);
                    }

                    //updating and saving 

                    foreach (Adm_College_Student_EmailIdDTO ph in datastdemail.Adm_College_Student_EmailIdDTO)
                    {
                        ph.AMCST_Id = datastdemail.AMCST_Id;

                        AdmCollegeStudentEmailIdDMO phone = Mapper.Map<AdmCollegeStudentEmailIdDMO>(ph);
                        if (phone.ACSTE_Id > 0)
                        {
                            var Phone_Noresult = _context.AdmCollegeStudentEmailIdDMO.Single(t => t.ACSTE_Id == ph.ACSTE_Id);
                            Phone_Noresult.UpdatedDate = DateTime.Now;
                            Phone_Noresult.CreatedDate = Phone_Noresult.CreatedDate;
                            Phone_Noresult.ACSTE_EmailId = ph.AMCST_emailId;
                            Mapper.Map(ph, Phone_Noresult);
                            _context.Update(Phone_Noresult);
                        }
                        else
                        {
                            phone.CreatedDate = DateTime.Now;
                            phone.UpdatedDate = DateTime.Now;
                            phone.ACSTE_EmailId = ph.AMCST_emailId;
                            _context.Add(phone);
                        }
                        //   _AdmissionFormContext.SaveChanges();
                    }
                }

            }
            catch (Exception e)
            {
                _context.Database.RollbackTransaction();
            }

            return datastdemail;
        }

        //Edit Student Data.
        public CollegePreadmissionstudnetDto Edit(CollegePreadmissionstudnetDto Edata)
        {
            try
            {

                Edata.activitydetails = _context.PA_College_Student_PrevExtracurricularDMO.Where(t => t.PACA_Id == Edata.PACA_Id).ToArray();

                Edata.achievementdata = _context.PA_College_Student_AchivementsTypeDMO.Where(t => t.PACA_Id == Edata.PACA_Id).ToArray();


                var StudentPrevSchoolDMO = _precontext.PA_College_Student_PrevSchool.Where(t => t.PACA_Id == Edata.PACA_Id).ToList();
                Edata.PrevSchoolDetails = StudentPrevSchoolDMO.ToArray();


                var StudentGuardianDMO = _precontext.PA_College_Student_Guardian.Where(t => t.PACA_Id == Edata.PACA_Id).ToList();
                Edata.StudentGuardianDetails = StudentGuardianDMO.ToArray();

                //var StudentPrevexamSchoolDMO = _context.PA_College_Student_SubjectMarks.Where(t => t.PACA_Id == Edata.PACA_Id).ToList();
                //Edata.Adm_College_Student_SubjectMarksDTO = StudentPrevexamSchoolDMO.ToArray();


                //Edata.subjectlist = _context.PA_College_Student_SubjectDMO.Where(t => t.PACA_Id == Edata.PACA_Id).ToArray();


                var adm_m_student = _precontext.PA_College_Application.Where(t => t.PACA_Id == Edata.PACA_Id).ToList();
                Edata.StudentList = adm_m_student.ToArray();

                Edata.AllCaste = (from m in _context.CasteCategory
                                  from n in _context.Caste
                                  where m.IMCC_Id == n.IMCC_Id && n.IMC_Id == adm_m_student.FirstOrDefault().IMC_Id && n.MI_Id == adm_m_student.FirstOrDefault().MI_Id
                                  select new CollegePreadmissionstudnetDto
                                  {
                                      IMC_Id = n.IMC_Id,
                                      IMC_CasteName = n.IMC_CasteName
                                  }).ToArray();

                Edata.PA_College_Student_CBPreference = (from a in _precontext.PA_College_Student_CBPreference
                                                         from b in _precontext.PA_College_Application
                                                         where (a.PACA_Id == b.PACA_Id && b.PACA_Id == Edata.PACA_Id)
                                                         select new PA_College_Student_CBPreferenceDTO
                                                         {
                                                             PACSTCBO_Id = a.PACSTCBO_Id,
                                                             AMB_Id = a.AMB_Id

                                                         }).ToArray();

                //var branches = _context.ClgMasterBranchDMO.Where(d => d.MI_Id == Edata.MI_Id && d.AMB_ActiveFlag == true).Select(d => new AdmMasterCollegeStudentDTO { AMB_Id = d.AMB_Id, branchName = d.AMB_BranchName }).ToList();
                //if (branches.Count > 0)
                //{
                //    Edata.branches = branches.ToArray();
                //}

                if (adm_m_student.Count() > 0)
                {


                    var branch = (from m in _context.CLG_Adm_College_AY_Course_BranchDMO
                                  from a in _context.CLG_Adm_College_AY_CourseDMO
                                  from n in _context.ClgMasterBranchDMO
                                  from b in _context.MasterCourseDMO
                                  where a.ACAYC_Id == m.ACAYC_Id && a.AMCO_Id == b.AMCO_Id && m.AMB_Id == n.AMB_Id && m.MI_Id == Edata.MI_Id && a.AMCO_Id == adm_m_student.FirstOrDefault().AMCO_Id && a.ASMAY_Id == adm_m_student.FirstOrDefault().ASMAY_Id
                                  && m.ACAYCB_ActiveFlag == true && n.AMB_ActiveFlag == true
                                  select new AdmMasterCollegeStudentDTO
                                  {
                                      AMB_Id = m.AMB_Id,
                                      branchName = n.AMB_BranchName,
                                      ACAYCB_Id = m.ACAYCB_Id,
                                      ACAYC_Id = m.ACAYC_Id
                                  }).Distinct().ToArray();
                    if (branch.Length > 0)
                    {
                        Edata.branches = branch;
                    }
                    var semester = _context.CLG_Adm_Master_SemesterDMO.Where(d => d.MI_Id == Edata.MI_Id && d.AMSE_ActiveFlg == true).Select(d => new AdmMasterCollegeStudentDTO { AMSE_Id = d.AMSE_Id, semesterName = d.AMSE_SEMName }).ToList();
                    if (semester.Count > 0)
                    {
                        Edata.semesters = semester.ToArray();
                    }
                }




                List<CollegeDocumentDTO> result = new List<CollegeDocumentDTO>();
                using (var cmd = _precontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PAStudentDocuments";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = adm_m_student.FirstOrDefault().MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                       SqlDbType.BigInt)
                    {
                        Value = Edata.PACA_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new CollegeDocumentDTO
                                {
                                    AMSMD_DocumentName = dataReader["AMSMD_DocumentName"].ToString(),
                                    AMSMD_Id = Convert.ToInt64(dataReader["AMSMD_Id"])
                                    //AMSMD_FLAG = Convert.ToBoolean(dataReader["AMSMD_FLAG"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                var StudDocumentList = (from sp in _precontext.PA_College_Student_Documents
                                        from cp in _precontext.MasterDocumentDMO
                                        where (sp.AMSMD_Id == cp.AMSMD_Id && sp.PACA_Id == Edata.PACA_Id)
                                        select new CollegeDocumentDTO
                                        {
                                            ACSTD_Doc_Path = cp.AMSMD_DocumentName,
                                            AMSMD_DocumentName = cp.AMSMD_DocumentName,
                                            AMSMD_FilePath = cp.AMSMD_FilePath,
                                            AMSMD_FileName = cp.AMSMD_FileName,
                                            AMSMD_Id = cp.AMSMD_Id,
                                            PACSTD_Id = sp.PACSTD_Id,
                                            PACA_Id = sp.PACA_Id,
                                            Document_Path = sp.ACSTD_Doc_Path
                                        }).ToList();

                for (int i = 0; i < result.Count; i++)
                {
                    StudDocumentList.Add(result[i]);
                }
                Edata.DocumentList = StudDocumentList.ToArray();

                List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                mstConfig = _db.mstConfig.Where(d => d.MI_Id.Equals(Edata.MI_Id) && d.ASMAY_Id.Equals(adm_m_student.FirstOrDefault().ASMAY_Id)).ToList();

                if (mstConfig.FirstOrDefault().ISPAC_ApplFeeFlag == 1)
                {
                    Edata.prospectusPaymentlist = _feecontext.Fee_Y_Payment_PA_Application.Where(t => t.FYPPA_Type == "R" && t.PACA_Id == Edata.PACA_Id).ToArray();
                    if (Edata.prospectusPaymentlist == null || Edata.prospectusPaymentlist.Length == 0)
                    {
                        Edata.pay = "Pay";
                    }
                }

                string rolename = _feecontext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == Edata.roleId).IVRMRT_Role;

                Edata.roleName = rolename;

                var asmccid = _precontext.PA_College_Application.Where(t => t.PACA_Id == Edata.PACA_Id).ToList();
                Edata.studentCategory = (from m in _precontext.ClgMasterCourseCategorycategoryMap
                                         from n in _precontext.mastercategory
                                         where m.AMCOC_Id == n.AMCOC_Id && m.AMCOC_Id == asmccid.FirstOrDefault().PACA_Id
                                         select new CollegePreadmissionstudnetDto
                                         {
                                             AMCOC_Id = m.AMCOC_Id,
                                             AMCOC_Name = n.AMCOC_Name
                                         }).ToArray();

                Edata.studentCompDetails = (from a in _precontext.PA_College_Student_CEMarksClgDMO
                                            from c in _precontext.PA_College_Application
                                            from d in _precontext.Master_Competitive_ExamsClgDMO
                                            where (a.PAMCEXM_Id == d.PAMCEXM_Id && a.PACA_Id == c.PACA_Id && c.MI_Id == Edata.MI_Id && a.PACSTCEM_ActiveFlg == true && c.PACA_Id == asmccid.FirstOrDefault().PACA_Id)
                                            select new PreadmissionDTOs.com.vaps.College.Preadmission.PA_College_Student_CEMarksClgDTO
                                            {
                                                PAMCEXM_Id = a.PAMCEXM_Id,
                                                PACSTCEM_Id = a.PACSTCEM_Id,
                                                PACSTCEM_RollNo = a.PACSTCEM_RollNo,
                                                PACSTCEM_RegistrationId = a.PACSTCEM_RegistrationId,
                                                PACSTCEM_ALLIndiaRank = a.PACSTCEM_ALLIndiaRank,
                                                PACSTCEM_CategoryRank = a.PACSTCEM_CategoryRank,
                                                PACSTCEM_TotalMaxMarks = a.PACSTCEM_TotalMaxMarks,
                                                PACSTCEM_ObtdMarks = a.PACSTCEM_ObtdMarks,
                                                PACSTCEM_Percentile = a.PACSTCEM_Percentile,
                                                PACSTCEM_Percentage = a.PACSTCEM_Percentage,
                                                PAMCEXM_CompetitiveExams = d.PAMCEXM_CompetitiveExams

                                            }
                                             ).ToArray();

                Edata.studentCompSubDetails = (from a in _precontext.PA_College_Student_CEMarks_SubjectClgDMO
                                               from c in _precontext.PA_College_Application
                                               from d in _precontext.Master_Competitive_ExamsClgDMO
                                               from e in _precontext.Master_CompetitiveExamsSubjectsClgDMO
                                               where (e.PAMCEXMSUB_Id == a.PAMCEXMSUB_Id && a.PAMCEXM_Id == d.PAMCEXM_Id && a.PACA_Id == c.PACA_Id && c.MI_Id == Edata.MI_Id && a.PACSTCEMS_ActiveFlg == true && c.PACA_Id == asmccid.FirstOrDefault().PACA_Id)
                                               select new PreadmissionDTOs.com.vaps.College.Preadmission.PA_College_Student_CEMarks_SubjectClgDTO
                                               {
                                                   PAMCEXM_Id = a.PAMCEXM_Id,
                                                   PACSTCEMS_Id = a.PACSTCEMS_Id,
                                                   PAMCEXMSUB_Id = a.PAMCEXMSUB_Id,
                                                   PAMCEXMSUB_MaxMarks = e.PAMCEXMSUB_MaxMarks,
                                                   PACSTCEMS_MaxMarks = a.PACSTCEMS_MaxMarks,
                                                   PACSTCEMS_SubjectMarks = a.PACSTCEMS_SubjectMarks,
                                                   PAMCEXM_CompetitiveExams = d.PAMCEXM_CompetitiveExams,
                                                   PAMCEXMSUB_SubjectName = e.PAMCEXMSUB_SubjectName

                                               }).ToArray();
                Edata.compExamarray = _precontext.Master_Competitive_ExamsClgDMO.Where(c => c.MI_Id == Edata.MI_Id && c.PAMCEXM_ActiveFlg == true).Distinct().OrderBy(d => d.PAMCEXM_CompetitiveExams).ToArray();

                Edata.compSubarray = _precontext.Master_CompetitiveExamsSubjectsClgDMO.Where(s => s.PAMCEXMSUB_ActiveFlg == true).Distinct().OrderBy(m => m.PAMCEXMSUB_SubjectName).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Edata;
        }


        public CollegePreadmissionstudnetDto getprintdata(CollegePreadmissionstudnetDto Edata)
        {
            try
            {
                //List<MasterStudentBondDMO> MasterStudentBondDMO = new List<MasterStudentBondDMO>();
                //MasterStudentBondDMO = _AdmissionFormContext.MasterStudentBondDMO.Where(t => t.AMST_Id == AdmMasterCollegeStudentDTO.AMST_Id).ToList().ToList();
                //AdmMasterCollegeStudentDTO.BondList = MasterStudentBondDMO.ToArray();



                Edata.subjectlist = (from a in _context.PA_College_Student_SubjectDMO
                                     from b in _context.IVRM_School_Master_SubjectsDMO

                                     where a.PACA_Id == Edata.PACA_Id && a.ISMS_Id == b.ISMS_Id && b.ISMS_LanguageFlg == 0
                                     select b).Distinct().ToArray();
                Edata.subjectlistlag = (from a in _context.PA_College_Student_SubjectDMO
                                        from b in _context.IVRM_School_Master_SubjectsDMO

                                        where a.PACA_Id == Edata.PACA_Id && a.ISMS_Id == b.ISMS_Id && b.ISMS_LanguageFlg == 1
                                        select b).Distinct().ToArray();



                Edata.activitydetails = _context.PA_College_Student_PrevExtracurricularDMO.Where(t => t.PACA_Id == Edata.PACA_Id).ToArray();

                Edata.achievementdata = _context.PA_College_Student_AchivementsTypeDMO.Where(t => t.PACA_Id == Edata.PACA_Id).ToArray();

                var StudentPrevSchoolDMO = _precontext.PA_College_Student_PrevSchool.Where(t => t.PACA_Id == Edata.PACA_Id).ToList();
                Edata.PrevSchoolDetails = StudentPrevSchoolDMO.ToArray();




                var StudentGuardianDMO = _precontext.PA_College_Student_Guardian.Where(t => t.PACA_Id == Edata.PACA_Id).ToList();
                Edata.StudentGuardianDetails = StudentGuardianDMO.ToArray();

                //var Studentsubjectmarks = _context.PA_College_Student_SubjectMarks.Where(t => t.PACA_Id == Edata.PACA_Id).ToList();
                //Edata.Studentsubjectmarksarry = Studentsubjectmarks.ToArray();



                var adm_m_student = _precontext.PA_College_Application.Where(t => t.PACA_Id == Edata.PACA_Id).ToList();
                Edata.StudentList = adm_m_student.ToArray();

                Edata.AllCaste = (from m in _context.CasteCategory
                                  from n in _context.Caste
                                  where m.IMCC_Id == n.IMCC_Id && n.IMC_Id == adm_m_student.FirstOrDefault().IMC_Id && n.MI_Id == adm_m_student.FirstOrDefault().MI_Id
                                  select new CollegePreadmissionstudnetDto
                                  {
                                      IMC_Id = n.IMC_Id,
                                      IMC_CasteName = n.IMC_CasteName
                                  }).ToArray();




                List<CollegeDocumentDTO> result = new List<CollegeDocumentDTO>();
                using (var cmd = _precontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PAStudentDocuments";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = adm_m_student.FirstOrDefault().MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                       SqlDbType.BigInt)
                    {
                        Value = Edata.PACA_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new CollegeDocumentDTO
                                {
                                    ACSTD_Doc_Name = dataReader["AMSMD_DocumentName"].ToString(),
                                    AMSMD_Id = Convert.ToInt64(dataReader["AMSMD_Id"])
                                    //AMSMD_FLAG = Convert.ToBoolean(dataReader["AMSMD_FLAG"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                var StudDocumentList = (from sp in _precontext.PA_College_Student_Documents
                                        from cp in _precontext.MasterDocumentDMO
                                        where (sp.AMSMD_Id == cp.AMSMD_Id && sp.PACA_Id == Edata.PACA_Id)
                                        select new CollegeDocumentDTO
                                        {
                                            ACSTD_Doc_Path = cp.AMSMD_DocumentName,
                                            AMSMD_Id = cp.AMSMD_Id,
                                            PACSTD_Id = sp.PACSTD_Id,
                                            PACA_Id = sp.PACA_Id,
                                            Document_Path = sp.ACSTD_Doc_Path
                                        }).ToList();

                for (int i = 0; i < result.Count; i++)
                {
                    StudDocumentList.Add(result[i]);
                }
                Edata.DocumentList = StudDocumentList.ToArray();

                Edata.studentcourse = (from a in _precontext.PA_College_Application
                                       from b in _precontext.MasterCourseDMO
                                       where (a.AMCO_Id == b.AMCO_Id && a.PACA_Id == Edata.PACA_Id)
                                       select new CollegePreadmissionstudnetDto
                                       {
                                           AMCO_CourseName = b.AMCO_CourseName
                                       }).ToArray();

                Edata.studentReligion = (from a in _precontext.PA_College_Application
                                         from c in _precontext.religion
                                         where (a.IVRMMR_Id == c.IVRMMR_Id && a.PACA_Id == Edata.PACA_Id)
                                         select new CollegePreadmissionstudnetDto
                                         {
                                             IVRMMR_Name = c.IVRMMR_Name
                                         }).ToArray();


                Edata.studentcastecate = (from a in _precontext.PA_College_Application
                                          from e in _precontext.caste
                                          where (a.IMC_Id == e.IMC_Id && a.PACA_Id == Edata.PACA_Id)
                                          select new CollegePreadmissionstudnetDto
                                          {
                                              IMC_CasteName = e.IMC_CasteName
                                          }).ToArray();


                Edata.studentpreviousstate = (from a in _precontext.PA_College_Student_PrevSchool
                                              from b in _precontext.state
                                              where (Convert.ToInt64(a.PACSTPS_PreSchoolState) == b.IVRMMS_Id && a.PACA_Id == Edata.PACA_Id)

                                              select new CollegePreadmissionstudnetDto
                                              {
                                                  studpreviousstate = b.IVRMMS_Name,
                                                  statecode = b.IVRMMS_Code
                                              }).ToArray();

                //dt.fathersubcaste = (from a in _StudentApplicationContext.Enq
                //                     from d in _StudentApplicationContext.subcaste
                //                     where (a.PASR_Fathersubcaste == d.IMSC_ID && a.pasr_id == dt.pasR_Id)
                //                     select new StudentApplicationDTO
                //                     {
                //                         IMC_CasteName = d.IMSC_Caste_Name
                //                     }).ToArray();

                //dt.mothersubcaste = (from a in _StudentApplicationContext.Enq
                //                     from d in _StudentApplicationContext.subcaste
                //                     where (a.PASR_Mothersubcatse == d.IMSC_ID && a.pasr_id == dt.pasR_Id)
                //                     select new StudentApplicationDTO
                //                     {
                //                         IMC_CasteName = d.IMSC_Caste_Name
                //                     }).ToArray();

                //dt.subcaste = (from a in _StudentApplicationContext.Enq
                //               from d in _StudentApplicationContext.subcaste
                //               where (a.PASR_Subcaste == d.IMSC_ID && a.pasr_id == dt.pasR_Id)
                //               select new StudentApplicationDTO
                //               {
                //                   IMC_CasteName = d.IMSC_Caste_Name
                //               }).ToArray();

                //dt.sylabusss = (from a in _StudentApplicationContext.Enq
                //                from d in _StudentApplicationContext.Pre_Adm_Syllabus
                //                where (a.PASL_ID == d.PASL_ID && a.pasr_id == dt.pasR_Id)
                //                select new StudentApplicationDTO
                //                {
                //                    IMC_CasteName = d.PASL_Name
                //                }).ToArray();

                //dt.motherreligion = (from a in _StudentApplicationContext.Enq
                //                     from d in _StudentApplicationContext.religion
                //                     where (a.PASR_MotherReligion == d.IVRMMR_Id && a.pasr_id == dt.pasR_Id)
                //                     select new StudentApplicationDTO
                //                     {
                //                         IMC_CasteName = d.IVRMMR_Name
                //                     }).ToArray();

                //dt.fatherreligion = (from a in _StudentApplicationContext.Enq
                //                     from d in _StudentApplicationContext.religion
                //                     where (a.PASR_FatherReligion == d.IVRMMR_Id && a.pasr_id == dt.pasR_Id)
                //                     select new StudentApplicationDTO
                //                     {
                //                         IMC_CasteName = d.IVRMMR_Name
                //                     }).ToArray();

                //dt.mothercaste = (from a in _StudentApplicationContext.Enq
                //                  from d in _StudentApplicationContext.caste
                //                  where (a.PASR_MotherCaste == d.IMC_Id && a.pasr_id == dt.pasR_Id)
                //                  select new StudentApplicationDTO
                //                  {
                //                      IMC_CasteName = d.IMC_CasteName
                //                  }).ToArray();

                //dt.studentcaste = (from a in _StudentApplicationContext.Enq
                //                   from d in _StudentApplicationContext.caste
                //                   where (a.Caste_Id == d.IMC_Id && a.pasr_id == dt.pasR_Id)
                //                   select new StudentApplicationDTO
                //                   {
                //                       IMC_CasteName = d.IMC_CasteName
                //                   }).ToArray();


                Edata.studentperstate = (from a in _precontext.PA_College_Application
                                         from b in _precontext.state
                                         where (a.PACA_PerState == b.IVRMMS_Id && a.PACA_Id == Edata.PACA_Id)
                                         select new CollegePreadmissionstudnetDto
                                         {
                                             studperstate = b.IVRMMS_Name,
                                             statecode = b.IVRMMS_Code
                                         }).ToArray();


                Edata.studentconstate = (from a in _precontext.PA_College_Application
                                         from b in _precontext.state
                                         where (a.PACA_ConState == b.IVRMMS_Id && a.PACA_Id == Edata.PACA_Id)
                                         select new CollegePreadmissionstudnetDto
                                         {
                                             studconstate = b.IVRMMS_Name
                                         }).ToArray();

                Edata.studentpreviouscountry = (from a in _precontext.PA_College_Student_PrevSchool
                                                from b in _precontext.country
                                                where (Convert.ToInt64(a.PACSTPS_PreSchoolCountry) == b.IVRMMC_Id && a.PACA_Id == Edata.PACA_Id)
                                                select new CollegePreadmissionstudnetDto
                                                {
                                                    studconcountry = b.IVRMMC_CountryName
                                                }).ToArray();

                Edata.studentpasissuecountry = (from a in _precontext.PA_College_Application
                                                from b in _precontext.country
                                                where (a.PACA_PassportIssuedCounrty == (b.IVRMMC_Id) && a.PACA_Id == Edata.PACA_Id)
                                                select new CollegePreadmissionstudnetDto
                                                {
                                                    studconcountry = b.IVRMMC_CountryName
                                                }).ToArray();





                Edata.studentconcountry = (from a in _precontext.PA_College_Application
                                           from b in _precontext.country
                                           where (a.PACA_ConCountryId == (b.IVRMMC_Id) && a.PACA_Id == Edata.PACA_Id)
                                           select new CollegePreadmissionstudnetDto
                                           {
                                               studconcountry = b.IVRMMC_CountryName
                                           }).ToArray();

                Edata.studentpercountry = (from a in _precontext.PA_College_Application
                                           from b in _precontext.country
                                           where (a.PACA_Nationality == Convert.ToString(b.IVRMMC_Id) && a.PACA_Id == Edata.PACA_Id)
                                           select new CollegePreadmissionstudnetDto
                                           {
                                               studpercountry = b.IVRMMC_Nationality,
                                               countrycode = b.IVRMMC_CountryCode
                                           }).ToArray();

                Edata.CasteCategoryName = (from a in _precontext.PA_College_Application
                                           from b in _precontext.castecategory
                                           where (a.IMCC_Id == b.IMCC_Id && a.PACA_Id == Edata.PACA_Id)
                                           select new CollegePreadmissionstudnetDto
                                           {
                                               CategoryName = b.IMCC_CategoryName
                                           }).ToArray();

                Edata.studentpreffredbranch = (from a in _precontext.PA_College_Application
                                               from b in _precontext.PA_College_Student_CBPreference
                                               from c in _precontext.ClgMasterBranchDMO
                                               where (a.PACA_Id == b.PACA_Id && b.AMB_Id == c.AMB_Id && a.PACA_Id == Edata.PACA_Id && b.PACA_Id == Edata.PACA_Id)
                                               select new CollegePreadmissionstudnetDto
                                               {
                                                   branchname = c.AMB_BranchName
                                               }).ToArray();

                Edata.studentcurrenrtbranch = (from a in _precontext.PA_College_Application
                                               from c in _precontext.ClgMasterBranchDMO
                                               where (a.AMB_Id == c.AMB_Id && a.PACA_Id == Edata.PACA_Id)
                                               select new CollegePreadmissionstudnetDto
                                               {
                                                   studentbranchname = c.AMB_BranchName
                                               }).ToArray();

                //Edata.fathernationalitys = (from a in _StudentApplicationContext.Enq
                //                         from b in _StudentApplicationContext.country
                //                         where (a.PASR_FatherNationality == b.IVRMMC_Id && a.pasr_id == dt.pasR_Id)
                //                         select new StudentApplicationDTO
                //                         {
                //                             fathernationality = b.IVRMMC_Nationality
                //                         }).ToArray();

                //dt.mothernationalitys = (from a in _StudentApplicationContext.Enq
                //                         from b in _StudentApplicationContext.country
                //                         where (a.PASR_MotherNationality == b.IVRMMC_Id && a.pasr_id == dt.pasR_Id)
                //                         select new StudentApplicationDTO
                //                         {
                //                             mothernationality = b.IVRMMC_Nationality

                //                         }).ToArray();




                //dt.concessioncategory = (from a in _StudentApplicationContext.Enq
                //                         from b in _StudentApplicationContext.Fee_Master_ConcessionDMO
                //                         where (b.FMCC_Id == a.FMCC_ID && a.pasr_id == dt.pasR_Id)
                //                         select new StudentApplicationDTO
                //                         {
                //                             concessioncats = b.FMCC_ConcessionName

                //                         }).ToArray();

                var asmccid = _precontext.PA_College_Application.Where(t => t.PACA_Id == Edata.PACA_Id).ToList();
                Edata.studentCategory = (from m in _precontext.ClgMasterCourseCategorycategoryMap
                                         from n in _precontext.mastercategory
                                         where m.AMCOC_Id == n.AMCOC_Id && m.AMCOC_Id == asmccid.FirstOrDefault().PACA_Id
                                         select new CollegePreadmissionstudnetDto
                                         {
                                             AMCOC_Id = m.AMCOC_Id,
                                             AMCOC_Name = n.AMCOC_Name
                                         }).ToArray();

                //ADDED ON june-30 Praveen
                Edata.yearname = _precontext.AcademicYear.Where(t => t.ASMAY_Id == asmccid.FirstOrDefault().ASMAY_Id).Select(d => d.ASMAY_Year).FirstOrDefault();
                //END ON june-30 Praveen
                //added by roopa 13-10-2021

                Edata.studentCompDetails = (from a in _precontext.PA_College_Student_CEMarksClgDMO
                                            from c in _precontext.PA_College_Application
                                            from d in _precontext.Master_Competitive_ExamsClgDMO
                                            where (a.PAMCEXM_Id == d.PAMCEXM_Id && a.PACA_Id == c.PACA_Id && c.MI_Id == Edata.MI_Id && a.PACSTCEM_ActiveFlg == true && c.PACA_Id == asmccid.FirstOrDefault().PACA_Id)
                                            select new PreadmissionDTOs.com.vaps.College.Preadmission.PA_College_Student_CEMarksClgDTO
                                            {
                                                PAMCEXM_Id = a.PAMCEXM_Id,
                                                PACSTCEM_Id = a.PACSTCEM_Id,
                                                PACSTCEM_RollNo = a.PACSTCEM_RollNo,
                                                PACSTCEM_RegistrationId = a.PACSTCEM_RegistrationId,
                                                PACSTCEM_ALLIndiaRank = a.PACSTCEM_ALLIndiaRank,
                                                PACSTCEM_CategoryRank = a.PACSTCEM_CategoryRank,
                                                PACSTCEM_TotalMaxMarks = a.PACSTCEM_TotalMaxMarks,
                                                PACSTCEM_ObtdMarks = a.PACSTCEM_ObtdMarks,
                                                PACSTCEM_Percentile = a.PACSTCEM_Percentile,
                                                PACSTCEM_Percentage = a.PACSTCEM_Percentage,
                                                PAMCEXM_CompetitiveExams = d.PAMCEXM_CompetitiveExams

                                            }
                               ).ToArray();

                Edata.studentCompSubDetails = (from a in _precontext.PA_College_Student_CEMarks_SubjectClgDMO
                                               from c in _precontext.PA_College_Application
                                               from d in _precontext.Master_Competitive_ExamsClgDMO
                                               from e in _precontext.Master_CompetitiveExamsSubjectsClgDMO
                                               where (e.PAMCEXMSUB_Id == a.PAMCEXMSUB_Id && a.PAMCEXM_Id == d.PAMCEXM_Id && a.PACA_Id == c.PACA_Id && c.MI_Id == Edata.MI_Id && a.PACSTCEMS_ActiveFlg == true && c.PACA_Id == asmccid.FirstOrDefault().PACA_Id)
                                               select new PreadmissionDTOs.com.vaps.College.Preadmission.PA_College_Student_CEMarks_SubjectClgDTO
                                               {
                                                   PAMCEXM_Id = a.PAMCEXM_Id,
                                                   PACSTCEMS_Id = a.PACSTCEMS_Id,
                                                   PAMCEXMSUB_Id = a.PAMCEXMSUB_Id,
                                                   PAMCEXMSUB_MaxMarks = e.PAMCEXMSUB_MaxMarks,
                                                   PACSTCEMS_MaxMarks = a.PACSTCEMS_MaxMarks,
                                                   PACSTCEMS_SubjectMarks = a.PACSTCEMS_SubjectMarks,
                                                   PAMCEXM_CompetitiveExams = d.PAMCEXM_CompetitiveExams,
                                                   PAMCEXMSUB_SubjectName = e.PAMCEXMSUB_SubjectName

                                               }).ToArray();
                //



                //added by roopa 13-10-2021
                ReadTemplateFromAzure obj = new ReadTemplateFromAzure();

                string accountname = "";
                string accesskey = "";
                var datatstu = _db.IVRM_Storage_path_Details.ToList();
                if (datatstu != null && datatstu.Count() > 0)
                {
                    accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
                }
                string html = "";
                try
                {
                    html = obj.getHtmlContentFromAzure(accountname, accesskey, "admissionform/" + Edata.MI_Id, "Admissionform.html", 0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Edata.htmldata = html;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Edata;
        }

        //
        public AdmMasterCollegeStudentDTO checkDuplicate(AdmMasterCollegeStudentDTO dto)
        {

            try

            {

                if (dto.AMCST_Id > 0)
                {
                    var studRecord = _context.Adm_Master_College_StudentDMO.Where(t => t.AMCST_Id == dto.AMCST_Id).ToList();
                    //if (studRecord.FirstOrDefault().AMST_MobileNo != dto.AMST_MobileNo)
                    //{
                    //    var duplicatemob = _AdmissionFormContext.Adm_M_Student.Where(t => t.AMST_MobileNo == dto.AMST_MobileNo && t.MI_Id == dto.MI_Id && t.AMST_ActiveFlag == 1 && t.AMCST_SOL.Equals("S")).ToList();
                    //    if (duplicatemob.Count > 0)
                    //    {
                    //        dto.duplicateMobNo = duplicatemob.Count;
                    //    }
                    //    else
                    //    {
                    //        dto.duplicateMobNo = 0;
                    //    }
                    //}
                    //if (!studRecord.FirstOrDefault().AMCST_emailId.Equals(dto.AMCST_emailId))
                    //{
                    //    var duplicateemail = _AdmissionFormContext.Adm_M_Student.Where(t => t.AMCST_emailId == dto.AMCST_emailId && t.MI_Id == dto.MI_Id && t.AMST_ActiveFlag == 1 && t.AMCST_SOL.Equals("S")).ToList();
                    //    if (duplicateemail.Count > 0)
                    //    {
                    //        dto.duplicateEmailId = duplicateemail.Count;
                    //    }
                    //    else
                    //    {
                    //        dto.duplicateEmailId = 0;
                    //    }
                    //}
                    if (studRecord.FirstOrDefault().AMCST_AadharNo != dto.AMCST_AadharNo)
                    {
                        var duplicateaadhar = _context.Adm_Master_College_StudentDMO.Where(t => t.AMCST_AadharNo == dto.AMCST_AadharNo && t.MI_Id == dto.MI_Id && t.AMCST_ActiveFlag == true && t.AMCST_SOL.Equals("S")).ToList();
                        if (duplicateaadhar.Count > 0)
                        {
                            dto.duplicateAdharNo = duplicateaadhar.Count;
                        }
                        else
                        {
                            dto.duplicateAdharNo = 0;
                        }
                    }
                    //Check Duplicate Adm.No Or Reg.No.If set to prevent duplicate in Transaction Numbering.
                    if (dto.admRegManualFlag == "true" && dto.preventdupRegNo == "Yes")
                    {
                        if (studRecord.FirstOrDefault().AMCST_RegistrationNo != dto.AMCST_RegistrationNo)
                        {
                            dto.duplicateRegNo = _context.Adm_Master_College_StudentDMO.Where(d => d.MI_Id == dto.MI_Id && d.AMCST_ActiveFlag == true && d.AMCST_SOL.Equals("S") && d.AMCST_RegistrationNo.Equals(dto.AMCST_RegistrationNo)).Count();
                        }
                    }
                    if (dto.admAdmManualFlag == "true" && dto.preventdupAdmNo == "Yes")
                    {
                        if (studRecord.FirstOrDefault().AMCST_AdmNo != dto.AMCST_AdmNo)
                        {
                            dto.duplicateAdmNo = _context.Adm_Master_College_StudentDMO.Where(d => d.MI_Id == dto.MI_Id && d.AMCST_ActiveFlag == true && d.AMCST_SOL.Equals("S") && d.AMCST_AdmNo.Equals(dto.AMCST_AdmNo)).Count();
                        }
                    }
                }
                else
                {
                    // dto.duplicateMobNo = _AdmissionFormContext.Adm_M_Student.Where(d => d.AMST_MobileNo == dto.AMST_MobileNo && d.MI_Id == dto.MI_Id && d.AMST_ActiveFlag == 1 && d.AMCST_SOL.Equals("S")).ToList().Count;

                    //  dto.duplicateEmailId = _AdmissionFormContext.Adm_M_Student.Where(d => d.AMCST_emailId.Equals(dto.AMCST_emailId) && d.MI_Id == dto.MI_Id && d.AMST_ActiveFlag == 1 && d.AMCST_SOL.Equals("S")).ToList().Count;

                    dto.duplicateAdharNo = _context.Adm_Master_College_StudentDMO.Where(d => d.AMCST_AadharNo == dto.AMCST_AadharNo && d.MI_Id == dto.MI_Id && d.AMCST_ActiveFlag == true && d.AMCST_SOL.Equals("S")).ToList().Count;

                    //Check Duplicate Adm.No Or Reg.No.If set to prevent duplicate in Transaction Numbering.
                    if (dto.admRegManualFlag == "true" && dto.preventdupRegNo == "Yes")
                    {
                        dto.duplicateRegNo = _context.Adm_Master_College_StudentDMO.Where(d => d.MI_Id == dto.MI_Id && d.AMCST_ActiveFlag == true && d.AMCST_SOL.Equals("S") && d.AMCST_RegistrationNo.Equals(dto.AMCST_RegistrationNo)).Count();
                    }
                    if (dto.admAdmManualFlag == "true" && dto.preventdupAdmNo == "Yes")
                    {
                        dto.duplicateAdmNo = _context.Adm_Master_College_StudentDMO.Where(d => d.MI_Id == dto.MI_Id && d.AMCST_ActiveFlag == true && d.AMCST_SOL.Equals("S") && d.AMCST_AdmNo.Equals(dto.AMCST_AdmNo)).Count();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }

        public CollegePreadmissionstudnetDto getdpstate(CollegePreadmissionstudnetDto d)
        {
            try
            {
                var AllState = _context.State.Where(t => t.IVRMMC_Id.Equals(d.IVRMMC_Id)).ToList();
                d.AllState = AllState.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return d;
        }
        public CollegePreadmissionstudnetDto saveAddress(CollegePreadmissionstudnetDto adds)
        {
            try
            {
                if (adds.PACA_Id > 0)
                {
                    var result = _precontext.PA_College_Application.Single(a => a.MI_Id == adds.MI_Id && a.PACA_Id == adds.PACA_Id);
                    result.PACA_PerStreet = adds.PACA_PerStreet;
                    result.PACA_PerArea = adds.PACA_PerArea;
                    result.PACA_PerCity = adds.PACA_PerCity;
                    result.PACA_PerState = adds.PACA_PerState;
                    result.IVRMMC_Id = adds.IVRMMC_Id;
                    result.PACA_PerPincode = adds.PACA_PerPincode;
                    result.PACA_ConStreet = adds.PACA_ConStreet;
                    result.PACA_ConArea = adds.PACA_ConArea;
                    result.PACA_ConCity = adds.PACA_ConCity;
                    result.PACA_ConState = adds.PACA_ConState;
                    result.PACA_ConCountryId = adds.PACA_ConCountryId;
                    result.PACA_ConPincode = adds.PACA_ConPincode;
                    result.UpdatedDate = DateTime.Now;
                    if (result.PACA_CompleteFillflag < 2)
                    {
                        result.PACA_CompleteFillflag = 2;
                    }

                    _precontext.Update(result);
                    int n = _precontext.SaveChanges();
                    if (n > 0)
                    {
                        adds.message = "Update";

                    }
                    else
                    {
                        adds.message = "";
                    }
                }
                else
                {
                    var mapp = Mapper.Map<PA_College_Application>(adds);
                    mapp.PACA_ActiveFlag = true;
                    mapp.CreatedDate = DateTime.Now;
                    mapp.UpdatedDate = DateTime.Now;
                    mapp.PACA_CompleteFillflag = 2;
                    _precontext.Add(mapp);
                    int n = _precontext.SaveChanges();
                    if (n > 0)
                    {
                        adds.message = "Add";

                    }
                    else
                    {
                        adds.message = "";
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return adds;
        }
        public CollegePreadmissionstudnetDto saveParentsDetails(CollegePreadmissionstudnetDto ParentsData)
        {
            try
            {
                if (ParentsData.PACA_Id > 0)
                {
                    var result = _precontext.PA_College_Application.Single(a => a.MI_Id == ParentsData.MI_Id && a.PACA_Id == ParentsData.PACA_Id);
                    result.PACA_FatherAliveFlag = ParentsData.PACA_FatherAliveFlag;
                    result.PACA_FatherName = ParentsData.PACA_FatherName;
                    result.PACA_FatherSurname = ParentsData.PACA_FatherSurname;
                    result.PACA_FatherAadharNo = ParentsData.PACA_FatherAadharNo;
                    result.PACA_FatherEducation = ParentsData.PACA_FatherEducation;
                    result.PACA_FatherOfficeAdd = ParentsData.PACA_FatherOfficeAdd;
                    result.PACA_FatherResAdd = ParentsData.PACA_FatherResAdd;
                    result.PACA_FatherOccupation = ParentsData.PACA_FatherOccupation;
                    result.PACA_FatherDesignation = ParentsData.PACA_FatherDesignation;
                    result.PACA_FatherBankAccNo = ParentsData.PACA_FatherBankAccNo;
                    result.PACA_FatherBankIFSCCode = ParentsData.PACA_FatherBankIFSCCode;
                    result.PACA_FatherCasteCertiNo = ParentsData.PACA_FatherCasteCertiNo;
                    result.PACA_FatherResTelPhno = ParentsData.PACA_FatherResTelPhno;
                    result.PACA_FatherOfficeTelPhno = ParentsData.PACA_FatherOfficeTelPhno;
                    result.PACA_FatherPhoto = ParentsData.PACA_FatherPhoto;
                    result.PACA_MotherPhoto = ParentsData.PACA_MotherPhoto;
                    result.PACA_FatherNationality = ParentsData.PACA_FatherNationality;
                    result.PACA_FatherReligion = ParentsData.PACA_FatherReligion;
                    result.PACA_FatherCaste = ParentsData.PACA_FatherCaste;
                    result.PACA_FatherSubCaste = ParentsData.PACA_FatherSubCaste;
                    result.PACA_FatherMonIncome = ParentsData.PACA_FatherMonIncome;
                    result.PACA_FatherAnnIncome = ParentsData.PACA_FatherAnnIncome;
                    result.PACA_FatherMobleNo = ParentsData.PACA_FatherMobleNo;
                    result.PACA_FatherEmailId = ParentsData.PACA_FatherEmailId;
                    //Mother details
                    result.PACA_MotherAliveFlag = ParentsData.PACA_MotherAliveFlag;
                    result.PACA_MotherName = ParentsData.PACA_MotherName;
                    result.PACA_MotherSurname = ParentsData.PACA_MotherSurname;
                    result.PACA_MotherAadharNo = ParentsData.PACA_MotherAadharNo;
                    result.PACA_MotherEducation = ParentsData.PACA_MotherEducation;
                    result.PACA_MotherOfficeAdd = ParentsData.PACA_MotherOfficeAdd;
                    result.PACA_MotherOccupation = ParentsData.PACA_MotherOccupation;
                    result.PACA_MotherDesignation = ParentsData.PACA_MotherDesignation;
                    result.PACA_MotherBankAccNo = ParentsData.PACA_MotherBankAccNo;
                    result.PACA_MotherBankIFSCCode = ParentsData.PACA_MotherBankIFSCCode;
                    result.PACA_MotherCasteCertiNo = ParentsData.PACA_MotherCasteCertiNo;
                    result.PACA_MotherReligion = ParentsData.PACA_MotherReligion;
                    result.PACA_MotherCaste = ParentsData.PACA_MotherCaste;
                    result.PACA_MotherSubCaste = ParentsData.PACA_MotherSubCaste;
                    result.PACA_MotherMonIncome = ParentsData.PACA_MotherMonIncome;
                    result.PACA_MotherAnnIncome = ParentsData.PACA_MotherAnnIncome;
                    result.PACA_MotherMobleNo = ParentsData.PACA_MotherMobleNo;
                    result.PACA_MotherEmailId = ParentsData.PACA_MotherEmailId;
                    result.PACA_MotherNationality = ParentsData.PACA_MotherNationality;
                    result.PACA_MotherResTelPhno = ParentsData.PACA_MotherResTelPhno;
                    result.PACA_MotherOfficeTelPhno = ParentsData.PACA_MotherOfficeTelPhno;
                    result.PACA_MotherResAdd = ParentsData.PACA_MotherResAdd;
                    result.PACA_FatherOfficeAddPincode = ParentsData.PACA_FatherOfficeAddPincode;
                    result.PACA_FatherResidentialAddPinCode = ParentsData.PACA_FatherResidentialAddPinCode;
                    result.PACA_MotherOfficeAddPinCode = ParentsData.PACA_MotherOfficeAddPinCode;
                    result.PACA_MotherResidentialAddPinCode = ParentsData.PACA_MotherResidentialAddPinCode;
                    result.PACA_MotherCountryCode = ParentsData.PACA_MotherCountryCode;
                    result.PACA_FatherCountryCode = ParentsData.PACA_FatherCountryCode;

                    if (ParentsData.PACA_IncomeCertificateFlg == null)
                    {
                        ParentsData.PACA_IncomeCertificateFlg = false;
                    }
                    result.PACA_IncomeCertificateFlg = ParentsData.PACA_IncomeCertificateFlg;



                    if (result.PACA_CompleteFillflag < 3)
                    {
                        result.PACA_CompleteFillflag = 3;
                    }

                    _precontext.Update(result);

                    int n = _precontext.SaveChanges();
                    if (n > 0)
                    {
                        ParentsData.message = "Update";

                    }
                    else
                    {
                        ParentsData.message = "Update";

                    }
                }
                else
                {
                    if (ParentsData.PACA_IncomeCertificateFlg == null)
                    {
                        ParentsData.PACA_IncomeCertificateFlg = false;
                    }
                    var mapp = Mapper.Map<PA_College_Application>(ParentsData);
                    mapp.CreatedDate = DateTime.Now;
                    mapp.UpdatedDate = DateTime.Now;
                    mapp.PACA_CompleteFillflag = 3;
                    mapp.PACA_ActiveFlag = true;
                    _precontext.Add(mapp);
                    int n = _precontext.SaveChanges();
                    if (n > 0)
                    {
                        ParentsData.message = "Add";

                    }
                    else
                    {
                        ParentsData.message = "Add";

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ParentsData;
        }
        //save and update multiple father mobile number
        public AdmMasterCollegeStudentDTO father_mobile_no(AdmMasterCollegeStudentDTO datamobileno)
        {
            try
            {
                if (datamobileno.FatherMultipleMobileNoDTO != null)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();
                    //getting all mobilenumbers
                    foreach (FatherMultipleMobileNoDTO ph in datamobileno.FatherMultipleMobileNoDTO)
                    {
                        temparr.Add(ph.ACSTPMN_Id);
                    }

                    //removing mobile number 
                    Array Phone_Noresultremove = _context.AdmCollegeStudentParentsMobileNoDMO.Where(t => !temparr.Contains(t.ACSTPMN_Id) && t.AMCST_Id == datamobileno.AMCST_Id && t.ACSTPMN_Flag.Equals("F")).ToArray();

                    foreach (AdmCollegeStudentParentsMobileNoDMO ph1 in Phone_Noresultremove)
                    {
                        _context.Remove(ph1);
                    }

                    //updating and saving 

                    foreach (FatherMultipleMobileNoDTO ph in datamobileno.FatherMultipleMobileNoDTO)
                    {
                        if (ph.ACSTPMN_Id > 0)
                        {
                            var Phone_Noresult = _context.AdmCollegeStudentParentsMobileNoDMO.Single(t => t.ACSTPMN_Id == ph.ACSTPMN_Id);
                            Phone_Noresult.UpdatedDate = DateTime.Now;
                            Phone_Noresult.ACSTPMN_MobileNo = Convert.ToInt64(ph.AMCST_FatherMobleNo);
                            _context.Update(Phone_Noresult);
                        }
                        else
                        {
                            if (ph.AMCST_FatherMobleNo != null)
                            {
                                AdmCollegeStudentParentsMobileNoDMO phone1 = new AdmCollegeStudentParentsMobileNoDMO();
                                phone1.CreatedDate = DateTime.Now;
                                phone1.UpdatedDate = DateTime.Now;
                                phone1.ACSTPMN_MobileNo = Convert.ToInt64(ph.AMCST_FatherMobleNo);
                                phone1.ACSTPMN_Flag = "F";
                                phone1.AMCST_Id = datamobileno.AMCST_Id;
                                _context.Add(phone1);
                            }
                            else
                            {

                            }

                        }
                        //  _AdmissionFormContext.SaveChanges();
                    }

                }
            }
            catch (Exception e)
            {
                _context.Database.RollbackTransaction();
            }

            return datamobileno;
        }

        //save and update multiple father email ids
        public AdmMasterCollegeStudentDTO father_email_ids(AdmMasterCollegeStudentDTO dataemailid)
        {
            try
            {
                // save Prev School details
                if (dataemailid.FatherMultipleEmailIdDTO != null)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();
                    //getting all emails
                    foreach (FatherMultipleEmailIdDTO ph in dataemailid.FatherMultipleEmailIdDTO)
                    {
                        temparr.Add(ph.ACSTPE_Id);
                    }

                    //removing email number 
                    Array Phone_Noresultremove = _context.AdmCollegeStudentParentsEmailIdDMO.Where(t => !temparr.Contains(t.ACSTPE_Id) && t.AMCST_Id == dataemailid.AMCST_Id && t.ACSTPE_Flag.Equals("F")).ToArray();
                    foreach (AdmCollegeStudentParentsEmailIdDMO ph1 in Phone_Noresultremove)
                    {
                        _context.Remove(ph1);
                    }

                    //updating and saving 

                    foreach (FatherMultipleEmailIdDTO ph in dataemailid.FatherMultipleEmailIdDTO)
                    {

                        if (ph.ACSTPE_Id > 0)
                        {
                            var Phone_Noresult = _context.AdmCollegeStudentParentsEmailIdDMO.Single(t => t.ACSTPE_Id == ph.ACSTPE_Id);
                            Phone_Noresult.UpdatedDate = DateTime.Now;
                            Phone_Noresult.ACSTPE_EmailId = ph.AMCST_FatheremailId;
                            _context.Update(Phone_Noresult);
                        }
                        else
                        {
                            if (ph.AMCST_FatheremailId != null)
                            {
                                AdmCollegeStudentParentsEmailIdDMO phone1 = new AdmCollegeStudentParentsEmailIdDMO();
                                phone1.CreatedDate = DateTime.Now;
                                phone1.UpdatedDate = DateTime.Now;
                                phone1.ACSTPE_EmailId = ph.AMCST_FatheremailId;
                                phone1.ACSTPE_Flag = "F";
                                phone1.AMCST_Id = dataemailid.AMCST_Id;
                                _context.Add(phone1);
                            }

                        }
                        //   _AdmissionFormContext.SaveChanges();
                    }
                }

            }
            catch (Exception e)
            {
                _context.Database.RollbackTransaction();
            }
            return dataemailid;
        }

        //save and update multiple mother mobile number
        public AdmMasterCollegeStudentDTO mother_mobile_no(AdmMasterCollegeStudentDTO datamobilenomother)
        {
            try
            {
                if (datamobilenomother.MotherMultipleMobileNoDTO != null)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();
                    //getting all mobilenumbers
                    foreach (MotherMultipleMobileNoDTO ph in datamobilenomother.MotherMultipleMobileNoDTO)
                    {
                        temparr.Add(ph.ACSTPMN_Id);
                    }

                    //removing mobile number 
                    Array Phone_Noresultremove = _context.AdmCollegeStudentParentsMobileNoDMO.Where(t => !temparr.Contains(t.ACSTPMN_Id) && t.AMCST_Id == datamobilenomother.AMCST_Id && t.ACSTPMN_Flag.Equals("M")).ToArray();
                    foreach (AdmCollegeStudentParentsMobileNoDMO ph1 in Phone_Noresultremove)
                    {
                        _context.Remove(ph1);
                    }

                    //updating and saving 

                    foreach (MotherMultipleMobileNoDTO ph in datamobilenomother.MotherMultipleMobileNoDTO)
                    {

                        if (ph.ACSTPMN_Id > 0)
                        {
                            var Phone_Noresult = _context.AdmCollegeStudentParentsMobileNoDMO.Single(t => t.ACSTPMN_Id == ph.ACSTPMN_Id);
                            Phone_Noresult.UpdatedDate = DateTime.Now;
                            Phone_Noresult.ACSTPMN_MobileNo = Convert.ToInt64(ph.AMCST_MotherMobleNo);
                            _context.Update(Phone_Noresult);
                        }
                        else
                        {
                            if (ph.AMCST_MotherMobleNo != null)
                            {
                                AdmCollegeStudentParentsMobileNoDMO phone1 = new AdmCollegeStudentParentsMobileNoDMO();
                                phone1.CreatedDate = DateTime.Now;
                                phone1.UpdatedDate = DateTime.Now;
                                phone1.ACSTPMN_Flag = "M";
                                phone1.ACSTPMN_MobileNo = Convert.ToInt64(ph.AMCST_MotherMobleNo);
                                phone1.AMCST_Id = datamobilenomother.AMCST_Id;
                                _context.Add(phone1);
                            }

                        }
                        //  _AdmissionFormContext.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {

                _context.Database.RollbackTransaction();
            }

            return datamobilenomother;
        }

        //saving and updating mother email ids
        public AdmMasterCollegeStudentDTO mother_email_id(AdmMasterCollegeStudentDTO dataemailidmother)
        {
            try
            {
                if (dataemailidmother.MotherMultipleEmailIdDTO != null)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();
                    //getting all emails
                    foreach (MotherMultipleEmailIdDTO ph in dataemailidmother.MotherMultipleEmailIdDTO)
                    {
                        temparr.Add(ph.ACSTPE_Id);
                    }

                    //removing email number 
                    Array Phone_Noresultremove = _context.AdmCollegeStudentParentsEmailIdDMO.Where(t => !temparr.Contains(t.ACSTPE_Id) && t.AMCST_Id == dataemailidmother.AMCST_Id && t.ACSTPE_Flag.Equals("M")).ToArray();
                    foreach (AdmCollegeStudentParentsEmailIdDMO ph1 in Phone_Noresultremove)
                    {
                        _context.Remove(ph1);
                    }

                    //updating and saving 

                    foreach (MotherMultipleEmailIdDTO ph in dataemailidmother.MotherMultipleEmailIdDTO)
                    {
                        if (ph.ACSTPE_Id > 0)
                        {
                            var Phone_Noresult = _context.AdmCollegeStudentParentsEmailIdDMO.Single(t => t.ACSTPE_Id == ph.ACSTPE_Id);
                            Phone_Noresult.UpdatedDate = DateTime.Now;
                            Phone_Noresult.ACSTPE_EmailId = ph.AMCST_MotheremailId;
                            _context.Update(Phone_Noresult);
                        }
                        else
                        {
                            if (ph.AMCST_MotheremailId != null)
                            {
                                AdmCollegeStudentParentsEmailIdDMO phone1 = new AdmCollegeStudentParentsEmailIdDMO();
                                phone1.CreatedDate = DateTime.Now;
                                phone1.UpdatedDate = DateTime.Now;
                                phone1.ACSTPE_EmailId = ph.AMCST_MotheremailId;
                                phone1.ACSTPE_Flag = "M";
                                phone1.AMCST_Id = dataemailidmother.AMCST_Id;
                                _context.Add(phone1);
                            }

                        }
                        //   _AdmissionFormContext.SaveChanges();
                    }
                }

            }
            catch (Exception e)
            {

                _context.Database.RollbackTransaction();
            }
            return dataemailidmother;
        }
        public CollegePreadmissionstudnetDto StateByCountryName(CollegePreadmissionstudnetDto ct)
        {
            try
            {
                var statename = _context.Country.Where(d => d.IVRMMC_Id == Convert.ToInt64(ct.countryName)).ToList();
                if (statename.Count > 0)
                {
                    ct.prevStateList = _context.State.Where(d => d.IVRMMC_Id == statename.FirstOrDefault().IVRMMC_Id).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ct;
        }
        public CollegePreadmissionstudnetDto saveOthersDetails(CollegePreadmissionstudnetDto others)
        {
            try
            {
                if (others.PACA_Id > 0)
                {

                    PrevSchooldetailsAddUpdate1(others, others.PACA_Id);
                    GuardianAddUpdate(others);

                   // previousexamsubjectwise1(others);
                    addactivity(others);
                    addachievement(others);
                    //_precontext.Update(result);
                    //int n = _precontext.SaveChanges();
                    //if (n > 0)
                    //{

                    //added 
                    stuCompexamdetailse1(others);
                    stuCompexamSubMarks1(others);
                    previousexamsubjectwise(others);
                    others.message = "Update";

                    //}
                    //else
                    //{
                    //    others.message = "";
                    //}
                    if (others.PACA_Id > 0)
                    {
                        var result = _precontext.PA_College_Application.Single(a => a.MI_Id == others.MI_Id && a.PACA_Id == others.PACA_Id);
                        if (result.PACA_CompleteFillflag < 4)
                        {
                            result.PACA_CompleteFillflag = 4;
                        }
                        _precontext.Update(result);

                        int n = _precontext.SaveChanges();
                    }


                }
                else
                {


                    PrevSchooldetailsAddUpdate(others);
                    GuardianAddUpdate(others);
                    previousexamsubjectwise(others);
                    addactivity(others);
                    addachievement(others);

                    stuCompexamdetailse1(others);
                    stuCompexamSubMarks1(others);
                    previousexamsubjectwise(others);
                    //_precontext.Add(result);
                    //int n = _precontext.SaveChanges();
                    //if (n > 0)
                    //{
                    others.message = "Add";
                    //}
                    //else
                    //{
                    //    others.message = "";
                    //}
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return others;
        }

        public CollegePreadmissionstudnetDto previousexamsubjectwise(CollegePreadmissionstudnetDto prevadd)
        {
            try
            {
                // save Prev School details
                if (prevadd.Adm_College_Student_SubjectMarksTempDTO != null)
                {
                    foreach (PreadmissionDTOs.com.vaps.College.Preadmission.Adm_College_Student_SubjectMarksDTO mob in prevadd.Adm_College_Student_SubjectMarksTempDTO)
                    {
                        if (mob.PACSTSUM_SubjectName != null && mob.PACSTSUM_SubjectName != "")
                        {
                            mob.PACA_Id = prevadd.PACA_Id;

                            PA_College_Student_SubjectMarks PrevexamSchool = Mapper.Map<PA_College_Student_SubjectMarks>(mob);
                            if (PrevexamSchool.PACSTSUM_Id > 0)
                            {
                                var PrevexamSchoolresult = _context.PA_College_Student_SubjectMarks.Single(t => t.PACSTSUM_Id == mob.PACSTSUM_Id);
                                PrevexamSchoolresult.UpdatedDate = DateTime.Now;
                                PrevexamSchoolresult.CreatedDate = PrevexamSchoolresult.CreatedDate;
                                Mapper.Map(mob, PrevexamSchoolresult);
                                _context.Update(PrevexamSchoolresult);
                            }
                            else
                            {  //added by 02/02/2017
                                PrevexamSchool.CreatedDate = DateTime.Now;
                                PrevexamSchool.UpdatedDate = DateTime.Now;
                                _context.Add(PrevexamSchool);
                            }
                            _context.SaveChanges();
                        }

                    }
                }

            }
            catch (Exception e)
            {

                _context.Database.RollbackTransaction();
            }
            return prevadd;
        }
        public CollegePreadmissionstudnetDto addactivity(CollegePreadmissionstudnetDto data)
        {
            try
            {
                // save Prev School details
                if (data.activitydto != null)
                {

                    var removedata = _context.PA_College_Student_PrevExtracurricularDMO.Where(e => e.PACA_Id == data.PACA_Id).ToList();

                    if (removedata.Count > 0)
                    {
                        foreach (var item in removedata)
                        {
                            _context.Remove(item);
                        }
                    }

                    foreach (var item in data.activitydto)
                    {

                        if (item.PACSPER_ActivityName != null && item.PACSPER_ActivityName != "")
                        {
                            PA_College_Student_PrevExtracurricularDMO obj = new PA_College_Student_PrevExtracurricularDMO();
                            obj.PACA_Id = data.PACA_Id;
                            obj.PACSPER_ActivityName = item.PACSPER_ActivityName;
                            obj.PACSPER_Type = item.PACSPER_Type;
                            _context.Add(obj);

                        }

                    }

                    _context.SaveChanges();







                }

            }
            catch (Exception e)
            {

                _context.Database.RollbackTransaction();
            }
            return data;
        }
        public CollegePreadmissionstudnetDto addachievement(CollegePreadmissionstudnetDto data)
        {
            try
            {
                // save Prev School details
                if (data.achievedto != null)
                {

                    var removedata = _context.PA_College_Student_AchivementsTypeDMO.Where(e => e.PACA_Id == data.PACA_Id).ToList();

                    if (removedata.Count > 0)
                    {
                        foreach (var item in removedata)
                        {
                            _context.Remove(item);
                        }
                    }

                    foreach (var item in data.achievedto)
                    {

                        if (item.PACSAT_AchivementsName != null && item.PACSAT_AchivementsName != "")
                        {
                            PA_College_Student_AchivementsTypeDMO obj = new PA_College_Student_AchivementsTypeDMO();
                            obj.PACA_Id = data.PACA_Id;
                            obj.PACSAT_AchivementsName = item.PACSAT_AchivementsName;
                            obj.PACSAT_type = item.PACSAT_type;
                            obj.PACSAT_Filename = item.PACSAT_Filename;
                            obj.PACSAT_Filepath = item.PACSAT_Filepath;
                            _context.Add(obj);

                        }

                    }

                    _context.SaveChanges();







                }

            }
            catch (Exception e)
            {

                _context.Database.RollbackTransaction();
            }
            return data;
        }
        // save and update Refference details
        public AdmMasterCollegeStudentDTO RefferenceDetailsAddUpdate(AdmMasterCollegeStudentDTO refdetadd)
        {
            string str = "0";
            try
            {
                //add & update Reference details
                if (refdetadd.SelectedRefrenceDetails != null)
                {
                    foreach (AdmCollegeStudentReferenceDTO refer in refdetadd.SelectedRefrenceDetails)
                    {
                        str = str + "," + refer.PAMR_Id;
                    }
                    List<AdmCollegeStudentReferenceDTO> result = new List<AdmCollegeStudentReferenceDTO>();
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CLG.getReference";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                           SqlDbType.BigInt)
                        {
                            Value = refdetadd.AMCST_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@str",
                           SqlDbType.VarChar)
                        {
                            Value = str
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        //var data = cmd.ExecuteNonQuery();

                        try
                        {
                            // var data = cmd.ExecuteNonQuery();

                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new AdmCollegeStudentReferenceDTO
                                    {
                                        ASRR_Id = Convert.ToInt64(dataReader["ASRR_Id"])
                                    });
                                    refdetadd.referenceIds = result.ToArray();
                                }
                            }
                            if (refdetadd.referenceIds != null)
                            {
                                foreach (AdmCollegeStudentReferenceDTO act1 in refdetadd.referenceIds)
                                {
                                    var Referenceresult = _context.AdmCollegeStudentReferenceDMO.Where(t => t.ASRR_Id == act1.ASRR_Id && t.AMCST_Id == refdetadd.AMCST_Id).ToList();
                                    if (Referenceresult.Any())
                                    {
                                        _context.Remove(Referenceresult.ElementAt(0));
                                        _context.SaveChanges();
                                    }
                                }
                            }
                            foreach (AdmCollegeStudentReferenceDTO mob in refdetadd.SelectedRefrenceDetails)
                            {
                                mob.AMCST_Id = refdetadd.AMCST_Id;

                                var Activityresult1 = _context.AdmCollegeStudentReferenceDMO.Where(t => t.ASRR_Id == mob.PAMR_Id && t.AMCST_Id == refdetadd.AMCST_Id).ToList();
                                if (Activityresult1.Count == 0)
                                {
                                    AdmCollegeStudentReferenceDMO Reference = Mapper.Map<AdmCollegeStudentReferenceDMO>(mob);
                                    Reference.ASRR_Id = mob.PAMR_Id;
                                    Reference.CreatedDate = DateTime.Now;
                                    Reference.UpdatedDate = DateTime.Now;
                                    _context.Add(Reference);
                                    //  _AdmissionFormContext.SaveChanges();
                                }

                                //    if (Reference.AMSTR_Id > 0)
                                //    {
                                //        var Referenceresult = _AdmissionFormContext.StudentReferenceDMO.Single(t => t.AMSTR_Id == mob.AMSTR_Id);
                                //        //added by 02/02/2017

                                //        Referenceresult.UpdatedDate = DateTime.Now;
                                //        Referenceresult.CreatedDate = Referenceresult.CreatedDate;

                                //        Mapper.Map(mob, Referenceresult);

                                //        _AdmissionFormContext.Update(Referenceresult);
                                //    }
                                //    else
                                //    {  //added by 02/02/2017
                                //        Reference.CreatedDate = DateTime.Now;
                                //        Reference.UpdatedDate = DateTime.Now;
                                //        _AdmissionFormContext.Add(Reference);
                                //    }
                                //    _AdmissionFormContext.SaveChanges();
                                //}
                            }

                        }
                        catch (Exception ex)
                        {
                            // _log.LogInformation("Student Reference  error");
                            // _log.LogDebug(ex.Message);
                            _context.Database.RollbackTransaction();
                        }

                    }
                }
            }
            catch (Exception e)
            {

                //  _log.LogInformation("Student Reference  error");
                //  _log.LogDebug(e.Message);
            }
            return refdetadd;
        }
        // save and update Source details
        public AdmMasterCollegeStudentDTO SourceDetailsAddUpdate(AdmMasterCollegeStudentDTO sourcedetadd)
        {
            string str = "0";
            try
            {
                //add & update Source details
                if (sourcedetadd.SelectedSourceDetails != null)
                {
                    foreach (AdmCollegeStudentSourceDTO src in sourcedetadd.SelectedSourceDetails)
                    {
                        str = str + "," + src.PAMS_Id;
                    }
                    List<AdmCollegeStudentSourceDTO> result = new List<AdmCollegeStudentSourceDTO>();
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CLG.getSource";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                           SqlDbType.BigInt)
                        {
                            Value = sourcedetadd.AMCST_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@str",
                           SqlDbType.VarChar)
                        {
                            Value = str
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        //var data = cmd.ExecuteNonQuery();

                        try
                        {
                            // var data = cmd.ExecuteNonQuery();

                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new AdmCollegeStudentSourceDTO
                                    {
                                        ASRS_Id = Convert.ToInt64(dataReader["ASRS_Id"])
                                    });
                                    sourcedetadd.sourceIds = result.ToArray();
                                }
                            }
                            if (sourcedetadd.sourceIds != null)
                            {
                                foreach (AdmCollegeStudentSourceDTO source in sourcedetadd.sourceIds)
                                {
                                    var Sourceresult = _context.AdmCollegeStudentSourceDMO.Where(t => t.ASRS_Id == source.ASRS_Id && t.AMCST_Id == sourcedetadd.AMCST_Id).ToList();
                                    if (Sourceresult.Any())
                                    {
                                        _context.Remove(Sourceresult.ElementAt(0));

                                    }
                                }
                                _context.SaveChanges();
                            }
                            foreach (AdmCollegeStudentSourceDTO mob in sourcedetadd.SelectedSourceDetails)
                            {
                                mob.AMCST_Id = sourcedetadd.AMCST_Id;
                                var Sourceresult1 = _context.AdmCollegeStudentSourceDMO.Where(t => t.ASRS_Id == mob.PAMS_Id && t.AMCST_Id == sourcedetadd.AMCST_Id).ToList();
                                if (Sourceresult1.Count == 0)
                                {
                                    AdmCollegeStudentSourceDMO Source = Mapper.Map<AdmCollegeStudentSourceDMO>(mob);
                                    Source.CreatedDate = DateTime.Now;
                                    Source.UpdatedDate = DateTime.Now;
                                    Source.ASRS_Id = mob.PAMS_Id;
                                    _context.Add(Source);
                                    // _AdmissionFormContext.SaveChanges();
                                }

                                //    if (Source.AMSTS_Id > 0)
                                //    {
                                //        var Sourceresult = _AdmissionFormContext.StudentSourceDMO.Single(t => t.AMSTS_Id == mob.AMSTS_Id);
                                //        //added by 02/02/2017

                                //        Sourceresult.UpdatedDate = DateTime.Now;
                                //        Sourceresult.CreatedDate = Sourceresult.CreatedDate;
                                //        Mapper.Map(mob, Sourceresult);

                                //        _AdmissionFormContext.Update(Sourceresult);
                                //    }
                                //    else
                                //    {  //added by 02/02/2017
                                //        Source.CreatedDate = DateTime.Now;
                                //        Source.UpdatedDate = DateTime.Now;
                                //        _AdmissionFormContext.Add(Source);
                                //    }
                                //    _AdmissionFormContext.SaveChanges();
                                //}
                            }

                        }
                        catch (Exception ex)
                        {
                            _context.Database.RollbackTransaction();
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return sourcedetadd;
        }
        //save and update Siblings details
        public AdmMasterCollegeStudentDTO SiblingsAddUpdate(AdmMasterCollegeStudentDTO sib)
        {
            try
            {
                //add & update Siblings details
                if (sib.SelectedSiblingDetails != null)
                {
                    foreach (AdmCollegeStudentSiblingsDetailsDTO mob in sib.SelectedSiblingDetails)
                    {
                        if (mob.ACSTS_SiblingsName != null && mob.ACSTS_SiblingsName != "")
                        {
                            mob.AMCST_Id = sib.AMCST_Id;
                            AdmCollegeStudentSiblingsDetailsDMO sibling = Mapper.Map<AdmCollegeStudentSiblingsDetailsDMO>(mob);
                            if (sibling.ACSTS_Id > 0)
                            {
                                var siblingNoresult = _context.AdmCollegeStudentSiblingsDetailsDMO.Single(t => t.ACSTS_Id == mob.ACSTS_Id);
                                //added by 02/02/2017

                                siblingNoresult.UpdatedDate = DateTime.Now;
                                siblingNoresult.CreatedDate = siblingNoresult.CreatedDate;

                                Mapper.Map(mob, siblingNoresult);

                                _context.Update(siblingNoresult);
                            }
                            else
                            {  //added by 02/02/2017
                                sibling.CreatedDate = DateTime.Now;
                                sibling.UpdatedDate = DateTime.Now;
                                _context.Add(sibling);
                            }
                            //  _AdmissionFormContext.SaveChanges();
                        }

                    }
                }
            }
            catch (Exception e)
            {
                _context.Database.RollbackTransaction();
            }
            return sib;
        }

        //save and update Prev School details.




        public CollegePreadmissionstudnetDto previousexamsubjectwise1(CollegePreadmissionstudnetDto prevadd)
        {
            try
            {


                List<long> PACSTSUM_Idss = new List<long>();

                List<PA_College_Student_SubjectMarks> remove = new List<PA_College_Student_SubjectMarks>();
                foreach (var item in prevadd.Adm_College_Student_SubjectMarksTempDTO)
                {
                    PACSTSUM_Idss.Add(item.PACSTSUM_Id);
                }
                if (PACSTSUM_Idss.Count > 0)
                {
                    remove = _context.PA_College_Student_SubjectMarks.Where(t => t.PACA_Id == prevadd.PACA_Id && !PACSTSUM_Idss.Contains(t.PACSTSUM_Id)).Distinct().ToList();
                }
                else
                {
                    remove = _context.PA_College_Student_SubjectMarks.Where(t => t.PACA_Id == prevadd.PACA_Id).Distinct().ToList();
                }
                if (remove.Count > 0)
                {
                    foreach (var item in remove)
                    {
                        _context.Remove(item);
                    }

                    _context.SaveChanges();

                }


                // save Prev School details
                if (prevadd.Adm_College_Student_SubjectMarksTempDTO != null)
                {
                    foreach (PreadmissionDTOs.com.vaps.College.Preadmission.Adm_College_Student_SubjectMarksDTO mob in prevadd.Adm_College_Student_SubjectMarksTempDTO)
                    {
                        if (mob.PACSTSUM_SubjectName != null && mob.PACSTSUM_SubjectName != "")
                        {
                            mob.PACA_Id = prevadd.PACA_Id;

                            PA_College_Student_SubjectMarks PrevexamSchool = Mapper.Map<PA_College_Student_SubjectMarks>(mob);
                            if (PrevexamSchool.PACSTSUM_Id > 0)
                            {
                                var PrevexamSchoolresult = _context.PA_College_Student_SubjectMarks.Single(t => t.PACSTSUM_Id == mob.PACSTSUM_Id);
                                PrevexamSchoolresult.UpdatedDate = DateTime.Now;
                                PrevexamSchoolresult.CreatedDate = PrevexamSchoolresult.CreatedDate;
                                Mapper.Map(mob, PrevexamSchoolresult);
                                _context.Update(PrevexamSchoolresult);
                            }
                            else
                            {  //added by 02/02/2017
                                PrevexamSchool.CreatedDate = DateTime.Now;
                                PrevexamSchool.UpdatedDate = DateTime.Now;
                                _context.Add(PrevexamSchool);
                            }
                            _context.SaveChanges();
                        }

                    }
                }

            }
            catch (Exception e)
            {

                _context.Database.RollbackTransaction();
            }
            return prevadd;
        }
        public CollegePreadmissionstudnetDto PrevSchooldetailsAddUpdate1(CollegePreadmissionstudnetDto prevadd, long PACA_Id
             )
        {
            try
            {
                // save Prev School details


                List<long> PACSTPS_Idss = new List<long>();

                List<PA_College_Student_PrevSchool> remove = new List<PA_College_Student_PrevSchool>();
                foreach (var item in prevadd.SelectedPrevSchoolDetails)
                {
                    PACSTPS_Idss.Add(item.PACSTPS_Id);
                }
                if (PACSTPS_Idss.Count > 0)
                {
                    remove = _precontext.PA_College_Student_PrevSchool.Where(t => t.PACA_Id == PACA_Id && !PACSTPS_Idss.Contains(t.PACSTPS_Id)).Distinct().ToList();
                }
                else
                {
                    remove = _precontext.PA_College_Student_PrevSchool.Where(t => t.PACA_Id == PACA_Id).Distinct().ToList();
                }
                if (remove.Count > 0)
                {
                    foreach (var item in remove)
                    {
                        _precontext.Remove(item);
                    }

                    _precontext.SaveChanges();

                }


                if (prevadd.SelectedPrevSchoolDetails != null)
                {


                    foreach (CollegeStudentPrevSchoolDTO mob in prevadd.SelectedPrevSchoolDetails)
                    {
                        if (mob.PACSTPS_PrvSchoolName != null && mob.PACSTPS_PrvSchoolName != "")
                        {
                            mob.PACA_Id = prevadd.PACA_Id;
                            PA_College_Student_PrevSchool PrevSchool = Mapper.Map<PA_College_Student_PrevSchool>(mob);
                            if (PrevSchool.PACSTPS_Id > 0)
                            {
                                var PrevSchoolresult = _precontext.PA_College_Student_PrevSchool.Single(t => t.PACSTPS_Id == mob.PACSTPS_Id);
                                PrevSchoolresult.UpdatedDate = DateTime.Now;
                                PrevSchoolresult.CreatedDate = PrevSchoolresult.CreatedDate;
                                Mapper.Map(mob, PrevSchoolresult);
                                _precontext.Update(PrevSchoolresult);
                            }
                            else
                            {  //added by 02/02/2017
                                PrevSchool.CreatedDate = DateTime.Now;
                                PrevSchool.UpdatedDate = DateTime.Now;
                                _precontext.Add(PrevSchool);

                            }
                            _precontext.SaveChanges();
                        }

                    }
                }

            }
            catch (Exception e)
            {

                _precontext.Database.RollbackTransaction();
            }
            return prevadd;
        }




        public CollegePreadmissionstudnetDto PrevSchooldetailsAddUpdate(CollegePreadmissionstudnetDto prevadd)
        {
            try
            {
                // save Prev School details





                if (prevadd.SelectedPrevSchoolDetails != null)
                {


                    foreach (CollegeStudentPrevSchoolDTO mob in prevadd.SelectedPrevSchoolDetails)
                    {
                        if (mob.PACSTPS_PrvSchoolName != null && mob.PACSTPS_PrvSchoolName != "")
                        {
                            mob.PACA_Id = prevadd.PACA_Id;
                            PA_College_Student_PrevSchool PrevSchool = Mapper.Map<PA_College_Student_PrevSchool>(mob);
                            if (PrevSchool.PACSTPS_Id > 0)
                            {
                                var PrevSchoolresult = _precontext.PA_College_Student_PrevSchool.Single(t => t.PACSTPS_Id == mob.PACSTPS_Id);
                                PrevSchoolresult.UpdatedDate = DateTime.Now;
                                PrevSchoolresult.CreatedDate = PrevSchoolresult.CreatedDate;
                                Mapper.Map(mob, PrevSchoolresult);
                                _precontext.Update(PrevSchoolresult);
                            }
                            else
                            {  //added by 02/02/2017
                                PrevSchool.CreatedDate = DateTime.Now;
                                PrevSchool.UpdatedDate = DateTime.Now;
                                _precontext.Add(PrevSchool);

                            }
                            _precontext.SaveChanges();
                        }

                    }
                }

            }
            catch (Exception e)
            {

                _precontext.Database.RollbackTransaction();
            }
            return prevadd;
        }

        //save and update gaurdian details
        public CollegePreadmissionstudnetDto GuardianAddUpdate(CollegePreadmissionstudnetDto guardadd)
        {
            try
            {
                if (guardadd.SelectedGuardianDetails != null)
                {
                    foreach (CollegeStudentGuardianDTO mob in guardadd.SelectedGuardianDetails)
                    {
                        if (mob.PACSTG_GuardianName != null && mob.PACSTG_GuardianName != "")
                        {
                            mob.PACA_Id = guardadd.PACA_Id;
                            PA_College_Student_Guardian Guardian = Mapper.Map<PA_College_Student_Guardian>(mob);
                            if (Guardian.PACSTG_Id > 0)
                            {
                                var Guardianresult = _precontext.PA_College_Student_Guardian.Single(t => t.PACSTG_Id == mob.PACSTG_Id);
                                Guardianresult.CreatedDate = Guardianresult.CreatedDate;
                                Guardianresult.UpdatedDate = DateTime.Now;
                                Guardianresult.PACSTG_GuardianPhoto = mob.PACSTG_GuardianPhoto;
                                Guardianresult.PACSTG_GuardianSign = mob.PACSTG_GuardianSign;
                                Guardianresult.PACSTG_Fingerprint = mob.PACSTG_Fingerprint;
                                Guardianresult.PACSTG_GuardianOfficeTelPhno = mob.PACSTG_GuardianOfficeTelPhno;
                                Guardianresult.PACSTG_GuardianOfficeTelPhno = mob.PACSTG_GuardianOfficeTelPhno;
                                Guardianresult.PACSTG_AnnualIncome = mob.PACSTG_AnnualIncome;
                                Guardianresult.PACSTG_Occupation = mob.PACSTG_Occupation;
                                Guardianresult.PACSTG_GuardianAddressPinCode = mob.PACSTG_GuardianAddressPinCode;
                                Guardianresult.PACSTG_CoutryCode = mob.PACSTG_CoutryCode;
                                Mapper.Map(mob, Guardianresult);
                                _precontext.Update(Guardianresult);
                            }
                            else
                            {
                                Guardian.CreatedDate = DateTime.Now;
                                Guardian.UpdatedDate = DateTime.Now;
                                _precontext.Add(Guardian);
                            }
                            _precontext.SaveChanges();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _precontext.Database.RollbackTransaction();

            }
            return guardadd;
        }
        public CollegePreadmissionstudnetDto saveDocuments(CollegePreadmissionstudnetDto docs)
        {
            try
            {
                if (docs.PACA_Id > 0)
                {
                    var result = _precontext.PA_College_Application.Single(a => a.MI_Id == docs.MI_Id && a.PACA_Id == docs.PACA_Id);
                    stud_doc_upload(docs);
                    GuardianAddUpdate(docs);
                    if (docs.PACA_PassportNo != "")
                    {
                        result.PACA_PassportNo = docs.PACA_PassportNo;
                        result.PACA_PassportIssuedAt = docs.PACA_PassportIssuedAt;
                        result.PACA_PassportIssueDate = docs.PACA_PassportIssueDate;
                        result.PACA_PassportIssuedCounrty = docs.PACA_PassportIssuedCounrty;
                        result.PACA_PassportIssuedPlace = docs.PACA_PassportIssuedPlace;
                        result.PACA_PassportExpiryDate = docs.PACA_PassportExpiryDate;
                    }
                    if (docs.PACA_VISAIssuedBy != "")
                    {
                        result.PACA_VISAIssuedBy = docs.PACA_VISAIssuedBy;
                        result.PACA_VISAValidFrom = docs.PACA_VISAValidFrom;
                        result.PACA_VISAValidTo = docs.PACA_VISAValidTo;
                    }

                    result.PACA_FatherPhoto = docs.PACA_FatherPhoto;
                    result.PACA_MotherPhoto = docs.PACA_MotherPhoto;
                    result.PACA_FatherSign = docs.PACA_FatherSign;
                    result.PACA_FatherFingerprint = docs.PACA_FatherFingerprint;
                    result.PACA_MotherSign = docs.PACA_MotherSign;
                    result.PACA_MotherFingerprint = docs.PACA_MotherFingerprint;
                    if (result.PACA_CompleteFillflag < 5)
                    {
                        result.PACA_CompleteFillflag = 5;
                    }
                    _precontext.Update(result);
                    int n = _precontext.SaveChanges();
                    if (n > 0)
                    {
                        docs.message = "Update";
                        List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                        mstConfig = _db.mstConfig.Where(d => d.MI_Id.Equals(docs.MI_Id) && d.ASMAY_Id.Equals(result.ASMAY_Id)).ToList();

                        if (mstConfig.FirstOrDefault().ISPAC_ApplFeeFlag == 1)
                        {
                            docs.prospectusPaymentlist = _feecontext.Fee_Y_Payment_PA_Application.Where(t => t.FYPPA_Type == "R" && t.PACA_Id == docs.PACA_Id).ToArray();
                            if (docs.prospectusPaymentlist == null || docs.prospectusPaymentlist.Length == 0)
                            {
                                docs.pay = "Pay";
                            }
                        }
                    }
                    else
                    {
                        docs.message = "";
                    }


                }
                else
                {
                    PA_College_Application result = new PA_College_Application();
                    result.MI_Id = docs.MI_Id;
                    stud_doc_upload(docs);
                    GuardianAddUpdate(docs);
                    if (docs.PACA_PassportNo != "")
                    {
                        result.PACA_PassportNo = docs.PACA_PassportNo;
                        result.PACA_PassportIssuedAt = docs.PACA_PassportIssuedAt;
                        result.PACA_PassportIssueDate = docs.PACA_PassportIssueDate;
                        result.PACA_PassportIssuedCounrty = docs.PACA_PassportIssuedCounrty;
                        result.PACA_PassportIssuedPlace = docs.PACA_PassportIssuedPlace;
                        result.PACA_PassportExpiryDate = docs.PACA_PassportExpiryDate;
                    }
                    if (docs.PACA_VISAIssuedBy != "")
                    {
                        result.PACA_VISAIssuedBy = docs.PACA_VISAIssuedBy;
                        result.PACA_VISAValidFrom = docs.PACA_VISAValidFrom;
                        result.PACA_VISAValidTo = docs.PACA_VISAValidTo;
                    }
                    result.PACA_FatherPhoto = docs.PACA_FatherPhoto;
                    result.PACA_MotherPhoto = docs.PACA_MotherPhoto;
                    result.PACA_FatherSign = docs.PACA_FatherSign;
                    result.PACA_FatherFingerprint = docs.PACA_FatherFingerprint;
                    result.PACA_MotherSign = docs.PACA_MotherSign;
                    result.PACA_MotherFingerprint = docs.PACA_MotherFingerprint;
                    result.PACA_CompleteFillflag = 5;
                    result.CreatedDate = DateTime.Now;
                    result.UpdatedDate = DateTime.Now;
                    _precontext.Add(result);
                    int n = _precontext.SaveChanges();
                    if (n > 0)
                    {
                        docs.PACA_Id = result.PACA_Id;
                        docs.message = "Add";
                        List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                        mstConfig = _db.mstConfig.Where(d => d.MI_Id.Equals(docs.MI_Id) && d.ASMAY_Id.Equals(result.ASMAY_Id)).ToList();

                        if (mstConfig.FirstOrDefault().ISPAC_ApplFeeFlag == 1)
                        {
                            docs.prospectusPaymentlist = _feecontext.Fee_Y_Payment_PA_Application.Where(t => t.FYPPA_Type == "R" && t.PACA_Id == docs.PACA_Id).ToArray();
                            if (docs.prospectusPaymentlist == null || docs.prospectusPaymentlist.Length == 0)
                            {
                                docs.pay = "Pay";
                            }
                        }
                    }
                    else
                    {
                        docs.message = "";

                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return docs;
        }
        //save and update Documents details
        public CollegePreadmissionstudnetDto stud_doc_upload(CollegePreadmissionstudnetDto docsupld)
        {
            try
            {
                //store student documents
                if (docsupld.Uploaded_documentList.Count() > 0)
                {
                    foreach (CollegeDocumentDTO dto in docsupld.Uploaded_documentList)
                    {

                        dto.PACA_Id = docsupld.PACA_Id;

                        if (dto.Document_Path != null && dto.Document_Path != "")
                        {
                            PA_College_Student_Documents document = new PA_College_Student_Documents();
                            if (dto.PACSTD_Id > 0)
                            {
                                var documentNoresult = _precontext.PA_College_Student_Documents.Single(t => t.PACSTD_Id == dto.PACSTD_Id);
                                documentNoresult.AMSMD_Id = dto.AMSMD_Id;
                                documentNoresult.ACSTD_Doc_Name = dto.ACSTD_Doc_Name;
                                documentNoresult.ACSTD_Doc_Path = dto.Document_Path;
                                document.CreatedDate = documentNoresult.CreatedDate;
                                document.UpdatedDate = DateTime.Now;
                                _precontext.Update(documentNoresult);
                            }
                            else
                            {

                                document.ACSTD_Doc_Name = dto.ACSTD_Doc_Name;
                                document.ACSTD_Doc_Path = dto.Document_Path;
                                document.PACA_Id = docsupld.PACA_Id;
                                document.AMSMD_Id = dto.AMSMD_Id;
                                document.CreatedDate = DateTime.Now;
                                document.UpdatedDate = DateTime.Now;
                                _precontext.Add(document);
                            }
                            _precontext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _precontext.Database.RollbackTransaction();
            }
            return docsupld;
        }



        public CollegePreadmissionstudnetDto savesubjects(CollegePreadmissionstudnetDto data)
        {
            try
            {
                // save Prev School details
                if (data.optsubids != null && data.langsubids != null)
                {

                    var removedata = _context.PA_College_Student_SubjectDMO.Where(e => e.PACA_Id == data.PACA_Id).ToList();

                    if (removedata.Count > 0)
                    {
                        foreach (var item in removedata)
                        {
                            _context.Remove(item);
                        }
                    }

                    foreach (var item in data.langsubids)
                    {


                        PA_College_Student_SubjectDMO obj = new PA_College_Student_SubjectDMO();
                        obj.PACA_Id = data.PACA_Id;
                        obj.ISMS_Id = item.ISMS_Id;
                        obj.PACSS_UpdatedBy = data.ID;
                        obj.PACSS_CreatedBy = data.ID;
                        obj.PACSS_CreatedDate = DateTime.Now;
                        obj.PACSS_UpdatedDate = DateTime.Now;
                        obj.PACSS_ActiveFlg = true;
                        _context.Add(obj);



                    }



                    foreach (var item1 in data.optsubids)
                    {


                        PA_College_Student_SubjectDMO obj = new PA_College_Student_SubjectDMO();
                        obj.PACA_Id = data.PACA_Id;
                        obj.ISMS_Id = item1.ISMS_Id;
                        obj.PACSS_UpdatedBy = data.ID;
                        obj.PACSS_CreatedBy = data.ID;
                        obj.PACSS_CreatedDate = DateTime.Now;
                        obj.PACSS_UpdatedDate = DateTime.Now;
                        obj.PACSS_ActiveFlg = true;
                        _context.Add(obj);



                    }

                    _context.SaveChanges();







                }

            }
            catch (Exception e)
            {

                _context.Database.RollbackTransaction();
            }
            return data;
        }

        public string insertdatainfeetables(string miid, string groupidss, string studentid, string headid, decimal amount, string transid, string refid, string yearid, DateTime indianTime, string transactionid)
        {
            var contactexisttransaction = 0;

            try
            {
                string recnoen = "";
                var fetchfmhotid = (from a in _feecontext.Fee_M_Online_TransactionDMO
                                    where (a.AMST_Id == Convert.ToInt64(studentid) && a.FMOT_Trans_Id == transid.ToString())
                                    select new CollegeFeeTransactionDTO
                                    {
                                        FMHOT_Id = a.FMOT_Id,
                                        FMA_Amount = a.FMOT_Amount
                                    }).ToArray();

                for (int r = 0; r < fetchfmhotid.Count(); r++)
                {
                    var fethchfmgids = (from a in _feecontext.CLG_Fee_Yearly_Group_Head_Mapping
                                        from b in _feecontext.FeeHeadClgDMO
                                        where (b.MI_Id == Convert.ToInt64(miid) && a.FMH_Id == b.FMH_Id && b.FMH_Flag == "R" && a.ASMAY_Id == Convert.ToInt32(yearid))
                                        select new CollegeFeeTransactionDTO
                                        {
                                            FMG_Id = a.FMG_Id,
                                            FMH_Id = a.FMH_Id
                                        }).Distinct().ToArray();

                    List<long> grpid = new List<long>();
                    List<long> headidss = new List<long>();

                    foreach (var item in fethchfmgids)
                    {
                        grpid.Add(item.FMG_Id);
                        headidss.Add(item.FMH_Id);
                        groupidss = groupidss + "," + item.FMG_Id;
                    }

                    List<CollegeFeeTransactionDTO> list_all = new List<CollegeFeeTransactionDTO>();
                    List<CollegeFeeTransactionDTO> list_repts = new List<CollegeFeeTransactionDTO>();
                    list_all = (from b in _feecontext.Fee_Groupwise_AutoReceiptDMO
                                from c in _feecontext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                where (b.MI_Id == Convert.ToInt64(miid) && b.ASMAY_Id == Convert.ToInt64(yearid) && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                                select new CollegeFeeTransactionDTO
                                {
                                    FGAR_PrefixName = b.FGAR_PrefixName,
                                    FGAR_SuffixName = b.FGAR_SuffixName,
                                    FGAR_Id = c.FGAR_Id,
                                }
                         ).Distinct().ToList();

                    using (var cmd = _feecontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "receiptnogeneration";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                            SqlDbType.VarChar, 100)
                        {
                            Value = Convert.ToInt32(miid)
                        });

                        cmd.Parameters.Add(new SqlParameter("@asmayid",
                           SqlDbType.NVarChar, 100)
                        {
                            Value = Convert.ToInt32(yearid)
                        });
                        cmd.Parameters.Add(new SqlParameter("@fmgid",
                       SqlDbType.NVarChar, 100)
                        {
                            Value = groupidss
                        });

                        cmd.Parameters.Add(new SqlParameter("@receiptno",
            SqlDbType.NVarChar, 500)
                        {
                            Direction = ParameterDirection.Output
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var data1 = cmd.ExecuteNonQuery();

                        recnoen = cmd.Parameters["@receiptno"].Value.ToString();

                        var groupwisefmaids = (from a in _feecontext.Fee_T_Online_TransactionDMO
                                               from c in _feecontext.Fee_M_Online_TransactionDMO
                                               where (a.FMOT_Id == c.FMOT_Id && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id && c.AMST_Id == Convert.ToInt64(studentid))
                                               select new CollegeFeeTransactionDTO
                                               {
                                                   FCMAS_Id = a.FMA_Id,
                                                   FCSS_ToBePaid = Convert.ToInt64(a.FTOT_Amount)
                                               }
                             ).ToArray();

                        Fee_Y_PaymentDMO onlinemtrans = new Fee_Y_PaymentDMO();

                        onlinemtrans.ASMAY_Id = Convert.ToInt64(yearid);
                        onlinemtrans.FYP_ReceiptNo = recnoen;

                        onlinemtrans.FYP_ReceiptDate = indianTime;
                        onlinemtrans.FYP_TotalPaidAmount = Convert.ToDecimal(fetchfmhotid[r].FMA_Amount);
                        onlinemtrans.FYP_Remarks = "Online Regular Payment";
                        onlinemtrans.FYP_Currency = "1";
                        onlinemtrans.FYP_PayModeType = "Single";
                        onlinemtrans.FYP_TransactionTypeFlag = "O";

                        onlinemtrans.FYP_ChequeBounceFlag = "CL";
                        onlinemtrans.MI_Id = Convert.ToInt64(miid);
                        onlinemtrans.FYP_DOE = indianTime;
                        onlinemtrans.CreatedDate = indianTime;
                        onlinemtrans.UpdatedDate = indianTime;

                        //added on 02-07-2018
                        var Euserid = (from a in _feecontext.FeeGroupClgDMO
                                       where (a.MI_Id == Convert.ToInt64(miid) && grpid.Contains(a.FMG_Id))
                                       select new CollegeFeeTransactionDTO
                                       {
                                           enduserid = a.user_id,
                                       }
                           ).Distinct().Take(1).ToArray();
                        //added on 02-07-2018

                        onlinemtrans.User_Id = Convert.ToInt64(Euserid[0].enduserid);
                        onlinemtrans.FYP_Transaction_Id = transid;

                        onlinemtrans.FYP_ChallanStatusFlag = "Sucessfull";
                        //onlinemtrans.FYP_PaymentReference_Id = refid.ToString();
                        onlinemtrans.FYP_ChallanNo = "";

                        _feecontext.Fee_Y_PaymentDMO.Add(onlinemtrans);

                        Fee_Y_Payment_PaymentModeDMO onlinestuappmode = new Fee_Y_Payment_PaymentModeDMO();

                        onlinestuappmode.FYP_Id = onlinemtrans.FYP_Id;
                        onlinestuappmode.FYPPM_BankName = "";
                        onlinestuappmode.FYPPM_ClearanceDate = indianTime;
                        onlinestuappmode.FYPPM_ClearanceStatusFlag = "1";
                        onlinestuappmode.FYPPM_DDChequeDate = indianTime;
                        onlinestuappmode.FYPPM_DDChequeNo = "0";
                        onlinestuappmode.FYPPM_PaymentReference_Id = refid.ToString();
                        onlinestuappmode.FYPPM_TransactionTypeFlag = "O";
                        onlinestuappmode.FYPPM_TotalPaidAmount = Convert.ToDecimal(fetchfmhotid[r].FMA_Amount);
                        onlinestuappmode.FYPPM_LedgerId = 0;
                        onlinestuappmode.FYPPM_Transaction_Id = transid;

                        _feecontext.Fee_Y_Payment_PaymentModeDMO.Add(onlinestuappmode);


                        Fee_Y_Payment_PA_Application onlinestuapp = new Fee_Y_Payment_PA_Application();

                        onlinestuapp.FYP_Id = onlinemtrans.FYP_Id;
                        onlinestuapp.PACA_Id = Convert.ToInt64(studentid);
                        onlinestuapp.FYPPA_TotalPaidAmount = Convert.ToDecimal(fetchfmhotid[r].FMA_Amount);
                        onlinestuapp.FYPPA_ActiveFlag = 1;
                        onlinestuapp.FYPPA_Type = "R";

                        _feecontext.Fee_Y_Payment_PA_Application.Add(onlinestuapp);

                        for (int s = 0; s < groupwisefmaids.Count(); s++)
                        {
                            Fee_T_College_PaymentDMO onlinettrans = new Fee_T_College_PaymentDMO();
                            onlinettrans.FYP_Id = onlinemtrans.FYP_Id;
                            onlinettrans.FCMAS_Id = groupwisefmaids[s].FCMAS_Id;
                            onlinettrans.FTCP_PaidAmount = groupwisefmaids[s].FCSS_ToBePaid;
                            onlinettrans.FTCP_FineAmount = 0;
                            onlinettrans.FTCP_ConcessionAmount = 0;
                            onlinettrans.FTCP_WaivedAmount = 0;
                            onlinettrans.FTCP_Remarks = "Online Regular Payment";

                            _feecontext.Fee_T_College_PaymentDMO.Add(onlinettrans);

                        }

                        groupidss = "0";
                    }

                }

                using (var dbCtxTxn = _feecontext.Database.BeginTransaction())
                {
                    try
                    {
                        contactexisttransaction = _feecontext.SaveChanges();
                        dbCtxTxn.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        dbCtxTxn.Rollback();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return contactexisttransaction.ToString();
        }
        //master competitive exam
        public CollegePreadmissionstudnetDto stuCompexamdetailse1(CollegePreadmissionstudnetDto prevadd)
        {
            try
            {


                //List<long> PACSTCEM_Idss = new List<long>();

                //List<PA_College_Student_CEMarksClgDMO> remove = new List<PA_College_Student_CEMarksClgDMO>();
                //foreach (var item in prevadd.PA_College_Student_CEMarksClgDTO)
                //{
                //    PACSTCEM_Idss.Add(item.PAMCEXM_Id);
                //}
                //if (PACSTCEM_Idss.Count > 0)
                //{
                //    remove = _precontext.PA_College_Student_CEMarksClgDMO.Where(t => t.PACA_Id == prevadd.PACA_Id && !PACSTCEM_Idss.Contains(t.PAMCEXM_Id)).Distinct().ToList();
                //}
                //else
                //{
                //    remove = _precontext.PA_College_Student_CEMarksClgDMO.Where(t => t.PACA_Id == prevadd.PACA_Id).Distinct().ToList();
                //}
                //if (remove.Count > 0)
                //{
                //    foreach (var item in remove)
                //    {
                //        _precontext.Remove(item);
                //    }

                //    _precontext.SaveChanges();

                //}



                if (prevadd.PA_College_Student_CEMarksClgDTO != null)
                {
                    foreach (PreadmissionDTOs.com.vaps.College.Preadmission.PA_College_Student_CEMarksClgDTO mob in prevadd.PA_College_Student_CEMarksClgDTO)
                    {
                        if (mob.PAMCEXM_Id != null && mob.PAMCEXM_Id != 0)
                        {
                            mob.PACA_Id = prevadd.PACA_Id;
                            mob.PACSTCEM_ActiveFlg = true;

                            PA_College_Student_CEMarksClgDMO compExamDetails = Mapper.Map<PA_College_Student_CEMarksClgDMO>(mob);
                            var compExamDetailsdup = _precontext.PA_College_Student_CEMarksClgDMO.Count(t => t.PACA_Id == mob.PACA_Id && t.PACSTCEM_ActiveFlg == true);
                            if (compExamDetailsdup > 0)
                            {
                               // var compExamDetailsresult = _precontext.PA_College_Student_CEMarksClgDMO.FirstOrDefault(t => t.PACSTCEM_Id == mob.PACSTCEM_Id && t.PACSTCEM_ActiveFlg == true);

                                //compExamDetailsresult.PACSTCEM_ActiveFlg = true;
                                Mapper.Map(mob, compExamDetails);
                                _precontext.Update(compExamDetails);
                            }
                            else
                            {  //added by 02/02/2017
                                compExamDetails.PACSTCEM_ActiveFlg = true;
                                _precontext.Add(compExamDetails);
                            }
                            _context.SaveChanges();
                        }

                    }
                }

            }
            catch (Exception e)
            {

                _precontext.Database.RollbackTransaction();
            }
            return prevadd;
        }

        public CollegePreadmissionstudnetDto stuCompexamSubMarks1(CollegePreadmissionstudnetDto prevadd)
        {
            try
            {


                //List<long> PACSTCEM_Idss = new List<long>();


                //List<PA_College_Student_CEMarks_SubjectClgDMO> remove = new List<PA_College_Student_CEMarks_SubjectClgDMO>();
                //foreach (var item in prevadd.PA_College_Student_CEMarks_SubjectClgDTO)
                //{
                //    PACSTCEM_Idss.Add(item.PACSTCEM_Id);
                //}

                //if (PACSTCEM_Idss.Count > 0 )
                //{
                //    remove = _precontext.PA_College_Student_CEMarks_SubjectClgDMO.Where(t => t.PACA_Id == prevadd.PACA_Id && !PACSTCEM_Idss.Contains(t.PAMCEXM_Id) ).Distinct().ToList();
                //}
                //else
                //{
                //    remove = _precontext.PA_College_Student_CEMarks_SubjectClgDMO.Where(t => t.PACA_Id == prevadd.PACA_Id ).Distinct().ToList();
                //}
                //if (remove.Count > 0)
                //{
                //    foreach (var item in remove)
                //    {
                //        _precontext.Remove(item);
                //    }

                //    _precontext.SaveChanges();

                //}



                if (prevadd.PA_College_Student_CEMarks_SubjectClgDTO != null)
                {
                    foreach (PreadmissionDTOs.com.vaps.College.Preadmission.PA_College_Student_CEMarks_SubjectClgDTO mob in prevadd.PA_College_Student_CEMarks_SubjectClgDTO)
                    {
                        //foreach (var sub in prevadd.PA_College_Student_CEMarks_SubjectClgDTO)
                        //{
                            if (mob.PAMCEXM_Id != null && mob.PAMCEXM_Id != 0)
                            {

                                mob.PACA_Id = prevadd.PACA_Id;
                                mob.PACSTCEMS_ActiveFlg = true;
                                var compSubMarksdup = _precontext.PA_College_Student_CEMarks_SubjectClgDMO.Count(t => t.PACA_Id == mob.PACA_Id && t.PACSTCEMS_ActiveFlg == true && t.PACSTCEMS_Id == mob.PACSTCEMS_Id);
                                PA_College_Student_CEMarks_SubjectClgDMO compSubMarksDetails = Mapper.Map<PA_College_Student_CEMarks_SubjectClgDMO>(mob);

                            //if (compSubMarksdup > 0)
                            //{

                            //    //if (compSubMarksDetails.PACSTCEMS_Id > 0)
                            //    //{
                            //    var compSubMarksDetailsresult = _precontext.PA_College_Student_CEMarks_SubjectClgDMO.Single(t => t.PACSTCEMS_Id == sub.PACSTCEMS_Id && t.PACSTCEMS_ActiveFlg == true);
                            //    // compSubMarksDetailsresult.PACSTCEMS_ActiveFlg = true;
                            //    Mapper.Map(mob, compSubMarksDetailsresult);
                            //    _precontext.Update(compSubMarksDetailsresult);
                            //    // }

                            //}


                            if (compSubMarksdup >0)
                            {

                                if (compSubMarksDetails.PACSTCEMS_Id > 0)
                                {
                                    //var compSubMarksDetailsresult = _precontext.PA_College_Student_CEMarks_SubjectClgDMO.Single(t => t.PACSTCEMS_Id == mob.PACSTCEMS_Id && t.PACSTCEMS_ActiveFlg == true);
                                    // compSubMarksDetailsresult.PACSTCEMS_ActiveFlg = true;
                                      Mapper.Map(mob, compSubMarksDetails);
                                    _precontext.Update(compSubMarksDetails);
                                }
 
                            }
                            else
                            {  //added by 02/02/2017
                                compSubMarksDetails.PACSTCEMS_MaxMarks = prevadd.PACSTCEMS_MaxMarks;
                                compSubMarksDetails.PACSTCEMS_ActiveFlg = true;
                                _precontext.Add(compSubMarksDetails);
                            }
                        }
                            _context.SaveChanges();
                      //  }

                    }
                }

            }
            catch (Exception e)
            {

                _precontext.Database.RollbackTransaction();
            }
            return prevadd;
        }

        //master competitive exam

        public CollegePreadmissionstudnetDto compExamName(CollegePreadmissionstudnetDto ct)
        {
            try
            {
                List<long> sub_ids = new List<long>();
                if (ct.tempidlist != null)
                {
                    if (ct.tempidlist.Length > 0)
                    {
                        foreach (var item in ct.tempidlist)
                        {
                            sub_ids.Add(item.PAMCEXMSUB_Id);
                        }
                    }
                }

                var subname = _precontext.Master_CompetitiveExamsSubjectsClgDMO.Where(d => d.PAMCEXMSUB_Id == Convert.ToInt64(ct.PAMCEXMSUB_Id) && d.PAMCEXMSUB_ActiveFlg == true).ToList();
                if (ct.subflg == true)
                {
                    
                    if (subname.Count > 0)
                    {
                        //if(ct.tempidlist != null)
                        //{
                        //    if (ct.tempidlist.Length > 0)
                        //    {
                        //        ct.compSubList = _precontext.Master_CompetitiveExamsSubjectsClgDMO.Where(d => d.PAMCEXMSUB_Id == subname.FirstOrDefault().PAMCEXMSUB_Id && d.PAMCEXMSUB_ActiveFlg == true && !sub_ids.Contains(d.PAMCEXMSUB_Id)).ToArray();
                        //    }
                          
                        //}
                        //else
                        //{
                            ct.compSubList = _precontext.Master_CompetitiveExamsSubjectsClgDMO.Where(d => d.PAMCEXMSUB_Id == subname.FirstOrDefault().PAMCEXMSUB_Id && d.PAMCEXMSUB_ActiveFlg == true).ToArray();
                        //}

                    }
                }
                else
                {
                    var examname = _precontext.Master_Competitive_ExamsClgDMO.Where(d => d.PAMCEXM_Id == Convert.ToInt64(ct.PAMCEXM_Id) && d.MI_Id == ct.MI_Id && d.PAMCEXM_ActiveFlg == true).ToList();
                    if (examname.Count > 0)
                    {
                        ct.compExamList = _precontext.Master_CompetitiveExamsSubjectsClgDMO.Where(d => d.PAMCEXM_Id == examname.FirstOrDefault().PAMCEXM_Id && d.PAMCEXMSUB_ActiveFlg == true).ToArray();
                    }
                    if (ct.tempidlist != null)
                    {
                        if (ct.tempidlist.Length > 0)
                        {
                            ct.compExamList = _precontext.Master_CompetitiveExamsSubjectsClgDMO.Where(d =>d.PAMCEXMSUB_ActiveFlg == true && !sub_ids.Contains(d.PAMCEXMSUB_Id) && d.PAMCEXM_Id== examname.FirstOrDefault().PAMCEXM_Id).ToArray();
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ct;
        }


        //Razor pay
        public PaymentDetails razorgetpaymentresponse(PaymentDetails response)
        {
            try
            {
                PaymentDetails dto = new PaymentDetails();
                // StudentApplicationDTO stu = new StudentApplicationDTO();
                CollegePreadmissionstudnetDto stu = new CollegePreadmissionstudnetDto();
                Fee_StudentTransactionClgDTO data = new Fee_StudentTransactionClgDTO();
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                string url = "https://api.razorpay.com/v1/payments/" + response.razorpay_payment_id + "/transfers";
                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _db.Fee_PaymentGateway_Details.Where(t => t.MI_Id == response.IVRMOP_MIID && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();
                //RazorpayClient client = new RazorpayClient(paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);
                //Razorpay.Api.Payment payment = client.Payment.Fetch(response.razorpay_payment_id);
                ////response.order_id = payment.Attributes["order_id"];

                //single account added on 17/12/2019

                var accountvalidation = (from a in _db.Fee_PaymentGateway_Details
                                         where (a.MI_Id == response.IVRMOP_MIID && a.FPGD_PGName == "RAZORPAY")
                                         select new Fee_StudentTransactionClgDTO
                                         {
                                             FPGD_SubMerchantId = a.FPGD_SubMerchantId
                                         }).Distinct().ToArray();

                //single account added on 17/12/2019

                var fetchfmhotid = (from a in _db.Fee_M_Online_TransactionDMO
                                    where (a.FMOT_Trans_Id == response.order_id.ToString() && a.FMOT_Amount > 0)
                                    select new Fee_StudentTransactionClgDTO
                                    {
                                        FMA_Amount = a.FMOT_Amount,
                                        MI_Id = a.MI_Id,
                                        ASMAY_Id = a.ASMAY_ID,
                                        PACA_Id = a.PASR_Id,
                                        FMOT_Receipt_no = a.FMOT_Receipt_no
                                    }).ToArray();
                var fetchstudentdeatils = (from a in _db.PA_College_Application
                                           where (a.PACA_Id == Convert.ToInt64(fetchfmhotid[0].PACA_Id))
                                           select new Fee_StudentTransactionClgDTO
                                           {
                                               pacA_MobileNo = a.PACA_MobileNo,
                                               pacA_emailId = a.PACA_emailId,
                                               ASMCL_ID = a.AMCO_Id,
                                               PACA_RegistrationNo = a.PACA_RegistrationNo,
                                               PACA_FirstName = a.PACA_FirstName + ' ' + a.PACA_MiddleName + ' ' + a.PACA_MiddleName,
                                               PACA_Id = a.PACA_Id
                                           }).ToArray();
                Dictionary<String, object> transfersnotes = new Dictionary<String, object>();
                Dictionary<String, object> transfers = new Dictionary<String, object>();

                for (int r = 0; r < fetchfmhotid.Count(); r++)
                {
                    transfers.Clear();
                    //var fetchaccountid = (from a in _feecontext.Clg_Fee_AmountEntry_DMO
                    //                      from b in _feecontext.FeeHeadClgDMO
                    //                      from c in _feecontext.Fee_OnlinePayment_Mapping
                    //                      from d in _feecontext.Fee_PaymentGateway_Details
                    //                      from e in _feecontext.PAYUDETAILS
                    //                      from f in _feecontext.feeYCC
                    //                      from g in _feecontext.feeYCCC
                    //                      where (a.FMH_Id == b.FMH_Id && a.FMH_Id == c.FMH_Id && a.FMG_Id == c.fmg_id && c.fpgd_id == d.FPGD_Id && d.IMPG_Id == e.IMPG_Id && a.FMCC_Id == f.FMCC_Id && f.FYCC_Id == g.FYCC_Id && a.ASMAY_Id == f.ASMAY_Id && a.MI_Id == fetchfmhotid[0].MI_Id && a.ASMAY_Id == fetchfmhotid[0].ASMAY_Id && g.ASMCL_Id == fetchstudentdeatils[0].ASMCL_ID && b.FMH_Flag == "R" && e.IMPG_PGFlag == "RAZORPAY")
                    //                      select new FeeStudentTransactionDTO
                    //                      {
                    //                          FPGD_SubMerchantId = d.FPGD_SubMerchantId
                    //                      }).Distinct().ToArray();

                    var fetchaccountid = (from a in _feecontext.Clg_Fee_AmountEntry_DMO
                                          from b in _feecontext.FeeHeadClgDMO
                                          from c in _feecontext.Fee_OnlinePayment_Mapping
                                          from d in _feecontext.Fee_PaymentGateway_Details
                                          from e in _feecontext.PAYUDETAILS
                                          where (a.FMH_Id == b.FMH_Id && a.FMH_Id == c.FMH_Id && a.FMG_Id == c.fmg_id && c.fpgd_id == d.FPGD_Id && d.IMPG_Id == e.IMPG_Id  && a.MI_Id == fetchfmhotid[0].MI_Id && a.ASMAY_Id == fetchfmhotid[0].ASMAY_Id &&  b.FMH_Flag == "R" && e.IMPG_PGFlag == "RAZORPAY")
                                          select new Fee_StudentTransactionClgDTO
                                          {
                                              FPGD_SubMerchantId = d.FPGD_SubMerchantId
                                          }).Distinct().ToArray();

                    transfersnotes.Add("notes_1", fetchstudentdeatils[0].PACA_FirstName);
                    transfersnotes.Add("notes_2", fetchstudentdeatils[0].PACA_RegistrationNo);
                    transfersnotes.Add("notes_3", fetchstudentdeatils[0].PACA_Id);
                    transfersnotes.Add("notes_4", fetchstudentdeatils[0].pacA_MobileNo);
                    transfersnotes.Add("notes_5", fetchstudentdeatils[0].pacA_emailId);

                    transfers.Add("account", (fetchaccountid.FirstOrDefault().FPGD_SubMerchantId));
                    transfers.Add("amount", (Convert.ToInt32(fetchfmhotid.FirstOrDefault().FMA_Amount.ToString()) * 100).ToString());
                    transfers.Add("currency", "INR");
                    transfers.Add("notes", transfersnotes);

                    if (accountvalidation.Count() > 1)
                    {
                      //  Razorpay.Api.Transfer payment1 = client.Transfer.Create(transfers);
                        //transferAPI trapay = new transferAPI();
                        //if (payment1.Attributes["id"] != "")
                        //{
                        //    trapay.transfer_id = payment1.Attributes["id"];
                        //    trapay.entity = payment1.Attributes["entity"];
                        //    trapay.source = payment1.Attributes["source"];
                        //    trapay.recipient = payment1.Attributes["recipient"];
                        //    trapay.amount = payment1.Attributes["amount"];
                        //    trapay.created_at = payment1.Attributes["created_at"];

                        //    FEE_RAZOR_TRANSFER_API_DETAILS fet = new FEE_RAZOR_TRANSFER_API_DETAILS();
                        //    fet.TRANSFER_ID = trapay.transfer_id;
                        //    fet.ENTITY = trapay.entity;
                        //    fet.SOURCE = trapay.source;
                        //    fet.RECIPIENT = trapay.recipient;
                        //    fet.AMOUNT = (Convert.ToInt32(trapay.amount) / 100).ToString();
                        //    fet.CREATED_AT = trapay.created_at;
                        //    fet.ORDER_ID = response.order_id;

                        //    fet.PAYMENT_ID = response.razorpay_payment_id;
                        //    fet.MI_ID = Convert.ToInt32(fetchfmhotid[0].MI_Id);
                        //    fet.SETTLEMENT_FLAG = "0";

                        //    fet.CREATED_BY = indianTime;
                        //    fet.UPDATED_BY = indianTime;
                        //    _db.Add(fet);
                        //    var contactExists = _db.SaveChanges();
                        //    if (contactExists == 1)
                        //    {
                        //        response.status = "success";
                        //    }
                        //    else
                        //    {
                        //        response.status = "Failure";
                        //    }
                        //}
                    }

                    else
                    {
                        FEE_RAZOR_TRANSFER_API_DETAILS fet = new FEE_RAZOR_TRANSFER_API_DETAILS();
                        fet.TRANSFER_ID = "";
                        fet.ENTITY = "";
                        fet.SOURCE = "";
                        fet.RECIPIENT = "";
                        fet.AMOUNT = (Convert.ToInt32(fetchfmhotid.FirstOrDefault().FMA_Amount.ToString()) * 100).ToString();
                        fet.CREATED_AT = indianTime.ToString();
                        fet.ORDER_ID = response.order_id;

                        fet.PAYMENT_ID = response.razorpay_payment_id;
                        fet.MI_ID = Convert.ToInt32(fetchfmhotid[0].MI_Id);
                        fet.SETTLEMENT_FLAG = "0";

                        fet.CREATED_BY = indianTime;
                        fet.UPDATED_BY = indianTime;
                        _db.Add(fet);
                        var contactExists = _db.SaveChanges();
                        if (contactExists == 1)
                        {
                            response.status = "success";
                        }
                        else
                        {
                            response.status = "Failure";
                        }
                    }
                }
                //   FeePaymentDetailsDMO feeypayment = Mapper.Map<FeePaymentDetailsDMO>(response);
                if (response.status == "success")
                {
                    stu.MI_Id = Convert.ToInt64(fetchfmhotid[0].MI_Id);
                    stu.PACA_MobileNo = fetchstudentdeatils[0].pacA_MobileNo;
                    stu.PACA_Id = Convert.ToInt64(fetchfmhotid[0].PACA_Id);
                    stu.PACA_emailId = fetchstudentdeatils[0].pacA_emailId;
                    stu.ASMAY_Id = Convert.ToInt64(fetchfmhotid[0].ASMAY_Id);
                    data.MI_Id = Convert.ToInt64(fetchfmhotid[0].MI_Id);
                    data.ASMCL_ID = Convert.ToInt64(fetchstudentdeatils[0].ASMCL_ID);
                    data.ASMAY_Id = Convert.ToInt64(fetchfmhotid[0].ASMAY_Id);
                    response.amount = fetchfmhotid.FirstOrDefault().FMA_Amount;
                    //response.responseupdate = fetchfmhotid.FirstOrDefault().FMOT_Receipt_no;
                    string recno = get_grp_reptno(data);
                    var confirmstatus = 0;
                    //if (recno != "0")
                    //{
                    //    response.responseupdate = recno;
                    //    confirmstatus = _ProspectusContext.Database.ExecuteSqlCommand("Preadmission_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", stu.MI_Id, data.ASMCL_ID, stu.pasR_Id, stu.ASMAY_Id, response.amount, response.order_id, response.razorpay_payment_id, response.udf6, recno);
                    //}
                    //else
                    //{
                    //    response.responseupdate = response.txnid;
                    //    recno = response.txnid;
                    //    confirmstatus = _ProspectusContext.Database.ExecuteSqlCommand("Preadmission_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", stu.MI_Id, data.ASMCL_ID, stu.pasR_Id, stu.ASMAY_Id, response.amount, response.order_id, response.razorpay_payment_id, response.udf6, recno);
                    //}
                    if (confirmstatus > 0)
                    {
                        List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                        mstConfig = _db.mstConfig.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.ASMAY_Id.Equals(stu.ASMAY_Id)).ToList();
                        if (mstConfig.FirstOrDefault().ISPAC_ApplMailFlag == 1)
                        {
                            Email Email = new Email(_db);
                            Email.sendmail(stu.MI_Id, stu.PACA_emailId, "STUDENT_REGISTRATION", stu.PACA_Id);
                        }
                        if (mstConfig.FirstOrDefault().ISPAC_ApplSMSFlag == 1)
                        {
                            SMS sms = new SMS(_db);
                            sms.sendSms(stu.MI_Id, stu.PACA_MobileNo, "STUDENT_REGISTRATION", stu.PACA_Id);
                        }
                    }
                }
                else
                {
                    dto.status = response.status;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return response;
        }



        public CollegePreadmissionstudnetDto Clgapplicationstudocs(CollegePreadmissionstudnetDto data)
        {
            try
            {
                //data.ddoc = (from b in _precontext.MasterDocumentDMO
                            
                //             where (a.AMSMD_Id == b.AMSMD_Id && a.PACA_Id == c.PACA_Id && c.PACA_Id == data.PACA_Id && a.PACA_Id == data.PACA_Id)
                //             select new CollegePreadmissionstudnetDto
                //             {

                //                 PACA_Id = c.PACA_Id,
                //                 PACSTD_Id = a.PACSTD_Id,
                //                 ACSTD_Doc_Path = a.ACSTD_Doc_Path,
                //                 AMSMD_DocumentName = b.AMSMD_DocumentName,
                //                 AMSMD_Id = a.AMSMD_Id,
                //                 PACA_StudentPhoto = c.PACA_StudentPhoto,
                //                 PACA_FirstName = c.PACA_FirstName,
                //                 PACA_MiddleName = c.PACA_MiddleName,
                //                 PACA_LastName = c.PACA_LastName
                //             }
                //      ).ToArray();


                data.docdownload = _precontext.MasterDocumentDMO.Where(a=>a.AMSMD_Id==data.AMSMD_Id).ToArray();



            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return data;
        }
    }
}
