using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Master_Leave")]
    public class HR_Master_Leave_DMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRML_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRML_LeaveName { get; set; }
        public string HRML_LeaveCode { get; set; }
        public string HRML_LeaveDetails { get; set; }
        public bool HRML_LeaveCreditFlg { get; set; }
        public string HRML_LeaveType { get; set; }
        public bool HRML_LateDeductFlag { get; set; }
        public int HRML_LateDeductOrder { get; set; }
        public string HRML_WhenToApplyFlg { get; set; }
        public decimal? HRML_NoOfDays { get; set; }
    }
}
