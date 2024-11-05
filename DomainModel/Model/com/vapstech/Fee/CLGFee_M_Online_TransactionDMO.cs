using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_M_Online_Transaction_College")]
    public class CLGFee_M_Online_TransactionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FMOTC_Id { get; set; }
        public string FMOTC_Trans_Id { get; set; }
        public decimal FMOTC_Amount { get; set; }
        public DateTime FMOTC_Date { get; set; }
        public string FMOTC_Flag { get; set; }
        public long AMCST_Id { get; set; }
        public long PACA_Id { get; set; }

        public string FMOTC_Receipt_no { get; set; }

        public long MI_Id { get; set; }
        public long ASMAY_ID { get; set; }

        public string FYP_PayModeType { get; set; }
        public string FMOTC_PayGatewayType { get; set; }
    }
}
