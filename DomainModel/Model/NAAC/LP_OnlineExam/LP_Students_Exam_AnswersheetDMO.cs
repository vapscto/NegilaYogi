using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Students_Exam_Answersheet")]
    public class LP_Students_Exam_AnswersheetDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPSTUEXAS_Id { get; set; }
        public long LPSTUEX_Id { get; set; }
        public string LPSTUEXAS_AnswerSheetFile { get; set; }
        public string LPSTUEXAS_AnswerSheetPath { get; set; }
        public string LPSTUEXAS_StaffOrStudentUploadFlag { get; set; }
        public bool? LPSTUEXAS_ActiveFlg { get; set; }
        public long LPSTUEXAS_CreatedBy { get; set; }
        public DateTime? LPSTUEXAS_CreatedDate { get; set; }
        public long LPSTUEXAS_UpdatedBy { get; set; }
        public DateTime? LPSTUEXAS_UpdatedDate { get; set; }
    }
}
