using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TTMonthEndReportDTO
    {
        public long MI_ID { get; set; }
        public long ASMAY_ID { get; set; }
        public long month { get; set; }
        public long year { get; set; }
        public Array acdlist { get; set; }
        public Array monthlist { get; set; }
        public Array reportdatelist { get; set; }
        public Array classlist { get; set; }
    }
}
