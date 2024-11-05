using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam.LessonPlanner
{
    public class CollegeStaffperiodtransactionreportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long Userid { get; set; }
        public long roleId { get; set; }
        public string rolename { get; set; }
        public string username { get; set; }
        public string employeename { get; set; }
        public Array getyear { get; set; }
        public Array getcourse { get; set; }
        public Array getbranch { get; set; }
        public Array getsemester { get; set; }
        public Array getsection { get; set; }
        public Array getemployee { get; set; }
        public Array getreport { get; set; }
        public Array getreportemployee { get; set; }

        public Array getdevationreport { get; set; }
        public Array getdevationreportemployee { get; set; }
    }
}
