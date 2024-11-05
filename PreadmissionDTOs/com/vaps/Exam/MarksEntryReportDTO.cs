using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class MarksEntryReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long EME_Id { get; set; }
        public Array getyear { get; set; }
        public Array getclass { get; set; }
        public Array getsection { get; set; }
        public Array getexam { get; set; }
        public Array getreport { get; set; }


    }
}
