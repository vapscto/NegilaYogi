using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterSourceDTO : CommonParamDTO
    {
        public long PAMS_Id { get; set; }
        public string PAMS_SourceName { get; set; }
        public string PAMS_SourceDesc { get; set; }
        public Array pagesdata { get; set; }
        public bool returnval { get; set; }

        public string returnMsg { get; set; }
    }
}
