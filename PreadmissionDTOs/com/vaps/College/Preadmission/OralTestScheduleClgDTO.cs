using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Preadmission
{
   public class OralTestScheduleClgDTO
    {
        public long PAOTS_Id { get; set; }
        public long New_PAOTS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long IVRMSTAUL_Id { get; set; }
        public DateTime? PAOTS_EntryDate { get; set; }
        public string PAOTS_ScheduleName { get; set; }
        public DateTime? PAOTS_ScheduleDate { get; set; }
        public string PAOTS_ScheduleTime { get; set; }
        public string PAOTS_ScheduleTimeTo { get; set; }
        public string PAOTS_AM_PM { get; set; }
        public string checkadd { get; set; }
        public string PAOTS_Remarks { get; set; }
        public DateTime? PAOTS_LB_FT { get; set; }
        public DateTime? PAOTS_LB_TT { get; set; }
        public int PAOTS_TimePeriod { get; set; }
        public bool PAOTS_TPFlag { get; set; }

        public bool autoschedule { get; set; }
        public int PAOTS_Strength { get; set; }

        public long PASR_Id { get; set; }
        public long User_Id { get; set; }
        public string username { get; set; }

        public Array OralTestSchedule { get; set; }

        public Array SelectedStudentDetails { get; set; }

        public List<StudentDetailsDTO> SelectedStudentData { get; set; }

        public List<StudentDetailsDTO> DeleteStudentData { get; set; }

        public List<StudentDetailsDTO> SelectedStudentDataForEdit { get; set; }

        public string PAOTS_Superviser { get; set; }

        public string PAOTS_Skills { get; set; }

        public string returnvalue { get; set; }

        public long pamsT_Id { get; set; }

        public long userid { get; set; }

        public bool meetingvc { get; set; }
        //public bool vcmeeting { get; set; }

        public bool returnval { get; set; }

        public long LMSLMEET_Id { get; set; }

        public DateTime PlannedDate { get; set; }

        public string PlannedStartTime { get; set; }
        public string PlannedEndTime { get; set; }
        public string MeetingTopic { get; set; }
        public string meetingid { get; set; }
        public Array stafflist { get; set; }
        public Array mappedstafflist { get; set; }
        public string HRME_EmployeeFirstName { get; set; }

        public long HRME_Id { get; set; }
        public stfiddto[] stfids { get; set; }
        public bool principalflg { get; set; }
        public bool managerflg { get; set; }
        public bool anybodystart { get; set; }
        public bool stafflag { get; set; }
        public bool pavidc { get; set; }

    }
    public class stfiddto
    {
        public long? UserId { get; set; }
        public long? HRME_Id { get; set; }

    }
}

