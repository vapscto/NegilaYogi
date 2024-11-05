using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Training_Create_IntTrainer")]
    public class HR_Training_Create_IntTrainer_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRTCRINTTR_Id { get; set; }
        public long HRTCR_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRTCRINTTR_TrainingDesc { get; set; }
        public bool HRTCRINTTR_ActiveFlg { get; set; }
        public int HRTCRINTTR_Rating { get; set; }
        public DateTime? HRTCRINTTR_StartDate { get; set; }
        public DateTime? HRTCRINTTR_EndDate { get; set; }
        public string HRTCRINTTR_StartTime { get; set; }
        public string HRTCRINTTR_EndTime { get; set; }
        public long HRMTT_Id { get; set; }
    }
}
