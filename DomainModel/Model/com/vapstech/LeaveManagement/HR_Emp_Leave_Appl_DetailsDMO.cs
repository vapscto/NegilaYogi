using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Emp_Leave_Appl_Details")]
    public class HR_Emp_Leave_Appl_DetailsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRELAPD_Id { get; set; }
        [ForeignKey("HRELAP_Id")]
        public long HRELAP_Id { get; set; }
        [ForeignKey("HRML_Id")]
        public long HRML_Id { get; set; }
        public DateTime? HRELAPD_FromDate { get; set; }
        public DateTime? HRELAPD_ToDate { get; set; }
        public decimal HRELAPD_TotalDays { get; set; }
        public string HRELAPD_LeaveStatus { get; set; }
        public bool HRELAPD_ActiveFlag { get; set; }
        public long HRELAPD_UpdatedBy { get; set; }
        public long HRELAPD_CreatedBy { get; set; }
        public string HRELAPD_InTime { get; set; }
        public string HRELAPD_OutTime { get; set; }
    }

}
