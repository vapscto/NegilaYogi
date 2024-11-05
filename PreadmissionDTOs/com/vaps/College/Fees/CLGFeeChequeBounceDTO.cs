using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Fees
{
    public class CLGFeeChequeBounceDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long AMCST_Id { get; set; }
        public Array yearlist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semisterlist { get; set; }
        public Array sectionlist { get; set; }
        public Array studentlist { get; set; }
        public Array receiptlist { get; set; }
        public long FYP_Id { get; set; }
        public string FYP_ReceiptNo { get; set; }
        public string AMCST_FirstName { get; set; }
        public long FCCB_Id { get; set; }
        public DateTime FCCB_Date { get; set; }
        public decimal FCCB_Amount { get; set; }
        public string FCCB_Remarks { get; set; }
        public bool returnval { get; set; }
        public string ASMAY_Year { get; set; }
        public Array alldata { get; set; }
    }
}
