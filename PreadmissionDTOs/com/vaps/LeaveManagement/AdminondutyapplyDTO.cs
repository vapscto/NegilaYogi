using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.LeaveManagement
{
    public class AdminondutyapplyDTO
    {
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long UserId { get; set; }
        public long HRELAP_Id { get; set; }
        public Array leaveData { get; set; }
        public Array employeeList { get; set; }
        public string HRME_LeftFlag { get; set; }
        public bool HRME_ActiveFlag { get; set; }

        public Array EmployeeDeatils { get; set; }
        public string returnmsg { get; set; }
        public bool returnval { get; set; }
       public Array editresult { get; set; }
        public Array editresults { get; set; }
        public Array employee_list_dd { get; set; }
        public long HRML_Id { get; set; }
        public long asmay_id { get; set; }
        public string HRELAP_ApplicationID { get; set; }
        public DateTime? HRELAP_FromDate { get; set; }
        public DateTime? HRELAP_ToDate { get; set; }
        public string HRELT_SupportingDocument { get; set; }
        public string HRELAP_LeaveReason { get; set; }
        public decimal HRELAP_TotalDays { get; set; }
        public string HRELAPD_InTime { get; set; }
        public string HRELAPD_OutTime { get; set; }
        //---------------Leave Name---------------------
        public string HRML_LeaveName { get; set; }
        public string HRML_LeaveCode { get; set; }
        public string HRML_LeaveDetails { get; set; }
        public bool HRML_LeaveCreditFlg { get; set; }
        public string HRML_LeaveType { get; set; }

        public Array leave_name { get; set; }
        public Array commentlist { get; set; }
    }
}
