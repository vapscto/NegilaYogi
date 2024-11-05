using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.FeedBack
{
    [Table("Feedback_Staff_Transaction")]
    public class Feedback_Staff_TransactionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FSTTR_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime FSSTR_FeedbackDate { get; set; }
        public long FMTY_Id { get; set; }
        public long FMQE_Id { get; set; }
        public long FMOP_Id { get; set; }
        public bool FSTTR_ActiveFlag { get; set; }
        public string FSTTR_FeedBack { get; set; }
        public long FSTTR_CreatedBy { get; set; }
        public long FSTTR_UpdatedBy { get; set; }

    }
}
