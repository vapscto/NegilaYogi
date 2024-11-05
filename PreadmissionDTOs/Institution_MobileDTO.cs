using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class Institution_MobileDTO : CommonParamDTO
    {
        public long MIMN_Id { get; set; }
        public long MI_Id { get; set; }
        public long MIMN_MobileNo { get; set; }
    }
}
