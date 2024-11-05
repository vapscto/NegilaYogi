using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeReceiptDTO
    {
        public long MI_ID { get; set; }
        public Array newreplist { get; set; }
        public long miid { get; set; }
        public string miname { get; set; }
        public string insname { get; set; }
        public string insaddress { get; set; }
        public Array insdata { get; set; }
        public Array categoryarray { get; set; }

        public string receiptNo { get; set; }
        public long paymentid { get; set; }
        public long acayyearid { get; set; }
        public string recpno { get; set; }
        public Array reportdatelist { get; set; }
        //--------------------------
        public string studentnaem { get; set; }
        public string admno { get; set; }
        public string academicyea { get; set; }
        public string classname { get; set; }
      
        public string radioval { get; set; }
        public Array yearlist { get; set; }
    }
}
