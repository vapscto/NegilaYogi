using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Refund")]
    public class FeeMasterRefundDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FR_ID { get; set; }
        public long FMH_ID { get; set; }
        public long AMST_ID { get; set; }
        public DateTime? FR_Date { get; set; }
        public decimal FR_RefundAmount { get; set; }
        public string FR_RefundFlag { get; set; }
        public long ASMAY_ID { get; set; }
        public string FR_RefundRemarks { get; set; }
        public string FR_RefundNo { get; set; }
        public string FR_BANK_CASH { get; set; }
        public string FR_Favor { get; set; }
        public bool FR_BC_Flag { get; set; }
        public DateTime? FR_CheqDate { get; set; }
        public long FR_CheqNo { get; set; }
        public long FMG_Id { get; set; }

        public long MI_Id { get; set; }
        public string FR_BankName { get; set; }
        public long FTI_Id { get; set; }
        public long User_Id { get; set; }

        //  public string REF_BANK_NAME { get; set; }

    }
}
