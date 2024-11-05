using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.FeedBack
{
    [Table("Feedback_College_StudentToStaff", Schema = "CLG")]
    public class Feedback_College_StudentToStaffDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FCSTST_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public DateTime FCSTST_FeedbackDate { get; set; }
        public long FMTY_Id { get; set; }
        public long FMQE_Id { get; set; }
        public long FMOP_Id { get; set; }
        public bool FCSTST_ActiveFlag { get; set; }
        public long FCSTST_CreatedBy { get; set; }
        public long FCSTST_UpdatedBy { get; set; }
        public string FCSTST_FeedBack { get; set; }
    }
}
