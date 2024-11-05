using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class PassFailReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EMCA_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string Exam { get; set; }
        public int No_of_times_Failed { get; set; }
        public long EME_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ESTMP_Result { get; set; }

        public Array Acdlist { get; set; }
        public Array catlist { get; set; }
        public Array ctlist { get; set; }
        public Array seclist { get; set; }
        public Array examlist { get; set; }
        public Array studentlist { get; set; }
        public Array datareport { get; set; }
        public Array datareport2 { get; set; }
        public Array datareport3 { get; set; }
        public Array datareport4 { get; set; }
        public Array getinstitution { get; set; }
        public long Strength { get; set; }
        public long Passed { get; set; }
        public long Failed { get; set; }
        public string report_type { get; set; }
        public decimal? ESTMPS_ObtainedMarks { get; set; }
        public decimal? ESTMPS_MaxMarks { get; set; }
    }
}
