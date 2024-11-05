using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.FeedBack
{
    [Table("Feedback_Master_Options")]
    public class Feedback_Master_OptionsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMOP_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMOP_FeedbackOptions { get; set; }
        public int FMOP_OptionsValue { get; set; }
        public string FMOP_FeedbackORemarks { get; set; }
        public int FMOP_FOOrder { get; set; }
        public bool FMOP_ActiveFlag { get; set; }
        public long FMOP_CreatedBy { get; set; }
        public long FMOP_UpdatedBy { get; set; }
    }
}
