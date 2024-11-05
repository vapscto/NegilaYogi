using DomainModel.Model.NAAC.FeedBack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.FeedBack
{
    [Table("Feedback_Type_Questions")]
    public class Feedback_Type_QuestionsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMTQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMTY_Id { get; set; }
        public long FMQE_Id { get; set; }
        public int FMTQ_TQOrder { get; set; }
        public bool FMTQ_ActiveFlag { get; set; }
        public long FMTQ_CreatedBy { get; set; }
        public long FMTQ_UpdatedBy { get; set; }
        public List<Feedback_Type_Questions_OptionsDMO> Feedback_Type_Questions_OptionsDMO { get; set; }
    }
}
