using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.IssueManager.PettyCash
{
    [Table("PC_Indent")]
    public class PC_IndentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PCINDENT_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string PCINDENT_IndentNo { get; set; }       
        public DateTime? PCINDENT_Date { get; set; }
        public string PCINDENT_Desc { get; set; }
        public decimal? PCINDENT_RequestedAmount { get; set; }
        public decimal? PCINDENT_SanctionedAmt { get; set; }
        public bool PCINDENT_ActiveFlg { get; set; }
        public DateTime? PCINDENT_CreatedDate { get; set; }
        public DateTime? PCINDENT_UpdatedDate { get; set; }
        public long PCINDENT_CreatedBy { get; set; }
        public long PCINDENT_UpdatedBy { get; set; } 
        public List<PC_Indent_DetailsDMO> PC_Indent_DetailsDMO { get; set; }
    }
}
