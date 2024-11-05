using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class Organisation_Phone_NoDTO : CommonParamDTO
    {
        public long MOPN_Id { get; set; }
        public long MO_Id { get; set; }
        public long MOPN_PhoneNo { get; set; }
        public string MOP_Flag { get; set; }
    }
}
