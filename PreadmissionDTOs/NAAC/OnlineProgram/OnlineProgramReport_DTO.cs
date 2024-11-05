using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.OnlineProgram
{
   public class OnlineProgramReport_DTO
    {
        public long MI_Id { get; set; }
        public Array yearlist { get; set; }
        public Array typelist { get; set; }
        public Array activitylist { get; set; }
        public Array levellist { get; set; }
        public Array reportlist { get; set; }
        public Array departmentlist { get; set; }
        public DateTime PRYR_StartDate { get; set; }
        public DateTime PRYR_EndDate { get; set; }
        public string ASMAY_Year { get; set; }
        public long ASMAY_Id { get; set; }
        public long PRYR_TotalParticipants { get; set; }
        public long PRMTY_Id { get; set; }
        public long PRYRA_Id { get; set; }
        public long PRMTLE_Id { get; set; }
        public long HRMD_Id { get; set; }
        public string PRYR_StartTime { get; set; }
        public string PRYR_EndTime { get; set; }
        public string PRYR_ProgramName { get; set; }
        public string PRYRA_ActivityName { get; set; }
        public string PRMTLE_ProgramLevel { get; set; }
        public string PRMTY_ProgramType { get; set; }
        public OnlineProgramReport_DTO[] selectedyearlist { get; set; }
        public OnlineProgramReport_DTO[] selectedtypelist { get; set; }
        public OnlineProgramReport_DTO[] selectedactivitylist { get; set; }
        public OnlineProgramReport_DTO[] selectedlevellist { get; set; }
        public OnlineProgramReport_DTO[] selecteddepartmentlist { get; set; }

    }
}
