using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("VMS_BankAccount_BRS")]
    public class VMS_BankAccount_BRSDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VBABRS_Id { get; set; }
        public long MI_Id { get; set; }
        public long? HRMBD_Id { get; set; }
        public string VBABRS_RefrenceId { get; set; }
        public DateTime? VBABRS_Date { get; set; }
        public decimal? VBABRS_Amount { get; set; }
        public string VBABRS_CRDRFlg { get; set; }
        public string VBABRS_Remarks { get; set; }
        public bool VBABRS_ClearedFlg { get; set; }
        public DateTime? VBABRS_ClearedDate { get; set; }
        public bool VBABRS_ActiveFlg { get; set; }
        public long VBABRS_CreatedBy { get; set; }
        public DateTime VBABRS_CreatedDate { get; set; }
        public long VBABRS_UpdatedBy { get; set; }
        public DateTime VBABRS_UpdatedDate { get; set; }

        public long? VPAYVOU_Id { get; set; }
        public decimal? VBABRS_BalanceAsperVoucher { get; set; }
        public long? ISMMCLT_Id { get; set; }
        public long? VRECVOU_Id { get; set; }

        public decimal? VBABRS_ActualBankBalance { get; set; }
    }
}
