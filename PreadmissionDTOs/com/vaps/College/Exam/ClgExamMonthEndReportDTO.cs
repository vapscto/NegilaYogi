using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam
{
    public class ClgExamMonthEndReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long EME_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long Pass { get; set; }
        public long Fail { get; set; }
        public string ASMAY_Year { get; set; }
        public string EME_ExamName { get; set; }
        public long IVRM_Month_Id { get; set; }
        public string IVRM_Month_Name { get; set; }

        public Array Acdlist { get; set; }
        public Array examlist { get; set; }
        public Array monthlist { get; set; }
        public Array datareport { get; set; }
    }
}
