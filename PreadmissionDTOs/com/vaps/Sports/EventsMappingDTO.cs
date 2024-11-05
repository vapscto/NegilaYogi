using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class EventsMappingDTO:CommonParamDTO
    {
        public long SPCCE_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long SPCCME_Id { get; set; }
        public long SPCCMEV_Id { get; set; }
        public DateTime? SPCCE_StartDate { get; set; }
        public string SPCCE_StartTime { get; set; }
        public DateTime? SPCCE_EndDate { get; set; }
        public string SPCCE_EndTime { get; set; }
        public string SPCCE_Remarks { get; set; }
        public bool SPCCE_ActiveFlag { get; set; }
        public string SPCCEST_Remarks { get; set; }


        public int count { get; set; }
        public string returnVal { get; set; }
        public Array eventmappingList { get; set; }
        public Array academicYear { get; set; }
        public Array eventsList { get; set; }
        public Array sponsorList { get; set; }
        public Array venuelist { get; set; }
        public Array editDetails { get; set; }
        public string ASMAY_Year { get; set; }
        public string SPCCME_EventName { get; set; }
        public string SPCCMEV_EventVenue { get; set; }       
        //public long SPCCPM_Id { get; set; }

        public long SPCCESP_Id { get; set; }
        public long SPCCMSP_Id { get; set; }
        public bool SPCCESP_ActiveFlag { get; set; }
        public bool SPCCESTR_RecordBrokenFlag { get; set; }
        public string SPCCMSP_SponsorName { get; set; }
        public bool SPCCE_SponsorFlag { get; set; }
        public bool returnval { get; set; }
        public Array modalsponsorlist { get; set; }
        public string SPCCMSP_ContactPerson { get; set; }
        public long SPCCMSP_ContactNo { get; set; }
        public string SPCCMSP_SponsorDetails { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
      

        public EventsMappingDTO[] sponsordata { get; set; }
    }
    public class sponsordata
    {
        public long SPCCMSP_Id { get; set; }
    }
}
