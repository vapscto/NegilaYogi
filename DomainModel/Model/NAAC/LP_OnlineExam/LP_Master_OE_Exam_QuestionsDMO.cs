using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Master_OE_Exam_Questions")]
    public class LP_Master_OE_Exam_QuestionsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPMOEEXQNS_Id { get; set; }
        //public long LPMOEEX_Id { get; set; }
        public long? LPMOEQ_Id { get; set; }
        public decimal? LPMOEEXQNS_Marks { get; set; }
        public bool LPMOEEXQNS_ActiveFlg { get; set; }
        public long LPMOEEXQNS_CreatedBy { get; set; }
        public DateTime LPMOEEXQNS_CreatedDate { get; set; }
        public long LPMOEEXQNS_UpdatedBy { get; set; }
        public DateTime LPMOEEXQNS_UpdatedDate { get; set; }
        public long? LPMOEEXLVL_Id { get; set; }
        public long? LPMOEEXQNS_QnsOrder { get; set; }
        public string LPMOEEXQNS_Question { get; set; }
        public string LPMOEEXQNS_Answer { get; set; }
        public bool? LPMOEEXQNS_SubjectiveFlg { get; set; }
        public bool? LPMOEEXQNS_MatchTheFollowingFlg { get; set; }
        public long? LPMOEEXQNS_NoOfOptions { get; set; }
        public long? LPMOEEXQNS_NoOfRows { get; set; }
        public long? LPMOEEXQNS_NoOfColumns { get; set; }
        public string LPMOEEXQNS_QuestionType { get; set; }
        public List<LP_Master_OE_Exam_Questions_OptionsDMO> LP_Master_OE_Exam_Questions_OptionsDMO { get; set; }
        public List<LP_Master_OE_Exam_Questions_FilesDMO> LP_Master_OE_Exam_Questions_FilesDMO { get; set; }
    }
}