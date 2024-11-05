using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class Organisation_MobileDTO : CommonParamDTO
    {
        public long MOMN_Id { get; set; }
        public long MO_Id { get; set; }
        public long MOMN_MobileNo { get; set; }
        public string MOM_Flag { get; set; }
    }
}
