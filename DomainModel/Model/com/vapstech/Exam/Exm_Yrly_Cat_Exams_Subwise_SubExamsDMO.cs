using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Yrly_Cat_Exams_Subwise_SubExams", Schema = "Exm")]
    public class Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EYCESSE_Id { get; set; }
        [ForeignKey("EYCES_Id")]
        public int EYCES_Id { get; set; }
        public int EMSE_Id { get; set; }
        public int EMGR_Id { get; set; }
        public decimal? EYCESSE_MaxMarks { get; set; }
        public decimal? EYCESSE_MinMarks { get; set; }
        public bool EYCESSE_ExemptedFlg { get; set; }
        public decimal? EYCESSE_ExemptedPer { get; set; }
        public bool EYCESSE_ActiveFlg { get; set; }
        public int EYCESSE_SubExamOrder { get; set; }
        // public List<Exm_Yearly_Category_Group_SubjectsDMO> Exm_Yearly_Category_Group_Subjects { get; set; }
    }
}
