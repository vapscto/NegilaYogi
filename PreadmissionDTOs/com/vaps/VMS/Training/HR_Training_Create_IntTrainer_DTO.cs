using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class HR_Training_Create_IntTrainer_DTO
    {
        public long HRTCRINTTR_Id { get; set; }
        public long HRTCR_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRTCRINTTR_TrainingDesc { get; set; }
        public bool HRTCRINTTR_ActiveFlg { get; set; }
        public DateTime HRTCRINTTR_StartDate { get; set; }
        public DateTime HRTCRINTTR_EndDate { get; set; }
        public string HRTCRINTTR_StartTime { get; set; }
        public string HRTCRINTTR_EndTime { get; set; }
    }
}
