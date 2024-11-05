using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class HR_Training_Create_ExtTrainer_DTO
    {
        public long HRTCREXTTR_Id { get; set; }
        public long HRTCR_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRTCREXTTR_TrainingDesc { get; set; }
        public bool HRTCREXTTR_ActiveFlg { get; set; }
        public DateTime HRTCREXTTR_StartDate { get; set; }
        public DateTime HRTCREXTTR_EndDate { get; set; }
        public string HRTCREXTTR_StartTime { get; set; }
        public string HRTCREXTTR_EndTime { get; set; }
    }
}
