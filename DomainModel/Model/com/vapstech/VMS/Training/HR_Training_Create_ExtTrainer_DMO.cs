using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Training_Create_ExtTrainer")]
    public class HR_Training_Create_ExtTrainer_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRTCREXTTR_Id { get; set; }
        public long HRTCR_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRTCREXTTR_TrainingDesc { get; set; }
        public bool HRTCREXTTR_ActiveFlg { get; set; }
        public int HRTCREXTTR_Rating { get; set; }
        public DateTime? HRTCREXTTR_StartDate { get; set; }
        public DateTime? HRTCREXTTR_EndDate { get; set; }
        public string HRTCREXTTR_StartTime { get; set; }
        public string HRTCREXTTR_EndTime { get; set; }
        public long HRMTT_Id { get; set; }
    }
}
