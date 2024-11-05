using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.College.Admission;
using Microsoft.Extensions.Logging;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using System.Collections.Concurrent;
using DomainModel.Model.com.vapstech.College.Admission;
using AutoMapper;
using PreadmissionDTOs.com.vaps.College.Fee;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model.com.vapstech.College.Fees;
using CollegeFeeService.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.College.Fees;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class ClgQuotaFeeGroupImpl:Interfaces.ClgQuotaFeeGroupInterface
    {
        private static ConcurrentDictionary<string, FeeGroupClgDTO> _login =
         new ConcurrentDictionary<string, FeeGroupClgDTO>();

        private static ConcurrentDictionary<string, FeeYearlyGroupClgDTO> _login1 =
             new ConcurrentDictionary<string, FeeYearlyGroupClgDTO>();

        public CollFeeGroupContext _FeeGroupContext;
        readonly ILogger<ClgQuotaFeeGroupImpl> _logger;
        public ClgQuotaFeeGroupImpl(CollFeeGroupContext frgContext, ILogger<ClgQuotaFeeGroupImpl> log)
        {
            _FeeGroupContext = frgContext;
            _logger = log;
        }

        public ClgQuotaFeeGroupDTO SaveGroupData(int id, ClgQuotaFeeGroupDTO FGpage)
        {
            bool returnresult = false;
            ClgQuotaFeeGroupDMO feepge = Mapper.Map<ClgQuotaFeeGroupDMO>(FGpage);
      
            string retval = "";
           


                try
                {
               
                    if (feepge.FCQCFG_Id > 0)
                    {
           var res = _FeeGroupContext.ClgQuotaFeeGroupDMO.Where(t => t.FCQCFG_Id == feepge.FCQCFG_Id && t.FMG_Id == feepge.FMG_Id && t.MI_Id == feepge.MI_Id &&  t.ACQC_Id == feepge.ACQC_Id && t.FCQCFG_CompulsoryFlg== FGpage.FCQCFG_CompulsoryFlg).ToList();
                        if (res.Count() > 0)
                        {
                            retval = "Duplicate";
                            FGpage.returnduplicatestatus = retval;
                                       }
                        else
                        {

                            var result = _FeeGroupContext.ClgQuotaFeeGroupDMO.Single(t => t.FCQCFG_Id == feepge.FCQCFG_Id );
                            result.MI_Id = FGpage.MI_Id;
                            result.ACQC_Id = FGpage.ACQC_Id;
                            result.FCQCFG_CompulsoryFlg = FGpage.FCQCFG_CompulsoryFlg;
                            result.FMG_Id = id;
                            result.FCQCFG_ActiveFlg = true;
                            result.FCQCFG_UpdatedBy = FGpage.user_id;
                            result.FCQCFG_UpdatedDate = DateTime.Now;
                            //result.FCQCFG_CreatedDate = DateTime.Now;
                            //result.FCQCFG_CreatedBy = feepge.FCQCFG_CreatedBy;

                            _FeeGroupContext.Update(result);
                            var contactExists = _FeeGroupContext.SaveChanges();
                            if (contactExists == 1)
                            {
                                retval = "Update";
                                FGpage.returnduplicatestatus = retval;
                                FGpage.message = "Update";
                            }
                            else
                            {
                                retval = "NotUpdate";
                                FGpage.returnduplicatestatus = retval;
                            }
                            List<ClgQuotaFeeGroupDMO> allpages = new List<ClgQuotaFeeGroupDMO>();
                            allpages = _FeeGroupContext.ClgQuotaFeeGroupDMO.Where(t => t.MI_Id == feepge.MI_Id).ToList();
                            FGpage.GroupData = allpages.Distinct().ToArray();
                        }
                    }
                    else
                    {
                        var result = _FeeGroupContext.ClgQuotaFeeGroupDMO.Where(t => t.FCQCFG_Id != feepge.FCQCFG_Id && t.FMG_Id == feepge.FMG_Id && t.MI_Id == feepge.MI_Id && t.FCQCFG_CompulsoryFlg == feepge.FCQCFG_CompulsoryFlg && t.ACQC_Id == feepge.ACQC_Id).ToList();
                        if (result.Count() > 0)
                        {
                            retval = "Duplicate";
                            FGpage.returnduplicatestatus = retval;

                    }
                        else
                        {
                            feepge.MI_Id = FGpage.MI_Id;
                            feepge.ACQC_Id = FGpage.ACQC_Id;
                            feepge.FCQCFG_CompulsoryFlg = FGpage.FCQCFG_CompulsoryFlg;
                            feepge.FMG_Id =id;
                            feepge.FCQCFG_ActiveFlg = true;
                            feepge.FCQCFG_UpdatedBy = FGpage.user_id;
                            feepge.FCQCFG_UpdatedDate = DateTime.Now;
                            feepge.FCQCFG_CreatedDate = DateTime.Now;

                            feepge.FCQCFG_CreatedBy = FGpage.user_id;
                            _FeeGroupContext.Add(feepge);
                            var contactExists = _FeeGroupContext.SaveChanges();
                            if (contactExists == 1)
                            {
                                retval = "Save";
                                FGpage.returnduplicatestatus = retval;
                            }
                            else
                            {
                                retval = "NotSave";
                                FGpage.returnduplicatestatus = retval;
                            }

                        }
                    }

                   
             
                
                FGpage.GroupData = (from a in _FeeGroupContext.ClgQuotaFeeGroupDMO
                                   from b in _FeeGroupContext.FeeGroupClgDMO
                                   from c in _FeeGroupContext.Clg_Adm_College_Quota_CategoryDMO
                                   where (a.FMG_Id == b.FMG_Id && a.ACQC_Id == c.ACQC_Id &&  b.FMG_ActiceFlag == true && c.ACQC_ActiveFlg == true)
                                   select new ClgQuotaFeeGroupDTO
                                   {
                                       FMG_GroupName = b.FMG_GroupName,
                                       ACQC_CategoryName = c.ACQC_CategoryName,
                                       FCQCFG_Id = a.FCQCFG_Id,
                                       FCQCFG_CompulsoryFlg = a.FCQCFG_CompulsoryFlg,
                                       FCQCFG_ActiveFlg = a.FCQCFG_ActiveFlg
                                   }

                                 ).ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            
            return FGpage;
        }
        public ClgQuotaFeeGroupDTO getdetails(ClgQuotaFeeGroupDTO FGRDT)
        {
            //ClgQuotaFeeGroupDTO FGRDT = new ClgQuotaFeeGroupDTO();
           
            try
            {
                //List<ClgQuotaFeeGroupDMO> feegrp = new List<ClgQuotaFeeGroupDMO>();
                var feegrp = _FeeGroupContext.FeeGroupClgDMO.Where(t => t.MI_Id == FGRDT.MI_Id && t.FMG_ActiceFlag==true).OrderBy(t => t.FMG_GroupName).ToList();
                //feegrp = _FeeGroupContext.FeeGroupClgDMO.Where(t => t.MI_Id == FGRDT.MI_Id).OrderByDescending(t => t.FMG_Id).ToList();
                
                FGRDT.feegroup = feegrp.ToArray();

               
                List<Clg_Adm_College_Quota_CategoryDMO> Categorylist = new List<Clg_Adm_College_Quota_CategoryDMO>();
                Categorylist = _FeeGroupContext.Clg_Adm_College_Quota_CategoryDMO.Where(t => t.MI_Id == FGRDT.MI_Id && t.ACQC_ActiveFlg==true).ToList();
                FGRDT.Category = Categorylist.Distinct().ToArray();


                FGRDT.GroupData = (from a in _FeeGroupContext.ClgQuotaFeeGroupDMO
                                   from b in _FeeGroupContext.FeeGroupClgDMO
                                   from c in _FeeGroupContext.Clg_Adm_College_Quota_CategoryDMO
                                   where (a.FMG_Id == b.FMG_Id && a.ACQC_Id == c.ACQC_Id && b.FMG_ActiceFlag == true && c.ACQC_ActiveFlg==true)
                                    select new ClgQuotaFeeGroupDTO
                                    {
                                        FMG_GroupName = b.FMG_GroupName,
                                        ACQC_CategoryName = c.ACQC_CategoryName,
                                        FCQCFG_Id = a.FCQCFG_Id,
                                        FCQCFG_CompulsoryFlg = a.FCQCFG_CompulsoryFlg,
                                        FCQCFG_ActiveFlg = a.FCQCFG_ActiveFlg
                                    }  

                                   ).ToArray();




            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public ClgQuotaFeeGroupDTO EditgroupDetails(int id)
        {
            ClgQuotaFeeGroupDTO FMG = new ClgQuotaFeeGroupDTO();
            try
            {
                List<ClgQuotaFeeGroupDMO> masterfeegroup = new List<ClgQuotaFeeGroupDMO>();
                masterfeegroup = _FeeGroupContext.ClgQuotaFeeGroupDMO.AsNoTracking().Where(t => t.FCQCFG_Id.Equals(id)).ToList();

                FMG.GroupData = masterfeegroup.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FMG;
        }
        public ClgQuotaFeeGroupDTO GetGroupSearchData(ClgQuotaFeeGroupDTO mas)
        {

            ClgQuotaFeeGroupDTO FGRDT = new ClgQuotaFeeGroupDTO();
            try
            {
                List<ClgQuotaFeeGroupDMO> feegrp = new List<ClgQuotaFeeGroupDMO>();
                feegrp = _FeeGroupContext.ClgQuotaFeeGroupDMO.ToList();
                FGRDT.GroupData = feegrp.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FGRDT;
        }
        public ClgQuotaFeeGroupDTO getpageedit(int id)
        {
            ClgQuotaFeeGroupDTO page = new ClgQuotaFeeGroupDTO();
            try
            {
                List<ClgQuotaFeeGroupDMO> lorg = new List<ClgQuotaFeeGroupDMO>();
                lorg = _FeeGroupContext.ClgQuotaFeeGroupDMO.AsNoTracking().Where(t => t.FCQCFG_Id.Equals(id)).ToList();
                page.GroupData = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
      
        public ClgQuotaFeeGroupDTO deactivate(ClgQuotaFeeGroupDTO acd)
        {
            try
            {
                ClgQuotaFeeGroupDMO feepge = Mapper.Map<ClgQuotaFeeGroupDMO>(acd);
                if (feepge.FCQCFG_Id > 0)
                {



                    var result = _FeeGroupContext.ClgQuotaFeeGroupDMO.Single(t => t.FCQCFG_Id == feepge.FCQCFG_Id);
                    result.FCQCFG_UpdatedBy = acd.user_id;

                    if (result.FCQCFG_ActiveFlg == true)
                    {
                        result.FCQCFG_ActiveFlg = false;
                        acd.confirmmgs = "Deactivated";


                    }
                    else
                    {
                        result.FCQCFG_ActiveFlg = true;
                        acd.confirmmgs = "Activated";
                    }
                    _FeeGroupContext.Update(result);
                    
                    var flag = _FeeGroupContext.SaveChanges();
                    acd.returnval = "true";
                }  

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                
            }
            return acd;
        }




       
      
    }
}
