using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeTermWiseRebateSettingDTO
    {
        public long MI_Id { get; set; }

        public long user_id { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array GroupHeadData { get; set; }
        public Array academicdrp { get; set; }

        public bool returnval { get; set; }
        public bool dupr { get; set; }
        public string message { get; set; }
        public long FMTRS_Id { get; set; }
        public long FMT_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime FMTRS_RebateApplicableDate { get; set; }
        public string FMTRS_RebateAmountPercentFlg { get; set; }
        public string ASMAY_Year { get; set; }
        public decimal FMTRS_RebateAmountPercentValue { get; set; }

        public Array termlist { get; set; }

        public string FMT_Name { get; set; }
    }
}
