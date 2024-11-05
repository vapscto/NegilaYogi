using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.IssueManager.PettyCash
{
    [Table("PC_Requisition")]
    public class PC_RequisitionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PCREQTN_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string PCREQTN_RequisitionNo { get; set; }
        public DateTime? PCREQTN_Date { get; set; }
        public string PCREQTN_Purpose { get; set; }
        public decimal? PCREQTN_TotAmount { get; set; }
        public decimal? PCREQTN_SanctionedAmt { get; set; }
        public bool PCREQTN_ActiveFlg { get; set; }
        public DateTime? PCREQTN_CreatedDate { get; set; }
        public DateTime? PCREQTN_UpdatedDate { get; set; }
        public long PCREQTN_CreatedBy { get; set; }
        public long PCREQTN_UpdatedBy { get; set; }
        public List<PC_Requisition_DetailsDMO> PC_Requisition_DetailsDMO { get; set; }
    }
}
