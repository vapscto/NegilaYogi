using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
  public  class SMSemailStaffReportDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public Array getDepartment { get; set; }
        public Array getdesination { get; set; }
        public departmentListone[] departmentOne { get; set; }
    }
    public class departmentListone
    {
        public long HRMD_Id { get; set; }
    }
}
