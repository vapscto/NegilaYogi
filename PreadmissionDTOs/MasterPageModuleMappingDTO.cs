using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterPageModuleMappingDTO : CommonParamDTO
    {
        public long IVRMMP_Id { get; set; }
        public long IVRMM_Id { get; set; }
        public long IVRMP_Id { get; set; }
        public Array fillmodule { get; set; }
        public Array fillpagesdata { get; set; }

        public Array thirdgriddata { get; set; }
        public MasterPageDTO[] savetmpdata { get; set; }
        public MasterPageDTO masterPage { get; set; }
        public MasterModuleDTO mastermodule { get; set; }
        public string returnval { get; set; }

        public string ivrmmP_PageName { get; set; }
        public string ivrmM_ModuleName { get; set; }

    }
}
