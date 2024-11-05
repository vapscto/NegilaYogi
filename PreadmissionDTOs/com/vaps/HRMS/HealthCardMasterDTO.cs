using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
   public  class HealthCardMasterDTO
    {
        public long HMTPD_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HMTPD_MemberId { get; set; }
        public DateTime? HMTPD_PlanStartDate { get; set; }
        public DateTime? HMTPD_PlanEndDate { get; set; }
        public string HMTPD_PolicyName { get; set; }
        public string HMTPD_PlanName { get; set; }
        public string HMTPD_PolicyProvider { get; set; }
        public bool HMTPD_ActiveFlag { get; set; }             
        public long UserId { get; set; }
        public string return_val { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public long HMTRSCD_Id { get; set; }
        public Array editarray { get; set; }

    }
}
