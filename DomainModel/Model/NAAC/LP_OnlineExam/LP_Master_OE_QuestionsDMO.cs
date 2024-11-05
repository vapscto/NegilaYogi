using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Master_OE_Questions")]
    public class LP_Master_OE_QuestionsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPMOEQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long LPMT_Id { get; set; }
        public long? LPMCOMP_Id { get; set; }
        public decimal? LPMOEQ_Marks { get; set; }
        public string LPMOEQ_Question { get; set; }
        public string LPMOEQ_QuestionDesc { get; set; }
        public bool LPMOEQ_ActiveFlg { get; set; }
        public long LPMOEQ_CreatedBy { get; set; }
        public long LPMOEQ_UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long LPMOEQ_NoOfOptions { get; set; }
        public bool? LPMOEQ_SubjectiveFlg { get; set; }
        public bool? LPMOEQ_MatchTheFollowingFlg { get; set; }
        public string LPMOEQ_StructuralFlg { get; set; }
        public string LPMOEQ_Answer { get; set; }
        public int? LPMOEQ_MFRowCount { get; set; }
        public int? LPMOEQ_MFColumnCount { get; set; }
        public List<LP_Master_OE_Questions_FilesDMO> LP_Master_OE_Questions_FilesDMO { get; set; }
        public List<LP_Master_OE_QNS_OptionsDMO> LP_Master_OE_QNS_OptionsDMO { get; set; }
    }
}