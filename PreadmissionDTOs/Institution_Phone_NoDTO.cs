using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class Institution_Phone_NoDTO : CommonParamDTO
    {
        public long MIPN_Id { get; set; }
        public long MI_Id { get; set; }
        public long MIPN_PhoneNo { get; set; }
    }
}
