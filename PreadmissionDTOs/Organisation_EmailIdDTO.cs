using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class Organisation_EmailIdDTO : CommonParamDTO
    {
        public long MOE_Id { get; set; }
        public long MO_Id { get; set; }
        public string MOE_EmailId { get; set; }
        public string MOE_Flag { get; set; }
    }
}
