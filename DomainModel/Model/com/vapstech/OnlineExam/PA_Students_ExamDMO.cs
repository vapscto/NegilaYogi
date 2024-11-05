using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.OnlineExam
{
    [Table("PA_Students_Exam")]
     public  class PA_Students_ExamDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


        public long PASTE_Id { get; set; }
     // public long PAMOEQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long PASR_Id { get; set; }
        public DateTime PASTE_Date { get; set; }
        public string PASTE_TotalTime { get; set; }
        public long? PASTE_TotalQnsAnswered { get; set; }
        public long? PASTE__TotalCorrectAns { get; set; }
        public long? PASTE_TotalMaxMarks { get; set; }
        public long? PASTE_TotalMarks { get; set; }
        public decimal? PASTE_Percentage { get; set; }
        public List<PA_Students_Exam_AnswerDMO> PA_Students_Exam_AnswerDMO { get; set; }
    }
}
