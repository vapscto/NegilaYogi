using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Y_Payment_School_Student")]
    public class Fee_Y_Payment_School_StudentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FYPS_Id { get; set; }
        public long FYP_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public decimal FTP_TotalPaidAmount { get; set; }
        public decimal FTP_TotalWaivedAmount { get; set; }
        public decimal FTP_TotalConcessionAmount { get; set; }
        public decimal FTP_TotalFineAmount { get; set; }

        public FeePaymentDetailsDMO fydd { get; set; }
    }
}
