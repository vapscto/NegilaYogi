using DomainModel.Model.com.vapstech.Exam;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Stu_MP_Promo_Subjectwise_Groupwise", Schema = "Exm")]
    public class Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ESTMPPSG_Id { get; set; }
        [ForeignKey("ESTMPPS_Id")]
        public int ESTMPPS_Id { get; set; }
        public int EMPSG_Id { get; set; }
        public decimal? ESTMPPSG_GroupMaxMarks { get; set; }
        public decimal? ESTMPPSG_GroupObtMarks { get; set; }
        public string ESTMPPSG_GroupObtGrade { get; set; }
        public decimal? ESTMPPSG_GradePoints { get; set; }
        public decimal? ESTMPPSG_GroupTotalMarks { get; set; }
        public decimal? ESTMPPSG_GroupObtMarksOutOfGroupTotal { get; set; }
        public decimal? ESTMPPSG_ObtMarksOutOfSubjectMaxMarks { get; set; }
        public decimal? ESTMPPSG_GroupPercentage { get; set; }        
        public string ESTMPPSG_GroupMarksGrade { get; set; }
        public int? EMGD_Id { get; set; }
        public int? EMGD_Id_GroupTotalMarks { get; set; }
        public List<Exm_Stu_MP_Promo_Subject_Groupwise_ExamDMO> Exm_Stu_MP_Promo_Subject_Groupwise_ExamDMO { get; set; }

    }
}
