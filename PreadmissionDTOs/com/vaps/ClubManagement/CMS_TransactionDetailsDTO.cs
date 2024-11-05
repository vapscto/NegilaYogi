using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.ClubManagement
{
    public class CMS_TransactionDetailsDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string returnval { get; set; }
        public Array getreport { get; set; }
        public long CMSTRANSDET_Id { get; set; }
        public long CMSTRANS_Id { get; set; }
        public long CMSTRANSMEMTYINT_Id { get; set; }
        public decimal CMSTRANSDET_Qty { get; set; }
        public decimal CMSTRANSDET_Amount { get; set; }
        public decimal CMSTRANSDET_Tax { get; set; }
        public decimal CMSTRANSDET_NetAmount { get; set; }
        public bool CMSTRANSDET_ActiveFlg { get; set; }
        public DateTime? CMSTRANSDET_CreatedDate { get; set; }
        public Array editarray { get; set; }
    }
}
