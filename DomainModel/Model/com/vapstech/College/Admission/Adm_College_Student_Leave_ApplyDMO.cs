using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Students_Leave_Apply", Schema = "CLG")]
    public class Adm_College_Student_Leave_ApplyDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSLA_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ACSLA_LeaveId { get; set; }
        public string ACSLA_Reason { get; set; }
        public DateTime ACSLA_ApplyDate { get; set; }
        public DateTime ACSLA_FromDate { get; set; }
        public DateTime ACSLA_ToDate { get; set; }
        public string ACSLA_Status { get; set; }
        public bool ACSLA_Flag { get; set; }
        public long MI_Id { get; set; }
    }
}
