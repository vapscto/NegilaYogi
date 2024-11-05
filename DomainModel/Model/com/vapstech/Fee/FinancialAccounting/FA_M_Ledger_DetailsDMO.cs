using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Fee.FinancialAccounting
{
    [Table("FA_M_Ledger_Details")]
    public class FA_M_Ledger_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FAMLEDD_Id { get; set; }
        public long FAMLED_Id { get; set; }
        public long IMFY_Id { get; set; }
        public decimal FAMLEDD_OpeningBalance { get; set; }
        public string FAMLEDD_OBCRDRFlg { get; set; }
        public decimal FAMLEDD_ClosingBalance { get; set; }
        public string FAMLEDD_CBCRDRFlg { get; set; }
        public DateTime? FAMLEDD_OBDate { get; set; }
        public decimal FAMLEDD_BudgetAmount { get; set; }
        public string FAMLEDD_Remarks { get; set; }
        public bool FAMLEDD_ActiveFlg { get; set; }
        public DateTime? FAMLEDD_CreatedDate { get;set;}
        public DateTime? FAMLEDD_UpdatedDate { get; set; }
        public long FAMLEDD_CreatedBy { get; set; }
        public long FAMLEDD_UpdatedBy { get; set; }

    }
}
