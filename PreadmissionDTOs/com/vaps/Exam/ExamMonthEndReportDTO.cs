using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ExamMonthEndReportDTO
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
        public string genderflage { get; set; }

        public Array Acdlist { get; set; }
        public Array examlist { get; set; }
        public Array monthlist { get; set; }
        public Array datareport { get; set; }

        public long male { get; set; }
        public long female { get; set; }
    }
}
