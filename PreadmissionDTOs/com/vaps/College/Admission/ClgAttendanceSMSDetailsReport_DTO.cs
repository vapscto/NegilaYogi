using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
   public class ClgAttendanceSMSDetailsReport_DTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public Array yearlist { get; set; }
        public Array getcourse { get; set; }
        public Array getbranch { get; set; }
        public Array getsemester { get; set; }
        public Array sectionlist { get; set; }
        public Array reportlist { get; set; }
        public ClgAttendanceSMSDetailsReport_DTO[] selectedbranchlist { get; set; }
        public ClgAttendanceSMSDetailsReport_DTO[] selectedsemesterlist { get; set; }
    }
}
