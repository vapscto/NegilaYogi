using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.PDA
{
    [Table("PDA_Expenditure")]
    public class PDA_ExpenditureDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PDAE_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime PDAE_Date { get; set; }
        public string PDAE_TransactionNo { get; set; }
        public decimal PDAE_TotAmount { get; set; }
        //public DateTime? CreatedDate { get; set; }
       // public DateTime? UpdatedDate { get; set; }
        public long ASMAY_ID { get; set; }
        public bool PDAE_CreditFlg { get; set; }
        public string PDAE_ModeOfPayment { get; set; }
        public List<PDA_Expenditure_HeadsDMO> PDA_Expenditure_HeadsDMO { get; set; }
    }
}
