using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Canteen
{
    [Table("CM_Transaction")]
    public  class FoodtransactionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long CMTRANS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ACMST_Id { get; set; }
        public long HRME_Id { get; set; }
        public string CMTRANS_MemberFlg { get; set; }
        public decimal CMTRANS_Amount { get; set; }
        public decimal CMTRANS_TaxAmount { get; set; }
        public decimal CMTRANS_TotalAmount { get; set; }
        public string CMTRANS_Remarks { get; set; }
        public decimal CMTRANS_PaidAmount { get; set; }
        public decimal CMTRANS_PendingAmount { get; set; }
        public bool CMTRANS_KOTPrintedFlg { get; set; }
        public string CMTRANS_NoofKOTPrints { get; set; }
        public bool CMTRANS_VoidKotFlg { get; set; }
        public string CMTRANS_VoidReasons { get; set; }
        public bool CMTRANS_SelfCheckInFlg { get; set; }
        public int? CMTRANS_SecurityCode { get; set; }
        public bool CMTRANS_ActiveFlg { get; set; }
        public long? CMTRANS_CreatedBy { get; set; }
        public long? CMTRANS_UpdatedBy { get; set; }
        public DateTime? CMTRANS_CreatedDate { get; set; }
        public DateTime? CMTRANS_Updateddate { get; set; }
        public string CM_Transactionnum { get; set; }
        public long CM_orderID { get; set; }
        public bool CM_PrintFlag { get; set; }
        public List<CM_Transaction_TaxDMO> CM_Transaction_TaxDMO { get; set; }
        public List<CM_Transaction_PaymentModeDMO> CM_Transaction_PaymentModeDMO { get; set; }
        public List<CM_Transaction_ItemsDMO> CM_Transaction_ItemsDMO { get; set; }

    }
}
