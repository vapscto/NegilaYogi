using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterSectionDTO : CommonParamDTO
    {
        public long ASMC_Id { get; set; }
        public long MI_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMC_SectionCode { get; set; }
        public int ASMC_Order { get; set; }
        public int  ASMC_ActiveFlag { get; set; }
        public int ASMC_MaxCapacity { get; set; }

        public Array  MasterSectionData { get; set; }
        public string returnval { get; set; }
        public string message { get; set; }
        public Array getsectionlist { get; set; }
    }
}
