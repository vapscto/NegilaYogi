using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
   public  class HR_Training_Create_Details_DTO
    {
         public long HRTCRD_Id { get; set; }
        public long HRTCR_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRTCRD_TrainerHRME_Id { get; set; }
        public long HRMETR_Id { get; set; }
        public DateTime HRTCRD_Date { get; set; }
        public string HRTCRD_StartTime { get; set; }
        public string HRTCRD_EndTime { get; set; }
        public string HRTCRD_Content { get; set; }

        public string HRTCRD_Status { get; set; }
        public long HRTCRD_Rating { get; set; }
        public string HRTCRD_TrainerRemarks { get; set; }
        public bool HRTCRD_ActiveFlg { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public Array tname_list { get; set; }
    }
}
