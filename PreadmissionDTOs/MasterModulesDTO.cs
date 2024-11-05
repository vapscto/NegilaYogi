using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterModulesDTO : CommonParamDTO
    {
        public long IVRMM_Id { get; set; }
        public string IVRMM_ModuleName { get; set; }
        public string IVRMM_ModuleDesc { get; set; }
        public Array masterModulesname { get; set; }

        public int Module_ActiveFlag { get; set; }

        public string returnval { get; set; }
        public string returnduplicatestatus { get; set; }

        public long userid { get; set; }

    }
}
