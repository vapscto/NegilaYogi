using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class StaffCompliantsDTO
    {       
        public long HREREM_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREREM_Subject { get; set; }
        public string HREREM_Remarks { get; set; }
        public DateTime? HREREM_Date { get; set; }
        public string HREREM_FileName { get; set; }
        public string HREREM_FilePath { get; set; }
        public long HREREM_RemarksBy { get; set; }
        public bool? HREREM_ActiveFlg { get; set; }       
        public long UserId { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime ? ToDate { get; set; }
        public DateTime? HREREM_CreatedDate { get; set; }
        public Array getemployeelist { get; set; }
        public Array getemployeedetails { get; set; }
        public Array getemployeesaveddetails { get; set; }
        public Array getsavedetails { get; set; }
        public Array geteditemployeedetails { get; set; }
        public Array editlist { get; set; }
        public Array getreportdetails { get; set; }
    }
}