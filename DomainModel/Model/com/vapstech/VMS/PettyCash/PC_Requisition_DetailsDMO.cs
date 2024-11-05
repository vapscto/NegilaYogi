using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.IssueManager.PettyCash
{
    [Table("PC_Requisition_Details")]
    public class PC_Requisition_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PCREQTNDET_Id { get; set; }
        public long PCREQTN_Id { get; set; }
        public long PCMPART_Id { get; set; }
        public decimal? PCREQTNDET_Amount { get; set; }
        public string PCREQTNDET_Remarks { get; set; }
        public decimal? PCREQTNDET_SanctionedAmount { get; set; }
        public bool PCREQTNDET_ActiveFlg { get; set; }
        public DateTime? PCREQTNDET_CreatedDate { get; set; }
        public DateTime? PCREQTNDET_UpdatedDate { get; set; }
        public long PCREQTNDET_CreatedBy { get; set; }
        public long PCREQTNDET_UpdatedBy { get; set; }
    }
}
