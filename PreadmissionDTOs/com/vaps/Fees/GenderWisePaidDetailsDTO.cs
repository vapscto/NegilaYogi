using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class GenderWisePaidDetailsDTO
    {
        public long MI_ID { get; set; }
        public long userid { get; set; }
        public long asmay_id { get; set; }
        public Array fillmastergroup { get; set; }
        public Array customlist { get; set; }
        public Array grouplist { get; set; }
        public Array getreportdata { get; set; }
        public Array acayear { get; set; }
        public long[] FMGG_Ids { get; set; }
        public long[] FMG_Ids { get; set; }
        public long[] FMT_Ids { get; set; }

        public string type { get; set; }



    }
}
