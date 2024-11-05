using AutoMapper;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Preadmission;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Preadmission;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class CollegeStudentAdmissionImpl : Interface.CollegeStudentAdmissionInterface
    {
        ClgAdmissionContext _context;

        private readonly DomainModelMsSqlServerContext _db;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly ILogger<CollegeStudentAdmissionImpl> _log;
        public CollegeStudentAdmissionImpl(ClgAdmissionContext context, DomainModelMsSqlServerContext _dbcontext, ILogger<CollegeStudentAdmissionImpl> _lo,
            UserManager<ApplicationUser> UserManager)
        {
            _context = context;
            _db = _dbcontext;
            _log = _lo;
            _UserManager = UserManager;
        }
        public AdmMasterCollegeStudentDTO Getdetails(AdmMasterCollegeStudentDTO obj)
        {

            try
            {
                var allyearonload = _context.AcademicYear.Where(d => d.MI_Id == obj.MI_Id && d.Is_Active == true).Select(d => new MasterAcademic { ASMAY_Id = d.ASMAY_Id, ASMAY_Year = d.ASMAY_Year }).ToList();
                if (allyearonload.Count > 0)
                {
                    obj.academicYearOnLoad = allyearonload.ToArray();
                    //obj.academicYearOnLoad=(from a in _context.AcademicYear
                    //                         where (a.MI_Id==obj.MI_Id && a.ASMAY_Id !=obj.ASMAY_Id && a.Is_Active == true)
                    //                         select new MasterAcademic
                    //                         {
                    //                             ASMAY_Id = a.ASMAY_Id,
                    //                             ASMAY_Year = a.ASMAY_Year
                    //                         }).ToArray();
                    obj.AllAcademicYear = _context.AcademicYear.Where(d => d.MI_Id == obj.MI_Id && d.Is_Active == true && d.ASMAY_Id == obj.ASMAY_Id).Select(d => new MasterAcademic { ASMAY_Id = d.ASMAY_Id, ASMAY_Year = d.ASMAY_Year }).ToArray();
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
                              where m.AMCO_Id == n.AMCO_Id && m.MI_Id == obj.MI_Id && m.ASMAY_Id == obj.ASMAY_Id && m.ACAYC_ActiveFlag == true
                              && n.AMCO_ActiveFlag == true
                              select new AdmMasterCollegeStudentDTO
                              {
                                  AMCO_Id = m.AMCO_Id,
                                  ACAYC_Id = m.ACAYC_Id,
                                  courseName = n.AMCO_CourseName,
                                  ASMAY_Id = m.ASMAY_Id
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


                var Students = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                from crse in _context.MasterCourseDMO
                                from branch in _context.ClgMasterBranchDMO
                                from sem in _context.CLG_Adm_Master_SemesterDMO
                                from batc in _context.AdmCollegeMasterBatchDMO
                                where (adm_stu.AMCO_Id == crse.AMCO_Id && adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMSE_Id == sem.AMSE_Id && adm_stu.ACMB_Id == batc.ACMB_Id && adm_stu.MI_Id == obj.MI_Id && adm_stu.AMCST_ActiveFlag == true && adm_stu.AMCST_SOL != "Del")
                                select new AdmMasterCollegeStudentDTO
                                {
                                    //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                    //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                    //AMCST_LastName = adm_stu.AMCST_LastName,
                                    AMCST_Date = adm_stu.AMCST_Date,
                                    AMCST_Sex = adm_stu.AMCST_Sex,
                                    AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                    AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                    AMCST_emailId = adm_stu.AMCST_emailId,
                                    stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                    AMCST_Id = adm_stu.AMCST_Id,
                                    courseName = crse.AMCO_CourseName,
                                    branchName = branch.AMB_BranchName,
                                    semesterName = sem.AMSE_SEMName,
                                    batchName = batc.ACMSN_SessionName,
                                    AMCST_SOL = adm_stu.AMCST_SOL,
                                    AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                    (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                    (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()

                                }).OrderByDescending(d => d.AMCST_Id).Take(10).ToList();

                //multiple mobile number feteching   start
                var query = (from a in _context.Adm_Master_College_StudentDMO
                             from b in _context.AdmCollegeStudentSMSNoDMO
                             where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == obj.MI_Id && a.AMCST_ActiveFlag == true && a.AMCST_SOL != "Del")
                             select new Adm_M_Student_TempMobileNo
                             {
                                 UserName = b.AMCST_Id,
                                 Role = b.ACSTSMS_MobileNo.ToString()
                             }).OrderByDescending(d => d.UserName).Take(10).ToList();
                //AdmMasterCollegeStudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                int count = query.Count() + 1;
                Adm_M_Student_TempMobileNo[] temp = new Adm_M_Student_TempMobileNo[count];

                query.CopyTo(temp);

                string value = null;
                Dictionary<long, string> tempDictionary = new Dictionary<long, string>();
                for (int i = 0; i < query.Count(); i++)
                {
                    if (query[i].UserName == temp[i].UserName)
                    {
                        if (!tempDictionary.ContainsKey(query[i].UserName))
                        {
                            tempDictionary.Add(query[i].UserName, query[i].Role);
                        }
                        else
                        {

                            tempDictionary.TryGetValue(query[i].UserName, out value);
                            value = value + ",  " + query[i].Role;
                            tempDictionary[query[i].UserName] = value;
                        }
                    }
                }

                List<Adm_M_Student_TempMobileNo> list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                obj.stdmobile = list.ToArray();
                //end here 

                //assigning the mobile number to main list start
                for (int k = 0; k < Students.Count(); k++)
                {
                    if (k < list.Count())
                    {
                        if (list[k].UserName == Students[k].AMCST_Id)
                        {
                            Students[k].stdmobilenumber = list[k].Role;
                        }
                        else
                        {
                            Students[k].stdmobilenumber = Students[k].stdmobilenumber;
                        }
                    }
                    else
                    {
                        Students[k].stdmobilenumber = Students[k].stdmobilenumber;
                    }
                }

                //end

                //multiple email ids start 
                var query1 = (from a in _context.Adm_Master_College_StudentDMO
                              from b in _context.AdmCollegeStudentEmailIdDMO
                              where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == obj.MI_Id && a.AMCST_ActiveFlag == true && a.AMCST_SOL != "Del")
                              select new Adm_M_Student_TempEmailId
                              {
                                  UserNameemail = a.AMCST_Id,
                                  Roleemail = b.ACSTE_EmailId
                              }).OrderByDescending(d => d.UserNameemail).Take(10).ToList();


                int count1 = query1.Count() + 1;
                Adm_M_Student_TempEmailId[] temp1 = new Adm_M_Student_TempEmailId[count1];

                query1.CopyTo(temp1);

                string value1 = null;
                Dictionary<long, string> tempDictionary1 = new Dictionary<long, string>();
                for (int i = 0; i < query1.Count(); i++)
                {
                    if (query1[i].UserNameemail == temp1[i].UserNameemail)
                    {
                        if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                        {
                            tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                        }
                        else
                        {
                            tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                            value1 = value1 + ",  " + query1[i].Roleemail;
                            tempDictionary1[query1[i].UserNameemail] = value1;
                        }
                    }
                }
                List<Adm_M_Student_TempEmailId> list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                obj.stdemail = list1.ToArray();
                //end

                //assigning the email ids to main list start
                for (int k = 0; k < Students.Count(); k++)
                {
                    if (k < list1.Count())
                    {
                        if (list1[k].UserNameemail == Students[k].AMCST_Id)
                        {
                            Students[k].AMCST_emailId = list1[k].Roleemail;
                        }
                        else
                        {
                            Students[k].AMCST_emailId = Students[k].AMCST_emailId;
                        }
                    }
                    else
                    {
                        Students[k].AMCST_emailId = Students[k].AMCST_emailId;
                    }
                }

                //end
                obj.StudentList = Students.ToArray();
                //Master Board.
                obj.boardList = _context.MasterBorad.Where(d => d.MI_Id == obj.MI_Id && d.Is_Active == true).ToArray();
                //School Type
                obj.Schooltypelist = _context.MasterSchoolType.Where(d => d.Is_Active == true).ToArray();

                //Master Documnets.
                var MasterDocumentDMO = _context.MasterDocumentDMO.Where(t => t.MI_Id == obj.MI_Id).ToList();
                obj.DocumentList = MasterDocumentDMO.ToArray();

                var combination = _context.Adm_Prv_Sch_CombinationDMO.Where(a => a.MI_Id == obj.MI_Id && a.ADMCB_Activeflag == true).ToList();
                obj.combinationlist = combination.ToArray();

                obj.mastersectionlist = _context.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == obj.MI_Id && a.ACMS_ActiveFlag == true).OrderBy(a => a.ACMS_Order).ToArray();


                obj.compExamarray = _context.Master_Competitive_AdmExamsClgDMO.Where(c => c.MI_Id == obj.MI_Id && c.AMCEXM_ActiveFlg == true).Distinct().OrderBy(d => d.AMCEXM_CompetitiveExams).ToArray();

                obj.compSubarray = _context.Master_CompetitiveExamsSubjectsAdmClgDMO.Where(s => s.AMCEXMSUB_ActiveFlg == true).Distinct().OrderBy(m => m.AMCEXMSUB_SubjectName).ToArray();

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
        public AdmMasterCollegeStudentDTO getCourse(AdmMasterCollegeStudentDTO data)
        {
            try
            {
                var course = (from m in _context.CLG_Adm_College_AY_CourseDMO
                              from n in _context.MasterCourseDMO
                              where m.AMCO_Id == n.AMCO_Id && m.MI_Id == data.MI_Id && m.ASMAY_Id == data.ASMAY_Id && m.ACAYC_ActiveFlag == true
                              && n.AMCO_ActiveFlag == true
                              select new AdmMasterCollegeStudentDTO
                              {
                                  AMCO_Id = m.AMCO_Id,
                                  ACAYC_Id = m.ACAYC_Id,
                                  courseName = n.AMCO_CourseName,
                                  ASMAY_Id = m.ASMAY_Id
                              }).Distinct().ToList();
                if (course.Count > 0)
                {
                    data.courses = course.ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AdmMasterCollegeStudentDTO getBranch(AdmMasterCollegeStudentDTO data)
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
        public AdmMasterCollegeStudentDTO getSemester(AdmMasterCollegeStudentDTO dt)
        {
            try
            {
                var MaxCapacity = _context.ClgMasterBranchDMO.Where(d => d.MI_Id == dt.MI_Id && d.AMB_ActiveFlag == true && d.AMB_Id == dt.AMB_Id).Select(d => d.AMB_StudentCapacity).ToList();

                var StudCount = _context.Adm_Master_College_StudentDMO.Where(d => d.MI_Id == dt.MI_Id && d.ASMAY_Id == dt.ASMAY_Id && d.AMCST_SOL != "D" && d.AMB_Id == dt.AMB_Id).Count();

                if (StudCount >= MaxCapacity.FirstOrDefault())
                {
                    dt.Message = "MaxCapacity";
                    return dt;
                }
                else
                {
                    var semester = (from m in _context.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    from n in _context.CLG_Adm_Master_SemesterDMO
                                    from m1 in _context.CLG_Adm_College_AY_Course_BranchDMO
                                    from a1 in _context.CLG_Adm_College_AY_CourseDMO
                                    where m.AMSE_Id == n.AMSE_Id && m1.ACAYC_Id == a1.ACAYC_Id && m1.ACAYCB_Id == m.ACAYCB_Id && a1.ASMAY_Id == dt.ASMAY_Id
                                    && m.MI_Id == dt.MI_Id && m.ACAYCB_Id == dt.ACAYCB_Id && m.ACAYCBS_ActiveFlag == true
                                    && n.AMSE_ActiveFlg == true
                                    select new AdmMasterCollegeStudentDTO
                                    {
                                        AMSE_Id = m.AMSE_Id,
                                        semesterName = n.AMSE_SEMName,
                                        ACAYCBS_Id = m.ACAYCBS_Id,
                                        ACAYCB_Id = m.ACAYCB_Id
                                    }).Distinct().ToArray();
                    if (semester.Length > 0)
                    {
                        dt.semesters = semester;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dt;
        }
        public AdmMasterCollegeStudentDTO getcaste(AdmMasterCollegeStudentDTO dto)
        {
            try
            {
                dto.AllCaste = (from m in _context.CasteCategory
                                from n in _context.Caste
                                where m.IMCC_Id == n.IMCC_Id && n.IMCC_Id == dto.IMCC_Id && n.MI_Id == dto.MI_Id
                                select new AdmMasterCollegeStudentDTO
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
        public async Task<save_firsttab_details> saveStudentDetails(save_firsttab_details data)
        {
            List<retuen_student_DTO> studentListdto = new List<retuen_student_DTO>();
            try
            {
                if (data.AMCST_Id > 0)
                {
                    var result = _context.Adm_Master_College_StudentDMO.Single(a => a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id);

                    result.ACMB_Id = data.ACMB_Id;
                    result.ACQC_Id = data.ACQC_Id;
                    result.ACQ_Id = data.ACQ_Id;
                    result.AMB_Id = data.AMB_Id;
                    result.AMCOC_Id = data.AMCOC_Id;
                    result.AMCO_Id = data.AMCO_Id;
                    result.AMCST_AadharNo = data.AMCST_AadharNo;
                    result.AMCST_AdmNo = data.AMCST_AdmNo;
                    result.AMCST_Age = data.AMCST_Age;
                    result.AMCST_BirthCertNo = data.AMCST_BirthCertNo;
                    result.AMCST_BirthPlace = data.AMCST_BirthPlace;
                    result.AMCST_BloodGroup = data.AMCST_BloodGroup;
                    result.AMCST_BPLCardFlag = data.AMCST_BPLCardFlag;
                    result.AMCST_BPLCardNo = data.AMCST_BPLCardNo;
                    result.AMCST_Date = Convert.ToDateTime(data.AMCST_Date);
                    result.AMCST_DOB = data.AMCST_DOB;
                    result.AMCST_BiometricId = data.AMCST_BiometricId;
                    result.AMCST_RFCardNo = data.AMCST_RFCardNo;
                    result.AMCST_DOBin_words = data.AMCST_DOBin_words;
                    result.AMCST_ECSFlag = data.AMCST_ECSFlag;
                    result.AMCST_FirstName = data.AMCST_FirstName;
                    //result.AMCST_GymReqdFlag = data.AMCST_GymReqdFlag;
                    //result.AMCST_HostelReqdFlag = data.AMCST_HostelReqdFlag;
                    result.AMCST_LastName = data.AMCST_LastName;
                    result.AMCST_MiddleName = data.AMCST_MiddleName;
                    result.AMCST_Nationality = data.AMCST_Nationality;
                    result.AMCST_RegistrationNo = data.AMCST_RegistrationNo;
                    result.AMCST_Sex = data.AMCST_Sex;
                    result.AMCST_StuBankAccNo = data.AMCST_StuBankAccNo;
                    result.AMCST_StuBankIFSCCode = data.AMCST_StuBankIFSCCode;
                    result.AMCST_StuCasteCertiNo = data.AMCST_StuCasteCertiNo;
                    result.AMCST_StudentPhoto = data.AMCST_StudentPhoto;
                    result.AMCST_StudentSubCaste = data.AMCST_StudentSubCaste;
                    //result.AMCST_TransportReqdFlag = data.AMCST_TransportReqdFlag;
                    result.AMCST_MotherTongue = data.AMCST_MotherTongue;
                    result.AMSE_Id = data.AMSE_Id;
                    result.ASMAY_Id = data.ASMAY_Id;
                    result.IMCC_Id = data.IMCC_Id;
                    result.IMC_Id = data.IMC_Id;
                    result.IVRMMR_Id = data.IVRMMR_Id;
                    result.ACST_Id = data.ACST_Id;
                    result.ACSS_Id = data.ACSS_Id;
                    result.AMCST_Taluk = data.AMCST_Taluk;
                    result.AMCST_Village = data.AMCST_Village;
                    result.AMCST_District = data.AMCST_District;
                    result.AMCST_Urban_Rural = data.AMCST_Urban_Rural;
                    result.AMCST_Divyangjan = data.AMCST_Divyangjan;
                    result.UpdatedDate = DateTime.Now;
                    result.AMCST_CoutryCode = data.Adm_College_Student_SMSNoDTO[0].ACSTSMS_CountryCode;
                    //student mobile saving
                    string stdmobileno = "";
                    string stdmobilenosms = "";
                    string stdemailsms = "";
                    string stdmobilecountrycode = "";
                    var count = data.Adm_College_Student_SMSNoDTO.Length;
                    for (int i = 0; i < count; i++)
                    {
                        stdmobilenosms = data.Adm_College_Student_SMSNoDTO[0].AMCST_MobileNo;
                        if (i == 0)
                        {
                            stdmobileno = data.Adm_College_Student_SMSNoDTO[i].AMCST_MobileNo;
                            stdmobilecountrycode = data.Adm_College_Student_SMSNoDTO[i].ACSTSMS_CountryCode;
                        }
                    }

                    //student email id saving
                    string stdemailids = "";
                    for (int j = 0; j < data.Adm_College_Student_EmailIdDTO.Length; j++)
                    {
                        stdemailsms = data.Adm_College_Student_EmailIdDTO[0].AMCST_emailId;
                        if (j == 0)
                        {
                            stdemailids = data.Adm_College_Student_EmailIdDTO[j].AMCST_emailId;
                        }
                    }
                    result.AMCST_MobileNo = Convert.ToInt64(stdmobilenosms);
                    result.AMCST_emailId = stdemailids;
                    data.AMCST_Id = data.AMCST_Id;
                    student_mobile_no(data);
                    student_email_id(data);
                    _context.Update(result);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        data.Message = "Update";

                        try
                        {
                            var getstdappid = _db.CollegeStudentlogin.Where(a => a.AMCST_Id == data.AMCST_Id && a.IVRMULSPGC_Flag == "S").
                                Select(a => a.IVRMUL_Id).ToList();

                            ApplicationUser user = new ApplicationUser();
                            user = await _UserManager.FindByIdAsync(getstdappid[0].ToString());
                            user.PhoneNumber = stdmobileno;
                            user.UserImagePath = data.AMCST_StudentPhoto;
                            user.Email = stdemailids;
                            user.NormalizedEmail = stdemailids;
                            var i = await _UserManager.UpdateAsync(user);

                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "clg_AdmStudentWiseAttendanceDone";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar) { Value = data.AMCST_Id });

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
                                                    dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                                );
                                            }
                                            retObject.Add((ExpandoObject)dataRow);
                                        }
                                    }
                                    data.attendanceArray = retObject.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            if(data.attendanceArray.Length==0)
                            {
                                var result_yearly = _context.Adm_College_Yearly_StudentDMO.Single(a => a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id);

                                result_yearly.AMB_Id = data.AMB_Id;
                                result_yearly.AMSE_Id = data.AMSE_Id;
                                result_yearly.AMCO_Id = Convert.ToInt64(data.AMCO_Id);
                                if (data.ACMS_Id != 0)
                                {
                                    result_yearly.ACMS_Id = data.ACMS_Id;
                                }
                                else
                                {

                                    long sec_id = _context.Adm_College_Master_SectionDMO.SingleOrDefault(s => s.ACMS_SectionName == "A" && s.MI_Id == data.MI_Id).ACMS_Id;
                                    if (sec_id > 0)
                                    {
                                        result_yearly.ACMS_Id = sec_id;
                                    }

                                }



                                result_yearly.ACYST_ActiveFlag = 1;
                                result_yearly.LoginId = Convert.ToInt64(data.LoginId);
                                result_yearly.UpdatedDate = DateTime.Now;
                                // _context.Add(result_yearly);
                                _context.Update(result_yearly);
                                int m = _context.SaveChanges();
                            }
                           
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        data.Message = "";
                    }
                }
                else
                {
                    var mapp = Mapper.Map<Adm_Master_College_StudentDMO>(data);
                    mapp.AMCST_Divyangjan = data.AMCST_Divyangjan;
                    mapp.AMCST_ActiveFlag = true;
                    mapp.AMCST_SOL = "S";
                    mapp.CreatedDate = DateTime.Now;
                    mapp.UpdatedDate = DateTime.Now;

                    //student mobile saving
                    string stdmobileno = "";
                    string stdmobilenosms = "";
                    string stdemailsms = "";
                    var count = data.Adm_College_Student_SMSNoDTO.Length;
                    for (int i = 0; i < count; i++)
                    {
                        stdmobilenosms = data.Adm_College_Student_SMSNoDTO[0].AMCST_MobileNo;
                        if (i == 0)
                        {
                            stdmobileno = data.Adm_College_Student_SMSNoDTO[i].AMCST_MobileNo;
                        }
                        else
                        {
                            // stdmobileno = stdmobileno + ',' + mas.Adm_M_Student_MobileNoDTO[i].AMST_MobileNo;
                        }
                    }

                    //student email id saving
                    string stdemailids = "";
                    for (int j = 0; j < data.Adm_College_Student_EmailIdDTO.Length; j++)
                    {
                        stdemailsms = data.Adm_College_Student_EmailIdDTO[0].AMCST_emailId;
                        if (j == 0)
                        {
                            stdemailids = data.Adm_College_Student_EmailIdDTO[j].AMCST_emailId;
                        }
                        else
                        {
                            //  stdemailids = stdemailids + ',' + mas.Adm_M_Student_EmailIdDTO[j].AMCST_emailId;
                        }
                    }
                    mapp.AMCST_MobileNo = Convert.ToInt64(stdmobilenosms);
                    mapp.AMCST_emailId = stdemailids;


                    if (data.transnumconfigsettings.IMN_AutoManualFlag == "Manual")
                    {
                        mapp.AMCST_RegistrationNo = data.AMCST_RegistrationNo;
                    }
                    else
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);

                        data.transnumconfigsettings.MI_Id = data.MI_Id;
                        data.transnumconfigsettings.ASMAY_Id = data.ASMAY_Id;
                        mapp.AMCST_RegistrationNo = a.GenerateNumber(data.transnumconfigsettings);
                    }
                    if (data.admissionNumbering.IMN_AutoManualFlag == "Manual")
                    {
                        // Manual Adm No.
                        mapp.AMCST_AdmNo = data.AMCST_AdmNo;
                    }
                    else
                    {
                        GenerateTransactionNumbering b = new GenerateTransactionNumbering(_db);
                        data.admissionNumbering.MI_Id = data.MI_Id;
                        data.admissionNumbering.ASMAY_Id = data.ASMAY_Id;
                        mapp.AMCST_AdmNo = b.GenerateNumber(data.admissionNumbering);
                    }

                    // string date = data.AMCST_DOB.Value.Date.ToString("yyyyMMdd");

                    string dateday = data.AMCST_DOB.ToString("dd");
                    string datemonth = data.AMCST_DOB.ToString("MM");
                    string dateyear = data.AMCST_DOB.ToString("yyyy");

                    dateyear = dateyear.Substring(2, 2);
                    string date = "";

                    date = dateyear + datemonth + dateyear + "001";

                    long tpin = 0;
                    int i1 = 0;
                    var checktpinexists = _context.Adm_Master_College_StudentDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCST_TPINNO.Equals(date)).ToList();
                    while (checktpinexists.Count > 0)
                    {
                        if (i1 == 0)
                        {
                            tpin = Convert.ToInt64(date) + 1;
                            i1 = i1 + 1;
                        }
                        else
                        {
                            tpin = Convert.ToInt64(tpin) + 1;
                        }

                        string date1 = Convert.ToString(tpin);
                        checktpinexists = _context.Adm_Master_College_StudentDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCST_TPINNO.Equals(date1.ToString())).ToList();
                    }
                    if (tpin == 0)
                    {
                        mapp.AMCST_TPINNO = Convert.ToString(date);
                    }
                    else
                    {
                        mapp.AMCST_TPINNO = Convert.ToString(tpin);
                    }
                    _context.Add(mapp);
                    data.AMCST_Id = mapp.AMCST_Id;
                    student_mobile_no(data);
                    student_email_id(data);

                    //  if (data.AMST_ECSFlag == 1)
                    //  {
                    //      saveupdate_ecsdetails(data);
                    //  }

                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        data.AMCST_Id = mapp.AMCST_Id;
                        //data.returnval = true;
                        data.Message = "Add";
                        //data.retuen_student_DTO.SetValue("Add",0);
                        if (data.ACMS_Id > 0)
                        {
                            int MaxCapacity = _context.Adm_College_Master_SectionDMO.SingleOrDefault(s => s.ACMS_Id == data.ACMS_Id).ACMS_MaxCapacity;

                            if (MaxCapacity == 0)
                            {
                                data.Messagesection = "Zero Capacity";

                            }
                            var countff = (from m in _context.Adm_Master_College_StudentDMO
                                           from nd in _context.Adm_College_Yearly_StudentDMO
                                           where (m.AMCST_Id == nd.AMCST_Id && m.MI_Id == data.MI_Id)
                                           select new ClgYearWiseStudentDTO
                                           {
                                               AMCST_Id = nd.AMCST_Id
                                           }).ToList();
                            //add Student Section details

                            try
                            {
                                var createdcount = _context.Adm_College_Yearly_StudentDMO.Where(t => t.ACYST_ActiveFlag.Equals(true) && t.AMB_Id == data.AMB_Id
                                && t.ACMS_Id == data.ACMS_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCO_Id == data.AMCO_Id && t.AMSE_Id == data.AMSE_Id).ToList();

                                if (createdcount.Count < MaxCapacity)
                                {
                                    if (countff.Count > 0)
                                    {
                                        var result = (from m in _context.Adm_Master_College_StudentDMO
                                                      from ndd in _context.Adm_College_Yearly_StudentDMO
                                                      where (m.AMCST_Id == ndd.AMCST_Id && m.MI_Id == data.MI_Id && ndd.ASMAY_Id == data.ASMAY_Id
                                                      && ndd.ACMS_Id == data.ACMS_Id && ndd.AMB_Id == data.AMB_Id && ndd.AMCO_Id == data.AMCO_Id
                                                      && ndd.AMSE_Id == data.AMSE_Id)
                                                      select new ClgYearWiseStudentDTO
                                                      {
                                                          AMCST_Id = ndd.AMCST_Id,
                                                          ACYST_RollNo = ndd.ACYST_RollNo
                                                      }).ToList();
                                        if (result.Count > 0)
                                        {
                                            var rollNo = result.OrderByDescending(e => e.ACYST_RollNo).First().ACYST_RollNo;
                                            data.ACYST_RollNo = rollNo + 1;
                                        }
                                        else
                                        {
                                            data.ACYST_RollNo = 1;
                                        }

                                    }
                                    else
                                    {
                                        data.ACYST_RollNo = 1;
                                    }

                                    Adm_College_Yearly_StudentDMO sct = new Adm_College_Yearly_StudentDMO();
                                    sct.ASMAY_Id = data.ASMAY_Id;
                                    sct.AMB_Id = data.AMB_Id;
                                    sct.AMSE_Id = data.AMSE_Id;
                                    sct.AMCO_Id = Convert.ToInt64(data.AMCO_Id);
                                    if (data.ACMS_Id != 0)
                                    {
                                        sct.ACMS_Id = data.ACMS_Id;
                                    }
                                    else
                                    {

                                        long sec_id = _context.Adm_College_Master_SectionDMO.SingleOrDefault(s => s.ACMS_SectionName == "A" && s.MI_Id == data.MI_Id).ACMS_Id;
                                        if (sec_id > 0)
                                        {
                                            sct.ACMS_Id = sec_id;
                                        }

                                    }

                                    sct.AMCST_Id = data.AMCST_Id;
                                    sct.ACYST_RollNo = data.ACYST_RollNo;
                                    sct.ACYST_ActiveFlag = 1;
                                    sct.LoginId = Convert.ToInt64(data.LoginId);
                                    sct.ACYST_DateTime = DateTime.Now;
                                    sct.CreatedDate = DateTime.Now;
                                    sct.UpdatedDate = DateTime.Now;
                                    _context.Add(sct);

                                    var ii = _context.SaveChanges();
                                    if (ii > 0)
                                    {
                                        data.Message = "Add";
                                        data.Messagesection = "Success";
                                    }
                                    else
                                    {
                                        data.Message = "Add";
                                    }
                                }
                                else
                                {
                                    data.Messagesection = "Maximum limit for this section is exceeded.";
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                data.Messagesection = "Failed";
                            }
                        }

                        //Auto Fee Group Mapping - No configuration
                        try
                        {
                           // var confirmstatus = _context.Database.ExecuteSqlCommand("Batchwise_Student_feeGroup_Mapping_New_Admission @p0,@p1,@p2,@p3",
                           //data.MI_Id, data.AMCST_Id, data.ASMAY_Id, data.AMSE_Id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        //Auto Fee Group Mapping - No configuration

                        var admConfig = _context.AdmissionStandardDMO.Single(t => t.MI_Id == data.MI_Id);
                        var studDet = _context.Adm_Master_College_StudentDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCST_Id == data.AMCST_Id).ToList();

                        if (admConfig.ASC_DefaultSMS_Flag == "M")
                        {
                            SMS sms = new SMS(_db);
                            if (data.Edit_flag == false)
                            {
                                string s = sms.sendSms(data.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().AMCST_MotherMobleNo), "ADMISSION_REGISTRATION_COLLEGE", data.AMCST_Id).Result;
                            }
                            else
                            {
                                string s = sms.sendSms(data.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().AMCST_MotherMobleNo), "ADMISSION_REGISTRATION_UPDATE_COLLEGE", data.AMCST_Id).Result;
                            }

                            Email Email = new Email(_db);
                            if (data.Edit_flag == false)
                            {
                                string m = Email.sendmail(data.MI_Id, studDet.FirstOrDefault().AMCST_MotheremailId, "ADMISSION_REGISTRATION_COLLEGE", data.AMCST_Id);
                            }
                            else
                            {
                                string m = Email.sendmail(data.MI_Id, studDet.FirstOrDefault().AMCST_MotheremailId, "ADMISSION_REGISTRATION_UPDATE_COLLEGE", data.AMCST_Id);
                            }

                        }
                        else if (admConfig.ASC_DefaultSMS_Flag == "F")
                        {
                            SMS sms = new SMS(_db);
                            if (data.Edit_flag == false)
                            {
                                string s = sms.sendSms(data.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().AMCST_FatherMobleNo), "ADMISSION_REGISTRATION_COLLEGE", data.AMCST_Id).Result;
                            }
                            else
                            {
                                string s = sms.sendSms(data.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().AMCST_FatherMobleNo), "ADMISSION_REGISTRATION_UPDATE_COLLEGE", data.AMCST_Id).Result;
                            }

                            Email Email = new Email(_db);
                            if (data.Edit_flag == false)
                            {
                                string m = Email.sendmail(data.MI_Id, studDet.FirstOrDefault().AMCST_FatheremailId, "ADMISSION_REGISTRATION_COLLEGE", data.AMCST_Id);
                            }
                            else
                            {
                                string m = Email.sendmail(data.MI_Id, studDet.FirstOrDefault().AMCST_FatheremailId, "ADMISSION_REGISTRATION_UPDATE_COLLEGE", data.AMCST_Id);
                            }
                        }
                        else
                        {
                            SMS sms = new SMS(_db);
                            if (data.Edit_flag == false)
                            {
                                string s = sms.sendSms(data.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().AMCST_MobileNo), "ADMISSION_REGISTRATION_COLLEGE", data.AMCST_Id).Result;
                            }
                            else
                            {
                                string s = sms.sendSms(data.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().AMCST_MobileNo), "ADMISSION_REGISTRATION_UPDATE_COLLEGE", data.AMCST_Id).Result;
                            }

                            Email Email = new Email(_db);
                            if (data.Edit_flag == false)
                            {
                                string m = Email.sendmail(data.MI_Id, studDet.FirstOrDefault().AMCST_emailId, "ADMISSION_REGISTRATION_COLLEGE", data.AMCST_Id);
                            }
                            else
                            {
                                string m = Email.sendmail(data.MI_Id, studDet.FirstOrDefault().AMCST_emailId, "ADMISSION_REGISTRATION_UPDATE_COLLEGE", data.AMCST_Id);
                            }
                        }
                    }
                    else
                    {
                        data.Message = "";
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
        public save_firsttab_details student_mobile_no(save_firsttab_details datastdmobile)
        {
            try
            {
                if (datastdmobile.Adm_College_Student_SMSNoDTO.Count() > 0)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();
                    //getting all mobilenumbers
                    foreach (Adm_College_Student_SMSNoDTO ph in datastdmobile.Adm_College_Student_SMSNoDTO)
                    {
                        temparr.Add(ph.ACSTSMS_Id);
                    }

                    //removing mobile number 
                    Array Phone_Noresultremove = _context.AdmCollegeStudentSMSNoDMO.Where(t => !temparr.Contains(t.ACSTSMS_Id) && t.AMCST_Id == datastdmobile.AMCST_Id).ToArray();
                    foreach (AdmCollegeStudentSMSNoDMO ph1 in Phone_Noresultremove)
                    {
                        _context.Remove(ph1);
                    }

                    //updating and saving 

                    foreach (Adm_College_Student_SMSNoDTO ph in datastdmobile.Adm_College_Student_SMSNoDTO)
                    {
                        ph.AMCST_Id = datastdmobile.AMCST_Id;
                        AdmCollegeStudentSMSNoDMO phone = Mapper.Map<AdmCollegeStudentSMSNoDMO>(ph);
                        if (phone.ACSTSMS_Id > 0)
                        {
                            var Phone_Noresult = _context.AdmCollegeStudentSMSNoDMO.Single(t => t.ACSTSMS_Id == ph.ACSTSMS_Id);
                            Phone_Noresult.UpdatedDate = DateTime.Now;
                            Phone_Noresult.CreatedDate = Phone_Noresult.CreatedDate;
                            Phone_Noresult.ACSTSMS_MobileNo = Convert.ToInt64(ph.AMCST_MobileNo);
                            Phone_Noresult.ACSTSMS_CountryCode = ph.ACSTSMS_CountryCode;
                            Mapper.Map(ph, Phone_Noresult);
                            _context.Update(Phone_Noresult);
                        }
                        else
                        {
                            phone.CreatedDate = DateTime.Now;
                            phone.UpdatedDate = DateTime.Now;
                            phone.ACSTSMS_MobileNo = Convert.ToInt64(ph.AMCST_MobileNo);
                            phone.ACSTSMS_CountryCode = ph.ACSTSMS_CountryCode;
                            _context.Add(phone);
                        }
                        //  _AdmissionFormContext.SaveChanges();
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
        public save_firsttab_details student_email_id(save_firsttab_details datastdemail)
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
                Console.WriteLine(e.Message);
                _context.Database.RollbackTransaction();
            }

            return datastdemail;
        }

        //Edit Student Data.
        public AdmMasterCollegeStudentDTO Edit(AdmMasterCollegeStudentDTO Edata)
        {
            try
            {
                //List<MasterStudentBondDMO> MasterStudentBondDMO = new List<MasterStudentBondDMO>();
                //MasterStudentBondDMO = _AdmissionFormContext.MasterStudentBondDMO.Where(t => t.AMST_Id == AdmMasterCollegeStudentDTO.AMST_Id).ToList().ToList();
                //AdmMasterCollegeStudentDTO.BondList = MasterStudentBondDMO.ToArray();


                var StudentPrevSchoolDMO = _context.AdmCollegeStudentPrevSchoolDMO.Where(t => t.AMCST_Id == Edata.AMCST_Id).ToList();
                Edata.PrevSchoolDetails = StudentPrevSchoolDMO.ToArray();

                var StudentPrevexamSchoolDMO = _context.Adm_College_Student_SubjectMarksDMO.Where(t => t.AMCST_Id == Edata.AMCST_Id).ToList();
                Edata.Adm_College_Student_SubjectMarksDTO = StudentPrevexamSchoolDMO.ToArray();


                var StudentGuardianDMO = _context.AdmCollegeStudentGuardianDMO.Where(t => t.AMCST_Id == Edata.AMCST_Id).ToList();
                Edata.StudentGuardianDetails = StudentGuardianDMO.ToArray();


                var StudentSiblingDMO = _context.AdmCollegeStudentSiblingsDetailsDMO.Where(t => t.AMCST_Id == Edata.AMCST_Id).ToList();
                Edata.StudentSiblingDetails = StudentSiblingDMO.ToArray();


                var adm_m_student = _context.Adm_Master_College_StudentDMO.Where(t => t.AMCST_Id == Edata.AMCST_Id).ToList();
                Edata.StudentList = adm_m_student.ToArray();

                Edata.AllCaste = (from m in _context.CasteCategory
                                  from n in _context.Caste
                                  where m.IMCC_Id == n.IMCC_Id && n.IMCC_Id == adm_m_student.FirstOrDefault().IMCC_Id && n.MI_Id == adm_m_student.FirstOrDefault().MI_Id
                                  select new AdmMasterCollegeStudentDTO
                                  {
                                      IMC_Id = n.IMC_Id,
                                      IMC_CasteName = n.IMC_CasteName
                                  }).ToArray();




                List<CollegeMasterDocumentDTO> result = new List<CollegeMasterDocumentDTO>();
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG.Adm_Student_Documents";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = adm_m_student.FirstOrDefault().MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                       SqlDbType.BigInt)
                    {
                        Value = Edata.AMCST_Id
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
                                result.Add(new CollegeMasterDocumentDTO
                                {
                                    AMSMD_DocumentName = dataReader["AMSMD_DocumentName"].ToString(),
                                    AMSMD_Id = Convert.ToInt64(dataReader["AMSMD_Id"]),
                                    AMSMD_FLAG = Convert.ToBoolean(dataReader["AMSMD_FLAG"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                var StudDocumentList = (from sp in _context.AdmCollegeStudentDocumentsDMO
                                        from cp in _context.MasterDocumentDMO
                                        where (sp.ACSMD_Id == cp.AMSMD_Id && sp.AMCST_Id == Edata.AMCST_Id)
                                        select new CollegeMasterDocumentDTO
                                        {
                                            AMSMD_DocumentName = cp.AMSMD_DocumentName,
                                            AMSMD_Id = cp.AMSMD_Id,
                                            ACSTD_Id = sp.ACSTD_Id,
                                            AMCST_Id = sp.AMCST_Id,
                                            Document_Path = sp.ACSTD_Doc_Path,
                                            AMSMD_FLAG = cp.AMSMD_FLAG
                                        }).ToList();

                for (int i = 0; i < result.Count; i++)
                {
                    StudDocumentList.Add(result[i]);
                }
                Edata.DocumentList = StudDocumentList.ToArray();


                //List<StudentAchivementDMO> StudentAchivementDMO = new List<StudentAchivementDMO>();
                //StudentAchivementDMO = _AdmissionFormContext.StudentAchivementDMO.Where(t => t.AMST_Id.Equals(AdmMasterCollegeStudentDTO.AMST_Id)).ToList();
                //AdmMasterCollegeStudentDTO.StudentAchivementDetails = StudentAchivementDMO.ToArray();


                var StudentReferenceDMO = _context.AdmCollegeStudentReferenceDMO.Where(t => t.AMCST_Id == Edata.AMCST_Id).ToList();
                Edata.StudentReferenceDetails = StudentReferenceDMO.ToArray();


                var StudentSourceDMO = _context.AdmCollegeStudentSourceDMO.Where(t => t.AMCST_Id == Edata.AMCST_Id).ToList();
                Edata.StudentSourceDetails = StudentSourceDMO.ToArray();

                //List<StudentActitvityDMO> StudentActitvityDMO = new List<StudentActitvityDMO>();
                //StudentActitvityDMO = _AdmissionFormContext.StudentActitvityDMO.Where(t => t.AMST_Id.Equals(AdmMasterCollegeStudentDTO.AMST_Id)).ToList();
                //AdmMasterCollegeStudentDTO.StudentActivityDetails = StudentActitvityDMO.ToArray();

                var asmccid = _context.Adm_Master_College_StudentDMO.Where(t => t.AMCST_Id == Edata.AMCST_Id).ToList();
                Edata.studentCategory = (from m in _context.ClgMasterCourseCategorycategoryMap
                                         from n in _context.mastercategory
                                         where m.AMCOC_Id == n.AMCOC_Id && m.AMCOC_Id == asmccid.FirstOrDefault().AMCOC_Id
                                         select new AdmMasterCollegeStudentDTO
                                         {
                                             AMCOC_Id = m.AMCOC_Id,
                                             AMCOC_Name = n.AMCOC_Name
                                         }).ToArray();


                Edata.currentyearstudentdetails = (from a in _context.Adm_College_Yearly_StudentDMO
                                                   from b in _context.Adm_Master_College_StudentDMO
                                                   from c in _context.MasterCourseDMO
                                                   from d in _context.ClgMasterBranchDMO
                                                   from e in _context.CLG_Adm_Master_SemesterDMO
                                                   from f in _context.Adm_College_Master_SectionDMO
                                                   where a.AMCST_Id == b.AMCST_Id && a.AMCO_Id == c.AMCO_Id && a.AMB_Id == d.AMB_Id
                                                   && a.AMSE_Id == e.AMSE_Id && a.ACMS_Id == f.ACMS_Id && b.MI_Id == Edata.MI_Id && a.ASMAY_Id == Edata.ASMAY_Id
                                                   && a.AMCST_Id == Edata.AMCST_Id && a.ACYST_ActiveFlag == 1 && b.AMCST_SOL == "S" && b.AMCST_ActiveFlag == true
                                                   select new AdmMasterCollegeStudentDTO
                                                   {
                                                       courseName = c.AMCO_CourseName,
                                                       branchName = d.AMB_BranchName,
                                                       semesterName = e.AMSE_SEMName,
                                                       sectionname = f.ACMS_SectionName,
                                                       ACMS_Id = a.ACMS_Id
                                                   }).Distinct().ToArray();



                Edata.FatherMultipleMobileNoDTO = (from a in _context.Adm_Master_College_StudentDMO
                                                   from b in _context.AdmCollegeStudentParentsMobileNoDMO
                                                   where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == Edata.MI_Id && a.AMCST_Id == Edata.AMCST_Id && b.ACSTPMN_Flag.Equals("F"))
                                                   select new FatherMultipleMobileNoDTO
                                                   {
                                                       ACSTPMN_Id = b.ACSTPMN_Id,
                                                       AMCST_FatherMobleNo = b.ACSTPMN_MobileNo,
                                                       ACSTPMN_CountryCode = b.ACSTPMN_CountryCode
                                                   }).ToArray();

                Edata.FatherMultipleEmailIdDTO = (from a in _context.Adm_Master_College_StudentDMO
                                                  from b in _context.AdmCollegeStudentParentsEmailIdDMO
                                                  where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == Edata.MI_Id && a.AMCST_Id == Edata.AMCST_Id && b.ACSTPE_Flag.Equals("F"))
                                                  select new FatherMultipleEmailIdDTO
                                                  {
                                                      ACSTPE_Id = b.ACSTPE_Id,
                                                      AMCST_FatheremailId = b.ACSTPE_EmailId
                                                  }).ToArray();


                Edata.MotherMultipleMobileNoDTO = (from a in _context.Adm_Master_College_StudentDMO
                                                   from b in _context.AdmCollegeStudentParentsMobileNoDMO
                                                   where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == Edata.MI_Id && a.AMCST_Id == Edata.AMCST_Id && b.ACSTPMN_Flag.Equals("M"))
                                                   select new MotherMultipleMobileNoDTO
                                                   {
                                                       ACSTPMN_Id = b.ACSTPMN_Id,
                                                       AMCST_MotherMobleNo = b.ACSTPMN_MobileNo,
                                                       ACSTPMN_CountryCode = b.ACSTPMN_CountryCode
                                                   }).ToArray();


                Edata.MotherMultipleEmailIdDTO = (from a in _context.Adm_Master_College_StudentDMO
                                                  from b in _context.AdmCollegeStudentParentsEmailIdDMO
                                                  where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == Edata.MI_Id && a.AMCST_Id == Edata.AMCST_Id && b.ACSTPE_Flag.Equals("M"))
                                                  select new MotherMultipleEmailIdDTO
                                                  {
                                                      ACSTPE_Id = b.ACSTPE_Id,
                                                      AMCST_MotheremailId = b.ACSTPE_EmailId
                                                  }).ToArray();

                Edata.Adm_College_Student_SMSNoDTO = (from a in _context.Adm_Master_College_StudentDMO
                                                      from b in _context.AdmCollegeStudentSMSNoDMO
                                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == Edata.MI_Id && a.AMCST_Id == Edata.AMCST_Id)
                                                      select new Adm_College_Student_SMSNoDTO
                                                      {
                                                          ACSTSMS_Id = b.ACSTSMS_Id,
                                                          AMCST_MobileNo = b.ACSTSMS_MobileNo.ToString(),
                                                          ACSTSMS_CountryCode = b.ACSTSMS_CountryCode
                                                      }).ToArray();

                Edata.Adm_College_Student_EmailIdDTO = (from a in _context.Adm_Master_College_StudentDMO
                                                        from b in _context.AdmCollegeStudentEmailIdDMO
                                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == Edata.MI_Id && a.AMCST_Id == Edata.AMCST_Id)
                                                        select new Adm_College_Student_EmailIdDTO
                                                        {
                                                            ACSTE_Id = b.ACSTE_Id,
                                                            AMCST_emailId = b.ACSTE_EmailId
                                                        }).ToArray();


                //AdmMasterCollegeStudentDTO.Adm_M_Student_ECSDTo = (from a in _db.Adm_M_Student
                //                                         from b in _db.Adm_Student_EcsDetailsDMO
                //                                         where (a.AMST_Id == b.AMST_Id && a.MI_Id == AdmMasterCollegeStudentDTO.MI_Id && a.AMST_Id == AdmMasterCollegeStudentDTO.AMST_Id)
                //                                         select new Adm_M_Student_ECSDTo
                //                                         {
                //                                             ASECS_Id = b.ASECS_Id,
                //                                             AMST_Id = b.AMST_Id,
                //                                             ASECS_AccountHolderName = b.ASECS_AccountHolderName,
                //                                             ASECS_AccountNo = b.ASECS_AccountNo,
                //                                             ASECS_AccountType = b.ASECS_AccountType,
                //                                             ASECS_BankName = b.ASECS_BankName,
                //                                             ASECS_Branch = b.ASECS_Branch,
                //                                             ASECS_MICRNo = b.ASECS_MICRNo
                //                                         }).ToArray();

                Edata.studentCompDetails = (from a in _context.Adm_College_Student_CEMarksDMO
                                            from c in _context.Adm_Master_College_StudentDMO
                                            from d in _context.Master_Competitive_AdmExamsClgDMO
                                            where (a.AMCEXM_Id == d.AMCEXM_Id && a.AMCST_Id == c.AMCST_Id && c.MI_Id == Edata.MI_Id && a.ACSTCEM_ActiveFlg == true && c.AMCST_Id == asmccid.FirstOrDefault().AMCST_Id)
                                            select new Adm_College_Student_CEMarksDTO
                                            {
                                                AMCEXM_Id = a.AMCEXM_Id,
                                                ACSTCEM_Id = a.ACSTCEM_Id,
                                                ACSTCEM_RollNo = a.ACSTCEM_RollNo,
                                                ACSTCEM_RegistrationId = a.ACSTCEM_RegistrationId,
                                                ACSTCEM_MeritNo = a.ACSTCEM_MeritNo,
                                                ACSTCEM_ALLIndiaRank = a.ACSTCEM_ALLIndiaRank,
                                                ACSTCEM_CategoryRank = a.ACSTCEM_CategoryRank,
                                                ACSTCEM_TotalMaxMarks = a.ACSTCEM_TotalMaxMarks,
                                                ACSTCEM_ObtdMarks = a.ACSTCEM_ObtdMarks,
                                                ACSTCEM_Percentile = a.ACSTCEM_Percentile,
                                                ACSTCEM_Percentage = a.ACSTCEM_Percentage,
                                                AMCEXM_CompetitiveExams = d.AMCEXM_CompetitiveExams

                                            }
                                           ).ToArray();

                Edata.studentCompSubDetails = (from a in _context.Adm_College_Student_CEMarks_SubjectDMO
                                               from c in _context.Adm_Master_College_StudentDMO
                                               from d in _context.Master_Competitive_AdmExamsClgDMO
                                               from e in _context.Master_CompetitiveExamsSubjectsAdmClgDMO
                                               where (e.AMCEXMSUB_Id == a.AMCEXMSUB_Id && a.AMCEXM_Id == d.AMCEXM_Id && a.AMCST_Id == c.AMCST_Id && c.MI_Id == Edata.MI_Id && a.ACSTCEMS_ActiveFlg == true && c.AMCST_Id == asmccid.FirstOrDefault().AMCST_Id)
                                               select new Adm_College_Student_CEMarks_SubjectDTO
                                               {
                                                   AMCEXM_Id = a.AMCEXM_Id,
                                                   ACSTCEMS_Id = a.ACSTCEMS_Id,
                                                   AMCEXMSUB_Id = a.AMCEXMSUB_Id,
                                                   AMCEXMSUB_MaxMarks = e.AMCEXMSUB_MaxMarks,
                                                   ACSTCEMS_MaxMarks = a.ACSTCEMS_MaxMarks,
                                                   ACSTCEMS_SubjectMarks = a.ACSTCEMS_SubjectMarks,
                                                   AMCEXM_CompetitiveExams = d.AMCEXM_CompetitiveExams,
                                                   AMCEXMSUB_SubjectName = e.AMCEXMSUB_SubjectName

                                               }).ToArray();
                Edata.compExamarray = _context.Master_Competitive_AdmExamsClgDMO.Where(c => c.MI_Id == Edata.MI_Id && c.AMCEXM_ActiveFlg == true).Distinct().OrderBy(d => d.AMCEXM_CompetitiveExams).ToArray();

                Edata.compSubarray = _context.Master_CompetitiveExamsSubjectsAdmClgDMO.Where(s => s.AMCEXMSUB_ActiveFlg == true).Distinct().OrderBy(m => m.AMCEXMSUB_SubjectName).ToArray();

                var Allcountrycode = _context.Country.Where(c => c.IVRMMC_CountryPhCode != null).ToList();
                if (Allcountrycode.Count > 0)
                {
                    Edata.AllCountrycode = Allcountrycode.ToArray();
                }




                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "clg_AdmStudentWiseAttendanceDone";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Edata.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Edata.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar) { Value = Edata.AMCST_Id });

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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        Edata.attendanceArray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Edata;
        }
        public AdmMasterCollegeStudentDTO checkDuplicate(AdmMasterCollegeStudentDTO dto)
        {
            try
            {

                if (dto.AMCST_Id > 0)
                {
                    var studRecord = _context.Adm_Master_College_StudentDMO.Where(t => t.AMCST_Id == dto.AMCST_Id).ToList();

                    var duplicateaadhar = _context.Adm_Master_College_StudentDMO.Where(t => t.AMCST_AadharNo == dto.AMCST_AadharNo && t.MI_Id == dto.MI_Id && t.AMCST_ActiveFlag == true && t.AMCST_SOL.Equals("S") && t.AMCST_Id != dto.AMCST_Id).ToList();
                    if (duplicateaadhar.Count > 0)
                    {
                        dto.duplicateAdharNo = duplicateaadhar.Count;
                    }
                    else
                    {
                        dto.duplicateAdharNo = 0;
                    }

                    //Check Duplicate Adm.No Or Reg.No.If set to prevent duplicate in Transaction Numbering.
                    if (dto.admRegManualFlag == "true" && dto.preventdupRegNo == "Yes")
                    {
                        dto.duplicateRegNo = _context.Adm_Master_College_StudentDMO.Where(d => d.MI_Id == dto.MI_Id && d.AMCST_ActiveFlag == true && d.AMCST_SOL.Equals("S") && d.AMCST_RegistrationNo.Equals(dto.AMCST_RegistrationNo) && d.AMCST_Id != dto.AMCST_Id).Count();
                    }
                    if (dto.admAdmManualFlag == "true" && dto.preventdupAdmNo == "Yes")
                    {
                        dto.duplicateAdmNo = _context.Adm_Master_College_StudentDMO.Where(d => d.MI_Id == dto.MI_Id && d.AMCST_ActiveFlag == true && d.AMCST_SOL.Equals("S") && d.AMCST_AdmNo.Equals(dto.AMCST_AdmNo) && d.AMCST_Id != dto.AMCST_Id).Count();
                    }
                }
                else
                {
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
        public AdmMasterCollegeStudentDTO getdpstate(AdmMasterCollegeStudentDTO d)
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
        public AdmMasterCollegeStudentDTO saveAddress(AdmMasterCollegeStudentDTO adds)
        {
            try
            {
                if (adds.AMCST_Id > 0)
                {
                    var result = _context.Adm_Master_College_StudentDMO.Single(a => a.MI_Id == adds.MI_Id && a.AMCST_Id == adds.AMCST_Id);
                    result.AMCST_PerStreet = adds.AMCST_PerStreet;
                    result.AMCST_PerArea = adds.AMCST_PerArea;
                    result.AMCST_PerCity = adds.AMCST_PerCity;
                    result.AMCST_PerState = adds.AMCST_PerState > 0 ? adds.AMCST_PerState : null;
                    result.IVRMMC_Id = adds.IVRMMC_Id > 0 ? adds.IVRMMC_Id : null; ;
                    result.AMCST_PerPincode = adds.AMCST_PerPincode;
                    result.AMCST_ConStreet = adds.AMCST_ConStreet;
                    result.AMCST_ConArea = adds.AMCST_ConArea;
                    result.AMCST_ConCity = adds.AMCST_ConCity;
                    result.AMCST_ConState = adds.AMCST_ConState > 0 ? adds.AMCST_ConState : null;
                    result.AMCST_ConCountryId = adds.AMCST_ConCountryId > 0 ? adds.AMCST_ConCountryId : null; ;
                    result.AMCST_ConPincode = adds.AMCST_ConPincode;
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        adds.Message = "Update";

                    }
                    else
                    {
                        adds.Message = "";
                    }
                }
                else
                {
                    var mapp = Mapper.Map<Adm_Master_College_StudentDMO>(adds);
                    mapp.AMCST_ActiveFlag = true;
                    mapp.AMCST_SOL = "S";
                    mapp.CreatedDate = DateTime.Now;
                    mapp.UpdatedDate = DateTime.Now;
                    _context.Add(mapp);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        adds.Message = "Add";

                    }
                    else
                    {
                        adds.Message = "";
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return adds;
        }
        public AdmMasterCollegeStudentDTO saveParentsDetails(AdmMasterCollegeStudentDTO ParentsData)
        {
            try
            {
                if (ParentsData.AMCST_Id > 0)
                {
                    var result = _context.Adm_Master_College_StudentDMO.Single(a => a.MI_Id == ParentsData.MI_Id && a.AMCST_Id == ParentsData.AMCST_Id);
                    result.AMCST_FatherAliveFlag = ParentsData.AMCST_FatherAliveFlag;
                    result.AMCST_FatherName = ParentsData.AMCST_FatherName;
                    result.AMCST_FatherSurname = ParentsData.AMCST_FatherSurname;
                    result.AMCST_FatherAadharNo = ParentsData.AMCST_FatherAadharNo;
                    result.AMCST_FatherEducation = ParentsData.AMCST_FatherEducation;
                    result.AMCST_FatherOfficeAdd = ParentsData.AMCST_FatherOfficeAdd;
                    result.AMCST_FatherOccupation = ParentsData.AMCST_FatherOccupation;
                    result.AMCST_FatherDesignation = ParentsData.AMCST_FatherDesignation;
                    result.AMCST_FatherBankAccNo = ParentsData.AMCST_FatherBankAccNo;
                    result.AMCST_FatherBankIFSCCode = ParentsData.AMCST_FatherBankIFSCCode;
                    result.AMCST_FatherCasteCertiNo = ParentsData.AMCST_FatherCasteCertiNo;

                    result.AMCST_FatherNationality = ParentsData.AMCST_FatherNationality;
                    result.AMCST_FatherReligion = ParentsData.AMCST_FatherReligion;
                    result.AMCST_FatherCaste = ParentsData.AMCST_FatherCaste;
                    result.AMCST_FatherSubCaste = ParentsData.AMCST_FatherSubCaste;
                    result.AMCST_FatherMonIncome = ParentsData.AMCST_FatherMonIncome;
                    result.AMCST_FatherAnnIncome = ParentsData.AMCST_FatherAnnIncome;
                    result.AMCST_FatherMobleNo = ParentsData.AMCST_FatherMobleNo;
                    result.AMCST_FatheremailId = ParentsData.AMCST_FatheremailId;

                    //Mother details
                    result.AMCST_MotherAliveFlag = ParentsData.AMCST_MotherAliveFlag;
                    result.AMCST_MotherName = ParentsData.AMCST_MotherName;
                    result.AMCST_MotherSurname = ParentsData.AMCST_MotherSurname;
                    result.AMCST_MotherAadharNo = ParentsData.AMCST_MotherAadharNo;
                    result.AMCST_MotherEducation = ParentsData.AMCST_MotherEducation;
                    result.AMCST_MotherOfficeAdd = ParentsData.AMCST_MotherOfficeAdd;
                    result.AMCST_MotherOccupation = ParentsData.AMCST_MotherOccupation;
                    result.AMCST_MotherDesignation = ParentsData.AMCST_MotherDesignation;
                    result.AMCST_MotherBankAccNo = ParentsData.AMCST_MotherBankAccNo;
                    result.AMCST_MotherBankIFSCCode = ParentsData.AMCST_MotherBankIFSCCode;
                    result.AMCST_MotherCasteCertiNo = ParentsData.AMCST_MotherCasteCertiNo;
                    result.AMCST_MotherReligion = ParentsData.AMCST_MotherReligion;
                    result.AMCST_MotherCaste = ParentsData.AMCST_MotherCaste;
                    result.AMCST_MotherSubCaste = ParentsData.AMCST_MotherSubCaste;
                    result.AMCST_MotherMonIncome = ParentsData.AMCST_MotherMonIncome;
                    result.AMCST_MotherAnnIncome = ParentsData.AMCST_MotherAnnIncome;
                    result.AMCST_MotherMobleNo = ParentsData.AMCST_MotherMobleNo;
                    result.AMCST_MotheremailId = ParentsData.AMCST_MotheremailId;
                    result.AMCST_MotherNationality = ParentsData.AMCST_MotherNationality;

                    //father mobile saving
                    string fathermobile = "";

                    for (int j = 0; j < ParentsData.FatherMultipleMobileNoDTO.Length; j++)
                    {
                        fathermobile = Convert.ToString(ParentsData.FatherMultipleMobileNoDTO[0].AMCST_FatherMobleNo);
                        if (j == 0)
                        {
                            if (fathermobile != "")
                            {
                                fathermobile = Convert.ToString(ParentsData.FatherMultipleMobileNoDTO[0].AMCST_FatherMobleNo);
                            }
                        }
                        else
                        {
                            //  fathermobile = fathermobile + ',' + Convert.ToString(mas.multiplemobileno[0].AMST_FatherMobleNo);
                        }
                    }

                    //father email id saving
                    string fatheremail = "";
                    for (int j = 0; j < ParentsData.FatherMultipleEmailIdDTO.Length; j++)
                    {
                        fatheremail = Convert.ToString(ParentsData.FatherMultipleEmailIdDTO[0].AMCST_FatheremailId);
                        if (j == 0)
                        {
                            fatheremail = Convert.ToString(ParentsData.FatherMultipleEmailIdDTO[0].AMCST_FatheremailId);
                        }
                        else
                        {
                            //fatheremail = fatheremail + ',' + Convert.ToString(mas.multipleemailid[0].AMST_FatheremailId);
                        }
                    }

                    //mother mobile saving
                    string mothermobile = "";
                    for (int j = 0; j < ParentsData.MotherMultipleMobileNoDTO.Length; j++)
                    {
                        mothermobile = Convert.ToString(ParentsData.MotherMultipleMobileNoDTO[0].AMCST_MotherMobleNo);
                        if (j == 0)
                        {
                            if (mothermobile != null)
                            {
                                mothermobile = Convert.ToString(ParentsData.MotherMultipleMobileNoDTO[0].AMCST_MotherMobleNo);
                            }

                        }
                        else
                        {
                            // mothermobile = mothermobile + ',' + Convert.ToString(mas.multiplemobilenomother[0].AMST_MotherMobileNo);
                        }
                    }

                    //mother email id saving
                    string motheremail = "";
                    for (int j = 0; j < ParentsData.MotherMultipleEmailIdDTO.Length; j++)
                    {
                        motheremail = Convert.ToString(ParentsData.MotherMultipleEmailIdDTO[0].AMCST_MotheremailId);
                        if (j == 0)
                        {
                            motheremail = Convert.ToString(ParentsData.MotherMultipleEmailIdDTO[0].AMCST_MotheremailId);
                        }
                        else
                        {

                        }
                    }
                    if (fathermobile != "")
                    {
                        result.AMCST_FatherMobleNo = Convert.ToInt64(fathermobile);
                    }
                    result.AMCST_FatheremailId = fatheremail;

                    if (mothermobile != "")
                    {
                        result.AMCST_MotherMobleNo = Convert.ToInt64(mothermobile);
                    }

                    result.AMCST_MotheremailId = motheremail;

                    _context.Update(result);

                    father_mobile_no(ParentsData);
                    father_email_ids(ParentsData);
                    mother_mobile_no(ParentsData);
                    mother_email_id(ParentsData);


                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        ParentsData.Message = "Update";

                    }
                    else
                    {
                        ParentsData.Message = "Update";

                    }
                }
                else
                {
                    var mapp = Mapper.Map<Adm_Master_College_StudentDMO>(ParentsData);
                    mapp.CreatedDate = DateTime.Now;
                    mapp.UpdatedDate = DateTime.Now;
                    mapp.AMCST_SOL = "S";
                    mapp.AMCST_ActiveFlag = true;

                    string fathermobile = "";

                    for (int j = 0; j < ParentsData.FatherMultipleMobileNoDTO.Length; j++)
                    {
                        fathermobile = Convert.ToString(ParentsData.FatherMultipleMobileNoDTO[0].AMCST_FatherMobleNo);
                        if (j == 0)
                        {
                            if (fathermobile != "")
                            {
                                fathermobile = Convert.ToString(ParentsData.FatherMultipleMobileNoDTO[0].AMCST_FatherMobleNo);
                            }
                        }
                        else
                        {
                            //  fathermobile = fathermobile + ',' + Convert.ToString(mas.multiplemobileno[0].AMST_FatherMobleNo);
                        }
                    }

                    //father email id saving
                    string fatheremail = "";
                    for (int j = 0; j < ParentsData.FatherMultipleEmailIdDTO.Length; j++)
                    {
                        fatheremail = Convert.ToString(ParentsData.FatherMultipleEmailIdDTO[0].AMCST_FatheremailId);
                        if (j == 0)
                        {
                            fatheremail = Convert.ToString(ParentsData.FatherMultipleEmailIdDTO[0].AMCST_FatheremailId);
                        }
                        else
                        {
                            //fatheremail = fatheremail + ',' + Convert.ToString(mas.multipleemailid[0].AMST_FatheremailId);
                        }
                    }

                    //mother mobile saving
                    string mothermobile = "";
                    for (int j = 0; j < ParentsData.MotherMultipleMobileNoDTO.Length; j++)
                    {
                        mothermobile = Convert.ToString(ParentsData.MotherMultipleMobileNoDTO[0].AMCST_MotherMobleNo);
                        if (j == 0)
                        {
                            if (mothermobile != null)
                            {
                                mothermobile = Convert.ToString(ParentsData.MotherMultipleMobileNoDTO[0].AMCST_MotherMobleNo);
                            }

                        }
                        else
                        {
                            // mothermobile = mothermobile + ',' + Convert.ToString(mas.multiplemobilenomother[0].AMST_MotherMobileNo);
                        }
                    }

                    //mother email id saving
                    string motheremail = "";
                    for (int j = 0; j < ParentsData.MotherMultipleEmailIdDTO.Length; j++)
                    {
                        motheremail = Convert.ToString(ParentsData.MotherMultipleEmailIdDTO[0].AMCST_MotheremailId);
                        if (j == 0)
                        {
                            motheremail = Convert.ToString(ParentsData.MotherMultipleEmailIdDTO[0].AMCST_MotheremailId);
                        }
                        else
                        {

                        }
                    }
                    if (fathermobile != "")
                    {
                        mapp.AMCST_FatherMobleNo = Convert.ToInt64(fathermobile);
                    }
                    mapp.AMCST_FatheremailId = fatheremail;

                    if (mothermobile != "")
                    {
                        mapp.AMCST_MotherMobleNo = Convert.ToInt64(mothermobile);
                    }

                    mapp.AMCST_MotheremailId = fatheremail;

                    father_mobile_no(ParentsData);
                    father_email_ids(ParentsData);
                    mother_mobile_no(ParentsData);
                    mother_email_id(ParentsData);


                    _context.Add(mapp);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        ParentsData.Message = "Add";

                    }
                    else
                    {
                        ParentsData.Message = "Add";

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
                            Phone_Noresult.ACSTPMN_CountryCode = ph.ACSTPMN_CountryCode;
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
                                phone1.ACSTPMN_CountryCode = ph.ACSTPMN_CountryCode;
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
                Console.WriteLine(e.Message);
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
                Console.WriteLine(e.Message);
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
                            Phone_Noresult.ACSTPMN_CountryCode = ph.ACSTPMN_CountryCode;
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
                                phone1.ACSTPMN_CountryCode = ph.ACSTPMN_CountryCode;
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
                Console.WriteLine(e.Message);
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
                Console.WriteLine(e.Message);
                _context.Database.RollbackTransaction();
            }
            return dataemailidmother;
        }
        public AdmMasterCollegeStudentDTO StateByCountryName(AdmMasterCollegeStudentDTO ct)
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
        public AdmMasterCollegeStudentDTO saveOthersDetails(AdmMasterCollegeStudentDTO others)
        {
            try
            {
                if (others.AMCST_Id > 0)
                {
                    var result = _context.Adm_Master_College_StudentDMO.Single(a => a.MI_Id == others.MI_Id && a.AMCST_Id == others.AMCST_Id);


                    if (others.AMCST_PassportNo != "")
                    {
                        result.AMCST_PassportNo = others.AMCST_PassportNo;
                        result.AMCST_PassportIssuedAt = others.AMCST_PassportIssuedAt;
                        result.AMCST_PassportIssueDate = others.AMCST_PassportIssueDate;
                        result.AMCST_PassportIssuedCounrty = others.AMCST_PassportIssuedCounrty;
                        result.AMCST_PassportIssuedPlace = others.AMCST_PassportIssuedPlace;
                        result.AMCST_PassportExpiryDate = others.AMCST_PassportExpiryDate;
                    }
                    if (others.AMCST_VisaIssuedBy != "")
                    {
                        result.AMCST_VisaIssuedBy = others.AMCST_VisaIssuedBy;
                        result.AMCST_VISAValidFrom = others.AMCST_VISAValidFrom;
                        result.AMCST_VISAValidTo = others.AMCST_VISAValidTo;
                    }


                    //  ActivityAddUppdate(others);
                    RefferenceDetailsAddUpdate(others);
                    SourceDetailsAddUpdate(others);
                    previousexamsubjectwise(others);
                    //   BondAddUpdate(others);
                    SiblingsAddUpdate(others);
                    //   AchivementAddUpdate(others);
                    PrevSchooldetailsAddUpdate(others);
                    GuardianAddUpdate(others);

                    //
                    stuCompexamdetailse1(others);
                    stuCompexamSubMarks1(others);

                    _context.Update(result);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        others.Message = "Update";
                    }
                    else
                    {
                        others.Message = "";
                    }
                }
                else
                {
                    Adm_Master_College_StudentDMO result = new Adm_Master_College_StudentDMO();
                    result.MI_Id = others.MI_Id;
                    // ActivityAddUppdate(others);
                    RefferenceDetailsAddUpdate(others);
                    SourceDetailsAddUpdate(others);
                    previousexamsubjectwise(others);
                    //  BondAddUpdate(others);
                    SiblingsAddUpdate(others);
                    //  AchivementAddUpdate(others);
                    PrevSchooldetailsAddUpdate(others);
                    GuardianAddUpdate(others);

                    stuCompexamdetailse1(others);
                    stuCompexamSubMarks1(others);

                    result.CreatedDate = DateTime.Now;
                    result.UpdatedDate = DateTime.Now;
                    _context.Add(result);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        others.Message = "Add";
                    }
                    else
                    {
                        others.Message = "";
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return others;
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
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            // _log.LogInformation("Student Reference  error");
                            // _log.LogDebug(ex.Message);
                            _context.Database.RollbackTransaction();
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
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
                if (sib.SelectedSiblingDetails != null)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();

                    foreach (AdmCollegeStudentSiblingsDetailsDTO ph in sib.SelectedSiblingDetails)
                    {
                        temparr.Add(ph.ACSTS_Id);
                    }

                    Array siblings_Noresultremove = _context.AdmCollegeStudentSiblingsDetailsDMO.Where(t => !temparr.Contains(t.ACSTS_Id)
                    && t.AMCST_Id == sib.AMCST_Id).ToArray();

                    foreach (AdmCollegeStudentSiblingsDetailsDMO ph1 in siblings_Noresultremove)
                    {
                        _context.Remove(ph1);
                    }

                    foreach (AdmCollegeStudentSiblingsDetailsDTO mob in sib.SelectedSiblingDetails)
                    {
                        if (mob.ACSTS_SiblingsName != null && mob.ACSTS_SiblingsName != "")
                        {
                            mob.AMCST_Id = sib.AMCST_Id;
                            AdmCollegeStudentSiblingsDetailsDMO sibling = Mapper.Map<AdmCollegeStudentSiblingsDetailsDMO>(mob);
                            if (sibling.ACSTS_Id > 0)
                            {
                                var siblingNoresult = _context.AdmCollegeStudentSiblingsDetailsDMO.Single(t => t.ACSTS_Id == mob.ACSTS_Id);
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
                Console.WriteLine(e.Message);
                _context.Database.RollbackTransaction();
            }
            return sib;
        }

        //save and update Prev School details
        public AdmMasterCollegeStudentDTO PrevSchooldetailsAddUpdate(AdmMasterCollegeStudentDTO prevadd)
        {
            try
            {
                // save Prev School details
                if (prevadd.SelectedPrevSchoolDetails != null)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();

                    foreach (AdmCollegeStudentPrevSchoolDTO ph in prevadd.SelectedPrevSchoolDetails)
                    {
                        temparr.Add(ph.ACSTPS_Id);
                    }

                    Array previous_Noresultremove = _context.AdmCollegeStudentPrevSchoolDMO.Where(t => !temparr.Contains(t.ACSTPS_Id)
                    && t.AMCST_Id == prevadd.AMCST_Id).ToArray();

                    foreach (AdmCollegeStudentPrevSchoolDMO ph1 in previous_Noresultremove)
                    {
                        _context.Remove(ph1);
                    }

                    foreach (AdmCollegeStudentPrevSchoolDTO mob in prevadd.SelectedPrevSchoolDetails)
                    {
                        if (mob.ACSTPS_PrvSchoolName != null && mob.ACSTPS_PrvSchoolName != "")
                        {
                            mob.AMCST_Id = prevadd.AMCST_Id;
                            AdmCollegeStudentPrevSchoolDMO PrevSchool = Mapper.Map<AdmCollegeStudentPrevSchoolDMO>(mob);
                            if (PrevSchool.ACSTPS_Id > 0)
                            {
                                var PrevSchoolresult = _context.AdmCollegeStudentPrevSchoolDMO.Single(t => t.ACSTPS_Id == mob.ACSTPS_Id);
                                PrevSchoolresult.UpdatedDate = DateTime.Now;
                                PrevSchoolresult.CreatedDate = PrevSchoolresult.CreatedDate;
                                Mapper.Map(mob, PrevSchoolresult);
                                _context.Update(PrevSchoolresult);
                            }
                            else
                            {  //added by 02/02/2017
                                PrevSchool.CreatedDate = DateTime.Now;
                                PrevSchool.UpdatedDate = DateTime.Now;
                                _context.Add(PrevSchool);
                            }
                            //  _AdmissionFormContext.SaveChanges();
                        }

                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _context.Database.RollbackTransaction();
            }
            return prevadd;
        }

        //save and update gaurdian details
        public AdmMasterCollegeStudentDTO GuardianAddUpdate(AdmMasterCollegeStudentDTO guardadd)
        {
            try
            {
                if (guardadd.SelectedGuardianDetails != null)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();

                    foreach (AdmCollegeStudentGuardianDTO ph in guardadd.SelectedGuardianDetails)
                    {
                        temparr.Add(ph.ACSTG_Id);
                    }

                    Array guardian_Noresultremove = _context.AdmCollegeStudentGuardianDMO.Where(t => !temparr.Contains(t.ACSTG_Id)
                    && t.AMCST_Id == guardadd.AMCST_Id).ToArray();

                    foreach (AdmCollegeStudentGuardianDMO ph1 in guardian_Noresultremove)
                    {
                        _context.Remove(ph1);
                    }

                    foreach (AdmCollegeStudentGuardianDTO mob in guardadd.SelectedGuardianDetails)
                    {
                        if (mob.ACSTG_GuardianName != null && mob.ACSTG_GuardianName != "")
                        {
                            mob.AMCST_Id = guardadd.AMCST_Id;
                            AdmCollegeStudentGuardianDMO Guardian = Mapper.Map<AdmCollegeStudentGuardianDMO>(mob);
                            if (Guardian.ACSTG_Id > 0)
                            {
                                var Guardianresult = _context.AdmCollegeStudentGuardianDMO.Single(t => t.ACSTG_Id == mob.ACSTG_Id);
                                Guardianresult.CreatedDate = Guardianresult.CreatedDate;
                                Guardianresult.UpdatedDate = DateTime.Now;
                                Guardianresult.ACSTG_GuardianPhoto = mob.ACSTG_GuardianPhoto;
                                Guardianresult.ACSTG_GuardianSign = mob.ACSTG_GuardianSign;
                                Guardianresult.ACSTG_Fingerprint = mob.ACSTG_Fingerprint;
                                Guardianresult.ACSTG_CoutryCode = mob.ACSTG_CoutryCode;
                                Mapper.Map(mob, Guardianresult);
                                _context.Update(Guardianresult);
                            }
                            else
                            {
                                Guardian.CreatedDate = DateTime.Now;
                                Guardian.UpdatedDate = DateTime.Now;
                                _context.Add(Guardian);
                            }
                            // _AdmissionFormContext.SaveChanges();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _context.Database.RollbackTransaction();

            }
            return guardadd;
        }
        public AdmMasterCollegeStudentDTO saveDocuments(AdmMasterCollegeStudentDTO docs)
        {
            try
            {
                if (docs.AMCST_Id > 0)
                {
                    var result = _context.Adm_Master_College_StudentDMO.Single(a => a.MI_Id == docs.MI_Id && a.AMCST_Id == docs.AMCST_Id);
                    stud_doc_upload(docs);
                    result.AMCST_GymReqdFlag = docs.AMCST_GymReqdFlag;
                    result.AMCST_HostelReqdFlag = docs.AMCST_HostelReqdFlag;
                    result.AMCST_TransportReqdFlag = docs.AMCST_TransportReqdFlag;
                    //  result.AMST_ECSFlag = mas.AMST_ECSFlag;
                    result.AMCST_FatherPhoto = docs.AMCST_FatherPhoto;
                    result.AMCST_MotherPhoto = docs.AMCST_MotherPhoto;
                    result.AMCST_FatherSign = docs.AMCST_FatherSign;
                    result.AMCST_FatherFingerprint = docs.AMCST_FatherFingerprint;
                    result.AMCST_MotherSign = docs.AMCST_MotherSign;
                    result.AMCST_MotherFingerprint = docs.AMCST_MotherFingerprint;

                    _context.Update(result);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        docs.Message = "Update";

                    }
                    else
                    {
                        docs.Message = "";
                    }
                }
                else
                {
                    Adm_Master_College_StudentDMO result = new Adm_Master_College_StudentDMO();
                    result.MI_Id = docs.MI_Id;
                    stud_doc_upload(docs);
                    result.AMCST_GymReqdFlag = docs.AMCST_GymReqdFlag;
                    result.AMCST_HostelReqdFlag = docs.AMCST_HostelReqdFlag;
                    result.AMCST_TransportReqdFlag = docs.AMCST_TransportReqdFlag;
                    // result.AMST_ECSFlag = mas.AMST_ECSFlag;
                    result.AMCST_FatherPhoto = docs.AMCST_FatherPhoto;
                    result.AMCST_MotherPhoto = docs.AMCST_MotherPhoto;
                    result.AMCST_FatherSign = docs.AMCST_FatherSign;
                    result.AMCST_FatherFingerprint = docs.AMCST_FatherFingerprint;
                    result.AMCST_MotherSign = docs.AMCST_MotherSign;
                    result.AMCST_MotherFingerprint = docs.AMCST_MotherFingerprint;
                    result.CreatedDate = DateTime.Now;
                    result.UpdatedDate = DateTime.Now;
                    _context.Add(result);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        docs.Message = "Add";
                    }
                    else
                    {
                        docs.Message = "";
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
        public AdmMasterCollegeStudentDTO stud_doc_upload(AdmMasterCollegeStudentDTO docsupld)
        {
            try
            {
                //store student documents
                if (docsupld.Uploaded_documentList.Count() > 0)
                {
                    foreach (CollegeMasterDocumentDTO dto in docsupld.Uploaded_documentList)
                    {
                        dto.AMCST_Id = docsupld.AMCST_Id;

                        if (dto.Document_Path != null && dto.Document_Path != "")
                        {
                            AdmCollegeStudentDocumentsDMO document = new AdmCollegeStudentDocumentsDMO();
                            if (dto.ACSTD_Id > 0)
                            {
                                var documentNoresult = _context.AdmCollegeStudentDocumentsDMO.Single(t => t.ACSTD_Id == dto.ACSTD_Id);
                                documentNoresult.ACSMD_Id = dto.AMSMD_Id;
                                documentNoresult.ACSTD_Doc_Name = dto.AMSMD_DocumentName;
                                documentNoresult.ACSTD_Doc_Path = dto.Document_Path;
                                document.CreatedDate = documentNoresult.CreatedDate;
                                document.UpdatedDate = DateTime.Now;
                                _context.Update(documentNoresult);
                            }
                            else
                            {
                                document.ACSMD_Id = dto.AMSMD_Id;
                                document.ACSTD_Doc_Name = dto.AMSMD_DocumentName;
                                document.ACSTD_Doc_Path = dto.Document_Path;
                                document.AMCST_Id = docsupld.AMCST_Id;
                                document.CreatedDate = DateTime.Now;
                                document.UpdatedDate = DateTime.Now;
                                _context.Add(document);
                            }
                            // _AdmissionFormContext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _context.Database.RollbackTransaction();
            }
            return docsupld;
        }

        //save and update previous exam subjectwise marks 
        public AdmMasterCollegeStudentDTO previousexamsubjectwise(AdmMasterCollegeStudentDTO prevadd)
        {
            try
            {
                // save Prev School details
                if (prevadd.Adm_College_Student_SubjectMarksTempDTO != null)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();

                    foreach (Adm_College_Student_SubjectMarksDTO ph in prevadd.Adm_College_Student_SubjectMarksTempDTO)
                    {
                        temparr.Add(ph.ACSTSUM_Id);
                    }

                    Array subject_Noresultremove = _context.Adm_College_Student_SubjectMarksDMO.Where(t => !temparr.Contains(t.ACSTSUM_Id)
                    && t.AMCST_Id == prevadd.AMCST_Id).ToArray();

                    foreach (Adm_College_Student_SubjectMarksDMO ph1 in subject_Noresultremove)
                    {
                        _context.Remove(ph1);
                    }

                    foreach (Adm_College_Student_SubjectMarksDTO mob in prevadd.Adm_College_Student_SubjectMarksTempDTO)
                    {
                        if (mob.ACSTSUM_SubjectName != null && mob.ACSTSUM_SubjectName != "")
                        {
                            mob.AMCST_Id = prevadd.AMCST_Id;

                            Adm_College_Student_SubjectMarksDMO PrevexamSchool = Mapper.Map<Adm_College_Student_SubjectMarksDMO>(mob);
                            if (PrevexamSchool.ACSTSUM_Id > 0)
                            {
                                var PrevexamSchoolresult = _context.Adm_College_Student_SubjectMarksDMO.Single(t => t.ACSTSUM_Id == mob.ACSTSUM_Id);
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
                            //  _AdmissionFormContext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _context.Database.RollbackTransaction();
            }
            return prevadd;
        }
        public AdmMasterCollegeStudentDTO SearchByColumn(AdmMasterCollegeStudentDTO adm)
        {
            try
            {
                //string mobno = "";
                //string email = "";
                List<Adm_Master_College_StudentDMO> students = new List<Adm_Master_College_StudentDMO>();
                List<long> list11 = new List<long>();
                if (adm.SearchColumn == "" || adm.SearchColumn == null)
                {
                    adm.SearchColumn = "0";
                }
                long asmayId = 0;
                List<AdmMasterCollegeStudentDTO> adm_m_student1 = new List<AdmMasterCollegeStudentDTO>();
                if (adm.ASMAY_Id == 0)
                {
                    switch (adm.SearchColumn)
                    {
                        case "0":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id
                                              && adm_stu.AMCST_SOL != "Del"
                                              && (adm_stu.AMCST_FirstName.Contains(adm.EnteredData) ||
                                    adm_stu.AMCST_MiddleName.Contains(adm.EnteredData) || adm_stu.AMCST_LastName.Contains(adm.EnteredData)))
                                              group new { adm_stu, cls, branch }
                                               by new { adm_stu.AMCST_Id } into g
                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = g.FirstOrDefault().adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = g.FirstOrDefault().adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = g.FirstOrDefault().adm_stu.AMCST_LastName,
                                                  AMCST_Date = g.FirstOrDefault().adm_stu.AMCST_Date,
                                                  AMCST_Sex = g.FirstOrDefault().adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = g.FirstOrDefault().adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = g.FirstOrDefault().adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = g.FirstOrDefault().adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(g.FirstOrDefault().adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = g.FirstOrDefault().adm_stu.AMCST_Id,
                                                  courseName = g.FirstOrDefault().cls.AMCO_CourseName,
                                                  branchName = g.FirstOrDefault().branch.AMB_BranchName,
                                                  AMCST_SOL = g.FirstOrDefault().adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((g.FirstOrDefault().adm_stu.AMCST_FirstName == null || g.FirstOrDefault().adm_stu.AMCST_FirstName
                                                  == "" ? "" : g.FirstOrDefault().adm_stu.AMCST_FirstName) + (g.FirstOrDefault().adm_stu.AMCST_MiddleName == null ||
                                                  g.FirstOrDefault().adm_stu.AMCST_MiddleName == "" ? "" : " " + g.FirstOrDefault().adm_stu.AMCST_MiddleName) +
                                                  (g.FirstOrDefault().adm_stu.AMCST_LastName == null || g.FirstOrDefault().adm_stu.AMCST_LastName == "" ? "" : " " +
                                                  g.FirstOrDefault().adm_stu.AMCST_LastName)).Trim()
                                              }).ToList();


                            //multiple mobile number feteching   start
                            var query = (from a in _context.Adm_Master_College_StudentDMO
                                         from b in _context.AdmCollegeStudentSMSNoDMO
                                         where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && (a.AMCST_FirstName.Contains(adm.EnteredData) ||
                                    a.AMCST_MiddleName.Contains(adm.EnteredData) || a.AMCST_LastName.Contains(adm.EnteredData)))
                                         select new Adm_M_Student_TempMobileNo
                                         {
                                             UserName = a.AMCST_Id,
                                             Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                         }).ToList();

                            int count = query.Count() + 1;
                            Adm_M_Student_TempMobileNo[] temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            string value = null;
                            Dictionary<long, string> tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            List<Adm_M_Student_TempMobileNo> list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            var query1 = (from a in _context.Adm_Master_College_StudentDMO
                                          from b in _context.AdmCollegeStudentEmailIdDMO
                                          where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id)
                                          select new Adm_M_Student_TempEmailId
                                          {
                                              UserNameemail = a.AMCST_Id,
                                              Roleemail = b.ACSTE_EmailId
                                          }).ToList();


                            int count1 = query1.Count() + 1;
                            Adm_M_Student_TempEmailId[] temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            string value1 = null;
                            Dictionary<long, string> tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            List<Adm_M_Student_TempEmailId> list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();


                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }



                            break;
                        case "1":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id
                                              //&&  adm_stu.AMST_ActiveFlag == 1  
                                              && adm_stu.AMCST_SOL != "Del"
                                              && adm_stu.AMCST_FirstName.Contains(adm.EnteredData))

                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                    (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                    (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();



                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_FirstName.Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_FirstName.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();

                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "2":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id
                                              && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMCST_MiddleName.Contains(adm.EnteredData))
                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                                  (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                                  (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_MiddleName.Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_MiddleName.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();

                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "3":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMCST_LastName.Contains(adm.EnteredData))
                                              && adm_stu.AMCST_SOL != "Del"
                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                                  (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                                  (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_LastName.Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_LastName.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();


                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "4":
                            try
                            {
                                DateTime date = DateTime.ParseExact(adm.EnteredData, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                                adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                                  from cls in _context.MasterCourseDMO
                                                  from branch in _context.ClgMasterBranchDMO
                                                  where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMCST_Date.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd")))
                                                  && adm_stu.AMCST_SOL != "Del"

                                                  select new AdmMasterCollegeStudentDTO
                                                  {
                                                      //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                      //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                      //AMCST_LastName = adm_stu.AMCST_LastName,
                                                      AMCST_Date = adm_stu.AMCST_Date,
                                                      AMCST_Sex = adm_stu.AMCST_Sex,
                                                      AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                      AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                      AMCST_emailId = adm_stu.AMCST_emailId,
                                                      stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                      AMCST_Id = adm_stu.AMCST_Id,
                                                      courseName = cls.AMCO_CourseName,
                                                      branchName = branch.AMB_BranchName,
                                                      AMCST_SOL = adm_stu.AMCST_SOL,
                                                      AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                                  (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                                  (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                                  }).OrderByDescending(d => d.AMCST_Id).ToList();

                                //multiple mobile number feteching   start
                                query = (from a in _context.Adm_Master_College_StudentDMO
                                         from b in _context.AdmCollegeStudentSMSNoDMO
                                         where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_Date.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd")))
                                         select new Adm_M_Student_TempMobileNo
                                         {
                                             UserName = a.AMCST_Id,
                                             Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                         }).OrderByDescending(d => d.UserName).ToList();

                                count = query.Count() + 1;
                                temp = new Adm_M_Student_TempMobileNo[count];

                                query.CopyTo(temp);

                                value = null;
                                tempDictionary = new Dictionary<long, string>();
                                for (int i = 0; i < query.Count(); i++)
                                {
                                    if (query[i].UserName == temp[i].UserName)
                                    {
                                        if (!tempDictionary.ContainsKey(query[i].UserName))
                                        {
                                            tempDictionary.Add(query[i].UserName, query[i].Role);
                                        }
                                        else
                                        {
                                            tempDictionary.TryGetValue(query[i].UserName, out value);
                                            value = value + ", " + query[i].Role;
                                            tempDictionary[query[i].UserName] = value;
                                        }
                                    }
                                }
                                list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                                adm.stdmobile = list.ToArray();
                                //end here 

                                //assigning the mobile number to main list start
                                for (int k = 0; k < adm_m_student1.Count(); k++)
                                {
                                    if (k < list.Count())
                                    {
                                        if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                        {
                                            adm_m_student1[k].stdmobilenumber = list[k].Role;
                                        }
                                        else
                                        {
                                            adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                        }
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                //end

                                //multiple email ids start 
                                query1 = (from a in _context.Adm_Master_College_StudentDMO
                                          from b in _context.AdmCollegeStudentEmailIdDMO
                                          where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_Date.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd")))
                                          select new Adm_M_Student_TempEmailId
                                          {
                                              UserNameemail = a.AMCST_Id,
                                              Roleemail = b.ACSTE_EmailId
                                          }).OrderByDescending(d => d.UserNameemail).ToList();


                                count1 = query1.Count() + 1;
                                temp1 = new Adm_M_Student_TempEmailId[count1];

                                query1.CopyTo(temp1);

                                value1 = null;
                                tempDictionary1 = new Dictionary<long, string>();
                                for (int i = 0; i < query1.Count(); i++)
                                {
                                    if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                    {
                                        if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                        {
                                            tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                        }
                                        else
                                        {
                                            tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                            value1 = value1 + ", " + query1[i].Roleemail;
                                            tempDictionary1[query1[i].UserNameemail] = value1;
                                        }
                                    }
                                }
                                list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                                adm.stdemail = list1.ToArray();
                                //end

                                //assigning the email ids to main list start
                                for (int k = 0; k < adm_m_student1.Count(); k++)
                                {
                                    if (k < list1.Count())
                                    {
                                        if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                        {
                                            adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                        }
                                        else
                                        {
                                            adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                        }
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }

                                //end
                                adm.StudentList = adm_m_student1.ToArray();

                                if (adm.StudentList.Length > 0)
                                {
                                    adm.count = adm.StudentList.Length;
                                }
                                else
                                {
                                    adm.count = 0;
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                adm.Message = "Please Enter date in dd/MM/yyyy format";

                                adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                                  from cls in _context.MasterCourseDMO
                                                  from branch in _context.ClgMasterBranchDMO
                                                  where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id)
                                                  && adm_stu.AMCST_SOL != "Del"
                                                  select new AdmMasterCollegeStudentDTO
                                                  {
                                                      //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                      //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                      //AMCST_LastName = adm_stu.AMCST_LastName,
                                                      AMCST_Date = adm_stu.AMCST_Date,
                                                      AMCST_Sex = adm_stu.AMCST_Sex,
                                                      AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                      AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                      AMCST_emailId = adm_stu.AMCST_emailId,
                                                      stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                      AMCST_Id = adm_stu.AMCST_Id,
                                                      courseName = cls.AMCO_CourseName,
                                                      branchName = branch.AMB_BranchName,
                                                      AMCST_SOL = adm_stu.AMCST_SOL,
                                                      AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                                  (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                                  (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                                  }).OrderByDescending(d => d.AMCST_Id).ToList();

                                //multiple mobile number feteching   start
                                query = (from a in _context.Adm_Master_College_StudentDMO
                                         from b in _context.AdmCollegeStudentSMSNoDMO
                                         where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id)
                                         select new Adm_M_Student_TempMobileNo
                                         {
                                             UserName = a.AMCST_Id,
                                             Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                         }).OrderByDescending(d => d.UserName).Take(10).ToList();
                                //AdmMasterCollegeStudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                                //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                                count = query.Count() + 1;
                                temp = new Adm_M_Student_TempMobileNo[count];

                                query.CopyTo(temp);

                                value = null;
                                tempDictionary = new Dictionary<long, string>();
                                for (int i = 0; i < query.Count(); i++)
                                {
                                    if (query[i].UserName == temp[i].UserName)
                                    {
                                        if (!tempDictionary.ContainsKey(query[i].UserName))
                                        {
                                            tempDictionary.Add(query[i].UserName, query[i].Role);
                                        }
                                        else
                                        {

                                            tempDictionary.TryGetValue(query[i].UserName, out value);
                                            value = value + ", " + query[i].Role;
                                            tempDictionary[query[i].UserName] = value;
                                        }
                                    }
                                }

                                list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                                adm.stdmobile = list.ToArray();
                                //end here 

                                //assigning the mobile number to main list start
                                for (int k = 0; k < adm_m_student1.Count(); k++)
                                {
                                    if (k < list.Count())
                                    {
                                        if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                        {
                                            adm_m_student1[k].stdmobilenumber = list[k].Role;
                                        }
                                        else
                                        {
                                            adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                        }
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }

                                //end

                                //multiple email ids start 
                                query1 = (from a in _context.Adm_Master_College_StudentDMO
                                          from b in _context.AdmCollegeStudentEmailIdDMO
                                          where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id)
                                          select new Adm_M_Student_TempEmailId
                                          {
                                              UserNameemail = a.AMCST_Id,
                                              Roleemail = b.ACSTE_EmailId
                                          }).OrderByDescending(d => d.UserNameemail).Take(10).ToList();


                                count1 = query1.Count() + 1;
                                temp1 = new Adm_M_Student_TempEmailId[count1];

                                query1.CopyTo(temp1);

                                value1 = null;
                                tempDictionary1 = new Dictionary<long, string>();
                                for (int i = 0; i < query1.Count(); i++)
                                {
                                    if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                    {
                                        if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                        {
                                            tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                        }
                                        else
                                        {
                                            tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                            value1 = value1 + ", " + query1[i].Roleemail;
                                            tempDictionary1[query1[i].UserNameemail] = value1;
                                        }
                                    }
                                }
                                list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                                adm.stdemail = list1.ToArray();
                                //end

                                //assigning the email ids to main list start
                                for (int k = 0; k < adm_m_student1.Count(); k++)
                                {
                                    if (k < list1.Count())
                                    {
                                        if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                        {
                                            adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                        }
                                        else
                                        {
                                            adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                        }
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }

                                adm.StudentList = adm_m_student1.ToArray();

                                if (adm.StudentList.Length > 0)
                                {
                                    adm.count = adm.StudentList.Length;
                                }
                                else
                                {
                                    adm.count = 0;
                                }
                            }

                            break;
                        case "5":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMCST_Sex.Equals(adm.EnteredData))
                                              && adm_stu.AMCST_SOL != "Del"
                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                                  (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                                  (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();

                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_Sex.Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_Sex.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();

                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "6":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMCST_RegistrationNo.Contains(adm.EnteredData))
                                              && adm_stu.AMCST_SOL != "Del"
                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                                  (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                                  (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();

                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_RegistrationNo.Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_RegistrationNo.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();

                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "7":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMCST_AdmNo.Contains(adm.EnteredData))
                                              && adm_stu.AMCST_SOL != "Del"
                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                                  (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                                  (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();



                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_AdmNo.Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_AdmNo.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();

                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "8":

                            List<AdmMasterCollegeStudentDTO> GrpId = new List<AdmMasterCollegeStudentDTO>();

                            // List<long> list11 = new List<long>();

                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && b.ACSTE_EmailId.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            for (int k = 0; k < list1.Count; k++)
                            {
                                list11.Add(list1[k].UserNameemail);
                            }



                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && list11.Contains(adm_stu.AMCST_Id))
                                              && adm_stu.AMCST_SOL != "Del"
                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                                  (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                                  (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && list11.Contains(a.AMCST_Id))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end                       

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();

                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "9":
                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && Convert.ToString(b.ACSTSMS_MobileNo).ToString().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();

                            for (int k = 0; k < list.Count; k++)
                            {
                                list11.Add(list[k].UserName);
                            }

                            //end here 
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id
                                              && adm_stu.MI_Id == adm.MI_Id && list11.Contains(adm_stu.AMCST_Id))
                                              && adm_stu.AMCST_SOL != "Del"
                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                                  (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                                  (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();


                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && list11.Contains(a.AMCST_Id))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();

                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }
                            //end

                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            adm.StudentList = adm_m_student1.ToArray();

                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "10":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && cls.AMCO_CourseName.Contains(adm.EnteredData))
                                              && adm_stu.AMCST_SOL != "Del"

                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                                  (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                                  (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     from c in _context.MasterCourseDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCO_Id == c.AMCO_Id && c.AMCO_CourseName.Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //AdmMasterCollegeStudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      from c in _context.MasterCourseDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCO_Id == c.AMCO_Id && c.AMCO_CourseName.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();


                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "11":
                            if (adm.EnteredData.Equals("active", StringComparison.OrdinalIgnoreCase))

                            {
                                adm.EnteredData = "S";
                            }
                            else if (adm.EnteredData.Equals("deactive", StringComparison.OrdinalIgnoreCase))
                            {
                                adm.EnteredData = "D";
                            }
                            else if (adm.EnteredData.Equals("left", StringComparison.OrdinalIgnoreCase))
                            {
                                adm.EnteredData = "L";
                            }
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMCST_SOL.Equals(adm.EnteredData))
                                              && adm_stu.AMCST_SOL != "Del"

                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                                  (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                                  (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_SOL.Equals(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //AdmMasterCollegeStudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_SOL.Equals(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();



                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "12":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMCST_FatherName.Contains(adm.EnteredData))
                                              && adm_stu.AMCST_SOL != "Del"

                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                                  (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                                  (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_FatherName.Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //AdmMasterCollegeStudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_FatherName.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();


                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "13":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMCST_MotherName.Contains(adm.EnteredData))
                                              && adm_stu.AMCST_SOL != "Del"

                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                                  (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                                  (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();



                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_MotherName.Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //AdmMasterCollegeStudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_MotherName.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();



                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;

                        case "14":
                            var academicYearId = _context.AcademicYear.Where(d => d.ASMAY_Year.Equals(adm.EnteredData) && d.MI_Id == adm.MI_Id).ToList();
                            if (academicYearId.Count > 0)
                            {
                                asmayId = academicYearId.FirstOrDefault().ASMAY_Id;
                            }
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id
                                              && adm_stu.ASMAY_Id == asmayId)
                                              && adm_stu.AMCST_SOL != "Del"
                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                                  (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                                  (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();

                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.ASMAY_Id.Equals(asmayId))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //AdmMasterCollegeStudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.ASMAY_Id.Equals(asmayId))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();


                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;


                        default:
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id)
                                              && adm_stu.AMCST_SOL != "Del"
                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                                  (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                                  (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).Take(10).ToList();

                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id)
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).Take(10).ToList();
                            //AdmMasterCollegeStudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id)
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).Take(10).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();
                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;

                    }
                }

                //-----------------Else part is when search option is based on the year------------------//
                else
                {
                    switch (adm.SearchColumn)
                    {
                        case "0":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.ASMAY_Id == adm.ASMAY_Id && (adm_stu.AMCST_FirstName.Contains(adm.EnteredData) ||
                                    adm_stu.AMCST_MiddleName.Contains(adm.EnteredData) || adm_stu.AMCST_LastName.Contains(adm.EnteredData)))

                                              group new { adm_stu, cls, branch }
                                               by new { adm_stu.AMCST_Id } into g

                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = g.FirstOrDefault().adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = g.FirstOrDefault().adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = g.FirstOrDefault().adm_stu.AMCST_LastName,
                                                  AMCST_Date = g.FirstOrDefault().adm_stu.AMCST_Date,
                                                  AMCST_Sex = g.FirstOrDefault().adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = g.FirstOrDefault().adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = g.FirstOrDefault().adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = g.FirstOrDefault().adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(g.FirstOrDefault().adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = g.FirstOrDefault().adm_stu.AMCST_Id,
                                                  courseName = g.FirstOrDefault().cls.AMCO_CourseName,
                                                  branchName = g.FirstOrDefault().branch.AMB_BranchName,
                                                  AMCST_SOL = g.FirstOrDefault().adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((g.FirstOrDefault().adm_stu.AMCST_FirstName == null || g.FirstOrDefault().adm_stu.AMCST_FirstName == "" ? "" : g.FirstOrDefault().adm_stu.AMCST_FirstName) +
                                                  (g.FirstOrDefault().adm_stu.AMCST_MiddleName == null || g.FirstOrDefault().adm_stu.AMCST_MiddleName == "" ? "" : " " + g.FirstOrDefault().adm_stu.AMCST_MiddleName) +
                                                  (g.FirstOrDefault().adm_stu.AMCST_LastName == null || g.FirstOrDefault().adm_stu.AMCST_LastName == "" ? "" : " " + g.FirstOrDefault().adm_stu.AMCST_LastName)).Trim()
                                              }).ToList();


                            //multiple mobile number feteching   start
                            var query = (from a in _context.Adm_Master_College_StudentDMO
                                         from b in _context.AdmCollegeStudentSMSNoDMO
                                         where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && (a.AMCST_FirstName.Contains(adm.EnteredData) ||
                                    a.AMCST_MiddleName.Contains(adm.EnteredData) || a.AMCST_LastName.Contains(adm.EnteredData)))
                                         select new Adm_M_Student_TempMobileNo
                                         {
                                             UserName = a.AMCST_Id,
                                             Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                         }).ToList();

                            int count = query.Count() + 1;
                            Adm_M_Student_TempMobileNo[] temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            string value = null;
                            Dictionary<long, string> tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            List<Adm_M_Student_TempMobileNo> list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            var query1 = (from a in _context.Adm_Master_College_StudentDMO
                                          from b in _context.AdmCollegeStudentEmailIdDMO
                                          where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id)
                                          select new Adm_M_Student_TempEmailId
                                          {
                                              UserNameemail = a.AMCST_Id,
                                              Roleemail = b.ACSTE_EmailId
                                          }).ToList();


                            int count1 = query1.Count() + 1;
                            Adm_M_Student_TempEmailId[] temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            string value1 = null;
                            Dictionary<long, string> tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            List<Adm_M_Student_TempEmailId> list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();


                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }



                            break;
                        case "1":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMCST_FirstName.Contains(adm.EnteredData))
                                              && adm_stu.AMCST_SOL != "Del"
                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                    (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                    (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();



                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_FirstName.Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_FirstName.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();

                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "2":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMCST_MiddleName.Contains(adm.EnteredData))
                                              && adm_stu.AMCST_SOL != "Del"
                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                    (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                    (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_MiddleName.Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_MiddleName.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();

                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "3":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMCST_LastName.Contains(adm.EnteredData))
                                              && adm_stu.AMCST_SOL != "Del"
                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                    (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                    (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_LastName.Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_LastName.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();


                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "4":
                            try
                            {
                                DateTime date = DateTime.ParseExact(adm.EnteredData, "dd/MM/yyyy",
                                   CultureInfo.InvariantCulture);

                                adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                                  from cls in _context.MasterCourseDMO
                                                  from branch in _context.ClgMasterBranchDMO
                                                  where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMCST_Date.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd")))
                                                  && adm_stu.AMCST_SOL != "Del"

                                                  select new AdmMasterCollegeStudentDTO
                                                  {
                                                      //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                      //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                      //AMCST_LastName = adm_stu.AMCST_LastName,
                                                      AMCST_Date = adm_stu.AMCST_Date,
                                                      AMCST_Sex = adm_stu.AMCST_Sex,
                                                      AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                      AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                      AMCST_emailId = adm_stu.AMCST_emailId,
                                                      stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                      AMCST_Id = adm_stu.AMCST_Id,
                                                      courseName = cls.AMCO_CourseName,
                                                      branchName = branch.AMB_BranchName,
                                                      AMCST_SOL = adm_stu.AMCST_SOL,
                                                      AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                    (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                    (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                                  }).OrderByDescending(d => d.AMCST_Id).ToList();

                                //multiple mobile number feteching   start
                                query = (from a in _context.Adm_Master_College_StudentDMO
                                         from b in _context.AdmCollegeStudentSMSNoDMO
                                         where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_Date.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd")))
                                         select new Adm_M_Student_TempMobileNo
                                         {
                                             UserName = a.AMCST_Id,
                                             Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                         }).OrderByDescending(d => d.UserName).ToList();

                                count = query.Count() + 1;
                                temp = new Adm_M_Student_TempMobileNo[count];

                                query.CopyTo(temp);

                                value = null;
                                tempDictionary = new Dictionary<long, string>();
                                for (int i = 0; i < query.Count(); i++)
                                {
                                    if (query[i].UserName == temp[i].UserName)
                                    {
                                        if (!tempDictionary.ContainsKey(query[i].UserName))
                                        {
                                            tempDictionary.Add(query[i].UserName, query[i].Role);
                                        }
                                        else
                                        {
                                            tempDictionary.TryGetValue(query[i].UserName, out value);
                                            value = value + ", " + query[i].Role;
                                            tempDictionary[query[i].UserName] = value;
                                        }
                                    }
                                }
                                list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                                adm.stdmobile = list.ToArray();
                                //end here 

                                //assigning the mobile number to main list start
                                for (int k = 0; k < adm_m_student1.Count(); k++)
                                {
                                    if (k < list.Count())
                                    {
                                        if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                        {
                                            adm_m_student1[k].stdmobilenumber = list[k].Role;
                                        }
                                        else
                                        {
                                            adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                        }
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                //end

                                //multiple email ids start 
                                query1 = (from a in _context.Adm_Master_College_StudentDMO
                                          from b in _context.AdmCollegeStudentEmailIdDMO
                                          where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_Date.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd")))
                                          select new Adm_M_Student_TempEmailId
                                          {
                                              UserNameemail = a.AMCST_Id,
                                              Roleemail = b.ACSTE_EmailId
                                          }).OrderByDescending(d => d.UserNameemail).ToList();


                                count1 = query1.Count() + 1;
                                temp1 = new Adm_M_Student_TempEmailId[count1];

                                query1.CopyTo(temp1);

                                value1 = null;
                                tempDictionary1 = new Dictionary<long, string>();
                                for (int i = 0; i < query1.Count(); i++)
                                {
                                    if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                    {
                                        if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                        {
                                            tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                        }
                                        else
                                        {
                                            tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                            value1 = value1 + ", " + query1[i].Roleemail;
                                            tempDictionary1[query1[i].UserNameemail] = value1;
                                        }
                                    }
                                }
                                list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                                adm.stdemail = list1.ToArray();
                                //end

                                //assigning the email ids to main list start
                                for (int k = 0; k < adm_m_student1.Count(); k++)
                                {
                                    if (k < list1.Count())
                                    {
                                        if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                        {
                                            adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                        }
                                        else
                                        {
                                            adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                        }
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }

                                //end
                                adm.StudentList = adm_m_student1.ToArray();

                                if (adm.StudentList.Length > 0)
                                {
                                    adm.count = adm.StudentList.Length;
                                }
                                else
                                {
                                    adm.count = 0;
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                adm.Message = "Please Enter date in dd/MM/yyyy format";
                                adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                                  from cls in _context.MasterCourseDMO
                                                  from branch in _context.ClgMasterBranchDMO
                                                  where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id)
                                                  && adm_stu.AMCST_SOL != "Del"
                                                  select new AdmMasterCollegeStudentDTO
                                                  {
                                                      //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                      //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                      //AMCST_LastName = adm_stu.AMCST_LastName,
                                                      AMCST_Date = adm_stu.AMCST_Date,
                                                      AMCST_Sex = adm_stu.AMCST_Sex,
                                                      AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                      AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                      AMCST_emailId = adm_stu.AMCST_emailId,
                                                      stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                      AMCST_Id = adm_stu.AMCST_Id,
                                                      courseName = cls.AMCO_CourseName,
                                                      branchName = branch.AMB_BranchName,
                                                      AMCST_SOL = adm_stu.AMCST_SOL,
                                                      AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                    (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                    (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                                  }).OrderByDescending(d => d.AMCST_Id).ToList();

                                //multiple mobile number feteching   start
                                query = (from a in _context.Adm_Master_College_StudentDMO
                                         from b in _context.AdmCollegeStudentSMSNoDMO
                                         where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id)
                                         select new Adm_M_Student_TempMobileNo
                                         {
                                             UserName = a.AMCST_Id,
                                             Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                         }).OrderByDescending(d => d.UserName).Take(10).ToList();
                                //AdmMasterCollegeStudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                                //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                                count = query.Count() + 1;
                                temp = new Adm_M_Student_TempMobileNo[count];

                                query.CopyTo(temp);

                                value = null;
                                tempDictionary = new Dictionary<long, string>();
                                for (int i = 0; i < query.Count(); i++)
                                {
                                    if (query[i].UserName == temp[i].UserName)
                                    {
                                        if (!tempDictionary.ContainsKey(query[i].UserName))
                                        {
                                            tempDictionary.Add(query[i].UserName, query[i].Role);
                                        }
                                        else
                                        {

                                            tempDictionary.TryGetValue(query[i].UserName, out value);
                                            value = value + ", " + query[i].Role;
                                            tempDictionary[query[i].UserName] = value;
                                        }
                                    }
                                }

                                list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                                adm.stdmobile = list.ToArray();
                                //end here 

                                //assigning the mobile number to main list start
                                for (int k = 0; k < adm_m_student1.Count(); k++)
                                {
                                    if (k < list.Count())
                                    {
                                        if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                        {
                                            adm_m_student1[k].stdmobilenumber = list[k].Role;
                                        }
                                        else
                                        {
                                            adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                        }
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }

                                //end

                                //multiple email ids start 
                                query1 = (from a in _context.Adm_Master_College_StudentDMO
                                          from b in _context.AdmCollegeStudentEmailIdDMO
                                          where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id)
                                          select new Adm_M_Student_TempEmailId
                                          {
                                              UserNameemail = a.AMCST_Id,
                                              Roleemail = b.ACSTE_EmailId
                                          }).OrderByDescending(d => d.UserNameemail).Take(10).ToList();


                                count1 = query1.Count() + 1;
                                temp1 = new Adm_M_Student_TempEmailId[count1];

                                query1.CopyTo(temp1);

                                value1 = null;
                                tempDictionary1 = new Dictionary<long, string>();
                                for (int i = 0; i < query1.Count(); i++)
                                {
                                    if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                    {
                                        if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                        {
                                            tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                        }
                                        else
                                        {
                                            tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                            value1 = value1 + ", " + query1[i].Roleemail;
                                            tempDictionary1[query1[i].UserNameemail] = value1;
                                        }
                                    }
                                }
                                list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                                adm.stdemail = list1.ToArray();
                                //end

                                //assigning the email ids to main list start
                                for (int k = 0; k < adm_m_student1.Count(); k++)
                                {
                                    if (k < list1.Count())
                                    {
                                        if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                        {
                                            adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                        }
                                        else
                                        {
                                            adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                        }
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }

                                adm.StudentList = adm_m_student1.ToArray();

                                if (adm.StudentList.Length > 0)
                                {
                                    adm.count = adm.StudentList.Length;
                                }
                                else
                                {
                                    adm.count = 0;
                                }
                            }

                            break;
                        case "5":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMCST_Sex.Equals(adm.EnteredData))
                                              && adm_stu.AMCST_SOL != "Del"
                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                    (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                    (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();

                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_Sex.Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_Sex.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();

                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "6":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMCST_RegistrationNo.Contains(adm.EnteredData))
                                              && adm_stu.AMCST_SOL != "Del"
                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                    (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                    (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();

                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_RegistrationNo.Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_RegistrationNo.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();

                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "7":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMCST_AdmNo.Contains(adm.EnteredData))
                                              && adm_stu.AMCST_SOL != "Del"

                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                    (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                    (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_AdmNo.Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_AdmNo.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();

                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "8":

                            List<AdmMasterCollegeStudentDTO> GrpId = new List<AdmMasterCollegeStudentDTO>();

                            // List<long> list11 = new List<long>();

                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.ASMAY_Id == adm.ASMAY_Id && b.ACSTE_EmailId.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            for (int k = 0; k < list1.Count; k++)
                            {
                                list11.Add(list1[k].UserNameemail);
                            }



                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && list11.Contains(adm_stu.AMCST_Id))
                                              && adm_stu.AMCST_SOL != "Del"
                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                    (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                    (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();

                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && list11.Contains(a.AMCST_Id))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end                       

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();

                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "9":
                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.ASMAY_Id == adm.ASMAY_Id && Convert.ToString(b.ACSTSMS_MobileNo).ToString().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();

                            for (int k = 0; k < list.Count; k++)
                            {
                                list11.Add(list[k].UserName);
                            }

                            //end here 
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && list11.Contains(adm_stu.AMCST_Id)) && adm_stu.AMCST_SOL != "Del"
                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                    (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                    (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();


                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && list11.Contains(a.AMCST_Id))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();

                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }
                            //end

                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            adm.StudentList = adm_m_student1.ToArray();

                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "10":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.ASMAY_Id == adm.ASMAY_Id && cls.AMCO_CourseName.Contains(adm.EnteredData))
                                              && adm_stu.AMCST_SOL != "Del"

                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                    (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                    (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     from c in _context.MasterCourseDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCO_Id == c.AMCO_Id && c.AMCO_CourseName.Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //AdmMasterCollegeStudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      from c in _context.MasterCourseDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCO_Id == c.AMCO_Id && c.AMCO_CourseName.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();


                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "11":
                            if (adm.EnteredData.Equals("active", StringComparison.OrdinalIgnoreCase))

                            {
                                adm.EnteredData = "S";
                            }
                            else if (adm.EnteredData.Equals("deactive", StringComparison.OrdinalIgnoreCase))
                            {
                                adm.EnteredData = "D";
                            }
                            else if (adm.EnteredData.Equals("left", StringComparison.OrdinalIgnoreCase))
                            {
                                adm.EnteredData = "L";
                            }
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMCST_SOL.Equals(adm.EnteredData))
                                              && adm_stu.AMCST_SOL != "Del"

                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                    (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                    (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_SOL.Equals(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //AdmMasterCollegeStudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_SOL.Equals(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();



                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "12":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMCST_FatherName.Contains(adm.EnteredData))
                                              && adm_stu.AMCST_SOL != "Del"

                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                    (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                    (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_FatherName.Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //AdmMasterCollegeStudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_FatherName.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();


                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "13":
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMCST_MotherName.Contains(adm.EnteredData))
                                              && adm_stu.AMCST_SOL != "Del"

                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                    (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                    (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();



                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_MotherName.Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //AdmMasterCollegeStudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.AMCST_MotherName.Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();



                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;

                        case "14":
                            var academicYearId = _context.AcademicYear.Where(d => d.ASMAY_Year.Equals(adm.EnteredData) && d.MI_Id == adm.MI_Id).ToList();
                            if (academicYearId.Count > 0)
                            {
                                asmayId = academicYearId.FirstOrDefault().ASMAY_Id;
                            }
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.ASMAY_Id == asmayId) && adm_stu.AMCST_SOL != "Del"
                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                    (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                    (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).ToList();

                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.ASMAY_Id.Equals(asmayId))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //AdmMasterCollegeStudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id && a.ASMAY_Id.Equals(asmayId))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();


                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;


                        default:
                            adm_m_student1 = (from adm_stu in _context.Adm_Master_College_StudentDMO
                                              from cls in _context.MasterCourseDMO
                                              from branch in _context.ClgMasterBranchDMO
                                              where (adm_stu.AMB_Id == branch.AMB_Id && adm_stu.AMCO_Id == cls.AMCO_Id && adm_stu.MI_Id == adm.MI_Id)
                                              && adm_stu.AMCST_SOL != "Del"
                                              select new AdmMasterCollegeStudentDTO
                                              {
                                                  //AMCST_FirstName = adm_stu.AMCST_FirstName,
                                                  //AMCST_MiddleName = adm_stu.AMCST_MiddleName,
                                                  //AMCST_LastName = adm_stu.AMCST_LastName,
                                                  AMCST_Date = adm_stu.AMCST_Date,
                                                  AMCST_Sex = adm_stu.AMCST_Sex,
                                                  AMCST_RegistrationNo = adm_stu.AMCST_RegistrationNo,
                                                  AMCST_AdmNo = adm_stu.AMCST_AdmNo,
                                                  AMCST_emailId = adm_stu.AMCST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMCST_MobileNo),
                                                  AMCST_Id = adm_stu.AMCST_Id,
                                                  courseName = cls.AMCO_CourseName,
                                                  branchName = branch.AMB_BranchName,
                                                  AMCST_SOL = adm_stu.AMCST_SOL,
                                                  AMCST_FirstName = ((adm_stu.AMCST_FirstName == null || adm_stu.AMCST_FirstName == "" ? "" : adm_stu.AMCST_FirstName) +
                                    (adm_stu.AMCST_MiddleName == null || adm_stu.AMCST_MiddleName == "" ? "" : " " + adm_stu.AMCST_MiddleName) +
                                    (adm_stu.AMCST_LastName == null || adm_stu.AMCST_LastName == "" ? "" : " " + adm_stu.AMCST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMCST_Id).Take(10).ToList();

                            //multiple mobile number feteching   start
                            query = (from a in _context.Adm_Master_College_StudentDMO
                                     from b in _context.AdmCollegeStudentSMSNoDMO
                                     where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id)
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMCST_Id,
                                         Role = Convert.ToString(b.ACSTSMS_MobileNo)
                                     }).OrderByDescending(d => d.UserName).Take(10).ToList();
                            //AdmMasterCollegeStudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _context.Adm_Master_College_StudentDMO
                                      from b in _context.AdmCollegeStudentEmailIdDMO
                                      where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == adm.MI_Id)
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMCST_Id,
                                          Roleemail = b.ACSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).Take(10).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMCST_Id)
                                    {
                                        adm_m_student1[k].AMCST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMCST_emailId = adm_m_student1[k].AMCST_emailId;
                                }
                            }

                            //end
                            adm.StudentList = adm_m_student1.ToArray();
                            if (adm.StudentList.Length > 0)
                            {
                                adm.count = adm.StudentList.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;

                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return adm;
        }
        public AdmMasterCollegeStudentDTO DeleteEntry(AdmMasterCollegeStudentDTO data)
        {
            try
            {
                var checkmapped = _context.Adm_College_Yearly_StudentDMO.Where(a => a.AMCST_Id == data.AMCST_Id && a.ACYST_ActiveFlag == 1).ToList();
                if (checkmapped.Count() > 0)
                {
                    data.Message = "Sorry...You Can't Delete This Record.Section Is Already Allotted For This Student";
                }
                else
                {
                    var result = _context.Adm_Master_College_StudentDMO.Single(t => t.AMCST_Id == data.AMCST_Id);
                    result.AMCST_ActiveFlag = false;
                    result.AMCST_SOL = "Del";
                    _context.Update(result);
                    var flag = _context.SaveChanges();
                    if (flag > 0)
                    {
                        data.Message = "Update";
                    }
                    else
                    {
                        data.Message = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AdmMasterCollegeStudentDTO ViewStudentProfile(AdmMasterCollegeStudentDTO data)
        {
            try
            {
                data.viewstudentjoineddetails = (from a in _context.Adm_Master_College_StudentDMO
                                                 from b in _context.AcademicYear
                                                 from c in _context.MasterCourseDMO
                                                 where (a.ASMAY_Id == b.ASMAY_Id && a.AMCO_Id == c.AMCO_Id && a.AMCST_Id == data.AMCST_Id
                                                 && a.MI_Id == data.MI_Id)
                                                 select new AdmMasterCollegeStudentDTO
                                                 {
                                                     studentname = ((a.AMCST_FirstName == null || a.AMCST_FirstName == "" ? "" : a.AMCST_FirstName) +
                                                     (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "" ? "" : " " + a.AMCST_MiddleName) +
                                                     (a.AMCST_LastName == null || a.AMCST_LastName == "" ? "" : " " + a.AMCST_LastName)).Trim(),
                                                     AMCST_AdmNo = a.AMCST_AdmNo,
                                                     AMCST_RegistrationNo = a.AMCST_RegistrationNo,
                                                     ASMAY_Year = b.ASMAY_Year,
                                                     AMCO_CourseName = c.AMCO_CourseName,

                                                     AMCST_StudentPhoto = a.AMCST_StudentPhoto,
                                                     AMCST_Sex = a.AMCST_Sex,
                                                     AMCST_SOL = a.AMCST_SOL,
                                                     AMCST_Date = a.AMCST_Date,
                                                     AMCST_DOB = a.AMCST_DOB,
                                                     AMCST_TPINNO = Convert.ToInt64(a.AMCST_TPINNO)
                                                 }).Distinct().ToArray();

                data.viewstudentdetails = _context.Adm_Master_College_StudentDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id).ToArray();

                data.viewstudentacademicyeardetails = (from a in _context.Adm_College_Yearly_StudentDMO
                                                       from b in _context.AcademicYear
                                                       from c in _context.MasterCourseDMO
                                                       from d in _context.ClgMasterBranchDMO
                                                       from e in _context.CLG_Adm_Master_SemesterDMO
                                                       where (a.ASMAY_Id == b.ASMAY_Id && a.AMCO_Id == c.AMCO_Id && a.AMB_Id == d.AMB_Id && a.AMSE_Id == e.AMSE_Id
                                                       && a.AMCST_Id == data.AMCST_Id)
                                                       select new AdmMasterCollegeStudentDTO
                                                       {
                                                           ASMAY_Year = b.ASMAY_Year,
                                                           AMCO_CourseName = c.AMCO_CourseName,
                                                           AMB_BranchName = d.AMB_BranchName,
                                                           AMSE_SEMName = e.AMSE_SEMName,
                                                           order = b.ASMAY_Order,
                                                           ASMAY_Id = a.ASMAY_Id,
                                                           ACYST_RollNo = a.ACYST_RollNo,
                                                           Status_Flag = a.ASMAY_Id == data.ASMAY_Id ? "Current Year" : ""
                                                       }).Distinct().OrderByDescending(a => a.order).ToArray();


                data.viewstudentguardiandetails = _context.AdmCollegeStudentGuardianDMO.Where(a => a.AMCST_Id == data.AMCST_Id).ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "clg_Adm_View_StudentWise_Attendance";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar) { Value = data.AMCST_Id });

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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.viewstudentattendancetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                data.viewstudentsubjectdetails = (from a in _context.Exm_Col_Studentwise_SubjectsDMO
                                                  from b in _context.IVRM_School_Master_SubjectsDMO
                                                  where (a.ISMS_Id == b.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id)
                                                  select new AdmMasterCollegeStudentDTO
                                                  {
                                                      ISMS_Id = a.ISMS_Id,
                                                      ISMS_SubjectName = b.ISMS_SubjectName,
                                                      subjorder = b.ISMS_OrderFlag,
                                                      ECSTSU_ElectiveFlag = a.ECSTSU_ElectiveFlag

                                                  }).Distinct().OrderBy(a => a.subjorder).ToArray();


            }
            catch (Exception e)
            {
                _log.LogInformation("in admission form");
                _log.LogDebug(e.Message);
            }
            return data;
        }

        //master competitive exam

        public AdmMasterCollegeStudentDTO compExamName(AdmMasterCollegeStudentDTO ct)
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
                            sub_ids.Add(item.AMCEXMSUB_Id);
                        }
                    }
                }
                if (ct.subflg == true)
                {
                    var subname = _context.Master_CompetitiveExamsSubjectsAdmClgDMO.Where(d => d.AMCEXMSUB_Id == Convert.ToInt64(ct.AMCEXMSUB_Id) && d.AMCEXMSUB_ActiveFlg == true).ToList();
                    if (subname.Count > 0)
                    {
                        ct.compSubList = _context.Master_CompetitiveExamsSubjectsAdmClgDMO.Where(d => d.AMCEXMSUB_Id == subname.FirstOrDefault().AMCEXMSUB_Id && d.AMCEXMSUB_ActiveFlg == true).ToArray();
                    }
                }
                else
                {
                    var examname = _context.Master_Competitive_AdmExamsClgDMO.Where(d => d.AMCEXM_Id == Convert.ToInt64(ct.AMCEXM_Id) && d.MI_Id == ct.MI_Id && d.AMCEXM_ActiveFlg == true).ToList();
                    if (examname.Count > 0)
                    {
                        ct.compExamList = _context.Master_CompetitiveExamsSubjectsAdmClgDMO.Where(d => d.AMCEXM_Id == examname.FirstOrDefault().AMCEXM_Id && d.AMCEXMSUB_ActiveFlg == true).ToArray();
                    }
                    if (ct.tempidlist != null)
                    {
                        if (ct.tempidlist.Length > 0)
                        {
                            ct.compExamList = _context.Master_CompetitiveExamsSubjectsAdmClgDMO.Where(d => d.AMCEXMSUB_ActiveFlg == true && !sub_ids.Contains(d.AMCEXMSUB_Id) && d.AMCEXM_Id == examname.FirstOrDefault().AMCEXM_Id).ToArray();
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


        public AdmMasterCollegeStudentDTO stuCompexamdetailse1(AdmMasterCollegeStudentDTO prevadd)
        {
            try
            {
                if (prevadd.Adm_College_Student_CEMarksDTO != null)
                {
                    foreach (Adm_College_Student_CEMarksDTO mob in prevadd.Adm_College_Student_CEMarksDTO)
                    {
                        if (mob.AMCEXM_Id != null && mob.AMCEXM_Id != 0)
                        {
                            mob.AMCST_Id = prevadd.AMCST_Id;
                            mob.ACSTCEM_ActiveFlg = true;

                            Adm_College_Student_CEMarksDMO compExamDetails = Mapper.Map<Adm_College_Student_CEMarksDMO>(mob);
                            var compExamDetailsdup = _context.Adm_College_Student_CEMarksDMO.Count(t => t.AMCST_Id == mob.AMCST_Id && t.ACSTCEM_ActiveFlg == true && t.AMCEXM_Id == mob.AMCEXM_Id);
                            if (compExamDetailsdup > 0)
                            {
                                Mapper.Map(mob, compExamDetails);
                                _context.Update(compExamDetails);
                            }
                            else if (compExamDetailsdup == 0)
                            {  //added by 02/02/2017
                                compExamDetails.ACSTCEM_ActiveFlg = true;
                                _context.Add(compExamDetails);
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

        public AdmMasterCollegeStudentDTO stuCompexamSubMarks1(AdmMasterCollegeStudentDTO prevadd)
        {
            try
            {
                if (prevadd.Adm_College_Student_CEMarks_SubjectDTO != null)
                {
                    foreach (Adm_College_Student_CEMarks_SubjectDTO mob in prevadd.Adm_College_Student_CEMarks_SubjectDTO)
                    {
                        if (mob.AMCEXM_Id != null && mob.AMCEXM_Id != 0)
                        {

                            mob.AMCST_Id = prevadd.AMCST_Id;
                            mob.ACSTCEMS_ActiveFlg = true;
                            var compSubMarksdup = _context.Adm_College_Student_CEMarks_SubjectDMO.Count(t => t.AMCST_Id == mob.AMCST_Id && t.ACSTCEMS_ActiveFlg == true && t.AMCEXM_Id == mob.AMCEXM_Id);
                            Adm_College_Student_CEMarks_SubjectDMO compSubMarksDetails = Mapper.Map<Adm_College_Student_CEMarks_SubjectDMO>(mob);
                            if (compSubMarksdup == 0)
                            {

                                if (compSubMarksDetails.ACSTCEMS_Id > 0)
                                {
                                    Mapper.Map(mob, compSubMarksDetails);
                                    _context.Update(compSubMarksDetails);
                                }



                                else
                                {  //added by 02/02/2017
                                    compSubMarksDetails.ACSTCEMS_MaxMarks = prevadd.ACSTCEMS_MaxMarks;
                                    compSubMarksDetails.ACSTCEMS_ActiveFlg = true;
                                    _context.Add(compSubMarksDetails);
                                }
                            }
                        }
                        _context.SaveChanges();
                        //  }

                    }
                }

            }
            catch (Exception e)
            {

                _context.Database.RollbackTransaction();
            }
            return prevadd;
        }


        //document view
        public AdmMasterCollegeStudentDTO getprintdata(AdmMasterCollegeStudentDTO Edata)
        {
            try
            {
                //List<MasterStudentBondDMO> MasterStudentBondDMO = new List<MasterStudentBondDMO>();
                //MasterStudentBondDMO = _AdmissionFormContext.MasterStudentBondDMO.Where(t => t.AMST_Id == AdmMasterCollegeStudentDTO.AMST_Id).ToList().ToList();
                //AdmMasterCollegeStudentDTO.BondList = MasterStudentBondDMO.ToArray();



                //Edata.subjectlist = (from a in _context.Adm_Master_College_StudentDMO
                //                     from b in _context.IVRM_School_Master_SubjectsDMO

                //                     where a.AMCST_Id == Edata.AMCST_Id && a.ISMS_Id == b.ISMS_Id && b.ISMS_LanguageFlg == 0
                //                     select b).Distinct().ToArray();
                //Edata.subjectlistlag = (from a in _context.PA_College_Student_SubjectDMO
                //                        from b in _context.IVRM_School_Master_SubjectsDMO

                //                        where a.PACA_Id == Edata.PACA_Id && a.ISMS_Id == b.ISMS_Id && b.ISMS_LanguageFlg == 1
                //                        select b).Distinct().ToArray();






                // Edata.activitydetails = _context.PA_College_Student_PrevExtracurricularDMO.Where(t => t.PACA_Id == Edata.PACA_Id).ToArray();

                //   Edata.achievementdata = _context.PA_College_Student_AchivementsTypeDMO.Where(t => t.PACA_Id == Edata.PACA_Id).ToArray();

                var StudentPrevSchoolDMO = _context.AdmCollegeStudentPrevSchoolDMO.Where(t => t.AMCST_Id == Edata.AMCST_Id).ToList();
                Edata.PrevSchoolDetails = StudentPrevSchoolDMO.ToArray();




                var StudentGuardianDMO = _context.AdmCollegeStudentGuardianDMO.Where(t => t.AMCST_Id == Edata.AMCST_Id).ToList();
                Edata.StudentGuardianDetails = StudentGuardianDMO.ToArray();

                //var Studentsubjectmarks = _context.PA_College_Student_SubjectMarks.Where(t => t.PACA_Id == Edata.PACA_Id).ToList();
                //Edata.Studentsubjectmarksarry = Studentsubjectmarks.ToArray();



                var adm_m_student = _context.Adm_Master_College_StudentDMO.Where(t => t.AMCST_Id == Edata.AMCST_Id).ToList();
                Edata.StudentList = adm_m_student.ToArray();

                Edata.AllCaste = (from m in _context.CasteCategory
                                  from n in _context.Caste
                                  where m.IMCC_Id == n.IMCC_Id && n.IMC_Id == adm_m_student.FirstOrDefault().IMC_Id && n.MI_Id == adm_m_student.FirstOrDefault().MI_Id
                                  select new AdmMasterCollegeStudentDTO
                                  {
                                      IMC_Id = n.IMC_Id,
                                      IMC_CasteName = n.IMC_CasteName
                                  }).ToArray();




                List<CollegeDocumentReportDTO> result = new List<CollegeDocumentReportDTO>();
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
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
                                result.Add(new CollegeDocumentReportDTO
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
                var StudDocumentList = (from sp in _context.AdmCollegeStudentDocumentsDMO
                                        from cp in _context.MasterDocumentDMO
                                        where (sp.ACSMD_Id == cp.AMSMD_Id && sp.AMCST_Id == Edata.AMCST_Id)
                                        select new CollegeDocumentReportDTO
                                        {
                                            ACSTD_Doc_Path = cp.AMSMD_DocumentName,
                                            AMSMD_Id = cp.AMSMD_Id,
                                            ACSTD_Id = sp.ACSTD_Id,
                                            AMCST_Id = sp.AMCST_Id,
                                            Document_Path = sp.ACSTD_Doc_Path
                                        }).ToList();

                for (int i = 0; i < result.Count; i++)
                {
                    StudDocumentList.Add(result[i]);
                }
                Edata.DocumentList = StudDocumentList.ToArray();

                Edata.studentcourse = (from a in _context.Adm_Master_College_StudentDMO
                                       from b in _context.MasterCourseDMO
                                       where (a.AMCO_Id == b.AMCO_Id && a.AMCST_Id == Edata.AMCST_Id)
                                       select new AdmMasterCollegeStudentDTO
                                       {
                                           AMCO_CourseName = b.AMCO_CourseName
                                       }).ToArray();

                Edata.studentReligion = (from a in _context.Adm_Master_College_StudentDMO
                                         from c in _context.Religion
                                         where (a.IVRMMR_Id == c.IVRMMR_Id && a.AMCST_Id == Edata.AMCST_Id)
                                         select new AdmMasterCollegeStudentDTO
                                         {
                                             IVRMMR_Name = c.IVRMMR_Name
                                         }).ToArray();


                Edata.studentcastecate = (from a in _context.Adm_Master_College_StudentDMO
                                          from e in _context.Caste
                                          where (a.IMC_Id == e.IMC_Id && a.AMCST_Id == Edata.AMCST_Id)
                                          select new AdmMasterCollegeStudentDTO
                                          {
                                              IMC_CasteName = e.IMC_CasteName
                                          }).ToArray();


                Edata.studentpreviousstate = (from a in _context.AdmCollegeStudentPrevSchoolDMO
                                              from b in _context.State
                                              where (Convert.ToInt64(a.ACSTPS_PreSchoolState) == b.IVRMMS_Id && a.AMCST_Id == Edata.PACA_Id)

                                              select new AdmMasterCollegeStudentDTO
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


                Edata.studentperstate = (from a in _context.Adm_Master_College_StudentDMO
                                         from b in _context.State
                                         where (a.AMCST_PerState == b.IVRMMS_Id && a.AMCST_Id == Edata.AMCST_Id)
                                         select new AdmMasterCollegeStudentDTO
                                         {
                                             studperstate = b.IVRMMS_Name,
                                             statecode = b.IVRMMS_Code
                                         }).ToArray();


                Edata.studentconstate = (from a in _context.Adm_Master_College_StudentDMO
                                         from b in _context.State
                                         where (a.AMCST_ConState == b.IVRMMS_Id && a.AMCST_Id == Edata.AMCST_Id)
                                         select new AdmMasterCollegeStudentDTO
                                         {
                                             studconstate = b.IVRMMS_Name
                                         }).ToArray();

                Edata.studentpreviouscountry = (from a in _context.AdmCollegeStudentPrevSchoolDMO
                                                from b in _context.Country
                                                where (Convert.ToInt64(a.ACSTPS_PreSchoolCountry) == b.IVRMMC_Id && a.AMCST_Id == Edata.AMCST_Id)
                                                select new AdmMasterCollegeStudentDTO
                                                {
                                                    studconcountry = b.IVRMMC_CountryName
                                                }).ToArray();

                Edata.studentpasissuecountry = (from a in _context.Adm_Master_College_StudentDMO
                                                from b in _context.Country
                                                where (a.AMCST_PassportIssuedCounrty == (b.IVRMMC_Id) && a.AMCST_Id == Edata.AMCST_Id)
                                                select new AdmMasterCollegeStudentDTO
                                                {
                                                    studconcountry = b.IVRMMC_CountryName
                                                }).ToArray();





                Edata.studentconcountry = (from a in _context.Adm_Master_College_StudentDMO
                                           from b in _context.Country
                                           where (a.AMCST_ConCountryId == (b.IVRMMC_Id) && a.AMCST_Id == Edata.AMCST_Id)
                                           select new AdmMasterCollegeStudentDTO
                                           {
                                               studconcountry = b.IVRMMC_CountryName
                                           }).ToArray();

                Edata.studentpercountry = (from a in _context.Adm_Master_College_StudentDMO
                                           from b in _context.Country
                                           where (a.AMCST_Nationality == b.IVRMMC_Id && a.AMCST_Id == Edata.AMCST_Id)
                                           select new AdmMasterCollegeStudentDTO
                                           {
                                               studpercountry = b.IVRMMC_Nationality,
                                               countrycode = b.IVRMMC_CountryCode
                                           }).ToArray();

                Edata.CasteCategoryName = (from a in _context.Adm_Master_College_StudentDMO
                                           from b in _context.CasteCategory
                                           where (a.IMCC_Id == b.IMCC_Id && a.AMCST_Id == Edata.AMCST_Id)
                                           select new AdmMasterCollegeStudentDTO
                                           {
                                               CategoryName = b.IMCC_CategoryName
                                           }).ToArray();

                //Edata.studentpreffredbranch = (from a in _context.Adm_Master_College_StudentDMO
                //                               from b in _context.PA_College_Student_CBPreference
                //                               from c in _context.ClgMasterBranchDMO
                //                               where (a.PACA_Id == b.PACA_Id && b.AMB_Id == c.AMB_Id && a.PACA_Id == Edata.PACA_Id && b.PACA_Id == Edata.PACA_Id)
                //                               select new CollegePreadmissionstudnetDto
                //                               {
                //                                   branchname = c.AMB_BranchName
                //                               }).ToArray();

                Edata.studentcurrenrtbranch = (from a in _context.Adm_Master_College_StudentDMO
                                               from c in _context.ClgMasterBranchDMO
                                               where (a.AMB_Id == c.AMB_Id && a.AMCST_Id == Edata.AMCST_Id)
                                               select new AdmMasterCollegeStudentDTO
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

                var asmccid = _context.Adm_Master_College_StudentDMO.Where(t => t.AMCST_Id == Edata.AMCST_Id).ToList();
                Edata.studentCategory = (from m in _context.ClgMasterCourseCategorycategoryMap
                                         from n in _context.mastercategory
                                         where m.AMCOC_Id == n.AMCOC_Id && m.AMCOC_Id == asmccid.FirstOrDefault().AMCST_Id
                                         select new AdmMasterCollegeStudentDTO
                                         {
                                             AMCOC_Id = m.AMCOC_Id,
                                             AMCOC_Name = n.AMCOC_Name
                                         }).ToArray();

                //ADDED ON june-30 Praveen
                Edata.yearname = _context.AcademicYear.Where(t => t.ASMAY_Id == asmccid.FirstOrDefault().ASMAY_Id).Select(d => d.ASMAY_Year).FirstOrDefault();
                //END ON june-30 Praveen
                //added by roopa 13-10-2021

                Edata.studentCompDetails = (from a in _context.Adm_College_Student_CEMarksDMO
                                            from c in _context.Adm_Master_College_StudentDMO
                                            from d in _context.Master_Competitive_AdmExamsClgDMO
                                            where (a.AMCEXM_Id == d.AMCEXM_Id && a.AMCST_Id == c.AMCST_Id && c.MI_Id == Edata.MI_Id && a.ACSTCEM_ActiveFlg == true && c.AMCST_Id == asmccid.FirstOrDefault().AMCST_Id)
                                            select new Adm_College_Student_CEMarksDTO
                                            {
                                                AMCEXM_Id = a.AMCEXM_Id,
                                                ACSTCEM_Id = a.ACSTCEM_Id,
                                                ACSTCEM_RollNo = a.ACSTCEM_RollNo,
                                                ACSTCEM_RegistrationId = a.ACSTCEM_RegistrationId,
                                                ACSTCEM_MeritNo = a.ACSTCEM_MeritNo,
                                                ACSTCEM_ALLIndiaRank = a.ACSTCEM_ALLIndiaRank,
                                                ACSTCEM_CategoryRank = a.ACSTCEM_CategoryRank,
                                                ACSTCEM_TotalMaxMarks = a.ACSTCEM_TotalMaxMarks,
                                                ACSTCEM_ObtdMarks = a.ACSTCEM_ObtdMarks,
                                                ACSTCEM_Percentile = a.ACSTCEM_Percentile,
                                                ACSTCEM_Percentage = a.ACSTCEM_Percentage,
                                                AMCEXM_CompetitiveExams = d.AMCEXM_CompetitiveExams

                                            }
                                                          ).ToArray();

                Edata.studentCompSubDetails = (from a in _context.Adm_College_Student_CEMarks_SubjectDMO
                                               from c in _context.Adm_Master_College_StudentDMO
                                               from d in _context.Master_Competitive_AdmExamsClgDMO
                                               from e in _context.Master_CompetitiveExamsSubjectsAdmClgDMO
                                               where (e.AMCEXMSUB_Id == a.AMCEXMSUB_Id && a.AMCEXM_Id == d.AMCEXM_Id && a.AMCST_Id == c.AMCST_Id && c.MI_Id == Edata.MI_Id && a.ACSTCEMS_ActiveFlg == true && c.AMCST_Id == asmccid.FirstOrDefault().AMCST_Id)
                                               select new Adm_College_Student_CEMarks_SubjectDTO
                                               {
                                                   AMCEXM_Id = a.AMCEXM_Id,
                                                   ACSTCEMS_Id = a.ACSTCEMS_Id,
                                                   AMCEXMSUB_Id = a.AMCEXMSUB_Id,
                                                   AMCEXMSUB_MaxMarks = e.AMCEXMSUB_MaxMarks,
                                                   ACSTCEMS_MaxMarks = a.ACSTCEMS_MaxMarks,
                                                   ACSTCEMS_SubjectMarks = a.ACSTCEMS_SubjectMarks,
                                                   AMCEXM_CompetitiveExams = d.AMCEXM_CompetitiveExams,
                                                   AMCEXMSUB_SubjectName = e.AMCEXMSUB_SubjectName

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

        public AdmMasterCollegeStudentDTO checkbiometriccode(AdmMasterCollegeStudentDTO data)
        {
            try
            {
                if (data.AMCST_Id > 0)
                {
                    var checkresult = _context.Adm_Master_College_StudentDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCST_BiometricId == data.AMCST_BiometricId
                    && a.AMCST_Id != data.AMCST_Id).ToList();

                    if (checkresult.Count() > 0)
                    {
                        data.Message = "Duplicate";
                    }
                }
                else
                {
                    var checkresult = _context.Adm_Master_College_StudentDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCST_BiometricId == data.AMCST_BiometricId).ToList();

                    if (checkresult.Count() > 0)
                    {
                        data.Message = "Duplicate";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public AdmMasterCollegeStudentDTO checkrfcardduplicate(AdmMasterCollegeStudentDTO data)
        {
            try
            {
                if (data.AMCST_Id > 0)
                {
                    var checkresult = _context.Adm_Master_College_StudentDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCST_RFCardNo == data.AMCST_RFCardNo
                    && a.AMCST_Id != data.AMCST_Id).ToList();

                    if (checkresult.Count() > 0)
                    {
                        data.Message = "Duplicate";
                    }
                }
                else
                {
                    var checkresult = _context.Adm_Master_College_StudentDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCST_RFCardNo == data.AMCST_RFCardNo).ToList();

                    if (checkresult.Count() > 0)
                    {
                        data.Message = "Duplicate";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }


    }
}