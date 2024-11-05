using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Leave_Policy_Configuration")]
    public class HR_Leave_Policy_Config_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRLPC_Id { get; set; }
        public long MI_Id { get; set; }
        [ForeignKey("HRML_Id")]
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
        public string HRLPC_SpOrGen { get; set; }
        public bool HRLPC_AbsentLOPFlag { get; set; }
        public bool HRLPC_AbsentLeaveFlag { get; set; }
        public long? HRLPC_CreatedBy { get; set; }
        public long? HRLPC_UpdatedBy { get; set; }
        

    }
}

















