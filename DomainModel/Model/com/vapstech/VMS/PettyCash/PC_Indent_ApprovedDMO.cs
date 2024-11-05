using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.IssueManager.PettyCash
{
    [Table("PC_Indent_Approved")]
    public class PC_Indent_ApprovedDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PCINDENTAP_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long PCINDENT_Id { get; set; }
        public string PCINDENTAPT_IndentNo { get; set; }
        public DateTime? PCINDENTAPT_Date { get; set; }
        public string PCINDENTAPT_Desc { get; set; }
        public decimal? PCINDENTAPT_RequestedAmount { get; set; }
        public decimal? PCINDENTAPT_SanctionedAmt { get; set; }
        public decimal? PCINDENTAPT_AmountSpent { get; set; }
        public decimal? PCINDENTAPT_BalanceAmount { get; set; }
        public bool PCINDENTAPT_ActiveFlg { get; set; }
        public DateTime? PCINDENTAPT_CreatedDate { get; set; }
        public DateTime? PCINDENTAPT_UpdatedDate { get; set; }
        public long PCINDENTAPT_CreatedBy { get; set; }
        public long PCINDENTAPT_UpdatedBy { get; set; }
        public List<PC_Indent_Approved_DetailsDMO> PC_Indent_Approved_DetailsDMO { get; set; }
    }
}
