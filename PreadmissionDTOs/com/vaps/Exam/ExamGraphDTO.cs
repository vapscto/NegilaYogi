using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ExamGraphDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long EME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public decimal? ESTMPS_ClassAverage { get; set; }
        public string ASMC_SectionName { get; set; }
        public decimal? ESTMPS_SectionAverage { get; set; }
        public Array yearlist { get; set; }
        public Array seclist { get; set; }
        public Array classlist { get; set; }
        public Array exmstdlist { get; set; }
        public Array sublist { get; set; }
        public Array datareport { get; set; }
        public Array datareport1 { get; set; }
        public int EMCA_Id { get; set; }
        public string report_type { get; set; }
        public Array getclasssection { get; set; }
        public Array get_exam_list { get; set; }
        public Array get_marks_avg { get; set; }
        public Array Exm_Master_Category { get; set; }
        public Array studentwiseavg { get; set; }
        public Array instname { get; set; }
        public Array classteacher { get; set; }
        public decimal? ESTMP_Percentage { get; set; }
        public string studentname { get; set; }
        public int EME_ExamOrder { get; set; }
        public tempexamlist[] tempexamlist { get; set; }
        public Array getnewexamlist { get; set; }
    }
    public class tempexamlist
    {
        public long EME_Id { get; set; }
    }
}
