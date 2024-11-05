using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Student_Marks_Process_Subjectwise", Schema = "Exm")]
    public class ExmStudentMarksProcessSubjectwiseDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ESTMPS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EME_Id { get; set; }
        public decimal? ESTMPS_MaxMarks { get; set; }
        public decimal? ESTMPS_ConverionMaxMarks { get; set; }
        public decimal? ESTMPS_ObtainedMarks { get; set; }
        public decimal? ESTMPS_ConvertedMarks { get; set; }
        public decimal? ESTMPS_Medical_MaxMarks { get; set; }
        public string ESTMPS_ObtainedGrade { get; set; }
        public string ESTMPS_PassFailFlg { get; set; }
        public decimal? ESTMPS_ClassAverage { get; set; }
        public decimal? ESTMPS_SectionAverage { get; set; }
        public decimal? ESTMPS_ClassHighest { get; set; }
        public decimal? ESTMPS_SectionHighest { get; set; }
        public decimal? ESTMPS_GradePoints { get; set; }
        public bool? ESTMPS_AplResultFlg { get; set; }
        public int? EMGD_Id { get; set; }
        public int? ESTMPS_ClassRank { get; set; }
        public int? ESTMPS_SectionRank { get; set; }
        public int? EMPATY_Id { get; set; }
        public decimal? ESTMPS_Percentage { get; set; }
    }
}
