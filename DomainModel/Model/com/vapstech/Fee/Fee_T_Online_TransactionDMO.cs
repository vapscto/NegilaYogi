using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("fee_T_online_transaction")]
    public class Fee_T_Online_TransactionDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FTOT_Id { get; set; }
        public long FMOT_Id { get; set; }
        public long FMA_Id { get; set; }
        public decimal FTOT_Amount { get; set; }
        public DateTime FTOT_Created_date { get; set; }
        public DateTime FTOT_Updated_date { get; set; }
        public long FTOT_Concession { get; set; }
        public long FTOT_Fine { get; set; }
        public long FTOT_RebateAmount { get; set; }

        public Fee_M_Online_TransactionDMO Fee_M_Online_Transaction { get; set; }

    }
}
