using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Principal
{
    [Table("IVRM_Principal_Class")]
    public class IVRM_Principal_ClassDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long IPRC_Id { get; set; }
        public long IPR_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public bool IRPC_ActiveFlag { get; set; }
    }
}
