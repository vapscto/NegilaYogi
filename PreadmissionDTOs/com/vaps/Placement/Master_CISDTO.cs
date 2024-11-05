using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Placement
{
   public class PL_CampusInterview_ScheduleDTO
    {
        public string returnduplicatestatus;

        public long PLCISCH_Id { get; set; }
        public long MI_Id { get; set; }
        
        public DateTime? PLCISCH_DriveFromDate { get; set; }
        public DateTime? PLCISCH_DriveToDate { get; set; }
        public string PLCISCH_JobDetails { get; set; }
        public bool PLCISCH_OnCampusFlg { get; set; }
        public string PLCISCH_InterviewVenue { get; set; }
        public bool PLCISCH_ActiveFlag { get; set; }
        public DateTime? PLCISCH_CreatedDate { get; set; }
        public DateTime? PLCISCH_UpdatedDate { get; set; }
        public long? PLCISCH_CreatedBy { get; set; }
        public long? PLCISCH_UpdatedBy { get; set; }
        public long User_Id { get; set; }
        public string returnval { get; set; }

        public Array pages { get; set; }
        public Array getdata { get; set; }
        public Array getsavedata { get; set; }
        public Array editdata { get; set; }
        public int UserId { get; set; }
    }
}
