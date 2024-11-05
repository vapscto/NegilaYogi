using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
   public class SMSMail_HeaderDTO
    {
        public long ISMH_Id { get; set; }
        public string ISMH_HeaderName { get; set; }
        public long UserId { get; set; }
        public Array alldata { get; set; }
        public Array Editlist { get; set; }
        public string msg { get; set; }
        public bool duplicate { get; set; }
    }
}
