using System;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Fees;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model.com.vaps.Fee;
using AutoMapper;


namespace CollegeFeeService.com.vaps.Implementation
{
    public class MasterClgFeeConfigImpl:Interfaces.MasterClgFeeConfigInterface
    {
        private static ConcurrentDictionary<string, MasterClgFeeConfigDTO> _login =
      new ConcurrentDictionary<string, MasterClgFeeConfigDTO>();

        public CollFeeGroupContext _FeeGroupContext;
        readonly ILogger<MasterClgFeeConfigImpl> _logger;
        public MasterClgFeeConfigImpl(CollFeeGroupContext frgContext, ILogger<MasterClgFeeConfigImpl> log)
        {
            _logger = log;
            _FeeGroupContext = frgContext;
        }

        public MasterClgFeeConfigDTO getdetailsY(MasterClgFeeConfigDTO FGRDT)
        {
            try
            {
                if (FGRDT.rolenme !="Admin")
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

                FGRDT.feeconfiglist = (from a in _FeeGroupContext.master_institution
                                       from b in _FeeGroupContext.feemastersettings
                                    from c in _FeeGroupContext.applicationUser
                                       where (a.MI_Id == b.MI_Id && b.userid == c.Id && c.Id == b.userid && a.MI_Id == FGRDT.MI_Id)
                                       select new MasterClgFeeConfigDTO
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

        public MasterClgFeeConfigDTO SaveconfigData(MasterClgFeeConfigDTO FGpage)
        {
            //bool returnresult = false;
            FeeMasterConfigurationDMO feepge = Mapper.Map<FeeMasterConfigurationDMO>(FGpage);

            string retval = "";
            try
            {
                if (feepge.FMC_Id > 0)
                {
                    var result = _FeeGroupContext.feemastersettings.Single(t => t.FMC_Id == feepge.FMC_Id);
                    result.MI_Id = feepge.MI_Id;
                    result.FMC_GroupOrTermFlg = feepge.FMC_GroupOrTermFlg;
                    result.FMC_Areawise_FeeFlg = feepge.FMC_Areawise_FeeFlg;
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
                    result.userid = feepge.userid;

                    result.FMC_EableStaffTrans = feepge.FMC_EableStaffTrans;
                    result.FMC_EableOtherStudentTrans = feepge.FMC_EableOtherStudentTrans;

                    result.FMC_FineEnableDisable = feepge.FMC_FineEnableDisable;
                    //result.FMC_BtachwiseFeeGlg = feepge.FMC_BtachwiseFeeGlg;
                    result.FMC_MakerCheckerReqdFlg = feepge.FMC_MakerCheckerReqdFlg;

                    result.FMC_RoomwiseHostelFeeFlg = feepge.FMC_RoomwiseHostelFeeFlg;
                    result.FMC_CommonHostelFeeFlg = feepge.FMC_CommonHostelFeeFlg;
                    result.FMC_CommonTransportLocationFeeFlg = feepge.FMC_CommonTransportLocationFeeFlg;
                    result.FMC_TransportFeeLocationFlag = feepge.FMC_TransportFeeLocationFlag;
                    result.FMC_CommonTransportAreaFeeFlg = feepge.FMC_CommonTransportAreaFeeFlg;

                    result.FMC_FineMapping = feepge.FMC_FineMapping;

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
                    var result = _FeeGroupContext.feemastersettings.Where(t => t.FMC_Id == feepge.FMC_Id && t.MI_Id == feepge.MI_Id);

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
                List<FeeMasterConfigurationDMO> allpages1 = new List<FeeMasterConfigurationDMO>();
                allpages1 = _FeeGroupContext.feemastersettings.Where(t => t.MI_Id == feepge.MI_Id).OrderBy(t => t.FMC_Id).ToList();
                FGpage.datasaved = allpages1.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            //   }
            return FGpage;
        }

        public MasterClgFeeConfigDTO editdetails(MasterClgFeeConfigDTO FGRDT)
        {
            try
            {
                List<FeeMasterConfigurationDMO> feeheads = new List<FeeMasterConfigurationDMO>();
                feeheads = _FeeGroupContext.feemastersettings.Where(t => t.MI_Id == FGRDT.MI_Id && t.FMC_Id == FGRDT.FMC_Id).ToList();
                FGRDT.masterdata = feeheads.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGRDT;
        }

    }
}
