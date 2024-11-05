using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.OnlineExam
{
    [Table("LMS_Students_Exam_College")]
     public  class LMS_Students_Exam_CollegeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


        public long LMSSTECO_Id { get; set; }
       // public long LMSMOEQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_ID { get; set; }
        public DateTime LMSSTECO_Date { get; set; }
        public long? LMSSTECO_TotalTime { get; set; }
        public long? LMSSTECO_TotalQnsAnswered { get; set; }
        public long? LMSSTECO_TotalCorrectAns { get; set; }
        public long? LMSSTECO_TotalMaxMarks { get; set; }
        public long? LMSSTECO_TotalMarks { get; set; }
        public decimal? LMSSTECO_Percentage { get; set; }
        public List<LMS_Students_Exam_Answer_CollegeDMO> LMS_Students_Exam_Answer_CollegeDMO { get; set; }
    }
}
