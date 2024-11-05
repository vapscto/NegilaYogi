using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.Student
{
   public class SmsEmailReportDTO
    {
        public long MI_Id { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Mobile_no { get; set; }

        public string optionflag { get; set; }

        public Array yearlist { get; set; }
        public Array studlist { get; set; }
        public long ASMAY_Id { get; set; }
    }
}
