using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_T_Payment_Online")]
    public class Fee_T_Payment_OnlineDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FTPONLINE_Id { get; set; }
        public long FYP_Id { get; set; }
        public long FTPONLINE_FYP_Id { get; set; }
        public long IMPG_Id { get; set; }
        public string FTPONLINE_OnlineTransactionId { get; set; }
        public decimal FTPONLINE_TotalFeeAmount { get; set; }
        public string FTPONLINE_CardType { get; set; }
        public string FTPONLINE_CardNetworkType { get; set; }
        public decimal FTPONLINE_TotalTDRAmount { get; set; }
        public decimal FTPONLINE_TDRFee { get; set; }
        public decimal FTPONLINE_TDRTax { get; set; }

        public decimal FTPONLINE_VAPSAmount { get; set; }
        public string FTPONLINE_Remarks { get; set; }
        




    }
}
