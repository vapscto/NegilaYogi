using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.FeedBack
{
    [Table("Feedback_School_StudentToStaff")]
    public class Feedback_School_StudentToStaffDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FSSTST_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public DateTime FSSTST_FeedbackDate { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long FMTY_Id { get; set; }
        public long FMQE_Id { get; set; }
        public long FMOP_Id { get; set; }
        public bool FSSTST_ActiveFlag { get; set; }
        public long FSSTST_CreatedBy { get; set; }
        public long FSSTST_UpdatedBy { get; set; }
        public string FSSTST_FeedBack { get; set; }
    }

}
