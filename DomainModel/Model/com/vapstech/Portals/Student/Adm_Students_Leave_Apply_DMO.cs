using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Student
{
    [Table("Adm_Students_Leave_Apply")]
    public class Adm_Students_Leave_Apply_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASLA_Id { get; set; }
        public string ASLA_Reason { get; set; }
        public int ASLA_LeaveId { get; set; }
        public DateTime ASLA_ApplyDate { get; set; }
        public DateTime ASLA_FromDate { get; set; }
        public DateTime ASLA_ToDate { get; set; }
        public string ASLA_Status { get; set; }
        public string ASLA_Flag { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime? ASLA_ApprovedFromDate { get; set; }
        public DateTime? ASLA_ApprovedToDate { get; set; }
        public bool ASLA_ActiveFlag { get; set; }


        public List<Adm_Students_Leave_Approval_DMO> Adm_Students_Leave_Approval_DMO { get; set; }

    }
}
