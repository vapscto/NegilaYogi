using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class SummaryreportsDTO
    {
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public long MI_Id { get; set; }
        public long Userid { get; set; }
        public long roleid { get; set; }
        public long? HRME_Id { get; set; }
        public string HRMETRTY_ExternalTrainingType { get; set; }
        public decimal HREXTTRN_TotalHrs { get; set; }
        public decimal HRMETRTY_MinimumTrainingHrs { get; set; }
        public decimal? HREXTTRNAPP_ApprovedHrs { get; set; }
        public int Rvalue { get; set; }
       public Array getloaddetails { get; set; }
        public Array employeedetails { get; set; }
        public Array summaryreport { get; set; }
        public Array traningsummaryreport { get; set; }
        public Array internalemployeedetails { get; set; }
        public Array internalreport { get; set; }
        public Array programname { get; set; }
        public Array certificatedetails { get; set; }
        public string MI_Name { get; set; }
        public string EmplYoeeName { get; set; }
        public string HRTCR_PrgogramName { get; set; }
        public long? HRTCR_Id { get; set; }
        public long HRTCR_StatusFlg { get; set; }
        public DateTime? HRTCR_StartDate { get; set; }
        public DateTime? HRTCR_EndDate { get; set; }
        public string principalNmae { get; set; }
    }
}
