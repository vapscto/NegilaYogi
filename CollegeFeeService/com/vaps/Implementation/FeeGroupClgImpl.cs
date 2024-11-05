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

namespace CollegeFeeService.com.vaps.Implementation
{
    public class FeeGroupClgImpl : FeeGroupClgInterface
    {

        private static ConcurrentDictionary<string, FeeGroupClgDTO> _login =
           new ConcurrentDictionary<string , FeeGroupClgDTO>();

        private static ConcurrentDictionary<string, FeeYearlyGroupClgDTO> _login1 =
             new ConcurrentDictionary<string, FeeYearlyGroupClgDTO>();

        public CollFeeGroupContext _FeeGroupContext;
        readonly ILogger<FeeGroupClgImpl> _logger;
        public FeeGroupClgImpl(CollFeeGroupContext frgContext, ILogger<FeeGroupClgImpl> log)
        {
            _FeeGroupContext = frgContext;
            _logger = log;
        }
     
        public FeeGroupClgDTO SaveGroupData(FeeGroupClgDTO FGpage)
        {
            bool returnresult = false;
            string retval = "";
         
            FeeGroupClgDMO feepge = Mapper.Map<FeeGroupClgDMO>(FGpage);
            var fmccount = _FeeGroupContext.feemastersettings.Where(t => (t.FMC_CommonHostelFeeFlg == true || t.FMC_CommonTransportLocationFeeFlg == true) &&  t.MI_Id== FGpage.MI_Id).ToList();
            var feegroupcount = 0 ;


            if (fmccount.Count>0)
            {
                for(int i=0;i< fmccount.Count;i++)
                {
                    if(fmccount[i].FMC_CommonHostelFeeFlg==true)
                    {
                        var feegroup = _FeeGroupContext.FeeGroupClgDMO.Where(t => t.FMG_GroupName == FGpage.FMG_GroupName && t.FMG_ActiceFlag == true && t.MI_Id == FGpage.MI_Id).ToList();
                        if (feegroup.Count > 0)
                        {
                            feegroupcount = 1;
                        }
                       
                    }
                    if(fmccount[i].FMC_CommonTransportLocationFeeFlg == true)
                    {
                        var  feegroup = _FeeGroupContext.FeeGroupClgDMO.Where(t => t.FMG_GroupName == FGpage.FMG_GroupName && t.FMG_ActiceFlag == true && t.MI_Id == FGpage.MI_Id).ToList();
                        if (feegroup.Count > 0)
                        {
                            feegroupcount = 1;
                        }
                    }
                }

            }
            if(feegroupcount==0)
            {

         
         
            try
            {
                if (feepge.FMG_Id > 0)
                {
                    var res = _FeeGroupContext.FeeGroupClgDMO.Where(t => t.FMG_Id != feepge.FMG_Id && t.FMG_GroupName == feepge.FMG_GroupName && t.MI_Id == feepge.MI_Id && t.FMG_CompulsoryFlag == feepge.FMG_CompulsoryFlag && t.FMG_HostelFlg==feepge.FMG_HostelFlg && t.FMG_TransportFlg==feepge.FMG_TransportFlg && t.FMG_BatchwiseFeeApplFlg==feepge.FMG_BatchwiseFeeApplFlg && t.FMG_RegNewFlg==feepge.FMG_RegNewFlg).ToList();
                    if (res.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                        List<FeeGroupClgDMO> allpages = new List<FeeGroupClgDMO>();
                        allpages = _FeeGroupContext.FeeGroupClgDMO.Where(t => t.FMG_Id != feepge.FMG_Id && t.FMG_GroupName == feepge.FMG_GroupName && t.MI_Id == feepge.MI_Id && t.FMG_CompulsoryFlag == feepge.FMG_CompulsoryFlag).ToList();
                        FGpage.GroupData = allpages.OrderBy(t => t.FMG_GroupName).Distinct().ToArray();
                    }
                    else
                    {

                        var result = _FeeGroupContext.FeeGroupClgDMO.Single(t => t.FMG_Id == feepge.FMG_Id && t.MI_Id == feepge.MI_Id);
                        result.MI_Id = feepge.MI_Id;
                        result.FMG_GroupName = feepge.FMG_GroupName;
                        result.FMG_Remarks = feepge.FMG_Remarks;
                        result.FMG_CompulsoryFlag = feepge.FMG_CompulsoryFlag;
                        result.FMG_ActiceFlag = feepge.FMG_ActiceFlag;
                        result.user_id = feepge.user_id;
                        result.UpdatedDate = DateTime.Now;
                        result.FMG_BatchwiseFeeApplFlg = feepge.FMG_BatchwiseFeeApplFlg;
                        result.FMG_HostelFlg = feepge.FMG_HostelFlg;
                        result.FMG_TransportFlg = feepge.FMG_TransportFlg;
                        result.FMG_RegNewFlg = feepge.FMG_RegNewFlg;
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
                        List<FeeGroupClgDMO> allpages = new List<FeeGroupClgDMO>();
                        allpages = _FeeGroupContext.FeeGroupClgDMO.Where(t => t.MI_Id == feepge.MI_Id).ToList();
                        FGpage.GroupData = allpages.Distinct().ToArray();
                    }
                }
                else
                {
                    var result = _FeeGroupContext.FeeGroupClgDMO.Where(t => t.FMG_Id != feepge.FMG_Id && t.FMG_GroupName == feepge.FMG_GroupName && t.MI_Id == feepge.MI_Id && t.FMG_CompulsoryFlag == feepge.FMG_CompulsoryFlag && t.FMG_HostelFlg == feepge.FMG_HostelFlg && t.FMG_TransportFlg == feepge.FMG_TransportFlg && t.FMG_BatchwiseFeeApplFlg == feepge.FMG_BatchwiseFeeApplFlg && t.FMG_RegNewFlg==feepge.FMG_RegNewFlg).ToList();
                    if (result.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                        List<FeeGroupClgDMO> allpages = new List<FeeGroupClgDMO>();
                        allpages = _FeeGroupContext.FeeGroupClgDMO.Where(t => t.FMG_Id != feepge.FMG_Id && t.FMG_GroupName == feepge.FMG_GroupName && t.MI_Id == feepge.MI_Id && t.FMG_CompulsoryFlag == feepge.FMG_CompulsoryFlag).ToList();
                        FGpage.GroupData = allpages.OrderBy(t => t.FMG_GroupName).Distinct().ToArray();
                    }
                    else
                    {
                        feepge.CreatedDate = DateTime.Now;
                        feepge.UpdatedDate = DateTime.Now;
                        feepge.FMG_BatchwiseFeeApplFlg = feepge.FMG_BatchwiseFeeApplFlg;
                        feepge.FMG_HostelFlg = feepge.FMG_HostelFlg;
                        feepge.FMG_TransportFlg = feepge.FMG_TransportFlg;
                        feepge.FMG_RegNewFlg = feepge.FMG_RegNewFlg;
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
                        List<FeeGroupClgDMO> allpages = new List<FeeGroupClgDMO>();
                        allpages = _FeeGroupContext.FeeGroupClgDMO.Where(t => t.MI_Id == feepge.MI_Id).ToList();
                        FGpage.GroupData = allpages.Distinct().ToArray();
                    }
                }
                //List<FeeGroupClgDMO> allpages = new List<FeeGroupClgDMO>();
                //allpages = _FeeGroupContext.FeeGroupClgDMO.Where(t => t.MI_Id == feepge.MI_Id).OrderByDescending(t => t.CreatedDate).ToList();
                //FGpage.GroupData = allpages.ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            }
            else
            {
                retval = "Already";
                FGpage.returnduplicatestatus = retval;

            }
            return FGpage;
        }
        public FeeGroupClgDTO getdetails(FeeGroupClgDTO FGRDT)
        {
            //FeeGroupClgDTO FGRDT = new FeeGroupClgDTO();
            FeeYearlyGroupClgDTO fygdto = new FeeYearlyGroupClgDTO();

            try
            {
                List<FeeGroupClgDMO> feegrp = new List<FeeGroupClgDMO>();
                //feegrp = _FeeGroupContext.FeeGroupClgDMO.Where(t => t.MI_Id == FGRDT.MI_Id).OrderBy(t => t.FMG_GroupName).ToList();
                feegrp = _FeeGroupContext.FeeGroupClgDMO.Where(t => t.MI_Id == FGRDT.MI_Id).OrderByDescending(t => t.FMG_Id).ToList();
                FGRDT.GroupData = feegrp.Distinct().ToArray();
                FGRDT.arraychkgrp = feegrp.ToArray();

                List<FeeGroupClgDMO> feegrpact = new List<FeeGroupClgDMO>();
                feegrpact = _FeeGroupContext.FeeGroupClgDMO.Where(t => t.FMG_ActiceFlag.Equals(true) && t.MI_Id == FGRDT.MI_Id).ToList();
                FGRDT.activegrpnames = feegrpact.ToArray();

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                // allyear = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == FGRDT.MI_Id && t.ASMAY_Id == FGRDT.ASMAY_Id).ToList();
                allyear = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == FGRDT.MI_Id && t.Is_Active == true).ToList();
                FGRDT.academicdrp = allyear.Distinct().ToArray();

                List<FeeYearGroupClgDMO> feeyeargrp = new List<FeeYearGroupClgDMO>();
                feeyeargrp = _FeeGroupContext.FeeYearGroupDMO.Where(t => t.MI_Id == FGRDT.MI_Id).ToList();
                FGRDT.retriveYearlyGrpdata = feeyeargrp.Distinct().ToArray();

                FGRDT.newary = Getduedates(fygdto, FGRDT);

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public FeeGroupClgDTO EditgroupDetails(int id)
        {
            FeeGroupClgDTO FMG = new FeeGroupClgDTO();
            try
            {
                List<FeeGroupClgDMO> masterfeegroup = new List<FeeGroupClgDMO>();
                masterfeegroup = _FeeGroupContext.FeeGroupClgDMO.AsNoTracking().Where(t => t.FMG_Id.Equals(id)).ToList();

                FMG.GroupData = masterfeegroup.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FMG;
        }
        public FeeGroupClgDTO GetGroupSearchData(FeeGroupClgDTO mas)
        {

            FeeGroupClgDTO FGRDT = new FeeGroupClgDTO();
            try
            {
                List<FeeGroupClgDMO> feegrp = new List<FeeGroupClgDMO>();
                feegrp = _FeeGroupContext.FeeGroupClgDMO.ToList();
                FGRDT.GroupData = feegrp.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FGRDT;
        }
        public FeeGroupClgDTO getpageedit(int id)
        {
            FeeGroupClgDTO page = new FeeGroupClgDTO();
            try
            {
                List<FeeGroupClgDMO> lorg = new List<FeeGroupClgDMO>();
                lorg = _FeeGroupContext.FeeGroupClgDMO.AsNoTracking().Where(t => t.FMG_Id.Equals(id)).ToList();
                page.GroupData = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public FeeGroupClgDTO deleterec(int id)
        {
            bool returnresult = false;
            FeeGroupClgDTO page = new FeeGroupClgDTO();
            List<Fee_College_Master_Student_GroupHeadDMO> styu = new List<Fee_College_Master_Student_GroupHeadDMO>();
            styu = _FeeGroupContext.Fee_College_Master_Student_GroupHeadDMO.Where(t => t.FMG_Id.Equals(id)).ToList();
            if (styu.Count == 0)
            {
                List<CLG_Fee_Yearly_Group_Head_Mapping> pmm = new List<CLG_Fee_Yearly_Group_Head_Mapping>();
                pmm = _FeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping.Where(t => t.FMG_Id.Equals(id)).ToList();
                if (pmm.Count == 0)
                {
                    List<FeeGroupClgDMO> lorg = new List<FeeGroupClgDMO>();
                    lorg = _FeeGroupContext.FeeGroupClgDMO.Where(t => t.FMG_Id.Equals(id)).ToList();

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

                        List<FeeGroupClgDMO> allpages = new List<FeeGroupClgDMO>();
                        allpages = _FeeGroupContext.FeeGroupClgDMO.ToList();
                        page.GroupData = allpages.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                else
                {
                    page.retvalue = true;
                }
            }
            else
            {
                page.retvalue = true;
            }

            return page;
        }
        public FeeGroupClgDTO deactivate(FeeGroupClgDTO acd)
        {
            try
            {
                FeeGroupClgDMO feepge = Mapper.Map<FeeGroupClgDMO>(acd);
                if (feepge.FMG_Id > 0)
                {
                    var deletegroup = (from a in _FeeGroupContext.FeeGroupClgDMO
                                       from b in _FeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping
                                       where (a.FMG_Id == b.FMG_Id && b.MI_Id == acd.MI_Id && a.FMG_Id == feepge.FMG_Id)
                                       select new FeeGroupClgDTO
                                       {
                                           FMG_Id = Convert.ToInt32(a.FMG_Id)
                                       }
        ).OrderBy(t => t.FMG_Id).Take(1).Select(t => t.FMG_Id).ToList();

                    if (deletegroup.Count == 0)
                    {
                        var result = _FeeGroupContext.FeeGroupClgDMO.Single(t => t.FMG_Id == feepge.FMG_Id);
                        result.UpdatedDate = DateTime.Now;

                        if (result.FMG_ActiceFlag == true)
                        {
                            result.FMG_ActiceFlag = false;
                        }
                        else
                        {
                            result.FMG_ActiceFlag = true;
                        }
                        _FeeGroupContext.Update(result);
                        var flag = _FeeGroupContext.SaveChanges();
                        if (flag == 1)
                        {
                            acd.returnval = true;
                        }
                        else
                        {
                            acd.returnval = false;
                        }
                    }
                    else
                    {
                        acd.returnval = false;
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }


        //for yearly 


        [Route("years")]
        public async Task<FeeGroupClgDTO> getIndependentDropDowns(FeeGroupClgDTO yrs)
        {
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = await _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == yrs.MI_Id && t.Is_Active == true).ToListAsync();
                yrs.academicdrp = allyear.ToArray();


            }
            catch (Exception ex)
            {
                var str = ex.Message;
            }

            return yrs;
        }
        public FeeYearlyGroupClgDTO SaveYearlyGroupData(int id, FeeYearlyGroupClgDTO FGpage)
        {
            bool returnresult = false;

            string retval = "";
            try
            {
                FeeYearGroupClgDMO feepge = Mapper.Map<FeeYearGroupClgDMO>(FGpage);
                if (feepge.FYG_Id > 0)
                {
                    var result1 = _FeeGroupContext.FeeYearGroupDMO.Where(t => t.FYG_Id != feepge.FYG_Id && t.ASMAY_Id == feepge.ASMAY_Id && t.FMG_Id == feepge.FMG_Id);
                    if (result1.Count() >= 1)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {
                        var result = _FeeGroupContext.FeeYearGroupDMO.Single(t => t.FYG_Id == feepge.FYG_Id);
                        result.MI_Id = feepge.MI_Id;
                        result.ASMAY_Id = feepge.ASMAY_Id;
                        result.FMG_Id = id;
                        result.FYG_ActiveFlag = feepge.FYG_ActiveFlag;
                        result.user_id = feepge.user_id;

                        result.UpdatedDate = DateTime.Now;
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
                    }
                }
                else
                {
                    var result = _FeeGroupContext.FeeYearGroupDMO.Where(t => t.FYG_Id != feepge.FYG_Id && t.ASMAY_Id == feepge.ASMAY_Id && t.FMG_Id == feepge.FMG_Id);
                    if (result.Count() >= 1)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {
                        feepge.CreatedDate = DateTime.Now;
                        feepge.UpdatedDate = DateTime.Now;
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
                List<FeeYearGroupClgDMO> allpages1 = new List<FeeYearGroupClgDMO>();
                allpages1 = _FeeGroupContext.FeeYearGroupDMO.OrderByDescending(t => t.CreatedDate).ToList();
                FGpage.groupYearData = allpages1.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            //   }
            return FGpage;
        }
        public FeeYearlyGroupClgDTO getdetailsY(int id)
        {
            FeeYearlyGroupClgDTO FGRDT = new FeeYearlyGroupClgDTO();
            try
            {
                List<FeeYearGroupClgDMO> feegrp = new List<FeeYearGroupClgDMO>();
                feegrp = _FeeGroupContext.FeeYearGroupDMO.Where(t => t.MI_Id == id).OrderBy(t => t.FYG_Id).ToList();
                FGRDT.groupYearData = feegrp.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public FeeYearlyGroupClgDTO deactivateY(FeeYearlyGroupClgDTO acd)
        {
            try
            {
                FeeYearGroupClgDMO feepge = Mapper.Map<FeeYearGroupClgDMO>(acd);
                if (feepge.FYG_Id > 0)
                {

                    var deletegroup = (from a in _FeeGroupContext.FeeGroupClgDMO
                                       from b in _FeeGroupContext.Fee_College_Student_StatusDMO
                                       where (a.FMG_Id == b.FMG_Id && b.MI_Id == acd.MI_Id && b.ASMAY_Id == acd.ASMAY_Id)
                                       select new FeeGroupClgDTO
                                       {
                                           FMG_Id = Convert.ToInt32(a.FMG_Id)
                                       }
       ).OrderBy(t => t.FMG_Id).Take(1).Select(t => t.FMG_Id).ToList();

                    if (deletegroup.Count > 0)
                    {
                        var result = _FeeGroupContext.FeeYearGroupDMO.Single(t => t.FYG_Id == feepge.FYG_Id);

                        result.UpdatedDate = DateTime.Now;

                        if (result.FYG_ActiveFlag == true)
                        {
                            result.FYG_ActiveFlag = false;
                        }
                        else
                        {
                            result.FYG_ActiveFlag = true;
                        }
                        _FeeGroupContext.Update(result);
                        var flag = _FeeGroupContext.SaveChanges();
                        if (flag == 1)
                        {
                            acd.returnval = true;
                        }
                        else
                        {
                            acd.returnval = false;
                        }
                    }
                    else
                    {
                        acd.returnval = false;
                    }

                    List<FeeYearGroupClgDMO> allorganisation = new List<FeeYearGroupClgDMO>();
                    allorganisation = _FeeGroupContext.FeeYearGroupDMO.ToList();
                    acd.groupYearData = allorganisation.ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }
        public FeeYearlyGroupClgDTO getpageeditY(int id)
        {
            FeeYearlyGroupClgDTO page = new FeeYearlyGroupClgDTO();
            try
            {
                List<FeeYearGroupClgDMO> lorg = new List<FeeYearGroupClgDMO>();
                lorg = _FeeGroupContext.FeeYearGroupDMO.AsNoTracking().Where(t => t.FYG_Id.Equals(id)).ToList();
                page.groupYearData = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public FeeYearlyGroupClgDTO deleterecY(int id)
        {
            bool returnresult = false;
            FeeYearlyGroupClgDTO page = new FeeYearlyGroupClgDTO();
            List<CLG_Fee_Yearly_Group_Head_Mapping> pmm = new List<CLG_Fee_Yearly_Group_Head_Mapping>();
            pmm = _FeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping.Where(t => t.FMI_Id.Equals(id)).ToList();
            if (pmm.Count == 0)
            {
                List<FeeYearGroupClgDMO> lorg = new List<FeeYearGroupClgDMO>();
                lorg = _FeeGroupContext.FeeYearGroupDMO.Where(t => t.FYG_Id.Equals(id)).ToList();
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
                    List<FeeYearGroupClgDMO> allpages = new List<FeeYearGroupClgDMO>();
                    allpages = _FeeGroupContext.FeeYearGroupDMO.ToList();
                    page.groupYearData = allpages.ToArray();
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }
            else
            {
                page.retflg = true;
            }

            return page;
        }
        public FeeYearlyGroupClgDTO[] Getduedates(FeeYearlyGroupClgDTO mas, FeeGroupClgDTO data)
        {
            List<FeeYearlyGroupClgDTO> AllInOne = new List<FeeYearlyGroupClgDTO>();
            List<FeeYearGroupClgDMO> Allrows = new List<FeeYearGroupClgDMO>();
            Allrows = _FeeGroupContext.FeeYearGroupDMO.Where(t => t.MI_Id == data.MI_Id).OrderByDescending(t => t.FYG_Id).ToList();
            for (int i = 0; i < Allrows.Count; i++)
            {
                FeeYearlyGroupClgDTO temp = new FeeYearlyGroupClgDTO();
                List<FeeGroupClgDMO> Allname10 = new List<FeeGroupClgDMO>();
                Allname10 = _FeeGroupContext.FeeGroupClgDMO.Where(t => t.FMG_Id.Equals(Allrows[i].FMG_Id) && t.MI_Id == data.MI_Id).ToList().ToList();
                List<MasterAcademic> Allname101 = new List<MasterAcademic>();
                Allname101 = _FeeGroupContext.AcademicYear.Where(t => t.ASMAY_Id.Equals(Allrows[i].ASMAY_Id) && t.MI_Id == data.MI_Id).ToList().ToList();

                if (Allrows.Count > 0)
                {
                    temp.FYG_Id = Allrows[i].FYG_Id;
                }

                if (Allname10.Count > 0)
                {
                    temp.grpname = Allname10[0].FMG_GroupName;
                }
                if (Allname101.Count > 0)
                {
                    temp.yearname = Allname101[0].ASMAY_Year;
                }
                if (Allrows.Count > 0)
                {
                    temp.FYG_ActiveFlag = Allrows[i].FYG_ActiveFlag;
                    AllInOne.Add(temp);
                }
            }
            return AllInOne.ToArray();
        }

        public FeeYearlyGroupClgDTO selectacade(FeeYearlyGroupClgDTO data)
        {
            try
            {
                data.fillmastergroup = (from a in _FeeGroupContext.FeeGroupClgDMO
                                        from b in _FeeGroupContext.FeeYearGroupDMO
                                        where (b.ASMAY_Id == data.ASMAY_Id && a.FMG_ActiceFlag == true)
                                        select new FeeAmountEntryDTO
                                        {
                                            FMG_Id = a.FMG_Id,
                                            FMG_GroupName = a.FMG_GroupName,
                                        }
         ).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
