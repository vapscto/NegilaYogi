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
    public class Atten_Subject_MaxPeriodImpl : Atten_Subject_MaxPeriodInterface
    {
        public ClgAdmissionContext _ClgAdmissionContext;
        ILogger<Atten_Subject_MaxPeriodImpl> _logbranch;
        public Atten_Subject_MaxPeriodImpl(ClgAdmissionContext ClgAdmissionContext, ILogger<Atten_Subject_MaxPeriodImpl> log)
        {
            _ClgAdmissionContext = ClgAdmissionContext;
            _logbranch = log;
        }
        public Atten_Subject_MaxPeriodDTO getalldetails(Atten_Subject_MaxPeriodDTO data)
        {
            try
            {
                data.yearlist = _ClgAdmissionContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
                data.courselist = _ClgAdmissionContext.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                data.branchlist = _ClgAdmissionContext.ClgMasterBranchDMO.Where(a => a.MI_Id == data.MI_Id && a.AMB_ActiveFlag).Distinct().OrderBy(a => a.AMB_Order).ToArray();
                data.semisterlist = _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
                data.sectionlist = _ClgAdmissionContext.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag).Distinct().OrderBy(a => a.ACMS_Order).ToArray();

                data.alldetails = (from a in _ClgAdmissionContext.Adm_College_Atten_Subject_MaxPeriodDMO
                                   from b in _ClgAdmissionContext.MasterCourseDMO
                                   from c in _ClgAdmissionContext.ClgMasterBranchDMO
                                   from d in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                   from e in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                                   from f in _ClgAdmissionContext.AcademicYear
                                   where (a.AMCO_Id == b.AMCO_Id && a.AMB_Id == c.AMB_Id && a.AMSE_Id == d.AMSE_Id && a.ACMS_Id == e.ACMS_Id && a.ASMAY_Id == f.ASMAY_Id && a.MI_Id == data.MI_Id)
                                   select new Atten_Subject_MaxPeriodDTO
                                   {
                                       AMCO_CourseName = b.AMCO_CourseName,
                                       AMB_BranchName = c.AMB_BranchName,
                                       AMSE_SEMName = d.AMSE_SEMName,
                                       ACMS_SectionName = e.ACMS_SectionName,
                                       ASMAY_Year = f.ASMAY_Year,
                                       ASMAY_Id = a.ASMAY_Id,
                                       AMCO_Id = a.AMCO_Id,
                                       AMB_Id = a.AMB_Id,
                                       AMSE_Id = a.AMSE_Id,
                                       ACMS_Id = a.ACMS_Id,
                                   }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Atten_Subject_MaxPeriod  getalldetails :" + ex.Message);
            }
            return data;
        }

        public Atten_Subject_MaxPeriodDTO get_courses(Atten_Subject_MaxPeriodDTO data)
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
                _logbranch.LogInformation("Atten_Subject_MaxPeriod  get_courses :" + ex.Message);
            }
            return data;
        }
        public Atten_Subject_MaxPeriodDTO get_branches(Atten_Subject_MaxPeriodDTO data)
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
                _logbranch.LogInformation("Atten_Subject_MaxPeriod  get_branches :" + ex.Message);
            }
            return data;
        }
        public Atten_Subject_MaxPeriodDTO get_semisters(Atten_Subject_MaxPeriodDTO data)
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
                _logbranch.LogInformation("Atten_Subject_MaxPeriod  get_semisters :" + ex.Message);
            }
            return data;
        }

        public Atten_Subject_MaxPeriodDTO get_subjects(Atten_Subject_MaxPeriodDTO data)
        {
            try
            {

                var subjects_mapped = _ClgAdmissionContext.Adm_College_Atten_Subject_MaxPeriodDMO.Where(b => b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id && b.ACMS_Id == data.ACMS_Id).Distinct().ToList();
                data.saveddata = subjects_mapped.ToArray();
                data.subjectlist = _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1 && a.ISMS_AttendanceFlag).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Atten_Subject_MaxPeriod  get_subjects :" + ex.Message);
            }
            return data;
        }
        public Atten_Subject_MaxPeriodDTO savedata(Atten_Subject_MaxPeriodDTO data)
        {

            try
            {
                foreach (var x in data.sub_data)
                {
                    var result321 = _ClgAdmissionContext.Adm_College_Atten_Subject_MaxPeriodDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id && t.AMSE_Id == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id && t.ISMS_Id == x.ISMS_Id);
                    if (result321.Count() > 0)
                    {
                        var result = _ClgAdmissionContext.Adm_College_Atten_Subject_MaxPeriodDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id && t.AMSE_Id == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id && t.ISMS_Id == x.ISMS_Id);
                        result.ACASMP_MaxPeriod = x.ACASMP_MaxPeriod;
                        result.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Update(result);
                    }
                    else
                    {
                        Adm_College_Atten_Subject_MaxPeriodDMO objpge1 = Mapper.Map<Adm_College_Atten_Subject_MaxPeriodDMO>(data);
                        //objpge1.ACASMP_Id = 0;
                        objpge1.ISMS_Id = x.ISMS_Id;
                        objpge1.ACASMP_MaxPeriod = x.ACASMP_MaxPeriod;
                        objpge1.ACASMP_ActiveFlag = true;
                        objpge1.CreatedDate = DateTime.Now;
                        objpge1.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Add(objpge1);
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

            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Atten_Subject_MaxPeriod savedata :" + ex.Message);
            }
            return data;
        }
        public Atten_Subject_MaxPeriodDTO Deletedetails(Atten_Subject_MaxPeriodDTO data)
        {
            try
            {
                var result = _ClgAdmissionContext.Adm_College_Atten_Subject_MaxPeriodDMO.Single(t => t.ACASMP_Id == data.ACASMP_Id);
                if (result.ACASMP_ActiveFlag)
                {
                    result.ACASMP_ActiveFlag = false;
                    result.UpdatedDate = DateTime.Now;
                }
                else if (!result.ACASMP_ActiveFlag)
                {
                    result.ACASMP_ActiveFlag = true;
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
                _logbranch.LogInformation("Atten_Subject_MaxPeriod Deletedetails :" + ex.Message);
            }

            return data;
        }

        public Atten_Subject_MaxPeriodDTO showmodaldetails(Atten_Subject_MaxPeriodDTO data)
        {
            try
            {
                data.alldetailsshow = (from a in _ClgAdmissionContext.Adm_College_Atten_Subject_MaxPeriodDMO
                                       from b in _ClgAdmissionContext.MasterCourseDMO
                                       from c in _ClgAdmissionContext.ClgMasterBranchDMO
                                       from d in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                       from e in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                                       from f in _ClgAdmissionContext.AcademicYear
                                       from g in _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO
                                       where (a.AMCO_Id == b.AMCO_Id && a.AMB_Id == c.AMB_Id && a.AMSE_Id == d.AMSE_Id && a.ACMS_Id == e.ACMS_Id
                                       && a.ASMAY_Id == f.ASMAY_Id && a.MI_Id == data.MI_Id && a.ISMS_Id == g.ISMS_Id && a.AMCO_Id == data.AMCO_Id
                                       && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACMS_Id == data.ACMS_Id && a.ASMAY_Id == data.ASMAY_Id)
                                       select new Atten_Subject_MaxPeriodDTO
                                       {
                                           AMCO_CourseName = b.AMCO_CourseName,
                                           AMB_BranchName = c.AMB_BranchName,
                                           AMSE_SEMName = d.AMSE_SEMName,
                                           ACMS_SectionName = e.ACMS_SectionName,
                                           ASMAY_Year = f.ASMAY_Year,
                                           ISMS_SubjectName = g.ISMS_SubjectName,
                                           ACASMP_MaxPeriod = a.ACASMP_MaxPeriod,
                                           ACASMP_ActiveFlag = a.ACASMP_ActiveFlag,
                                           ISMS_Id = a.ISMS_Id,
                                           ACASMP_Id = a.ACASMP_Id,
                                           ASMAY_Id = a.ASMAY_Id,
                                           AMCO_Id = a.AMCO_Id,
                                           AMB_Id = a.AMB_Id,
                                           AMSE_Id = a.AMSE_Id,
                                           ACMS_Id = a.ACMS_Id,
                                       }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Atten_Subject_MaxPeriod Deletedetails :" + ex.Message);
            }

            return data;
        }
        public Atten_Subject_MaxPeriodDTO deactivesem(Atten_Subject_MaxPeriodDTO data)
        {
            try
            {
                var result = _ClgAdmissionContext.Adm_College_Atten_Subject_MaxPeriodDMO.Single(a => a.MI_Id == data.MI_Id && a.ACASMP_Id == data.ACASMP_Id);
                if (result.ACASMP_ActiveFlag == true)
                {
                    result.ACASMP_ActiveFlag = false;
                }
                else
                {
                    result.ACASMP_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _ClgAdmissionContext.Update(result);
                var i = _ClgAdmissionContext.SaveChanges();
                if (i > 0)
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
                _logbranch.LogInformation("Atten_Subject_MaxPeriod Deletedetails :" + ex.Message);
                data.returnval = false;
            }

            return data;
        }

    }
}
