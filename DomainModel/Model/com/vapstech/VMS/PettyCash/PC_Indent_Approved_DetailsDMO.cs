using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.IssueManager.PettyCash
{
    [Table("PC_Indent_Approved_Details")]
    public class PC_Indent_Approved_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PCINDENTAPDT_Id { get; set; }
        public long PCINDENTAP_Id { get;set;}
        public long PCMPART_Id { get;set;}
        public decimal? PCINDENTAPDT_RequestedAmount { get;set;}
        public string PCINDENTAPDT_Remarks { get;set;}
        public decimal? PCINDENTAPDT_SanctionedAmt { get;set;}
        public decimal? PCINDENTAPDT_AmountSpent { get;set;}
        public decimal? PCINDENTAPDT_BalanceAmount { get;set;}
        public bool PCINDENTAPDT_ActiveFlg { get;set;}
        public DateTime? PCINDENTAPDT_CreatedDate { get;set;}
        public DateTime? PCINDENTAPDT_UpdatedDate { get;set;}
        public long PCINDENTAPDT_CreatedBy { get;set;}
        public long PCINDENTAPDT_UpdatedBy { get;set;}
    }
}
