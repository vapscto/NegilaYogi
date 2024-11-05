using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeConsolidatedReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public Array acayear { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array reportdatelist { get; set; }

        public long acyrid { get; set; }
        public long secidpass { get; set; }
        public long claspass { get; set; }
        public string flagrpt { get; set; }

        public long userid { get; set; }
        public Array fillinstallment { get; set; }


    }
  
}
