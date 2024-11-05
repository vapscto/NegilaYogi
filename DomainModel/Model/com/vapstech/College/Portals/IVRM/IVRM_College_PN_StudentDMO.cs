using DomainModel.Model.com.vapstech.Portals.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Portals.IVRM
{
    [Table("IVRM_College_PN_Student")]
    public class IVRM_College_PN_StudentDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ICPNS_Id { get; set; }
        public long IPN_Id { get; set; }
        public long AMCST_Id { get; set; }     
        public bool ICPNS_ActiveFlag { get; set; }

        public IVRM_PushNotificationDMO IVRM_PushNotificationDMO { get; set; }
    }
}
