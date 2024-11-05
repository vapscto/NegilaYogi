using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Emp_Leave_Trans")]
    public class HR_Emp_Leave_Trans_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


        public long HRELT_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMLY_Id { get; set; }
        public long HRELT_LeaveId { get; set; }
        public DateTime HRELT_FromDate { get; set; }
        public DateTime HRELT_ToDate { get; set; }
        public decimal HRELT_TotDays { get; set; }
        public DateTime HRELT_Reportingdate { get; set; }
        public string HRELT_LeaveReason { get; set; }
        public string HRELT_Status { get; set; }
        public bool HRELT_ActiveFlag { get; set; }
        public string HRELT_SupportingDocument { get; set; }
    }
}
