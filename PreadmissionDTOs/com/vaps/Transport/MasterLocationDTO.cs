using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class MasterLocationDTO
    {
        public long TRML_Id { get; set; }
        public long MI_Id { get; set; }
        public long TRMA_Id { get; set; }
        public string TRML_Latitude { get; set; }
        public string TRML_Longitude { get; set; }
        public bool TRML_ActiveFlg { get; set; }
        public Array getlocationareadata { get; set; }
        public string message { get; set; }
        public bool retrunval { get; set; }
        public string TRMA_AreaName { get; set; }
        public string TRML_LocationName { get; set; }
        public Array getzonearea { get; set; }
        public Array geteditdata { get; set; }
    }
}
