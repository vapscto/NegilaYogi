using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam
{
    public class CollegeExamGeneralReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public string reporttype { get; set; }
        public int EMGR_Id { get; set; }
        public Array MasterGradeList { get; set; }
        public Array MasterInstitution { get; set; }
        public Array GradeList { get; set; }
        public Array GradeListDetails { get; set; }
    }
}
