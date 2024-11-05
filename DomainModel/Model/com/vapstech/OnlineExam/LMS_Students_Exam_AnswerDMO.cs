using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.OnlineExam
{
    [Table("LMS_Students_Exam_Answer")]
    public class LMS_Students_Exam_AnswerDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LMSSTEA_Id { get; set; }
        [ForeignKey("LMSSTE_Id")]
        public long LMSSTE_Id { get; set; }
        [ForeignKey("LMSMOEQ_Id")]
        public long LMSMOEQ_Id { get; set; }
        public long LMSMOEQOA_Id { get; set; }
        public bool LMSSTEA_CorrectAnsFlg { get; set; }

    }
}
