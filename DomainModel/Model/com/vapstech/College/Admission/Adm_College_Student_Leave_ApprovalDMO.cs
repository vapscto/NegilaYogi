using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Students_Leave_Approval", Schema = "CLG")]
    public class Adm_College_Student_Leave_ApprovalDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSLAP_Id { get; set; }
        public long ACSLA_Id { get; set; }
        public long? IVRMALU_Id { get; set; }
        public DateTime? ACSLAP_AppRejDate { get; set; }
        public DateTime? ACSLAP_AppFromDate { get; set; }
        public DateTime? ACSLAP_AppToDate { get; set; }
        public string ACSLAP_LeaveStatus { get; set; }
        public string ACSALP_RejectReason { get; set; }
    }
}
