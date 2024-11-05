using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class PA_School_Application_ProspectusDTO:CommonParamDTO
    {
        public long PASAP_Id { get; set; }
        public long PASR_Id { get; set; }
        public long PASP_Id { get; set; }
    }
}
