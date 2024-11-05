using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class MasterEventVenueDTO:CommonParamDTO
    {
        public long SPCCMEV_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMEV_EventVenue { get; set; }
        public string SPCCMEV_EventVenueDesc { get; set; }
        public bool SPCCMEV_ActiveFlag { get; set; }
        public int count { get; set; }
        public string returnVal { get; set; }
        public Array eventVenueList { get; set; }
        public Array editDetails { get; set; }
    }
}
