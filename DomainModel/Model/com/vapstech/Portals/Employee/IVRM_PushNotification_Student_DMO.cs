using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_PushNotification_Student")]
   public class IVRM_PushNotification_Student_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IPNS_Id { get; set; }
        public long IPN_Id { get; set; }
        public long AMST_Id { get; set; }
        public bool IPNS_ActiveFlag { get; set; }

    }
}
