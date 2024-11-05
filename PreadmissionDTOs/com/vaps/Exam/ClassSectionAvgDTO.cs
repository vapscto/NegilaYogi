using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ClassSectionAvgDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EMCA_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long EME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public decimal? ESTMPS_ClassAverage { get; set; }
        public decimal? ESTMPS_SectionAverage { get; set; }

        public Array Acdlist { get; set; }
        public Array catlist { get; set; }
        public Array ctlist { get; set; }
        public Array seclist { get; set; }
        public Array examlist { get; set; }
        public Array sublist { get; set; }
        public Array datareport { get; set; }
        public string report_type { get; set; }
        public string check_type { get; set; }
    }
}
