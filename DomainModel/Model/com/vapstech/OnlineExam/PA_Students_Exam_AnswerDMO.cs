using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.OnlineExam
{
    [Table("PA_Students_Exam_Answer")]
    public class PA_Students_Exam_AnswerDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PASTEA_Id { get; set; }
        [ForeignKey("PASTE_Id")]
        public long PASTE_Id { get; set; }
        public long PAMOEQOA_Id { get; set; }
        public long PAMOEQ_Id { get; set; }
        public bool? PAMOEQOA_CorrectAnsFlg { get; set; }

    }
}
