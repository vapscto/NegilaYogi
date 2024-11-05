using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.IssueManager.PettyCash
{
    [Table("PC_Master_Particulars")]
    public class PC_Master_ParticularsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PCMPART_Id { get; set; }
        public long MI_Id { get; set; }
        public string PCMPART_ParticularName { get; set; }
        public string PCMPART_ParticularDesc { get; set; }
        public bool PCMPART_ActiveFlg { get; set; }
        public DateTime? PCMPART_CreatedDate { get; set; }
        public DateTime? PCMPART_UpdatedDate { get; set; }
        public long PCMPART_CreatedBy { get; set; }
        public long PCMPART_UpdatedBy { get; set; }
    }
}
