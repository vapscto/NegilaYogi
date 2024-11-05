using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class Institution_ModuleDTO : CommonParamDTO
    {
        public long IVRMIM_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMM_Id { get; set; }
        public int IVRMIM_Flag { get; set; }
        public int IVRMIM_ModuleOrder { get; set; }
        public string IVRMIM_Flag_New { get; set; }
    }
}
