using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class Institution_EmailIdDTO : CommonParamDTO
    {
        public long MIE_Id { get; set; }
        public long MI_Id { get; set; }
        public string MIE_EmailId { get; set; }
    }
}
