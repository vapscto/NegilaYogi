using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class Preadmission_School_Registration_Concession_StatusDTO : CommonParamDTO
    {
        public long PSRCS_ID { get; set; }
        public long PASR_ID { get; set; }
        public long PASRS_Id { get; set; }
        public bool Flag { get; set; }
    }
}
