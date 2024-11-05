using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.IVRS
{
   public class IVRS_Call_StatusDTO
    {
        public long IMCS_Id { get; set; }
        public string IMCS_VirtualNo { get; set; }
        public long IMCS_MI_Id { get; set; }
        public string IMCS_SchoolName { get; set; }
        public string IMCS_URL { get; set; }
        public long IMCS_Year { get; set; }
        public string IMCS_Month { get; set; }
        public long IMCS_AssignedCall { get; set; }
        public long IMCS_InboundCalls { get; set; }
        public long IMCS_OutboundCalls { get; set; }
        public long IMCS_AvailableCalls { get; set; }
        public bool IMCS_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
