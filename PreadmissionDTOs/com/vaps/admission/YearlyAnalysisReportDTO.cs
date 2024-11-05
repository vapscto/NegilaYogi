using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class YearlyAnalysisReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int Noofyears { get; set; }
        public Array getyearlist { get; set; }
        public Array getreportacademicyearlist { get; set; }
        public Array getreport { get; set; }
        public Array getclasslist { get; set; }
        public string reporttype { get; set; }
        public int tcflag { get; set; }
        public int deactiveflag { get; set; }
    }
}
