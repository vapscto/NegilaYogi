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
using DomainModel.Model.com.vapstech.Fee.Tally;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeGroupImplimentation : interfaces.FeeGroupInterface
    {

        private static ConcurrentDictionary<string, FeeGroupDTO> _login =
         new ConcurrentDictionary<string, FeeGroupDTO>();

        private static ConcurrentDictionary<string, FeeYearlyGroupDTO> _login1 =
             new ConcurrentDictionary<string, FeeYearlyGroupDTO>();

        public FeeGroupContext _FeeGroupContext;
        readonly ILogger<FeeGroupImplimentation> _logger;
        public FeeGroupImplimentation(FeeGroupContext frgContext, ILogger<FeeGroupImplimentation> log)
        {
            _FeeGroupContext = frgContext;
            _logger = log;
        }
        //public FeeGroupDTO SaveGroupData(FeeGroupDTO FGpage)
        //{
        //    bool returnresult = false;
        //    FeeGroupDMO feepge = Mapper.Map<FeeGroupDMO>(FGpage);
        //    // FeeGroupDMO feepge = new FeeGroupDMO();
        //    string retval = "";
        //    try
        //    {
        //        if (feepge.FMG_Id > 0)
        //        {

        //            var result = _FeeGroupContext.feeGroup.Single(t => t.FMG_Id == feepge.FMG_Id && t.MI_Id== feepge.MI_Id);
        //            result.MI_Id = feepge.MI_Id;
        //            result.FMG_GroupName = feepge.FMG_GroupName;
        //            result.FMG_Remarks = feepge.FMG_Remarks;
        //            result.FMG_CompulsoryFlag = feepge.FMG_CompulsoryFlag;
        //            result.FMG_ActiceFlag = feepge.FMG_ActiceFlag;
        //            result.user_id = feepge.user_id;
        //            result.UpdatedDate = DateTime.Now;
        //            _FeeGroupContext.Update(result);
        //            var contactExists = _FeeGroupContext.SaveChanges();
        //            if (contactExists == 1)
        //            {
        //                returnresult = true;
        //                FGpage.returnval = returnresult;
        //            }
        //            else
        //            {
        //                returnresult = false;
        //                FGpage.returnval = returnresult;
        //            }
        //        }
        //        else
        //        {
        //            var result = _FeeGroupContext.feeGroup.Where(t => t.FMG_GroupName == feepge.FMG_GroupName && t.MI_Id== feepge.MI_Id);
        //            if (result.Count() > 0)
        //            {
        //                retval = "Duplicate";
        //                FGpage.returnduplicatestatus = retval;
        //            }
        //            else
        //            {
        //                feepge.CreatedDate = DateTime.Now;
        //                feepge.UpdatedDate = DateTime.Now;
        //                _FeeGroupContext.Add(feepge);
        //                var contactExists = _FeeGroupContext.SaveChanges();
        //                if (contactExists == 1)
        //                {
        //                    returnresult = true;
        //                    FGpage.returnval = returnresult;
        //                }
        //                else
        //                {
        //                    returnresult = false;
        //                    FGpage.returnval = returnresult;
        //                }
        //            }
        //        }
        //        List<FeeGroupDMO> allpages = new List<FeeGroupDMO>();
        //        allpages = _FeeGroupContext.feeGroup.Where(t=>t.MI_Id== feepge.MI_Id).OrderByDescending(t => t.CreatedDate).ToList();
        //        FGpage.GroupData = allpages.ToArray();
        //    }
        //    catch (Exception ee)
        //    {
        //        _logger.LogError(ee.Message);
        //        Console.WriteLine(ee.Message);
        //    }
        //    return FGpage;
        //}

        //savedataFTally
        public Fee_FeeGroup_CompanyMappingDTO savedataFTally(Fee_FeeGroup_CompanyMappingDTO data)
        {
           
            try
            {              

                if (data.FFGCMA_Id > 0)
                {
                    var resultone = _FeeGroupContext.Fee_FeeGroup_CompanyMappingDMO.Where(R => R.FFGCMA_Id == data.FFGCMA_Id).FirstOrDefault();
                    for (int c = 0; c < data.TempararyArrayList.Length; c++)
                    {
                       
                        resultone.FTMCOM_Id = data.FTMCOM_Id;
                        resultone.FMG_Id = data.TempararyArrayList[c].FMG_Id;
                        _FeeGroupContext.Update(resultone);
                       
                    }
                    var contactExists = _FeeGroupContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        data.return_val = "Update";
                    }
                    else
                    {
                        data.return_val = "Notupdate";

                    }
                }
                else
                {
                    for (int c = 0; c < data.TempararyArrayList.Length; c++)
                    {
                       // var duplicate = 0;
                       var duplicate = _FeeGroupContext.Fee_FeeGroup_CompanyMappingDMO.Where(P => P.MI_Id == data.MI_Id && P.FMG_Id == data.TempararyArrayList[c].FMG_Id).ToList();
                        if (duplicate.Count == 0)
                        {
                            Fee_FeeGroup_CompanyMappingDMO obj = new Fee_FeeGroup_CompanyMappingDMO();
                            obj.FFGCMA_Id = data.FFGCMA_Id;
                            obj.MI_Id = data.MI_Id;
                            obj.FMG_Id = data.TempararyArrayList[c].FMG_Id;
                            obj.FTMCOM_Id = data.FTMCOM_Id;
                            obj.FFGCMA_ActiveId = true;
                            _FeeGroupContext.Add(obj);
                        }
                        else
                        {
                            if (data.return_val != "")
                            {
                                data.return_valtwo = data.TempararyArrayList[c].FMGG_GroupName;
                            }
                            else
                            {
                                data.return_valtwo = data.return_valtwo + ", " + data.TempararyArrayList[c].FMGG_GroupName;
                            }
                        }



                    }
                    var contactExists = _FeeGroupContext.SaveChanges();
                    if (contactExists == 1)
                    {

                        data.return_val = "save";
                    }
                    else
                    {
                        if (data.return_valtwo !=null)
                        {

                        }
                        else
                        {
                            data.return_val = "Notsave";
                        }
                       
                    }
                }
                
                   

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public FeeGroupDTO SaveGroupData(FeeGroupDTO FGpage)
        {
            bool returnresult = false;
            FeeGroupDMO feepge = Mapper.Map<FeeGroupDMO>(FGpage);
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            string retval = "";
            try
            {
                if (feepge.FMG_Id > 0)
                {
                    var res = _FeeGroupContext.feeGroup.Where(t =>t.FMG_Id!= feepge.FMG_Id && t.FMG_GroupName == feepge.FMG_GroupName && t.MI_Id == feepge.MI_Id && t.FMG_CompulsoryFlag==feepge.FMG_CompulsoryFlag).ToList();
                    if (res.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                        List<FeeGroupDMO> allpages = new List<FeeGroupDMO>();
                        allpages = _FeeGroupContext.feeGroup.Where(t => t.FMG_Id != feepge.FMG_Id && t.FMG_GroupName == feepge.FMG_GroupName && t.MI_Id == feepge.MI_Id && t.FMG_CompulsoryFlag == feepge.FMG_CompulsoryFlag).ToList();
                        FGpage.GroupData = allpages.OrderBy(t => t.FMG_GroupName).Distinct().ToArray();
                    }
                    else
                    {

                        var result = _FeeGroupContext.feeGroup.Single(t => t.FMG_Id == feepge.FMG_Id && t.MI_Id == feepge.MI_Id);
                        result.MI_Id = feepge.MI_Id;
                        result.FMG_GroupName = feepge.FMG_GroupName;
                        result.FMG_Remarks = feepge.FMG_Remarks;

                        if (feepge.FMG_CompulsoryFlag.Equals("N"))
                        {
                            result.FMG_PRE_FLAG = "1";
                        }
                        else
                        {
                            result.FMG_PRE_FLAG = "0";
                        }

                        result.FMG_CompulsoryFlag = feepge.FMG_CompulsoryFlag;
                        result.FMG_ActiceFlag = feepge.FMG_ActiceFlag;
                        result.user_id = feepge.user_id;
                        result.UpdatedDate = indianTime;
                        result.FMG_UpdatedBy = feepge.user_id;
                        result.FMG_HostelFlg = FGpage.FMG_HostelFlg;
                        result.FMG_TransportFlg = FGpage.FMG_TransportFlg;
                        result.FMG_RegNewFlg = FGpage.FMG_RegNewFlg;
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
                        List<FeeGroupDMO> allpages = new List<FeeGroupDMO>();
                        allpages = _FeeGroupContext.feeGroup.Where(t => t.MI_Id == feepge.MI_Id).ToList();
                        FGpage.GroupData = allpages.Distinct().ToArray();
                    }
                }
                else
                {
                    var result = _FeeGroupContext.feeGroup.Where(t => t.FMG_Id != feepge.FMG_Id && t.FMG_GroupName == feepge.FMG_GroupName && t.MI_Id == feepge.MI_Id).ToList();
                    if (result.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                        List<FeeGroupDMO> allpages = new List<FeeGroupDMO>();
                        allpages = _FeeGroupContext.feeGroup.Where(t => t.FMG_Id != feepge.FMG_Id && t.FMG_GroupName == feepge.FMG_GroupName && t.MI_Id == feepge.MI_Id && t.FMG_CompulsoryFlag == feepge.FMG_CompulsoryFlag).ToList();
                        FGpage.GroupData = allpages.OrderBy(t=>t.FMG_GroupName).Distinct().ToArray();
                    }
                    else
                    {
                        if(feepge.FMG_CompulsoryFlag.Equals("N"))
                        {
                            feepge.FMG_PRE_FLAG = "1";
                        }
                        else
                        {
                            feepge.FMG_PRE_FLAG = "0";
                        }
                        
                        feepge.CreatedDate = indianTime;
                        feepge.UpdatedDate = indianTime;
                        feepge.FMG_CreatedBy = feepge.user_id;
                        feepge.FMG_UpdatedBy = feepge.user_id;

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
                        List<FeeGroupDMO> allpages = new List<FeeGroupDMO>();
                        allpages = _FeeGroupContext.feeGroup.Where(t => t.MI_Id == feepge.MI_Id).ToList();
                        FGpage.GroupData = allpages.Distinct().ToArray();
                    }
                }
                //List<FeeGroupDMO> allpages = new List<FeeGroupDMO>();
                //allpages = _FeeGroupContext.feeGroup.Where(t => t.MI_Id == feepge.MI_Id).OrderByDescending(t => t.CreatedDate).ToList();
                //FGpage.GroupData = allpages.ToArray();
              
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGpage;
        }
        //deletedataYYY
        public Fee_FeeGroup_CompanyMappingDTO deletedataYYY(Fee_FeeGroup_CompanyMappingDTO data)
        {

            try
            {
                // var result=_FeeGroupContext.Fee_FeeGroup_CompanyMappingDMO.Where()
                
                if (data.FFGCMA_Id > 0)
                {
                    var resultone = _FeeGroupContext.Fee_FeeGroup_CompanyMappingDMO.Where(R => R.FFGCMA_Id == data.FFGCMA_Id).FirstOrDefault();
                    if (resultone.FFGCMA_ActiveId==true)
                    {
                        resultone.FFGCMA_ActiveId = false;
                    }
                    else
                    {
                        resultone.FFGCMA_ActiveId = true;
                    }
                    _FeeGroupContext.Update(resultone);
                    var i = _FeeGroupContext.SaveChanges();
                    if(i > 1)
                    {
                        data.return_val = "Delete";
                    }
                    else
                    {
                        data.return_val = "NotDelete";
                    }
                }
            



            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public FeeGroupDTO getdetails(FeeGroupDTO FGRDT)
        {
            //FeeGroupDTO FGRDT = new FeeGroupDTO();
            FGRDT.getarray = (from a in _FeeGroupContext.Fee_FeeGroup_CompanyMappingDMO
                              from b in _FeeGroupContext.feeGroup
                              from c in _FeeGroupContext.Fee_Tally_Master_CompanyDMO
                              where (a.FMG_Id == b.FMG_Id && a.MI_Id == FGRDT.MI_Id && a.MI_Id == b.MI_Id &&a.FTMCOM_Id==c.FTMCOM_Id && c.FTMCOM_ActiveId==true)
                              select new Fee_FeeGroup_CompanyMappingDTO
                              {
                                  FTMCOM_CompanyName=c.FTMCOM_CompanyName,
                                  FMG_GroupName=b.FMG_GroupName,
                                  FTMCOM_CompanyCode=c.FTMCOM_CompanyCode,
                                  FFGCMA_Id =a.FFGCMA_Id,
                                  FMGG_Id=a.FMG_Id,
                                  FTMCOM_Id=a.FTMCOM_Id,
                                  FFGCMA_ActiveId =a.FFGCMA_ActiveId

                              }
                            ).Distinct().ToArray();
            FGRDT.feeGroupname = _FeeGroupContext.Fee_Tally_Master_CompanyDMO.Where(R => R.FTMCOM_ActiveId == true && R.MI_Id==FGRDT.MI_Id).Distinct().ToArray();
            
            FeeYearlyGroupDTO fygdto = new FeeYearlyGroupDTO();
            try
            {
                List<FeeGroupDMO> feegrp = new List<FeeGroupDMO>();
                //feegrp = _FeeGroupContext.feeGroup.Where(t => t.MI_Id == FGRDT.MI_Id).OrderBy(t => t.FMG_GroupName).ToList();
                feegrp = _FeeGroupContext.feeGroup.Where(t => t.MI_Id == FGRDT.MI_Id).OrderByDescending(t=>t.FMG_Id).ToList();
                FGRDT.GroupData = feegrp.Distinct().ToArray();
                FGRDT.arraychkgrp = feegrp.ToArray();

                List<FeeGroupDMO> feegrpact = new List<FeeGroupDMO>();
                feegrpact = _FeeGroupContext.feeGroup.Where(t => t.FMG_ActiceFlag.Equals(true) && t.MI_Id == FGRDT.MI_Id).ToList();
                FGRDT.activegrpnames = feegrpact.ToArray();

                List<MasterAcademic> allyear = new List<MasterAcademic>();
           // allyear = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == FGRDT.MI_Id && t.ASMAY_Id == FGRDT.ASMAY_Id).ToList();
                allyear = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == FGRDT.MI_Id && t.Is_Active==true).OrderByDescending(o=>o.ASMAY_Order).ToList();
                FGRDT.academicdrp = allyear.Distinct().ToArray();

                List<FeeYearGroupDMO> feeyeargrp = new List<FeeYearGroupDMO>();
                feeyeargrp = _FeeGroupContext.Yearlygroups.Where(t => t.MI_Id == FGRDT.MI_Id).ToList();
                FGRDT.retriveYearlyGrpdata = feeyeargrp.Distinct().ToArray();

                FGRDT.newary = Getduedates(fygdto, FGRDT);
               // FGRDT.previlage = _FeeGroupContext.UserLoginPrivileges.Where(t => t.MI_Id == FGRDT.MI_Id && t.IVRMSTUUP_ActiveFlag == true && t.IVRMIMP_Id == FGRDT.pageid && t.Id == FGRDT.user_id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public FeeGroupDTO EditgroupDetails(int id)
        {
            FeeGroupDTO FMG = new FeeGroupDTO();
            try
            {
                List<FeeGroupDMO> masterfeegroup = new List<FeeGroupDMO>();
                masterfeegroup = _FeeGroupContext.feeGroup.AsNoTracking().Where(t => t.FMG_Id.Equals(id)).ToList();

                FMG.GroupData = masterfeegroup.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FMG;
        }
        public FeeGroupDTO GetGroupSearchData(FeeGroupDTO mas)
        {

            FeeGroupDTO FGRDT = new FeeGroupDTO();
            try
            {
                List<FeeGroupDMO> feegrp = new List<FeeGroupDMO>();
                feegrp = _FeeGroupContext.feeGroup.ToList();
                FGRDT.GroupData = feegrp.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FGRDT;
        }
        public FeeGroupDTO getpageedit(int id)
        {
            FeeGroupDTO page = new FeeGroupDTO();
            try
            {
                List<FeeGroupDMO> lorg = new List<FeeGroupDMO>();
                lorg = _FeeGroupContext.feeGroup.AsNoTracking().Where(t => t.FMG_Id.Equals(id)).ToList();
                page.GroupData = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public FeeGroupDTO deleterec(int id)
        {
            bool returnresult = false;
            FeeGroupDTO page = new FeeGroupDTO();
            List<FeeStudentGroupMappingDMO> styu = new List<FeeStudentGroupMappingDMO>();
            styu = _FeeGroupContext.FeeStudentGroupMappingDMO.Where(t => t.FMG_Id.Equals(id)).ToList();
            if (styu.Count == 0)
            {
                List<FeeYearlygroupHeadMappingDMO> pmm = new List<FeeYearlygroupHeadMappingDMO>();
                pmm = _FeeGroupContext.FeeYearlygroupHeadMappingDMO.Where(t => t.FMG_Id.Equals(id)).ToList();
                if (pmm.Count == 0)
                {
                    List<FeeGroupDMO> lorg = new List<FeeGroupDMO>();
                    lorg = _FeeGroupContext.feeGroup.Where(t => t.FMG_Id.Equals(id)).ToList();

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

                        List<FeeGroupDMO> allpages = new List<FeeGroupDMO>();
                        allpages = _FeeGroupContext.feeGroup.ToList();
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
        public FeeGroupDTO deactivate(FeeGroupDTO acd)
        {
            try
            {  //Commented By Praveen
                //        FeeGroupDMO feepge = Mapper.Map<FeeGroupDMO>(acd);
                //        if (feepge.FMG_Id > 0)
                //        {
                //            var deletegroup = (from a in _FeeGroupContext.FeeGroupDMO
                //                               from b in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                //                               where (a.FMG_Id==b.FMG_Id && b.MI_Id == acd.MI_Id && a.FMG_Id== feepge.FMG_Id)
                //                               select new FeeGroupDTO
                //                               {
                //                                   FMG_Id = Convert.ToInt32(a.FMG_Id)
                //                               }
                //).OrderBy(t => t.FMG_Id).Take(1).Select(t => t.FMG_Id).ToList();

                //            if(deletegroup.Count==0)
                //            {
                //                var result = _FeeGroupContext.feeGroup.Single(t => t.FMG_Id == feepge.FMG_Id);
                //                result.UpdatedDate = DateTime.Now;

                //                if (result.FMG_ActiceFlag == true)
                //                {
                //                    result.FMG_ActiceFlag = false;
                //                }
                //                else
                //                {
                //                    result.FMG_ActiceFlag = true;
                //                }
                //                _FeeGroupContext.Update(result);
                //                var flag = _FeeGroupContext.SaveChanges();
                //                if (flag == 1)
                //                {
                //                    acd.returnval = true;
                //                }
                //                else
                //                {
                //                    acd.returnval = false;
                //                }
                //            }
                //            else
                //            {
                //                acd.returnval = false;
                //            }

                //        }

                //Commented By Praveen End 
               
                if (acd.FMG_Id > 0)
                {
                    FeeGroupDMO feepge = Mapper.Map<FeeGroupDMO>(acd);

                    var deletegroup = (from a in _FeeGroupContext.FeeGroupDMO
                                       from b in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                       where (a.FMG_Id == b.FMG_Id && b.MI_Id == acd.MI_Id && a.FMG_Id == feepge.FMG_Id)
                                       select new FeeGroupDTO
                                       {
                                           FMG_Id = Convert.ToInt32(a.FMG_Id)
                                       }
                ).OrderBy(t => t.FMG_Id).Take(1).Select(t => t.FMG_Id).ToList();


                    var deletegroup1 = (from a in _FeeGroupContext.FeeGroupDMO
                                       from b in _FeeGroupContext.Yearlygroups
                                       where (a.FMG_Id == b.FMG_Id && b.MI_Id == acd.MI_Id && a.FMG_Id == acd.FMG_Id && b.FYG_ActiveFlag==true)
                                       select new FeeGroupDTO
                                       {
                                           FMG_Id = Convert.ToInt32(a.FMG_Id)
                                       }
        ).OrderBy(t => t.FMG_Id).Take(1).Select(t => t.FMG_Id).ToList();

                    if (deletegroup.Count == 0 && deletegroup1.Count == 0)
                    {
                        var result = _FeeGroupContext.feeGroup.Single(t => t.FMG_Id == acd.FMG_Id);
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
        public async Task<FeeGroupDTO> getIndependentDropDowns(FeeGroupDTO yrs)
        {
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = await _FeeGroupContext.AcademicYear.Where(t=>t.MI_Id==yrs.MI_Id && t.Is_Active==true).OrderByDescending(t=>t.ASMAY_Order).ToListAsync();
                yrs.academicdrp = allyear.ToArray();


            }
            catch (Exception ex)
            {
                var str = ex.Message;
            }

            return yrs;
        }
        public FeeYearlyGroupDTO SaveYearlyGroupData(int id, FeeYearlyGroupDTO FGpage)
        {
            bool returnresult = false;
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            string retval = "";
            try
            {
                FeeYearGroupDMO feepge = Mapper.Map<FeeYearGroupDMO>(FGpage);
                if (feepge.FYG_Id > 0)
                {
                    var result1 = _FeeGroupContext.Yearlygroups.Where(t => t.FYG_Id!= feepge.FYG_Id && t.ASMAY_Id == feepge.ASMAY_Id && t.FMG_Id == feepge.FMG_Id);
                    if (result1.Count() >= 1)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {
                        var result = _FeeGroupContext.Yearlygroups.Single(t => t.FYG_Id == feepge.FYG_Id);
                        result.MI_Id = feepge.MI_Id;
                        result.ASMAY_Id = feepge.ASMAY_Id;
                        result.FMG_Id = id;
                        result.FYG_ActiveFlag = feepge.FYG_ActiveFlag;
                        result.user_id = feepge.user_id;
                        result.FYG_UpdatedBy = feepge.user_id;
                        result.UpdatedDate = indianTime;
                        result.FYG_PartialRebateAmtOrPercentageValue = feepge.FYG_PartialRebateAmtOrPercentageValue;
                        result.FYG_RebateApplicableFlg = feepge.FYG_RebateApplicableFlg;
                        result.FYG_RebateTypeFlg = feepge.FYG_RebateTypeFlg;

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
                    var result = _FeeGroupContext.Yearlygroups.Where(t => t.FYG_Id != feepge.FYG_Id && t.ASMAY_Id == feepge.ASMAY_Id && t.FMG_Id == feepge.FMG_Id);
                    if (result.Count() >= 1)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {
                        feepge.CreatedDate = indianTime;
                        feepge.UpdatedDate = indianTime;
                        feepge.FYG_CreatedBy = feepge.user_id;
                        feepge.FYG_UpdatedBy = feepge.user_id;
                        feepge.FYG_PartialRebateAmtOrPercentageValue = feepge.FYG_PartialRebateAmtOrPercentageValue;
                        feepge.FYG_RebateApplicableFlg = feepge.FYG_RebateApplicableFlg;
                        feepge.FYG_RebateTypeFlg = feepge.FYG_RebateTypeFlg;

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
                List<FeeYearGroupDMO> allpages1 = new List<FeeYearGroupDMO>();
                allpages1 = _FeeGroupContext.Yearlygroups.OrderByDescending(t => t.CreatedDate).ToList();
                FGpage.groupYearData = allpages1.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            //   }
            return FGpage;
        }
        public FeeYearlyGroupDTO getdetailsY(int id)
        {
            FeeYearlyGroupDTO FGRDT = new FeeYearlyGroupDTO();
            try
            {
                List<FeeYearGroupDMO> feegrp = new List<FeeYearGroupDMO>();
                feegrp = _FeeGroupContext.Yearlygroups.Where(t => t.MI_Id == id).OrderBy(t => t.FYG_Id).ToList();
                FGRDT.groupYearData = feegrp.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public FeeYearlyGroupDTO deactivateY(FeeYearlyGroupDTO acd)
        {
            try
            {
                FeeYearGroupDMO feepge = Mapper.Map<FeeYearGroupDMO>(acd);
                if (feepge.FYG_Id > 0)
                {

       //             var deletegroup = ( from a in _FeeGroupContext.Yearlygroups
       //                                from b in _FeeGroupContext.FeeStudentTransactionDMO
       //                                where (a.FMG_Id == b.FMG_Id &&  b.MI_Id == acd.MI_Id && b.ASMAY_Id == acd.ASMAY_Id && a.ASMAY_Id==b.ASMAY_Id && b.FSS_ActiveFlag==true && a.FYG_Id == feepge.FYG_Id && a.MI_Id==b.MI_Id )
       //                                select new FeeGroupDTO
       //                                {
       //                                    FMG_Id = Convert.ToInt32(a.FMG_Id)
       //                                }
       //).OrderBy(t => t.FMG_Id).Take(1).Select(t => t.FMG_Id).ToList();

                    var deletegroup = (from a in _FeeGroupContext.Yearlygroups
                                       from c in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                       where ( a.FMG_Id == c.FMG_Id && c.MI_Id == acd.MI_Id && c.ASMAY_Id == acd.ASMAY_Id && a.ASMAY_Id == c.ASMAY_Id && c.FYGHM_ActiveFlag == "1" && a.FYG_Id == feepge.FYG_Id && a.MI_Id == c.MI_Id)
                                       select new FeeGroupDTO
                                       {
                                           FMG_Id = Convert.ToInt32(a.FMG_Id)
                                       }
      ).OrderBy(t => t.FMG_Id).Take(1).Select(t => t.FMG_Id).ToList();

                    if (deletegroup.Count==0)
                    {
                        var result = _FeeGroupContext.Yearlygroups.Single(t => t.FYG_Id == feepge.FYG_Id);

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

                    List<FeeYearGroupDMO> allorganisation = new List<FeeYearGroupDMO>();
                    allorganisation = _FeeGroupContext.Yearlygroups.ToList();
                    acd.groupYearData = allorganisation.ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }
        public FeeYearlyGroupDTO getpageeditY(int id)
        {
            FeeYearlyGroupDTO page = new FeeYearlyGroupDTO();
            try
            {
                List<FeeYearGroupDMO> lorg = new List<FeeYearGroupDMO>();
                lorg = _FeeGroupContext.Yearlygroups.AsNoTracking().Where(t => t.FYG_Id.Equals(id)).ToList();
                page.groupYearData = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public FeeYearlyGroupDTO deleterecY(int id)
        {
            bool returnresult = false;
            FeeYearlyGroupDTO page = new FeeYearlyGroupDTO();
            List<FeeYearlygroupHeadMappingDMO> pmm = new List<FeeYearlygroupHeadMappingDMO>();
            pmm = _FeeGroupContext.FeeYearlygroupHeadMappingDMO.Where(t => t.FMI_Id.Equals(id)).ToList();
            if (pmm.Count == 0)
            {
                List<FeeYearGroupDMO> lorg = new List<FeeYearGroupDMO>();
                lorg = _FeeGroupContext.Yearlygroups.Where(t => t.FYG_Id.Equals(id)).ToList();
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
                    List<FeeYearGroupDMO> allpages = new List<FeeYearGroupDMO>();
                    allpages = _FeeGroupContext.Yearlygroups.ToList();
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
        public FeeYearlyGroupDTO[] Getduedates(FeeYearlyGroupDTO mas, FeeGroupDTO data)
        {
            List<FeeYearlyGroupDTO> AllInOne = new List<FeeYearlyGroupDTO>();
            List<FeeYearGroupDMO> Allrows = new List<FeeYearGroupDMO>();
            Allrows = _FeeGroupContext.Yearlygroups.Where(t => t.MI_Id == data.MI_Id).OrderByDescending(t=>t.FYG_Id).ToList() ;
            for (int i = 0; i < Allrows.Count; i++)
            {
                FeeYearlyGroupDTO temp = new FeeYearlyGroupDTO();
                List<FeeGroupDMO> Allname10 = new List<FeeGroupDMO>();
                Allname10 = _FeeGroupContext.feeGroup.Where(t => t.FMG_Id.Equals(Allrows[i].FMG_Id) && t.MI_Id== data.MI_Id).ToList().ToList();
                List<MasterAcademic> Allname101 = new List<MasterAcademic>();
                Allname101 = _FeeGroupContext.AcademicYear.Where(t => t.ASMAY_Id.Equals(Allrows[i].ASMAY_Id) && t.MI_Id==data.MI_Id).ToList().ToList();

                if(Allrows.Count>0)
                {
                    temp.FYG_Id = Allrows[i].FYG_Id;
                    temp.FYG_PartialRebateAmtOrPercentageValue = Allrows[i].FYG_PartialRebateAmtOrPercentageValue;
                    temp.FYG_RebateApplicableFlg = Allrows[i].FYG_RebateApplicableFlg;
                    temp.FYG_RebateTypeFlg = Allrows[i].FYG_RebateTypeFlg;

                }

                if (Allname10.Count > 0)
                {
                    temp.grpname = Allname10[0].FMG_GroupName;
                }
                if (Allname101.Count > 0)
                {
                    temp.ASMAY_Id= Allname101[0].ASMAY_Id;
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

        public FeeYearlyGroupDTO selectacade(FeeYearlyGroupDTO data)
        {
            try
            {
                data.fillmastergroup = (from a in _FeeGroupContext.FeeGroupDMO
                                        from b in _FeeGroupContext.Yearlygroups
                                        where (b.ASMAY_Id == data.ASMAY_Id && a.FMG_ActiceFlag == true)
                                        select new FeeAmountEntryDTO
                                        {
                                            FMG_Id = a.FMG_Id,
                                            FMG_GroupName = a.FMG_GroupName,
                                        }
         ).ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
