using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class MasterAreaDTO
    {
        public long TRMA_Id { get; set; }
        public long MI_Id { get; set; }
        public string TRMA_AreaName { get; set; }
        public string TRMA_AliasName { get; set; }
        public bool TRMA_ActiveFlg { get; set; }
        public string message { get; set; }
        public bool retrval { get; set; }
        public Array getmasterarea { get; set; }
        public Array geteditdataarea { get; set; }
    }
}
