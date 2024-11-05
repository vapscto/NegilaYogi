using DomainModel.Model.com.vaps.Fee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Y_Payment_PaymentMode", Schema ="CLG")]
    public class Fee_Y_Payment_PaymentModeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FYPPM_Id { get; set; }
        public long FYP_Id { get; set; }
        public string FYPPM_TransactionTypeFlag { get; set; }
        public decimal FYPPM_TotalPaidAmount { get; set; }
        public long FYPPM_LedgerId { get; set; }
        public string FYPPM_BankName { get; set; }
        public string FYPPM_DDChequeNo { get; set; }
        public DateTime? FYPPM_DDChequeDate { get; set; }
        public string FYPPM_Transaction_Id { get; set; }
        public string FYPPM_PaymentReference_Id { get; set; }
        public string FYPPM_ClearanceStatusFlag { get; set; }
        public DateTime? FYPPM_ClearanceDate { get; set; }
    }
}
