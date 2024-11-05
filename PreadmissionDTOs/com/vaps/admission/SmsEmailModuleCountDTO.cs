using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
   public class SmsEmailModuleCountDTO
    {
        public long MI_Id { get; set; }
        public Array acayear { get; set; }
        public Array fillmonth { get; set; }
        public Array Modulelist { get; set; }
        public rptmonth[] rptmonth { get; set; }
        public Array Smscount { get; set; }
        public Array Emailcount { get; set; }
        public string year { get; set; }
        public string radioption { get; set; }
    }
    public class rptmonth
    {
        public string ivrM_Month_Name { get; set; }
        public long ivrM_Month_Id { get; set; }
    }
}
