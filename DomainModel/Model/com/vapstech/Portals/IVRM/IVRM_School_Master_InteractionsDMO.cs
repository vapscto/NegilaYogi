using DomainModel.Model.com.vapstech.admission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.IVRM
{
    [Table("IVRM_School_Master_Interactions")]
    public class IVRM_School_Master_InteractionsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMINT_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMINT_InteractionId { get; set; }
        public long ASMAY_Id { get; set; }
        public string ISMINT_ComposedByFlg { get; set; }
        public string ISMINT_GroupOrIndFlg { get; set; }
        public long ISMINT_ComposedById { get; set; }
        public string ISMINT_Subject { get; set; }
        public DateTime ISMINT_DateTime { get; set; }
        public string ISMINT_Interaction { get; set; }
        public bool ISMINT_ActiveFlag { get; set; }
        public long ISMINT_CreatedBy { get; set; }
        public long ISMINT_UpdatedBy { get; set; }
        public string ISMINT_Attachment { get; set; }
        public string ISMINT_ISPIPAddress { get; set; }
        public string ISMINT_MACAddress { get; set; }

        public List<IVRM_School_Transaction_InteractionsDMO> IVRM_School_Transaction_InteractionsDMO { get; set; }


    }
}

