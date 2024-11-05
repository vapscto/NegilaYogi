using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Student_MP_Promotion", Schema = "Exm")]
    public class Exm_Student_MP_PromotionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ESTMPP_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        //public int EME_Id { get; set; }
        public decimal? ESTMPP_TotalMaxMarks { get; set; }
        public decimal? ESTMPP_TotalObtMarks { get; set; }
        public decimal? ESTMPP_GraceMarks { get; set; }
        public decimal? ESTMPP_BonusMarks { get; set; }
        public decimal? ESTMPP_TotalMarks { get; set; }        
        public decimal? ESTMPP_Percentage { get; set; }
        public string ESTMPP_TotalGrade { get; set; }
        public int? ESTMPP_ClassRank { get; set; }
        public int? ESTMPP_SectionRank { get; set; }
        public string ESTMPP_Result { get; set; }
        public bool? ESTMPP_PublishToStudentFlg { get; set; }
        public decimal ? ESTMPP_GrandTotal { get; set; }
        public int? ESTMPP_SectionPosition { get; set; }
        public int? ESTMPP_ClassPosition { get; set; }
        public decimal? ESTMPP_GradePoints { get; set; }
        public int? EMGD_Id { get; set; }
    }
}
