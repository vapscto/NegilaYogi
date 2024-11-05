using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.FeedBack
{
    [Table("Feedback_School_Student_Transaction")]
    public class Feedback_School_Student_TransactionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FSSTR_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public DateTime FSSTR_FeedbackDate { get; set; }
        public long FMTY_Id { get; set; }
        public long FMQE_Id { get; set; }
        public long FMOP_Id { get; set; }
       public string  FSSTR_StudParFlg { get; set; }
       public bool FSSTR_ActiveFlag { get; set; }
        public long FSSTR_CreatedBy { get; set; }
        public long FSSTR_UpdatedBy { get; set; }
        public string FSSTR_FeedBack { get; set; }
    }
}
