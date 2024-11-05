using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class PercentagewiseDetailsReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EMCA_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long EME_Id { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public decimal? ESTMP_TotalMaxMarks { get; set; }
        public decimal? ESTMP_TotalObtMarks { get; set; }
        public decimal? ESTMP_Percentage { get; set; }

        public Array Acdlist { get; set; }
        public Array catlist { get; set; }
        public Array ctlist { get; set; }
        public Array seclist { get; set; }
        public Array examlist { get; set; }
        public Array studentlist { get; set; }
        public Array datareport { get; set; }
        public Array institution { get; set; }
        public string report_type { get; set; }
        public string percent { get; set; }
    }
}
