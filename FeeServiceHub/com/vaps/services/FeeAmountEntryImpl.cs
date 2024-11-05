﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using System.Dynamic;
using Microsoft.Extensions.Logging;
using DomainModel.Model.com.vapstech.Fee;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeAmountEntryImpl : interfaces.FeeAmountEntryInterfaces
    {
        private static ConcurrentDictionary<string, FeeAmountEntryDTO> _login =
            new ConcurrentDictionary<string, FeeAmountEntryDTO>();

        public FeeGroupContext _YearlyFeeGroupMappingContext;
        readonly ILogger<FeeAmountEntryImpl> _logger;
        public FeeAmountEntryImpl(FeeGroupContext YearlyFeeGroupMappingContext, ILogger<FeeAmountEntryImpl> log)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _logger = log;
        }

        public FeeAmountEntryDTO deleterec(FeeAmountEntryDTO page)
        {
            try
            {
                if (page.selectiontype.Equals("stud"))
                {
                    using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                    {
                        // FeeAmountEntryDTO page = new FeeAmountEntryDTO();
                        List<FeeAmountEntryDMO> lorg = new List<FeeAmountEntryDMO>();
                        List<FeeTDueDateRegularDMO> lorgduedate = new List<FeeTDueDateRegularDMO>();
                        List<FeeTDueDateECSDMO> lorgduedateecs = new List<FeeTDueDateECSDMO>();
                        List<FeeTFineSlabDMO> fineslab = new List<FeeTFineSlabDMO>();
                        List<FeeTFineSlabECSDMO> fineslabecs = new List<FeeTFineSlabECSDMO>();

                        List<FeeStudentTransactionDMO> feestutrans = new List<FeeStudentTransactionDMO>();

                        lorg = _YearlyFeeGroupMappingContext.FeeAmountEntryDMO.Where(t => t.FMA_Id.Equals(page.FMA_Id)).ToList();

                        lorgduedate = _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO.Where(t => t.FMA_Id.Equals(page.FMA_Id)).ToList();

                        lorgduedateecs = _YearlyFeeGroupMappingContext.feeTDueDateECSDMO.Where(t => t.FMA_Id.Equals(page.FMA_Id)).ToList();

                        fineslab = _YearlyFeeGroupMappingContext.feeTFineSlabDMO.Where(t => t.FMA_Id.Equals(page.FMA_Id)).ToList();

                        fineslabecs = _YearlyFeeGroupMappingContext.feeTFineSlabECSDMO.Where(t => t.FMA_Id.Equals(page.FMA_Id)).ToList();

                        feestutrans = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Where(t => t.FMA_Id.Equals(page.FMA_Id) && t.FSS_PaidAmount > 0 && t.MI_Id == page.MI_Id && t.ASMAY_Id == page.ASMAY_Id).ToList();

                        try
                        {
                            if (feestutrans.Count < 1)
                            {
                                if (lorg.Any())
                                {
                                    if (lorgduedate.Count > 0)
                                    {
                                        _YearlyFeeGroupMappingContext.Remove(lorgduedate.ElementAt(0));
                                        var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
                                    }
                                    if (lorgduedateecs.Count > 0)
                                    {
                                        _YearlyFeeGroupMappingContext.Remove(lorgduedateecs.ElementAt(0));
                                        var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
                                    }
                                    if (fineslab.Count > 0)
                                    {
                                        _YearlyFeeGroupMappingContext.Remove(fineslab.ElementAt(0));
                                        var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
                                    }
                                    if (fineslabecs.Count > 0)
                                    {
                                        _YearlyFeeGroupMappingContext.Remove(fineslabecs.ElementAt(0));
                                        var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
                                    }
                                    if (lorg.Count > 0)
                                    {
                                        _YearlyFeeGroupMappingContext.Remove(lorg.ElementAt(0));
                                        var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();

                                        if (contactExists >= 1)
                                        {
                                            page.returnval = "true";
                                            transaction.Commit();
                                        }
                                        else
                                        {
                                            page.returnval = "false";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                page.returnval = "RecordExists";
                            }

                            page.alldata = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                            from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                            from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                            from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                            where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == page.MI_Id && a.ASMAY_Id == page.ASMAY_Id)
                                            select new FeeAmountEntryDTO
                                            {
                                                FMG_GroupName = b.FMG_GroupName,
                                                FMH_FeeName = c.FMH_FeeName,
                                                FTI_Name = d.FTI_Name,
                                                FMA_Amount = a.FMA_Amount
                                            }
                            ).ToArray();

                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                            _logger.LogError(ee.Message);
                            page.returnval = "false";
                        }

                    }
                }
                else if (page.selectiontype.Equals("stfoth") || page.selectiontype.Equals("stfothamt"))
                {
                    using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                    {
                        // FeeAmountEntryDTO page = new FeeAmountEntryDTO();
                        List<Fee_Master_Amount_OthStaffs> lorg = new List<Fee_Master_Amount_OthStaffs>();
                        List<Fee_T_Due_Date_OthStaffs> lorgduedate = new List<Fee_T_Due_Date_OthStaffs>();
                        List<Fee_T_Fine_Slabs_OthStaffs> fineslab = new List<Fee_T_Fine_Slabs_OthStaffs>();
                        List<FeeStudentTransactionDMO> feestutrans = new List<FeeStudentTransactionDMO>();

                        lorg = _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs.Where(t => t.FMAOST_Id == page.FMA_Id).ToList();

                        lorgduedate = _YearlyFeeGroupMappingContext.Fee_T_Due_Date_OthStaffs.Where(t => t.FMAOST_Id == page.FMA_Id).ToList();

                        fineslab = _YearlyFeeGroupMappingContext.Fee_T_Fine_Slabs_OthStaffs.Where(t => t.FMAOST_Id == page.FMA_Id).ToList();

                        feestutrans = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Where(t => t.FSS_PaidAmount > 0 && t.MI_Id == page.MI_Id && t.ASMAY_Id == page.ASMAY_Id).ToList();

                        try
                        {
                            if (feestutrans.Count == 0)
                            {
                                if (lorg.Any())
                                {
                                    if (lorgduedate.Count > 0)
                                    {
                                        _YearlyFeeGroupMappingContext.Remove(lorgduedate.ElementAt(0));
                                        var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
                                    }

                                    if (fineslab.Count > 0)
                                    {
                                        _YearlyFeeGroupMappingContext.Remove(fineslab.ElementAt(0));
                                        var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
                                    }

                                    if (lorg.Count > 0)
                                    {
                                        _YearlyFeeGroupMappingContext.Remove(lorg.ElementAt(0));
                                        var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();

                                        if (contactExists >= 1)
                                        {
                                            page.returnval = "true";
                                            transaction.Commit();
                                        }
                                        else
                                        {
                                            page.returnval = "false";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                page.returnval = "RecordExists";
                            }

                            if (page.selectiontype.Equals("stfoth"))
                            {
                                page.alldata = (from a in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                                from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                                from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                                where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == page.MI_Id && a.ASMAY_Id == page.ASMAY_Id && a.FMAOST_OthStaffFlag == "S")
                                                select new FeeAmountEntryDTO
                                                {
                                                    FMG_GroupName = b.FMG_GroupName,
                                                    FMH_FeeName = c.FMH_FeeName,
                                                    FTI_Name = d.FTI_Name,
                                                    FMA_Amount = a.FMAOST_Amount
                                                }
                       ).ToArray();
                            }

                            if (page.selectiontype.Equals("stfothamt"))
                            {
                                page.alldata = (from a in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                                from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                                from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                                where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == page.MI_Id && a.ASMAY_Id == page.ASMAY_Id && a.FMAOST_OthStaffFlag == "O")
                                                select new FeeAmountEntryDTO
                                                {
                                                    FMG_GroupName = b.FMG_GroupName,
                                                    FMH_FeeName = c.FMH_FeeName,
                                                    FTI_Name = d.FTI_Name,
                                                    FMA_Amount = a.FMAOST_Amount
                                                }
                       ).ToArray();
                            }
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                            _logger.LogError(ee.Message);
                            page.returnval = "false";
                        }

                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return page;
        }

        public FeeAmountEntryDTO EditMasterscetionDetails(FeeAmountEntryDTO data)
        {
            try
            {
                data.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FYGHM_Id == data.FYGHM_Id)
                                select new FeeYearlygroupHeadMappingDTO
                                {
                                    FMH_Id = a.FMH_Id,
                                    FMG_Id = a.FMG_Id,
                                    FYGHM_Id = a.FYGHM_Id,
                                    FMG_GroupName = b.FMG_GroupName,
                                    FMH_FeeName = c.FMH_FeeName,
                                    FMI_Name = d.FMI_Name,
                                    FMI_Id = d.FMI_Id,
                                    FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag,
                                    FYGHM_Common_AmountFlag = a.FYGHM_Common_AmountFlag,
                                    FYGHM_ActiveFlag = a.FYGHM_ActiveFlag,
                                }
       ).ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public FeeAmountEntryDTO getdata(FeeAmountEntryDTO fee)
        {
            try
            {

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == fee.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                fee.academicdrp = allyear.Distinct().ToArray();

                List<FeeHeadDMO> head = new List<FeeHeadDMO>();
                head = _YearlyFeeGroupMappingContext.FeeHeadDMO.Where(t => t.MI_Id == fee.MI_Id && t.FMH_ActiveFlag == true).ToList();
                fee.fillmasterhead = head.ToArray();

                List<FeeGroupDMO> group = new List<FeeGroupDMO>();
                group = _YearlyFeeGroupMappingContext.FeeGroupDMO.Where(t => t.MI_Id == fee.MI_Id && t.FMG_ActiceFlag == true).ToList();
                fee.fillmastergroup = group.ToArray();

                List<MasterCompanyDMO> company = new List<MasterCompanyDMO>();
                company = _YearlyFeeGroupMappingContext.MasterCompanyDMO.ToList();
                fee.fillcompany = company.ToArray();


                List<MasterMonthDMO> mon = new List<MasterMonthDMO>();
                mon = _YearlyFeeGroupMappingContext.masterMonthDMO.ToList();
                fee.fillmonth = mon.ToArray();


                List<MasterMonthECSDMO> monecs = new List<MasterMonthECSDMO>();
                monecs = _YearlyFeeGroupMappingContext.masterMonthECSDMO.ToList();
                fee.fillmonthecs = monecs.ToArray();

                List<FeeInstallmentDMO> installment = new List<FeeInstallmentDMO>();
                // installment = _YearlyFeeGroupMappingContext.FeeInstallmentDMO.ToList();

                fee.fillinstallment = (from a in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                       from b in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                       where (a.FMI_Id == b.FMI_Id && a.MI_Id == fee.MI_Id && a.FMI_ActiceFlag == true)
                                       select new FeeAmountEntryDTO
                                       {
                                           FTI_Id = b.FTI_Id,
                                           FTI_Name = b.FTI_Name,
                                       }
       ).ToArray();

                //fee.fillinstallment = installment.ToArray();

                //List<FeeClassCategoryDMO> category = new List<FeeClassCategoryDMO>();
                //category = _YearlyFeeGroupMappingContext.FeeClassCategoryDMO.Where(t=>t.MI_Id==fee.MI_Id && t.FMCC_ActiveFlag==true).ToList();
                //fee.fillcategory = category.ToArray();

                fee.fillcategory = (from a in _YearlyFeeGroupMappingContext.FeeClassCategoryDMO
                                    from b in _YearlyFeeGroupMappingContext.feeYCC
                                    from c in _YearlyFeeGroupMappingContext.feeYCCC
                                    where (a.FMCC_Id == b.FMCC_Id && a.MI_Id == fee.MI_Id && b.ASMAY_Id == fee.ASMAY_Id && b.FYCC_Id == c.FYCC_Id && a.FMCC_ActiveFlag == true && b.FYCC_ActiveFlag == true)
                                    select new FeeAmountEntryDTO
                                    {
                                        FMCC_Id = b.FMCC_Id,
                                        FMCC_ClassCategoryName = a.FMCC_ClassCategoryName,
                                    }
              ).Distinct().ToArray();


                fee.alldata = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                               from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                               from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                               from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                               from e in _YearlyFeeGroupMappingContext.FeeClassCategoryDMO
                               from f in _YearlyFeeGroupMappingContext.AcademicYear
                               where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == fee.MI_Id && e.FMCC_Id == a.FMCC_Id && f.ASMAY_Id == a.ASMAY_Id)
                               select new FeeAmountEntryDTO
                               {
                                   FMG_GroupName = b.FMG_GroupName,
                                   FMH_FeeName = c.FMH_FeeName,
                                   FTI_Name = d.FTI_Name,
                                   FMA_Amount = a.FMA_Amount,
                                   FMA_Id = a.FMA_Id,
                                   FMCC_ClassCategoryName = e.FMCC_ClassCategoryName,
                                   ASMAY_Year = f.ASMAY_Year
                               }
               ).OrderBy(t => t.FMH_FeeName).ToArray();

                var dat = _YearlyFeeGroupMappingContext.AcademicYear.Single(y => y.MI_Id == fee.MI_Id && y.ASMAY_Id == fee.ASMAY_Id);
                fee.ASMAY_Year = dat.ASMAY_Year;


                List<FeeMasterConfigurationDMO> config = new List<FeeMasterConfigurationDMO>();
                config = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id == fee.MI_Id && t.userid == fee.user_id).ToList();
                fee.feeconfiguration = config.Distinct().ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return fee;

        }

        public FeeAmountEntryDTO getgroupheaddetails(FeeAmountEntryDTO data)
        {
            try
            {
                if (data.selectiontype == "stud")
                {
                    List<FeeYearlygroupHeadMappingDMO> commamtflag = new List<FeeYearlygroupHeadMappingDMO>();
                    commamtflag = _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.FYGHM_ActiveFlag == "1" && t.FYGHM_Common_AmountFlag == "Y" && t.FMG_Id == data.FMG_Id && t.ASMAY_Id==data.ASMAY_Id).ToList();
                    data.commountamountflag = commamtflag.ToArray();


                    data.newlyupdatedrec = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                            from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                            from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                            from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                            from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                            where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && d.FMI_Id == e.FMI_Id)
                                            select new FeeAmountEntryDTO
                                            {
                                                FMH_Id = a.FMH_Id,
                                                FTI_Id = e.FTI_Id,
                                                FMG_GroupName = b.FMG_GroupName,
                                                FMH_FeeName = c.FMH_FeeName,
                                                FMI_Name = e.FTI_Name,
                                                FMA_Amount = 0,
                                                FMH_Order = c.FMH_Order,
                                                FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag,
                                                FMA_DueDate = DateTime.Now,
                                                FTDDE_DueDate = DateTime.Now
                                            }
      ).OrderByDescending(t => t.FMH_Order).ToArray();

                    //feeamtentry.allgroupheaddata var p1
                    data.allgroupheaddata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                             from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                             from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                             from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                             from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                             from f in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                             from g in _YearlyFeeGroupMappingContext.feeTDueDateECSDMO
                                             from h in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                                             where (f.FMH_Id == c.FMH_Id && f.FMG_Id == b.FMG_Id && f.FTI_Id == e.FTI_Id && a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && f.FMCC_Id == data.FMCC_Id && d.FMI_Id == e.FMI_Id && f.FMA_Id == g.FMA_Id && f.FMA_Id == h.FMA_Id && a.ASMAY_Id == f.ASMAY_Id)
                                             select new FeeAmountEntryDTO
                                             {
                                                 FMA_Id = f.FMA_Id,
                                                 FMH_Id = a.FMH_Id,
                                                 FTI_Id = e.FTI_Id,
                                                 FMG_GroupName = b.FMG_GroupName,
                                                 FMH_FeeName = c.FMH_FeeName,
                                                 FMI_Name = e.FTI_Name,
                                                 FMA_Amount = f.FMA_Amount,
                                                 FTDDE_Day = g.FTDDE_Day,
                                                 FTDDE_Month = g.FTDDE_Month,
                                                 FTDD_Day = h.FTDD_Day,
                                                 FTDD_Month = h.FTDD_Month,
                                                 FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag,
                                                 //FMA_DueDate = f.FMA_DueDate,
                                                 //FTDDE_DueDate=f.FMA_ECSDueDate
                                                 ////FTDDE_DueDate =Convert.ToDateTime(g.FTDDE_Day+"/"+ g.FTDDE_Month+"/"+ f.FMA_DueDate.Value.Year + "00:00:00"),
                                                 FMA_DueDate = (f.FMA_DueDate == null ? DateTime.Now : f.FMA_DueDate),
                                          
                                                 FTDDE_DueDate = f.FMA_ECSDueDate,
                                                 FMA_PartialRebateApplicableDate = f.FMA_PartialRebateApplicableDate

                                             }
          ).OrderByDescending(t => t.FMH_Id).ToArray();


                    data.fineslabdetails = (from a in _YearlyFeeGroupMappingContext.feeFS
                                            from b in _YearlyFeeGroupMappingContext.feeTFineSlabDMO
                                            from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                            where (a.FMFS_Id == b.FMFS_Id && b.FMA_Id == c.FMA_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FMG_Id == data.FMG_Id && c.FMCC_Id == data.FMCC_Id)
                                            select new FeeAmountEntryDTO
                                            {
                                                FMA_Id = c.FMA_Id,
                                                FTFS_Amount = b.FTFS_Amount,
                                                FMFS_FineType = a.FMFS_FineType,
                                                FMFS_ECSFlag = a.FMFS_ECSFlag,
                                                FTFS_FineType = b.FTFS_FineType,
                                                FMG_Id = c.FMG_Id,
                                                FMH_Id = c.FMH_Id,
                                                FTI_Id = c.FTI_Id,
                                                FMCC_Id = c.FMCC_Id,
                                                FMFS_FromDay = a.FMFS_FromDay,
                                                FMFS_ToDay = a.FMFS_ToDay,
                                                FMFS_Id = b.FMFS_Id,
                                                FMA_DueDate = c.FMA_DueDate,
                                                FTDDE_DueDate = c.FMA_ECSDueDate,
                                                FMA_PartialRebateApplicableDate = c.FMA_PartialRebateApplicableDate,


                                            }
                        ).ToArray();


                    data.fineslabdetailsecs = (from a in _YearlyFeeGroupMappingContext.feeFS
                                               from b in _YearlyFeeGroupMappingContext.feeTFineSlabECSDMO
                                               from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                               where (a.FMFS_Id == b.FMFS_Id && b.FMA_Id == c.FMA_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FMG_Id == data.FMG_Id && c.FMCC_Id == data.FMCC_Id)
                                               select new FeeAmountEntryDTO
                                               {
                                                   FMA_Id = c.FMA_Id,
                                                   FMFS_FineType = a.FMFS_FineType,
                                                   FTFSE_Amount = b.FTFSE_Amount,
                                                   FMFS_ECSFlag = a.FMFS_ECSFlag,
                                                   FTFSE_FineType = b.FTFSE_FineType,
                                                   FMG_Id = c.FMG_Id,
                                                   FMH_Id = c.FMH_Id,
                                                   FTI_Id = c.FTI_Id,
                                                   FMCC_Id = c.FMCC_Id,
                                                   FMFS_FromDay = a.FMFS_FromDay,
                                                   FMFS_ToDay = a.FMFS_ToDay,
                                                   FTFSE_Id = b.FTFSE_Id,
                                                   FMA_DueDate = c.FMA_DueDate,
                                                   FTDDE_DueDate = c.FMA_ECSDueDate
                                               }
                       ).ToArray();


                    if (data.allgroupheaddata.Length <= 0)
                    {
                        //var p2
                        data.allgroupheaddata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                                 from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                                 from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                 from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                                 from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                                 where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && d.FMI_Id == e.FMI_Id)
                                                 select new FeeAmountEntryDTO
                                                 {
                                                     FMH_Id = a.FMH_Id,
                                                     FTI_Id = e.FTI_Id,
                                                     FMG_GroupName = b.FMG_GroupName,
                                                     FMH_FeeName = c.FMH_FeeName,
                                                     FMI_Name = e.FTI_Name,
                                                     FMA_Amount = 0,
                                                     FMH_Order = c.FMH_Order,
                                                     FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag,
                                                  
                                                     FMA_DueDate = DateTime.Now,
                                                     FMA_PartialRebateApplicableDate = DateTime.Now,
                                                     FTDDE_DueDate = DateTime.Now

                                                 }
         ).OrderByDescending(t => t.FMH_Order).ToArray();

                    }
                }

                else if (data.selectiontype == "stfoth")
                {
                    List<FeeYearlygroupHeadMappingDMO> commamtflag = new List<FeeYearlygroupHeadMappingDMO>();
                    commamtflag = _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.FYGHM_ActiveFlag == "1" && t.FYGHM_Common_AmountFlag == "Y" && t.FMG_Id == data.FMG_Id && t.ASMAY_Id==data.ASMAY_Id).ToList();
                    data.commountamountflag = commamtflag.ToArray();

                    data.newlyupdatedrec = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                            from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                            from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                            from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                            from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                            where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && d.FMI_Id == e.FMI_Id)
                                            select new FeeAmountEntryDTO
                                            {
                                                FMH_Id = a.FMH_Id,
                                                FTI_Id = e.FTI_Id,
                                                FMG_GroupName = b.FMG_GroupName,
                                                FMH_FeeName = c.FMH_FeeName,
                                                FMI_Name = e.FTI_Name,
                                                FMA_Amount = 0,
                                                FMH_Order = c.FMH_Order,
                                                FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag,
                                                FMA_DueDate = DateTime.Now,
                                                FTDDE_DueDate = DateTime.Now
                                            }
     ).OrderByDescending(t => t.FMH_Order).ToArray();

                    //feeamtentry.allgroupheaddata var p1
                    data.allgroupheaddata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                             from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                             from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                             from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                             from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                             from f in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                                 // from g in _YearlyFeeGroupMappingContext.feeTDueDateECSDMO
                                             from h in _YearlyFeeGroupMappingContext.Fee_T_Due_Date_OthStaffs
                                             where (f.FMH_Id == c.FMH_Id && f.FMG_Id == b.FMG_Id && f.FTI_Id == e.FTI_Id && a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && d.FMI_Id == e.FMI_Id && f.FMAOST_Id == h.FMAOST_Id && f.FMAOST_OthStaffFlag == "S" && a.ASMAY_Id == f.ASMAY_Id)
                                             select new FeeAmountEntryDTO
                                             {
                                                 FMAOST_Id = f.FMAOST_Id,
                                                 FMH_Id = a.FMH_Id,
                                                 FTI_Id = e.FTI_Id,
                                                 FMG_GroupName = b.FMG_GroupName,
                                                 FMH_FeeName = c.FMH_FeeName,
                                                 FMI_Name = e.FTI_Name,
                                                 FMA_Amount = f.FMAOST_Amount,
                                                 FTDD_Day = h.FTDD_Day,
                                                 FTDD_Month = h.FTDD_Month,
                                                 FTDD_Year = Convert.ToInt32(h.FTDD_Year),
                                                 FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag,
                                                 FMA_DueDate = DateTime.Now,
                                                 FTDDE_DueDate = DateTime.Now
                                             }
          ).OrderByDescending(t => t.FMH_Id).ToArray();


                    data.fineslabdetails = (from a in _YearlyFeeGroupMappingContext.feeFS
                                            from b in _YearlyFeeGroupMappingContext.Fee_T_Fine_Slabs_OthStaffs
                                            from c in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                            where (a.FMFS_Id == b.FMFS_Id && b.FMAOST_Id == c.FMAOST_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FMG_Id == data.FMG_Id && c.FMAOST_OthStaffFlag == "S")
                                            select new FeeAmountEntryDTO
                                            {
                                                FMA_Id = c.FMAOST_Id,
                                                FTFS_Amount = b.FTFSOST_Amount,
                                                FMFS_FineType = a.FMFS_FineType,
                                                FMFS_ECSFlag = a.FMFS_ECSFlag,
                                                FTFS_FineType = b.FTFSOST_FineType,
                                                FMG_Id = c.FMG_Id,
                                                FMH_Id = c.FMH_Id,
                                                FTI_Id = c.FTI_Id,
                                                FMFS_FromDay = a.FMFS_FromDay,
                                                FMFS_ToDay = a.FMFS_ToDay,
                                                FMFS_Id = a.FMFS_Id
                                            }
                        ).ToArray();

                    if (data.allgroupheaddata.Length <= 0)
                    {
                        //var p2
                        data.allgroupheaddata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                                 from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                                 from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                 from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                                 from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                                 where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && d.FMI_Id == e.FMI_Id)
                                                 select new FeeAmountEntryDTO
                                                 {
                                                     FMH_Id = a.FMH_Id,
                                                     FTI_Id = e.FTI_Id,
                                                     FMG_GroupName = b.FMG_GroupName,
                                                     FMH_FeeName = c.FMH_FeeName,
                                                     FMI_Name = e.FTI_Name,
                                                     FMA_Amount = 0,
                                                     FMH_Order = c.FMH_Order,
                                                     FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag,
                                                     FMA_DueDate = DateTime.Now,
                                                     FTDDE_DueDate = DateTime.Now
                                                 }
         ).OrderByDescending(t => t.FMH_Order).ToArray();

                    }
                }

                else if (data.selectiontype == "stfothamt")
                {
                    List<FeeYearlygroupHeadMappingDMO> commamtflag = new List<FeeYearlygroupHeadMappingDMO>();
                    commamtflag = _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.FYGHM_ActiveFlag == "1" && t.FYGHM_Common_AmountFlag == "Y" && t.FMG_Id == data.FMG_Id && t.ASMAY_Id==data.ASMAY_Id).ToList();
                    data.commountamountflag = commamtflag.ToArray();

                    data.newlyupdatedrec = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                            from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                            from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                            from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                            from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                            where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && d.FMI_Id == e.FMI_Id)
                                            select new FeeAmountEntryDTO
                                            {
                                                FMH_Id = a.FMH_Id,
                                                FTI_Id = e.FTI_Id,
                                                FMG_GroupName = b.FMG_GroupName,
                                                FMH_FeeName = c.FMH_FeeName,
                                                FMI_Name = e.FTI_Name,
                                                FMA_Amount = 0,
                                                FMH_Order = c.FMH_Order,
                                                FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag
                                            }
     ).OrderByDescending(t => t.FMH_Order).ToArray();

                    //feeamtentry.allgroupheaddata var p1
                    data.allgroupheaddata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                             from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                             from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                             from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                             from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                             from f in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                                 // from g in _YearlyFeeGroupMappingContext.feeTDueDateECSDMO
                                             from h in _YearlyFeeGroupMappingContext.Fee_T_Due_Date_OthStaffs
                                             where (f.FMH_Id == c.FMH_Id && f.FMG_Id == b.FMG_Id && f.FTI_Id == e.FTI_Id && a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && d.FMI_Id == e.FMI_Id && f.FMAOST_Id == h.FMAOST_Id && f.FMAOST_OthStaffFlag == "O" && a.ASMAY_Id == f.ASMAY_Id)
                                             select new FeeAmountEntryDTO
                                             {
                                                 FMAOST_Id = f.FMAOST_Id,
                                                 FMH_Id = a.FMH_Id,
                                                 FTI_Id = e.FTI_Id,
                                                 FMG_GroupName = b.FMG_GroupName,
                                                 FMH_FeeName = c.FMH_FeeName,
                                                 FMI_Name = e.FTI_Name,
                                                 FMA_Amount = f.FMAOST_Amount,
                                                 FTDD_Day = h.FTDD_Day,
                                                 FTDD_Month = h.FTDD_Month,
                                                 FTDD_Year = Convert.ToInt32(h.FTDD_Year),
                                                 FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag
                                             }
          ).OrderByDescending(t => t.FMH_Id).ToArray();


                    data.fineslabdetails = (from a in _YearlyFeeGroupMappingContext.feeFS
                                            from b in _YearlyFeeGroupMappingContext.Fee_T_Fine_Slabs_OthStaffs
                                            from c in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                            where (a.FMFS_Id == b.FMFS_Id && b.FMAOST_Id == c.FMAOST_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FMG_Id == data.FMG_Id && c.FMAOST_OthStaffFlag == "O")
                                            select new FeeAmountEntryDTO
                                            {
                                                FMA_Id = c.FMAOST_Id,
                                                FTFS_Amount = b.FTFSOST_Amount,
                                                FMFS_FineType = a.FMFS_FineType,
                                                FMFS_ECSFlag = a.FMFS_ECSFlag,
                                                FTFS_FineType = b.FTFSOST_FineType,
                                                FMG_Id = c.FMG_Id,
                                                FMH_Id = c.FMH_Id,
                                                FTI_Id = c.FTI_Id,
                                                FMFS_FromDay = a.FMFS_FromDay,
                                                FMFS_ToDay = a.FMFS_ToDay,
                                                FMFS_Id = a.FMFS_Id
                                            }
                        ).ToArray();

                    if (data.allgroupheaddata.Length <= 0)
                    {
                        //var p2
                        data.allgroupheaddata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                                 from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                                 from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                 from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                                 from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                                 where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && d.FMI_Id == e.FMI_Id)
                                                 select new FeeAmountEntryDTO
                                                 {
                                                     FMH_Id = a.FMH_Id,
                                                     FTI_Id = e.FTI_Id,
                                                     FMG_GroupName = b.FMG_GroupName,
                                                     FMH_FeeName = c.FMH_FeeName,
                                                     FMI_Name = e.FTI_Name,
                                                     FMA_Amount = 0,
                                                     FMH_Order = c.FMH_Order,
                                                     FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag
                                                 }
         ).OrderByDescending(t => t.FMH_Order).ToArray();

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;

        }

        public FeeAmountEntryDTO getsearchdata(int id, FeeAmountEntryDTO org)
        {
            try
            {
                List<FeeAmountEntryDTO> lorg = new List<FeeAmountEntryDTO>();
                if (org.FMH_FeeName == "Group Name")
                {
                    org.alldata = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                   from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                   where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == org.MI_Id && a.ASMAY_Id == org.ASMAY_Id && b.FMG_GroupName.Contains(org.FMG_GroupName))
                                   select new FeeAmountEntryDTO
                                   {
                                       FMG_GroupName = b.FMG_GroupName,
                                       FMH_FeeName = c.FMH_FeeName,
                                       FTI_Name = d.FTI_Name,
                                       FMA_Amount = a.FMA_Amount
                                   }
      ).ToArray();

                }
                if (org.FMH_FeeName == "Head Name")
                {
                    org.alldata = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                   from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                   where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == org.MI_Id && a.ASMAY_Id == org.ASMAY_Id && c.FMH_FeeName.Contains(org.FMG_GroupName))
                                   select new FeeAmountEntryDTO
                                   {
                                       FMG_GroupName = b.FMG_GroupName,
                                       FMH_FeeName = c.FMH_FeeName,
                                       FTI_Name = d.FTI_Name,
                                       FMA_Amount = a.FMA_Amount
                                   }
      ).ToArray();

                }

                if (org.FMH_FeeName == "Installment")
                {
                    org.alldata = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                   from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                   where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == org.MI_Id && a.ASMAY_Id == org.ASMAY_Id && d.FTI_Name.Contains(org.FMG_GroupName))
                                   select new FeeAmountEntryDTO
                                   {
                                       FMG_GroupName = b.FMG_GroupName,
                                       FMH_FeeName = c.FMH_FeeName,
                                       FTI_Name = d.FTI_Name,
                                       FMA_Amount = a.FMA_Amount
                                   }
      ).ToArray();

                }

                if (org.FMH_FeeName == "All")
                {
                    org.alldata = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                   from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                   where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == org.MI_Id && a.ASMAY_Id == org.ASMAY_Id)
                                   select new FeeAmountEntryDTO
                                   {
                                       FMG_GroupName = b.FMG_GroupName,
                                       FMH_FeeName = c.FMH_FeeName,
                                       FTI_Name = d.FTI_Name,
                                       FMA_Amount = a.FMA_Amount
                                   }
       ).ToArray();

                }

                // org.thirdgriddata = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }

        public FeeAmountEntryDTO savestaffdata(FeeAmountEntryDTO pgmod)
        {
            try
            {
                if (pgmod.savetmpdata[0].FMAOST_Id > 0)
                {

                    if (pgmod.savetmpdata != null)
                    {

                        int j = 0;

                        while (j < pgmod.savetmpdata.Count())
                        {
                            Fee_Master_Amount_OthStaffs pmm = new Fee_Master_Amount_OthStaffs();
                            if (pgmod.savetmpdata[j].FTDD_Month == null)
                            {
                                pgmod.savetmpdata[j].FTDD_Month = "0";
                            }

                            if (pgmod.savetmpdata[j].FTDDE_Month == null)
                            {
                                pgmod.savetmpdata[j].FTDDE_Month = "0";
                            }
                            if (pgmod.savetmpdata[j].FTDD_Day == null)
                            {
                                pgmod.savetmpdata[j].FTDD_Day = "0";
                            }
                            if (pgmod.savetmpdata[j].FTDDE_Day == null)
                            {
                                pgmod.savetmpdata[j].FTDDE_Day = "0";
                            }
                            if (pgmod.savetmpdata[j].FTDD_Year == 0)
                            {
                                pgmod.savetmpdata[j].FTDD_Year = 0;
                            }
                            if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                            {
                                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "Insert_Fee_Amount_Entry_Staff";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                        SqlDbType.BigInt)
                                    {
                                        Value = pgmod.MI_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                                       SqlDbType.BigInt)
                                    {
                                        Value = pgmod.FMG_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                   SqlDbType.BigInt)
                                    {
                                        Value = pgmod.ASMAY_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMH_Id",
                                SqlDbType.BigInt)
                                    {
                                        Value = pgmod.savetmpdata[j].FMH_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTI_Id",
                                      SqlDbType.BigInt)
                                    {
                                        Value = pgmod.savetmpdata[j].FTI_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMAOST_Amount",
                                      SqlDbType.Decimal)
                                    {
                                        Value = pgmod.savetmpdata[j].FMA_Amount
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMAOST_OthStaffFlag",
                                   SqlDbType.VarChar)
                                    {
                                        Value = "S"
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Month",
                                   SqlDbType.VarChar)
                                    {
                                        Value = pgmod.savetmpdata[j].FTDD_Month
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Day",
                                  SqlDbType.VarChar)
                                    {
                                        Value = pgmod.savetmpdata[j].FTDD_Day
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Year",
                                  SqlDbType.Int)
                                    {
                                        Value = pgmod.savetmpdata[j].FTDD_Year
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMAOST_Id",
                                SqlDbType.BigInt)
                                    {
                                        Value = pgmod.savetmpdata[j].FMAOST_Id
                                    });


                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();

                                    var data = cmd.ExecuteNonQuery();

                                    if (data >= 1)
                                    {
                                        pgmod.returnval = "true";
                                    }
                                    else
                                    {
                                        pgmod.returnval = "false";
                                    }
                                }
                            }
                            j++;
                        }
                    }

                    pgmod.amtentrystatus = "Updated";
                    int r = 0;
                    while (r < pgmod.savefineslabreg.Count())
                    {
                        if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                        {
                            //_YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slab @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabreg[r].FMH_ID, pgmod.savefineslabreg[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabreg[r].FTFS_FineType, pgmod.savefineslabreg[r].FTFS_Amount, pgmod.savefineslabreg[r].FMFS_Id, pgmod.FMCC_Id);

                            _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slabs_OtherStaffs @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabreg[r].FMH_ID, pgmod.savefineslabreg[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabreg[r].FTFS_FineType, pgmod.savefineslabreg[r].FTFS_Amount, pgmod.savefineslabreg[r].FMFS_Id);
                        }
                        r++;
                    }

                    // r = 0;

                    //while (r < pgmod.savefineslabecs.Count())
                    //{
                    //    if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                    //    {
                    //        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slab_ecs @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabecs[r].FMH_ID, pgmod.savefineslabecs[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabecs[r].FTFSE_FineType, pgmod.savefineslabecs[r].FTFSE_Amount, pgmod.savefineslabecs[r].FMFS_Id, pgmod.FMCC_Id);
                    //    }
                    //    r++;
                    //}
                }
                else
                {
                    if (pgmod.savetmpdata != null)
                    {

                        int j = 0;

                        while (j < pgmod.savetmpdata.Count())
                        {
                            Fee_Master_Amount_OthStaffs pmm = new Fee_Master_Amount_OthStaffs();
                            if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                            {
                                pmm.FMH_Id = pgmod.savetmpdata[j].FMH_Id;
                                pmm.FTI_Id = pgmod.savetmpdata[j].FTI_Id;
                                pmm.FMAOST_Amount = pgmod.savetmpdata[j].FMA_Amount;
                                pmm.MI_Id = pgmod.MI_Id;
                                pmm.ASMAY_Id = pgmod.ASMAY_Id;
                                pmm.FMG_Id = pgmod.FMG_Id;
                                if (pgmod.savetmpdata[j].FTDD_Month == null)
                                {
                                    pgmod.savetmpdata[j].FTDD_Month = "0";
                                }

                                if (pgmod.savetmpdata[j].FTDDE_Month == null)
                                {
                                    pgmod.savetmpdata[j].FTDDE_Month = "0";
                                }
                                if (pgmod.savetmpdata[j].FTDD_Day == null)
                                {
                                    pgmod.savetmpdata[j].FTDD_Day = "0";
                                }
                                if (pgmod.savetmpdata[j].FTDDE_Day == null)
                                {
                                    pgmod.savetmpdata[j].FTDDE_Day = "0";
                                }
                                if (pgmod.savetmpdata[j].FTDD_Year == 0)
                                {
                                    pgmod.savetmpdata[j].FTDD_Year = 0;
                                }

                                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "Insert_Fee_Amount_Entry_Staff";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                        SqlDbType.BigInt)
                                    {
                                        Value = pgmod.MI_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                                       SqlDbType.BigInt)
                                    {
                                        Value = pgmod.FMG_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                   SqlDbType.BigInt)
                                    {
                                        Value = pgmod.ASMAY_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMH_Id",
                                SqlDbType.BigInt)
                                    {
                                        Value = pmm.FMH_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTI_Id",
                                      SqlDbType.BigInt)
                                    {
                                        Value = pmm.FTI_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMAOST_Amount",
                                      SqlDbType.Decimal)
                                    {
                                        Value = pmm.FMAOST_Amount
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMAOST_OthStaffFlag",
                                   SqlDbType.VarChar)
                                    {
                                        Value = "S"
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Month",
                                   SqlDbType.VarChar)
                                    {
                                        Value = pgmod.savetmpdata[j].FTDD_Month
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Day",
                                  SqlDbType.VarChar)
                                    {
                                        Value = pgmod.savetmpdata[j].FTDD_Day
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Year",
                                  SqlDbType.Int)
                                    {
                                        Value = pgmod.savetmpdata[j].FTDD_Year
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMAOST_Id",
                                SqlDbType.BigInt)
                                    {
                                        Value = 0
                                    });


                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();
                                    var data = cmd.ExecuteNonQuery();

                                    if (data >= 1)
                                    {
                                        pgmod.returnval = "true";
                                        pgmod.amtentrystatus = "Saved";
                                    }
                                    else
                                    {
                                        pgmod.returnval = "false";
                                    }
                                }
                            }
                            j++;
                        }
                    }

                    int r = 0;
                    while (r < pgmod.savefineslabreg.Count())
                    {
                        if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                        {
                            _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slabs_OtherStaffs @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabreg[r].FMH_ID, pgmod.savefineslabreg[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabreg[r].FTFS_FineType, pgmod.savefineslabreg[r].FTFS_Amount, pgmod.savefineslabreg[r].FMFS_Id);
                        }
                        r++;
                    }

                    //  r = 0;

                    //while (r < pgmod.savefineslabecs.Count())
                    //{
                    //    if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                    //    {
                    //        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slab_ecs @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabecs[r].FMH_ID, pgmod.savefineslabecs[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabecs[r].FTFSE_FineType, pgmod.savefineslabecs[r].FTFSE_Amount, pgmod.savefineslabecs[r].FMFS_Id, pgmod.FMCC_Id);
                    //    }
                    //    r++;
                    //}

                }

                pgmod.alldata = (from a in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                 from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                 from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                 where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id)
                                 select new FeeAmountEntryDTO
                                 {
                                     FMG_GroupName = b.FMG_GroupName,
                                     FMH_FeeName = c.FMH_FeeName,
                                     FTI_Name = d.FTI_Name,
                                     FMA_Amount = a.FMAOST_Amount
                                 }
       ).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return pgmod;
        }

        public FeeAmountEntryDTO saveothersdata(FeeAmountEntryDTO pgmod)
        {
            try
            {
                if (pgmod.savetmpdata[0].FMAOST_Id > 0)
                {
                    if (pgmod.savetmpdata != null)
                    {
                        int j = 0;

                        while (j < pgmod.savetmpdata.Count())
                        {
                            Fee_Master_Amount_OthStaffs pmm = new Fee_Master_Amount_OthStaffs();
                            if (pgmod.savetmpdata[j].FTDD_Month == null)
                            {
                                pgmod.savetmpdata[j].FTDD_Month = "0";
                            }

                            if (pgmod.savetmpdata[j].FTDDE_Month == null)
                            {
                                pgmod.savetmpdata[j].FTDDE_Month = "0";
                            }
                            if (pgmod.savetmpdata[j].FTDD_Day == null)
                            {
                                pgmod.savetmpdata[j].FTDD_Day = "0";
                            }
                            if (pgmod.savetmpdata[j].FTDDE_Day == null)
                            {
                                pgmod.savetmpdata[j].FTDDE_Day = "0";
                            }
                            if (pgmod.savetmpdata[j].FTDD_Year == 0)
                            {
                                pgmod.savetmpdata[j].FTDD_Year = 0;
                            }
                            if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                            {
                                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "Insert_Fee_Amount_Entry_Staff";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                        SqlDbType.BigInt)
                                    {
                                        Value = pgmod.MI_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                                       SqlDbType.BigInt)
                                    {
                                        Value = pgmod.FMG_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                   SqlDbType.BigInt)
                                    {
                                        Value = pgmod.ASMAY_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMH_Id",
                                SqlDbType.BigInt)
                                    {
                                        Value = pgmod.savetmpdata[j].FMH_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTI_Id",
                                      SqlDbType.BigInt)
                                    {
                                        Value = pgmod.savetmpdata[j].FTI_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMAOST_Amount",
                                      SqlDbType.Decimal)
                                    {
                                        Value = pgmod.savetmpdata[j].FMA_Amount
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMAOST_OthStaffFlag",
                                   SqlDbType.VarChar)
                                    {
                                        Value = "O"
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Month",
                                   SqlDbType.VarChar)
                                    {
                                        Value = pgmod.savetmpdata[j].FTDD_Month
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Day",
                                  SqlDbType.VarChar)
                                    {
                                        Value = pgmod.savetmpdata[j].FTDD_Day
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Year",
                                  SqlDbType.Int)
                                    {
                                        Value = pgmod.savetmpdata[j].FTDD_Year
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMAOST_Id",
                                SqlDbType.BigInt)
                                    {
                                        Value = pgmod.savetmpdata[j].FMAOST_Id
                                    });


                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();

                                    var data = cmd.ExecuteNonQuery();

                                    if (data >= 1)
                                    {
                                        pgmod.returnval = "true";
                                    }
                                    else
                                    {
                                        pgmod.returnval = "false";
                                    }
                                }
                            }
                            j++;
                        }
                    }

                    pgmod.amtentrystatus = "Updated";
                    int r = 0;
                    while (r < pgmod.savefineslabreg.Count())
                    {
                        if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                        {
                            //_YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slab @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabreg[r].FMH_ID, pgmod.savefineslabreg[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabreg[r].FTFS_FineType, pgmod.savefineslabreg[r].FTFS_Amount, pgmod.savefineslabreg[r].FMFS_Id, pgmod.FMCC_Id);

                            _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slabs_OtherStaffs @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabreg[r].FMH_ID, pgmod.savefineslabreg[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabreg[r].FTFS_FineType, pgmod.savefineslabreg[r].FTFS_Amount, pgmod.savefineslabreg[r].FMFS_Id);
                        }
                        r++;
                    }

                    // r = 0;

                    //while (r < pgmod.savefineslabecs.Count())
                    //{
                    //    if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                    //    {
                    //        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slab_ecs @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabecs[r].FMH_ID, pgmod.savefineslabecs[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabecs[r].FTFSE_FineType, pgmod.savefineslabecs[r].FTFSE_Amount, pgmod.savefineslabecs[r].FMFS_Id, pgmod.FMCC_Id);
                    //    }
                    //    r++;
                    //}
                }
                else
                {
                    if (pgmod.savetmpdata != null)
                    {

                        int j = 0;

                        while (j < pgmod.savetmpdata.Count())
                        {
                            Fee_Master_Amount_OthStaffs pmm = new Fee_Master_Amount_OthStaffs();
                            if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                            {
                                pmm.FMH_Id = pgmod.savetmpdata[j].FMH_Id;
                                pmm.FTI_Id = pgmod.savetmpdata[j].FTI_Id;
                                pmm.FMAOST_Amount = pgmod.savetmpdata[j].FMA_Amount;
                                pmm.MI_Id = pgmod.MI_Id;
                                pmm.ASMAY_Id = pgmod.ASMAY_Id;
                                pmm.FMG_Id = pgmod.FMG_Id;
                                if (pgmod.savetmpdata[j].FTDD_Month == null)
                                {
                                    pgmod.savetmpdata[j].FTDD_Month = "0";
                                }

                                if (pgmod.savetmpdata[j].FTDDE_Month == null)
                                {
                                    pgmod.savetmpdata[j].FTDDE_Month = "0";
                                }
                                if (pgmod.savetmpdata[j].FTDD_Day == null)
                                {
                                    pgmod.savetmpdata[j].FTDD_Day = "0";
                                }
                                if (pgmod.savetmpdata[j].FTDDE_Day == null)
                                {
                                    pgmod.savetmpdata[j].FTDDE_Day = "0";
                                }
                                if (pgmod.savetmpdata[j].FTDD_Year == 0)
                                {
                                    pgmod.savetmpdata[j].FTDD_Year = 0;
                                }

                                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "Insert_Fee_Amount_Entry_Staff";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                        SqlDbType.BigInt)
                                    {
                                        Value = pgmod.MI_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                                       SqlDbType.BigInt)
                                    {
                                        Value = pgmod.FMG_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                   SqlDbType.BigInt)
                                    {
                                        Value = pgmod.ASMAY_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMH_Id",
                                SqlDbType.BigInt)
                                    {
                                        Value = pmm.FMH_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTI_Id",
                                      SqlDbType.BigInt)
                                    {
                                        Value = pmm.FTI_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMAOST_Amount",
                                      SqlDbType.Decimal)
                                    {
                                        Value = pmm.FMAOST_Amount
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMAOST_OthStaffFlag",
                                   SqlDbType.VarChar)
                                    {
                                        Value = "O"
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Month",
                                   SqlDbType.VarChar)
                                    {
                                        Value = pgmod.savetmpdata[j].FTDD_Month
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Day",
                                  SqlDbType.VarChar)
                                    {
                                        Value = pgmod.savetmpdata[j].FTDD_Day
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Year",
                                  SqlDbType.Int)
                                    {
                                        Value = pgmod.savetmpdata[j].FTDD_Year
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMAOST_Id",
                                SqlDbType.BigInt)
                                    {
                                        Value = 0
                                    });


                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();
                                    var data = cmd.ExecuteNonQuery();

                                    if (data >= 1)
                                    {
                                        pgmod.returnval = "true";
                                        pgmod.amtentrystatus = "Saved";
                                    }
                                    else
                                    {
                                        pgmod.returnval = "false";
                                    }
                                }
                            }
                            j++;
                        }
                    }

                    int r = 0;
                    while (r < pgmod.savefineslabreg.Count())
                    {
                        if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                        {
                            _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slabs_OtherStaffs @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabreg[r].FMH_ID, pgmod.savefineslabreg[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabreg[r].FTFS_FineType, pgmod.savefineslabreg[r].FTFS_Amount, pgmod.savefineslabreg[r].FMFS_Id);
                        }
                        r++;
                    }

                    //  r = 0;

                    //while (r < pgmod.savefineslabecs.Count())
                    //{
                    //    if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                    //    {
                    //        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slab_ecs @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabecs[r].FMH_ID, pgmod.savefineslabecs[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabecs[r].FTFSE_FineType, pgmod.savefineslabecs[r].FTFSE_Amount, pgmod.savefineslabecs[r].FMFS_Id, pgmod.FMCC_Id);
                    //    }
                    //    r++;
                    //}

                }

                pgmod.alldata = (from a in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                 from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                 from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                 where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id)
                                 select new FeeAmountEntryDTO
                                 {
                                     FMG_GroupName = b.FMG_GroupName,
                                     FMH_FeeName = c.FMH_FeeName,
                                     FTI_Name = d.FTI_Name,
                                     FMA_Amount = a.FMAOST_Amount
                                 }
       ).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return pgmod;
        }

        public FeeAmountEntryDTO savedetails(FeeAmountEntryDTO pgmod)
        {
            FeeAmountEntryDTO someObj = new FeeAmountEntryDTO();
            try
            {
                FeeAmountEntryDTO pgmodule = Mapper.Map<FeeAmountEntryDTO>(pgmod);

                if (pgmod.selectiontype == "stfoth")
                {
                    return savestaffdata(pgmod);
                }
                if (pgmod.selectiontype == "stfothamt")
                {
                    return saveothersdata(pgmod);
                }
                else
                {
                    if (pgmod.savetmpdata[0].FMA_Id > 0)
                    {

                        var a = "";

                        if (pgmod.savetmpdata != null)
                        {
                            int j = 0;

                            while (j < pgmod.savetmpdata.Count())
                            {
                                FeeAmountEntryDMO pmm = new FeeAmountEntryDMO();
                                if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                                {
                                    pmm.FMH_Id = pgmod.savetmpdata[j].FMH_Id;
                                    pmm.FTI_Id = pgmod.savetmpdata[j].FTI_Id;
                                    pmm.FMA_Amount = pgmod.savetmpdata[j].FMA_Amount;

                                    pmm.MI_Id = pgmod.MI_Id;
                                    pmm.ASMAY_Id = pgmod.ASMAY_Id;
                                    pmm.FMG_Id = pgmod.FMG_Id;
                                    pmm.FMCC_Id = pgmod.FMCC_Id;
                                    pmm.FMA_Flag = "1";

                                    using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd.CommandText = "Insert_Fee_Amount_Entry";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                            SqlDbType.BigInt)
                                        {
                                            Value = pmm.MI_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                                           SqlDbType.BigInt)
                                        {
                                            Value = pmm.FMG_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                       SqlDbType.BigInt)
                                        {
                                            Value = pmm.ASMAY_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FMCC_Id",
                                    SqlDbType.BigInt)
                                        {
                                            Value = pmm.FMCC_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FTI_Id",
                                          SqlDbType.BigInt)
                                        {
                                            Value = pmm.FTI_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FMA_Amount",
                                          SqlDbType.Decimal)
                                        {
                                            Value = pmm.FMA_Amount
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FMA_Flag",
                                       SqlDbType.BigInt)
                                        {
                                            Value = pmm.FMA_Flag
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FMH_Id",
                                       SqlDbType.BigInt)
                                        {
                                            Value = pmm.FMH_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FTDD_Month",
                                      SqlDbType.BigInt)
                                        {
                                            Value = pgmod.savetmpdata[j].FMA_DueDate.Value.Month
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FTDD_Day",
                                      SqlDbType.BigInt)
                                        {
                                            Value = pgmod.savetmpdata[j].FMA_DueDate.Value.Day
                                        });
                                        string month;
                                        string day;
                                        if (pgmod.savetmpdata[j].FTDDE_DueDate == null)
                                        {
                                            month = "0";
                                        }
                                        else
                                        {
                                            month = pgmod.savetmpdata[j].FTDDE_DueDate.Value.Month.ToString();
                                        }

                                        cmd.Parameters.Add(new SqlParameter("@FTDDE_Month",
                                      SqlDbType.BigInt)
                                        {
                                            Value = month
                                        });

                                        if (pgmod.savetmpdata[j].FTDDE_DueDate == null)
                                        {
                                           day = "0";
                                        }
                                        else
                                        {
                                            day = pgmod.savetmpdata[j].FTDDE_DueDate.Value.Day.ToString();
                                        }

                                        cmd.Parameters.Add(new SqlParameter("@FTDDE_Day",
                                      SqlDbType.BigInt)
                                        {
                                            Value = day
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FMA_ID",
                                     SqlDbType.BigInt)
                                        {
                                            Value = pgmod.savetmpdata[j].FMA_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@User_id",
                              SqlDbType.BigInt)
                                        {
                                            Value = pgmod.user_id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@DueDate",
                                    SqlDbType.DateTime)
                                        {
                                            Value = pgmod.savetmpdata[j].FMA_DueDate
                                        });
                                        if(pgmod.savetmpdata[j].FTDDE_DueDate!=null)
                                        {
                                            cmd.Parameters.Add(new SqlParameter("@ECSDueDate",
                             SqlDbType.DateTime)
                                            {
                                                Value = pgmod.savetmpdata[j].FTDDE_DueDate
                                            });
                                        }
                                        else
                                        {
                                            cmd.Parameters.Add(new SqlParameter("@ECSDueDate",
                              SqlDbType.DateTime)
                                            {
                                                Value = ""
                                            });
                                        }
                                        if (pgmod.savetmpdata[j].FMA_PartialRebateApplicableDate != null)
                                        {
                                            cmd.Parameters.Add(new SqlParameter("@FMA_PartialRebateApplicableDate",
                             SqlDbType.DateTime)
                                            {
                                                Value = pgmod.savetmpdata[j].FMA_PartialRebateApplicableDate
                                            });
                                        }
                                        else
                                        {
                                            cmd.Parameters.Add(new SqlParameter("@FMA_PartialRebateApplicableDate",
                              SqlDbType.DateTime)
                                            {
                                                Value = ""
                                            });
                                        }


                                        if (cmd.Connection.State != ConnectionState.Open)
                                            cmd.Connection.Open();

                                        var data = cmd.ExecuteNonQuery();

                                        if (data >= 1)
                                        {
                                            pgmod.returnval = "true";
                                            a = "true";
                                        }
                                        else
                                        {
                                            pgmod.returnval = "false";
                                        }
                                    }
                                }
                                //}

                                j++;
                            }
                        }
                        if (a == "true")
                        {
                            pgmod.returnval = "true";
                        }

                        pgmod.amtentrystatus = "Updated";
                        int r = 0;
                        while (r < pgmod.savefineslabreg.Count())
                        {
                            if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                            {
                                _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slab @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabreg[r].FMH_ID, pgmod.savefineslabreg[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabreg[r].FTFS_FineType, pgmod.savefineslabreg[r].FTFS_Amount, pgmod.savefineslabreg[r].FMFS_Id, pgmod.FMCC_Id);
                            }
                            r++;
                        }

                        r = 0;

                        while (r < pgmod.savefineslabecs.Count())
                        {
                            if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                            {
                                _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slab_ecs @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabecs[r].FMH_ID, pgmod.savefineslabecs[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabecs[r].FTFSE_FineType, pgmod.savefineslabecs[r].FTFSE_Amount, pgmod.savefineslabecs[r].FTFSE_Id, pgmod.FMCC_Id);
                            }
                            r++;
                        }
                    }
                    else
                    {
                        if (pgmod.savetmpdata != null)
                        {

                            int j = 0;

                            while (j < pgmod.savetmpdata.Count())
                            {
                                FeeAmountEntryDMO pmm = new FeeAmountEntryDMO();
                                if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                                {
                                    pmm.FMH_Id = pgmod.savetmpdata[j].FMH_Id;
                                    pmm.FTI_Id = pgmod.savetmpdata[j].FTI_Id;
                                    pmm.FMA_Amount = pgmod.savetmpdata[j].FMA_Amount;

                                    pmm.MI_Id = pgmod.MI_Id;
                                    pmm.ASMAY_Id = pgmod.ASMAY_Id;
                                    pmm.FMG_Id = pgmod.FMG_Id;
                                    pmm.FMCC_Id = pgmod.FMCC_Id;
                                    pmm.FMA_Flag = "0";

                                    if (pgmod.savetmpdata[j].FTDD_Month == null)
                                    {
                                        pgmod.savetmpdata[j].FTDD_Month = "0";
                                    }

                                    if (pgmod.savetmpdata[j].FTDDE_Month == null)
                                    {
                                        pgmod.savetmpdata[j].FTDDE_Month = "0";
                                    }
                                    if (pgmod.savetmpdata[j].FTDD_Day == null)
                                    {
                                        pgmod.savetmpdata[j].FTDD_Day = "0";
                                    }
                                    if (pgmod.savetmpdata[j].FTDDE_Day == null)
                                    {
                                        pgmod.savetmpdata[j].FTDDE_Day = "0";
                                    }

                                    using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd.CommandText = "Insert_Fee_Amount_Entry";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                            SqlDbType.BigInt)
                                        {
                                            Value = pmm.MI_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                                           SqlDbType.BigInt)
                                        {
                                            Value = pmm.FMG_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                       SqlDbType.BigInt)
                                        {
                                            Value = pmm.ASMAY_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FMCC_Id",
                                    SqlDbType.BigInt)
                                        {
                                            Value = pmm.FMCC_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FTI_Id",
                                          SqlDbType.BigInt)
                                        {
                                            Value = pmm.FTI_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FMA_Amount",
                                          SqlDbType.Decimal)
                                        {
                                            Value = pmm.FMA_Amount
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FMA_Flag",
                                       SqlDbType.BigInt)
                                        {
                                            Value = pmm.FMA_Flag
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FMH_Id",
                                       SqlDbType.BigInt)
                                        {
                                            Value = pmm.FMH_Id
                                        });

                                        if (pgmod.savetmpdata[j].FTDD_Month != null)
                                        {
                                            cmd.Parameters.Add(new SqlParameter("@FTDD_Month",
                                     SqlDbType.BigInt)
                                            {
                                                Value = pgmod.savetmpdata[j].FMA_DueDate.Value.Month
                                            });
                                        }

                                        if (pgmod.savetmpdata[j].FTDD_Day != null)
                                        {
                                            cmd.Parameters.Add(new SqlParameter("@FTDD_Day",
                                    SqlDbType.BigInt)
                                            {
                                                Value = pgmod.savetmpdata[j].FMA_DueDate.Value.Day
                                            });
                                        }
                                        string month;
                                        string day;
                                        if (pgmod.savetmpdata[j].FTDDE_Month == null)
                                        {
                                            month = "0";
                                        }
                                        else
                                        {
                                            month = pgmod.savetmpdata[j].FTDDE_DueDate.Value.Month.ToString();
                                        }

                                        cmd.Parameters.Add(new SqlParameter("@FTDDE_Month",
                                      SqlDbType.BigInt)
                                        {
                                            Value = month
                                        });

                                        if (pgmod.savetmpdata[j].FTDDE_DueDate == null)
                                        {
                                            day = "0";
                                        }
                                        else
                                        {
                                            day = pgmod.savetmpdata[j].FTDDE_DueDate.Value.Day.ToString();
                                        }

                                        cmd.Parameters.Add(new SqlParameter("@FTDDE_Day",
                                      SqlDbType.BigInt)
                                        {
                                            Value = day
                                        });


                                        cmd.Parameters.Add(new SqlParameter("@FMA_ID",
                                SqlDbType.BigInt)
                                        {
                                            Value = "0"
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@User_id",
                               SqlDbType.BigInt)
                                        {
                                            Value = pgmod.user_id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@DueDate",
                                    SqlDbType.DateTime)
                                        {
                                            Value = pgmod.savetmpdata[j].FMA_DueDate
                                        });
                                        if (pgmod.savetmpdata[j].FTDDE_DueDate != null)
                                        {
                                            cmd.Parameters.Add(new SqlParameter("@ECSDueDate",
                             SqlDbType.DateTime)
                                            {
                                                Value = pgmod.savetmpdata[j].FTDDE_DueDate
                                            });
                                        }
                                        else
                                        {
                                            cmd.Parameters.Add(new SqlParameter("@ECSDueDate",
                              SqlDbType.DateTime)
                                            {
                                                Value = ""
                                            });
                                        }
                                        if (pgmod.savetmpdata[j].FMA_PartialRebateApplicableDate != null)
                                        {
                                            cmd.Parameters.Add(new SqlParameter("@FMA_PartialRebateApplicableDate",
                             SqlDbType.DateTime)
                                            {
                                                Value = pgmod.savetmpdata[j].FMA_PartialRebateApplicableDate
                                            });
                                        }
                                        else
                                        {
                                            cmd.Parameters.Add(new SqlParameter("@FMA_PartialRebateApplicableDate",
                              SqlDbType.DateTime)
                                            {
                                                Value = ""
                                            });
                                        }
                                        if (cmd.Connection.State != ConnectionState.Open)
                                            cmd.Connection.Open();
                                        var data = cmd.ExecuteNonQuery();

                                        if (data >= 1)
                                        {
                                            pgmod.returnval = "true";
                                            pgmod.amtentrystatus = "Saved";
                                        }
                                        else
                                        {
                                            pgmod.returnval = "false";
                                        }
                                    }
                                }
                                j++;
                            }
                        }

                        int r = 0;
                        while (r < pgmod.savefineslabreg.Count())
                        {
                            if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                            {
                                _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slab @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabreg[r].FMH_ID, pgmod.savefineslabreg[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabreg[r].FTFS_FineType, pgmod.savefineslabreg[r].FTFS_Amount, pgmod.savefineslabreg[r].FMFS_Id, pgmod.FMCC_Id);
                            }
                            r++;
                        }

                        r = 0;

                        while (r < pgmod.savefineslabecs.Count())
                        {
                            if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                            {
                                _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slab_ecs @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabecs[r].FMH_ID, pgmod.savefineslabecs[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabecs[r].FTFSE_FineType, pgmod.savefineslabecs[r].FTFSE_Amount, pgmod.savefineslabecs[r].FMFS_Id, pgmod.FMCC_Id);
                            }
                            r++;
                        }

                    }

                    pgmod.alldata = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                     from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                     from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                     from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                     where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id)
                                     select new FeeAmountEntryDTO
                                     {
                                         FMG_GroupName = b.FMG_GroupName,
                                         FMH_FeeName = c.FMH_FeeName,
                                         FTI_Name = d.FTI_Name,
                                         FMA_Amount = a.FMA_Amount
                                     }
           ).ToArray();

                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
                pgmod.returnval = "false";
            }

            return pgmod;
        }

        public FeeAmountEntryDTO paymentdetailsfnc(FeeAmountEntryDTO fee)
        {
            try
            {
                if (fee.regularfalg == "R")
                {
                    List<FeeFineSlabDMO> head = new List<FeeFineSlabDMO>();
                    head = _YearlyFeeGroupMappingContext.feeFS.Where(t => t.FMFS_ECSFlag == fee.regularfalg && t.MI_Id == fee.MI_Id && t.FMFS_ActiveFlag == true).ToList();
                    fee.fillslab = head.ToArray();
                }
                if (fee.ecsflag == "E")
                {
                    List<FeeFineSlabDMO> head = new List<FeeFineSlabDMO>();
                    head = _YearlyFeeGroupMappingContext.feeFS.Where(t => t.FMFS_ECSFlag == fee.ecsflag && t.MI_Id == fee.MI_Id && t.FMFS_ActiveFlag == true).ToList();
                    fee.fillslabecs = head.ToArray();
                }

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return fee;

        }

        public FeeAmountEntryDTO selectacade(FeeAmountEntryDTO data)
        {
            try
            {
                data.fillcategory = (from a in _YearlyFeeGroupMappingContext.feeYCC
                                     from b in _YearlyFeeGroupMappingContext.AcademicYear
                                     from c in _YearlyFeeGroupMappingContext.FeeClassCategoryDMO
                                     where (a.ASMAY_Id == b.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id && a.FYCC_ActiveFlag == true && a.FMCC_Id == c.FMCC_Id)
                                     select new FeeAmountEntryDTO
                                     {
                                         FMCC_Id = c.FMCC_Id,
                                         FMCC_ClassCategoryName = c.FMCC_ClassCategoryName,
                                     }
             ).ToArray();


                //    data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupDMO
                //                         from b in _YearlyFeeGroupMappingContext.Yearlygroups
                //                         where (b.ASMAY_Id == data.ASMAY_Id && a.FMG_ActiceFlag==true && a.FMG_Id==b.FMG_Id)
                //                         select new FeeAmountEntryDTO
                //                         {
                //                             FMG_Id = a.FMG_Id,
                //                             FMG_GroupName = a.FMG_GroupName,
                //                         }
                //).ToArray();

                data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                        from b in _YearlyFeeGroupMappingContext.Yearlygroups
                                        from c in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                        where (b.ASMAY_Id == data.ASMAY_Id && a.FMG_ActiceFlag == true && a.FMG_Id == b.FMG_Id && b.FMG_Id == c.FMG_Id && c.FYGHM_ActiveFlag == "1")
                                        select new FeeAmountEntryDTO
                                        {
                                            FMG_Id = a.FMG_Id,
                                            FMG_GroupName = a.FMG_GroupName,
                                        }
           ).Distinct().ToArray();

                data.alldata = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                from e in _YearlyFeeGroupMappingContext.FeeClassCategoryDMO
                                from f in _YearlyFeeGroupMappingContext.AcademicYear
                                where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == data.MI_Id && e.FMCC_Id == a.FMCC_Id && f.ASMAY_Id == a.ASMAY_Id && f.ASMAY_Id == data.ASMAY_Id)
                                select new FeeAmountEntryDTO
                                {
                                    FMG_GroupName = b.FMG_GroupName,
                                    FMH_FeeName = c.FMH_FeeName,
                                    FTI_Name = d.FTI_Name,
                                    FMA_Amount = a.FMA_Amount,
                                    FMA_Id = a.FMA_Id,
                                    FMCC_ClassCategoryName = e.FMCC_ClassCategoryName,
                                    ASMAY_Year = f.ASMAY_Year
                                }
           ).OrderBy(t => t.FMH_FeeName).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeAmountEntryDTO getalldetailsOnselectiontype(FeeAmountEntryDTO data)
        {
            try
            {
                if (data.selectiontype.Equals("stfoth"))
                {
                    data.alldata = (from a in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                    from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                    from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                    from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                    where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMAOST_OthStaffFlag == "S")
                                    select new FeeAmountEntryDTO
                                    {
                                        FMG_GroupName = b.FMG_GroupName,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FMA_Amount = a.FMAOST_Amount,
                                        FMA_Id = a.FMAOST_Id
                                    }
              ).OrderBy(t => t.FMH_FeeName).ToArray();
                }
                else if (data.selectiontype.Equals("stfothamt"))
                {
                    data.alldata = (from a in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                    from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                    from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                    from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                    where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMAOST_OthStaffFlag == "O")
                                    select new FeeAmountEntryDTO
                                    {
                                        FMG_GroupName = b.FMG_GroupName,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FMA_Amount = a.FMAOST_Amount,
                                        FMA_Id = a.FMAOST_Id
                                    }
              ).OrderBy(t => t.FMH_FeeName).ToArray();
                }
                else
                {
                    data.alldata = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                    from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                    from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                    from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                    where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                    select new FeeAmountEntryDTO
                                    {
                                        FMG_GroupName = b.FMG_GroupName,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FMA_Amount = a.FMA_Amount,
                                        FMA_Id = a.FMA_Id
                                    }
              ).OrderBy(t => t.FMH_FeeName).ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}