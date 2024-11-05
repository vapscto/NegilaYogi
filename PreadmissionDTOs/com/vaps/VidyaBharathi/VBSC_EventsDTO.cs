using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VidyaBharathi
{
    public class VBSC_EventsDTO
    {
        public long MI_Id { get; set; }
        public long MT_Id { get; set; }
        public long VBSCE_Id { get; set; }
        public long VBSCMCL_Id { get; set; }
        public long IVRMMS_ID { get; set; }
        public long IVRMMD_ID { get; set; }
        public long ASMAY_Id { get; set; }
        public long VBSCME_Id { get; set; }
        public string VBSCE_VenueName { get; set; }
        public DateTime VBSCE_StartDate { get; set; }
        public DateTime VBSCE_EndDate { get; set; }
        public string VBSCE_StartTime { get; set; }
        public string VBSCE_EndTime { get; set; }
        public string VBSCE_Remarks { get; set; }
        public bool VBSCE_ActiveFlag { get; set; }
        public DateTime? VBSCE_CreatedDate { get; set; }
        public DateTime? VBSCE_UpdatedDate { get; set; }
        public long VBSCE_CreatedBy { get; set; }
        public long VBSCE_UpdatedBy { get; set; }

        public string VBSCMCL_CompetitionLevel { get; set; }
        public string VBSCME_EventName { get; set; }
        public string ASMAY_Year { get; set; }

        public Array get_Competitionlevel { set; get; }
        public Array academicYear { set; get; }
        public Array get_eventlist { set; get; }
        public Array get_VBSCeventlist { set; get; }
        public Array editDetails { set; get; }


        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public string message { get; set; }
        public bool Count { get; set; }
    }
}
