using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Training_Feedback")]
    public class HR_Training_Feedback_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRTFEED_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRTCR_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRTFEED_IntORExtFlg { get; set; }
        public long HRTFEED_TrainerId { get; set; }
        public long HRMFQNS_Id { get; set; }
        public long HRMFOPT_Id { get; set; }
        public string HRTFEED_AboutTraining { get; set; }
        public string HRTFEED_Improvement { get; set; }
        public string HRTFEED_Response { get; set; }
        public bool HRTFEED_ActiveFlg { get; set; }
        public long HRTFEED_CreatedBy { get; set; }
        public long HRTFEED_UpdatedBy { get; set; }
        public DateTime? HRTFEED_CreatedDate { get; set; }
        public DateTime? HRTFEED_UpdatedDate { get; set; }
    }
}
