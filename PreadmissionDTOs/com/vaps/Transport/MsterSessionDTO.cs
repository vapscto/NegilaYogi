using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class MsterSessionDTO
    {
        public long TRMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string TRMS_SessionName { get; set; }
        public string TRMS_SessionDesc { get; set; }
        public string TRMS_Flag { get; set; }
        public bool TRMS_ActiveFlg { get; set; }
        public Array getloaddata { get; set; }
        public Array geteditdata { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
    }
}
