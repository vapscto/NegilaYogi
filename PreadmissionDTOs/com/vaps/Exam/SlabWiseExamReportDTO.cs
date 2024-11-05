using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam
{
  public  class SlabWiseExamReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EME_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ISMS_ID { get; set; }
        public long TOMARKS { get; set; }
        public long FROMMARKS { get; set; }
        public string reporttype { get; set; }
        public Array subjectlist { get; set; }
        public Array getslabreport { get; set; }
    }
}
