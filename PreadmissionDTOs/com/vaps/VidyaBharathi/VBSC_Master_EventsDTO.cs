using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VidyaBharathi
{
    public class VBSC_Master_EventsDTO
    {
        public string returnduplicatestatus { get; set; }
        public long MI_Id { get; set; }
        public long MT_Id { get; set; }
        public long VBSCME_Id { get; set; }
        public Array get_customer { get; set; }
        public Array Master_trust { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public string VBSCME_EventName { get; set; }
        public string VBSCME_EventNameDesc { get; set; }

    }
}
