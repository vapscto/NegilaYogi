using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class AreaGroupMappingDTO : CommonParamDTO
    {
        public long FGAM_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMG_Id { get; set; }
        public long TRMA_Id { get; set; }
        public bool FGAM_ActiveFlag { get; set; }
        public string FGAM_WayFlag { get; set; }
        public bool returnval { get; set; }
        public Array fillarea { get; set; }
        public Array fillgroup { get; set; }
        public Array fillareagroup { get; set; }
        
        public string FMG_GroupName { get; set; }
        public string TRMA_AreaName { get; set; }
        public string returnduplicatestatus { get; set; }
        public List<AreaGroupMappingDTO> TempararyArrayList { get; set; }

        public Array yearlist { get; set; }


        public Array areadata { get; set; }
    }
}
