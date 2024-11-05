using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Fee;
using FeeServiceHub.com.vaps.interfaces;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeTermWiseRebateSettingImpl : interfaces.FeeTermWiseRebateSettingInterface
    {
        private static ConcurrentDictionary<string, FeeTermWiseRebateSettingDTO> _login =
       new ConcurrentDictionary<string, FeeTermWiseRebateSettingDTO>();

        public FeeGroupContext _FeeGroupHeadContext;

        public FeeTermWiseRebateSettingImpl(FeeGroupContext frgContext)
        {

            _FeeGroupHeadContext = frgContext;

        }

        public FeeTermWiseRebateSettingDTO SaveGroupData(FeeTermWiseRebateSettingDTO FGpage)
        {
            bool returnresult = false;
            FeeTermWiseRebateSettingDMO feepge = Mapper.Map<FeeTermWiseRebateSettingDMO>(FGpage);
            string retval = "";
            try
            {
                if (feepge.FMTRS_Id > 0)
                {

                    //var result1 = _FeeGroupHeadContext.FeeTermWiseRebateSettingDMO.Where(t => t.FMH_FeeName == feepge.FMH_FeeName && t.FMT_Id == feepge.FMT_Id && t.FMH_Order == feepge.FMH_Order).ToList();
                    var result1 = _FeeGroupHeadContext.FeeTermWiseRebateSettingDMO.Where(t => t.FMTRS_Id != feepge.FMTRS_Id && t.FMT_Id == feepge.FMT_Id && t.ASMAY_Id == feepge.ASMAY_Id).ToList();
                    if (result1.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {


                        var result = _FeeGroupHeadContext.FeeTermWiseRebateSettingDMO.Single(t => t.FMTRS_Id == feepge.FMTRS_Id);
                        result.FMT_Id = FGpage.FMT_Id;
                        result.ASMAY_Id = FGpage.ASMAY_Id;
                        result.FMTRS_RebateAmountPercentFlg = FGpage.FMTRS_RebateAmountPercentFlg;
                        result.FMTRS_RebateApplicableDate = FGpage.FMTRS_RebateApplicableDate;
                        result.FMTRS_RebateAmountPercentValue = FGpage.FMTRS_RebateAmountPercentValue;
 
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
                    // var result = _FeeGroupHeadContext.FeeTermWiseRebateSettingDMO.Where(t => t.FMH_FeeName == feepge.FMH_FeeName && t.FMT_Id== feepge.FMT_Id && t.FMH_Order==feepge.FMH_Order).ToList();
                    var result = _FeeGroupHeadContext.FeeTermWiseRebateSettingDMO.Where(t => t.ASMAY_Id == feepge.ASMAY_Id && t.FMT_Id == feepge.FMT_Id).ToList();

                    if (result.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {
                        feepge.FMT_Id = feepge.FMT_Id;
                        feepge.ASMAY_Id = feepge.ASMAY_Id;
                        feepge.FMTRS_RebateAmountPercentFlg = feepge.FMTRS_RebateAmountPercentFlg;
                        feepge.FMTRS_RebateApplicableDate = feepge.FMTRS_RebateApplicableDate;
                        feepge.FMTRS_RebateAmountPercentValue = feepge.FMTRS_RebateAmountPercentValue;
                   
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

                FGpage.GroupHeadData = (from A in _FeeGroupHeadContext.FeeTermWiseRebateSettingDMO
                                        from B in _FeeGroupHeadContext.AcademicYear
                                        where (A.ASMAY_Id == B.ASMAY_Id  && A.FMT_Id == FGpage.FMT_Id)
                                        select new FeeTermWiseRebateSettingDTO
                                        {
                                            FMTRS_RebateAmountPercentFlg = A.FMTRS_RebateAmountPercentFlg,
                                            FMTRS_RebateApplicableDate = A.FMTRS_RebateApplicableDate,
                                            FMTRS_RebateAmountPercentValue = A.FMTRS_RebateAmountPercentValue,
                                            ASMAY_Year = B.ASMAY_Year,
                                            FMTRS_Id = A.FMTRS_Id,
                                            
                                            ASMAY_Id = A.ASMAY_Id
                                        }
                            ).ToArray();

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return FGpage;
        }


        public FeeTermWiseRebateSettingDTO getdetails(int id)
        {
            FeeTermWiseRebateSettingDTO FGRDT = new FeeTermWiseRebateSettingDTO();

            try
            {
                //List<FeeTermWiseRebateSettingDMO> feegrp = new List<FeeTermWiseRebateSettingDMO>();
                //feegrp = _FeeGroupHeadContext.FeeTermWiseRebateSettingDMO.Where(t => t.FMT_Id == id).OrderBy(t => t.FMTRS_Id).ToList();
                //FGRDT.GroupHeadData = feegrp.ToArray();

                FGRDT.GroupHeadData = (from A in _FeeGroupHeadContext.FeeTermWiseRebateSettingDMO
                                       from B in _FeeGroupHeadContext.AcademicYear
                                       from C in _FeeGroupHeadContext.feeTr
                                       where (A.ASMAY_Id == B.ASMAY_Id && A.FMT_Id == C.FMT_Id && C.MI_Id== id)
                                       select new FeeTermWiseRebateSettingDTO
                                       {
                                           FMTRS_RebateAmountPercentFlg = A.FMTRS_RebateAmountPercentFlg,
                                           FMTRS_RebateApplicableDate = A.FMTRS_RebateApplicableDate,
                                           FMTRS_RebateAmountPercentValue = A.FMTRS_RebateAmountPercentValue,
                                           ASMAY_Year = B.ASMAY_Year,
                                           FMTRS_Id = A.FMTRS_Id,
                                          
                                           ASMAY_Id = A.ASMAY_Id,
                                           FMT_Name=C.FMT_Name
                                       }
                            ).ToArray();






                List<MasterAcademic> allyear = new List<MasterAcademic>();
                // allyear = _FeeGroupHeadContext.AcademicYear.Where(t => t.FMT_Id == FGRDT.FMT_Id && t.ASMAY_Id == FGRDT.ASMAY_Id).ToList();
                allyear = _FeeGroupHeadContext.AcademicYear.Where(t => t.MI_Id == id && t.Is_Active == true).OrderByDescending(o => o.ASMAY_Order).ToList();
                FGRDT.academicdrp = allyear.Distinct().ToArray();

                FGRDT.termlist = _FeeGroupHeadContext.feeTr.Where(t => t.MI_Id == id && t.FMT_ActiveFlag == true).ToArray();


            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public FeeTermWiseRebateSettingDTO EditgroupDetails(int id)
        {
            FeeTermWiseRebateSettingDTO FMG = new FeeTermWiseRebateSettingDTO();
            try
            {
                List<FeeTermWiseRebateSettingDMO> masterfeegroup = new List<FeeTermWiseRebateSettingDMO>();
                masterfeegroup = _FeeGroupHeadContext.FeeTermWiseRebateSettingDMO.Where(t => t.FMTRS_Id.Equals(id)).ToList();
                FMG.GroupHeadData = masterfeegroup.ToArray();
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return FMG;
        }
        public FeeTermWiseRebateSettingDTO GetGroupSearchData(FeeTermWiseRebateSettingDTO mas)
        {

            FeeTermWiseRebateSettingDTO FGRDT = new FeeTermWiseRebateSettingDTO();
            try
            {
                List<FeeTermWiseRebateSettingDMO> feegrp = new List<FeeTermWiseRebateSettingDMO>();
                feegrp = _FeeGroupHeadContext.FeeTermWiseRebateSettingDMO.OrderBy(t => t.FMTRS_Id).ToList();
                FGRDT.GroupHeadData = feegrp.ToArray();

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public FeeTermWiseRebateSettingDTO getpageedit(int id)
        {
            FeeTermWiseRebateSettingDTO page = new FeeTermWiseRebateSettingDTO();
            try
            {
                List<FeeTermWiseRebateSettingDMO> lorg = new List<FeeTermWiseRebateSettingDMO>();
                lorg = _FeeGroupHeadContext.FeeTermWiseRebateSettingDMO.Where(t => t.FMTRS_Id.Equals(id)).ToList();
                page.GroupHeadData = lorg.ToArray();
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return page;
        }


        public FeeTermWiseRebateSettingDTO deactivate(FeeTermWiseRebateSettingDTO acd)
        {
            try
            {
                FeeTermWiseRebateSettingDMO feepge = Mapper.Map<FeeTermWiseRebateSettingDMO>(acd);
                if (feepge.FMTRS_Id > 0)
                {

                    var result = _FeeGroupHeadContext.FeeTermWiseRebateSettingDMO.Single(t => t.FMTRS_Id == feepge.FMTRS_Id);


                   
                    _FeeGroupHeadContext.Remove(result);
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
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException);
            }
            return acd;
        }


    }
}
