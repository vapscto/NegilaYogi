using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Reports
{
    public class NAACCriteria3ReportDTO 
    {

        public long ASMAY_Id { get; set; }
        public long NCACET343_Id { get; set; }
        public long NCAC533SPCAA_Id { get; set; }
        public long NCAC521PLA_Id { get; set; }
        public long MI_Id { get; set; }
        public Array yearlist { get; set; }
        public Array reportlist { get; set; }
        public NAACCriteria3ReportDTO[] selectedYear { get; set; }
        public NAACCriteria3ReportDTO[] yerlistdata { get; set; }
        public Array govtsclist { get; set; }
        public Array govtsclistfiles { get; set; }
        public int  ASMAY_Order { get; set; }
        public string ASMAY_Year { get; set; }
        public string scheme { get; set; }
        public long NCACSA343_Id { get; set; }
        public string actname { get; set; }
        public long noofstd { get; set; }
        public string  agency { get; set; }
        public long UserId { get; set; }
        public long cycleid { get; set; }
        public Array reportlist2 { get; set; }
        public Array getinstitutioncycle { get; set; }
        public Array getinstitution { get; set; }
        public NAACCriteria3ReportDTO[] selected_Inst { get; set; }

    }
}
