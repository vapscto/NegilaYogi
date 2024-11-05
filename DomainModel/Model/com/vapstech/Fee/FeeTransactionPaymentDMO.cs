using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("fee_t_payment")]
    public class FeeTransactionPaymentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FTP_Id { get; set; }
        public long FYP_Id { get; set; }
        public long FMA_Id { get; set; }
        public decimal FTP_Paid_Amt { get; set; }
        public decimal FTP_Fine_Amt { get; set; }
        public decimal FTP_Concession_Amt { get; set; }
        public decimal FTP_Waived_Amt { get; set; }
        public string ftp_remarks { get; set; }

        public decimal? FTP_RebateAmount { get; set; }

        public DateTime? FTP_CreatedDate { get; set; }
        public DateTime? FTP_UpdatedDate { get; set; }
        public long? FTP_CreatedBy { get; set; }
        public long? FTP_UpdatedBy { get; set; }
        public FeePaymentDetailsDMO feePaymentDetails { get; set; }

    }
}
