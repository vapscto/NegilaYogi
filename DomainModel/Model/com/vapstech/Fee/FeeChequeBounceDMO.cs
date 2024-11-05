using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Cheque_Bounce")]
    public class FeeChequeBounceDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FCB_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_ID { get; set; }
        public long MI_ID { get; set; }
        public long FYP_ID { get; set; }
        public decimal FCB_Amount { get; set; }
        public string FCB_Remarks { get; set; }
        public DateTime FCB_DATE { get; set; }
        public bool FCB_ActiveFlag { get; set; }
        public int user_id { get; set; }
    }
}
