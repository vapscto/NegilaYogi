using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_MonthEndReportDTO
    {

        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string roleflag { get; set; }
        public int month { get; set; }
        public string year { get; set; }
        public Array acayear { get; set; }
        public Array Month_array { get; set; }
        public Array get_monthendreport { get; set; }
    }
}
