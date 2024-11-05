using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.FeedBack
{
    [Table("Feedback_Alumni_Transaction")]
    public class Feedback_Alumni_TransactionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FALTR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ALMST_Id { get; set; }
        public DateTime FALTR_FeedbackDate { get; set; }
        public long FMTY_Id { get; set; }
        public long FMQE_Id { get; set; }
        public long FMOP_Id { get; set; }
        public bool FALTR_ActiveFlag { get; set; }
        public long FALTR_CreatedBy { get; set; }
        public long FALTR_UpdatedBy { get; set; }
        public string FALTR_FeedBack { get; set; }

    }
}
