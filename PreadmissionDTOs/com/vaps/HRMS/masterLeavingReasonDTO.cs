using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
  public class masterLeavingReasonDTO
    {
        public long HRMLREA_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string HRMLREA_LeavingReason { get; set; }
        public bool HRMLREA_TransferredFlg { get; set; }
        public bool HRMLREA_ActiveFlg { get; set; }
        public long HRMLREA_CreatedBy { get; set; }
        public long HRMLREA_UpdatedBy { get; set; }
        public DateTime? HRMLREA_CreatedDate { get; set; }
        public DateTime? HRMLREA_UpdatedDate { get; set; }
        public Array alldata { get; set; }
        public Array editlist { get; set; }
        public bool duplicate { get; set; }
        public string msg { get; set; }
        public bool returnval { get; set; }
    }
}
