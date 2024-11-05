using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Stu_MP_Promo_Subjectwise", Schema = "Exm")]
    public class Exm_Stu_MP_Promo_SubjectwiseDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ESTMPPS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ISMS_Id { get; set; }
        //public int EME_Id { get; set; }
        public decimal? ESTMPPS_MaxMarks { get; set; }
        public decimal? ESTMPPS_ObtainedMarks { get; set; }
        public string ESTMPPS_ObtainedGrade { get; set; }        
        public decimal? ESTMPPS_ClassAverage { get; set; }
        public decimal? ESTMPPS_SectionAverage { get; set; }
        public decimal? ESTMPPS_ClassHighest { get; set; }
        public decimal? ESTMPPS_SectionHighest { get; set; }
        public string ESTMPPS_PassFailFlg { get; set; }
        public string ESTMPPS_Remarks { get; set; }
        public decimal? ESTMPPS_GradePoints { get; set; }
        public int? EMGD_Id { get; set; }
        public int? ESTMPPS_ClassRank { get; set; }
        public int? ESTMPPS_SectionRank { get; set; }
        public List<Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO> Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO { get; set; }
    }
}
