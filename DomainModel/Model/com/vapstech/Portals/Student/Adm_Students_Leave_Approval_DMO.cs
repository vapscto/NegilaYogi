using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Student
{
    [Table("Adm_Students_Leave_Approval")]
    public class Adm_Students_Leave_Approval_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long   ASLAP_Id { get; set; }
        public long ASLA_Id { get; set; }
        public DateTime? ASLAP_AppRejDate { get; set; }
        public DateTime? ASLAP_AppFromDate { get; set; }
        public DateTime? ASLAP_AppToDate { get; set; }
        public string ASLAP_LeaveStatus { get; set; }
        public string ASALP_RejectReason { get; set; }
        public long? IVRMALU_Id { get; set; }
        public long MI_Id { get; set; }

    }
}
