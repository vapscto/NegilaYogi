using DomainModel.Model.com.vapstech.Portals.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Portals.IVRM
{
    [Table("Adm_College_Student_GFeedback", Schema = "CLG")]
    public class Adm_College_Student_GFeedbackDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSGFE_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public string ACSGFE_Feedback { get; set; }
        public DateTime? ACSGFE_FeedbackDate { get; set; }
        public bool ACSGFE_ActiveFlag { get; set; }
        public long ACSGFE_CreatedBy { get; set; }
        public long ACSGFE_UpdatedBy { get; set; }

    }
}
