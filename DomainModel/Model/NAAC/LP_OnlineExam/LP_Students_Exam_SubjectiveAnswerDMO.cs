using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Students_Exam_SubjectiveAnswer")]
    public class LP_Students_Exam_SubjectiveAnswerDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPSTUEXSANS_Id { get; set; }
        public long LPSTUEX_Id { get; set; }
        public long? LPMOEQ_Id { get; set; }
        public string LPSTUEXSANS_Answer { get; set; }
        public decimal? LPSTUEXSANS_Marks { get; set; }
        public string LPSTUEXANS_AttemptFlag { get; set; }
        public string LPSTUEXSANS_FileName { get; set; }
        public string LPSTUEXSANS_FilePath { get; set; }
        public long? HRME_Id { get; set; }
        public bool LPSTUEXANS_ActiveFlg { get; set; }
        public long LPSTUEXANS_CreatedBy { get; set; }
        public DateTime? LPSTUEXANS_CreatedDate { get; set; }
        public long LPSTUEXANS_UpdatedBy { get; set; }
        public DateTime? LPSTUEXANS_UpdatedDate { get; set; }
        public long? LPMOEEXQNS_Id { get; set; }
        public List<LP_Students_Exam_SubjectiveAnswer_FilesDMO> LP_Students_Exam_SubjectiveAnswer_FilesDMO { get; set; }
    }
}
