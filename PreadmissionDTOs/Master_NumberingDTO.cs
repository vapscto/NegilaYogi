using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class Master_NumberingDTO : CommonParamDTO
    {
        public long IMN_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string IMN_AutoManualFlag { get; set; }
        public string IMN_DuplicatesFlag { get; set; }
        public string IMN_StartingNo { get; set; }
        public string IMN_WidthNumeric { get; set; }
        public string IMN_ZeroPrefixFlag { get; set; }
        public bool IMN_PrefixAcadYearCode { get; set; }
        public string IMN_PrefixParticular { get; set; }
        public bool IMN_SuffixAcadYearCode { get; set; }
        public string IMN_SuffixParticular { get; set; }
        public string IMN_RestartNumFlag { get; set; }
        public string IMN_Flag { get; set; }

        public Array EnquiryNumberingArraylist { get; set; }
        public Array ProspectusNumberingArraylist { get; set; }
        public Array RegistrationNumberingArraylist { get; set; }
        public Array PreRegistrationNumberingArraylist { get; set; }
        public long ASMAY_Id { get; set; }
        //public Array PreadmissionRegistrationNumberingArraylist { get; set; }
        //public Array ReceiptNumberingArraylist { get; set; }

        public Array AdmissionNumberingArraylist { get; set; }
        public Array AdmissionRegNumberingArraylist { get; set; }

        public bool? IMN_PrefixFinYearCode { get; set; }
        public bool? IMN_PrefixCalYearCode { get; set; }

        public bool? IMN_SuffixFinYearCode { get; set; }
        public bool? IMN_SuffixCalYearCode { get; set; }

        public bool? IRN_RestartAcadYear { get; set; }
        public bool? IRN_RestartFinYear { get; set; }
        public bool? IRN_RestartcalendYear { get; set; }
        public string VoucherName { get; set; }
        public string VoucherType { get; set; }
        public string Observation { get; set; }

        public Array ApplicationNumberingArraylist { get; set; }
        public Array TransactionNumberingArraylist { get; set; }
        public Array ReceiptNumberingArraylist { get; set; }
        public Array VoucherNumberingArraylist { get; set; }

        public string IRN_TransactionName { get; set; }

        //--------------tcno--
        public Array tcNumberingArraylist { get; set; }
        //----------------------

        public Array loanNumberingArraylist { get; set; }

        //Bus Hire.
        public Array onlineBookingNumberingArraylist { get; set; }
        public Array tripNumberingArraylist { get; set; }
        public Array tripBillNumberingArraylist { get; set; }

        //Leave Numbering.

        public Array leaveNumberingArraylist { get; set; }
        public Array RolenoNumbering { get; set; }

        public Array RolenoNumberingConfig { get; set; }

        public Array FieldArray { get; set; }

        public string message { get; set; }


        public long IVRMARNC_Id { get; set; }

        public RollNumberingconfigg[] RollNumberingconfig { get; set; }


    }
    public class RollNumberingconfigg
    {
        public string IVRMARNC_Field { get; set; }
        public string IVRMARNC_AscDscOrder { get; set; }
        public long IVRMARNC_Order { get; set; }
        public long IVRMARNC_Id { get; set; }

    }
}
