using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Emp_Leave_Application_Deputation")]
   public class HR_Emp_Leave_Application_DeputationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long  HRELAPDD_Id { get; set; }
        public long HRELAPD_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime HRELAPDD_Date { get; set; }
        public string HRELAPDD_Period { get; set; }
        public string HRELAPDD_ApprovalFlg { get; set; }
        public string HRELAPDD_Remarks { get; set; }
        public bool HRELAPDD_ActiveFlag{ get; set; }
        public DateTime? HRELAPDD_CreatedDate { get; set; }
        public DateTime? HRELAPDD_UpdatedDate { get; set; }
        public long HRELAPDD_CreatedBy { get; set; }
        public long HRELAPDD_UpdatedBy { get; set; }
    }
}
