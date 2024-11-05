using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Library
{
    public class MasterFloorDTO:CommonParamDTO
    {
        public long Floor_Id { get; set; }
        public long MI_Id { get; set; }
        public string FloorName { get; set; }
        public bool Floor_ActiveFlag { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array floorlist { get; set; }

    }
}
