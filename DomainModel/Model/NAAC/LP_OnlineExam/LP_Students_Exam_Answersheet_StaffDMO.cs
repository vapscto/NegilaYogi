using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Students_Exam_Answersheet_Staff")]
    public class LP_Students_Exam_Answersheet_StaffDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPSTUEXASTF_Id { get; set; }
        public long LPSTUEX_Id { get; set; }
        public string LPSTUEXASTF_AnswerSheetFile { get; set; }
        public string LPSTUEXASTF_AnswerSheetPath { get; set; }
        public bool LPSTUEXASTF_ActiveFlg { get; set; }
        public long LPSTUEXASTF_CreatedBy { get; set; }
        public DateTime? LPSTUEXASTF_CreatedDate { get; set; }
        public long LPSTUEXASTF_UpdatedBy { get; set; }
        public DateTime? LPSTUEXASTF_UpdatedDate { get; set; }
    }
}
