using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
   public  class Fee_Payment_ManagementDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long termswise { get; set; }
        public long ISMMCLT_Id { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public Array clientlist { get; set; }
        public Array getreportdetails { get; set; }
        public clientlistarray1[] clientlistarray { get; set; }


        public class clientlistarray1
        {
            public long ISMMCLT_Id { get; set; }
        }
    }
}
