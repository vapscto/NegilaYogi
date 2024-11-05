using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Canteen
{
    [Table("CM_Transaction_PaymentMode")]
    public class CM_Transaction_PaymentModeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long CMTRANSPM_Id { get; set; }
        public long CMTRANS_Id { get; set; }
        public int CMTRANSPM_PaymentModeId { get; set; }
        public string CMTRANSPM_PaymentMode { get; set; }
        public long? CMTRANSPM_CreatedBy { get; set; }
        public long? CMTRANSPM_UpdatedBy { get; set; }
        public DateTime? CMTRANSPM_CreatedDate { get; set; }
        public DateTime? CMTRANSPM_Updateddate { get; set; }
    }
}
