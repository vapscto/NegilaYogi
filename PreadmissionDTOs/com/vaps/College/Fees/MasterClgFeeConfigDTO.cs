using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Fees
{
    public class MasterClgFeeConfigDTO : CommonParamDTO
    {
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
        public Array datasaved { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public Array masterdata { get; set; }
        public int RClassFlag { get; set; }
        public int test { get; set; }
        public long ASMAY_ID { get; set; }

        public string FMC_Receipt_Format { get; set; }

        public bool cardchargesflag { get; set; }
        public string debitcardcharges { get; set; }
        public string creditcardcharges { get; set; }

        public long userid { get; set; }

        public int FMC_EableStaffTrans { get; set; }
        public int FMC_EableOtherStudentTrans { get; set; }
        public int FMC_No_Receipt { get; set; }

        public bool fineenabledisable { get; set; }

        public bool FMC_BtachwiseFeeGlg { get; set; }
        public bool? FMC_MakerCheckerReqdFlg { get; set; }

        public bool? FMC_RoomwiseHostelFeeFlg { get; set; }
        public bool? FMC_CommonHostelFeeFlg { get; set; }
        public bool? FMC_CommonTransportLocationFeeFlg { get; set; }
        public bool? FMC_TransportFeeLocationFlag { get; set; }

        public bool? FMC_FineMapping { get; set; }

        public string institutionname { get; set; }
        public string HRME_EmployeeFirstName { get; set; }

        public string rolenme { get; set; }

        public Array feeconfiglist { get; set; }

        public bool? FMC_CommonTransportAreaFeeFlg { get; set; }
    }
}
