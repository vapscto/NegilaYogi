using System;
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
    public class FeeAmountEntryStthomasImpl : interfaces.FeeAmountEntryStthomasInterface
    {

        private static ConcurrentDictionary<string, FeeAmountEntryStthomasDTO> _login =
            new ConcurrentDictionary<string, FeeAmountEntryStthomasDTO>();

        public FeeGroupContext _YearlyFeeGroupMappingContext;
        readonly ILogger<FeeAmountEntryImpl> _logger;
        public FeeAmountEntryStthomasImpl(FeeGroupContext YearlyFeeGroupMappingContext)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;

        }

        public FeeAmountEntryStthomasDTO deleterec(FeeAmountEntryStthomasDTO page)
        {
            try
            {

                using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                {
                    // FeeAmountEntryStthomasDTO page = new FeeAmountEntryStthomasDTO();
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
                                        select new FeeAmountEntryStthomasDTO
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return page;
        }

        public FeeAmountEntryStthomasDTO EditMasterscetionDetails(FeeAmountEntryStthomasDTO data)
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

        public FeeAmountEntryStthomasDTO getdata(FeeAmountEntryStthomasDTO fee)
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
                group = _YearlyFeeGroupMappingContext.FeeGroupDMO.Where(t => t.MI_Id == fee.MI_Id && t.FMG_ActiceFlag == true).OrderBy(t => t.FMG_Order).ToList();
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
                                       select new FeeAmountEntryStthomasDTO
                                       {
                                           FTI_Id = b.FTI_Id,
                                           FTI_Name = b.FTI_Name,
                                       }
       ).ToArray();


                fee.fillcategory = (from a in _YearlyFeeGroupMappingContext.FeeClassCategoryDMO
                                    from b in _YearlyFeeGroupMappingContext.feeYCC
                                    from c in _YearlyFeeGroupMappingContext.feeYCCC
                                    where (a.FMCC_Id == b.FMCC_Id && a.MI_Id == fee.MI_Id && b.ASMAY_Id == fee.ASMAY_Id && b.FYCC_Id == c.FYCC_Id && a.FMCC_ActiveFlag == true && b.FYCC_ActiveFlag == true)
                                    select new FeeAmountEntryStthomasDTO
                                    {
                                        FMCC_Id = b.FMCC_Id,
                                        FMCC_ClassCategoryName = a.FMCC_ClassCategoryName,
                                    }
              ).Distinct().ToArray();


                fee.alldata = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                               from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                               from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                               from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                               where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == fee.MI_Id)
                               select new FeeAmountEntryStthomasDTO
                               {
                                   FMG_GroupName = b.FMG_GroupName,
                                   FMH_FeeName = c.FMH_FeeName,
                                   FTI_Name = d.FTI_Name,
                                   FMA_Amount = a.FMA_Amount,
                                   FMA_Id = a.FMA_Id
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

        public FeeAmountEntryStthomasDTO getgroupheaddetails(FeeAmountEntryStthomasDTO data)
        {
            try
            {

                List<FeeYearlygroupHeadMappingDMO> commamtflag = new List<FeeYearlygroupHeadMappingDMO>();
                commamtflag = _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.FYGHM_ActiveFlag == "1" && t.FYGHM_Common_AmountFlag == "Y" && t.FMG_Id == data.FMG_Id && t.ASMAY_Id == data.ASMAY_Id).ToList();
                data.commountamountflag = commamtflag.ToArray();


                data.newlyupdatedrec = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                        from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                        from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                        from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                        from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                        where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && d.FMI_Id == e.FMI_Id)
                                        select new FeeAmountEntryStthomasDTO
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
                                         select new FeeAmountEntryStthomasDTO
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
                                        where (a.FMFS_Id == b.FMFS_Id && b.FMA_Id == c.FMA_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FMG_Id == data.FMG_Id && c.FMCC_Id == data.FMCC_Id && c.FTI_Id == data.FTI_Id)
                                        select new FeeAmountEntryStthomasDTO
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
                                            FTFS_Date = b.FTFS_Date,
                                            FTFS_Id = b.FTFS_Id


                                        }
                    ).ToArray();


                data.fineslabdetailsecs = (from a in _YearlyFeeGroupMappingContext.feeFS
                                           from b in _YearlyFeeGroupMappingContext.feeTFineSlabECSDMO
                                           from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                           where (a.FMFS_Id == b.FMFS_Id && b.FMA_Id == c.FMA_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FMG_Id == data.FMG_Id && c.FMCC_Id == data.FMCC_Id)
                                           select new FeeAmountEntryStthomasDTO
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
                                             select new FeeAmountEntryStthomasDTO
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


                var ftilist = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                               from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                               from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                               from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                               from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                               where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && d.FMI_Id == e.FMI_Id)
                               select (e.FTI_Id)

     ).ToList();


                var FineApplicableFlag = _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO.Where(t => t.FMG_Id == data.FMG_Id && t.FYGHM_FineApplicableFlag == "Y" && t.ASMAY_Id == data.ASMAY_Id).ToList();


                if (FineApplicableFlag.Count > 0)
                {
                    data.instllmentdetails = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                              from b in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                              from c in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                              where (a.FMH_Id == b.FMH_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == c.FMI_Id && a.FMG_Id == data.FMG_Id && a.FYGHM_FineApplicableFlag == "Y")
                                              select new FeeAmountEntryStthomasDTO
                                              {
                                                  FTI_Id = c.FTI_Id,
                                                  FMH_Id = b.FMH_Id,
                                                  FTI_Name = c.FTI_Name

                                              }

                        ).ToArray();
                }
                else
                {
                    data.instllmentdetails = _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO.Where(t => t.MI_ID == data.MI_Id && ftilist.Contains(t.FTI_Id)).ToArray();
                }


                // data.instllmentdetails = _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO.Where(t => t.MI_ID == data.MI_Id && ftilist.Contains(t.FTI_Id)).ToArray();
                //data.headdetails = _YearlyFeeGroupMappingContext.FeeHeadDMO.Where(t => t.MI_Id == data.MI_Id && FMHlist.Contains(t.FMH_Id)).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;

        }

        public FeeAmountEntryStthomasDTO getsearchdata(int id, FeeAmountEntryStthomasDTO org)
        {
            try
            {
                List<FeeAmountEntryStthomasDTO> lorg = new List<FeeAmountEntryStthomasDTO>();
                if (org.FMH_FeeName == "Group Name")
                {
                    org.alldata = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                   from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                   where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == org.MI_Id && a.ASMAY_Id == org.ASMAY_Id && b.FMG_GroupName.Contains(org.FMG_GroupName))
                                   select new FeeAmountEntryStthomasDTO
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
                                   select new FeeAmountEntryStthomasDTO
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
                                   select new FeeAmountEntryStthomasDTO
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
                                   select new FeeAmountEntryStthomasDTO
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
            }
            return org;
        }



        public FeeAmountEntryStthomasDTO savedetails(FeeAmountEntryStthomasDTO pgmod)
        {
            FeeAmountEntryStthomasDTO someObj = new FeeAmountEntryStthomasDTO();
            try
            {
                FeeAmountEntryStthomasDTO pgmodule = Mapper.Map<FeeAmountEntryStthomasDTO>(pgmod);


                if (pgmod.savetmpdata[0].FMA_Id > 0)
                {

                    var a = "";

                    if (pgmod.savetmpdata != null)
                    {
                        int K = 0;
                        // while (j < pgmod.savetmpdata.Count())
                        foreach (var j in pgmod.savetmpdata)
                        {
                            string FTDDE_DueDate = "";
                            if (j.FTDDE_DueDate != null)
                            {
                                FTDDE_DueDate = j.FTDDE_DueDate.Value.ToString("yyyy-MM-dd");
                            }
                            FeeAmountEntryDMO pmm = new FeeAmountEntryDMO();
                            if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                            {
                                pmm.FMH_Id = j.FMH_Id;
                                pmm.FTI_Id = j.FTI_Id;
                                pmm.FMA_Amount = j.FMA_Amount;
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
                                        Value = j.FMA_DueDate.Value.Month
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Day",
                                  SqlDbType.BigInt)
                                    {
                                        Value = j.FMA_DueDate.Value.Day
                                    });
                                    string month;
                                    string day;
                                    if (j.FTDDE_DueDate == null)
                                    {
                                        month = "0";
                                    }
                                    else
                                    {
                                        month = j.FTDDE_DueDate.Value.Month.ToString();
                                    }

                                    cmd.Parameters.Add(new SqlParameter("@FTDDE_Month",
                                  SqlDbType.BigInt)
                                    {
                                        Value = month
                                    });

                                    if (j.FTDDE_DueDate == null)
                                    {
                                        day = "0";
                                    }
                                    else
                                    {
                                        day = j.FTDDE_DueDate.Value.Day.ToString();
                                    }

                                    cmd.Parameters.Add(new SqlParameter("@FTDDE_Day",
                                  SqlDbType.BigInt)
                                    {
                                        Value = day
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMA_ID",
                                 SqlDbType.BigInt)
                                    {
                                        Value = j.FMA_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@User_id",
                          SqlDbType.BigInt)
                                    {
                                        Value = pgmod.user_id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@DueDate",
                                SqlDbType.DateTime)
                                    {
                                        Value = FTDDE_DueDate
                                    });
                                    if (j.FTDDE_DueDate != null)
                                    {
                                        cmd.Parameters.Add(new SqlParameter("@ECSDueDate",
                         SqlDbType.DateTime)
                                        {
                                            Value = FTDDE_DueDate
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
                                    if (j.FMA_PartialRebateApplicableDate != null)
                                    {
                                        cmd.Parameters.Add(new SqlParameter("@FMA_PartialRebateApplicableDate",
                         SqlDbType.DateTime)
                                        {
                                            Value = j.FMA_PartialRebateApplicableDate
                                        });
                                    }
                                    else
                                    {
                                        cmd.Parameters.Add(new SqlParameter("@FMA_PartialRebateApplicableDate",
                          SqlDbType.DateTime)
                                        {
                                            Value = FTDDE_DueDate
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

                            K++;

                            //FeeAmountEntry
                            if (a == "true")
                            {
                                pgmod.returnval = "true";
                            }

                            pgmod.amtentrystatus = "Updated";
                            int r = 0;
                            var finesladid = _YearlyFeeGroupMappingContext.FeeFineSlabDMO.Where(t => t.MI_Id == pgmod.MI_Id && t.FMFS_FineType == "FixedDate").ToList();
                            if(j.savefineslabreg !=null && j.savefineslabreg.Length >0)
                            {
                                foreach (var t in j.savefineslabreg)
                                {

                                    _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_SlabStthoms @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10", pgmod.MI_Id, pgmod.ASMAY_Id, t.FMH_ID, t.FTI_ID, pgmod.FMG_Id, t.FTFS_FineType, t.FTFS_Amount, finesladid.FirstOrDefault().FMFS_Id, pgmod.FMCC_Id, t.FMFS_Duedate,t.FTFS_Id);

                                }

                            }

                            //while (r < pgmod.savefineslabreg.Count())
                            //{
                            //    if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                            //    {
                            //        var finesladid = _YearlyFeeGroupMappingContext.FeeFineSlabDMO.Where(t => t.MI_Id == pgmod.MI_Id && t.FMFS_FineType == "FixedDate").ToList();

                            //        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_SlabStthoms @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabreg[r].FMH_ID, pgmod.savefineslabreg[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabreg[r].FTFS_FineType, pgmod.savefineslabreg[r].FTFS_Amount, finesladid[0].FMFS_Id, pgmod.FMCC_Id, pgmod.savefineslabreg[r].FMFS_Duedate);

                            //    }
                            //    r++;
                            //}

                            r = 0;


                        }
                    }
                    //if (a == "true")
                    //{
                    //    pgmod.returnval = "true";
                    //}

                    //pgmod.amtentrystatus = "Updated";
                    //int r = 0;
                    //while (r < pgmod.savefineslabreg.Count())
                    //{
                    //    if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                    //    {
                    //        var finesladid = _YearlyFeeGroupMappingContext.FeeFineSlabDMO.Where(t => t.MI_Id == pgmod.MI_Id && t.FMFS_FineType == "FixedDate").ToList();

                    //        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_SlabStthoms @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabreg[r].FMH_ID, pgmod.savefineslabreg[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabreg[r].FTFS_FineType, pgmod.savefineslabreg[r].FTFS_Amount, finesladid[0].FMFS_Id, pgmod.FMCC_Id, pgmod.savefineslabreg[r].FMFS_Duedate);
                    //    }
                    //    r++;
                    //}

                    //r = 0;


                }
                else
                {
                    if (pgmod.savetmpdata != null)
                    {

                        int j = 0;
                        foreach (var K in pgmod.savetmpdata)
                        // while (j < pgmod.savetmpdata.Count())
                        {
                            FeeAmountEntryDMO pmm = new FeeAmountEntryDMO();
                            if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                            {
                                pmm.FMH_Id = K.FMH_Id;
                                pmm.FTI_Id = K.FTI_Id;
                                pmm.FMA_Amount = K.FMA_Amount;

                                pmm.MI_Id = pgmod.MI_Id;
                                pmm.ASMAY_Id = pgmod.ASMAY_Id;
                                pmm.FMG_Id = pgmod.FMG_Id;
                                pmm.FMCC_Id = pgmod.FMCC_Id;
                                pmm.FMA_Flag = "0";

                                if (K.FTDD_Month == null)
                                {
                                    K.FTDD_Month = "0";
                                }

                                if (K.FTDDE_Month == null)
                                {
                                    K.FTDDE_Month = "0";
                                }
                                if (K.FTDD_Day == null)
                                {
                                    K.FTDD_Day = "0";
                                }
                                if (K.FTDDE_Day == null)
                                {
                                    K.FTDDE_Day = "0";
                                }
                                 var FTDDE_DueDate = "";
                              //  DateTime FTDDE_DueDate = K.FTDDE_DueDate.Value.Date.ToString("yyyy-MM-dd");
                                if (K.FTDDE_DueDate !=null)
                                {
                                    FTDDE_DueDate = K.FTDDE_DueDate.Value.ToString("yyyy-MM-dd");
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

                                    if (K.FTDD_Month != null)
                                    {
                                        cmd.Parameters.Add(new SqlParameter("@FTDD_Month",
                                 SqlDbType.BigInt)
                                        {
                                            Value = K.FMA_DueDate.Value.Month
                                        });
                                    }

                                    if (K.FTDD_Day != null)
                                    {
                                        cmd.Parameters.Add(new SqlParameter("@FTDD_Day",
                                SqlDbType.BigInt)
                                        {
                                            Value = K.FMA_DueDate.Value.Day
                                        });
                                    }
                                    string month;
                                    string day;
                                    if (K.FTDDE_Month == null)
                                    {
                                        month = "0";
                                    }
                                    else
                                    {
                                        month = K.FTDDE_DueDate.Value.Month.ToString();
                                    }

                                    cmd.Parameters.Add(new SqlParameter("@FTDDE_Month",
                                  SqlDbType.BigInt)
                                    {
                                        Value = month
                                    });

                                    if (K.FTDDE_DueDate == null)
                                    {
                                        day = "0";
                                    }
                                    else
                                    {
                                        day = K.FTDDE_DueDate.Value.Day.ToString();
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
                                        Value = FTDDE_DueDate
                                    });
                                    if (K.FTDDE_DueDate != null)
                                    {
                                        cmd.Parameters.Add(new SqlParameter("@ECSDueDate",
                         SqlDbType.DateTime)
                                        {
                                            Value = FTDDE_DueDate
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
                                    if (K.FMA_PartialRebateApplicableDate != null)
                                    {
                                        cmd.Parameters.Add(new SqlParameter("@FMA_PartialRebateApplicableDate",
                         SqlDbType.DateTime)
                                        {
                                            Value = K.FMA_PartialRebateApplicableDate
                                        });
                                    }
                                    else
                                    {
                                        cmd.Parameters.Add(new SqlParameter("@FMA_PartialRebateApplicableDate",
                          SqlDbType.DateTime)
                                        {
                                            Value = FTDDE_DueDate
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
                                     var finesladid = _YearlyFeeGroupMappingContext.FeeFineSlabDMO.Where(t => t.MI_Id == pgmod.MI_Id && t.FMFS_FineType == "FixedDate").ToList();
                                if(K.savefineslabreg !=null && K.savefineslabreg.Length > 0)
                                {
                                    foreach (var t in K.savefineslabreg)
                                    {
                                        //_YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slab @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabreg[r].FMH_ID, pgmod.savefineslabreg[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabreg[r].FTFS_FineType, pgmod.savefineslabreg[r].FTFS_Amount, finesladid[0].FMFS_Id, pgmod.FMCC_Id);

                                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_SlabStthoms @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10", pgmod.MI_Id, pgmod.ASMAY_Id, t.FMH_ID, t.FTI_ID, pgmod.FMG_Id, t.FTFS_FineType, t.FTFS_Amount, finesladid.FirstOrDefault().FMFS_Id, pgmod.FMCC_Id, t.FMFS_Duedate,0);

                                    }

                                }


                            }
                            j++;
                        }
                    }

                    int r = 0;
                    //while (r < pgmod.savefineslabreg.Count())
                    //{
                    //    if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                    //    {


                    //        var finesladid = _YearlyFeeGroupMappingContext.FeeFineSlabDMO.Where(t => t.MI_Id == pgmod.MI_Id && t.FMFS_FineType == "FixedDate").ToList();


                    //        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slab @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabreg[r].FMH_ID, pgmod.savefineslabreg[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabreg[r].FTFS_FineType, pgmod.savefineslabreg[r].FTFS_Amount, finesladid[0].FMFS_Id, pgmod.FMCC_Id);
                    //    }
                    //    r++;
                    //}

                    r = 0;



                }

                pgmod.alldata = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                 from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                 from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                 where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id)
                                 select new FeeAmountEntryStthomasDTO
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
                pgmod.returnval = "false";
            }

            return pgmod;
        }

        public FeeAmountEntryStthomasDTO paymentdetailsfnc(FeeAmountEntryStthomasDTO fee)
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

        public FeeAmountEntryStthomasDTO selectacade(FeeAmountEntryStthomasDTO data)
        {
            try
            {
                data.fillcategory = (from a in _YearlyFeeGroupMappingContext.feeYCC
                                     from b in _YearlyFeeGroupMappingContext.AcademicYear
                                     from c in _YearlyFeeGroupMappingContext.FeeClassCategoryDMO
                                     where (a.ASMAY_Id == b.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id && a.FYCC_ActiveFlag == true && a.FMCC_Id == c.FMCC_Id)
                                     select new FeeAmountEntryStthomasDTO
                                     {
                                         FMCC_Id = c.FMCC_Id,
                                         FMCC_ClassCategoryName = c.FMCC_ClassCategoryName,
                                     }
             ).ToArray();



                data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                        from b in _YearlyFeeGroupMappingContext.Yearlygroups
                                        from c in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                        where (b.ASMAY_Id == data.ASMAY_Id && a.FMG_ActiceFlag == true && a.FMG_Id == b.FMG_Id && b.FMG_Id == c.FMG_Id && c.FYGHM_ActiveFlag == "1")
                                        select new FeeAmountEntryStthomasDTO
                                        {
                                            FMG_Id = a.FMG_Id,
                                            FMG_GroupName = a.FMG_GroupName,
                                            FMG_Order = a.FMG_Order,
                                        }
           ).OrderBy(t => t.FMG_Order).Distinct().ToArray();

                data.alldata = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                select new FeeAmountEntryStthomasDTO
                                {
                                    FMG_GroupName = b.FMG_GroupName,
                                    FMH_FeeName = c.FMH_FeeName,
                                    FTI_Name = d.FTI_Name,
                                    FMA_Amount = a.FMA_Amount,
                                    FMA_Id = a.FMA_Id
                                }
           ).OrderBy(t => t.FMH_FeeName).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeAmountEntryStthomasDTO getalldetailsOnselectiontype(FeeAmountEntryStthomasDTO data)
        {
            try
            {

                data.alldata = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                select new FeeAmountEntryStthomasDTO
                                {
                                    FMG_GroupName = b.FMG_GroupName,
                                    FMH_FeeName = c.FMH_FeeName,
                                    FTI_Name = d.FTI_Name,
                                    FMA_Amount = a.FMA_Amount,
                                    FMA_Id = a.FMA_Id
                                }
          ).OrderBy(t => t.FMH_FeeName).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
