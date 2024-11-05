using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Students_Exam_Answer")]
    public class LP_Students_Exam_AnswerDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LPSTUEXANS_Id { get; set; }
        public long LPSTUEX_Id { get; set; }
        public long? LPMOEQ_Id { get; set; }
        public long? LPMOEQOA_Id { get; set; }
        public long? LPMOEQOAMF_Id { get; set; }
        public bool LPSTUEXANS_CorrectAnsFlg { get; set; }
        public string LPSTUEXANS_AttemptFlag { get; set; }
        public bool LPSTUEXANS_ActiveFlg { get; set; }
        public long LPSTUEXANS_CreatedBy { get; set; }
        public long LPSTUEXANS_UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }        
        public long? LPMOEEXQNS_Id { get; set; }
        public long? LPMOEEXQNSOPT_Id { get; set; }
        public long? LPMOEEXQNSOPTMF_Id { get; set; }
        public decimal? LPSTUEXANS_Marks { get; set; }
    }
}
