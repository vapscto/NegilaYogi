using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Emp_Leave_Trans_Details")]
    public class HR_Emp_Leave_Trans_Details_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRELTD_Id { get; set; }
        public long HRME_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRELT_Id { get; set; }
        public long HRML_Id { get; set; }
        public DateTime HRELTD_FromDate { get; set; }
        public DateTime HRELTD_ToDate { get; set; }
        public decimal HRELTD_TotDays { get; set; }
        public bool? HRELTD_LWPFlag { get; set; }
    }
}
