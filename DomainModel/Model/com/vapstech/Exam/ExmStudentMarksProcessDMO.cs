using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Student_Marks_Process", Schema = "Exm")]
    public class ExmStudentMarksProcessDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ESTMP_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public int EME_Id { get; set; }
        public decimal? ESTMP_TotalMaxMarks { get; set; }
        public decimal? ESTMP_TotalConverionMaxMarks { get; set; }
        public decimal? ESTMP_TotalObtMarks { get; set; }
        public decimal? ESTMP_TotalConvertedMarks { get; set; }
        public decimal? ESTMP_Percentage { get; set; }
        public string ESTMP_TotalGrade { get; set; }
        public int? ESTMP_ClassRank { get; set; }
        public int? ESTMP_SectionRank { get; set; }
        public string ESTMP_Result { get; set; }
        public decimal? ESTMP_BRPercentage { get; set; }
        public bool? ESTMP_PublishToStudentFlg { get; set; }
        public decimal? ESTMP_GrandTotal { get; set; }
        public int? ESTMP_SectionPosition { get; set; }
        public int? ESTMP_ClassPosition { get; set; }
        public int? EMGD_Id { get; set; }
        public decimal? ESTMP_GradePoints { get; set; }
        public string ESTMP_Points { get; set; }
    }
}
