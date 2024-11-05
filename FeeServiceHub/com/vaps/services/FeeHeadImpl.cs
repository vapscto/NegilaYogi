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
    public class FeeHeadImpl : interfaces.FeeHeadInterface
    {
        private static ConcurrentDictionary<string, FeeHeadDTO> _login =
        new ConcurrentDictionary<string, FeeHeadDTO>();

        public FeeGroupContext _FeeGroupHeadContext;
        readonly ILogger<FeeHeadImpl> _logger;
        public FeeHeadImpl(FeeGroupContext frgContext, ILogger<FeeHeadImpl> log)
        {
            _logger = log;
            _FeeGroupHeadContext = frgContext;

        }
        //public FeeHeadContext _FeeGroupHeadContext;
        //public FeeHeadImpl(FeeHeadContext frgHeadContext)
        //{
        //    _FeeGroupHeadContext = frgHeadContext;
        //}
        public FeeHeadDTO SaveGroupData(FeeHeadDTO FGpage)
        {
            bool returnresult = false;
            FeeHeadDMO feepge = Mapper.Map<FeeHeadDMO>(FGpage);
            string retval = "";
            try
            {
                if (feepge.FMH_Id > 0)
                {

                    //var result1 = _FeeGroupHeadContext.feehead.Where(t => t.FMH_FeeName == feepge.FMH_FeeName && t.MI_Id == feepge.MI_Id && t.FMH_Order == feepge.FMH_Order).ToList();
                    var result1 = _FeeGroupHeadContext.feehead.Where(t => t.FMH_Id!= feepge.FMH_Id && t.FMH_FeeName == feepge.FMH_FeeName && t.MI_Id == feepge.MI_Id).ToList();
                    if (result1.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {
                        var result = _FeeGroupHeadContext.feehead.Single(t => t.FMH_Id == feepge.FMH_Id);
                        result.MI_Id = feepge.MI_Id;
                        result.FMH_FeeName = feepge.FMH_FeeName;
                        result.FMH_Flag = feepge.FMH_Flag;
                        result.FMH_Order = feepge.FMH_Order;
                        result.FMH_PDAFlag = feepge.FMH_PDAFlag;
                        result.FMH_RefundFlag = feepge.FMH_RefundFlag;
                        result.FMH_SpecialFeeFlag = feepge.FMH_SpecialFeeFlag;
                        result.FMH_ActiveFlag = feepge.FMH_ActiveFlag;
                        result.user_id = feepge.user_id;
                        result.FMH_UpdatedBy = feepge.user_id;
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
                   // var result = _FeeGroupHeadContext.feehead.Where(t => t.FMH_FeeName == feepge.FMH_FeeName && t.MI_Id== feepge.MI_Id && t.FMH_Order==feepge.FMH_Order).ToList();
                    var result = _FeeGroupHeadContext.feehead.Where(t => t.FMH_FeeName == feepge.FMH_FeeName && t.MI_Id == feepge.MI_Id).ToList();

                    if (result.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {
                        feepge.CreatedDate = DateTime.Now;
                        feepge.UpdatedDate = DateTime.Now;

                        feepge.FMH_CreatedBy = feepge.user_id;
                        feepge.FMH_UpdatedBy = feepge.user_id;

                        _FeeGroupHeadContext.Add(feepge);
                        var contactExists = _FeeGroupHeadContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            FeeHeadDTO dto = Mapper.Map<FeeHeadDTO>(feepge);
                            dto.FMH_Order = Convert.ToInt32(dto.FMH_Id);
                            var res = _FeeGroupHeadContext.feehead.Single(t => t.FMH_Id == dto.FMH_Id);
                            Mapper.Map(dto, res);
                            _FeeGroupHeadContext.Update(res);
                            _FeeGroupHeadContext.SaveChanges();

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
                List<FeeHeadDMO> allpages = new List<FeeHeadDMO>();
                allpages = _FeeGroupHeadContext.feehead.OrderBy(t => t.FMH_Order).ToList();
                FGpage.GroupHeadData = allpages.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGpage;
        }

        public FeeHeadDTO changeorderData(FeeHeadDTO dto)
        {
            bool returnresult = false;
            try
            {
                if (dto.CourseDTO.Count() > 0)
                {
                    foreach (FeeHeadDTO mob in dto.CourseDTO)
                    {
                        if (mob.FMH_Id > 0)
                        {
                            var result = _FeeGroupHeadContext.feehead.Single(t => t.FMH_Id.Equals(mob.FMH_Id));
                            Mapper.Map(mob, result);
                            _FeeGroupHeadContext.Update(result);
                            _FeeGroupHeadContext.SaveChanges();
                        }
                    }
                    returnresult = true;
                    dto.returnval = returnresult;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                returnresult = false;
                dto.returnval = returnresult;
            }
            return dto;
        }

        public FeeHeadDTO getdetails(int id)
        {
            FeeHeadDTO FGRDT = new FeeHeadDTO();

            try
            {
                List<FeeHeadDMO> feegrp = new List<FeeHeadDMO>();
                feegrp = _FeeGroupHeadContext.feehead.Where(t=>t.MI_Id==id).OrderBy(t => t.FMH_Order).ToList();
                FGRDT.GroupHeadData = feegrp.ToArray();


            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public FeeHeadDTO EditgroupDetails(int id)
        {
            FeeHeadDTO FMG = new FeeHeadDTO();
            try
            {
                List<FeeHeadDMO> masterfeegroup = new List<FeeHeadDMO>();
                masterfeegroup = _FeeGroupHeadContext.feehead.AsNoTracking().Where(t => t.FMH_Id.Equals(id)).ToList();
                FMG.GroupHeadData = masterfeegroup.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FMG;
        }
        public FeeHeadDTO GetGroupSearchData(FeeHeadDTO mas)
        {

            FeeHeadDTO FGRDT = new FeeHeadDTO();
            try
            {
                List<FeeHeadDMO> feegrp = new List<FeeHeadDMO>();
                feegrp = _FeeGroupHeadContext.feehead.OrderBy(t => t.FMH_Order).ToList();
                FGRDT.GroupHeadData = feegrp.ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public FeeHeadDTO getpageedit(int id)
        {
            FeeHeadDTO page = new FeeHeadDTO();
            try
            {
                List<FeeHeadDMO> lorg = new List<FeeHeadDMO>();
                lorg = _FeeGroupHeadContext.feehead.AsNoTracking().Where(t => t.FMH_Id.Equals(id)).ToList();
                page.GroupHeadData = lorg.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public FeeHeadDTO deleterec(int id)
        {
            bool returnresult = false;
            bool dupl = false;
            FeeHeadDTO page = new FeeHeadDTO();
            List<FeeYearlygroupHeadMappingDMO> lorgrecords = new List<FeeYearlygroupHeadMappingDMO>();
            lorgrecords = _FeeGroupHeadContext.FeeYearlygroupHeadMappingDMO.Where(t => t.FMH_Id.Equals(id)).ToList();
            if (lorgrecords.Count == 0)
            {

                List<FeeHeadDMO> lorg = new List<FeeHeadDMO>();
                lorg = _FeeGroupHeadContext.feehead.Where(t => t.FMH_Id.Equals(id)).ToList();

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

                    List<FeeHeadDMO> allpages = new List<FeeHeadDMO>();
                    allpages = _FeeGroupHeadContext.feehead.OrderBy(t => t.FMH_Order).ToList();
                    page.GroupHeadData = allpages.ToArray();
                }
                catch (Exception ee)
                {
                    _logger.LogError(ee.Message);
                    Console.WriteLine(ee.Message);
                }
            }
            else
            {
                dupl = false;
                page.dupr = dupl;
            }
            return page;
        }
        public FeeHeadDTO deactivate(FeeHeadDTO acd)
        {
            try
            {
                FeeHeadDMO feepge = Mapper.Map<FeeHeadDMO>(acd);
                if (feepge.FMH_Id > 0)
                {

                    var result = _FeeGroupHeadContext.feehead.Single(t => t.FMH_Id == feepge.FMH_Id);
                    var feestutrans = _FeeGroupHeadContext.FeeStudentTransactionDMO.Where(t => t.FMH_Id == feepge.FMH_Id).ToList();
                    if (feestutrans.Count > 0)
                    {
                        acd.message = "used";
                        return acd;
                    }
                    else
                    {
                        if (result.FMH_ActiveFlag == true)
                        {
                            result.FMH_ActiveFlag = false;
                        }
                        else
                        {
                            result.FMH_ActiveFlag = true;
                        }
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
                    
                    List<FeeHeadDMO> allorganisation = new List<FeeHeadDMO>();
                    allorganisation = _FeeGroupHeadContext.feehead.OrderBy(t => t.FMH_Order).ToList();
                    acd.GroupHeadData = allorganisation.ToArray();
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
