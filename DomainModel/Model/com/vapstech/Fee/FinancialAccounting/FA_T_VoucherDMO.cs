using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Fee.FinancialAccounting
{
    [Table("FA_T_Voucher")]
    public class FA_T_VoucherDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FATVOU_Id { get; set; }
        public long FAMVOU_Id { get; set; }
        public long FAMLED_Id { get; set; }
        public decimal? FATVOU_Amount { get; set; }
        public string FATVOU_CRDRFlg { get; set; }
        public string FATVOU_TransactionTypeFlg { get; set; }
        public string FATVOU_Narration { get; set; }
        public string FATVOU_ChequNo { get; set; }
        public DateTime? FATVOU_ChequeDate { get; set; }
        public string FATVOU_BankName { get; set; }
        public string FATVOU_ReferrenceNo { get; set; }
        public bool FATVOU_BillwiseFlg { get; set; }
        public string FATVOU_Description { get; set; }
        public bool FATVOU_ActiveFlg { get; set; }
        public DateTime? FATVOU_CreatedDate { get; set; }
        public DateTime? FATVOU_UpdatedDate { get; set; }
        public long FATVOU_CreatedBy { get; set; }
        public long FATVOU_UpdatedBy { get; set; }
    }
}
