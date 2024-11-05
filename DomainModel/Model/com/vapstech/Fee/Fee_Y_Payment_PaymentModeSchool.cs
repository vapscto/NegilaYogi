using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Y_Payment_PaymentMode")]
    public class Fee_Y_Payment_PaymentModeSchool
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FYPPM_Id { get; set; }
        public long FYP_Id { get; set; }
        public string FYP_TransactionTypeFlag { get; set; }
        public decimal FYPPM_TotalPaidAmount { get; set; }
        public long FYPPM_LedgerId { get; set; }
        public string FYPPM_BankName { get; set; }
        public string FYPPM_DDChequeNo { get; set; }
        public DateTime FYPPM_DDChequeDate { get; set; }
        public string FYPPM_TransactionId { get; set; }
        public string FYPPM_PaymentReferenceId { get; set; }
        public string FYPPM_ClearanceStatusFlag { get; set; }
        public DateTime FYPPM_ClearanceDate { get; set; }

        public FeePaymentDetailsDMO fyds { get; set; }

    }
}
