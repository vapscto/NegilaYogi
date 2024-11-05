using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class HR_Training_Create_Participants_DTO
    {
        public long HRTCRP_Id { get; set; }
        public long HRTCR_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool HRTCRP_ActiveFlg { get; set; }
    }
}
