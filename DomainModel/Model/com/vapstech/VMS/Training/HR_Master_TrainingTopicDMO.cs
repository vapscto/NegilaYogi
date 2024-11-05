using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Master_TrainingTopic")]
    public class HR_Master_TrainingTopicDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMTT_Id { get; set; }
        public string HRMTT_Topic { get; set; }
        public bool HRMTT_ActiveFlg { get; set; }
        public long HRMTT_CreatedBy { get; set; }
        public long HRMTT_UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
