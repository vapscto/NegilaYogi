using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.FeedBack
{
    [Table("Feedback_College_Student_Transaction" , Schema = "CLG")]
    public class Feedback_College_Student_TransactionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FCSTR_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public DateTime FCSTR_FeedbackDate { get; set; }
        public long FMTY_Id { get; set; }
        public long FMQE_Id { get; set; }
        public long FMOP_Id { get; set; }
        public string FCSTR_StudParFlg { get; set; }
        public string FCSTR_FeedBack { get; set; }
        public bool FCSTR_ActiveFlag { get; set; }
        public long FCSTR_CreatedBy { get; set; }
        public long FCSTR_UpdatedBy { get; set; }     
    }
}
