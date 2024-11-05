using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeConcessionInstallmentDTO
    {
        public long FSCI_ID { get; set; }
        public long FSCI_FSC_Id { get; set; }
        public long FTI_Id { get; set; }
        public long FSCI_ConcessionAmount { get; set; }
    }
}
