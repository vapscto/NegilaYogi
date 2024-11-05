using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.FeedBack
{
    [Table("Feedback_Type_Options")]
    public class Feedback_Type_OptionsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMTO_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMTY_Id { get; set; }
        public long FMOP_Id { get; set; }
        public int FMTO_TQOrder { get; set; }
        public bool FMTO_ActiveFlag { get; set; }
        public long FMTO_CreatedBy { get; set; }
        public long FMTO_UpdatedBy { get; set; }

    }
}
