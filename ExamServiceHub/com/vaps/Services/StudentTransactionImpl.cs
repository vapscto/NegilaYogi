using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.MobileApp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class StudentTransactionImpl : Interfaces.StudentTransactionInterface
    {
        DomainModelMsSqlServerContext _db;
        ExamContext _exmstContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public StudentTransactionImpl(DomainModelMsSqlServerContext db, ExamContext exmstContext, UserManager<ApplicationUser> _user)
        {
            _db = db;
            _exmstContext = exmstContext;
            _userManager = _user;
        }
        public StudentTransactionDTO getDetails(StudentTransactionDTO dto)
        {
            try
            {
                var acdYear = _db.AcademicYear.Where(d => d.MI_Id == dto.MI_Id && d.Is_Active == true).Select(d => new AcademicDTO
                { ASMAY_Id = d.ASMAY_Id, ASMAY_Year = d.ASMAY_Year, ASMAY_Order = d.ASMAY_Order }).OrderByDescending(a => a.ASMAY_Order).ToList();
                if (acdYear.Count > 0)
                {
                    dto.academicYear = acdYear.ToArray();
                }
                var clas = _db.School_M_Class.Where(d => d.MI_Id == dto.MI_Id && d.ASMCL_ActiveFlag == true).Select(d => new School_M_ClassDTO
                { ASMCL_Id = d.ASMCL_Id, ASMCL_ClassName = d.ASMCL_ClassName, ASMCL_Order = d.ASMCL_Order }).OrderBy(t => t.ASMCL_Order).ToList();
                if (clas.Count > 0)
                {
                    dto.classList = clas.ToArray();
                }
                var section = _db.School_M_Section.Where(d => d.MI_Id == dto.MI_Id && d.ASMC_ActiveFlag == 1).Select(d => new Section_DTO { ASMS_Id = d.ASMS_Id, ASMC_SectionName = d.ASMC_SectionName, ASMC_Order = d.ASMC_Order }).OrderBy(t => t.ASMC_Order).ToList();
                if (section.Count > 0)
                {
                    dto.sectionList = section.ToArray();
                }

                var exmTerms = _exmstContext.CCE_Exam_M_TermsDMO.Where(d => d.MI_Id == dto.MI_Id && d.ECT_ActiveFlag == true).Select(d => new CCE_Exam_M_TermsDMO { ECT_Id = d.ECT_Id, ECT_TermName = d.ECT_TermName }).ToList();
                if (exmTerms.Count > 0)
                {
                    dto.examTerms = exmTerms.ToArray();
                }

                var skills = _exmstContext.CCE_Master_Life_SkillsDMO.Where(d => d.MI_Id == dto.MI_Id && d.ECS_ActiveFlag == true).Select(d => new CCE_Master_Life_SkillsDMO { ECS_Id = d.ECS_Id, ECS_SkillName = d.ECS_SkillName }).ToList();

                if (skills.Count > 0)
                {
                    dto.skillsList = skills.ToArray();
                }

                var activities = _exmstContext.Exm_CCE_ActivitiesDMO.Where(d => d.MI_Id == dto.MI_Id && d.ECACT_ActiveFlag == true).Select(d => new Exm_CCE_ActivitiesDMO { ECACT_Id = d.ECACT_Id, ECACT_SkillName = d.ECACT_SkillName }).ToList();
                if (activities.Count > 0)
                {
                    dto.activitiesList = activities.ToArray();
                }

                if (dto.stringmobileorportal == "Mobile")
                {
                    List<IVRM_User_MobileApp_Login_Privileges> Staffmobileappprivileges = new List<IVRM_User_MobileApp_Login_Privileges>();
                    Staffmobileappprivileges = _db.IVRM_User_MobileApp_Login_Privileges.Where(t => t.IVRMUL_Id == dto.Userid && t.MI_Id == dto.MI_Id).ToList();

                    if (Staffmobileappprivileges.Count() > 0)
                    {
                        dto.Staffmobileappprivileges = (from Mobilepage in _db.IVRM_MobileApp_Page
                                                        from MobileRolePrivileges in _db.IVRM_Role_MobileApp_Privileges
                                                        from UserRolePrivileges in _db.IVRM_User_MobileApp_Login_Privileges
                                                        where (MobileRolePrivileges.MI_ID == UserRolePrivileges.MI_Id
                                                        && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id
                                                        && Mobilepage.IVRMMAP_Id == UserRolePrivileges.IVRMMAP_Id && MobileRolePrivileges.IVRMRT_Id == dto.Roleid
                                                        && MobileRolePrivileges.MI_ID == dto.MI_Id && UserRolePrivileges.IVRMUL_Id == dto.Userid)
                                                        select new StudentTransactionDTO
                                                        {
                                                            Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                            Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                            Pageurl = Mobilepage.IVRMMAP_AppPageURL,
                                                            IVRMRMAP_Id = MobileRolePrivileges.IVRMRMAP_Id,
                                                            IVRMMAP_AddFlg = UserRolePrivileges.IVRMUMALP_AddFlg,
                                                            IVRMMAP_UpdateFlg = UserRolePrivileges.IVRMUMALP_UpdateFlg,
                                                            IVRMMAP_DeleteFlg = UserRolePrivileges.IVRMUMALP_DeleteFlg
                                                        }).OrderBy(d => d.IVRMRMAP_Id).ToArray();

                        dto.mobileprivileges = "true";
                    }
                    else
                    {
                        dto.mobileprivileges = "false";
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public StudentTransactionDTO getStudentList(StudentTransactionDTO dto)
        {
            try
            {
                List<StudentTransactionDTO> list = new List<StudentTransactionDTO>();

                var students = (from m in _exmstContext.Adm_M_Student
                                from n in _exmstContext.School_Adm_Y_Student
                                where m.AMST_Id == n.AMST_Id && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1
                                && m.MI_Id == dto.MI_Id && n.ASMAY_Id == dto.ASMAY_Id && n.ASMCL_Id == dto.ASMCL_Id && n.ASMS_Id == dto.ASMS_Id
                                select new StudentTransactionDTO
                                {
                                    studentName = m.AMST_FirstName + (string.IsNullOrEmpty(m.AMST_MiddleName) ? "" : ' ' + m.AMST_MiddleName) + 
                                    (string.IsNullOrEmpty(m.AMST_LastName) ? "" : ' ' + m.AMST_LastName),
                                    regNumber = m.AMST_RegistrationNo,
                                    rollNumber = n.AMAY_RollNo,
                                    AMST_Id = n.AMST_Id
                                }).Distinct().OrderBy(a => a.rollNumber).ToList();

                if (students.Count > 0)
                {
                    dto.studentList = students.ToArray();
                }

                if (dto.Skills.Equals("Activities"))
                {
                    var areas = (from m in _exmstContext.Exm_CCE_ActivitiesDMO
                                 from n in _exmstContext.EXM_CCE_Activities_AREADMO
                                 from o in _exmstContext.Exm_CCE_Activities_AREA_MappingDMO
                                 where m.ECACT_Id == o.ECACT_Id && n.ECACTA_Id == o.ECACTA_Id
                                 && o.MI_Id == dto.MI_Id && o.ECACTAM_ActiveFlag == true && o.ECACT_Id == dto.ECACT_Id && o.ASMAY_Id == dto.ASMAY_Id
                                 && o.ECACTA_Id == dto.ECACTA_Id
                                 select new StudentTransactionDTO
                                 {
                                     ECACT_Id = m.ECACT_Id,
                                     ECACTA_Id = n.ECACTA_Id,
                                     areaName = n.ECACTA_SkillArea,
                                     EMGR_Id = o.EMGR_Id,
                                     ECACTA_SkillOrder = n.ECACTA_SkillOrder
                                 }).OrderBy(a => a.ECACTA_SkillOrder).ToList();

                    if (areas.Count > 0)
                    {
                        dto.areasList = areas.ToArray();
                        var ecacta = areas.Select(d => d.ECACTA_Id).ToList();

                        var amstIds = students.Select(d => d.AMST_Id).ToList();

                        var dd_activites = _exmstContext.Exm_CCE_Activities_TransactionDMO.Where(d => d.MI_Id == dto.MI_Id && d.ASMAY_Id == dto.ASMAY_Id
                        && d.ASMCL_Id == dto.ASMCL_Id && d.ASMS_Id == dto.ASMS_Id && d.ECACT_Id == dto.ECACT_Id && ecacta.Contains(d.ECACTA_Id) 
                        && amstIds.Contains(d.AMST_Id)).ToList();

                        if (dto.exam_termwise_flag == "ExamWise")
                        {
                            dd_activites = dd_activites.Where(a => a.EME_Id == dto.EME_Id).ToList();
                        }
                        else
                        {
                            dd_activites = dd_activites.Where(a => a.ECT_Id == dto.ECT_Id).ToList();
                        }

                        var query = dd_activites.Where(d => d.MI_Id == dto.MI_Id && d.ASMAY_Id == dto.ASMAY_Id 
                        && d.ASMCL_Id == dto.ASMCL_Id && d.ASMS_Id == dto.ASMS_Id && d.ECACT_Id == dto.ECACT_Id 
                        && ecacta.Contains(d.ECACTA_Id) && amstIds.Contains(d.AMST_Id)).Select(d => new StudentTransactionDTO
                        {
                            AMST_Id = d.AMST_Id,
                            ECSACTT_Score = d.ECSACTT_Score,
                            ECACTA_Id = d.ECACTA_Id,
                            EMGR_Id = d.EMGR_Id,
                            ECSACTT_Id = d.ECSACTT_Id
                        }).ToArray();

                        dto.getsaveddata = query.ToArray();
                    }
                    if (list.Count > 0)
                    {
                        dto.studentList = list.ToArray();
                    }
                }

                else if (dto.Skills.Equals("Skills"))
                {
                    var areas = (from m in _exmstContext.CCE_Master_Life_SkillsDMO
                                 from n in _exmstContext.CCE_Master_Life_Skill_AreasDMO
                                 from o in _exmstContext.CCE_Master_Life_Skill_Areas_MappingDMO
                                 where m.ECS_Id == o.ECS_Id && n.ECSA_Id == o.ECSA_Id
                                 && o.MI_Id == dto.MI_Id && o.ECSAM_ActiveFlag == true && o.ECS_Id == dto.ECS_Id && o.ASMAY_Id == dto.ASMAY_Id
                                 && o.ECSA_Id == dto.ECSA_Id
                                 select new StudentTransactionDTO
                                 {
                                     ECS_Id = m.ECS_Id,
                                     ECSA_Id = n.ECSA_Id,
                                     areaName = n.ECSA_SkillArea,
                                     EMGR_Id = o.EMGR_Id,
                                     ECSA_SkillOrder = n.ECSA_SkillOrder
                                 }).OrderBy(a => a.ECSA_SkillOrder).ToList();

                    if (areas.Count > 0)
                    {
                        dto.areasList = areas.ToArray();

                        var ecacta = areas.Select(d => d.ECSA_Id).ToList();
                        var amstIds = students.Select(d => d.AMST_Id).ToList();

                        var dd_skills = _exmstContext.Exm_CCE_SKILLS_TransactionDMO.Where(d => d.MI_Id == dto.MI_Id && d.ASMAY_Id == dto.ASMAY_Id
                        && d.ASMCL_Id == dto.ASMCL_Id && d.ASMS_Id == dto.ASMS_Id && d.ECS_Id == dto.ECS_Id && ecacta.Contains(d.ECSA_Id)
                        && amstIds.Contains(d.AMST_Id)).ToList();

                        if (dto.exam_termwise_flag == "ExamWise")
                        {
                            dd_skills = dd_skills.Where(a => a.EME_Id == dto.EME_Id).ToList();
                        }
                        else
                        {
                            dd_skills = dd_skills.Where(a => a.ECT_Id == dto.ECT_Id).ToList();
                        }

                        var query = dd_skills.Where(d => d.MI_Id == dto.MI_Id && d.ASMAY_Id == dto.ASMAY_Id && d.ASMCL_Id == dto.ASMCL_Id
                        && d.ASMS_Id == dto.ASMS_Id  && d.ECS_Id == dto.ECS_Id && ecacta.Contains(d.ECSA_Id) && amstIds.Contains(d.AMST_Id)).Select(d => new StudentTransactionDTO
                        {
                            AMST_Id = d.AMST_Id,
                            ECST_Score = d.ECST_Score,
                            ECSA_Id = d.ECSA_Id,
                            EMGR_Id = d.EMGR_Id,
                            ECST_Id = d.ECST_Id
                        }).ToArray();

                        dto.getsaveddata = query;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public StudentTransactionDTO onchangeyear(StudentTransactionDTO dto)
        {
            try
            {
                var userid = getuserid(dto.UserName);

                var check_rolename = (from a in _exmstContext.MasterRoleType
                                      where (a.IVRMRT_Id == dto.Roleid)
                                      select new JSHSExamReportsDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }
                                   ).ToList();

                var empcode_check = (from a in _exmstContext.Staff_User_Login
                                     where (a.MI_Id == dto.MI_Id && a.Id.Equals(dto.Userid))
                                     select new JSHSExamReportsDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count() > 0)
                {
                    var check_classteacher = _exmstContext.ClassTeacherMappingDMO.Where(a => a.MI_Id == dto.MI_Id && a.ASMAY_Id == dto.ASMAY_Id
                    && a.IMCT_ActiveFlag == true && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code).ToList();


                    if (check_classteacher.Count() > 0)
                    {

                        dto.classList = (from a in _exmstContext.ClassTeacherMappingDMO
                                         from b in _exmstContext.AdmissionClass
                                         from c in _exmstContext.AcademicYear
                                         where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == dto.ASMAY_Id
                                         && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.IMCT_ActiveFlag == true && b.ASMCL_ActiveFlag == true)
                                         select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();

                    }

                }
                else
                {
                    dto.classList = (from a in _exmstContext.Exm_Category_ClassDMO
                                     from b in _exmstContext.AdmissionClass
                                     where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == dto.MI_Id && a.ASMAY_Id == dto.ASMAY_Id && a.ECAC_ActiveFlag == true)
                                     select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public StudentTransactionDTO onchangeclass(StudentTransactionDTO dto)
        {
            try
            {
                var check_rolename = (from a in _exmstContext.MasterRoleType
                                      where (a.IVRMRT_Id == dto.Roleid)
                                      select new JSHSExamReportsDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var empcode_check = (from a in _exmstContext.Staff_User_Login
                                     where (a.MI_Id == dto.MI_Id && a.Id.Equals(dto.Userid))
                                     select new JSHSExamReportsDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count() > 0)
                {
                    var check_classteacher = _exmstContext.ClassTeacherMappingDMO.Where(a => a.MI_Id == dto.MI_Id && a.ASMAY_Id == dto.ASMAY_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code).ToList();

                    if (check_classteacher.Count() > 0)
                    {

                        dto.sectionList = (from a in _exmstContext.ClassTeacherMappingDMO
                                           from b in _exmstContext.School_M_Section
                                           from c in _exmstContext.AcademicYear
                                           from d in _exmstContext.AdmissionClass
                                           where (a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == dto.ASMAY_Id
                                           && a.ASMCL_Id == dto.ASMCL_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.IMCT_ActiveFlag == true
                                           && b.ASMC_ActiveFlag == 1)
                                           select b).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                    }
                }
                else
                {
                    dto.sectionList = (from a in _exmstContext.Exm_Category_ClassDMO
                                       from b in _exmstContext.AdmissionClass
                                       from c in _exmstContext.School_M_Section
                                       where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == c.ASMS_Id && a.MI_Id == dto.MI_Id && a.ASMAY_Id == dto.ASMAY_Id
                                       && a.ASMCL_Id == dto.ASMCL_Id && a.ECAC_ActiveFlag == true)
                                       select c).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public StudentTransactionDTO onchangesection(StudentTransactionDTO dto)
        {
            try
            {
                var getemcaid = _exmstContext.Exm_Category_ClassDMO.Where(a => a.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id && a.ASMS_Id == dto.ASMS_Id && a.ASMAY_Id == dto.ASMAY_Id && a.ECAC_ActiveFlag == true).ToList();

                var geteyceid = _exmstContext.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == dto.MI_Id && a.ASMAY_Id == dto.ASMAY_Id && a.EYC_ActiveFlg == true
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).ToList();


                if (dto.exam_termwise_flag == "ExamWise")
                {
                    dto.exammaster = (from a in _exmstContext.Exm_Yearly_Category_ExamsDMO
                                      from b in _exmstContext.masterexam
                                      where (a.EME_Id == b.EME_Id && a.EYCE_ActiveFlg == true && a.EYC_Id == geteyceid.FirstOrDefault().EYC_Id)
                                      select b).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
                }
                else
                {
                    dto.examTerms = _exmstContext.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == dto.MI_Id && a.ASMAY_Id == dto.ASMAY_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public StudentTransactionDTO onchangeskills(StudentTransactionDTO dto)
        {
            try
            {
                var areas = (from m in _exmstContext.CCE_Master_Life_SkillsDMO
                             from n in _exmstContext.CCE_Master_Life_Skill_AreasDMO
                             from o in _exmstContext.CCE_Master_Life_Skill_Areas_MappingDMO
                             where m.ECS_Id == o.ECS_Id && n.ECSA_Id == o.ECSA_Id
                             && o.MI_Id == dto.MI_Id && o.ECSAM_ActiveFlag == true && o.ECS_Id == dto.ECS_Id && o.ASMAY_Id == dto.ASMAY_Id
                             select new StudentTransactionDTO
                             {
                                 ECS_Id = m.ECS_Id,
                                 ECSA_Id = n.ECSA_Id,
                                 areaName = n.ECSA_SkillArea,
                                 EMGR_Id = o.EMGR_Id,
                                 ECSA_SkillOrder = n.ECSA_SkillOrder
                             }).OrderBy(a => a.ECSA_SkillOrder).ToList();

                dto.areasList = areas.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public StudentTransactionDTO onchangeactivites(StudentTransactionDTO dto)
        {
            try
            {
                var areas = (from m in _exmstContext.Exm_CCE_ActivitiesDMO
                             from n in _exmstContext.EXM_CCE_Activities_AREADMO
                             from o in _exmstContext.Exm_CCE_Activities_AREA_MappingDMO
                             where m.ECACT_Id == o.ECACT_Id && n.ECACTA_Id == o.ECACTA_Id
                             && o.MI_Id == dto.MI_Id && o.ECACTAM_ActiveFlag == true && o.ECACT_Id == dto.ECACT_Id && o.ASMAY_Id == dto.ASMAY_Id
                             select new StudentTransactionDTO
                             {
                                 ECACT_Id = m.ECACT_Id,
                                 ECACTA_Id = n.ECACTA_Id,
                                 areaName = n.ECACTA_SkillArea,
                                 EMGR_Id = o.EMGR_Id,
                                 ECACTA_SkillOrder = n.ECACTA_SkillOrder
                             }).OrderBy(a => a.ECACTA_SkillOrder).ToList();


                dto.areasList = areas.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public StudentTransactionDTO save(StudentTransactionDTO obj)
        {
            try
            {
                if (obj.SelectedStudentScore.Length > 0)
                {
                    if (obj.Skills.Equals("Skills"))
                    {
                        List<long> amstid = new List<long>();

                        foreach (var c in obj.SelectedStudentScore)
                        {
                            amstid.Add(c.AMST_Id);
                        }

                        var resultdelete = _exmstContext.Exm_CCE_SKILLS_TransactionDMO.Where(a => !amstid.Contains(a.AMST_Id) && a.ASMAY_Id == obj.ASMAY_Id
                        && a.ASMCL_Id == obj.ASMCL_Id && a.ASMS_Id == obj.ASMS_Id && a.ECS_Id == obj.ECS_Id && a.ECSA_Id == obj.ECSA_Id).ToArray();

                        if (obj.exam_termwise_flag == "ExamWise")
                        {
                            resultdelete = resultdelete.Where(a => a.EME_Id == obj.EME_Id).ToArray();
                        }
                        else
                        {
                            resultdelete = resultdelete.Where(a => a.ECT_Id == obj.ECT_Id).ToArray();
                        }                       

                        foreach (var d in resultdelete)
                        {
                            _exmstContext.Remove(d);
                        }

                        foreach (StudentTransactionDTO item in obj.SelectedStudentScore)
                        {
                            if (item.ECST_Id > 0)
                            {
                                var update = _exmstContext.Exm_CCE_SKILLS_TransactionDMO.Single(d => d.ECST_Id == item.ECST_Id);
                                update.ECST_Score = item.ECSACTT_Score;
                                update.ECSA_Id = Convert.ToInt32(item.areaName);
                                update.UpdatedDate = DateTime.Now;
                                _exmstContext.Update(update);
                            }
                            else
                            {
                                Exm_CCE_SKILLS_TransactionDMO mapp = new Exm_CCE_SKILLS_TransactionDMO();
                                mapp.MI_Id = obj.MI_Id;
                                mapp.ASMAY_Id = obj.ASMAY_Id;
                                mapp.ASMCL_Id = obj.ASMCL_Id;
                                mapp.ASMS_Id = obj.ASMS_Id;
                                mapp.AMST_Id = item.AMST_Id;
                                mapp.ECS_Id = obj.ECS_Id;
                                mapp.CreatedDate = DateTime.Now;
                                mapp.ECSA_Id = Convert.ToInt32(item.areaName);
                                mapp.ECT_Id = obj.exam_termwise_flag== "ExamWise" ? (int?)null : obj.ECT_Id;
                                mapp.EME_Id = obj.exam_termwise_flag== "ExamWise" ? obj.EME_Id  : (int?)null;
                                mapp.ECST_ActiveFlag = true;
                                mapp.ECST_Score = item.ECSACTT_Score;
                                mapp.EMGR_Id = item.EMGR_Id;
                                mapp.UpdatedDate = DateTime.Now;
                                _exmstContext.Add(mapp);
                            }
                        }
                        var flag = _exmstContext.SaveChanges();
                        if (flag > 0)
                        {
                            obj.returnVal = "saved";
                        }
                        else
                        {
                            obj.returnVal = "savingFailed";
                        }
                    }

                    else if (obj.Skills.Equals("Activities"))
                    {
                        List<long> amstidd = new List<long>();

                        foreach (var c in obj.SelectedStudentScore)
                        {
                            amstidd.Add(c.AMST_Id);
                        }

                        var resultdelete = _exmstContext.Exm_CCE_Activities_TransactionDMO.Where(a => !amstidd.Contains(a.AMST_Id) && a.ASMAY_Id == obj.ASMAY_Id
                        && a.ASMCL_Id == obj.ASMCL_Id && a.ASMS_Id == obj.ASMS_Id && a.ECT_Id == obj.ECT_Id && a.ECACT_Id == obj.ECACT_Id
                        && a.ECACTA_Id == obj.ECACTA_Id).ToArray();

                        foreach (var d in resultdelete)
                        {
                            _exmstContext.Remove(d);
                        }

                        foreach (StudentTransactionDTO item in obj.SelectedStudentScore)
                        {
                            if (item.ECSACTT_Id > 0)
                            {
                                var result = _exmstContext.Exm_CCE_Activities_TransactionDMO.Single(d => d.ECSACTT_Id == item.ECSACTT_Id);
                                result.ECSACTT_Score = item.ECSACTT_Score;
                                result.ECACTA_Id = Convert.ToInt32(item.areaName);
                                result.UpdatedDate = DateTime.Now;
                                _exmstContext.Update(result);
                            }
                            else
                            {
                                Exm_CCE_Activities_TransactionDMO mapp = new Exm_CCE_Activities_TransactionDMO();
                                mapp.MI_Id = obj.MI_Id;
                                mapp.ASMAY_Id = obj.ASMAY_Id;
                                mapp.ASMCL_Id = obj.ASMCL_Id;
                                mapp.ASMS_Id = obj.ASMS_Id;
                                mapp.AMST_Id = item.AMST_Id;
                                mapp.ECT_Id = obj.exam_termwise_flag == "ExamWise" ? (int?)null : obj.ECT_Id;
                                mapp.EME_Id = obj.exam_termwise_flag == "ExamWise" ? obj.EME_Id : (int?)null;
                                mapp.ECACT_Id = obj.ECACT_Id;
                                mapp.ECACTA_Id = Convert.ToInt32(item.areaName);
                                mapp.ECSACTT_Score = item.ECSACTT_Score;
                                mapp.EMGR_Id = item.EMGR_Id;
                                mapp.ECSACTT_ActiveFlag = true;
                                mapp.UpdatedDate = DateTime.Now;
                                mapp.CreatedDate = DateTime.Now;
                                _exmstContext.Add(mapp);
                            }

                        }
                        var flag = _exmstContext.SaveChanges();
                        if (flag > 0)
                        {
                            obj.returnVal = "saved";
                        }
                        else
                        {
                            obj.returnVal = "savingFailed";
                        }
                    }
                }
                else
                {
                    //Something Went Wrong.
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public StudentTransactionDTO onchangeactivitesskillflag(StudentTransactionDTO data)
        {
            try
            {
                if (data.Skills == "Skills")
                {
                    var skills = _exmstContext.CCE_Master_Life_SkillsDMO.Where(d => d.MI_Id == data.MI_Id && d.ECS_ActiveFlag == true).Select(d => new CCE_Master_Life_SkillsDMO { ECS_Id = d.ECS_Id, ECS_SkillName = d.ECS_SkillName }).ToList();


                    data.skillsList = (from a in _exmstContext.CCE_Master_Life_SkillsDMO
                                       from b in _exmstContext.CCE_Master_Life_Skill_Areas_MappingDMO
                                       where (a.ECS_Id == b.ECS_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ECSAM_ActiveFlag == true)
                                       select new StudentTransactionDTO
                                       {
                                           ECS_Id = a.ECS_Id,
                                           ECS_SkillName = a.ECS_SkillName
                                       }).Distinct().OrderBy(a => a.ECS_SkillName).ToArray();
                }
                else if (data.Skills == "Activities")
                {
                    data.activitiesList = (from a in _exmstContext.Exm_CCE_ActivitiesDMO
                                           from b in _exmstContext.Exm_CCE_Activities_AREA_MappingDMO
                                           where (a.ECACT_Id == b.ECACT_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ECACTAM_ActiveFlag == true)
                                           select new StudentTransactionDTO
                                           {
                                               ECACT_Id = a.ECACT_Id,
                                               ECACT_SkillName = a.ECACT_SkillName
                                           }).Distinct().OrderBy(a => a.ECACT_SkillName).ToArray();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<string> getuserid(string data)
        {
            string username = data.ToString();
            string id = "";
            ApplicationUser user = new ApplicationUser();
            user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                id = user.Id.ToString(); ;
            }
            return id;
        }

        //
        public StudentTransactionDTO getmobiletList(StudentTransactionDTO dto)
        {
            try
            {
                string ECSA_Id = "0";
                string ECACTA_Id = "0";
                if (dto.Skills.Equals("Activities"))
                {
                    var areas = (from m in _exmstContext.Exm_CCE_ActivitiesDMO
                                 from n in _exmstContext.EXM_CCE_Activities_AREADMO
                                 from o in _exmstContext.Exm_CCE_Activities_AREA_MappingDMO
                                 where m.ECACT_Id == o.ECACT_Id && n.ECACTA_Id == o.ECACTA_Id
                                 && o.MI_Id == dto.MI_Id && o.ECACTAM_ActiveFlag == true && o.ECACT_Id == dto.ECACT_Id && o.ASMAY_Id == dto.ASMAY_Id
                                 && o.ECACTA_Id == dto.ECACTA_Id
                                 select new StudentTransactionDTO
                                 {
                                     ECACT_Id = m.ECACT_Id,
                                     ECACTA_Id = n.ECACTA_Id,
                                     areaName = n.ECACTA_SkillArea,
                                     EMGR_Id = o.EMGR_Id,
                                     ECACTA_SkillOrder = n.ECACTA_SkillOrder
                                 }).OrderBy(a => a.ECACTA_SkillOrder).ToList();

                    if (areas.Count > 0)
                    {
                        dto.areasList = areas.ToArray();
                      
                        foreach (var d in areas)
                        {
                            ECACTA_Id = ECACTA_Id + "," + d.ECACTA_Id;

                        }                                           
                    }
                  
                }

                else if (dto.Skills.Equals("Skills"))
                {
                    var areas = (from m in _exmstContext.CCE_Master_Life_SkillsDMO
                                 from n in _exmstContext.CCE_Master_Life_Skill_AreasDMO
                                 from o in _exmstContext.CCE_Master_Life_Skill_Areas_MappingDMO
                                 where m.ECS_Id == o.ECS_Id && n.ECSA_Id == o.ECSA_Id
                                 && o.MI_Id == dto.MI_Id && o.ECSAM_ActiveFlag == true && o.ECS_Id == dto.ECS_Id && o.ASMAY_Id == dto.ASMAY_Id
                                 && o.ECSA_Id == dto.ECSA_Id
                                 select new StudentTransactionDTO
                                 {
                                     ECS_Id = m.ECS_Id,
                                     ECSA_Id = n.ECSA_Id,
                                     areaName = n.ECSA_SkillArea,
                                     EMGR_Id = o.EMGR_Id,
                                     ECSA_SkillOrder = n.ECSA_SkillOrder
                                 }).OrderBy(a => a.ECSA_SkillOrder).ToList();

                    if (areas.Count > 0)
                    {
                        dto.areasList = areas.ToArray();                        
                       foreach(var d in areas)
                        {
                            ECSA_Id = ECSA_Id + "," + d.ECSA_Id;
                        }                     
                 
                    }
                }
                //exec

                using (var cmd = _exmstContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exm_Students_SkillsActivities_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = dto.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = dto.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = dto.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@ECSA_Id", SqlDbType.VarChar) { Value = ECSA_Id });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id", SqlDbType.VarChar) { Value = dto.ECT_Id });
                    cmd.Parameters.Add(new SqlParameter("@ECACTA_Id", SqlDbType.VarChar) { Value = ECACTA_Id });
                    cmd.Parameters.Add(new SqlParameter("@ECACT_Id", SqlDbType.VarChar) { Value = dto.ECACT_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = dto.EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar) { Value = dto.Skills });
                    cmd.Parameters.Add(new SqlParameter("@exam_termwise_flag", SqlDbType.VarChar) { Value = dto.exam_termwise_flag });//
                    cmd.Parameters.Add(new SqlParameter("@ECS_Id", SqlDbType.VarChar) { Value = dto.ECS_Id });
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
                        dto.studentList = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
    }
}
