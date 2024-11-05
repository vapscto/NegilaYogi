using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.FeedBack
{
    [Table("Feedback_College_Alumni_Transaction", Schema = "CLG")]
    public class Feedback_CLG_Alumni_TransactionDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FCALTR_Id { get; set; }
        public long MI_Id { get; set; }

        public long FMTY_Id { get; set; }

        public long ALCSREG_Id { get; set; }
        public DateTime FCALTR_FeedbackDate { get; set; }
        public long FMQE_Id { get; set; }
        public long FMOP_Id { get; set; }
        public bool FCALTR_ActiveFlag { get; set; }
        public long FCALTR_CreatedBy { get; set; }
        public long FCALTR_UpdatedBy { get; set; }

        public string FCALTR_FeedBack { get; set; }
        public DateTime FCALTR_CreatedDate { get; set; }
        public DateTime FCALTR_UpdatedDate { get; set; }

        public long ASMAY_ID { get; set; }

    }
}
