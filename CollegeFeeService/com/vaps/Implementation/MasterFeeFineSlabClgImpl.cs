using System;
using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Fees;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CollegeFeeService.com.vaps.Interfaces;

using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class MasterFeeFineSlabClgImpl:Interfaces.MasterFeeFineSlabClgInterface
    {
        private static ConcurrentDictionary<string, MasterFeeFineSlabClg_DTO> _login=new ConcurrentDictionary<string, MasterFeeFineSlabClg_DTO>();

        public CollFeeGroupContext _FeeGroupHeadContext;
        readonly ILogger<MasterFeeFineSlabClgImpl> _logger;
        public MasterFeeFineSlabClgImpl(CollFeeGroupContext frgContext, ILogger<MasterFeeFineSlabClgImpl> log)
        {
            _FeeGroupHeadContext = frgContext;
            _logger = log;
        }

        public MasterFeeFineSlabClg_DTO SaveGroupData(MasterFeeFineSlabClg_DTO FGpage)
        {
            bool returnresult = false;
            FeeFineSlabDMO feepge = Mapper.Map<FeeFineSlabDMO>(FGpage);
            string retval = "";
            try
            {  
                if (feepge.FMFS_Id > 0)
                {
                    var res = _FeeGroupHeadContext.feeFS.Where(t => t.FMFS_FineType == feepge.FMFS_FineType && t.FMFS_FromDay == feepge.FMFS_FromDay && t.FMFS_ToDay == feepge.FMFS_ToDay && t.MI_Id == feepge.MI_Id && t.FMFS_ECSFlag == feepge.FMFS_ECSFlag).ToList();
                    if (res.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                        FGpage.returnval = false;
                    }
                    else
                    {
                        var result = _FeeGroupHeadContext.feeFS.Single(t => t.FMFS_Id == feepge.FMFS_Id);
                        result.MI_Id = feepge.MI_Id;
                        result.FMFS_FineType = feepge.FMFS_FineType;
                        result.FMFS_FromDay = feepge.FMFS_FromDay;
                        result.FMFS_ToDay = feepge.FMFS_ToDay;
                        result.FMFS_ECSFlag = feepge.FMFS_ECSFlag;
                        result.FMFS_ActiveFlag = feepge.FMFS_ActiveFlag;
                        result.UpdatedDate = DateTime.Now;
                        _FeeGroupHeadContext.Update(result);
                        var contactExists = _FeeGroupHeadContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnresult = true;
                            FGpage.returnval = returnresult;
                            FGpage.message = "Update";
                        }
                        else
                        {
                            returnresult = false;
                            FGpage.returnval = returnresult;
                        }
                    }
                }
                else
                {
                    var result = _FeeGroupHeadContext.feeFS.Where(t => t.FMFS_FineType == feepge.FMFS_FineType && t.FMFS_FromDay == feepge.FMFS_FromDay && t.FMFS_ToDay == feepge.FMFS_ToDay && t.MI_Id == feepge.MI_Id && t.FMFS_ECSFlag == feepge.FMFS_ECSFlag).ToList();
                    if (result.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                        FGpage.returnval = false;
                    }
                    else
                    {
                        feepge.CreatedDate = DateTime.Now;
                        feepge.UpdatedDate = DateTime.Now;
                        _FeeGroupHeadContext.Add(feepge);
                        var contactExists = _FeeGroupHeadContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnresult = true;
                            FGpage.returnval = returnresult;
                        }
                        else
                        {
                            returnresult = false;
                            FGpage.returnval = returnresult;
                        }
                    }
                }
                List<FeeFineSlabDMO> allpages = new List<FeeFineSlabDMO>();
                allpages = _FeeGroupHeadContext.feeFS.Where(t => t.MI_Id == feepge.MI_Id).OrderByDescending(t => t.CreatedDate).ToList();
                FGpage.GroupFineSlab = allpages.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGpage;
        }
        public MasterFeeFineSlabClg_DTO getdetails(int id)
        {
            MasterFeeFineSlabClg_DTO FGRDT = new MasterFeeFineSlabClg_DTO();

            try
            {
                List<FeeFineSlabDMO> feegrp = new List<FeeFineSlabDMO>();
                feegrp = _FeeGroupHeadContext.feeFS.Where(t => t.MI_Id == id).OrderByDescending(t => t.CreatedDate).ToList();
                FGRDT.GroupFineSlab = feegrp.ToArray();


            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public MasterFeeFineSlabClg_DTO EditgroupDetails(int id)
        {
            MasterFeeFineSlabClg_DTO FMG = new MasterFeeFineSlabClg_DTO();
            try
            {
                List<FeeFineSlabDMO> masterfeegroup = new List<FeeFineSlabDMO>();
                masterfeegroup = _FeeGroupHeadContext.feeFS.AsNoTracking().Where(t => t.FMFS_Id.Equals(id)).ToList();
                FMG.GroupFineSlab = masterfeegroup.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FMG;
        }
        public MasterFeeFineSlabClg_DTO GetGroupSearchData(MasterFeeFineSlabClg_DTO mas)
        {

            MasterFeeFineSlabClg_DTO FGRDT = new MasterFeeFineSlabClg_DTO();
            try
            {
                List<FeeFineSlabDMO> feegrp = new List<FeeFineSlabDMO>();
                feegrp = _FeeGroupHeadContext.feeFS.ToList();
                FGRDT.GroupFineSlab = feegrp.ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public MasterFeeFineSlabClg_DTO getpageedit(int id)
        {
            MasterFeeFineSlabClg_DTO page = new MasterFeeFineSlabClg_DTO();
            try
            {
                List<FeeFineSlabDMO> lorg = new List<FeeFineSlabDMO>();
                lorg = _FeeGroupHeadContext.feeFS.AsNoTracking().Where(t => t.FMFS_Id.Equals(id)).ToList();
                page.GroupFineSlab = lorg.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public MasterFeeFineSlabClg_DTO deleterec(int id)
        {
            bool returnresult = false;
            MasterFeeFineSlabClg_DTO page = new MasterFeeFineSlabClg_DTO();
            List<FeeFineSlabDMO> lorg = new List<FeeFineSlabDMO>();
            lorg = _FeeGroupHeadContext.feeFS.Where(t => t.FMFS_Id.Equals(id)).ToList();

            try
            {
                if (lorg.Any())
                {
                    _FeeGroupHeadContext.Remove(lorg.ElementAt(0));
                    var contactExists = _FeeGroupHeadContext.SaveChanges();
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

                List<FeeFineSlabDMO> allpages = new List<FeeFineSlabDMO>();
                allpages = _FeeGroupHeadContext.feeFS.ToList();
                page.GroupFineSlab = allpages.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public MasterFeeFineSlabClg_DTO deactivate(MasterFeeFineSlabClg_DTO acd)
        {
            try
            {
                FeeFineSlabDMO feepge = Mapper.Map<FeeFineSlabDMO>(acd);
                acd.message = "";
                if (feepge.FMFS_Id > 0)
                {
                    var result = _FeeGroupHeadContext.feeFS.Single(t => t.FMFS_Id == feepge.FMFS_Id);
                    if (result.FMFS_ActiveFlag == true)
                    {

                        //List<FeeTFineSlabDMO> resultnew = _FeeGroupHeadContext.feeTFineSlabDMO.Where(k => k.FMFS_Id.Equals(feepge.FMFS_Id)).ToList();
                        var resultnew = _FeeGroupHeadContext.CLG_Fee_College_T_Fine_Slabs.Where(k => k.FMFS_Id == feepge.FMFS_Id).Select(t => t.FMFS_Id).ToList();
                    
                        if (resultnew.Count > 0)
                        {
                            acd.message = "used";
                            return acd;
                        }
                        else
                        {
                            result.FMFS_ActiveFlag = false;
                        }
                    }
                    else
                    {
                        result.FMFS_ActiveFlag = true;
                    }
                    if (acd.message != "used")
                    {
                        result.UpdatedDate = DateTime.Now;
                        _FeeGroupHeadContext.Update(result);
                        var flag = _FeeGroupHeadContext.SaveChanges();
                        if (flag == 1)
                        {
                            acd.returnval = true;
                        }
                        else
                        {
                            acd.returnval = false;
                        }
                    }
                    List<FeeFineSlabDMO> allorganisation = new List<FeeFineSlabDMO>();
                    allorganisation = _FeeGroupHeadContext.feeFS.ToList();
                    acd.GroupFineSlab = allorganisation.ToArray();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }
    }
}
