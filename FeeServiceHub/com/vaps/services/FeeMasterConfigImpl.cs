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
    public class FeeMasterConfigImpl : interfaces.FeeMasterConfigInterface
    {


        private static ConcurrentDictionary<string, FeeMasterConfigurationDTO> _login =
       new ConcurrentDictionary<string, FeeMasterConfigurationDTO>();

        public FeeGroupContext _FeeGroupContext;
        readonly ILogger<FeeMasterConfigImpl> _logger;
        public FeeMasterConfigImpl(FeeGroupContext frgContext, ILogger<FeeMasterConfigImpl> log)
        {
            _logger = log;
            _FeeGroupContext = frgContext;
        }
        public FeeMasterConfigurationDTO getdetailsY(FeeMasterConfigurationDTO FGRDT)
        {
            try
            {
                if(FGRDT.rolenme=="Fee End User")
                {
                    List<FeeMasterConfigurationDMO> feeheads = new List<FeeMasterConfigurationDMO>();
                    feeheads = _FeeGroupContext.feemastersettings.Where(t => t.MI_Id == FGRDT.MI_Id && t.userid == FGRDT.userid).ToList();
                    FGRDT.masterdata = feeheads.ToArray();
                }
                else
                {
                    List<FeeMasterConfigurationDMO> feeheads = new List<FeeMasterConfigurationDMO>();
                    feeheads = _FeeGroupContext.feemastersettings.Where(t => t.MI_Id == FGRDT.MI_Id).ToList();
                    FGRDT.masterdata = feeheads.ToArray();
                }

                //  FGRDT.feeconfiglist = (from a in _FeeGroupContext.master_institution
                //                   from b in _FeeGroupContext.feemastersettings
                //                   from c in _FeeGroupContext.Staff_User_Login
                //                   from d in _FeeGroupContext.MasterEmployee
                //                    where (a.MI_Id==b.MI_Id && b.userid==c.Id && c.Emp_Code==d.HRME_Id && a.MI_Id== FGRDT.MI_Id)
                //                   select new FeeStudentTransactionDTO
                //                   {
                //                       institutionname=a.MI_Name,
                //                       HRME_EmployeeFirstName=d.HRME_EmployeeFirstName,
                //                       FMC_Id =b.FMC_Id
                //                   }
                //).ToArray();

                FGRDT.feeconfiglist = (from a in _FeeGroupContext.master_institution
                                       from b in _FeeGroupContext.feemastersettings
                                       from c in _FeeGroupContext.applicationUser
                                       where (a.MI_Id == b.MI_Id && b.userid == c.Id && c.Id == b.userid && a.MI_Id == FGRDT.MI_Id)
                                       select new FeeStudentTransactionDTO
                                       {
                                           institutionname = a.MI_Name,
                                           HRME_EmployeeFirstName = c.UserName,
                                           FMC_Id = b.FMC_Id
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


        public FeeMasterConfigurationDTO editdetails(FeeMasterConfigurationDTO FGRDT)
        {
            try
            {
                List<FeeMasterConfigurationDMO> feeheads = new List<FeeMasterConfigurationDMO>();
                feeheads = _FeeGroupContext.feemastersettings.Where(t => t.MI_Id == FGRDT.MI_Id && t.FMC_Id== FGRDT.FMC_Id).ToList();
                FGRDT.masterdata = feeheads.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGRDT;
        }

        public FeeMasterConfigurationDTO SaveconfigData(FeeMasterConfigurationDTO FGpage)
        {
            bool returnresult = false;
            FeeMasterConfigurationDMO feepge = Mapper.Map<FeeMasterConfigurationDMO>(FGpage);
           
            string retval = "";
            try
            {
                if (feepge.FMC_Id > 0)
                {
                    var result = _FeeGroupContext.feemastersettings.Single(t => t.FMC_Id == feepge.FMC_Id);
                    result.MI_Id = feepge.MI_Id;
                    result.FMC_GroupOrTermFlg = feepge.FMC_GroupOrTermFlg;
                    result.FMC_Areawise_FeeFlg =feepge.FMC_Areawise_FeeFlg;
                    result.FMC_TransportFeeAreaFlag = feepge.FMC_TransportFeeAreaFlag;
                    result.FMC_TransportFeeZoneFlag = feepge.FMC_TransportFeeZoneFlag;
                    result.FMC_DOACheckFlag = feepge.FMC_DOACheckFlag;
                    result.FMC_Default_Currency = feepge.FMC_Default_Currency;
                    result.FMC_ArrearColumn = feepge.FMC_ArrearColumn;
                    result.FMC_Fine_Column = feepge.FMC_Fine_Column;
                    result.FMC_ArrearLedgerFlag = feepge.FMC_ArrearLedgerFlag;
                    result.FMC_Fine_LedgerFlag = feepge.FMC_Fine_LedgerFlag;
                    result.FMC_ArrearAfterFlag = feepge.FMC_ArrearAfterFlag;
                    result.FMC_Receipt_Signatory = feepge.FMC_Receipt_Signatory;
                    result.FMC_No_Receipt = feepge.FMC_No_Receipt;
                    result.FMC_Receipt_SignatoryImage = feepge.FMC_Receipt_SignatoryImage;
                    result.FMC_ChallanOptionFlag = feepge.FMC_ChallanOptionFlag;
                    result.FMC_AutoReceiptFeeGroupFlag = feepge.FMC_AutoReceiptFeeGroupFlag;
                    result.FMC_GroupRemarksFlag = feepge.FMC_GroupRemarksFlag;
                    result.FMC_RInstallmentsFlag = feepge.FMC_RInstallmentsFlag;
                    result.FMC_RInstallmentsMergeFlag = feepge.FMC_RInstallmentsMergeFlag;
                    result.FMC_RFineFlag = feepge.FMC_RFineFlag;
                    result.FMC_RConcessionFlag = feepge.FMC_RConcessionFlag;
                    result.FMC_RWaivedFlag = feepge.FMC_RWaivedFlag;
                    result.FMC_RBalanceFlag = feepge.FMC_RBalanceFlag;
                    result.FMC_RAmountFlag = feepge.FMC_RAmountFlag;
                    result.FMC_RBankFlag = feepge.FMC_RBankFlag;
                    result.FMC_RDueDateFlag = feepge.FMC_RDueDateFlag;
                    result.FMC_RAddressFlag = feepge.FMC_RAddressFlag;
                    result.FMC_RPaperSizeFlag = feepge.FMC_RPaperSizeFlag;
                    result.FMC_RFeeGroupFeeHeadFlag = feepge.FMC_RFeeGroupFeeHeadFlag;
                    result.FMC_RSplFeeHeadFlag = feepge.FMC_RSplFeeHeadFlag;
                    result.FMC_RHeaderTitleFlag = feepge.FMC_RHeaderTitleFlag;
                    result.FMC_RClassFlag = feepge.FMC_RClassFlag;
                    result.FMC_RSectionFlag = feepge.FMC_RSectionFlag;
                    result.FMC_RUserNameFlag = feepge.FMC_RUserNameFlag;
                    result.FMC_RFatherNameFlag = feepge.FMC_RFatherNameFlag;
                    result.FMC_MotherNameFlag = feepge.FMC_MotherNameFlag;
                    result.FMC_RFeeHeaderFlag = feepge.FMC_RFeeHeaderFlag;
                    result.FMC_RPaymentDetailsFlag = feepge.FMC_RPaymentDetailsFlag;
                    result.FMC_RAmountReceivedFlag = feepge.FMC_RAmountReceivedFlag;
                    result.FMC_RRemarksFlag = feepge.FMC_RRemarksFlag;
                    result.FMC_RCurrentDateFlag = feepge.FMC_RCurrentDateFlag;
                    result.FMC_StudentwiseJVFlag = feepge.FMC_StudentwiseJVFlag;
                    result.FMC_RebateTypeFlag = feepge.FMC_RebateTypeFlag;
                    result.FMC_Online_Payment_Aca_Yr_Flag = feepge.FMC_Online_Payment_Aca_Yr_Flag;
                    result.FMC_RebateAplicableFlg = feepge.FMC_RebateAplicableFlg;
                    result.FMC_RebateAgainstFullPaymentFlg = feepge.FMC_RebateAgainstFullPaymentFlg;
                    result.FMC_RebateAgainstPartialPaymentFlg = feepge.FMC_RebateAgainstPartialPaymentFlg;
                   result.FMC_EnablePartialPaymentFlg = feepge.FMC_EnablePartialPaymentFlg;




                    //if(FGpage.rolenme!="Admin")
                    //{
                    //    result.userid = result.userid;
                    //}

                    result.FMC_EableStaffTrans = feepge.FMC_EableStaffTrans;
                    result.FMC_EableOtherStudentTrans = feepge.FMC_EableOtherStudentTrans;

                    result.FMC_FineEnableDisable = feepge.FMC_FineEnableDisable;
                    result.FMC_FineMapping = feepge.FMC_FineMapping;

                    result.FMC_AutoRecieptPrintFlag = feepge.FMC_AutoRecieptPrintFlag;

                    result.FMC_USER_PREVILEDGE = feepge.FMC_USER_PREVILEDGE;
                    result.FMC_StaffConcessionCheck = feepge.FMC_StaffConcessionCheck;

                    result.FMC_ShowPreviousFeeFisrtFlg = feepge.FMC_ShowPreviousFeeFisrtFlg;
                    result.FMC_FeeSearchNoOfDigits = feepge.FMC_FeeSearchNoOfDigits;
                    result.FMC_MakerCheckerReqdFlg = feepge.FMC_MakerCheckerReqdFlg;


                    //result.cardchargesflag = feepge.cardchargesflag;
                    //result.debitcardcharges = feepge.debitcardcharges;
                    //result.creditcardcharges = feepge.creditcardcharges;

                    result.UpdatedDate = DateTime.Now;
                    _FeeGroupContext.Update(result);
                    var contactExists = _FeeGroupContext.SaveChanges();

                    if (contactExists == 1)
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
                else
                {
                    var result = _FeeGroupContext.feemastersettings.Where(t => t.MI_Id == feepge.MI_Id && t.userid==FGpage.userid);

                    if (result.Count() > 0)
                    {
                        retval = "Configuration settings are mapped for the user";
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
                List<FeeMasterConfigurationDMO> allpages1 = new List<FeeMasterConfigurationDMO>();
                allpages1 = _FeeGroupContext.feemastersettings.Where(t=>t.MI_Id== feepge.MI_Id).OrderBy(t => t.FMC_Id).ToList();
                FGpage.datasaved = allpages1.ToArray();

                //    FGpage.feeconfiglist = (from a in _FeeGroupContext.master_institution
                //                           from b in _FeeGroupContext.feemastersettings
                //                           from c in _FeeGroupContext.Staff_User_Login
                //                           from d in _FeeGroupContext.MasterEmployee
                //                           where (a.MI_Id == b.MI_Id && b.userid == c.Id && c.Emp_Code == d.HRME_Id && a.MI_Id == FGpage.MI_Id)
                //                           select new FeeStudentTransactionDTO
                //                           {
                //                               institutionname = a.MI_Name,
                //                               HRME_EmployeeFirstName = d.HRME_EmployeeFirstName,
                //                               FMC_Id = b.FMC_Id
                //                           }
                //).ToArray();

                FGpage.feeconfiglist = (from a in _FeeGroupContext.master_institution
                                       from b in _FeeGroupContext.feemastersettings
                                       from c in _FeeGroupContext.applicationUser
                                       where (a.MI_Id == b.MI_Id && b.userid == c.Id && c.Id == b.userid && a.MI_Id == FGpage.MI_Id)
                                       select new FeeStudentTransactionDTO
                                       {
                                           institutionname = a.MI_Name,
                                           HRME_EmployeeFirstName = c.UserName,
                                           FMC_Id = b.FMC_Id
                                       }
                 ).ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            //   }
            return FGpage;
        }
    }
}
