using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_School_Transaction_Interactions", Schema = "ALU")]
    public class Alumni_School_Transaction_Interactions_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ALSTINT_Id { get; set; }
        public long ALSMINT_Id { get; set; }
        public long ALSTINT_ToId { get; set; }
        public long ALSTINT_ComposedById { get; set; }
        public string ALSTINT_Interaction { get; set; }
        public string ALSTINT_ToFlg { get; set; }
        public string ALSTINT_Attachment { get; set; }
        public DateTime? ALSTINT_DateTime { get; set; }
        public string ALSTINT_ComposedByFlg { get; set; }
        public int ALSTINT_InteractionOrder { get; set; }
        public bool ALSTINT_ActiveFlag { get; set; }
        public DateTime? ALSTINT_CreatedDate { get; set; }
        public DateTime? ALSTINT_UpdatedDate { get; set; }
        public long ALSTINT_CreatedBy { get; set; }
        public long ALSTINT_UpdatedBy { get; set; }
    }
}
