using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Exm_Stu_MP_Promo_Subject_Groupwise_Exam", Schema = "Exm")]
    public class Exm_Stu_MP_Promo_Subject_Groupwise_ExamDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESTMPPSGE_Id { get; set; }
        public int ESTMPPSG_Id { get; set; }
        public int EME_Id { get; set; }
        public decimal? ESTMPPSGE_ExamActualMaxMarks { get; set; }
        public decimal? ESTMPPSGE_ExamActualMarks { get; set; }
        public decimal? ESTMPPSGE_ExamConvertedMaxMarks { get; set; }
        public decimal? ESTMPPSGE_ExamConvertedMarks { get; set; }
        public string ESTMPPSGE_ExamConvertedGrade { get; set; }
        public string ESTMPPSGE_ExamPassFailFlag { get; set; }
        public decimal? ESTMPPSGE_ExamConvertedPoints { get; set; }
        public bool? ESTMPPSGE_ActiveFlg { get; set; }
        public int? EMGD_Id { get; set; }
    }
}
