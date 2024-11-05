using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.IssueManager.PettyCash
{
    [Table("PC_Expenditure")]
    public class PC_ExpenditureDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PCEXPTR_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long PCMPART_Id { get; set; }
        public decimal? PCEXPTR_Amount { get; set; }
        public long PCINDENTAP_Id { get; set; }
        public string PCEXPTR_ExpenditureNo { get; set; }
        public DateTime? PCEXPTR_Date { get; set; }
        public string PCEXPTR_Desc { get; set; }
        public string PCEXPTR_ModeOfPayment { get; set; }
        public string PCEXPTR_ReferenceNo { get; set; }
        public string PCEXPTR_DeletedRemarks { get; set; }
        public bool PCEXPTR_ActiveFlg { get; set; }
        public DateTime? PCEXPTR_CreatedDate { get; set; }
        public DateTime? PCEXPTR_UpdatedDate { get; set; }
        public long PCEXPTR_CreatedBy { get; set; }
        public long PCEXPTR_UpdatedBy { get; set; }
    }
}
