using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeDefaulterReportStthomasDTO
    {
         public long MI_ID { get; set;}
         public long userid { get; set;}

        public long ASMAY_Id { get; set; }
        public long Amst_Id { get; set; }
        public long fillseccls { get; set; }
        public long fillclasflg { get; set; }
        public string typeofrpt { get; set; }
        public Array getreportdata { get; set; }

        public Array termlist { get; set; }
        public Array getfeedefaultertemplate { get; set; }
        public long[] FMT_Ids { get; set; }
    }
}
