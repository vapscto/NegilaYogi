using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
     public class TrainingtypewisereportDTO
    {
        public long MI_Id { get; set; }
        public long Userid { get; set; }
        public long roleid { get; set; }
        public Array getloaddetails { get; set; }
        public Array trainingtypewisereport { get; set; }
        public string EmplYoeeName { get; set; }
        public string shedule { get; set; }
        public decimal HREXTTRN_TotalHrs { get; set;}
        public long HRMETRTY_Id { get; set; }
        public string HRMETRTY_ExternalTrainingType { get; set; }
        public string HRMETRCEN_CenterAddress { get; set; }
        public string HREXTTRN_CertificateFileName { get; set; }
        public string HREXTTRN_CertificateFilePath { get; set; }
        public string HREXTTRN_EndTime { get; set; }
        public string HREXTTRN_StartTime { get; set; }
        public string HREXTTRN_ApprovedFlg { get; set; }
        public DateTime? HREXTTRN_EndDate { get; set; }
        public DateTime? HREXTTRN_StartDate { get; set; }
    }
}
