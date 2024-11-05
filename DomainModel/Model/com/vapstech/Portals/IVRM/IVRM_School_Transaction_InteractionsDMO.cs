using DomainModel.Model.com.vapstech.admission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.IVRM
{
    [Table("IVRM_School_Transaction_Interactions")]
    public class IVRM_School_Transaction_InteractionsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISTINT_Id { get; set; }
        public long ISMINT_Id { get; set; }
        public long ISTINT_ToId { get; set; }
        public string ISTINT_ToFlg { get; set; }
        public long ISTINT_ComposedById { get; set; }
        public string ISTINT_Interaction { get; set; }
        public DateTime ISTINT_DateTime { get; set; }
        public string ISTINT_ComposedByFlg { get; set; }
        public int ISTINT_InteractionOrder { get; set; }
        public bool ISTINT_ActiveFlag { get; set; }
        public long ISTINT_CreatedBy { get; set; }
        public long ISTINT_UpdatedBy { get; set; }
        public string ISTINT_Attachment { get; set; }
        public bool? ISTINT_ReadFlg { get; set; }
        public string ISTINT_ISPIPAddress { get; set; }
        public string ISTINT_MACAddress { get; set; }
        public IVRM_School_Master_InteractionsDMO IVRM_School_Master_InteractionsDMO { get; set; }

    }
}

