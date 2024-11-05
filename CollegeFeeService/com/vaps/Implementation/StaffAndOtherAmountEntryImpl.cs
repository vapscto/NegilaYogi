using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;

using DomainModel.Model.com.vaps.Fee;
using System.Dynamic;
using Microsoft.Extensions.Logging;
using DomainModel.Model.com.vapstech.Fee;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using PreadmissionDTOs.com.vaps.College.Fees;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vapstech.College.Fees;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class StaffAndOtherAmountEntryImpl : Interfaces.StaffAndOtherAmountEntryInterface
    {
        private static ConcurrentDictionary<string, CLGFeeAmountEntryDTO> _login =
    new ConcurrentDictionary<string, CLGFeeAmountEntryDTO>();

        public CollFeeGroupContext _YearlyFeeGroupMappingContext;
        readonly ILogger<CLGFeeAmountEntryImpl> _logger;
        public StaffAndOtherAmountEntryImpl(CollFeeGroupContext YearlyFeeGroupMappingContext, ILogger<CLGFeeAmountEntryImpl> log)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _logger = log;
        }
        public FeeAmountEntryDTO deleterec(FeeAmountEntryDTO page)
        {
            try
            {
         
                 if (page.selectiontype.Equals("stfoth") || page.selectiontype.Equals("stfothamt"))
                {
                    //using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                    //{
                      // FeeAmountEntryDTO page = new FeeAmountEntryDTO();
                    //    List<Fee_Master_College_Amount_OthStaffs> lorg = new List<Fee_Master_College_Amount_OthStaffs>();
                      //  List<Fee_T_Due_Date_CollegeOthStaffs> lorgduedate = new List<Fee_T_Due_Date_CollegeOthStaffs>();
                      //  List<Fee_T_Fine_Slabs_CollegeOthStaffs> fineslab = new List<Fee_T_Fine_Slabs_CollegeOthStaffs>();
                       // List<FeeStudentTransactionDMO> feestutrans = new List<FeeStudentTransactionDMO>();

                        var lorg = _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs.Where(t => t.FMCAOST_Id == page.FMA_Id).ToList();

                        var lorgduedate = _YearlyFeeGroupMappingContext.Fee_T_Due_Date_CollegeOthStaffs.Where(t => t.FMCAOST_Id == page.FMA_Id).ToList();

                        var fineslab = _YearlyFeeGroupMappingContext.Fee_T_Fine_Slabs_CollegeOthStaffs.Where(t => t.FMCAOST_Id == page.FMA_Id).ToList();

                  // feestutrans = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Where(t => t.FCSS_PaidAmount > 0 && t.MI_Id == page.MI_Id && t.ASMAY_Id == page.ASMAY_Id).ToList();

                        try
                        {
                           /// if (feestutrans.Count == 0)
                           // {
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
                                          //  transaction.Commit();
                                        }
                                        else
                                        {
                                            page.returnval = "false";
                                        }
                                    }
                                }
                           // }
                            //else
                            //{
                            //    page.returnval = "RecordExists";
                           // }

                            if (page.selectiontype.Equals("stfoth"))
                            {
                                page.alldata = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                                from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                                from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                                where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == page.MI_Id && a.ASMAY_Id == page.ASMAY_Id && a.FMCAOST_OthStaffFlag == "S")
                                                select new FeeAmountEntryDTO
                                                {
                                                    FMG_GroupName = b.FMG_GroupName,
                                                    FMH_FeeName = c.FMH_FeeName,
                                                    FTI_Name = d.FTI_Name,
                                                    FMA_Amount = a.FMCAOST_Amount
                                                }
                       ).ToArray();
                            }

                            if (page.selectiontype.Equals("stfothamt"))
                            {
                                page.alldata = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                                from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                                from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                                where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == page.MI_Id && a.ASMAY_Id == page.ASMAY_Id && a.FMCAOST_OthStaffFlag == "O")
                                                select new FeeAmountEntryDTO
                                                {
                                                    FMG_GroupName = b.FMG_GroupName,
                                                    FMH_FeeName = c.FMH_FeeName,
                                                    FTI_Name = d.FTI_Name,
                                                    FMA_Amount = a.FMCAOST_Amount
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

                  //  }
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
                data.alldata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
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

                // List<MasterAcademic> allyear = new List<MasterAcademic>();
                var allyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == fee.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                fee.academicdrp = allyear.Distinct().ToArray();

                //List<FeeHeadDMO> head = new List<FeeHeadDMO>();
                var head = _YearlyFeeGroupMappingContext.FeeHeadClgDMO.Where(t => t.MI_Id == fee.MI_Id && t.FMH_ActiveFlag == true).ToList();
                fee.fillmasterhead = head.ToArray();

                //List<FeeGroupDMO> group = new List<FeeGroupDMO>();
                var group = _YearlyFeeGroupMappingContext.FeeGroupClgDMO.Where(t => t.MI_Id == fee.MI_Id && t.FMG_ActiceFlag == true).ToList();
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


                fee.fillinstallment = (from a in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                       from b in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                       where (a.FMI_Id == b.FMI_Id && a.MI_Id == fee.MI_Id && a.FMI_ActiceFlag == true)
                                       select new FeeAmountEntryDTO
                                       {
                                           FTI_Id = b.FTI_Id,
                                           FTI_Name = b.FTI_Name,
                                       }
       ).ToArray();


                fee.alldata = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == fee.MI_Id && a.FMCAOST_OthStaffFlag == "S")
                                select new FeeAmountEntryDTO
                                {
                                    FMG_GroupName = b.FMG_GroupName,
                                    FMH_FeeName = c.FMH_FeeName,
                                    FTI_Name = d.FTI_Name,
                                    FMA_Amount = a.FMCAOST_Amount,
                                    FMA_Id = a.FMCAOST_Id
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
         

                 if (data.selectiontype == "stfoth")
                {
          
                    var commamtflag = _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping.Where(t => t.MI_Id == data.MI_Id && t.FYGHM_ActiveFlag == "1" && t.FYGHM_Common_AmountFlag == "Y" && t.FMG_Id == data.FMG_Id).ToList();
                    data.commountamountflag = commamtflag.ToArray();

                    data.newlyupdatedrec = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                            from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                            from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                            from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                            from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
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
                    data.allgroupheaddata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                             from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                             from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                             from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                             from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                             from f in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                                 // from g in _YearlyFeeGroupMappingContext.feeTDueDateECSDMO
                                             from h in _YearlyFeeGroupMappingContext.Fee_T_Due_Date_CollegeOthStaffs
                                             where (f.FMH_Id == c.FMH_Id && f.FMG_Id == b.FMG_Id && f.FTI_Id == e.FTI_Id && a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && d.FMI_Id == e.FMI_Id && f.FMCAOST_Id == h.FMCAOST_Id && f.FMCAOST_OthStaffFlag == "S" && a.ASMAY_Id == f.ASMAY_Id)
                                             select new FeeAmountEntryDTO
                                             {
                                                 FMAOST_Id = f.FMCAOST_Id,
                                                 FMH_Id = a.FMH_Id,
                                                 FTI_Id = e.FTI_Id,
                                                 FMG_GroupName = b.FMG_GroupName,
                                                 FMH_FeeName = c.FMH_FeeName,
                                                 FMI_Name = e.FTI_Name,
                                                 FMA_Amount = f.FMCAOST_Amount,
                                                 FTDD_Day = h.FCTDDOST_Day,
                                                 FTDD_Month = h.FCTDDOST_Month,
                                                 FTDD_Year = Convert.ToInt32(h.FCTDDOST_Year),
                                                 FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag
                                             }
          ).OrderByDescending(t => t.FMH_Id).ToArray();


                    data.fineslabdetails = (from a in _YearlyFeeGroupMappingContext.feeFS
                                            from b in _YearlyFeeGroupMappingContext.Fee_T_Fine_Slabs_CollegeOthStaffs
                                            from c in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                            where (a.FMFS_Id == b.FMFS_Id && b.FMCAOST_Id == c.FMCAOST_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FMG_Id == data.FMG_Id && c.FMCAOST_OthStaffFlag == "S")
                                            select new FeeAmountEntryDTO
                                            {
                                                FMA_Id = c.FMCAOST_Id,
                                                FTFS_Amount = b.FCTFSOST_Amount,
                                                FMFS_FineType = a.FMFS_FineType,
                                                FMFS_ECSFlag = a.FMFS_ECSFlag,
                                                FTFS_FineType = b.FCTFSOST_FineType,
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
                        data.allgroupheaddata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                                 from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                                 from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                 from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                                 from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
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

                else if (data.selectiontype == "stfothamt")
                {
                    // List<CLG_Fee_Yearly_Group_Head_Mapping> commamtflag = new List<CLG_Fee_Yearly_Group_Head_Mapping>();
                    var commamtflag = _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping.Where(t => t.MI_Id == data.MI_Id && t.FYGHM_ActiveFlag == "1" && t.FYGHM_Common_AmountFlag == "Y" && t.FMG_Id == data.FMG_Id).ToList();
                    data.commountamountflag = commamtflag.ToArray();

                    data.newlyupdatedrec = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                            from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                            from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                            from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                            from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
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
                    data.allgroupheaddata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                             from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                             from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                             from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                             from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                             from f in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                                 // from g in _YearlyFeeGroupMappingContext.feeTDueDateECSDMO
                                             from h in _YearlyFeeGroupMappingContext.Fee_T_Due_Date_CollegeOthStaffs
                                             where (f.FMH_Id == c.FMH_Id && f.FMG_Id == b.FMG_Id && f.FTI_Id == e.FTI_Id && a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && d.FMI_Id == e.FMI_Id && f.FMCAOST_Id == h.FMCAOST_Id && f.FMCAOST_OthStaffFlag == "O" && a.ASMAY_Id == f.ASMAY_Id)
                                             select new FeeAmountEntryDTO
                                             {
                                                 FMAOST_Id = f.FMCAOST_Id,
                                                 FMH_Id = a.FMH_Id,
                                                 FTI_Id = e.FTI_Id,
                                                 FMG_GroupName = b.FMG_GroupName,
                                                 FMH_FeeName = c.FMH_FeeName,
                                                 FMI_Name = e.FTI_Name,
                                                 FMA_Amount = f.FMCAOST_Amount,
                                                 FTDD_Day = h.FCTDDOST_Day,
                                                 FTDD_Month = h.FCTDDOST_Month,
                                                 FTDD_Year = Convert.ToInt32(h.FCTDDOST_Year),
                                                 FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag
                                             }
          ).OrderByDescending(t => t.FMH_Id).ToArray();


                    data.fineslabdetails = (from a in _YearlyFeeGroupMappingContext.feeFS
                                            from b in _YearlyFeeGroupMappingContext.Fee_T_Fine_Slabs_CollegeOthStaffs
                                            from c in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                            where (a.FMFS_Id == b.FMFS_Id && b.FMCAOST_Id == c.FMCAOST_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FMG_Id == data.FMG_Id && c.FMCAOST_OthStaffFlag == "O")
                                            select new FeeAmountEntryDTO
                                            {
                                                FMA_Id = c.FMCAOST_Id,
                                                FTFS_Amount = b.FCTFSOST_Amount,
                                                FMFS_FineType = a.FMFS_FineType,
                                                FMFS_ECSFlag = a.FMFS_ECSFlag,
                                                FTFS_FineType = b.FCTFSOST_FineType,
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
                        data.allgroupheaddata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                                 from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                                 from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                 from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                                 from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
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
                                   from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                   from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
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
                                   from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                   from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
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
                                   from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                   from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
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
                                   from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                   from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
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
                            Fee_Master_College_Amount_OthStaffs pmm = new Fee_Master_College_Amount_OthStaffs();
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
                                    cmd.CommandText = "Insert_Fee_Amount_Entry_StaffCollege";
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
                                        // Value = pgmod.savetmpdata[j].FTDD_Month
                                       Value = pgmod.savetmpdata[j].Duedate.Month
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Day",
                                  SqlDbType.VarChar)
                                    {
                                        Value = pgmod.savetmpdata[j].Duedate.Day
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Year",
                                  SqlDbType.Int)
                                    {
                                        Value = pgmod.savetmpdata[j].Duedate.Year
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMAOST_Id",
                                SqlDbType.BigInt)
                                    {
                                        Value = pgmod.savetmpdata[j].FMAOST_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@USER_Id",
                              SqlDbType.BigInt)
                                    {
                                        Value = pgmod.user_id
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
                            _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slabs_OtherStaffsCollege @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabreg[r].FMH_ID, pgmod.savefineslabreg[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabreg[r].FTFS_FineType, pgmod.savefineslabreg[r].FTFS_Amount, pgmod.savefineslabreg[r].FMFS_Id,pgmod.user_id);
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
                            Fee_Master_College_Amount_OthStaffs pmm = new Fee_Master_College_Amount_OthStaffs();
                            if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                            {
                                pmm.FMH_Id = pgmod.savetmpdata[j].FMH_Id;
                                pmm.FTI_Id = pgmod.savetmpdata[j].FTI_Id;
                                pmm.FMCAOST_Amount = pgmod.savetmpdata[j].FMA_Amount;
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
                                    cmd.CommandText = "Insert_Fee_Amount_Entry_StaffCollege";
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
                                        Value = pmm.FMCAOST_Amount
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMAOST_OthStaffFlag",
                                   SqlDbType.VarChar)
                                    {
                                        Value = "S"
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Month",
                                   SqlDbType.VarChar)
                                    {
                                        Value = pgmod.savetmpdata[j].Duedate.Month
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Day",
                                  SqlDbType.VarChar)
                                    {
                                        Value = pgmod.savetmpdata[j].Duedate.Day
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Year",
                                  SqlDbType.Int)
                                    {
                                        Value = pgmod.savetmpdata[j].Duedate.Year
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMAOST_Id",
                                SqlDbType.BigInt)
                                    {
                                        Value = 0
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@USER_Id",
                                SqlDbType.BigInt)
                                    {
                                        Value = pgmod.user_id
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
                            _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slabs_OtherStaffsCollege @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabreg[r].FMH_ID, pgmod.savefineslabreg[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabreg[r].FTFS_FineType, pgmod.savefineslabreg[r].FTFS_Amount, pgmod.savefineslabreg[r].FMFS_Id, pgmod.user_id);
                        }
                        r++;
                    }


                }

                pgmod.alldata = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                 from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                 from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                 where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id)
                                 select new FeeAmountEntryDTO
                                 {
                                     FMG_GroupName = b.FMG_GroupName,
                                     FMH_FeeName = c.FMH_FeeName,
                                     FTI_Name = d.FTI_Name,
                                     FMA_Amount = a.FMCAOST_Amount
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
                            Fee_Master_College_Amount_OthStaffs pmm = new Fee_Master_College_Amount_OthStaffs();
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
                                    cmd.CommandText = "Insert_Fee_Amount_Entry_StaffCollege";
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
                                        Value = pgmod.savetmpdata[j].Duedate.Month
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Day",
                                  SqlDbType.VarChar)
                                    {
                                        Value = pgmod.savetmpdata[j].Duedate.Day
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Year",
                                  SqlDbType.Int)
                                    {
                                        Value = pgmod.savetmpdata[j].Duedate.Year
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FMAOST_Id",
                              SqlDbType.BigInt)
                                    {
                                        Value = pgmod.savetmpdata[j].FMAOST_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@USER_Id",
                                SqlDbType.BigInt)
                                    {
                                        Value = pgmod.user_id
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
                            _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slabs_OtherStaffsCollege @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabreg[r].FMH_ID, pgmod.savefineslabreg[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabreg[r].FTFS_FineType, pgmod.savefineslabreg[r].FTFS_Amount, pgmod.savefineslabreg[r].FMFS_Id, pgmod.user_id);
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
                            Fee_Master_College_Amount_OthStaffs pmm = new Fee_Master_College_Amount_OthStaffs();
                            if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                            {
                                pmm.FMH_Id = pgmod.savetmpdata[j].FMH_Id;
                                pmm.FTI_Id = pgmod.savetmpdata[j].FTI_Id;
                                pmm.FMCAOST_Amount = pgmod.savetmpdata[j].FMA_Amount;
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
                                    cmd.CommandText = "Insert_Fee_Amount_Entry_StaffCollege";
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
                                        Value = pmm.FMCAOST_Amount
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMAOST_OthStaffFlag",
                                   SqlDbType.VarChar)
                                    {
                                        Value = "O"
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Month",
                                   SqlDbType.VarChar)
                                    {
                                        Value = pgmod.savetmpdata[j].Duedate.Month
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Day",
                                  SqlDbType.VarChar)
                                    {
                                        Value = pgmod.savetmpdata[j].Duedate.Day
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Year",
                                  SqlDbType.Int)
                                    {
                                        Value = pgmod.savetmpdata[j].Duedate.Year
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FMAOST_Id",
                                SqlDbType.BigInt)
                                    {
                                        Value = 0
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@USER_Id",
                              SqlDbType.BigInt)
                                    {
                                        Value = pgmod.user_id
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
                            _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_T_Fine_Slabs_OtherStaffsCollege @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabreg[r].FMH_ID, pgmod.savefineslabreg[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabreg[r].FTFS_FineType, pgmod.savefineslabreg[r].FTFS_Amount, pgmod.savefineslabreg[r].FMFS_Id, pgmod.user_id);
                        }
                        r++;
                    }


                }

                pgmod.alldata = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                 from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                 from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                 where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id)
                                 select new FeeAmountEntryDTO
                                 {
                                     FMG_GroupName = b.FMG_GroupName,
                                     FMH_FeeName = c.FMH_FeeName,
                                     FTI_Name = d.FTI_Name,
                                     FMA_Amount = a.FMCAOST_Amount
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
       

                data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                        from b in _YearlyFeeGroupMappingContext.FeeYearGroupDMO
                                        from c in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                        where (b.ASMAY_Id == data.ASMAY_Id && a.FMG_ActiceFlag == true && a.FMG_Id == b.FMG_Id && b.FMG_Id == c.FMG_Id && c.FYGHM_ActiveFlag == "1")
                                        select new FeeAmountEntryDTO
                                        {
                                            FMG_Id = a.FMG_Id,
                                            FMG_GroupName = a.FMG_GroupName,
                                        }
           ).Distinct().ToArray();

                data.alldata = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
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
                    data.alldata = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                    from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                    from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                    from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                    where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == data.MI_Id  && a.FMCAOST_OthStaffFlag == "S")
                                    select new FeeAmountEntryDTO
                                    {
                                        FMG_GroupName = b.FMG_GroupName,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FMA_Amount = a.FMCAOST_Amount,
                                        FMA_Id = a.FMCAOST_Id
                                    }
              ).OrderBy(t => t.FMH_FeeName).ToArray();
                }
                else if (data.selectiontype.Equals("stfothamt"))
                {
                    data.alldata = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                    from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                    from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                    from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                    where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == data.MI_Id  && a.FMCAOST_OthStaffFlag == "O")
                                    select new FeeAmountEntryDTO
                                    {
                                        FMG_GroupName = b.FMG_GroupName,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FMA_Amount = a.FMCAOST_Amount,
                                        FMA_Id = a.FMCAOST_Id
                                    }
              ).OrderBy(t => t.FMH_FeeName).ToArray();
                }
                else
                {
                    data.alldata = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                    from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                    from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                    from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
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
