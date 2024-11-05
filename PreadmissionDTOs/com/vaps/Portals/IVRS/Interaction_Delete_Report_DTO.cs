using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.IVRS
{
    public class Interaction_Delete_Report_DTO
    {
        public long AMST_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMRT_Id { get; set; }
        public string fromdate { get; set; }
        public string Role_flag { get; set; }
        public string todate { get; set; }
        public string Active { get; set; }
        public string DeActive { get; set; }
        public string Left { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_Id { get; set; }
        public string optionflag { get; set; }
        public Array deletemsglist { get; set; }
        public Array mobapplist { get; set; }
        public Array mobapplisttotalcount { get; set; }
        public Array mobapplistnotcount { get; set; }
        public Array fillyear { get; set; }
    }
}
