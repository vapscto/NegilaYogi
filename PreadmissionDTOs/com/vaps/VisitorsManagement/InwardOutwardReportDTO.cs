using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
    public class InwardOutwardReportDTO
    {
        public long MI_Id { get; set; }
        public Array viewlist { get; set; }
        public Array viewlist1 { get; set; }
        public string radiotype { get; set; }


        public string month_id { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string all1 { get; set; }

    }
}
