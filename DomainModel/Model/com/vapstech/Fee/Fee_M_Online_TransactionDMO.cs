using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("fee_m_online_transaction")]
    public class Fee_M_Online_TransactionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FMOT_Id { get; set; }
        public string FMOT_Trans_Id { get; set; }
        public decimal FMOT_Amount { get; set; }
        public DateTime FMOT_Date { get; set; }
        public string FMOT_Flag { get; set; }
        public long AMST_Id { get; set; }
        public long PASR_Id { get; set; }

        public string FMOT_Receipt_no { get; set; }

        public long MI_Id { get; set; }
        public long ASMAY_ID { get; set; }

        public string FYP_PayModeType { get; set; }
        public string FMOT_PayGatewayType { get; set; }
        public List<Fee_T_Online_TransactionDMO> Fee_T_Online_TransactionDMO { get; set; }

    }
}
