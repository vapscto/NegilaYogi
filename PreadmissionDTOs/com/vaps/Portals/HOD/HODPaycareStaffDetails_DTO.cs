using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.HOD
{
   public class HODPaycareStaffDetails_DTO:CommonParamDTO
    {

        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long user_id { get; set; }
        public long HRMDES_Id { get; set; }
        public long hrmd_id { get; set; }
        public string grouptypeName { get; set; }
        public string departmentName { get; set; }
        public string designationname { get; set; }
        public int depttotalEmployees { get; set; }       
        public long totalEmployees { get; set; }
        public long HRME_Id { get; set; }
        public long Emp_Code { get; set; }

        public Array employeeDetails { get; set; }
        public Array departmentdropdown { get; set; }
        public Array departmentgraph { get; set; }
        public Array filldesiganation { get; set; }



    }
}
