using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.PettyCash
{
    public class PC_ReportDTO
    {
        public long MI_Id { get; set; }
        public long Userid { get; set; }
        public long roleid { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public Array getrequisitionreportdetails { get; set; }
        public Array getindentreportdetails { get; set; }
        public Array getindenapprovedreportdetails { get; set; }
        public Array institutiondetails { get; set; }
        public Array getuserinstitution { get; set; }
        public string reporttype { get; set; }
    }
}
