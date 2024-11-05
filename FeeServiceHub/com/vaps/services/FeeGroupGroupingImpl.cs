using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using FeeServiceHub.com.vaps.interfaces;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.Extensions.Logging;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeGroupGroupingImpl : interfaces.FeeGroupGroupingInterface
    {
        private static ConcurrentDictionary<string, FeeGroupMappingDTO> _login =
       new ConcurrentDictionary<string, FeeGroupMappingDTO>();

        public FeeGroupContext _FeeGroupContext;
        readonly ILogger<FeeGroupGroupingImpl> _logger;
        public FeeGroupGroupingImpl(FeeGroupContext frgContext, ILogger<FeeGroupGroupingImpl> log)
        {
            _FeeGroupContext = frgContext;
            _logger = log;
        }
        public FeeGroupMappingDTO SaveYearlyGroupData(FeeGroupMappingDTO FGpage)
        {
           

            bool returnresult = false;
            FeeGroupMappingDMO feepge = Mapper.Map<FeeGroupMappingDMO>(FGpage);
            string retval = "";
            int dup = 0;
            try
            {
                if (feepge.FMGG_Id > 0)
                {
                    //var result321 = _FeeGroupContext.feegm.Where(t =>t.FMGG_Id!= feepge.FMGG_Id && t.FMGG_GroupName == feepge.FMGG_GroupName && t.FMGG_GroupCode == feepge.FMGG_GroupCode);
                    var result321 = _FeeGroupContext.feegm.Where(t => t.FMGG_Id != feepge.FMGG_Id && t.FMGG_GroupName == feepge.FMGG_GroupName && t.MI_Id==feepge.MI_Id);
                    if (result321.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {
                        var result = _FeeGroupContext.feegm.Single(t => t.FMGG_Id == feepge.FMGG_Id);
                        result.MI_Id = feepge.MI_Id;
                        result.FMGG_GroupName = feepge.FMGG_GroupName;
                        result.FMGG_GroupCode = feepge.FMGG_GroupCode;
                        result.FMGG_ActiveFlag = feepge.FMGG_ActiveFlag;
                        result.UpdatedDate = DateTime.Now;
                        _FeeGroupContext.Update(result);
                        var contactExists = _FeeGroupContext.SaveChanges();

                        List<FeeGroupGroupingDMO> lst = new List<FeeGroupGroupingDMO>();
                        lst = _FeeGroupContext.feeGGG.Where(t => t.FMGG_Id == feepge.FMGG_Id).ToList();
                        for (int i = 0; i < lst.Count; i++)
                        {
                            var result1 = _FeeGroupContext.feeGGG.Single(t => t.FMGGG_Id == lst[i].FMGGG_Id);
                            result1.FMG_Id = FGpage.TempararyArrayList[i].FMG_Id;
                            result1.FMGG_Id = feepge.FMGG_Id;
                            result1.UpdatedDate = DateTime.Now;
                            _FeeGroupContext.Update(result1);
                            var contactExists1 = _FeeGroupContext.SaveChanges();

                            if (contactExists == 1)
                            {
                                returnresult = true;
                                FGpage.returnval = returnresult;
                                FGpage.returnduplicatestatus = "Updated";
                            }
                            else
                            {
                                returnresult = false;
                                FGpage.returnval = returnresult;
                                FGpage.returnduplicatestatus = "Not Updated";
                            }
                        }
                    }
                }
                else
                {



                    //comment praveen
                    //var result = _FeeGroupContext.feegm.Where(t => t.FMGG_GroupName == feepge.FMGG_GroupName && t.FMGG_GroupCode == feepge.FMGG_GroupCode && t.MI_Id == feepge.MI_Id);



                    //if (result.Count() > 0)
                    //{
                    //    retval = "Duplicate";
                    //    FGpage.returnduplicatestatus = retval;
                    //}
                    //else
                    //{
                    //    feepge.CreatedDate = DateTime.Now;
                    //    feepge.UpdatedDate = DateTime.Now;
                    //    _FeeGroupContext.Add(feepge);
                    //    var contactExists = _FeeGroupContext.SaveChanges();
                    //    var result123 = _FeeGroupContext.feegm.Max(t => t.FMGG_Id);
                    //    for (int i = 0; i < FGpage.TempararyArrayList.Length; i++)
                    //    {
                    //        FeegroupgroupingDTO te = new FeegroupgroupingDTO();
                    //        FeeGroupGroupingDMO feepgeY = Mapper.Map<FeeGroupGroupingDMO>(te);
                    //        feepgeY.FMG_Id = Convert.ToInt64(FGpage.TempararyArrayList[i].FMG_Id);
                    //        feepgeY.FMGG_Id = result123;
                    //        feepgeY.CreatedDate = DateTime.Now;
                    //        feepgeY.UpdatedDate = DateTime.Now;
                    //        _FeeGroupContext.Add(feepgeY);
                    //        var contactExists1 = _FeeGroupContext.SaveChanges();
                    //        if (contactExists == 1)
                    //        {
                    //            returnresult = true;
                    //            FGpage.returnval = returnresult;
                    //            FGpage.returnduplicatestatus = "Saved";
                    //        }
                    //        else
                    //        {
                    //            returnresult = false;
                    //            FGpage.returnval = returnresult;
                    //            FGpage.returnduplicatestatus = "Not Saved";

                    //        }
                    //    }
                    //}
                    //comment end

                    //praveen 
                    //var result = _FeeGroupContext.feegm.Where(t => t.FMGG_GroupName == feepge.FMGG_GroupName && t.FMGG_GroupCode == feepge.FMGG_GroupCode && t.MI_Id == feepge.MI_Id).ToList();

                    var result = _FeeGroupContext.feegm.Where(t => t.FMGG_GroupName == feepge.FMGG_GroupName && t.MI_Id == feepge.MI_Id).ToList();
                    if (result.Count() > 0)
                    {
                        var updatefee = _FeeGroupContext.feegm.Single(t => t.FMGG_Id == result[0].FMGG_Id);
                        updatefee.CreatedDate = DateTime.Now;
                        updatefee.UpdatedDate = DateTime.Now;
                        _FeeGroupContext.Update(updatefee);
                        //retval = "Duplicate";
                        //FGpage.returnduplicatestatus = retval;

                        for (int i = 0; i < FGpage.TempararyArrayList.Length; i++)
                        {
                            var exist_grp = _FeeGroupContext.feeGGG.Where(t => t.FMG_Id == FGpage.TempararyArrayList[i].FMG_Id && t.FMGG_Id == result[0].FMGG_Id).ToList();
                            if (exist_grp.Count==0)
                            {
                               // FeegroupgroupingDTO te = new FeegroupgroupingDTO();
                               // FeeGroupGroupingDMO feepgeY = Mapper.Map<FeeGroupGroupingDMO>(te);
                                FeeGroupGroupingDMO feepgeY = new FeeGroupGroupingDMO();
                                feepgeY.FMG_Id = Convert.ToInt64(FGpage.TempararyArrayList[i].FMG_Id);
                                feepgeY.FMGG_Id = updatefee.FMGG_Id;
                                feepgeY.CreatedDate = DateTime.Now;
                                feepgeY.UpdatedDate = DateTime.Now;
                                _FeeGroupContext.Add(feepgeY);
                                dup += 1;
                            }
                           
                            
                          
                        }
                        if (dup > 0)
                        {
                            var contactExists = _FeeGroupContext.SaveChanges();
                            if (contactExists > 1)
                            {
                                returnresult = true;
                                FGpage.returnval = returnresult;
                                FGpage.returnduplicatestatus = "Saved";
                            }
                            else
                            {
                                returnresult = false;
                                FGpage.returnval = returnresult;
                                FGpage.returnduplicatestatus = "Not Saved";

                            }
                        }
                        else
                        {
                            retval = "Duplicate";
                           FGpage.returnduplicatestatus = retval;
                        }

                    }
                    else
                    {
                        feepge.CreatedDate = DateTime.Now;
                        feepge.UpdatedDate = DateTime.Now;
                        _FeeGroupContext.Add(feepge);
                        //var contactExists = _FeeGroupContext.SaveChanges();
                        //var result123 = _FeeGroupContext.feegm.Max(t => t.FMGG_Id);
                        for (int i = 0; i < FGpage.TempararyArrayList.Length; i++)
                        {
                            FeegroupgroupingDTO te = new FeegroupgroupingDTO();
                            FeeGroupGroupingDMO feepgeY = Mapper.Map<FeeGroupGroupingDMO>(te);
                            feepgeY.FMG_Id = Convert.ToInt64(FGpage.TempararyArrayList[i].FMG_Id);
                            feepgeY.FMGG_Id = feepge.FMGG_Id;
                            feepgeY.CreatedDate = DateTime.Now;
                            feepgeY.UpdatedDate = DateTime.Now;
                            _FeeGroupContext.Add(feepgeY);
                        }
                        var contactExists = _FeeGroupContext.SaveChanges();
                       
                        if (contactExists > 1)
                        {
                            returnresult = true;
                            FGpage.returnval = returnresult;
                            FGpage.returnduplicatestatus = "Saved";
                        }
                        else
                        {
                            returnresult = false;
                            FGpage.returnval = returnresult;
                            FGpage.returnduplicatestatus = "Not Saved";

                        }
                    }
                    //end




                }

                List<FeeGroupMappingDMO> allpages1 = new List<FeeGroupMappingDMO>();
                allpages1 = _FeeGroupContext.feegm.OrderBy(t => t.FMGG_Id).ToList();
                FGpage.GroupGroupingData = allpages1.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGpage;
        }
        public FeeGroupMappingDTO getdetailsY(int id)
        {
            FeeGroupMappingDTO FGRDT = new FeeGroupMappingDTO();
            try
            {
                List<FeeGroupMappingDMO> feegrp = new List<FeeGroupMappingDMO>();
                feegrp = _FeeGroupContext.feegm.Where(t => t.MI_Id == id).ToList();
                FGRDT.GroupGroupingData = feegrp.ToArray();

                List<FeeGroupDMO> allpages = new List<FeeGroupDMO>();
                allpages = _FeeGroupContext.feeGroup.Where(t => t.MI_Id == id && t.FMG_ActiceFlag==true).OrderBy(t => t.FMG_Id).ToList();
                FGRDT.GroupData = allpages.ToArray();

                FGRDT.newarydata = (from Fee_Master_Group_Grouping in _FeeGroupContext.feegm
                                    from Fee_Master_Group in _FeeGroupContext.feeGroup
                                    from Fee_Master_Group_Grouping_Groups in _FeeGroupContext.feeGGG
                                    where (Fee_Master_Group_Grouping_Groups.FMGG_Id == Fee_Master_Group_Grouping.FMGG_Id && Fee_Master_Group.FMG_Id == Fee_Master_Group_Grouping_Groups.FMG_Id && Fee_Master_Group_Grouping.MI_Id == id)
                                    select new FeeGroupMappingDTO
                                    {
                                        //fmggidbind = Fee_Master_Group_Grouping.FMGG_Id,
                                        //groupgrpnamebind = Fee_Master_Group_Grouping.FMGG_GroupName,
                                        //groupgroupcode = Fee_Master_Group_Grouping.FMGG_GroupCode,
                                        //grpnamebind = Fee_Master_Group.FMG_GroupName,
                                        //actflag = Fee_Master_Group_Grouping.FMGG_ActiveFlag,
                                        FMGG_Id = Fee_Master_Group_Grouping.FMGG_Id,
                                        FMGG_GroupName = Fee_Master_Group_Grouping.FMGG_GroupName,
                                        FMGG_GroupCode = Fee_Master_Group_Grouping.FMGG_GroupCode,
                                        grpnamebind = Fee_Master_Group.FMG_GroupName,
                                        FMGG_ActiveFlag = Fee_Master_Group_Grouping.FMGG_ActiveFlag,
                                        FMGGG_Id= Fee_Master_Group_Grouping_Groups.FMGGG_Id
                                    }
              ).ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }

        public FeeGroupMappingDTO deactivateY(FeeGroupMappingDTO id)
        {

            try
            {
                var res2 = _FeeGroupContext.feeGGG.Where(e => e.FMGGG_Id == id.FMGGG_Id).ToList();
                if (res2.Count > 0)
                {
                    var result = _FeeGroupContext.feeGGG.Single(d => d.FMGGG_Id == id.FMGGG_Id);
                    _FeeGroupContext.Remove(result);
                    var count = _FeeGroupContext.SaveChanges();
                    if (count > 0)
                    {
                        id.returnval = true;
                    }
                    else
                    {
                        id.returnval = false;
                    }
                }

            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);

            }
            return id;
        }
        //public FeeGroupMappingDTO deactivateY(FeeGroupMappingDTO acd)
        //{
        //    try
        //    {
        //        FeeGroupMappingDMO feepge = Mapper.Map<FeeGroupMappingDMO>(acd);
        //        if (feepge.FMGG_Id > 0)
        //        {
        //            var result = _FeeGroupContext.feegm.Single(t => t.FMGG_Id == feepge.FMGG_Id);
        //            result.UpdatedDate = DateTime.Now;
        //            if (result.FMGG_ActiveFlag == true)
        //            {
        //                result.FMGG_ActiveFlag = false;
        //            }
        //            else
        //            {
        //                result.FMGG_ActiveFlag = true;
        //            }
        //            _FeeGroupContext.Update(result);
        //            var flag = _FeeGroupContext.SaveChanges();
        //            if (flag == 1)
        //            {
        //                acd.returnval = true;
        //            }
        //            else
        //            {
        //                acd.returnval = false;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e.Message);
        //        Console.WriteLine(e.InnerException);
        //    }
        //    return acd;
        //}
        public FeeGroupMappingDTO getpageeditY(int id)
        {

            FeeGroupMappingDTO page = new FeeGroupMappingDTO();
            try
            {
                page.GroupGroupingData = (from Fee_Master_Group_Grouping in _FeeGroupContext.feegm
                                    from Fee_Master_Group_Grouping_Groups in _FeeGroupContext.feeGGG
                                    where (Fee_Master_Group_Grouping.FMGG_Id== Fee_Master_Group_Grouping_Groups.FMGG_Id && Fee_Master_Group_Grouping_Groups.FMGGG_Id==id)
                                    select new FeeGroupMappingDTO
                                    {
                                       FMGG_Id= Fee_Master_Group_Grouping_Groups.FMGG_Id,
                                       FMGGG_Id= Fee_Master_Group_Grouping_Groups.FMGGG_Id,
                                        FMGG_GroupName= Fee_Master_Group_Grouping.FMGG_GroupName,
                                        FMGG_GroupCode= Fee_Master_Group_Grouping.FMGG_GroupCode
                                    }
            ).ToArray();

                //List<FeeGroupMappingDMO> lorg = new List<FeeGroupMappingDMO>();
                //lorg = _FeeGroupContext.feegm.AsNoTracking().Where(t => t.FMGG_Id.Equals(id)).ToList();
                //page.GroupGroupingData = lorg.ToArray();

                List<FeeGroupGroupingDMO> Allname101 = new List<FeeGroupGroupingDMO>();
                Allname101 = _FeeGroupContext.feeGGG.AsNoTracking().Where(t => t.FMGGG_Id.Equals(id)).ToList();
                page.editid = Allname101[0].FMG_Id;
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public FeeGroupMappingDTO deleterecY(int id)
        {
            bool returnresult = false;
            FeeGroupMappingDTO page = new FeeGroupMappingDTO();
            List<FeeGroupMappingDMO> lorg = new List<FeeGroupMappingDMO>();
            lorg = _FeeGroupContext.feegm.Where(t => t.FMGG_Id.Equals(id)).ToList();
            bool resultvalu = deleterec(id);
            if (resultvalu == true)
            {
                try
                {
                    if (lorg.Any())
                    {
                        _FeeGroupContext.Remove(lorg.ElementAt(0));

                        var contactExists = _FeeGroupContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnresult = true;
                            page.returnval = returnresult;
                        }
                        else
                        {
                            returnresult = false;
                            page.returnval = returnresult;
                        }
                    }
                }
                catch (Exception ee)
                {
                    _logger.LogError(ee.Message);
                    Console.WriteLine(ee.Message);
                }
            }
            else
            {
                page.returnvalforeign = false;
            }


            return page;
        }
        public bool deleterec(long grpid)
        {
            FeegroupgroupingDTO te = new FeegroupgroupingDTO();
            FeeGroupGroupingDMO feepgeY = Mapper.Map<FeeGroupGroupingDMO>(te);
            bool returnresult = false;
            FeeGroupMappingDTO page = new FeeGroupMappingDTO();
            List<FeeGroupGroupingDMO> lorg = new List<FeeGroupGroupingDMO>();
            lorg = _FeeGroupContext.feeGGG.Where(t => t.FMGG_Id.Equals(grpid)).ToList();
            try
            {
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i;i++)
                    {
                        _FeeGroupContext.Remove(lorg.ElementAt(i));
                        var contactExists = _FeeGroupContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnresult = true;
                        }
                        else
                        {
                            returnresult = false;
                        }
                    }
                    
                }
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return returnresult;
        }
    }
}
