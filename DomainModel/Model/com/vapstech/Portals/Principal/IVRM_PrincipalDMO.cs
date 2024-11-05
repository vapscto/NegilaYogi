using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Principal
{
    [Table("IVRM_Principal")]
    public class IVRM_PrincipalDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long IPR_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public bool IPR_ActiveFlag{ get; set; }

        public List<IVRM_Principal_ClassDMO> IVRM_Principal_ClassDMO { get; set; }
        public List<IVRM_Principal_StaffDMO> IVRM_Principal_StaffDMO { get; set; }
    }
}
