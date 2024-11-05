using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class StudentPerformanceReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EMCA_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public long EME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public decimal? ESTMPS_ObtainedMarks { get; set; }
        public decimal? ESTMPS_ClassHighest { get; set; }
        public decimal? ESTMPS_SectionHighest { get; set; }

        public Array Acdlist { get; set; }
        public Array catlist { get; set; }
        public Array ctlist { get; set; }
        public Array seclist { get; set; }
        public Array examlist { get; set; }
        public Array studentlist { get; set; }
        public Array sublist { get; set; }
        public Array showgraph { get; set; }

    }
}
