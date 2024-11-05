using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.OnlineExam
{
    [Table("LMS_Students_Exam_Answer_College")]
     public  class LMS_Students_Exam_Answer_CollegeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long LMSSTEACO_Id { get; set; }
        [ForeignKey("LMSSTECO_Id")]
        public long LMSSTECO_Id { get; set; }
        [ForeignKey("LMSMOEQ_Id")]
        public long LMSMOEQ_Id { get; set; }
        public long LMSMOEQOA_Id { get; set; }
        public bool LMSSTEACO_CorrectAnsFlg { get; set; }
       // public List<LMS_Students_Exam_AnswerDMO> LMS_Students_Exam_AnswerDMO { get; set; }
    }
}
