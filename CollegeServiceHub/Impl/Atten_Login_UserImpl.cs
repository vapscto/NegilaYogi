using AutoMapper;
using CollegeServiceHub.Interface;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class Atten_Login_UserImpl : Atten_Login_UserInterface
    {
        public ClgAdmissionContext _ClgAdmissionContext;
        ILogger<Atten_Login_UserImpl> _logbranch;
        public Atten_Login_UserImpl(ClgAdmissionContext ClgAdmissionContext, ILogger<Atten_Login_UserImpl> log)
        {
            _ClgAdmissionContext = ClgAdmissionContext;
            _logbranch = log;
        }
        public Atten_Login_UserDTO getalldetails(Atten_Login_UserDTO data)
        {
            try
            {
                data.yearlist = _ClgAdmissionContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.stafflist = (from a in _ClgAdmissionContext.Staff_User_Login
                                  from b in _ClgAdmissionContext.HR_Master_Employee_DMO
                                  where (a.Emp_Code == b.HRME_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                  select new Atten_Login_UserDTO
                                  {
                                      HRME_Id = b.HRME_Id,
                                      HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                  }).Distinct().ToArray();

                data.courselist = _ClgAdmissionContext.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                data.branchlist = _ClgAdmissionContext.ClgMasterBranchDMO.Where(a => a.MI_Id == data.MI_Id && a.AMB_ActiveFlag).Distinct().OrderBy(a => a.AMB_Order).ToArray();
                data.semisterlist = _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
                data.sectionlist = _ClgAdmissionContext.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag).Distinct().OrderBy(a => a.ACMS_Order).ToArray();
                data.subjectlist = _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1 && a.ISMS_AttendanceFlag==true).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();

                data.saveddata = (from a in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                  from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                  from c in _ClgAdmissionContext.AcademicYear
                                  from d in _ClgAdmissionContext.HR_Master_Employee_DMO
                                  from e in _ClgAdmissionContext.MasterCourseDMO
                                  from f in _ClgAdmissionContext.ClgMasterBranchDMO
                                  from g in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                  from h in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                                      //  from i in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                                  where (a.MI_Id == data.MI_Id && c.MI_Id == a.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && g.MI_Id == a.MI_Id && h.MI_Id == a.MI_Id && b.ACALU_Id == a.ACALU_Id && c.Is_Active && c.ASMAY_Id == a.ASMAY_Id && d.HRME_ActiveFlag && d.HRME_Id == a.HRME_Id && e.AMCO_ActiveFlag && e.AMCO_Id == b.AMCO_Id && f.AMB_ActiveFlag && f.AMB_Id == b.AMB_Id && g.AMSE_ActiveFlg && g.AMSE_Id == b.AMSE_Id && h.ACMS_ActiveFlag && h.ACMS_Id == b.ACMS_Id)//&& i.MI_Id == a.MI_Id && i.ISMS_ActiveFlag == 1 && i.ISMS_AttendanceFlag && i.ISMS_Id==b.ISMS_Id
                                  select new Atten_Login_UserDTO
                                  {
                                      ACALU_Id = a.ACALU_Id,
                                      ASMAY_Id = a.ASMAY_Id,
                                      HRME_Id = a.HRME_Id,
                                      //ACALD_Id = b.ACALD_Id,
                                      AMCO_Id = b.AMCO_Id,
                                      AMB_Id = b.AMB_Id,
                                      AMSE_Id = b.AMSE_Id,
                                      ACMS_Id = b.ACMS_Id,
                                      //ISMS_Id = b.ISMS_Id,
                                      // ACALD_ActiveFlag = b.ACALD_ActiveFlag,
                                      ASMAY_Year = c.ASMAY_Year,
                                      HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                      AMCO_CourseName = e.AMCO_CourseName,
                                      AMB_BranchName = f.AMB_BranchName,
                                      AMSE_SEMName = g.AMSE_SEMName,
                                      ACMS_SectionName = h.ACMS_SectionName,
                                      // ISMS_SubjectName=i.ISMS_SubjectName
                                  }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Atten_Login_User  getalldetails :" + ex.Message);
            }
            return data;
        }

        public Atten_Login_UserDTO get_courses(Atten_Login_UserDTO data)
        {
            try
            {

                data.courselist = (from a in _ClgAdmissionContext.MasterCourseDMO
                                   from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Atten_Login_User  get_courses :" + ex.Message);
            }
            return data;
        }
        public Atten_Login_UserDTO get_branches(Atten_Login_UserDTO data)
        {
            try
            {
                var branchlist = (from a in _ClgAdmissionContext.ClgMasterBranchDMO
                                  from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                  from c in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                  select new ClgMasterBranchDMO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_BranchCode = a.AMB_BranchCode,
                                      AMB_BranchInfo = a.AMB_BranchInfo,
                                      AMB_BranchType = a.AMB_BranchType,
                                      AMB_StudentCapacity = a.AMB_StudentCapacity,
                                      AMB_Order = a.AMB_Order,
                                      AMB_AidedUnAided = a.AMB_AidedUnAided
                                  }).Distinct().ToList();
                data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();

            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Atten_Login_User  get_branches :" + ex.Message);
            }
            return data;
        }
        public Atten_Login_UserDTO get_semisters(Atten_Login_UserDTO data)
        {
            try
            {
                var semisterlist = (from a in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                    from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                    from c in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _ClgAdmissionContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                    select new CLG_Adm_Master_SemesterDMO
                                    {
                                        AMSE_Id = a.AMSE_Id,
                                        AMSE_SEMName = a.AMSE_SEMName,
                                        AMSE_SEMInfo = a.AMSE_SEMInfo,
                                        AMSE_SEMCode = a.AMSE_SEMCode,
                                        AMSE_SEMTypeFlag = a.AMSE_SEMTypeFlag,
                                        AMSE_SEMOrder = a.AMSE_SEMOrder,
                                        AMSE_Year = a.AMSE_Year,
                                        AMSE_EvenOdd = a.AMSE_EvenOdd
                                    }).Distinct().ToList();
                data.semisterlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();

            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Atten_Login_User  get_semisters :" + ex.Message);
            }
            return data;
        }


        public Atten_Login_UserDTO savedata(Atten_Login_UserDTO data)
        {

            try
            {
                var results = _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.HRME_Id == data.HRME_Id).ToList();

                if (results.Count == 0)
                {
                    Adm_College_Atten_Login_UserDMO obj_p = new Adm_College_Atten_Login_UserDMO();
                    obj_p.MI_Id = data.MI_Id;
                    obj_p.ASMAY_Id = data.ASMAY_Id;
                    obj_p.HRME_Id = data.HRME_Id;
                    obj_p.CreatedDate = DateTime.Now;
                    obj_p.UpdatedDate = DateTime.Now;
                    _ClgAdmissionContext.Add(obj_p);
                    data.ACALU_Id = obj_p.ACALU_Id;
                }
                else if (results.Count == 1)
                {
                    data.ACALU_Id = _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.HRME_Id == data.HRME_Id).ACALU_Id;
                }
                foreach (var x in data.Details)
                {
                    var result321 = _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO.Where(t => t.AMCO_Id == x.AMCO_Id && t.AMB_Id == x.AMB_Id && t.AMSE_Id == x.AMSE_Id && t.ACMS_Id == x.ACMS_Id && t.ACALU_Id == data.ACALU_Id).ToList();
                    if (result321.Count() > 0)
                    {
                        foreach (var res in result321)
                        {
                            res.ACALD_ActiveFlag = false;
                            res.UpdatedDate = DateTime.Now;
                            _ClgAdmissionContext.Update(res);
                        }
                    }
                    foreach (var sub_id in x.ISMS_Ids)
                    {
                        var child_objs = _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO.Where(t => t.AMCO_Id == x.AMCO_Id && t.AMB_Id == x.AMB_Id && t.AMSE_Id == x.AMSE_Id && t.ACMS_Id == x.ACMS_Id && t.ACALU_Id == data.ACALU_Id && t.ISMS_Id == sub_id).ToList();
                        if (child_objs.Count == 1)
                        {
                            child_objs[0].ACALD_ActiveFlag = true;
                            child_objs[0].UpdatedDate = DateTime.Now;
                            _ClgAdmissionContext.Update(child_objs[0]);
                        }
                        else if (child_objs.Count == 0)
                        {
                            Adm_College_Atten_Login_DetailsDMO obj_c = new Adm_College_Atten_Login_DetailsDMO();
                            obj_c.ACALU_Id = data.ACALU_Id;
                            obj_c.AMCO_Id = x.AMCO_Id;
                            obj_c.AMB_Id = x.AMB_Id;
                            obj_c.AMSE_Id = x.AMSE_Id;
                            obj_c.ACMS_Id = x.ACMS_Id;
                            obj_c.ISMS_Id = sub_id;
                            obj_c.ACALD_ActiveFlag = true;
                            obj_c.CreatedDate = DateTime.Now;
                            obj_c.UpdatedDate = DateTime.Now;
                            _ClgAdmissionContext.Add(obj_c);
                        }
                    }
                }
                var contactExists = _ClgAdmissionContext.SaveChanges();
                if (contactExists >= 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.saveddata = (from a in _ClgAdmissionContext.Adm_College_Atten_Login_UserDMO
                                  from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                  from c in _ClgAdmissionContext.AcademicYear
                                  from d in _ClgAdmissionContext.HR_Master_Employee_DMO
                                  from e in _ClgAdmissionContext.MasterCourseDMO
                                  from f in _ClgAdmissionContext.ClgMasterBranchDMO
                                  from g in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                  from h in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                                      //  from i in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                                  where (a.MI_Id == data.MI_Id && c.MI_Id == a.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && g.MI_Id == a.MI_Id && h.MI_Id == a.MI_Id && b.ACALU_Id == a.ACALU_Id && c.Is_Active && c.ASMAY_Id == a.ASMAY_Id && d.HRME_ActiveFlag && d.HRME_Id == a.HRME_Id && e.AMCO_ActiveFlag && e.AMCO_Id == b.AMCO_Id && f.AMB_ActiveFlag && f.AMB_Id == b.AMB_Id && g.AMSE_ActiveFlg && g.AMSE_Id == b.AMSE_Id && h.ACMS_ActiveFlag && h.ACMS_Id == b.ACMS_Id)//&& i.MI_Id == a.MI_Id && i.ISMS_ActiveFlag == 1 && i.ISMS_AttendanceFlag && i.ISMS_Id==b.ISMS_Id
                                  select new Atten_Login_UserDTO
                                  {
                                      ACALU_Id = a.ACALU_Id,
                                      ASMAY_Id = a.ASMAY_Id,
                                      HRME_Id = a.HRME_Id,
                                      //  ACALD_Id = b.ACALD_Id,
                                      AMCO_Id = b.AMCO_Id,
                                      AMB_Id = b.AMB_Id,
                                      AMSE_Id = b.AMSE_Id,
                                      ACMS_Id = b.ACMS_Id,
                                      //ISMS_Id = b.ISMS_Id,
                                      // ACALD_ActiveFlag = b.ACALD_ActiveFlag,
                                      ASMAY_Year = c.ASMAY_Year,
                                      HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                      AMCO_CourseName = e.AMCO_CourseName,
                                      AMB_BranchName = f.AMB_BranchName,
                                      AMSE_SEMName = g.AMSE_SEMName,
                                      ACMS_SectionName = h.ACMS_SectionName,
                                      // ISMS_SubjectName=i.ISMS_SubjectName
                                  }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Atten_Login_User savedata :" + ex.Message);
            }
            return data;
        }

        public Atten_Login_UserDTO view_subjects(Atten_Login_UserDTO data)
        {
            try
            {
                data.subjectlist = (from a in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                                    from b in _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO
                                    where (a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1 && a.ISMS_AttendanceFlag && b.ACALU_Id == data.ACALU_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id && b.ACMS_Id == data.ACMS_Id && a.ISMS_Id == b.ISMS_Id)
                                    select new Atten_Login_UserDTO
                                    {
                                        ACALU_Id = b.ACALU_Id,
                                        ACALD_Id = b.ACALD_Id,
                                        AMCO_Id = b.AMCO_Id,
                                        AMB_Id = b.AMB_Id,
                                        AMSE_Id = b.AMSE_Id,
                                        ACMS_Id = b.ACMS_Id,
                                        ISMS_Id = b.ISMS_Id,
                                        ACALD_ActiveFlag = b.ACALD_ActiveFlag,
                                        ISMS_SubjectName = a.ISMS_SubjectName
                                    }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Atten_Login_User  get_subjects :" + ex.Message);
            }
            return data;
        }
        public Atten_Login_UserDTO Deletedetails(Atten_Login_UserDTO data)
        {
            try
            {
                var result = _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO.Single(t => t.ACALD_Id == data.ACALD_Id);
                if (result.ACALD_ActiveFlag)
                {
                    result.ACALD_ActiveFlag = false;
                    result.UpdatedDate = DateTime.Now;
                }
                else if (!result.ACALD_ActiveFlag)
                {
                    result.ACALD_ActiveFlag = true;
                    result.UpdatedDate = DateTime.Now;
                }
                _ClgAdmissionContext.Update(result);
                var contactExists = _ClgAdmissionContext.SaveChanges();
                if (contactExists == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Atten_Login_User Deletedetails :" + ex.Message);
            }

            return data;
        }
    }
}
