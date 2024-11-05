using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_Master_Donation", Schema = "ALU")]
    public class Alumni_Master_Donation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ALMDON_Id { get; set; }
        public long MI_Id { get; set; }
        public string ALMDON_DonationName { get; set; }
        public  decimal ALMDON_Amount { get; set; }
        public bool ALMDON_ActiveFlag { get; set; }
        public DateTime? ALMDON_CreatedDate { get; set; }
        public long ALMDON_CreatedBy { get; set; }
        public long ALMDON_UpdatedBy { get; set; }
        public bool? ALMDON_RegistrationFeeFlag { get; set; }
        public DateTime? ALMDON_UpdatedDate { get; set; }
    }
}
