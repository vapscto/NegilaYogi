using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterModuleDTO : CommonParamDTO
    {
        public long IVRMM_Id { get; set; }
        public string IVRMM_ModuleName { get; set; }
        public string IVRMM_ModuleDesc { get; set; }

        public int Module_ActiveFlag { get; set; }
    }
}
