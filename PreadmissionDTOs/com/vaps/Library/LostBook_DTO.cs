using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class LostBook_DTO:CommonParamDTO
    {
        public long LMBANO_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMB_BookTitle { get; set; }
        public long LMC_Id { get; set; }
        public long LMB_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public Array booktitle { get; set; }
        public string LMBANO_AccessionNo { get; set; }
        public string booktype { get; set; }
        public string bookcat_type { get; set; }
        public string searchfilter { get; set; }
        public Array authorlist { get; set; }
        public string LMC_BNBFlg { get; set; }
        public string LMB_BookType { get; set; }
        public long? LMAU_Id { get; set; }
        public long? Login_Id { get; set; }
        public DateTime? LMBANO_LostDamagedDate { get; set; }
        public string LMBANO_LostDamagedReason { get; set; }
        public bool LMBANO_ActiveFlg { get; set; }
        public bool LMBANO_LostDamagedFlg { get; set; }
        public decimal? LMBANO_AmountCollected { get; set; }
        public string LMBANO_ModeOfPayment { get; set; }
        public string LMBANO_AvialableStatus { get; set; }
        public bool returnval { get; set; }
        public Array lostbooks { get; set; }

    }
}
