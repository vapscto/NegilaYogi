using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class VikasaLUReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public long AMAY_RollNo { get; set; }
        public long EME_Id { get; set; }
        public DateTime AMST_DOB { get; set; }
        public Array Acdlist { get; set; }
        public Array ctlist { get; set; }
        public Array seclist { get; set; }
        public Array studentlist { get; set; }
        public Array examlist { get; set; }
        public Array datareport { get; set; }
        public Array classteacher { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string EMSS_SubSubjectName { get; set; }
        public long ISMS_Id { get; set; }
        public int EMSS_Id { get; set; }
        public string ESTMPSSS_ObtainedGrade { get; set; }
        public string ESTMPSSS_PassFailFlg { get; set; }
        public string overallgrade { get; set; }
        public Array attendance { get; set; }
        public Array sports { get; set; }
    }
}
