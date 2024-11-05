using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.IVRS
{
   public class IVRSInOutCallsReportDTO
    {
        public long MI_ID { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public string typeofrpt { get; set; }
        public Array reportdatelist { get; set; }
        public string conso { get; set; }
        public string Received { get; set; }
        public string connected { get; set; }
       
    }
}
