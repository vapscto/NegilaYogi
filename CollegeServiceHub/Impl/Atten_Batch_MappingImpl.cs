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
    public class Atten_Batch_MappingImpl : Atten_Batch_MappingInterface
    {
        public ClgAdmissionContext _ClgAdmissionContext;
        ILogger<Atten_Batch_MappingImpl> _logbranch;
        public Atten_Batch_MappingImpl(ClgAdmissionContext ClgAdmissionContext, ILogger<Atten_Batch_MappingImpl> log)
        {
            _ClgAdmissionContext = ClgAdmissionContext;
            _logbranch = log;
        }
        public Atten_Batch_MappingDTO getalldetails(Atten_Batch_MappingDTO data)
        {
            try
            {
                data.yearlist = _ClgAdmissionContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
                data.batchlist = _ClgAdmissionContext.Adm_College_Attendance_BatchDMO.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();
                data.courselist = _ClgAdmissionContext.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                data.branchlist = _ClgAdmissionContext.ClgMasterBranchDMO.Where(a => a.MI_Id == data.MI_Id && a.AMB_ActiveFlag).Distinct().OrderBy(a => a.AMB_Order).ToArray();
                data.semisterlist = _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
                data.sectionlist = _ClgAdmissionContext.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag).Distinct().OrderBy(a => a.ACMS_Order).ToArray();
                data.subjectlist = _ClgAdmissionContext.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1 && a.ISMS_AttendanceFlag).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();

            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Atten_Batch_Mapping  getalldetails :" + ex.Message);
            }
            return data;
        }

        public Atten_Batch_MappingDTO savedata1(Atten_Batch_MappingDTO data)
        {
           
            try
            {
                if (data.ACAB_Id > 0)
                {
                    var result321 = _ClgAdmissionContext.Adm_College_Attendance_BatchDMO.Where(t => t.ACAB_BatchName.Equals(data.ACAB_BatchName) && t.MI_Id.Equals(data.MI_Id) && t.ACAB_Id != data.ACAB_Id);
                    if (result321.Count() > 0)
                    {
                        data.returnduplicatestatus = true;
                    }
                    else
                    {
                        var result = _ClgAdmissionContext.Adm_College_Attendance_BatchDMO.Single(t => t.ACAB_Id.Equals(data.ACAB_Id) && t.MI_Id.Equals(data.MI_Id));
                        result.ACAB_BatchName = data.ACAB_BatchName;
                        result.ACAB_StudentStrength = data.ACAB_StudentStrength;
                        result.UpdatedDate = DateTime.Now;
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
                }
                else
                {
                    var result = _ClgAdmissionContext.Adm_College_Attendance_BatchDMO.Where(t => t.ACAB_BatchName.Equals(data.ACAB_BatchName) && t.MI_Id.Equals(data.MI_Id));
                    if (result.Count() > 0)
                    {
                        data.returnduplicatestatus = true;
                    }
                    else
                    {
                        Adm_College_Attendance_BatchDMO objpge = new Adm_College_Attendance_BatchDMO();
                        objpge.MI_Id = data.MI_Id;
                        objpge.ACAB_BatchName = data.ACAB_BatchName;
                        objpge.ACAB_StudentStrength = data.ACAB_StudentStrength;
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Add(objpge);
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
                }
                data.batchlist = _ClgAdmissionContext.Adm_College_Attendance_BatchDMO.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();  
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Atten_Batch_Mapping  savedata1 :" + ex.Message);
            }
            return data;
        }

        public Atten_Batch_MappingDTO get_courses(Atten_Batch_MappingDTO data)
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
                _logbranch.LogInformation("Atten_Batch_Mapping  get_courses :" + ex.Message);
            }
            return data;
        }
        public Atten_Batch_MappingDTO get_branches(Atten_Batch_MappingDTO data)
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
                _logbranch.LogInformation("Atten_Batch_Mapping  get_branches :" + ex.Message);
            }
            return data;
        }
        public Atten_Batch_MappingDTO get_semisters(Atten_Batch_MappingDTO data)
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
                _logbranch.LogInformation("Atten_Batch_Mapping  get_semisters :" + ex.Message);
            }
            return data;
        }

        public Atten_Batch_MappingDTO get_students(Atten_Batch_MappingDTO data)
        {
            try
            {

                var students_mapped =(from a in _ClgAdmissionContext.Adm_College_Attendance_BatchDMO
                                      from b in _ClgAdmissionContext.Adm_College_Atten_Batch_SubjectsDMO
                                      from c in _ClgAdmissionContext.Adm_College_Atten_Batch_Subject_StudentsDMO
                                      where (a.MI_Id==data.MI_Id && a.ACAB_Id==data.ACAB_Id && b.ACAB_Id == a.ACAB_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id && b.ACMS_Id == data.ACMS_Id && b.ISMS_Id==data.ISMS_Id && c.ACABS_Id==b.ACABS_Id && c.ACABSS_ActiveFlg)select c.AMCST_Id).Distinct().ToList();
                data.saveddata = students_mapped.ToArray();

                data.studentlist = (from a in _ClgAdmissionContext.Adm_Master_College_StudentDMO
                                    from b in _ClgAdmissionContext.Adm_College_Yearly_StudentDMO
                                    where (a.MI_Id == data.MI_Id && a.AMCST_ActiveFlag && a.AMCST_SOL == "S" && b.AMCST_Id == a.AMCST_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACYST_ActiveFlag ==1 && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id && b.ACMS_Id == data.ACMS_Id)
                                    select new Atten_Batch_MappingDTO
                                    {
                                        AMCST_Id = a.AMCST_Id,
                                        AMCST_FirstName = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + " " + (a.AMCST_MiddleName == null ? " " : a.AMCST_MiddleName) + " " + (a.AMCST_LastName == null ? " " : a.AMCST_LastName)).Trim(),
                                        AMCST_AdmNo = a.AMCST_AdmNo == null ? "" : a.AMCST_AdmNo,
                                        ACYST_RollNo = b.ACYST_RollNo,
                                        AMCST_DOB = a.AMCST_DOB,
                                        AMCST_StudentPhoto = a.AMCST_StudentPhoto
                                    }).Distinct().OrderBy(a => a.ACYST_RollNo).ToArray();

            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Atten_Subject_MaxPeriod  get_subjects :" + ex.Message);
            }
            return data;
        }

        public Atten_Batch_MappingDTO savedata2(Atten_Batch_MappingDTO data)
        {

            try
            {
                var results = _ClgAdmissionContext.Adm_College_Atten_Batch_SubjectsDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id && t.AMSE_Id==data.AMSE_Id && t.ACMS_Id==data.ACMS_Id && t.ISMS_Id==data.ISMS_Id && t.ACAB_Id==data.ACAB_Id).ToList();

                if (results.Count == 0)
                {
                    Adm_College_Atten_Batch_SubjectsDMO obj_p = new Adm_College_Atten_Batch_SubjectsDMO();
                    obj_p.ASMAY_Id = data.ASMAY_Id;
                    obj_p.AMCO_Id = data.AMCO_Id;
                    obj_p.AMB_Id = data.AMB_Id;
                    obj_p.AMSE_Id = data.AMSE_Id;
                    obj_p.ACMS_Id = data.ACMS_Id;
                    obj_p.ISMS_Id = data.ISMS_Id;
                    obj_p.ACAB_Id = data.ACAB_Id;
                    obj_p.ACABS_StudentStrength = data.sub_data.Length;
                    obj_p.CreatedDate = DateTime.Now;
                    obj_p.UpdatedDate = DateTime.Now;
                    _ClgAdmissionContext.Add(obj_p);
                    data.ACABS_Id = obj_p.ACABS_Id;
                }
                else if (results.Count == 1)
                {
                    //data.ACABS_Id = _ClgAdmissionContext.Adm_College_Atten_Batch_SubjectsDMO.Single(t => t.ASMAY_Id == data.ASMAY_Id && t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id && t.AMSE_Id == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id && t.ISMS_Id == data.ISMS_Id && t.ACAB_Id == data.ACAB_Id).ACABS_Id;

                    var parent_obj = _ClgAdmissionContext.Adm_College_Atten_Batch_SubjectsDMO.Single(t => t.ASMAY_Id == data.ASMAY_Id && t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id && t.AMSE_Id == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id && t.ISMS_Id == data.ISMS_Id && t.ACAB_Id == data.ACAB_Id);
                    parent_obj.ACABS_StudentStrength = data.sub_data.Length;
                    data.ACABS_Id = parent_obj.ACABS_Id;
                    parent_obj.UpdatedDate = DateTime.Now;
                    _ClgAdmissionContext.Update(parent_obj);
                }
                var result321 = _ClgAdmissionContext.Adm_College_Atten_Batch_Subject_StudentsDMO.Where(t =>t.ACABS_Id == data.ACABS_Id).ToList();
                if (result321.Count() > 0)
                {
                    foreach (var res in result321)
                    {
                        res.ACABSS_ActiveFlg = false;
                        res.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Update(res);
                    }
                }
                foreach (var x in data.sub_data)
                {
                    var child_objs = _ClgAdmissionContext.Adm_College_Atten_Batch_Subject_StudentsDMO.Where(t =>t.ACABS_Id == data.ACABS_Id && t.AMCST_Id == x).ToList();
                    if (child_objs.Count == 1)
                    {
                        child_objs[0].ACABSS_ActiveFlg = true;
                        child_objs[0].UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Update(child_objs[0]);
                    }
                    else if (child_objs.Count == 0)
                    {
                        Adm_College_Atten_Batch_Subject_StudentsDMO obj_c = new Adm_College_Atten_Batch_Subject_StudentsDMO();
                        obj_c.ACABS_Id = data.ACABS_Id;
                        obj_c.AMCST_Id = x;                       
                        obj_c.ACABSS_ActiveFlg = true;
                        obj_c.CreatedDate = DateTime.Now;
                        obj_c.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Add(obj_c);
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
                _logbranch.LogInformation("Atten_Batch_Mapping savedata :" + ex.Message);
            }
            return data;
        }

        public Atten_Batch_MappingDTO view_subjects(Atten_Batch_MappingDTO data)
        {
            try
            {
               

            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Atten_Batch_Mapping  get_subjects :" + ex.Message);
            }
            return data;
        }
        public Atten_Batch_MappingDTO Deletedetails(Atten_Batch_MappingDTO data)
        {
            try
            {
                //var result = _ClgAdmissionContext.Adm_College_Atten_Login_DetailsDMO.Single(t => t.ACALD_Id == data.ACALD_Id);
                //if (result.ACALD_ActiveFlag)
                //{
                //    result.ACALD_ActiveFlag = false;
                //    result.UpdatedDate = DateTime.Now;
                //}
                //else if (!result.ACALD_ActiveFlag)
                //{
                //    result.ACALD_ActiveFlag = true;
                //    result.UpdatedDate = DateTime.Now;
                //}
                //_ClgAdmissionContext.Update(result);
                //var contactExists = _ClgAdmissionContext.SaveChanges();
                //if (contactExists == 1)
                //{
                //    data.returnval = true;
                //}
                //else
                //{
                //    data.returnval = false;
                //}
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Atten_Batch_Mapping Deletedetails :" + ex.Message);
            }

            return data;
        }
    }
}
