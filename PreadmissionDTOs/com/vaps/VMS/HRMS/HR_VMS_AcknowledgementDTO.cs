using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
   public class HR_VMS_AcknowledgementDTO
    {
        public long asmaY_Id { get; set; }       
        public string stcrclstyrd { get; set; }
        public long MI_Id { get; set; }
        public Array yearlist { get; set; }
        public Array insti { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public Array year { get; set; }
        public string HRMC_QulaificationName { get; set; }
        public long UserId { get; set; }
        public Array mix { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
       // public Array institution { get; set; }
        public string MI_Name { get; set; }
       // public Array aaaa { get; set; }
        public string ASMAY_Year { get; set; }

    }
}
