using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.Fee;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeYearlyRebateSettingImpl:interfaces.FeeYearlyRebateSettingInterface
    {
        private static ConcurrentDictionary<string, FeeYearlyRedateSettingDTO> _login =
       new ConcurrentDictionary<string, FeeYearlyRedateSettingDTO>();

        public FeeGroupContext _FeeGroupHeadContext;
       
        public FeeYearlyRebateSettingImpl(FeeGroupContext frgContext)
        {
           
            _FeeGroupHeadContext = frgContext;

        }
       
        public FeeYearlyRedateSettingDTO SaveGroupData(FeeYearlyRedateSettingDTO FGpage)
        {
            bool returnresult = false;
            FeeYearlyRebateSettingDMO feepge = Mapper.Map<FeeYearlyRebateSettingDMO>(FGpage);
            string retval = "";
            try
            {
                if (feepge.FYREBSET_Id > 0)
                {

                    //var result1 = _FeeGroupHeadContext.FeeYearlyRebateSettingDMO.Where(t => t.FMH_FeeName == feepge.FMH_FeeName && t.MI_Id == feepge.MI_Id && t.FMH_Order == feepge.FMH_Order).ToList();
                    var result1 = _FeeGroupHeadContext.FeeYearlyRebateSettingDMO.Where(t => t.FYREBSET_Id != feepge.FYREBSET_Id && t.FYREBSET_RebateTypeFlg == feepge.FYREBSET_RebateTypeFlg && t.MI_Id == feepge.MI_Id && t.ASMAY_Id== feepge.ASMAY_Id).ToList();
                    if (result1.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {
                         

                        var result = _FeeGroupHeadContext.FeeYearlyRebateSettingDMO.Single(t => t.FYREBSET_Id == feepge.FYREBSET_Id);
                        result.MI_Id = FGpage.MI_Id;
                        result.ASMAY_Id = FGpage.ASMAY_Id;
                        result.FYREBSET_RebateTypeFlg = FGpage.FYREBSET_RebateTypeFlg;
                        result.FYREBSET_RebateDate = FGpage.FYREBSET_RebateDate;
                        result.FYREBSET_RebateAmtOrPercentValue = FGpage.FYREBSET_RebateAmtOrPercentValue;
                        result.FYREBSET_ActiveId = true;
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
                    // var result = _FeeGroupHeadContext.FeeYearlyRebateSettingDMO.Where(t => t.FMH_FeeName == feepge.FMH_FeeName && t.MI_Id== feepge.MI_Id && t.FMH_Order==feepge.FMH_Order).ToList();
                    var result = _FeeGroupHeadContext.FeeYearlyRebateSettingDMO.Where(t => t.ASMAY_Id == feepge.ASMAY_Id).ToList();

                    if (result.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {
                        feepge.MI_Id = feepge.MI_Id;
                        feepge.ASMAY_Id = feepge.ASMAY_Id;
                        feepge.FYREBSET_RebateTypeFlg = feepge.FYREBSET_RebateTypeFlg;
                        feepge.FYREBSET_RebateDate = feepge.FYREBSET_RebateDate;
                        feepge.FYREBSET_RebateAmtOrPercentValue = feepge.FYREBSET_RebateAmtOrPercentValue;
                        feepge.FYREBSET_ActiveId = true;
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

                FGpage.GroupHeadData = (from A in _FeeGroupHeadContext.FeeYearlyRebateSettingDMO
                                       from B in _FeeGroupHeadContext.AcademicYear
                                       where (A.ASMAY_Id == B.ASMAY_Id && A.MI_Id == B.MI_Id && A.MI_Id == FGpage.MI_Id)
                                       select new FeeYearlyRedateSettingDTO
                                       {
                                           FYREBSET_RebateTypeFlg = A.FYREBSET_RebateTypeFlg,
                                           FYREBSET_RebateDate = A.FYREBSET_RebateDate,
                                           FYREBSET_RebateAmtOrPercentValue = A.FYREBSET_RebateAmtOrPercentValue,
                                           ASMAY_Year = B.ASMAY_Year,
                                           FYREBSET_Id = A.FYREBSET_Id,
                                           FYREBSET_ActiveId = A.FYREBSET_ActiveId,
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

    
        public FeeYearlyRedateSettingDTO getdetails(int id)
        {
            FeeYearlyRedateSettingDTO FGRDT = new FeeYearlyRedateSettingDTO();

            try
            {
                //List<FeeYearlyRebateSettingDMO> feegrp = new List<FeeYearlyRebateSettingDMO>();
                //feegrp = _FeeGroupHeadContext.FeeYearlyRebateSettingDMO.Where(t => t.MI_Id == id).OrderBy(t => t.FYREBSET_Id).ToList();
                //FGRDT.GroupHeadData = feegrp.ToArray();

                FGRDT.GroupHeadData = (from A in _FeeGroupHeadContext.FeeYearlyRebateSettingDMO 
                                       from B in _FeeGroupHeadContext.AcademicYear
                                       where(A.ASMAY_Id==B.ASMAY_Id && A.MI_Id==B.MI_Id && A.MI_Id==id)
                                       select new FeeYearlyRedateSettingDTO
                                       {
                                           FYREBSET_RebateTypeFlg =A.FYREBSET_RebateTypeFlg,
                                           FYREBSET_RebateDate = A.FYREBSET_RebateDate,
                                           FYREBSET_RebateAmtOrPercentValue =A.FYREBSET_RebateAmtOrPercentValue,
                                           ASMAY_Year = B.ASMAY_Year,
                                           FYREBSET_Id=A.FYREBSET_Id,
                                           FYREBSET_ActiveId=A.FYREBSET_ActiveId,
                                           ASMAY_Id=A.ASMAY_Id
                                       }
                            ).ToArray();






                List < MasterAcademic > allyear = new List<MasterAcademic>();
                // allyear = _FeeGroupHeadContext.AcademicYear.Where(t => t.MI_Id == FGRDT.MI_Id && t.ASMAY_Id == FGRDT.ASMAY_Id).ToList();
                allyear = _FeeGroupHeadContext.AcademicYear.Where(t => t.MI_Id == id && t.Is_Active == true).OrderByDescending(o => o.ASMAY_Order).ToList();
                FGRDT.academicdrp = allyear.Distinct().ToArray();

            }
            catch (Exception ee)
            {
               
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public FeeYearlyRedateSettingDTO EditgroupDetails(int id)
        {
            FeeYearlyRedateSettingDTO FMG = new FeeYearlyRedateSettingDTO();
            try
            {
                List<FeeYearlyRebateSettingDMO> masterfeegroup = new List<FeeYearlyRebateSettingDMO>();
                masterfeegroup = _FeeGroupHeadContext.FeeYearlyRebateSettingDMO.Where(t => t.FYREBSET_Id.Equals(id)).ToList();
                FMG.GroupHeadData = masterfeegroup.ToArray();
            }
            catch (Exception ee)
            {
               
                Console.WriteLine(ee.Message);
            }
            return FMG;
        }
        public FeeYearlyRedateSettingDTO GetGroupSearchData(FeeYearlyRedateSettingDTO mas)
        {

            FeeYearlyRedateSettingDTO FGRDT = new FeeYearlyRedateSettingDTO();
            try
            {
                List<FeeYearlyRebateSettingDMO> feegrp = new List<FeeYearlyRebateSettingDMO>();
                feegrp = _FeeGroupHeadContext.FeeYearlyRebateSettingDMO.OrderBy(t => t.FYREBSET_Id).ToList();
                FGRDT.GroupHeadData = feegrp.ToArray();

            }
            catch (Exception ee)
            {
               
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public FeeYearlyRedateSettingDTO getpageedit(int id)
        {
            FeeYearlyRedateSettingDTO page = new FeeYearlyRedateSettingDTO();
            try
            {
                List<FeeYearlyRebateSettingDMO> lorg = new List<FeeYearlyRebateSettingDMO>();
                lorg = _FeeGroupHeadContext.FeeYearlyRebateSettingDMO.Where(t => t.FYREBSET_Id.Equals(id)).ToList();
                page.GroupHeadData = lorg.ToArray();
            }
            catch (Exception ee)
            {
               
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public FeeYearlyRedateSettingDTO deleterec(int id)
        {
            bool returnresult = false;
            bool dupl = false;
            FeeYearlyRedateSettingDTO page = new FeeYearlyRedateSettingDTO();
            List<FeeYearlygroupHeadMappingDMO> lorgrecords = new List<FeeYearlygroupHeadMappingDMO>();
           

                List<FeeYearlyRebateSettingDMO> lorg = new List<FeeYearlyRebateSettingDMO>();
                lorg = _FeeGroupHeadContext.FeeYearlyRebateSettingDMO.Where(t => t.FYREBSET_Id.Equals(id)).ToList();

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

                    List<FeeYearlyRebateSettingDMO> allpages = new List<FeeYearlyRebateSettingDMO>();
                    allpages = _FeeGroupHeadContext.FeeYearlyRebateSettingDMO.OrderBy(t => t.FYREBSET_Id).ToList();
                    page.GroupHeadData = allpages.ToArray();
                }
                catch (Exception ee)
                {
                 
                    Console.WriteLine(ee.Message);
                }
          
            return page;
        }
        public FeeYearlyRedateSettingDTO deactivate(FeeYearlyRedateSettingDTO acd)
        {
            try
            {
                FeeYearlyRebateSettingDMO feepge = Mapper.Map<FeeYearlyRebateSettingDMO>(acd);
                if (feepge.FYREBSET_Id > 0)
                {

                    var result = _FeeGroupHeadContext.FeeYearlyRebateSettingDMO.Single(t => t.FYREBSET_Id == feepge.FYREBSET_Id);
                   
                      
                        if (result.FYREBSET_ActiveId == true)
                        {
                            result.FYREBSET_ActiveId = false;
                        }
                        else
                        {
                            result.FYREBSET_ActiveId = true;
                        }
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
                  

                    List<FeeYearlyRebateSettingDMO> allorganisation = new List<FeeYearlyRebateSettingDMO>();
                    allorganisation = _FeeGroupHeadContext.FeeYearlyRebateSettingDMO.OrderBy(t => t.FYREBSET_Id).ToList();
                    acd.GroupHeadData = allorganisation.ToArray();
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
