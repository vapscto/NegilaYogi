using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class MasterTimeSlabDTO:CommonParamDTO
    {
        public long LFSE_Id { get; set; }
        public long MI_Id { get; set; }
        // public long LMC_Id { get; set; }
        public string LFSE_UserType { get; set; }
        public string LFSE_SlabTypeFlg { get; set; }
        public bool LFSE_PerDayFlg { get; set; }
        public int LFSE_FromDay { get; set; }
        public int? LFSE_ToDay { get; set; }
        public decimal LFSE_Amount { get; set; }
        public bool LFSE_ActiveFlg { get; set; }
        public string LMC_CategoryName { get; set; }
       // public Array categorylist { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array alldata { get; set; }
    }
}
