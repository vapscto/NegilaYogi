using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class EventsSponsorDTO:CommonParamDTO
    {
        public long SPCCESP_Id { get; set; }
        public long MI_Id { get; set; }
        public long SPCCE_Id { get; set; }
        public long SPCCMSP_Id { get; set; }
        public bool SPCCESP_ActiveFlag { get; set; }
        public int count { get; set; }
        public string returnVal { get; set; }
        public Array sponsormappingList { get; set; }
        public Array editDetails { get; set; }
        public Array events { get; set; }
        public Array sponsorList { get; set; }
        public string eventName { get; set; }
        public string sponsorName { get; set; }
        public string spccmE_EventName { get; set; }
        public long SPCCME_Id { get; set; }
        public long SPCCPM_Id { get; set; }
        public string SPCCPM_Name { get; set; }
        public string SPCCME_Flag { get; set; }
    }
}
