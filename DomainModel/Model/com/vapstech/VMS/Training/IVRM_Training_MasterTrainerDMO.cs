using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("IVRM_Training_MasterTrainer")]
    public class IVRM_Training_MasterTrainerDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


        public long IVRMTMT_Id { get; set; }
        public long IVRMTT_Id { get; set; }
        public string IVRMTMT_TrainerName { get; set; }
        public long? IVRMTMT_TrainerId { get; set; }
        public string IVRMTMT_TrainerEmail { get; set; }
        public string IVRMTT_TrainerPhone { get; set; }
        public string IVRMTMT_Status { get; set; }
        public string IVRMTMT_Feedback { get; set; }
        public bool? IVRMTMT_ActiveFlag { get; set; }
        public DateTime? IVRMTMT_CreatedDate { get; set; }
        public DateTime? IVRMTMT_UpdatedDate { get; set; }
        public long? IVRMTMT_CreatedBy { get; set; }
        public long? IVRMTMT_UpdatedBy { get; set; }
    }
}
