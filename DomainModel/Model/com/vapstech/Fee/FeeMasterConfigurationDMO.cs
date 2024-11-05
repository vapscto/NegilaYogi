using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Master_Configuration")]

    public class FeeMasterConfigurationDMO : CommonParamDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMC_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMC_GroupOrTermFlg { get; set; }
        public int FMC_Areawise_FeeFlg { get; set; }
        public int FMC_TransportFeeAreaFlag { get; set; }
        public int FMC_TransportFeeZoneFlag { get; set; }
        public int FMC_DOACheckFlag { get; set; }
        public int FMC_Default_Currency { get; set; }
        public int FMC_ArrearColumn { get; set; }
        public int FMC_Fine_Column { get; set; }
        public int FMC_ArrearLedgerFlag { get; set; }
        public int FMC_Fine_LedgerFlag { get; set; }
        public int FMC_ArrearAfterFlag { get; set; }
        public string FMC_Receipt_Signatory { get; set; }
        public string FMC_Receipt_SignatoryImage { get; set; }
        public int FMC_ChallanOptionFlag { get; set; }
        public int FMC_AutoReceiptFeeGroupFlag { get; set; }
        public int FMC_GroupRemarksFlag { get; set; }
        public int FMC_RInstallmentsFlag { get; set; }
        public int FMC_RInstallmentsMergeFlag { get; set; }
        public int FMC_RFineFlag { get; set; }
        public int FMC_RConcessionFlag { get; set; }
        public int FMC_RWaivedFlag { get; set; }
        public int FMC_RBalanceFlag { get; set; }
        public int FMC_RAmountFlag { get; set; }
        public int FMC_RBankFlag { get; set; }
        public int FMC_RDueDateFlag { get; set; }
        public int FMC_RAddressFlag { get; set; }
        public int FMC_RPaperSizeFlag { get; set; }
        public int FMC_RFeeGroupFeeHeadFlag { get; set; }
        public int FMC_RSplFeeHeadFlag { get; set; }
        public int FMC_RHeaderTitleFlag { get; set; }
        public int FMC_RClassFlag { get; set; }
        public int FMC_RSectionFlag { get; set; }
        public int FMC_RUserNameFlag { get; set; }
        public int FMC_RFatherNameFlag { get; set; }
        public int FMC_MotherNameFlag { get; set; }
        public int FMC_RFeeHeaderFlag { get; set; }
        public int FMC_RPaymentDetailsFlag { get; set; }
        public int FMC_RAmountReceivedFlag { get; set; }
        public int FMC_RRemarksFlag { get; set; }
        public int FMC_RCurrentDateFlag { get; set; }
        public string FMC_StudentwiseJVFlag { get; set; }
        public string FMC_RebateTypeFlag { get; set; }
        public long ASMAY_ID { get; set; }
        public long userid { get; set; }
        public string FMC_Receipt_Format { get; set; }
        public bool cardchargesflag { get; set; }
        public string debitcardcharges { get; set; }
        public string creditcardcharges { get; set; }
        public int FMC_EableStaffTrans { get; set; }
        public int FMC_EableOtherStudentTrans { get; set; }
        public bool FMC_FineEnableDisable { get; set; }
        public bool? FMC_FineMapping { get; set; }
        public int fee_group_setting { get; set; }
        public string MI_Logo { get; set; }
        public string FMC_Online_Payment_Aca_Yr_Flag { get; set; }
        public int FMC_No_Receipt { get; set; }
        public string FMC_Partial_Pre_Payment_flag { get; set; }
        public bool? FMC_AutoRecieptPrintFlag { get; set; }
        public int FMC_USER_PREVILEDGE { get; set; }
        public bool FMC_AUTO_FEE_MAP_TC { get; set; }
        public bool FMC_InstallmentwiseJVFlg { get; set; }
        public bool FMC_FeeReceiptNoAsRVNoFlg { get; set; }
        public bool FMC_AutoJVFlg { get; set; }
        public bool FMC_StaffConcessionCheck { get; set; }
        public string FMC_DetailedDisplayFlg { get; set; }
        public bool? FMC_ShowPreviousFeeFisrtFlg { get; set; }
        public string FMC_OBAutoAdjustFlg { get; set; }

        public bool? FMC_BtachwiseFeeGlg { get; set; }

        public bool? FMC_MakerCheckerReqdFlg { get; set; }

        public bool? FMC_RoomwiseHostelFeeFlg { get; set; }
        public bool? FMC_CommonHostelFeeFlg { get; set; }
        public bool? FMC_CommonTransportLocationFeeFlg { get; set; }
        public bool? FMC_TransportFeeLocationFlag { get; set; }
        public bool? FMC_CommonTransportAreaFeeFlg { get; set; }
        public int? FMC_FeeSearchNoOfDigits { get; set; }

        public bool? FMC_RebateAplicableFlg { get; set; }
        public bool? FMC_RebateAgainstFullPaymentFlg { get; set; }
        public bool? FMC_RebateAgainstPartialPaymentFlg { get; set; }

       public int? FMC_EnablePartialPaymentFlg { get; set; }

        public int? FMC_ReadmitFineCalculationFlg { get; set; }

    }
}
