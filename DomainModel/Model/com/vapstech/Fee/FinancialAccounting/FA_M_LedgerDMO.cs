using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Fee.FinancialAccounting
{
    [Table("FA_M_Ledger")]
    public class FA_M_LedgerDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FAMLED_Id { get; set; }
        public long MI_Id { get; set; }
        public long FAMCOMP_Id { get; set; }
        public long IMFY_Id { get; set; }
        public long FAMGRP_Id { get; set; }
        public long FAUGRP_Id { get; set; }
        public string FAMLED_LedgerName { get; set; }
        public string FAMLED_LedgerAliasName { get; set; }
        public DateTime? FAMLED_LedgerCreatedDate { get; set; }
        public string FAMLED_Remarks { get; set; }
        public string FAMLED_PostalAddress { get; set; }
        public string FAMLED_EmailAddress { get; set; }
        public string FAMLED_Type { get; set; }
        public string FAMLED_Under { get; set; }
        public bool FAMLED_BillwiseFlg { get; set; }
        public bool FAMLED_ActiveFlg { get; set; }
        public DateTime? FAMLED_CreatedDate { get; set; }
        public DateTime? FAMLED_UpdatedDate { get; set; }
        public long FAMLED_CreatedBy { get; set; }
        public long FAMLED_UpdatedBy { get; set; }
        public List<FA_M_Ledger_DetailsDMO> FA_M_Ledger_DetailsDMO { get; set; }

    }
}
