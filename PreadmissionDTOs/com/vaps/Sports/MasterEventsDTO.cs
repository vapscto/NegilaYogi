using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class MasterEventsDTO:CommonParamDTO
    {
        public long SPCCME_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCME_EventName { get; set; }
        public string SPCCME_EventNameDesc { get; set; }
       // public string SPCCME_Flag { get; set; }
        public bool SPCCME_ActiveFlag { get; set; }
        public int count { get; set; }
        public string returnVal { get; set; }
        public Array eventList { get; set; }
        public Array editDetails { get; set; }
        public string radiotype { get; set; }
        public long SPCCPM_Id { get; set; }
        public string SPCCPM_Name { get; set; }
        public long ASMAY_Id { get; set; }
    }
}
