using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Library
{
    public class MasterCategory_DTO:CommonParamDTO
    {
        public long LMC_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMC_CategoryName { get; set; }
        public bool LMC_ActiveFlag { get; set; }
        public bool returnval { get; set; }
         public bool duplicate { get; set; }
        public Array categorylist { get; set; }
        public string LMC_CategoryCode { get; set; }
        public string LMC_BNBFlg { get; set; }

    }
}
