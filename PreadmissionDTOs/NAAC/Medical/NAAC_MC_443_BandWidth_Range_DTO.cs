using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Medical
{
    public class NAAC_MC_443_BandWidth_Range_DTO
    {

        public long NCMC443BWR_Id { get; set; }
        public long MI_Id { get; set; }
        public bool returnval { get; set; }
        public Array institutionlist { get; set; }
        public long ASMAY_Id { get; set; }
        public Array alldata1 { get; set; }
        public string ASMAY_Year { get; set; }
        public Array allacademicyear { get; set; }
        public long NCMC443BWR_Year { get; set; }
        public string NCMC443BWR_Range { get; set; }
        public bool NCMC443BWR_OneOrMoreGBPS { get; set; }
        public bool NCMC443BWR_500MBPSTo1GBPS { get; set; }
        public bool NCMC443BWR_250MBPSTo500MBPS { get; set; }
        public bool NCMC443BWR_50MBPSTo250MBPS { get; set; }
        public bool NCMC443BWR_LessThan50MBPS { get; set; }
        public string flag { get; set; }
        public Array editlist { get; set; }
        public string msg { get; set; }
        public NAAC_MC_443_BandWidth_Range_DTO[] filelist {get;set;}
        public long UserId { get; set; }
    }
}
