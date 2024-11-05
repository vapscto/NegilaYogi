using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Student_EnablePartialPayment")]
   public class FeeStudentEnablePartialPaymentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FSEPP_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string FSEPP_Remarks { get; set; }
        public DateTime? FSEPP_RemarksDate { get; set; }
        public bool FSEPP_ActiveFlag { get; set; }
        public DateTime? FSEPP_CreatedDate { get; set; }
        public DateTime? FSEPP_UpdatedDate { get; set; }
        public long? FSEPP_CreatedBy { get; set; }
        public long? FSEPP_UpdatedBy { get; set; }
    }
}
