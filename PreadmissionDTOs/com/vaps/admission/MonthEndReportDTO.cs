using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class MonthEndReportDTO
    {
     
        public long MI_ID { get; set; }
     
        public Array acayear { get; set; }
        public string  monthpass  { get; set; }
        public string acayid { get; set; }
        public DateTime? frmdate { get; set; }
        public DateTime? todate { get; set; }
        public Array reportdatelist { get; set; }
        //   public string bankcount { get; set; }
        public string cashcount { get; set; }
        public string onlinecount { get; set; }
        public string esccount { get; set; }

        public string newadmission { get; set; }

              public string month { get; set; }
        public string year { get; set; }
        public Array Month_array { get; set; }

        public bool categoryflag { get; set; }     

        public Array category_list { get; set; }
        public long AMC_Id { get; set; }

        public Array AMC_logo { get; set; }
        //public string monthpass { get; set; }
        //public string monthpass { get; set; }
        //public string monthpass { get; set; }
        //public string monthpass { get; set; }
        //public string monthpass { get; set; }

    }
}
