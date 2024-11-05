using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.FeedBack
{
    [Table("Feedback_Master_Questions")]
    public class Feedback_Master_QuestionsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMQE_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMQE_FeedbackQuestions { get; set; }
        public string FMQE_FeedbackQRemarks { get; set; }
        public int FMQE_FQOrder { get; set; }
        public bool FMQE_ActiveFlag { get; set; }
        public bool FMQE_ManualEntryFlg { get; set; }
        public long FMQE_CreatedBy { get; set; }
        public long FMQE_UpdatedBy { get; set; }

    }
}
