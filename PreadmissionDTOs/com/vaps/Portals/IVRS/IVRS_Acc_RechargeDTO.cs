using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.IVRS
{
   public class IVRS_Acc_RechargeDTO
    {
        public long IACRE_Id { get; set; }
        public string IACRE_VirtualNo { get; set; }
        public long MI_Id { get; set; }
        public string IACRE_Year { get; set; }
        public string IACRE_Month { get; set; }
        public long IACRE_RechargeAmt { get; set; }
        public string IACRE_PaymentMode { get; set; }
        public string IACRE_ReferneceNo { get; set; }
        public long IACRE_NoOfCalls { get; set; }
        public bool IACRE_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Array institute { get; set; }
        public Array yearlist { get; set; }
        public Array monthlist { get; set; }
        public Array maindata { get; set; }
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array maindata_grid { get; set; }
        public string aA_SchoolName { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
    }
}
