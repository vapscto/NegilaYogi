using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class MasterNarrationDTO
    {
        public long FMNAR_Id { get; set; }
        public long MI_ID { get; set; }
        public long user_id { get; set; }
        public string FMNAR_Narration { get; set; }
        public string FMNAR_NarrationDesc { get; set; }
        public bool FMNAR_ActiveFlag { get; set; }

        public string returnduplicatestatus{get;set;}
        public string message { get;set;}
        public bool returnval { get;set;}
        public Array GroupHeadData { get;set;}

    }
}
