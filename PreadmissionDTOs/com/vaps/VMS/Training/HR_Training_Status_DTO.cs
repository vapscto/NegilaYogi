using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class HR_Training_Status_DTO
    {
        public long HRTSTS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRTCR_Id { get; set; }
        public string HRTSTS_IntORExtFlg { get; set; }
        public long HRTSTS_TrainerId { get; set; }
        public long HRME_Id { get; set; }
        public string HRTSTS_Status { get; set; }
        public long HRTSTS_Rating { get; set; }
        public string HRTSTS_TrainerRemarks { get; set; }
        public bool HRTSTS_ActiveFlg { get; set; }
    }
}

