using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.mobile
{
    public class OnlinePaymentDTO
    {
        public class input
        {
            public long AMST_Id { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public string ftiidss { get; set; }
            public string grpidss { get; set; }
            public int userId { get; set; }
           // public decimal amount { get; set; }
        }
        public string MARCHANT_ID { get; set; }
        public string trans_id { get; set; }
        public string Seq { get; set; }
        public string amount { get; set; }
        public string productinfo { get; set; }
        public string firstname { get; set; }
        public string email { get; set; }
        public long phone { get; set; }
        public string udf1 { get; set; }
        public string udf2 { get; set; }
        public string udf3 { get; set; }

        public string udf4 { get; set; }

        public string udf5 { get; set; }
        public string udf6 { get; set; }
        public string SaltKey { get; set; }
        public string payu_URL { get; set; }
        public string transaction_response_url { get; set; }
        public string status { get; set; }
        public string service_provider { get; set; }
        public string hash { get; set; }
       public Array productinfoObj { get; set; }

        //public Array PaymentDetailsList { get; set; }
    }
}
