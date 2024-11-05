using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Principal
{

    [Table("IVRM_Principal_Staff")]
    public class IVRM_Principal_StaffDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long IPRS_Id { get; set; }
        public long IPR_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool IRPS_ActiveFlag { get; set; }
     
    }
}
