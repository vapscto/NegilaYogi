using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class Fee_Y_Payment_Preadmission_ApplicationDTO
    {
        public long FYPPA_Id { get; set; }
        public long FYP_Id { get; set; }
        public long PASA_Id { get; set; }
        public long FYPPA_TotalPaidAmount { get; set; }
        public long FYPPA_ActiveFlag { get; set; }
    }
}
