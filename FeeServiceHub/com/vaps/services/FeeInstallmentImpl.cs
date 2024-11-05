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
    public class FeeInstallmentImpl : interfaces.FeeInstallmentInterface
    {

        private static ConcurrentDictionary<string, FeeInstallmentDTO> _login =
         new ConcurrentDictionary<string, FeeInstallmentDTO>();

        public FeeGroupContext _FeeGroupContext;
        readonly ILogger<FeeInstallmentImpl> _logger;
        public FeeInstallmentImpl(FeeGroupContext frgContext, ILogger<FeeInstallmentImpl> log)
        {
            _logger = log;
            _FeeGroupContext = frgContext;
        }
        //public FeeInstallmentDTO SaveGroupData(FeeInstallmentDTO FGpage)
        //{
        //    bool returnresult = false;
        //    FeeInstallmentDMO feepge = Mapper.Map<FeeInstallmentDMO>(FGpage);
        //    string retval = "";
        //    try
        //    {
        //        if (feepge.FMI_Id > 0)
        //        {
        //            var result = _FeeGroupContext.feeMI.Single(t => t.FMI_Id == feepge.FMI_Id);
        //            result.MI_Id = feepge.MI_Id;
        //            result.FMI_Name = feepge.FMI_Name;
        //            result.FMI_No_Of_Installments = feepge.FMI_No_Of_Installments;
        //            result.FMI_Installment_Type = feepge.FMI_Installment_Type;
        //            result.FMI_ActiceFlag = feepge.FMI_ActiceFlag;
        //            result.UpdatedDate = DateTime.Now;
        //            _FeeGroupContext.Update(result);
        //            var contactExists = _FeeGroupContext.SaveChanges();
        //            if (contactExists == 1)
        //            {
        //                returnresult = true;
        //                FGpage.returnval = returnresult;
        //                saveyeardatanew(feepge.FMI_Id, 1, FGpage);
        //            }
        //            else
        //            {
        //                returnresult = false;
        //                FGpage.returnval = returnresult;
        //            }

        //        }
        //        else
        //        {
        //            var result = _FeeGroupContext.feeMI.Where(t => t.FMI_Id == feepge.FMI_Id || t.FMI_Name == feepge.FMI_Name && t.MI_Id== feepge.MI_Id);
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
        //                    for (int i = 0; i < FGpage.fydto.Count; i++)
        //                    {
        //                        saveyeardata(feepge.FMI_Id, FGpage.fydto[i].FTI_Name, FGpage.MI_Id);
        //                    }
        //                }
        //                else
        //                {
        //                    returnresult = false;
        //                    FGpage.returnval = returnresult;
        //                }                       
        //            }
        //        }
        //        List<FeeInstallmentDMO> allpages = new List<FeeInstallmentDMO>();
        //        allpages = _FeeGroupContext.feeMI.OrderByDescending(t => t.CreatedDate).ToList();
        //        FGpage.InstallmentData = allpages.ToArray();

        //    }
        //    catch (Exception ee)
        //    {
        //        _logger.LogError(ee.Message);
        //        Console.WriteLine(ee.Message);
        //    }
        //    return FGpage;
        //}
        //public FeeInstallmentDTO SaveGroupData(FeeInstallmentDTO FGpage)
        //{
        //    bool returnresult = false;
        //    FeeInstallmentDMO feepge = Mapper.Map<FeeInstallmentDMO>(FGpage);
        //    string retval = "";
        //    try
        //    {
        //        if (feepge.FMI_Id > 0)
        //        {
        //            var res = _FeeGroupContext.feeMI.Where(t => t.FMI_Id != feepge.FMI_Id && t.FMI_Name == feepge.FMI_Name && t.MI_Id == feepge.MI_Id);
        //            if (res.Count() > 0)
        //            {
        //                retval = "Duplicate";
        //                FGpage.returnduplicatestatus = retval;
        //            }
        //            else
        //            {
        //                var result = _FeeGroupContext.feeMI.Single(t => t.FMI_Id == feepge.FMI_Id);
        //                result.MI_Id = feepge.MI_Id;
        //                result.FMI_Name = feepge.FMI_Name;
        //                result.FMI_No_Of_Installments = feepge.FMI_No_Of_Installments;
        //                result.FMI_Installment_Type = feepge.FMI_Installment_Type;
        //                result.FMI_ActiceFlag = feepge.FMI_ActiceFlag;
        //                result.UpdatedDate = DateTime.Now;
        //                _FeeGroupContext.Update(result);
        //                var contactExists = _FeeGroupContext.SaveChanges();
        //                if (contactExists == 1)
        //                {
        //                    retval = "Update";
        //                    FGpage.returnduplicatestatus = retval;
        //                    saveyeardatanew(feepge.FMI_Id, 1, FGpage);
        //                }
        //                else
        //                {
        //                    retval = "NotUpdate";
        //                    FGpage.returnduplicatestatus = retval;
        //                }

        //            }
        //        }
        //        else
        //        {
        //            var result = _FeeGroupContext.feeMI.Where(t => t.FMI_Id == feepge.FMI_Id || t.FMI_Name == feepge.FMI_Name && t.MI_Id == feepge.MI_Id);

        //            if(feepge.FMI_Installment_Type=="2")
        //            {
        //                feepge.FMI_No_Of_Installments = 1;
        //            }
        //            else if(feepge.FMI_Installment_Type == "3")
        //            {
        //                feepge.FMI_No_Of_Installments = 12;
        //            }
        //            else if (feepge.FMI_Installment_Type == "4")
        //            {
        //                feepge.FMI_No_Of_Installments = 4;
        //            }
        //            else if (feepge.FMI_Installment_Type == "5")
        //            {
        //                feepge.FMI_No_Of_Installments = 1;
        //            }
        //            else if (feepge.FMI_Installment_Type == "6")
        //            {
        //                feepge.FMI_No_Of_Installments = 2;
        //            }
        //            else if (feepge.FMI_Installment_Type == "7")
        //            {
        //                feepge.FMI_No_Of_Installments = 24;
        //            }

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

        //                    retval = "Save";
        //                    FGpage.returnduplicatestatus = retval;
        //                    for (int i = 0; i < FGpage.fydto.Count; i++)
        //                    {
        //                        var result1 = _FeeGroupContext.feeMIY.Where(t => t.FMI_Id == feepge.FMI_Id && t.FTI_Name == FGpage.fydto[i].FTI_Name);
        //                        var result2 = _FeeGroupContext.feeMIY.Where(t=>t.FTI_Name == FGpage.fydto[i].FTI_Name);

        //                        if (result1.Count() > 0 || result2.Count() > 0)
        //                        {
        //                            retval = "Duplicate";
        //                            FGpage.returnduplicatestatus = retval;
        //                        }
        //                        else
        //                        {
        //                            saveyeardata(feepge.FMI_Id, FGpage.fydto[i].FTI_Name, FGpage.MI_Id);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    retval = "NotSave";
        //                    FGpage.returnduplicatestatus = retval;
        //                }
        //            }
        //        }
        //        List<FeeInstallmentDMO> allpages = new List<FeeInstallmentDMO>();
        //        allpages = _FeeGroupContext.feeMI.OrderByDescending(t => t.CreatedDate).ToList();
        //        FGpage.InstallmentData = allpages.ToArray();

        //    }
        //    catch (Exception ee)
        //    {
        //        _logger.LogError(ee.Message);
        //        Console.WriteLine(ee.Message);
        //    }
        //    return FGpage;
        //}

        public FeeInstallmentDTO SaveGroupData(FeeInstallmentDTO FGpage)
        {            
            FeeInstallmentDMO feepge = Mapper.Map<FeeInstallmentDMO>(FGpage);
            FeeInstallmentyeralyDTO te = new FeeInstallmentyeralyDTO();
            
            string retval = "";
            try
            {
                if (feepge.FMI_Id > 0)
                {
                    var duplicateKeys = FGpage.fydto.GroupBy(x => x.FTI_Name)
                           .Where(group => group.Count() > 1)
                           .Select(group => group.Key).ToList();
                    if (duplicateKeys.Count > 0)
                    {
                        FGpage.msg="Installment Name Contains Some Duplicate Values";
                        return FGpage;
                    }
                    else
                    {
                        var res = _FeeGroupContext.feeMI.Where(t => t.FMI_Id != feepge.FMI_Id && t.FMI_Name == feepge.FMI_Name && t.MI_Id == feepge.MI_Id);
                        if (res.Count() > 0)
                        {
                            retval = "Duplicate";
                            FGpage.returnduplicatestatus = retval;
                        }
                        else
                        {
                            var result = _FeeGroupContext.feeMI.Single(t => t.FMI_Id == feepge.FMI_Id);
                            result.MI_Id = feepge.MI_Id;
                            result.FMI_Name = feepge.FMI_Name;
                            result.FMI_No_Of_Installments = feepge.FMI_No_Of_Installments;
                            result.FMI_Installment_Type = feepge.FMI_Installment_Type;
                            result.FMI_ActiceFlag = feepge.FMI_ActiceFlag;
                            result.UpdatedDate = DateTime.Now;
                            _FeeGroupContext.Update(result);
                            for (int i = 0; i < FGpage.fydto.Count; i++)
                            {
                                // FeeInstallmentsyearlyDMO feepgeY = Mapper.Map<FeeInstallmentsyearlyDMO>(FGpage.fydto[i]);
                                //  var resultnew = _FeeGroupContext.feeMIY.FirstOrDefault(t => t.FTI_Id == FGpage.fydto[i].FTI_Id);
                              
                                if (FGpage.fydto[i].FTI_Id == 0)
                                {
                                    FeeInstallmentsyearlyDMO feepgeY = Mapper.Map<FeeInstallmentsyearlyDMO>(FGpage.fydto[i]);
                                    feepgeY.FMI_Id = feepge.FMI_Id;
                                    feepgeY.FTI_Name = FGpage.fydto[i].FTI_Name;
                                    feepgeY.MI_ID = FGpage.MI_Id;
                                    feepgeY.CreatedDate = DateTime.Now;
                                    feepgeY.UpdatedDate = DateTime.Now;
                                    _FeeGroupContext.Add(feepgeY);
                                }
                                else {
                                    FeeInstallmentsyearlyDMO feepgeY = _FeeGroupContext.FeeInstallmentsyearlyDMO.Single(t => t.FTI_Id == FGpage.fydto[i].FTI_Id);
                                   // feepgeY.FMI_Id = feepge.FMI_Id;
                                    feepgeY.FTI_Name = FGpage.fydto[i].FTI_Name;
                                   // feepgeY.MI_ID = FGpage.MI_Id;
                                   // feepgeY.CreatedDate = DateTime.Now;
                                    feepgeY.UpdatedDate = DateTime.Now;
                                    _FeeGroupContext.Update(feepgeY);
                                }                                
                            }
                            var contactExists = _FeeGroupContext.SaveChanges();
                            if (contactExists > 0)
                            {
                                retval = "Update";
                                FGpage.returnduplicatestatus = retval;

                            }
                            else
                            {
                                retval = "NotUpdate";
                                FGpage.returnduplicatestatus = retval;
                            }                           
                        }
                    }
                }

                else
                {
                    var duplicateKeys = FGpage.fydto.GroupBy(x => x.FTI_Name).Where(group => group.Count() > 1).Select(group => group.Key).ToList();
                    if (duplicateKeys.Count > 0)
                    {
                        FGpage.msg = "Installment Name Contains Some Duplicate Values";
                        return FGpage;
                    }
                    else
                    {
                        var result = _FeeGroupContext.feeMI.Where(t => t.FMI_Id == feepge.FMI_Id || t.FMI_Name == feepge.FMI_Name && t.MI_Id == feepge.MI_Id);

                        if (feepge.FMI_Installment_Type == "2")
                        {
                            feepge.FMI_No_Of_Installments = 1;
                        }
                        else if (feepge.FMI_Installment_Type == "3")
                        {
                            feepge.FMI_No_Of_Installments = 12;
                        }
                        else if (feepge.FMI_Installment_Type == "4")
                        {
                            feepge.FMI_No_Of_Installments = 4;
                        }
                        else if (feepge.FMI_Installment_Type == "5")
                        {
                            feepge.FMI_No_Of_Installments = 1;
                        }
                        else if (feepge.FMI_Installment_Type == "6")
                        {
                            feepge.FMI_No_Of_Installments = 2;
                        }
                        else if (feepge.FMI_Installment_Type == "7")
                        {
                            feepge.FMI_No_Of_Installments = 24;
                        }

                        if (result.Count() > 0)
                        {
                            retval = "Duplicate";
                            FGpage.returnduplicatestatus = retval;
                            return FGpage;
                        }
                        else
                        {
                            
                            feepge.CreatedDate = DateTime.Now;
                            feepge.UpdatedDate = DateTime.Now;
                            _FeeGroupContext.Add(feepge);
                            for (int i = 0; i < FGpage.fydto.Count; i++)
                            {
                                FeeInstallmentsyearlyDMO feepgeY = Mapper.Map<FeeInstallmentsyearlyDMO>(FGpage.fydto[i]);
                                feepgeY.FMI_Id = feepge.FMI_Id;
                                feepgeY.FTI_Name = FGpage.fydto[i].FTI_Name;
                                feepgeY.MI_ID = FGpage.MI_Id;
                                feepgeY.CreatedDate = DateTime.Now;
                                feepgeY.UpdatedDate = DateTime.Now;
                                _FeeGroupContext.Add(feepgeY);                                
                            }
                            var contactExists = _FeeGroupContext.SaveChanges();
                            if (contactExists > 0)
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
                
                List<FeeInstallmentDMO> allpages = new List<FeeInstallmentDMO>();
                allpages = _FeeGroupContext.feeMI.OrderByDescending(t => t.CreatedDate).ToList();
                FGpage.InstallmentData = allpages.ToArray();
               
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGpage;
        }

        public FeeInstallmentDTO getdetails(FeeInstallmentDTO FGRDT)
        {
            //FeeInstallmentDTO FGRDT = new FeeInstallmentDTO();
            try
            {
                List<FeeInstallmentDMO> feegrp = new List<FeeInstallmentDMO>();
                feegrp = _FeeGroupContext.feeMI.Where(t => t.MI_Id == FGRDT.MI_Id).OrderBy(t => t.FMI_Name).ToList();
                FGRDT.InstallmentData = feegrp.ToArray();

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == FGRDT.MI_Id && t.Is_Active == true).OrderByDescending(t=>t.ASMAY_Order).ToList();
                FGRDT.academicdrp = allyear.Distinct().OrderByDescending(b=>b.ASMAY_Order).ToArray();


                List<FeeInstallmentDMO> allinstypes = new List<FeeInstallmentDMO>();
                allinstypes = _FeeGroupContext.feeMI.Where(t => t.MI_Id == FGRDT.MI_Id && t.FMI_ActiceFlag == true).ToList();
                FGRDT.instypesdrp = allinstypes.ToArray();


                FGRDT.datasendhtml = (from a in _FeeGroupContext.feeMIY
                                      from b in _FeeGroupContext.feeIDDD
                                      where (a.FTI_Id == b.FTI_Id && a.MI_ID == FGRDT.MI_Id)
                                      select new FeeInstalmentDueDateDTO
                                      {
                                          FTIDD_Id = b.FTIDD_Id,
                                          fname = a.FTI_Name,
                                          //fdate1 = b.FTIDD_FromDate,
                                          //tdate1 = b.FTIDD_ToDate,
                                          //Aplc1 = b.FTIDD_ApplicableDate,
                                          //ddate1 = b.FTIDD_DueDate,
                                          FTIDD_FromDate = b.FTIDD_FromDate,
                                          FTIDD_ToDate = b.FTIDD_ToDate,
                                          FTIDD_ApplicableDate = b.FTIDD_ApplicableDate,
                                          FTIDD_DueDate = b.FTIDD_DueDate,
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

        public FeeInstallmentDTO EditgroupDetails(int id)
        {
            FeeInstallmentDTO FMG = new FeeInstallmentDTO();
            try
            {
                List<FeeInstallmentDMO> masterfeegroup = new List<FeeInstallmentDMO>();
                masterfeegroup = _FeeGroupContext.feeMI.AsNoTracking().Where(t => t.FMI_Id.Equals(id)).ToList();
                FMG.InstallmentData = masterfeegroup.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FMG;
        }
        public FeeInstallmentDTO GetGroupSearchData(FeeInstallmentDTO mas)
        {

            FeeInstallmentDTO FGRDT = new FeeInstallmentDTO();
            try
            {
                List<FeeInstallmentDMO> feegrp = new List<FeeInstallmentDMO>();
                feegrp = _FeeGroupContext.feeMI.ToList();
                FGRDT.InstallmentData = feegrp.ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGRDT;
        }
        public FeeInstallmentDTO getpageedit(int id)
        {
            FeeInstallmentDTO page = new FeeInstallmentDTO();
            try
            {
                List<FeeInstallmentDMO> lorg = new List<FeeInstallmentDMO>();
                lorg = _FeeGroupContext.feeMI.AsNoTracking().Where(t => t.FMI_Id.Equals(id)).ToList();
                page.InstallmentData = lorg.ToArray();

                List<FeeInstallmentsyearlyDMO> lorg123 = new List<FeeInstallmentsyearlyDMO>();
                lorg123 = _FeeGroupContext.feeMIY.AsNoTracking().Where(t => t.FMI_Id.Equals(id)).ToList();
                page.InTData = lorg123.ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public FeeInstalmentDueDateDTO getpageeditY(int id)
        {
            FeeInstalmentDueDateDTO page = new FeeInstalmentDueDateDTO();
            try
            {
                page.InstallmentDatad = (from a in _FeeGroupContext.feeMI
                                         from b in _FeeGroupContext.feeMIY
                                         from c in _FeeGroupContext.feeIDDD
                                         where (a.FMI_Id == b.FMI_Id && b.FTI_Id == c.FTI_Id && c.FTIDD_Id == id)
                                         select new FeeInstalmentDueDateDTO
                                         {

                                             FTIDD_Id = Convert.ToInt64(id),
                                             FTI_Id = b.FTI_Id,
                                             ftI_Name = b.FTI_Name,
                                             fmiid = a.FMI_Id,
                                             yrid = c.ASMAY_Id,
                                             FTIDD_FromDate = c.FTIDD_FromDate,
                                             FTIDD_ToDate = c.FTIDD_ToDate,
                                             FTIDD_ApplicableDate = c.FTIDD_ApplicableDate,
                                             FTIDD_DueDate = c.FTIDD_DueDate,
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
        public FeeInstallmentDTO deleterec(int id)
        {
            bool returnresult = false;
            FeeInstallmentDTO page = new FeeInstallmentDTO();
            bool rtvalue = deleteforeighkeyrecord(id);     //  first delete foreigh key table 
            if (rtvalue == true)  // true refere delete
            {


                List<FeeInstallmentDMO> lorg = new List<FeeInstallmentDMO>();
                lorg = _FeeGroupContext.feeMI.Where(t => t.FMI_Id.Equals(id)).ToList();
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

                    List<FeeInstallmentDMO> allpages = new List<FeeInstallmentDMO>();
                    allpages = _FeeGroupContext.feeMI.ToList();
                    page.InstallmentData = allpages.ToArray();
                }
                catch (Exception ee)
                {
                    _logger.LogError(ee.Message);
                    Console.WriteLine(ee.Message);
                }
            }
            else
            {
                returnresult = false;
                page.returnval = returnresult;
            }

            return page;

        }
        //public FeeInstalmentDueDateDTO deleterecY(int id)
        //{

        //    bool returnresult = false;

        //    FeeInstalmentDueDateDTO page = new FeeInstalmentDueDateDTO();
        //    List<FeeInstallmentDueDateDMO> lorg = new List<FeeInstallmentDueDateDMO>();


        //    var exists1 = _FeeGroupContext.Feeduedateinstall.Where(t => t.FTIDD_Id == id).ToArray();

        //    var exists2 = _FeeGroupContext.feeMIY.Where(t => t.FTI_Id == exists1.FirstOrDefault().FTI_Id).ToArray();
        //    var exists = _FeeGroupContext.FeeYearlygroupHeadMappingDMO.Where(t => t.FMI_Id == exists2.FirstOrDefault().FMI_Id).ToArray().Count();
        //    if (exists == 0)
        //    {
        //        lorg = _FeeGroupContext.feeIDDD.Where(t => t.FTIDD_Id.Equals(id)).ToList();

        //        try
        //        {
        //            if (lorg.Any())
        //            {
        //                _FeeGroupContext.Remove(lorg.ElementAt(0));

        //                var contactExists = _FeeGroupContext.SaveChanges();
        //                if (contactExists == 1)
        //                {
        //                    returnresult = true;
        //                    page.returnval = returnresult;
        //                }
        //                else
        //                {
        //                    returnresult = false;
        //                    page.returnval = returnresult;
        //                }
        //            }

        //            List<FeeInstallmentDueDateDMO> allpages = new List<FeeInstallmentDueDateDMO>();
        //            allpages = _FeeGroupContext.feeIDDD.ToList();
        //            page.InstallmentDatad = allpages.ToArray();
        //        }
        //        catch (Exception ee)
        //        {
        //            _logger.LogError(ee.Message);
        //            Console.WriteLine(ee.Message);
        //        }
        //    }
        //    else
        //    {

        //        page.returnvalexist = "Exist";
        //    }
        //    return page;
        //}

          //Added By Praveen gouda 15/12/2023
        public FeeInstalmentDueDateDTO deleterecY(FeeInstalmentDueDateDTO id)
        {

            bool returnresult = false;

            FeeInstalmentDueDateDTO page = new FeeInstalmentDueDateDTO();
            List<FeeInstallmentDueDateDMO> lorg = new List<FeeInstallmentDueDateDMO>();


            var exists1 = _FeeGroupContext.Feeduedateinstall.Where(t => t.FTIDD_Id == id.FTIDD_Id).ToArray();

            var exists2 = _FeeGroupContext.feeMIY.Where(t => t.FTI_Id == exists1.FirstOrDefault().FTI_Id).ToArray();
            var exists = _FeeGroupContext.FeeYearlygroupHeadMappingDMO.Where(t => t.FMI_Id == exists2.FirstOrDefault().FMI_Id).ToArray().Count();
            if (exists == 0)
            {
                lorg = _FeeGroupContext.feeIDDD.Where(t => t.FTIDD_Id.Equals(id.FTIDD_Id)).ToList();

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

                    List<FeeInstallmentDueDateDMO> allpages = new List<FeeInstallmentDueDateDMO>();
                    allpages = _FeeGroupContext.feeIDDD.ToList();
                    page.InstallmentDatad = allpages.ToArray();
                }
                catch (Exception ee)
                {
                    _logger.LogError(ee.Message);
                    Console.WriteLine(ee.Message);
                }
            }
            else
            {

                page.returnvalexist = "Exist";
            }
            return page;
        }

        public FeeInstallmentDTO deactivate(FeeInstallmentDTO acd)
        {
            try
            {
                FeeInstallmentDMO feepge = Mapper.Map<FeeInstallmentDMO>(acd);
                if (feepge.FMI_Id > 0)
                {
                    var result = _FeeGroupContext.feeMI.Single(t => t.FMI_Id == feepge.FMI_Id);

                    var feestutrans = (from a in _FeeGroupContext.feeMIY
                                       from b in _FeeGroupContext.FeeAmountEntryDMO
                                       where(a.FTI_Id==b.FTI_Id && a.FTI_Active==true && a.FMI_Id == feepge.FMI_Id)
                                       select a).ToList();
                    if (feestutrans.Count > 0)
                    {
                        acd.returnvalue = "used";
                        return acd;
                    }
                    else
                    {
                        result.UpdatedDate = DateTime.Now;

                        if (result.FMI_ActiceFlag == true)
                        {
                            result.FMI_ActiceFlag = false;
                            result.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            result.FMI_ActiceFlag = true;
                            result.UpdatedDate = DateTime.Now;
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
                    List<FeeInstallmentDMO> allorganisation = new List<FeeInstallmentDMO>();
                    allorganisation = _FeeGroupContext.feeMI.ToList();
                    acd.InstallmentData = allorganisation.ToArray();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }
        
        public FeeInstallmentyeralyDTO[] GetWrittenTestMarks(FeeInstallmentyeralyDTO mas)
        {
            List<FeeInstallmentyeralyDTO> AllInOne = new List<FeeInstallmentyeralyDTO>();
            //   instemp temp = new instemp();
            int count = Convert.ToInt32(mas.valueloop);
            for (int i = 0; i < count; i++)
            {
                FeeInstallmentyeralyDTO temp = new FeeInstallmentyeralyDTO();
                temp.fno1 = i + 1;
                temp.fname1 = "";
                AllInOne.Add(temp);
            }
            return AllInOne.ToArray();
        }
        [Route("years")]
        public async Task<FeeInstallmentDTO> getIndependentDropDowns(FeeInstallmentDTO yrs)
        {
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = await _FeeGroupContext.AcademicYear.ToListAsync();
                yrs.academicdrp = allyear.ToArray();


                List<FeeInstallmentDMO> allinstypes = new List<FeeInstallmentDMO>();
                allinstypes = await _FeeGroupContext.feeMI.ToListAsync();
                yrs.instypesdrp = allinstypes.ToArray();


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var str = ex.Message;
            }

            return yrs;
        }
        public FeeInstallmentyeralyDTO[] Getduedates(FeeInstallmentyeralyDTO mas)
        {
            //List<FeeInstallmentDueDateDMO> Allrows = new List<FeeInstallmentDueDateDMO>();
            //Allrows = _FeeGroupContext.feeIDDD.Where(t => t.ASMAY_Id==mas.ASMAY_Id && t.MI_Id==mas.MI_Id && t.FTI_Id==mas.FTI_Id).ToList();
            //mas.yrlData = Allrows.ToArray();

            mas.fillsaveddata = (from a in _FeeGroupContext.FeeInstallmentDMO
                                 from b in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                 from c in _FeeGroupContext.feeIDDD
                                 where (a.FMI_Id == b.FMI_Id && b.MI_ID == mas.MI_Id && c.ASMAY_Id == mas.ASMAY_Id)
                                 select new FeeInstallmentyeralyDTO
                                 {
                                     FTI_Id = b.FTI_Id,
                                     FTI_Name = b.FTI_Name,
                                     fdate = c.FTIDD_FromDate,
                                     tdate = c.FTIDD_ToDate,
                                     Aplc = c.FTIDD_ApplicableDate,
                                     ddate = c.FTIDD_DueDate
                                 }
                            ).ToArray();


            List<FeeInstallmentyeralyDTO> AllInOne = new List<FeeInstallmentyeralyDTO>();
            List<FeeInstallmentsyearlyDMO> Allrows = new List<FeeInstallmentsyearlyDMO>();
            //Allrows = _FeeGroupContext.feeMIY.Where(t => t.FMI_Id.Equals(mas.FMI_Id)).ToList();
            //mas.yrlData = Allrows.ToArray();

            //for (int i = 0; i < Allrows.Count; i++)
            //{

            FeeInstallmentyeralyDTO temp = new FeeInstallmentyeralyDTO();
            List<FeeInstallmentyeralyDTO> fillsaveddata = new List<FeeInstallmentyeralyDTO>();
            List<FeeInstallmentsyearlyDMO> Allname10 = new List<FeeInstallmentsyearlyDMO>();
            List<long> Allname2 = new List<long>();

            fillsaveddata = (
                                 from b in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                 from c in _FeeGroupContext.feeIDDD
                                 where (b.FTI_Id == c.FTI_Id && b.MI_ID == mas.MI_Id && c.ASMAY_Id == mas.ASMAY_Id && b.FMI_Id == mas.FMI_Id)
                                 select new FeeInstallmentyeralyDTO
                                 {
                                     FTI_Id = b.FTI_Id,
                                     FTI_Name = b.FTI_Name,
                                     fdate = c.FTIDD_FromDate,
                                     tdate = c.FTIDD_ToDate,
                                     Aplc = c.FTIDD_ApplicableDate,
                                     ddate = c.FTIDD_DueDate
                                 }
                       ).ToList();
            foreach (var a in fillsaveddata)
            {
                Allname2.Add(a.FTI_Id);
            }

            Allname10 = _FeeGroupContext.feeMIY.Where(t => t.FMI_Id.Equals(mas.FMI_Id) && !Allname2.Contains(t.FTI_Id)).ToList().ToList();


            if (Allname10.Count > 0)
            {
                for (int i = 0; i < Allname10.Count; i++)
                {
                    //mas.yrlData = Allname10.ToArray();
                    //temp.FTI_Id = Allname10[i].FTI_Id;
                    //temp.FTI_Name = Allname10[i].FTI_Name;
                    FeeInstallmentyeralyDTO dto = Mapper.Map<FeeInstallmentyeralyDTO>(Allname10[i]);
                    AllInOne.Add(dto);
                }
                mas.instlreturn = "Yes";
            }
            else
            {
                mas.instlreturn = "No";
            }



            //}

            // return mas;
            return AllInOne.ToArray();

        }
        public FeeInstallmentDTO savedetailDDD(FeeInstallmentDTO FGpage)
        {
            string retvalue = "false";
            for (int i = 0; i < FGpage.fidddto.Count; i++)
            {
                retvalue = savedetailDDDinDB(FGpage.MI_Id, FGpage.temyrid, FGpage.fidddto[i].FTI_Id, FGpage.fidddto[i].FTIDD_Id, FGpage.fidddto[i].FTIDD_FromDate, FGpage.fidddto[i].FTIDD_ToDate, FGpage.fidddto[i].FTIDD_ApplicableDate, FGpage.fidddto[i].FTIDD_DueDate);
            }
            FGpage.returnvalue = retvalue;
            return FGpage;
        }
        public string savedetailDDDinDB(long mid, long yrid, long id, long ftidd, DateTime fromdate, DateTime todate, DateTime apldate, DateTime ddudate)
        {
            string returnvalue = "false";
            FeeInstalmentDueDateDTO te = new FeeInstalmentDueDateDTO();
            FeeInstallmentDueDateDMO feepgeY = Mapper.Map<FeeInstallmentDueDateDMO>(te);
            try
            {
                if (ftidd > 0)
                {
                    var result = _FeeGroupContext.feeIDDD.Single(t => t.FTIDD_Id == ftidd);
                    result.MI_Id = mid;
                    result.FTI_Id = id;
                    result.ASMAY_Id = yrid;
                    result.FTIDD_FromDate = fromdate;
                    result.FTIDD_ToDate = todate;
                    result.FTIDD_ApplicableDate = apldate;
                    result.FTIDD_DueDate = ddudate;
                    result.UpdatedDate = DateTime.Now;
                    _FeeGroupContext.Update(result);
                    var contactExists = _FeeGroupContext.SaveChanges();
                    if (contactExists == 1)
                    {

                        returnvalue = "Update";
                    }
                    else
                    {
                        returnvalue = "NotUpdate";
                    }
                }
                else
                {
                    var result = _FeeGroupContext.feeIDDD.Where(t => t.FTI_Id == id && t.ASMAY_Id == yrid && t.MI_Id == mid);
                    if (result.Count() > 0)
                    {
                        returnvalue = "Duplicate";
                    }
                    else
                    {
                        feepgeY.MI_Id = mid;
                        feepgeY.FTI_Id = id;
                        feepgeY.ASMAY_Id = yrid;
                        feepgeY.FTIDD_FromDate = fromdate;
                        feepgeY.FTIDD_ToDate = todate;
                        feepgeY.FTIDD_ApplicableDate = apldate;
                        feepgeY.FTIDD_DueDate = ddudate;
                        feepgeY.CreatedDate = DateTime.Now;
                        feepgeY.UpdatedDate = DateTime.Now;
                        _FeeGroupContext.Add(feepgeY);
                        var contactExists = _FeeGroupContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnvalue = "Save";
                        }
                        else
                        {
                            returnvalue = "NotSave";
                        }
                    }
                }
                List<FeeInstallmentDueDateDMO> allpages = new List<FeeInstallmentDueDateDMO>();
                allpages = _FeeGroupContext.feeIDDD.OrderBy(t => t.FTIDD_Id).ToList();
                te.retrivedata = allpages.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }

            return returnvalue;
        }
        public bool deleteforeighkeyrecord(long id)
        {
            bool returnresult = false;
            List<FeeYearlygroupHeadMappingDMO> pmm = new List<FeeYearlygroupHeadMappingDMO>();
            pmm = _FeeGroupContext.FeeYearlygroupHeadMappingDMO.Where(t => t.FMI_Id.Equals(id)).ToList();
            if (pmm.Count == 0)
            {
                FeeInstallmentyeralyDTO page = new FeeInstallmentyeralyDTO();
                List<FeeInstallmentsyearlyDMO> lorg = new List<FeeInstallmentsyearlyDMO>();
                lorg = _FeeGroupContext.feeMIY.Where(t => t.FMI_Id.Equals(id)).ToList();
                try
                {
                    if (lorg.Count() > 0)
                    {
                        string condition = "deletefalse";
                        for (int i = 0; lorg.Count > i; i++)
                        {
                            FeeAmountEntryDTO page4 = new FeeAmountEntryDTO();
                            List<FeeAmountEntryDMO> lorg4 = new List<FeeAmountEntryDMO>();
                            lorg4 = _FeeGroupContext.FeeAmountEntryDMO.Where(t => t.FTI_Id.Equals(lorg[i].FTI_Id)).ToList();
                            if (lorg4.Count > 0)
                            {
                                condition = "deletefalse";
                            }
                            else
                            {
                                condition = "deletetrue";
                            }

                        }

                        if (condition == "deletetrue")
                        {
                            for (int i = 0; lorg.Count > i; i++)
                            {
                                FeeInstalmentDueDateDTO page1 = new FeeInstalmentDueDateDTO();
                                List<FeeInstallmentDueDateDMO> lorg1 = new List<FeeInstallmentDueDateDMO>();
                                lorg1 = _FeeGroupContext.feeIDDD.Where(t => t.FTI_Id.Equals(lorg[i].FTI_Id)).ToList();
                                try
                                {
                                    if (lorg1.Any())
                                    {
                                        _FeeGroupContext.Remove(lorg1.ElementAt(0));
                                        var contactExists1 = _FeeGroupContext.SaveChanges();
                                        if (contactExists1 == 1)
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
                                    Console.WriteLine(ee.Message);
                                }

                            }
                            for (int i = 0; lorg.Count > i; i++)
                            {
                                _FeeGroupContext.Remove(lorg.ElementAt(i));
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
                        else
                        {
                            returnresult = false;

                        }
                    }
                    else
                    {
                        returnresult = true;
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }
            else
            {
                returnresult = false;
            }

            return returnresult;
        }
        
    }
}
