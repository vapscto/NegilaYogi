using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Fee.FinancialAccounting
{
    [Table("FA_Master_Company")]
    public class FACompanyMasterDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FAMCOMP_Id { get; set; }
         public long MI_Id { get; set; }
       
        public string FAMCOMP_CompanyName { get; set; }
        public string FAMCOMP_Description { get; set; }
        public string FAMCOMP_CompanyAddress { get; set; }
        public string FAMCOMP_EMailId { get; set; }
        public string FAMCOMP_PhoneNo { get; set; }
        public string FAMCOMP_IncomeTaxNo { get; set; }
        public string FAMCOMP_SalesTaxNo { get; set; }
        public long FAMCOMP_UpdatedBy { get; set; }

        public DateTime? FAMCOMP_UpdatedDate { get; set; }

        public string FAMCOMP_Password { get; set; }
        public DateTime? FAMCOMP_BookBeginingDate { get; set; }
        public bool FAMCOMP_PrintReceiptFlg { get; set; }
        public bool FAMCOMP_DuplicateVoucherFlg { get; set; }
        public bool FAMCOMP_UseBillWiseDetailsFlg { get; set; }
        public bool FAMCOMP_CMPTypeFlg { get; set; }
        public bool FAMCOMP_UseDebitCreditFlg { get; set; }
        public bool FAMCOMP_SetTypeFlg { get; set; }
        public bool FAMCOMP_SetLedgerBalanceFlg { get; set; }
        public bool FAMCOMP_SetDispFlg { get; set; }
        public bool FAMCOMP_SetNegBalanceFlg { get; set; }
        public string FAMCOMP_StatusFlg { get; set; }

        public bool FAMCOMP_ActiveFlg { get; set; }

        public DateTime? FAMCOMP_CreatedDate { get; set; }
       

        public long FAMCOMP_CreatedBy { get; set; }
       
    }
}
