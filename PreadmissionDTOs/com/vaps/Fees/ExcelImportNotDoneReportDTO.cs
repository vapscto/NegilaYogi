using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class ExcelImportNotDoneReportDTO
    {
        public long MI_ID { get; set; }
        public long userid { get; set; }
        public long asmay_id { get; set; }
        public Array acayear { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }

        public Array getreportdata { get; set; }

        public bool returnval { get; set; }
     
    }
}
