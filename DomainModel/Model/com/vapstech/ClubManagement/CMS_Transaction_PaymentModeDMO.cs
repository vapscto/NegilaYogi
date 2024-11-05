using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_Transaction_PaymentMode", Schema = "CMS")]
    public class CMS_Transaction_PaymentModeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMSTRANSPM_Id { get; set; }
        public long CMSTRANS_Id { get; set; }
        public string CMSTRANSPM_PaymentMode { get; set; }
        public decimal CMSTRANSPM_Amount { get; set; }
        public string CMSTRANSPM_ReferenceNo { get; set; }
        public string CMSTRANSPM_ChequeDDNo { get; set; }
        public DateTime? CMSTRANSPM_ChequeDDDate { get; set; }
        public string CMSTRANSPM_BankName { get; set; }
        public bool CMSTRANSPM_ActiveFlg { get; set; }
        public DateTime? CMSTRANSPM_CreatedDate { get; set; }
        public long CMSTRANSPM_CreatedBy { get; set; }
        public DateTime? CMSTRANSPM_UpdatedDate { get; set; }
        public long CMSTRANSPM_UpdatedBy { get; set; }

    }
}
