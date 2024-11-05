using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.OnlineExam
{
    [Table("LMS_Students_Exam")]
     public  class LMS_Students_ExamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


        public long LMSSTE_Id { get; set; }
      //  [ForeignKey("LMSMOEQ_Id")]
       // public long LMSMOEQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime LMSSTE_Date { get; set; }
        public long? LMSSTE_TotalTime { get; set; }
        public long? LMSSTE_TotalQnsAnswered { get; set; }
        public long? LMSSTE_TotalCorrectAns { get; set; }
        public long? LMSSTE_TotalMaxMarks { get; set; }
        public long? LMSSTE_TotalMarks { get; set; }
        public decimal? LMSSTE_Percentage { get; set; }
        public List<LMS_Students_Exam_AnswerDMO> LMS_Students_Exam_AnswerDMO { get; set; }

    }
}
