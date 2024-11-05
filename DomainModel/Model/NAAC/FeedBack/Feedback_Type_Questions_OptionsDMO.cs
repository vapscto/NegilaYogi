using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.NAAC.FeedBack
{
    [Table("Feedback_Type_Questions_Options")]
    public class Feedback_Type_Questions_OptionsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMTQO_Id { get; set; }
        public long FMTQ_Id { get; set; }
        public long FMOP_Id { get; set; }
        public int FMTQO_TQOOrder { get; set; }
        public bool FMTQO_ActiveFlag { get; set; }
        public long FMTQO_CreatedBy { get; set; }
        public long FMTQO_UpdatedBy { get; set; }
    }
}
