using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class MasterSponserDTO:CommonParamDTO
    {
        public long SPCCMSP_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMSP_SponsorName { get; set; }
        public string SPCCMSP_ContactPerson { get; set; }
        public long SPCCMSP_ContactNo { get; set; }
        public string SPCCMSP_SponsorDetails { get; set; }
        public bool SPCCMSP_ActiveFlag { get; set; }

        public int count { get; set; }
        public string returnVal { get; set; }
        public Array sponserList { get; set; }
        public Array editDetails { get; set; }

        public bool returnval { get; set; }


    }
}
