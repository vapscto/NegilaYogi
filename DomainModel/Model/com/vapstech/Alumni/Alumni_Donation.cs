using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_Donation", Schema = "ALU")]
    public class Alumni_Donation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ALDON_Id { get; set; }
        public long ALMDON_Id { get; set; }
        public long ALSREG_Id { get; set; }
        public string ALDON_DonorName { get; set; }
        public decimal ALDON_Amount { get; set; }
        public DateTime ALDON_Date { get; set; }
        public string ALDON_ReceiptNo { get; set; }
        public string ALDON_ModeOfPayment { get; set; }
        public string ALDON_ReferenceNo { get; set; }
        public bool ALDON_ActiveFlag { get; set; }
        public long ALDON_CreatedBy { get; set; }
        public DateTime? ALDON_CreatedDate { get; set; }
        public long ALDON_UpdatedBy { get; set; }
        public DateTime? ALDON_UpdatedDate { get; set; }
        public bool ALDON_NRIFlg { get; set; }
        public string ALDON_DonarPANNo { get; set; }
        public string ALDON_Towards { get; set; }
    }
}
