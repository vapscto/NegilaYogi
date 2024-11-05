using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace PreadmissionDTOs.com.vaps.Library
{
    public class MasterPeriodicityDTO:CommonParamDTO
    {
        public long LMPE_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMPE_PeriodicityName { get; set; }
        public bool LMPE_ActiveFlg { get; set; }    
        public bool returnval { get; set; }
        public bool duplicate { get; set; }
        public Array periodlist { get; set; }
        public long UserId { get; set; }
    }
}
