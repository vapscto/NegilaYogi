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
    public class ClgyearlycoursemappingImpl : Interface.ClgyearlycoursemappingInterface
    {
        public ClgAdmissionContext _ClgAdmissionContext;
        ILogger<ClgyearlycoursemappingImpl> _logbranch;

        public ClgyearlycoursemappingImpl(ClgAdmissionContext ClgAdmissionContext, ILogger<ClgyearlycoursemappingImpl> logbranch)
        {
            _ClgAdmissionContext = ClgAdmissionContext;
            _logbranch = logbranch;
        }

        public ClgyearlycoursemappingDTO getalldetails(ClgyearlycoursemappingDTO data)
        {
            try
            {
                data.accyearlist = _ClgAdmissionContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id).OrderByDescending(a => a.ASMAY_Order).ToArray();
                data.courselist = _ClgAdmissionContext.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true).ToArray();
                data.semesterlistget = _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true).ToArray();



                data.getsaveddata = (from a in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                     from b in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                                     from c in _ClgAdmissionContext.AcademicYear
                                     from d in _ClgAdmissionContext.MasterCourseDMO
                                     from e in _ClgAdmissionContext.ClgMasterBranchDMO
                                     where (a.ACAYC_Id == b.ACAYC_Id && a.AMCO_Id == d.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMB_Id == e.AMB_Id
                                     && a.MI_Id == data.MI_Id)
                                     select new ClgyearlycoursemappingDTO
                                     {
                                         ASMAY_Year = c.ASMAY_Year,
                                         AMCO_CourseName = d.AMCO_CourseName,
                                         AMB_BranchName = e.AMB_BranchName,
                                         ASMAY_Order = c.ASMAY_Order,
                                         AMCO_Order = d.AMCO_Order,
                                         AMB_Order = e.AMB_Order,
                                         AMB_Id = b.AMB_Id,
                                         AMCO_Id = a.AMCO_Id,
                                         ASMAY_Id = a.ASMAY_Id,
                                         ACAYC_Id = a.ACAYC_Id,
                                         ACAYCB_Id = b.ACAYCB_Id

                                     }).Distinct().OrderByDescending(a => a.ASMAY_Order).ThenBy(a => a.AMCO_Order).ThenBy(a => a.AMB_Order).ToArray();


            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Clg Yearly Course Mapping getalldetails :" + ex.Message);
            }
            return data;
        }
        public ClgyearlycoursemappingDTO getbranches(ClgyearlycoursemappingDTO data)
        {
            try
            {
                data.branchlist = (from a in _ClgAdmissionContext.MasterCourseDMO
                                   from b in _ClgAdmissionContext.ClgMasterBranchDMO
                                   from c in _ClgAdmissionContext.Adm_Course_Branch_MappingDMO
                                   where (a.AMCO_Id == c.AMCO_Id && b.AMB_Id == c.AMB_Id && c.AMCOBM_ActiveFlg == true && b.AMB_ActiveFlag == true && a.AMCO_ActiveFlag == true && b.MI_Id == data.MI_Id && c.AMCO_Id == data.AMCO_Id)
                                   select new ClgyearlycoursemappingDTO
                                   {
                                       AMB_Id = b.AMB_Id,
                                       AMB_BranchName = b.AMB_BranchName
                                   }).Distinct().ToArray();

                //data.getcourselist = (from a in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                //                      from b in _ClgAdmissionContext.MasterCourseDMO
                //                      where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id)
                //                      select new ClgyearlycoursemappingDTO
                //                      {
                //                          ACAYC_To_Date = a.ACAYC_To_Date,
                //                          ACAYC_From_Date = a.ACAYC_From_Date
                //                      }).ToArray();
                data.getcourselist = _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.MI_Id == data.MI_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Clg Yearly Course Mapping getbranches :" + ex.Message);
            }
            return data;
        }
        public ClgyearlycoursemappingDTO getsemisters(ClgyearlycoursemappingDTO data)
        {
            try
            {
                data.semesterlist = (from a in _ClgAdmissionContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     from b in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                                     from c in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                     from d in _ClgAdmissionContext.MasterCourseDMO
                                     from e in _ClgAdmissionContext.ClgMasterBranchDMO
                                     from f in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                     where (a.ACAYCB_Id == b.ACAYCB_Id && b.ACAYC_Id == c.ACAYC_Id && c.AMCO_Id == d.AMCO_Id && b.AMB_Id == e.AMB_Id && a.AMSE_Id == f.AMSE_Id && b.AMB_Id == data.AMB_Id && c.AMCO_Id == data.AMCO_Id && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id && a.ACAYCBS_ActiveFlag == true)
                                     select new ClgyearlycoursemappingDTO
                                     {
                                         AMSE_Id = f.AMSE_Id,
                                         AMSE_SEMName = f.AMSE_SEMName,
                                         ACAYCBS_SemStartDate = a.ACAYCBS_SemStartDate,
                                         ACAYCBS_SemEndDate = a.ACAYCBS_SemEndDate,
                                         ACAYCBS_SemOrder = a.ACAYCBS_SemOrder
                                     }).Distinct().ToArray();

                if (data.semesterlist.Length == 0)
                {
                    data.semesterlist = _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true).Take(data.noofsem).ToArray();
                }

                //data.semesterlist = (from a in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                //                     from b in _ClgAdmissionContext.ClgMasterBranchDMO
                //                     from c in _ClgAdmissionContext.AdmCourseBranchSemesterMappingDMO
                //                     from d in _ClgAdmissionContext.Adm_Course_Branch_MappingDMO
                //                     where (a.AMSE_Id == c.AMSE_Id && b.AMB_Id == d.AMB_Id && c.AMCOBMS_ActiveFlg == true && b.AMB_ActiveFlag == true
                //                     && a.AMSE_ActiveFlg == true && b.MI_Id == data.MI_Id && d.AMB_Id == data.AMB_Id && c.AMCOBM_Id == d.AMCOBM_Id)
                //                     select new ClgyearlycoursemappingDTO
                //                     {
                //                         AMSE_Id = a.AMSE_Id,
                //                         AMSE_SEMName = a.AMSE_SEMName
                //                     }).Distinct().ToArray();



                data.branchlistdate = (from a in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                       from b in _ClgAdmissionContext.ClgMasterBranchDMO
                                       from c in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                                       where (c.AMB_Id == b.AMB_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == a.ACAYC_Id && c.AMB_Id == data.AMB_Id)
                                       select new ClgyearlycoursemappingDTO
                                       {
                                           ACAYCB_PreAdm_FDate = c.ACAYCB_PreAdm_FDate,
                                           ACAYCB_PreAdm_TDate = c.ACAYCB_PreAdm_TDate,
                                           ACAYB_ReferenceDate = c.ACAYB_ReferenceDate
                                       }).ToArray();


                data.semesterlistget = (from a in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                        from b in _ClgAdmissionContext.AdmCourseBranchSemesterMappingDMO
                                        from c in _ClgAdmissionContext.MasterCourseDMO
                                        from d in _ClgAdmissionContext.ClgMasterBranchDMO
                                        from e in _ClgAdmissionContext.Adm_Course_Branch_MappingDMO
                                        where (a.AMSE_Id == b.AMSE_Id && c.AMCO_Id == e.AMCO_Id && b.AMCOBM_Id == e.AMCOBM_Id && d.AMB_Id == e.AMB_Id && a.MI_Id == data.MI_Id && e.AMCO_Id == data.AMCO_Id && e.AMB_Id == data.AMB_Id)
                                        select a
                                        ).Distinct().OrderBy(t => t.AMSE_SEMOrder).ToArray();



                //_ClgAdmissionContext.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true).ToArray();

                //data.semesterlistget = _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true).Distinct().OrderBy(t=>t.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Clg Yearly Course Mapping getsemisters :" + ex.Message);
            }
            return data;
        }

        public ClgyearlycoursemappingDTO savedata(ClgyearlycoursemappingDTO data)
        {
            try
            {
                var result1 = _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCO_Id == data.AMCO_Id).Distinct().ToList();
                if (result1.Count == 0)
                {
                    CLG_Adm_College_AY_CourseDMO obj1 = new CLG_Adm_College_AY_CourseDMO();
                    obj1.MI_Id = data.MI_Id;
                    obj1.ASMAY_Id = data.ASMAY_Id;
                    obj1.AMCO_Id = data.AMCO_Id;
                    obj1.ACAYC_From_Date = data.ACAYC_From_Date;
                    obj1.ACAYC_To_Date = data.ACAYC_To_Date;
                    obj1.ACAYC_NoOfSEM = data.ACAYC_NoOfSEM;
                    obj1.ACAYC_ActiveFlag = true;
                    obj1.CreatedDate = DateTime.Now;
                    obj1.UpdatedDate = DateTime.Now;
                    _ClgAdmissionContext.Add(obj1);

                    CLG_Adm_College_AY_Course_BranchDMO obj2 = new CLG_Adm_College_AY_Course_BranchDMO();
                    obj2.MI_Id = data.MI_Id;
                    obj2.ACAYC_Id = obj1.ACAYC_Id;
                    obj2.AMB_Id = data.AMB_Id;
                    obj2.ACAYCB_PreAdm_FDate = data.ACAYCB_PreAdm_FDate;
                    obj2.ACAYCB_PreAdm_TDate = data.ACAYCB_PreAdm_TDate;
                    obj2.ACAYB_ReferenceDate = data.ACAYB_ReferenceDate;
                    obj2.ACAYCB_ActiveFlag = true;
                    obj2.CreatedDate = DateTime.Now;
                    obj2.UpdatedDate = DateTime.Now;
                    _ClgAdmissionContext.Add(obj2);

                    foreach (var x in data.temp_sem_branch_DTO)
                    {
                        CLG_Adm_College_AY_Course_Branch_SemesterDMO obj3 = new CLG_Adm_College_AY_Course_Branch_SemesterDMO();
                        obj3.MI_Id = data.MI_Id;
                        obj3.ACAYCB_Id = obj2.ACAYCB_Id;
                        obj3.AMSE_Id = x.AMSE_Id;
                        obj3.ACAYCBS_SemStartDate = x.ACAYCBS_SemStartDate;
                        obj3.ACAYCBS_SemEndDate = x.ACAYCBS_SemEndDate;
                        obj3.ACAYCBS_SemOrder = x.ACAYCBS_SemOrder;
                        obj3.ACAYCBS_ActiveFlag = true;
                        obj3.CreatedDate = DateTime.Now;
                        obj3.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Add(obj3);
                    }
                    var contactExist = _ClgAdmissionContext.SaveChanges();
                    if (contactExist >= 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else if (result1.Count == 1)
                {
                    var res_obj1 = _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCO_Id == data.AMCO_Id);
                    res_obj1.ACAYC_From_Date = data.ACAYC_From_Date;
                    res_obj1.ACAYC_To_Date = data.ACAYC_To_Date;
                    res_obj1.ACAYC_NoOfSEM = data.ACAYC_NoOfSEM;
                    res_obj1.UpdatedDate = DateTime.Now;
                    _ClgAdmissionContext.Update(res_obj1);

                    var result2 = _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO.Where(t => t.MI_Id == data.MI_Id && t.ACAYC_Id == res_obj1.ACAYC_Id && t.AMB_Id == data.AMB_Id).Distinct().ToList();
                    if (result2.Count == 0)
                    {
                        CLG_Adm_College_AY_Course_BranchDMO obj2 = new CLG_Adm_College_AY_Course_BranchDMO();
                        obj2.MI_Id = data.MI_Id;
                        obj2.ACAYC_Id = res_obj1.ACAYC_Id;
                        obj2.AMB_Id = data.AMB_Id;
                        obj2.ACAYCB_PreAdm_FDate = data.ACAYCB_PreAdm_FDate;
                        obj2.ACAYCB_PreAdm_TDate = data.ACAYCB_PreAdm_TDate;
                        obj2.ACAYB_ReferenceDate = data.ACAYB_ReferenceDate;
                        obj2.ACAYCB_ActiveFlag = true;
                        obj2.CreatedDate = DateTime.Now;
                        obj2.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Add(obj2);

                        foreach (var x in data.temp_sem_branch_DTO)
                        {
                            CLG_Adm_College_AY_Course_Branch_SemesterDMO obj3 = new CLG_Adm_College_AY_Course_Branch_SemesterDMO();
                            obj3.MI_Id = data.MI_Id;
                            obj3.ACAYCB_Id = obj2.ACAYCB_Id;
                            obj3.AMSE_Id = x.AMSE_Id;
                            obj3.ACAYCBS_SemStartDate = x.ACAYCBS_SemStartDate;
                            obj3.ACAYCBS_SemEndDate = x.ACAYCBS_SemEndDate;
                            obj3.ACAYCBS_SemOrder = x.ACAYCBS_SemOrder;
                            obj3.ACAYCBS_ActiveFlag = true;
                            obj3.CreatedDate = DateTime.Now;
                            obj3.UpdatedDate = DateTime.Now;
                            _ClgAdmissionContext.Add(obj3);
                        }
                    }
                    else if (result2.Count == 1)
                    {
                        var res_obj2 = _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO.Single(t => t.MI_Id == data.MI_Id && t.ACAYC_Id == res_obj1.ACAYC_Id && t.AMB_Id == data.AMB_Id);
                        res_obj2.ACAYCB_PreAdm_FDate = data.ACAYCB_PreAdm_FDate;
                        res_obj2.ACAYCB_PreAdm_TDate = data.ACAYCB_PreAdm_TDate;
                        res_obj2.ACAYB_ReferenceDate = data.ACAYB_ReferenceDate;
                        res_obj2.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Update(res_obj2);
                        List<long> amse_ids = new List<long>();
                        foreach (var x in data.temp_sem_branch_DTO)
                        {
                            var result3 = _ClgAdmissionContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO.Where(t => t.MI_Id == data.MI_Id && t.ACAYCB_Id == res_obj2.ACAYCB_Id && t.AMSE_Id == x.AMSE_Id).Distinct().ToList();
                            if (result3.Count == 0)
                            {
                                CLG_Adm_College_AY_Course_Branch_SemesterDMO obj3 = new CLG_Adm_College_AY_Course_Branch_SemesterDMO();
                                obj3.MI_Id = data.MI_Id;
                                obj3.ACAYCB_Id = res_obj2.ACAYCB_Id;
                                obj3.AMSE_Id = x.AMSE_Id;
                                obj3.ACAYCBS_SemStartDate = x.ACAYCBS_SemStartDate;
                                obj3.ACAYCBS_SemEndDate = x.ACAYCBS_SemEndDate;
                                obj3.ACAYCBS_SemOrder = x.ACAYCBS_SemOrder;
                                obj3.ACAYCBS_ActiveFlag = true;
                                obj3.CreatedDate = DateTime.Now;
                                obj3.UpdatedDate = DateTime.Now;
                                _ClgAdmissionContext.Add(obj3);
                            }
                            else if (result3.Count == 1)
                            {
                                var res_obj3 = _ClgAdmissionContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO.Single(t => t.MI_Id == data.MI_Id && t.ACAYCB_Id == res_obj2.ACAYCB_Id && t.AMSE_Id == x.AMSE_Id);
                                res_obj3.ACAYCBS_SemStartDate = x.ACAYCBS_SemStartDate;
                                res_obj3.ACAYCBS_SemEndDate = x.ACAYCBS_SemEndDate;
                                res_obj3.ACAYCBS_SemOrder = x.ACAYCBS_SemOrder;
                                res_obj3.UpdatedDate = DateTime.Now;
                                _ClgAdmissionContext.Update(res_obj3);
                            }
                            amse_ids.Add(x.AMSE_Id);
                        }

                        var result_delete = _ClgAdmissionContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO.Where(t => t.MI_Id == data.MI_Id && t.ACAYCB_Id == res_obj2.ACAYCB_Id && !amse_ids.Contains(t.AMSE_Id)).Distinct().ToList();
                        if (result_delete.Any())
                        {
                            for (int e = 0; result_delete.Count > e; e++)
                            {
                                _ClgAdmissionContext.Remove(result_delete.ElementAt(e));
                            }
                        }
                    }
                    var contactExist = _ClgAdmissionContext.SaveChanges();
                    if (contactExist >= 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                }
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Clg Yearly Course Mapping savedata :" + ex.Message);
            }
            return data;
        }
        public ClgyearlycoursemappingDTO searchdata(ClgyearlycoursemappingDTO data)
        {
            try
            {
                data.semesterlist = (from a in _ClgAdmissionContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     from b in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                                     from c in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                     from d in _ClgAdmissionContext.MasterCourseDMO
                                     from e in _ClgAdmissionContext.ClgMasterBranchDMO
                                     from f in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                     where (a.ACAYCB_Id == b.ACAYCB_Id && b.ACAYC_Id == c.ACAYC_Id && c.AMCO_Id == d.AMCO_Id && b.AMB_Id == e.AMB_Id && a.AMSE_Id == f.AMSE_Id && b.AMB_Id == data.AMB_Id && c.AMCO_Id == data.AMCO_Id && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id && a.ACAYCBS_ActiveFlag == true)
                                     select new ClgyearlycoursemappingDTO
                                     {
                                         AMSE_Id = f.AMSE_Id,
                                         AMSE_SEMName = f.AMSE_SEMName,
                                         ACAYCBS_SemStartDate = a.ACAYCBS_SemStartDate,
                                         ACAYCBS_SemEndDate = a.ACAYCBS_SemEndDate,
                                     }).Distinct().ToArray();

                if (data.semesterlist.Length == 0)
                {
                    data.semesterlist = _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true).Take(data.noofsem).ToArray();
                }
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Clg Yearly Course Mapping getsemisters :" + ex.Message);
            }
            return data;
        }

        public ClgyearlycoursemappingDTO viewrecordspopup(ClgyearlycoursemappingDTO data)
        {
            try
            {
                data.getviewdetails = (from a in _ClgAdmissionContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                       from b in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                                       from c in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                       from d in _ClgAdmissionContext.MasterCourseDMO
                                       from e in _ClgAdmissionContext.ClgMasterBranchDMO
                                       from f in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                       where (a.ACAYCB_Id == b.ACAYCB_Id && b.ACAYC_Id == c.ACAYC_Id && c.AMCO_Id == d.AMCO_Id && b.AMB_Id == e.AMB_Id && a.AMSE_Id == f.AMSE_Id && b.AMB_Id == data.AMB_Id && c.AMCO_Id == data.AMCO_Id && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id && a.ACAYCBS_ActiveFlag == true && a.ACAYCB_Id==data.ACAYCB_Id && b.ACAYC_Id==data.ACAYC_Id)
                                       select new ClgyearlycoursemappingDTO
                                       {
                                           AMSE_Id = f.AMSE_Id,
                                           AMSE_SEMName = f.AMSE_SEMName,
                                           ACAYCBS_SemStartDate = a.ACAYCBS_SemStartDate,
                                           ACAYCBS_SemEndDate = a.ACAYCBS_SemEndDate,
                                           AMSE_SEMOrder=f.AMSE_SEMOrder
                                       }).Distinct().OrderBy(a=>a.AMSE_SEMOrder).ToArray();


            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Clg Yearly Course Mapping getsemisters :" + ex.Message);
            }
            return data;
        }

    }
}
