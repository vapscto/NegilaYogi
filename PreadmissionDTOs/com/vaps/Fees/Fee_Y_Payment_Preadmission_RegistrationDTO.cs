using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class Fee_Y_Payment_Preadmission_RegistrationDTO
    {
        public long FYPPR_Id { get; set; }
        public long FYP_Id { get; set; }
        public long PASR_Id { get; set; }
        public long FYPPR_TotalPaidAmount { get; set; }
        public long FYPPR_ActiveFlag { get; set; }
    }
}
