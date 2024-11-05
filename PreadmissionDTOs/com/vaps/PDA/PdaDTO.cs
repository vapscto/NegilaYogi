using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.PDA
{
    public class PdaDTO
    {
        public long asmay_id { get; set; }
        public long mi_id { get; set; }
        public long user_id { get; set; }
        public Array headdata { get; set; }
        public Array pdadata { get; set; }
        public long fmh_id { get; set; }
        public string FMH_FeeName { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public long PDAMH_Id { get; set; }
        public string PDAMH_HeadName { get; set; }
        public bool returnresult { get; set; }

       

    }
}
