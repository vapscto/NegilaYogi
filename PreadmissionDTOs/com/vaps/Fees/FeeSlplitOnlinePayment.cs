using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeSlplitOnlinePayment
    {
        public string name { get; set; }
        public string merchantId { get; set; }
        public string value { get; set; }
        public string commission { get; set; }
        public string description { get; set; }

        public string Merchant { get; set; }
        public string CommonKey { get; set; }
        public string URL { get; set; }
        public string splitAmount { get; set; }

    }
}
