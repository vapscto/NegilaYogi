using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
   public class VisitorsMonthEndReport_DTO
    {

        public long MI_Id { get; set; }
        public Array fillmonth { get; set; }
        public Array fillyear { get; set; }
        public int month { get; set; }
        public string monthname { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }

        public string year { get; set; }
        public string IVRM_Month_Id { get; set; }

        public Array griddata { get; set; }
        public int ASMAY_Order { get; set; }

    }
}
