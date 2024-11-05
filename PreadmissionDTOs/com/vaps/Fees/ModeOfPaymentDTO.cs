using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class ModeOfPaymentDTO
    {
        public ModeOfPaymentDTO[] listdata07 { get; set; }
        public long IVRMMOD_Id { get; set; }
        public long MI_Id { get; set; }
        public string IVRMMOD_ModeOfPayment { get; set; }
        public bool? IVRMMOD_ActiveFlag { get; set; }
        public string IVRMMOD_Flag { get; set; }
        public string IVRMMOD_ModeOfPayment_Code { get; set; }
        public long UserId { get; set; }
        public long loaddata { get; set; }
        public long savedata { get; set; }
        public Array get_payment { get; set; }
        public bool duplicate { get; set; }
        public string msg { get; set; }
        public bool returnval { get; set; }
        public string return_value { get; set; }
        // public long paymentDecative { get; set; }
    }
}
