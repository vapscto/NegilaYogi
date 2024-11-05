using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Master_Leave_Details_EnCash")]
    public class HR_Master_Leave_Details_EnCash_DMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRMLDEC_Id { get; set; }
        public long HRMLD_Id { get; set; }
        public bool HRMLDEC_ServiceAplFlg { get; set; }
        public string HRMLDEC_ServiceYear { get; set; }
        public string HRMLDEC_ServiceMonth { get; set; }
        public string HRMLDEC_ServiceDays { get; set; }
        public bool HRMLDEC_MaxLeaveFlg { get; set; }
        public int HRMLDEC_MaxLeaves { get; set; }
        public bool HRMLDEC_MinLeaveFlg { get; set; }
        public int HRMLDEC_MinLeaves { get; set; }
        public string HRMLDEC_ScheduleFlg { get; set; }
        public string HRMLDEC_ScheduleYear { get; set; }
        public string HRMLDEC_ScheduleMonth { get; set; }
        public bool HRMLDEC_VariableFixedFlg { get; set; }
        public decimal HRMLDEC_FixedAmount { get; set; }
        public long? HRMLDEC_CreatedBy { get; set; }
        public long? HRMLDEC_UpdatedBy { get; set; }
        

    }
}
