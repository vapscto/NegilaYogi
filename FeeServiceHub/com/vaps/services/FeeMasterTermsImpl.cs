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
using DomainModel.Model.com.vapstech.Fee;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeMasterTermsImpl : interfaces.FeeMasterTermsInterface
    {
        private static ConcurrentDictionary<string, FeeTermDTO> _login =
        new ConcurrentDictionary<string, FeeTermDTO>();
        public FeeGroupContext _FeeGroupContext;
        readonly ILogger<FeeMasterTermsImpl> _logger;
        public FeeMasterTermsImpl(FeeGroupContext frgContext, ILogger<FeeMasterTermsImpl> log)
        {
            _FeeGroupContext = frgContext;
            _logger = log;
        }
        public FeeTermDTO SaveGroupData(FeeTermDTO FGpage)
        {
            bool returnresult = false;
            FeeTermDMO feepge = Mapper.Map<FeeTermDMO>(FGpage);
            string retval = "";
            try
            {
                if (feepge.FMT_Id > 0)
                {
                    var res = _FeeGroupContext.feeTr.Where(t => t.FMT_Name == feepge.FMT_Name && t.MI_Id == feepge.MI_Id && t.FMT_IncludeArrearFeeFlg== FGpage.FMT_IncludeArrearFeeFlg).ToList();
                    if (res.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {
                        var result = _FeeGroupContext.feeTr.Single(t => t.FMT_Id == feepge.FMT_Id && t.MI_Id == feepge.MI_Id);
                        result.FMT_Name = feepge.FMT_Name;
                        result.FMT_ActiveFlag = feepge.FMT_ActiveFlag;
                        result.UpdatedDate = DateTime.Now;
                        result.FMT_IncludeArrearFeeFlg = feepge.FMT_IncludeArrearFeeFlg;
                        _FeeGroupContext.Update(result);
                        var contactExists = _FeeGroupContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            retval = "Update";
                            FGpage.returnduplicatestatus = retval;
                            FGpage.status = "Update";
                        }
                        else
                        {
                            retval = "NotUpdate";
                            FGpage.returnduplicatestatus = retval;
                            FGpage.returnval = returnresult;
                        }
                    }
                }
                else
                {
                    var result = _FeeGroupContext.feeTr.Where(t => t.FMT_Name == feepge.FMT_Name && t.MI_Id == feepge.MI_Id && t.FMT_IncludeArrearFeeFlg == FGpage.FMT_IncludeArrearFeeFlg);
                    if (result.Count() > 0)
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
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGpage;
        }
        public FeeTermDTO getdetails(FeeTermDTO FGRDT)
        {
            try
            {
                List<FeeTermDMO> feegrp = new List<FeeTermDMO>();
                feegrp = _FeeGroupContext.feeTr.ToList();
                FGRDT.feetermsarray = feegrp.Where(t => t.MI_Id == FGRDT.MI_Id).ToArray();

                List<FeeTermDMO> feegrpDrop = new List<FeeTermDMO>();
                feegrpDrop = _FeeGroupContext.feeTr.ToList();
                FGRDT.feetermsarrayDrop = feegrpDrop.Where(t => t.MI_Id == FGRDT.MI_Id && t.FMT_ActiveFlag == true).ToArray();

                List<FeeTermDMO> feegrp1 = new List<FeeTermDMO>();
                feegrp = _FeeGroupContext.feeTr.ToList();
                FGRDT.feetermsarray1 = feegrp.Where(t => t.MI_Id == FGRDT.MI_Id && t.FMT_ActiveFlag==true).ToArray();

                List<FeeHeadDMO> feehd = new List<FeeHeadDMO>();
                feehd = _FeeGroupContext.feehead.Where(t => t.MI_Id == FGRDT.MI_Id && t.FMH_ActiveFlag ==true).OrderBy(t=>t.FMH_Order).ToList();
                FGRDT.hdnames = feehd.ToArray();

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == FGRDT.MI_Id && t.Is_Active == true).OrderByDescending(t=>t.ASMAY_Order).ToList();
                FGRDT.academicdrp = allyear.Distinct().ToArray();

                FGRDT.insnames = (from c in _FeeGroupContext.feeMI
                                      from a in _FeeGroupContext.feeMIY
                                      from b in _FeeGroupContext.feeIDDD
                                      where (c.FMI_Id == a.FMI_Id && a.FTI_Id == b.FTI_Id && a.MI_ID == FGRDT.MI_Id && c.FMI_ActiceFlag == true)
                                      select new FeeTermDTO
                                      {
                                          FTI_ID = a.FTI_Id,
                                          FTI_NAME = a.FTI_Name
                                      }
             ).ToArray();

                FGRDT.dataretrive = (from Fee_Master_Terms in _FeeGroupContext.feeTr // masterterm
                                     from Fee_Master_Head in _FeeGroupContext.feehead   // masterhead
                                     from Fee_T_Installment in _FeeGroupContext.feeMIY    // installment
                                     from Fee_Master_Terms_FeeHeads in _FeeGroupContext.feeMTH    // coman  table Fee_Master_Terms_FeeHeads
                                     where (Fee_Master_Terms.FMT_Id == Fee_Master_Terms_FeeHeads.FMT_Id && Fee_Master_Head.FMH_Id == Fee_Master_Terms_FeeHeads.FMH_Id && Fee_T_Installment.FTI_Id == Fee_Master_Terms_FeeHeads.FTI_Id && Fee_Master_Terms.MI_Id == FGRDT.MI_Id && Fee_Master_Terms.FMT_ActiveFlag==true)
                                     select new FeeTermDTO
                                     {
                                         termnaem = Fee_Master_Terms.FMT_Name,
                                         headname = Fee_Master_Head.FMH_FeeName,
                                         installmentname = Fee_T_Installment.FTI_Name,
                                         idforedt = Fee_Master_Terms_FeeHeads.FMTFH_Id,
                                     }
              ).ToArray();

                FGRDT.duadateget = (from Fee_Master_Terms in _FeeGroupContext.feeTr // masterterm
                                    from Adm_School_M_Academic_Year in _FeeGroupContext.AcademicYear   // academic year
                                    from Fee_Master_Terms_FeeHeads_DueDate in _FeeGroupContext.feeTHDDD    // duadates
                                    from Fee_Master_Terms_FeeHeads in _FeeGroupContext.feeMTH   // inner join
                                    from Fee_Master_Head in _FeeGroupContext.feehead   // feehead
                                    where (Fee_Master_Terms.FMT_Id == Fee_Master_Terms_FeeHeads.FMT_Id && Fee_Master_Terms_FeeHeads_DueDate.FMTFH_Id == Fee_Master_Terms_FeeHeads.FMTFH_Id && Adm_School_M_Academic_Year.ASMAY_Id == Fee_Master_Terms_FeeHeads_DueDate.ASMAY_Id && Fee_Master_Head.FMH_Id == Fee_Master_Terms_FeeHeads.FMH_Id && Fee_Master_Terms.MI_Id == FGRDT.MI_Id)
                                    select new FeeTermDTO
                                    {
                                        fmthddid = Fee_Master_Terms_FeeHeads_DueDate.FMTFHDD_Id,
                                        termname = Fee_Master_Terms.FMT_Name,
                                        headnamed = Fee_Master_Head.FMH_FeeName,
                                        yearname = Adm_School_M_Academic_Year.ASMAY_Year,
                                        fdate1 = Fee_Master_Terms_FeeHeads_DueDate.FMTFHDD_FromDate,
                                        tdate1 = Fee_Master_Terms_FeeHeads_DueDate.FMTFHDD_ToDate,
                                        aplc1 = Fee_Master_Terms_FeeHeads_DueDate.FMTFHDD_ApplicableDate,
                                        ddate1 = Fee_Master_Terms_FeeHeads_DueDate.FMTFHDD_DueDate,
                                    }
             ).ToArray();
                FGRDT.masteperiodarray = (from a in _FeeGroupContext.feeTr
                                          from b in _FeeGroupContext.AcademicYear  
                                          from c in _FeeGroupContext.FEE_MASTER_TERMWISE_PERIOD_DMO
                                          where (a.FMT_Id == c.FMT_Id && b.ASMAY_Id == c.ASMAY_ID && c.USER_ID == FGRDT.USER_ID)                                          
                                          select new FeeTermDTO
                                          {
                                              yearname = b.ASMAY_Year,
                                              termname = a.FMT_Name,
                                              FromMonth = c.FMTP_FROM_MONTH,
                                              fmtToMonth = c.FMTP_TO_MONTH,
                                              FMTP_Year = c.FMTP_Year,
                                              FeeFlag = c.FeeFlag,
                                              FMTP_Id = c.FMTP_Id
                                          }
            ).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public FeeTermDTO EditgroupDetails(int id)
        {
            FeeTermDTO FMG = new FeeTermDTO();
            try
            {
                List<FeeTermDMO> masterfeegroup = new List<FeeTermDMO>();
                masterfeegroup = _FeeGroupContext.feeTr.AsNoTracking().Where(t => t.FMT_Id.Equals(id)).ToList();
                FMG.feetermsarray = masterfeegroup.ToArray();



            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FMG;
        }
        public FeeTermDTO GetGroupSearchData(FeeTermDTO mas)
        {

            FeeTermDTO FGRDT = new FeeTermDTO();
            try
            {
                List<FeeTermDMO> feegrp = new List<FeeTermDMO>();
                feegrp = _FeeGroupContext.feeTr.ToList();
                FGRDT.feetermsarray = feegrp.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FGRDT;
        }
        public FeeTermDTO getpageedit(int id)
        {
            FeeTermDTO page = new FeeTermDTO();
            try
            {
                List<FeeTermDMO> lorg = new List<FeeTermDMO>();
                lorg = _FeeGroupContext.feeTr.AsNoTracking().Where(t => t.FMT_Id.Equals(id)).ToList();
                page.feetermsarray = lorg.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public FeeTermDTO deleterec(int id)
        {
            bool returnresult = false;
            FeeTermDTO page = new FeeTermDTO();
            try
            {
                List<FeeMasterTermHeadsDMO> lorgbase = new List<FeeMasterTermHeadsDMO>();
              var   lorgbase2 = _FeeGroupContext.feeMTH.Where(t => t.FMT_Id.Equals(id)).Count();
                if (lorgbase2 > 0)
                {
                    returnresult = false;
                    page.returnval = returnresult;
                    page.returnduplicatestatus = "Already";
                }
                else
                {
                    List<FeeTermDMO> lorg = new List<FeeTermDMO>();
                    lorg = _FeeGroupContext.feeTr.Where(t => t.FMT_Id.Equals(id)).ToList();
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
                List<FeeTermDMO> allpages = new List<FeeTermDMO>();
                allpages = _FeeGroupContext.feeTr.ToList();
                page.feetermsarray = allpages.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public FeeTermDTO deactivate(FeeTermDTO acd)
        {
            try
            {
                FeeTermDMO feepge = Mapper.Map<FeeTermDMO>(acd);
                if (feepge.FMT_Id > 0)
                {

                    var deleteterms = (from a in _FeeGroupContext.feeTr
                                      from b in _FeeGroupContext.feeMTH
                                      from c in _FeeGroupContext.FeeStudentTransactionDMO
                                      where (a.FMT_Id==b.FMT_Id && b.FMH_Id==c.FMH_Id && b.FTI_Id==c.FTI_Id && c.MI_Id== acd.MI_Id && c.ASMAY_Id==acd.ASMAY_ID && a.FMT_Id==feepge.FMT_Id)
                                      select new FeeTermDTO
                                      {
                                          FMT_Id = a.FMT_Id,
                                      }
           ).OrderBy(t => t.FMT_Id).Take(1).Select(t => t.FMT_Id).ToList();

                    if(deleteterms.Count==0)
                    {
                        var result = _FeeGroupContext.feeTr.Single(t => t.FMT_Id == feepge.FMT_Id && t.MI_Id == feepge.MI_Id);
                        if (result.FMT_ActiveFlag == true)
                        {
                            result.FMT_ActiveFlag = false;
                        }
                        else
                        {
                            result.FMT_ActiveFlag = true;
                        }
                        result.UpdatedDate = DateTime.Now;
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
                _logger.LogError(e.Message);
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }
        public FeeMasterTermHeadsDTO SaveYearlyGroupData(FeeMasterTermHeadsDTO FGpage)
        {
            bool returnresult = false;

            string retval = "";
            try
            {
                for (int i = 0; i < FGpage.TempararyArrayListhd.Length; i++)
                {
                    for (int j = 0; j < FGpage.TempararyArrayList.Length; j++)
                    {
                        if (FGpage.FMTFH_Id>0)
                        {
                            var exist1 = _FeeGroupContext.feeMTH.Where(t => t.FMTFH_Id == FGpage.FMTFH_Id && t.MI_Id == FGpage.MI_Id).ToList();
                            if (exist1.Count>0)
                            {
                                foreach (var item1 in exist1)
                                {
                                    _FeeGroupContext.Remove(item1);
                                }
                                var contactExists1 = _FeeGroupContext.SaveChanges();
                            }
                        }

                        var exist = _FeeGroupContext.feeMTH.Where(t => t.MI_Id == FGpage.MI_Id && t.FMT_Id == FGpage.FMT_Id && t.FMH_Id == FGpage.TempararyArrayListhd[i].FMH_Id && t.FTI_Id == FGpage.TempararyArrayList[j].FTI_Id).ToList();

                        if (exist.Count > 0)
                        {
                            foreach (var item in exist)
                            {
                                _FeeGroupContext.Remove(item);
                            }
                            var contactExists1 = _FeeGroupContext.SaveChanges();
                        }
                            var result = _FeeGroupContext.feeMTH.Where(t => t.FTI_Id == FGpage.TempararyArrayList[j].FTI_Id && t.FMT_Id == FGpage.FMT_Id && t.FMH_Id == FGpage.TempararyArrayListhd[i].FMH_Id && t.MI_Id == FGpage.MI_Id);
                            if (result.Count() > 0)
                            {
                                retval = "Update";
                                FGpage.returnduplicatestatus = retval;
                            }
                            else
                            {
                            FeeMasterTermHeadsDMO newfeepge = new FeeMasterTermHeadsDMO();
                            newfeepge.CreatedDate = DateTime.Now;
                            newfeepge.UpdatedDate = DateTime.Now;
                            newfeepge.FTI_Id = FGpage.TempararyArrayList[j].FTI_Id;
                            newfeepge.FMH_Id = FGpage.TempararyArrayListhd[i].FMH_Id;
                            newfeepge.MI_Id = FGpage.MI_Id;
                            newfeepge.FMT_Id = FGpage.FMT_Id;
                                _FeeGroupContext.Add(newfeepge);
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
                }
                FGpage.dataretrive = (from Fee_Master_Terms in _FeeGroupContext.feeTr // masterterm
                                      from Fee_Master_Head in _FeeGroupContext.feehead   // masterhead
                                      from Fee_T_Installment in _FeeGroupContext.feeMIY    // installment
                                      from Fee_Master_Terms_FeeHeads in _FeeGroupContext.feeMTH    // coman  table Fee_Master_Terms_FeeHeads
                                      where (Fee_Master_Terms.FMT_Id == Fee_Master_Terms_FeeHeads.FMT_Id && Fee_Master_Head.FMH_Id == Fee_Master_Terms_FeeHeads.FMH_Id && Fee_T_Installment.FTI_Id == Fee_Master_Terms_FeeHeads.FTI_Id && Fee_T_Installment.MI_ID== FGpage.MI_Id)
                                      select new FeeTermDTO
                                      {
                                          termnaem = Fee_Master_Terms.FMT_Name,
                                          headname = Fee_Master_Head.FMH_FeeName,
                                          installmentname = Fee_T_Installment.FTI_Name,
                                          idforedt = Fee_Master_Terms_FeeHeads.FMTFH_Id,
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
        public FeeMasterTermHeadsDTO getdetailsY(int id)
        {
            FeeMasterTermHeadsDTO FGRDT = new FeeMasterTermHeadsDTO();
            try
            {
                List<FeeMasterTermHeadsDMO> feegrp = new List<FeeMasterTermHeadsDMO>();
                feegrp = _FeeGroupContext.feeMTH.ToList();
                FGRDT.dataretrive = feegrp.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public FeeMasterTermHeadsDTO deactivateY(FeeMasterTermHeadsDTO acd)
        {
            try
            {
                FeeMasterTermHeadsDMO feepge = Mapper.Map<FeeMasterTermHeadsDMO>(acd);
                if (feepge.FMTFH_Id > 0)
                {
                    var result = _FeeGroupContext.feeMTH.Single(t => t.FMTFH_Id == feepge.FMTFH_Id);
                    result.UpdatedDate = DateTime.Now;
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
                    List<FeeMasterTermHeadsDMO> allorganisation = new List<FeeMasterTermHeadsDMO>();
                    allorganisation = _FeeGroupContext.feeMTH.ToList();
                    acd.dataretrive = allorganisation.ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }

        public FeeMasterTermHeadsDTO getpageeditY(int id)
        {
            FeeMasterTermHeadsDTO page = new FeeMasterTermHeadsDTO();
            try
            {
                List<FeeMasterTermHeadsDMO> lorg = new List<FeeMasterTermHeadsDMO>();
                lorg = _FeeGroupContext.feeMTH.AsNoTracking().Where(t => t.FMTFH_Id.Equals(id)).ToList();
                page.dataretrive = lorg.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public FeeMasterTermHeadsDTO deleterecY(FeeMasterTermHeadsDTO acd)
        {
            bool returnresult = false;
           
            try
            {
                var getdetails = _FeeGroupContext.FeeMasterTermHeadsDMO.Where(x => x.FMTFH_Id == acd.FMTFH_Id && x.MI_Id == acd.MI_Id).ToList();
                    

                var deleteitem = _FeeGroupContext.FeeStudentTransactionDMO.Where(t => t.MI_Id == acd.MI_Id && t.FMH_Id == getdetails[0].FMH_Id && t.FTI_Id == getdetails[0].FTI_Id && t.FSS_ActiveFlag == true).ToList();
                if (deleteitem.Count>0)
                {
                    returnresult = false;
                    acd.returnval = returnresult;
                    acd.returnduplicatestatus = "Already";
                }
                else
                {
                    List<FeeMasterTermHeadsDMO> lorg = new List<FeeMasterTermHeadsDMO>();
                    lorg = _FeeGroupContext.feeMTH.Where(t => t.FMTFH_Id.Equals(acd.FMTFH_Id)).ToList();
                    if (lorg.Any())
                    {
                        _FeeGroupContext.Remove(lorg.ElementAt(0));

                        var contactExists = _FeeGroupContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnresult = true;
                            acd.returnval = returnresult;
                        }
                        else
                        {
                            returnresult = false;
                            acd.returnval = returnresult;
                        }
                    }
                }

                List<FeeMasterTermHeadsDMO> allpages = new List<FeeMasterTermHeadsDMO>();
                allpages = _FeeGroupContext.feeMTH.ToList();
                acd.dataretrive = allpages.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return acd;
        }
        public FeeMasterTermHeadsDTO[] Getduedates(FeeMasterTermHeadsDTO mas)
        {

            List<FeeMasterTermHeadsDMO> HnT = new List<FeeMasterTermHeadsDMO>();
            HnT = _FeeGroupContext.feeMTH.Where(t => t.FMH_Id.Equals(mas.FMH_Id) && t.FMT_Id.Equals(mas.FMT_Id) && t.MI_Id == mas.MI_Id).ToList();
            List<FeeMasterTermHeadsDTO> AllInOne = new List<FeeMasterTermHeadsDTO>();
            for (int i = 0; i < HnT.Count; i++)
            {
                int ftiidget = Convert.ToInt32(HnT[i].FTI_Id);
                List<FeeInstallmentsyearlyDMO> BindInsName = new List<FeeInstallmentsyearlyDMO>();
                BindInsName = _FeeGroupContext.feeMIY.Where(t => t.FTI_Id.Equals(ftiidget) && t.MI_ID == mas.MI_Id).ToList();
                FeeMasterTermHeadsDTO temp = new FeeMasterTermHeadsDTO();
                temp.FTI_Id = BindInsName[0].FTI_Id;
                temp.insname = BindInsName[0].FTI_Name;
                temp.fmtfhdD_FromDate = DateTime.Now;
                temp.fmtfhdD_ToDate = DateTime.Now;
                temp.fmtfhdD_ApplicableDate = DateTime.Now;
                temp.fmtfhdD_DueDate = DateTime.Now;
                temp.FMTFH_Id = Convert.ToInt64(HnT[i].FMTFH_Id);
                AllInOne.Add(temp);
            }
            return AllInOne.ToArray();
        }
        public FeeMasterTermHeadsDTO savedetailDDD(FeeMasterTermHeadsDTO FGpage)
        {
            FeeMasterTermFeeHeadsDueDateDTO te = new FeeMasterTermFeeHeadsDueDateDTO();
            MasterTermFeeHeadsDueDateDMO feepgeY = Mapper.Map<MasterTermFeeHeadsDueDateDMO>(te);

            bool retvalue = false;
            for (int i = 0; i < FGpage.feetfhddd.Count; i++)
            {
                if (FGpage.mainid > 0)
                {
                    var result = _FeeGroupContext.feeTHDDD.Where(t => t.MI_Id == FGpage.MI_Id && t.ASMAY_Id == FGpage.temyrid && t.FMTFH_Id == FGpage.feetfhddd[i].FMTFH_Id);
                    if (result.Count() > 0)
                    {
                        FGpage.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        retvalue = savedetailDDDinDB(FGpage.mainid, FGpage.MI_Id, FGpage.feetfhddd[i].FMTFH_Id, FGpage.temyrid, FGpage.feetfhddd[i].FMTFHDD_FromDate, FGpage.feetfhddd[i].FMTFHDD_ToDate, FGpage.feetfhddd[i].FMTFHDD_ApplicableDate, FGpage.feetfhddd[i].FMTFHDD_DueDate);
                    }
                }
                else
                {
                    var result = _FeeGroupContext.feeTHDDD.Where(t => t.MI_Id == FGpage.MI_Id && t.ASMAY_Id == FGpage.temyrid && t.FMTFH_Id == FGpage.feetfhddd[i].FMTFH_Id);
                    if (result.Count() > 0)
                    {
                        FGpage.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        retvalue = savedetailDDDinDB(FGpage.mainid, FGpage.MI_Id, FGpage.feetfhddd[i].FMTFH_Id, FGpage.temyrid, FGpage.feetfhddd[i].FMTFHDD_FromDate, FGpage.feetfhddd[i].FMTFHDD_ToDate, FGpage.feetfhddd[i].FMTFHDD_ApplicableDate, FGpage.feetfhddd[i].FMTFHDD_DueDate);
                    }
                }
            }
            FGpage.returnvalue = retvalue;
            return FGpage;
        }
        public bool savedetailDDDinDB(long mainID, long mid, long FMTFHId, long yrid, DateTime fromdate, DateTime todate, DateTime apldate, DateTime ddudate)
        {
            bool returnvalue = false;
            FeeMasterTermFeeHeadsDueDateDTO te = new FeeMasterTermFeeHeadsDueDateDTO();
            MasterTermFeeHeadsDueDateDMO feepgeY = Mapper.Map<MasterTermFeeHeadsDueDateDMO>(te);
            try
            {
                if (mainID > 0)
                {
                    var result = _FeeGroupContext.feeTHDDD.Single(t => t.FMTFHDD_Id == mainID);
                    result.MI_Id = mid;
                    result.FMTFH_Id =result.FMTFH_Id;
                    result.ASMAY_Id = yrid;
                    result.FMTFHDD_FromDate = fromdate;
                    result.FMTFHDD_ToDate = todate;
                    result.FMTFHDD_ApplicableDate = apldate;
                    result.FMTFHDD_DueDate = ddudate;
                    result.UpdatedDate = DateTime.Now;
                    _FeeGroupContext.Update(result);
                    var contactExists = _FeeGroupContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        returnvalue = true;
                    }
                    else
                    {
                        returnvalue = false;
                    }
                }
                else
                {

                    feepgeY.MI_Id = mid;
                    feepgeY.FMTFH_Id = FMTFHId;
                    feepgeY.ASMAY_Id = yrid;
                    feepgeY.FMTFHDD_FromDate = fromdate;
                    feepgeY.FMTFHDD_ToDate = todate;
                    feepgeY.FMTFHDD_ApplicableDate = apldate;
                    feepgeY.FMTFHDD_DueDate = ddudate;
                    feepgeY.CreatedDate = DateTime.Now;
                    feepgeY.UpdatedDate = DateTime.Now;
                    _FeeGroupContext.Add(feepgeY);
                    var contactExists = _FeeGroupContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        returnvalue = true;
                    }
                    else
                    {
                        returnvalue = false;
                    }
                }
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return returnvalue;
        }
        public FeeMasterTermFeeHeadsDueDateDTO getdetailsDY(int id)
        {
            FeeMasterTermFeeHeadsDueDateDTO page = new FeeMasterTermFeeHeadsDueDateDTO();
            try
            {
                page.arrduadates = (from Fee_Master_Terms in _FeeGroupContext.feeTr // masterterm
                                    from Adm_School_M_Academic_Year in _FeeGroupContext.AcademicYear   // academic year
                                    from Fee_Master_Terms_FeeHeads_DueDate in _FeeGroupContext.feeTHDDD    // duadates
                                    from Fee_Master_Terms_FeeHeads in _FeeGroupContext.feeMTH   // inner join
                                    from Fee_Master_Head in _FeeGroupContext.feehead   // feehead
                                    where (Fee_Master_Terms.FMT_Id == Fee_Master_Terms_FeeHeads.FMT_Id && Fee_Master_Terms_FeeHeads_DueDate.FMTFH_Id == Fee_Master_Terms_FeeHeads.FMTFH_Id && Adm_School_M_Academic_Year.ASMAY_Id == Fee_Master_Terms_FeeHeads_DueDate.ASMAY_Id && Fee_Master_Head.FMH_Id == Fee_Master_Terms_FeeHeads.FMH_Id && Fee_Master_Terms_FeeHeads_DueDate.FMTFHDD_Id == id)
                                    select new FeeTermDTO
                                    {
                                        fmthddid = Convert.ToInt64(id),
                                        termname = Fee_Master_Terms.FMT_Name,
                                        headnamed = Fee_Master_Head.FMH_FeeName,
                                        yearname = Adm_School_M_Academic_Year.ASMAY_Year,
                                        fdate1 = Fee_Master_Terms_FeeHeads_DueDate.FMTFHDD_FromDate,
                                        tdate1 = Fee_Master_Terms_FeeHeads_DueDate.FMTFHDD_ToDate,
                                        aplc1 = Fee_Master_Terms_FeeHeads_DueDate.FMTFHDD_ApplicableDate,
                                        ddate1 = Fee_Master_Terms_FeeHeads_DueDate.FMTFHDD_DueDate,
                                        fmt = Fee_Master_Terms.FMT_Id,
                                        fmh = Fee_Master_Head.FMH_Id,
                                        asyid = Adm_School_M_Academic_Year.ASMAY_Id,
                                        fti = Fee_Master_Terms_FeeHeads.FTI_Id,
                                        FMTFHNew = Fee_Master_Terms_FeeHeads_DueDate.FMTFH_Id

                                    }
            ).ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public FeeMasterTermFeeHeadsDueDateDTO deletepagesthird(int id)
        {
            bool returnresult = false;
            FeeMasterTermFeeHeadsDueDateDTO page = new FeeMasterTermFeeHeadsDueDateDTO();
            List<MasterTermFeeHeadsDueDateDMO> lorg = new List<MasterTermFeeHeadsDueDateDMO>();
            lorg = _FeeGroupContext.feeTHDDD.Where(t => t.FMTFHDD_Id.Equals(id)).ToList();

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
                page.duadateget = (from Fee_Master_Terms in _FeeGroupContext.feeTr // masterterm
                                   from Adm_School_M_Academic_Year in _FeeGroupContext.AcademicYear   // academic year
                                   from Fee_Master_Terms_FeeHeads_DueDate in _FeeGroupContext.feeTHDDD    // duadates
                                   from Fee_Master_Terms_FeeHeads in _FeeGroupContext.feeMTH   // inner join
                                   from Fee_Master_Head in _FeeGroupContext.feehead   // feehead
                                   where (Fee_Master_Terms.FMT_Id == Fee_Master_Terms_FeeHeads.FMT_Id && Fee_Master_Terms_FeeHeads_DueDate.FMTFH_Id == Fee_Master_Terms_FeeHeads.FMTFH_Id && Adm_School_M_Academic_Year.ASMAY_Id == Fee_Master_Terms_FeeHeads_DueDate.ASMAY_Id && Fee_Master_Head.FMH_Id == Fee_Master_Terms_FeeHeads.FMH_Id)
                                   select new FeeTermDTO
                                   {
                                       fmthddid = Fee_Master_Terms_FeeHeads_DueDate.FMTFHDD_Id,
                                       termname = Fee_Master_Terms.FMT_Name,
                                       headnamed = Fee_Master_Head.FMH_FeeName,
                                       yearname = Adm_School_M_Academic_Year.ASMAY_Year,
                                       fdate1 = Fee_Master_Terms_FeeHeads_DueDate.FMTFHDD_FromDate,
                                       tdate1 = Fee_Master_Terms_FeeHeads_DueDate.FMTFHDD_ToDate,
                                       aplc1 = Fee_Master_Terms_FeeHeads_DueDate.FMTFHDD_ApplicableDate,
                                       ddate1 = Fee_Master_Terms_FeeHeads_DueDate.FMTFHDD_DueDate,
                                   }
             ).ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        //extra added
        public FeeMasterTermHeadsDTO DeleteYss(FeeMasterTermHeadsDTO acd)
        {
            string retval = "";
            try
            {
                List<FEE_MASTER_TERMWISE_PERIOD_DMO> lorg = new List<FEE_MASTER_TERMWISE_PERIOD_DMO>();
                lorg = _FeeGroupContext.FEE_MASTER_TERMWISE_PERIOD_DMO.Where(t => t.FMTP_Id.Equals(acd.FMTP_Id)).ToList();
                if (lorg.Any())
                {
                    _FeeGroupContext.Remove(lorg.ElementAt(0));

                    var contactExists = _FeeGroupContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        retval = "Remove";
                        acd.returnduplicatestatus = retval;
                    }
                    else
                    {
                        retval = "NotRemove";
                        acd.returnduplicatestatus = retval;
                    }
                }
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return acd;
        }
        public FeeMasterTermFeeHeadsDueDateDTO getdetailsDYfourth(int id)
        {
            FeeMasterTermFeeHeadsDueDateDTO page = new FeeMasterTermFeeHeadsDueDateDTO();
            try
            {

                page.masterdit = (from a in _FeeGroupContext.feeTr // 
                                  from b in _FeeGroupContext.AcademicYear   // academic year
                                  from c in _FeeGroupContext.FEE_MASTER_TERMWISE_PERIOD_DMO
                                  where (a.FMT_Id == c.FMT_Id && b.ASMAY_Id == c.ASMAY_ID && c.FMTP_Id == id)


                                  select new FeeTermDTO
                                  {

                                      yearname = b.ASMAY_Year,
                                      termname = a.FMT_Name,
                                      ASMAY_ID = c.ASMAY_ID,
                                      FromMonth = c.FMTP_FROM_MONTH,
                                      fmtToMonth = c.FMTP_TO_MONTH,
                                      FMTP_Year = c.FMTP_Year,
                                      FeeFlag = c.FeeFlag,
                                      FMT_Id = c.FMT_Id,
                                      FMTP_Id = c.FMTP_Id

                                  }
            ).ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public FeeMasterTermHeadsDTO savedetailfourth(FeeMasterTermHeadsDTO data)
        {

            try
            {
                string retval = "";
                FEE_MASTER_TERMWISE_PERIOD_DMO obj = Mapper.Map<FEE_MASTER_TERMWISE_PERIOD_DMO>(data);
                if (obj.FMTP_Id > 0)
                {
                    var result = _FeeGroupContext.FEE_MASTER_TERMWISE_PERIOD_DMO.Single(t => t.FMTP_Id == obj.FMTP_Id);
                    result.FMT_Id = data.FMT_Id;
                    result.FMTP_Year = data.FMTP_Year;
                    result.FMTP_FROM_MONTH = data.FMTP_FROM_MONTH;
                    result.FMTP_TO_MONTH = data.FMTP_TO_MONTH;
                    result.USER_ID = data.USER_ID;
                    result.ASMAY_ID = data.ASMAY_Id;
                    result.FeeFlag = data.FeeFlag;
                    result.FMTP_Id = data.FMTP_Id;
                    _FeeGroupContext.Update(result);
                    var contactExists = _FeeGroupContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        retval = "Update";
                        data.returnduplicatestatus = retval;
                    }
                    else
                    {
                        retval = "NotUpdate";
                        data.returnduplicatestatus = retval;

                    }
                }
                else if (obj.FMTP_Id == 0)
                {
                    var result = _FeeGroupContext.FEE_MASTER_TERMWISE_PERIOD_DMO.Where(R => R.FMT_Id == obj.FMT_Id && R.FMTP_Year == obj.FMTP_Year && R.FMTP_FROM_MONTH==obj.FMTP_FROM_MONTH && R.FMTP_TO_MONTH==obj.FMTP_TO_MONTH && R.ASMAY_ID==obj.ASMAY_ID && R.FeeFlag==obj.FeeFlag).ToList();
                    if(result.Count > 0)
                    {
                        data.returnduplicatestatus = "RecordExist";
                    }
                    else
                    {
                        obj.FMT_Id = data.FMT_Id;
                        obj.FMTP_Year = data.FMTP_Year;
                        obj.FMTP_FROM_MONTH = data.FMTP_FROM_MONTH;
                        obj.FMTP_TO_MONTH = data.FMTP_TO_MONTH;
                        obj.USER_ID = data.USER_ID;
                        obj.ASMAY_ID = data.ASMAY_Id;
                        obj.FeeFlag = data.FeeFlag;
                        obj.FMTP_Id = data.FMTP_Id;
                        _FeeGroupContext.Add(obj);
                        var contactExists = _FeeGroupContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            retval = "Save";
                            data.returnduplicatestatus = retval;
                        }
                        else
                        {
                            retval = "NotSave";
                            data.returnduplicatestatus = retval;
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

    }
}



