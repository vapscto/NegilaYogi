using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.LeaveManagement
{
    public class HR_Leave_Policy_Config_DTO
    {
        public long HRLPC_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRLPC_LeavePolicyName { get; set; }
        public string HRLPC_SPName { get; set; }
        public string HRLPC_ServiceName { get; set; }
        public bool HRLPC_LateInFlag { get; set; }
        public string HRLPC_LateInTime { get; set; }
        public bool HRLPC_EarlyOutFlag { get; set; }
        public string HRLPC_EarlyOutTime { get; set; }
        public bool HRLPC_CummulativeTimeFlag { get; set; }
        public string HRLPC_CummulativeTime { get; set; }
        public string HRLPC_AfterCummulativeTime { get; set; }
        public int HRLPC_NoOfLates { get; set; }
        public bool HRLPC_NoOfLatesFag { get; set; }
        public bool HRLPC_NoOfLatesCFFlag { get; set; }
        public bool HRLPC_LeavePrefixSuffixFlag { get; set; }
        public bool HRLPC_IncludeHolidayFlag { get; set; }
        public bool HRLPC_LateLOPFlag { get; set; }
        public bool HRLPC_LateLeaveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Array config_data { get; set; }
        public bool returnval { get; set; }
        public string HRLPC_SpOrGen { get; set; }
        public bool HRLPC_AbsentLOPFlag { get; set; }
        public bool HRLPC_AbsentLeaveFlag { get; set; }
        public long LoginId { get; set; }
    }
}
