using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeCategoryWiseReportDTO
    {
        public long MI_Id { get; set; }
        public DateTime frmdate { get; set; }
        public DateTime todate { get; set; }
        public long yearid { get; set; }
        
        public Array reportdatelist { get; set; }
    }
  
}
