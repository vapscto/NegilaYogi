using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Master_Feedback_Qns")]
    public class HR_Master_Feedback_Qns_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMFQNS_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMFQNS_QuestionName { get; set; }
        public string HRMFQNS_QuestionTypeFlg { get; set; }
        public int HRMFQNS_QuestionOrder { get; set; }
        public bool HRMFQNS_ActiveFlg { get; set; }
        public long? HRMFQNS_CreatedBy { get; set; }
        public long? HRMFQNS_UpdatedBy { get; set; }
        public DateTime? HRMFQNS_CreatedDate { get; set; }
        public DateTime? HRMFQNS_UpdatedDate { get; set; }
        public string HRMFQNS_QuestionForFlg { get; set; }
    }
}
