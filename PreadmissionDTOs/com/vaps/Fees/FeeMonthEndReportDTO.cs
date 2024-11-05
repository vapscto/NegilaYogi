using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeMonthEndReportDTO
    {
     
        public long MI_ID { get; set;}
     
        public Array acayear { get; set; }
        public long userid { get; set; }
        public string  monthpass  { get; set; }
        public string acayid { get; set; }
        //public DateTime? frmdate { get; set; }
        //public DateTime? todate { get; set; }
        public Array reportdatelist { get; set; }
        //   public string bankcount { get; set; }
        public long type { get; set; }
        public long yearid { get; set; }

        public long ASMAY_Id { get; set; }

        public Array fillmonth { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        //public string monthpass { get; set; }
        //public string monthpass { get; set; }
        //public string monthpass { get; set; }

    }
}
