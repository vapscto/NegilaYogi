using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.LeaveManagement
{
    public class EmployeeOnDutyReportDTO
    {
        public Array employeedropdown { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public long MI_Id { get; set; }
        
        public long HRELAP_Id { get; set; }
       
        public EmployeeOnDutyReportDTO[] employeelist { get; set; }
        public Array employeeDetails { get; set; }
        public DateTime? HRELAP_FromDate { get; set; }
        public DateTime? HRELAP_ToDate { get; set; }
        public string HRELAPD_InTime { get; set; }
        public string HRELAPD_OutTime { get; set; }
        public string HRELAP_ApplicationStatus{ get; set; }
        public string HRML_LeaveName { get; set; }
        public string HRML_LeaveCode { get; set; }
        public string HRELAPA_SanctioningLevel { get; set; }
        public Array Empreport { get; set; }
        public string retrunMsg { get; set; }

    }
}
