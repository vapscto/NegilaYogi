using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Dynamic;
using System.Text.RegularExpressions;
using CommonLibrary;
using CollegeServiceHub.Interface;
using DomainModel.Model.com.vapstech.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;

namespace CollegeServiceHub.Impl
{
    public class clg_CB_SEM_MappingImpl : Interface.clg_CB_SEM_MappingInterface
    {

        public ClgAdmissionContext _Context;
        public clg_CB_SEM_MappingImpl(ClgAdmissionContext Admissiondbcontext)
        {
            _Context = Admissiondbcontext;
        }
        public clg_CB_SEM_MappingDTO GetDropDownList(clg_CB_SEM_MappingDTO stu)
        {
            try
            {


                List<CLG_Adm_Master_SemesterDMO> semlist = new List<CLG_Adm_Master_SemesterDMO>();
                semlist = _Context.CLG_Adm_Master_SemesterDMO.Where(t => t.MI_Id == stu.MI_Id && t.AMSE_ActiveFlg == true).OrderBy(d => d.AMSE_SEMOrder).ToList();
                stu.semlist = semlist.ToArray();



                List<MasterCourseDMO> courselist = new List<MasterCourseDMO>();
                courselist = _Context.MasterCourseDMO.Where(t => t.MI_Id == stu.MI_Id && t.AMCO_ActiveFlag == true).OrderBy(d => d.AMCO_Order).ToList();
                stu.courselist = courselist.ToArray();

                stu.griddata = (from a in _Context.ClgMasterBranchDMO
                                from b in _Context.MasterCourseDMO
                                from c in _Context.Adm_Course_Branch_MappingDMO
                                from d in _Context.AdmCourseBranchSemesterMappingDMO
                                where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == stu.MI_Id && a.AMB_Id == c.AMB_Id && b.AMCO_Id == c.AMCO_Id && c.AMCOBM_Id == d.AMCOBM_Id && a.AMB_ActiveFlag == true && b.AMCO_ActiveFlag == true && c.AMCOBM_ActiveFlg == true)
                                select new clg_CB_SEM_MappingDTO
                                {
                                    AMCOBM_Id = d.AMCOBM_Id,
                                    AMB_BranchName = a.AMB_BranchName,
                                    AMCO_CourseName = b.AMCO_CourseName

                                }
                   ).Distinct().ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu;
        }
        public clg_CB_SEM_MappingDTO sempopup(clg_CB_SEM_MappingDTO data)
        {
            try
            {
                data.semdetails = (from a in _Context.ClgMasterBranchDMO
                                   from b in _Context.MasterCourseDMO
                                   from c in _Context.Adm_Course_Branch_MappingDMO
                                   from d in _Context.AdmCourseBranchSemesterMappingDMO
                                   from e in _Context.CLG_Adm_Master_SemesterDMO
                                   where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.AMB_Id == c.AMB_Id && b.AMCO_Id == c.AMCO_Id && c.AMCOBM_Id == d.AMCOBM_Id && a.AMB_ActiveFlag == true && b.AMCO_ActiveFlag == true && c.AMCOBM_ActiveFlg == true && e.AMSE_ActiveFlg == true && e.AMSE_Id == d.AMSE_Id && e.MI_Id == a.MI_Id)
                                   select new clg_CB_SEM_MappingDTO
                                   {
                                       AMCOBMS_Id = d.AMCOBMS_Id,
                                       AMCOBM_Id = d.AMCOBM_Id,
                                       AMB_Id = a.AMB_Id,
                                       AMCO_Id = b.AMCO_Id,
                                       AMB_BranchName = a.AMB_BranchName,
                                       AMCO_CourseName = b.AMCO_CourseName,
                                       AMSE_Id = d.AMSE_Id,
                                       AMSE_SEMName = e.AMSE_SEMName,
                                       AMCOBMS_ActiveFlg = d.AMCOBMS_ActiveFlg

                                   }
                   ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            return data;
        }

        public clg_CB_SEM_MappingDTO Editrecord(clg_CB_SEM_MappingDTO data)
        {
            try
            {
                data.editgriddata = (from a in _Context.ClgMasterBranchDMO
                                     from b in _Context.MasterCourseDMO
                                     from c in _Context.Adm_Course_Branch_MappingDMO
                                     from d in _Context.AdmCourseBranchSemesterMappingDMO
                                     where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.AMB_Id == c.AMB_Id && b.AMCO_Id == c.AMCO_Id && c.AMCOBM_Id == d.AMCOBM_Id && a.AMB_ActiveFlag == true && b.AMCO_ActiveFlag == true && c.AMCOBM_ActiveFlg == true && d.AMCOBMS_ActiveFlg == true)
                                     select new clg_CB_SEM_MappingDTO
                                     {
                                         AMCOBM_Id = d.AMCOBM_Id,
                                         AMB_Id = a.AMB_Id,
                                         AMCO_Id = b.AMCO_Id,
                                         AMB_BranchName = a.AMB_BranchName,
                                         AMCO_CourseName = b.AMCO_CourseName,
                                         AMSE_Id = d.AMSE_Id

                                     }
                   ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

            return data;
        }


        public clg_CB_SEM_MappingDTO Getbranch(clg_CB_SEM_MappingDTO data)
        {
            try
            {
                data.coursebranchlist = (from a in _Context.MasterCourseDMO
                                         from b in _Context.ClgMasterBranchDMO
                                         from c in _Context.Adm_Course_Branch_MappingDMO
                                         where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && c.AMCO_Id == a.AMCO_Id && c.AMCO_Id == data.AMCO_Id && b.AMB_Id == c.AMB_Id && c.AMCOBM_ActiveFlg == true)
                                         select new clg_CB_SEM_MappingDTO

                                         {
                                             AMCOBM_Id = c.AMCOBM_Id,
                                             AMB_Id = b.AMB_Id,
                                             AMB_BranchName = b.AMB_BranchName,
                                         }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

            return data;
        }


        public clg_CB_SEM_MappingDTO deactivate(clg_CB_SEM_MappingDTO data)
        {
            try
            {
                var query = _Context.AdmCourseBranchSemesterMappingDMO.Single(s => s.MI_Id == data.MI_Id && s.AMCOBMS_Id == data.AMCOBMS_Id);

                if (query.AMCOBMS_ActiveFlg == true)
                {
                    query.AMCOBMS_ActiveFlg = false;
                }
                else
                {
                    query.AMCOBMS_ActiveFlg = true;
                }
                query.UpdatedDate = DateTime.Now;
                _Context.Update(query);
                var contactExists = _Context.SaveChanges();
                if (contactExists == 1)
                {
                    data.returnval = true;
                    //data.returnsavestatus = "saved";
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public clg_CB_SEM_MappingDTO savesem(clg_CB_SEM_MappingDTO data)
        {
            try
            {
                //bool returnresult = false;
                //   AdmCourseBranchSemesterMappingDMO feepge = Mapper.Map<AdmCourseBranchSemesterMappingDMO>(data);
                //string retval = "";
                //    try
                //    {
                if (data.AMCOBMS_Id > 0)
                {
                    //var result321 = _Context.AdmCourseBranchSemesterMappingDMO.Where(t => t.AMCOBMS_Id != feepge.AMCOBMS_Id && t.AMCOBM_Id == feepge.AMCOBM_Id && t.AMSE_Id == feepge.AMSE_Id &&t.MI_Id==data.MI_Id );
                    //if (result321.Count() > 0)
                    //{
                    //    retval = "Duplicate";
                    //    data.returnduplicatestatus = retval;
                    //}
                    //else
                    //{
                    //    var result = _Context.AdmCourseBranchSemesterMappingDMO.Single(t => t.AMCOBMS_Id == feepge.AMCOBMS_Id);
                    //    result.MI_Id = feepge.MI_Id;
                    //    result.FMGG_GroupName = feepge.FMGG_GroupName;
                    //    result.FMGG_GroupCode = feepge.FMGG_GroupCode;
                    //    result.FMGG_ActiveFlag = feepge.FMGG_ActiveFlag;
                    //    result.UpdatedDate = DateTime.Now;
                    //    _FeeGroupContext.Update(result);
                    //    var contactExists = _FeeGroupContext.SaveChanges();

                    //    List<FeeGroupGroupingDMO> lst = new List<FeeGroupGroupingDMO>();
                    //    lst = _FeeGroupContext.feeGGG.Where(t => t.FMGG_Id == feepge.FMGG_Id).ToList();
                    //    for (int i = 0; i < lst.Count; i++)
                    //    {
                    //        var result1 = _FeeGroupContext.feeGGG.Single(t => t.FMGGG_Id == lst[i].FMGGG_Id);
                    //        result1.FMG_Id = FGpage.TempararyArrayList[i].FMG_Id;
                    //        result1.FMGG_Id = feepge.FMGG_Id;
                    //        result1.UpdatedDate = DateTime.Now;
                    //        _FeeGroupContext.Update(result1);
                    //        var contactExists1 = _FeeGroupContext.SaveChanges();

                    //        if (contactExists == 1)
                    //        {
                    //            returnresult = true;
                    //            FGpage.returnval = returnresult;
                    //            FGpage.returnduplicatestatus = "Updated";
                    //        }
                    //        else
                    //        {
                    //            returnresult = false;
                    //            FGpage.returnval = returnresult;
                    //            FGpage.returnduplicatestatus = "Not Updated";
                    //        }
                    //    }
                    //}
                }
                else
                {
                    var rr = _Context.AdmCourseBranchSemesterMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCOBM_Id == data.AMCOBM_Id).ToList();
                    if (rr.Count > 0)
                    {
                        for (int j = 0; j < rr.Count; j++)
                        {
                            var UPACBSM = rr[j];

                            UPACBSM.AMCOBMS_ActiveFlg = false;
                            UPACBSM.UpdatedDate = DateTime.Now;
                            _Context.Update(UPACBSM);
                        }

                        var contactExists = _Context.SaveChanges();

                    }


                    for (int i = 0; i < data.selectedsem.Length; i++)
                    {
                        var zz = _Context.AdmCourseBranchSemesterMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCOBM_Id == data.AMCOBM_Id && t.AMSE_Id == data.selectedsem[i].AMSE_Id).ToList();
                        if (zz.Count > 0)
                        {
                            // AdmCourseBranchSemesterMappingDMO ACBSM1 = new AdmCourseBranchSemesterMappingDMO();
                            var ACBSM1 = _Context.AdmCourseBranchSemesterMappingDMO.Single(t => t.MI_Id == data.MI_Id && t.AMCOBM_Id == data.AMCOBM_Id && t.AMSE_Id == data.selectedsem[i].AMSE_Id);
                            ACBSM1.AMCOBMS_ActiveFlg = true;
                            ACBSM1.UpdatedDate = DateTime.Now;
                            _Context.Update(ACBSM1);
                        }
                        else
                        {
                            AdmCourseBranchSemesterMappingDMO ACBSM = new AdmCourseBranchSemesterMappingDMO();
                            ACBSM.AMCOBM_Id = data.AMCOBM_Id;
                            ACBSM.AMSE_Id = data.selectedsem[i].AMSE_Id;
                            ACBSM.MI_Id = data.MI_Id;
                            ACBSM.AMCOBMS_ActiveFlg = true;
                            ACBSM.CreatedDate = DateTime.Now;
                            ACBSM.UpdatedDate = DateTime.Now;
                            _Context.Add(ACBSM);
                        }
                    }
                    var contactExists1 = _Context.SaveChanges();
                    if (contactExists1 > 0)
                    {

                        data.returnduplicatestatus = "Saved";
                    }
                    else
                    {
                        //returnresult = false;
                        //FGpage.returnval = returnresult;
                        data.returnduplicatestatus = "Not Saved";

                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
