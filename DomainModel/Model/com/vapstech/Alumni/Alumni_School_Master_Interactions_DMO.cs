using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_School_Master_Interactions", Schema = "ALU")]
    public class Alumni_School_Master_Interactions_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ALSMINT_Id { get; set; }
        public long MI_Id { get; set; }
        public string ALSMINT_InteractionId { get; set; }
        public long ASMAY_Id { get; set; }
        public string ALSMINT_ComposedByFlg { get; set; }
        public string ALSMINT_GroupOrIndFlg { get; set; }
        public long ALSMINT_ComposedById { get; set; }
        public string ALSMINT_Subject { get; set; }
        public DateTime? ALSMINT_DateTime { get; set; }
        public string ALSMINT_Interaction { get; set; }
        public string ALSMINT_Attachment { get; set; }
        public bool ALSMINT_ActiveFlag { get; set; }
        public DateTime? ALSMINT_CreatedDate { get; set; }
        public DateTime? ALSMINT_UpdatedDate { get; set; }
        public long ALSMINT_CreatedBy { get; set; }
        public long ALSMINT_UpdatedBy { get; set; }
        public List<Alumni_School_Transaction_Interactions_DMO> Alumni_School_Transaction_Interactions_DMO { get; set; }
    }
}
