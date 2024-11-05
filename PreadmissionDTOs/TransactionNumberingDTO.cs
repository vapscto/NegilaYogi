using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class TransactionNumberingDTO : CommonParamDTO
    {
        public long IRN_Id { get; set; }
        public long MI_Id { get; set; }
        public string IRN_TransactionName { get; set; }
        public string IRN_AutoManualFlag { get; set; }
        public string IRN_DuplicatesFlag { get; set; }
        public string IRN_StartingNo { get; set; }
        public string IRN_WidthNumeric { get; set; }
        public string IRN_ZeroPrefixFlag { get; set; }
        public bool IRN_PrefixAcadYearCode { get; set; }
        public bool IRN_PrefixFinYearCode { get; set; }
        public bool IRN_PrefixCalYearCode { get; set; }
        public string IRN_PrefixParticular { get; set; }
        public bool IRN_SuffixAcadYearCode { get; set; }
        public bool IRN_SuffixFinYearCode { get; set; }
        public bool IRN_SuffixCalYearCode { get; set; }
        public string IRN_SuffixParticular { get; set; }
        public string IRN_RestartNumFlag { get; set; }
        public bool IRN_RestartAcadYear { get; set; }
        public bool IRN_RestartFinYear { get; set; }
        public bool IRN_RestartcalendYear { get; set; }
        public int Ref_ITNT_Id { get; set; }
        public string IVN_VoucherName { get; set; }
        public string IVN_VoucherType { get; set; }
        public string IVN_Observation { get; set; }

        public Array ReceiptNumberingArraylist { get; set; }
        public Array VoucherNumberingArraylist { get; set; }
        public Array TransactionNumberingArraylist { get; set; }
        public Array ApplicationNumberingArraylist { get; set; }
        public Array RegistrationNumberingArraylist { get; set; }
        public Array EnquiryNumberingArraylist { get; set; }

    }
}
