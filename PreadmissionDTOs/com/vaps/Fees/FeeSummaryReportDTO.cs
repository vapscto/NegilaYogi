using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeSummaryReportDTO
    {
        public long MI_Id { get; set; }
        public Array acayear { get; set; }
        public Array fgrp { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public string FalgIn { get; set; }
        public string Falgout { get; set; }
        public TempDTOO[] TempararyArrayListnew { get; set; }
        public Array reportdatelist { get; set; }
        public long yerid { get; set; }
    }
  
}
