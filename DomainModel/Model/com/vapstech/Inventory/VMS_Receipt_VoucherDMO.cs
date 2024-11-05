using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{
    public class VMS_Receipt_VoucherDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VRECVOU_Id { get; set; }
        public long MI_Id { get; set; }
        public bool VRECVOU_FromClientFlg { get; set; }
        public long? ISMMCLT_Id { get; set; }
        public long? HRMBD_Id { get; set; }
        public string VRECVOU_ReceiptMode { get; set; }
        public string VRECVOU_ReceiptReference { get; set; }
        public string VRECVOU_VoucherNo { get; set; }
        public string VRECVOU_ReceivedFrom { get; set; }
        public string VRECVOU_ChequeDDNo { get; set; }
        public string VRECVOU_Remarks { get; set; }
        public decimal? VRECVOU_Amount { get; set; }
        public bool VRECVOU_ActiveFlg { get; set; }
        public long VRECVOU_CreatedBy { get; set; }
        public DateTime? VRECVOU_CreatedDate { get; set; }
        public long VRECVOU_UpdatedBy { get; set; }
        public DateTime? VRECVOU_UpdatedDate { get; set; }
        public long? VCBSRC_Id { get; set; }
        public long? ISMCPPD_Id { get; set; }
        public DateTime? VRECVOU_Date { get; set; }
        public List<VMS_BankAccount_BRSDMO> VMS_BankAccount_BRSDMO { get; set; }
    }
}
