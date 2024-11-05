using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.IssueManager.PettyCash
{
    [Table("PC_Indent_Details")]
    public class PC_Indent_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PCINDENTDET_Id { get; set; }
        public long PCINDENT_Id { get; set; }
        public long PCMPART_Id { get; set; }
        public long PCREQTN_Id { get; set; }
        public decimal? PCINDENTDET_RequestedAmount { get; set; }
        public string PCINDENTDET_Remarks { get; set; }
        public decimal? PCINDENTDET_SanctionedAmt { get; set; }
        public bool PCINDENTDET_ActiveFlg { get; set; }
        public DateTime? PCINDENTDET_CreatedDate { get; set; }
        public DateTime? PCINDENTDET_UpdatedDate { get; set; }
        public long PCINDENTDET_CreatedBy { get; set; }
        public long PCINDENTDET_UpdatedBy { get; set; }
    }
}
