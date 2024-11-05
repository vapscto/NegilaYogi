using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_PushNotification_Staff")]
    public class IVRM_PushNotification_Staff_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IPNST_Id { get; set; }
        public long IPN_Id { get; set; }
        public long HRME_Id { get; set; }
       public bool IPNST_ActiveFlag{ get; set; }

}
}
