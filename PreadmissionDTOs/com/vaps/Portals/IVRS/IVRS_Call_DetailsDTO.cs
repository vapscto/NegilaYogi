using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.IVRS
{
    public class IVRS_Call_DetailsDTO
    {
        public long IMCD_Id { get; set; }
        public string IMCD_VirtualNo { get; set; }
        public long IMCD_MI_Id { get; set; }
        public string IMCD_SchoolName { get; set; }
        public string IMCD_URL { get; set; }
        public long IMCD_MobileNo { get; set; }
        public string IMCD_InOutFlg { get; set; }
        public DateTime? IMCD_DateTime { get; set; }
        public string IMCD_CallStatus { get; set; }
        public string IMCD_CallDuration { get; set; }
        public long IMCD_PulseCount { get; set; }
        public bool IMCD_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
