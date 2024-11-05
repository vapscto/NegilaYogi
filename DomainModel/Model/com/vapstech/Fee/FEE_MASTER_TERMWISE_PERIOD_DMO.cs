using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("FEE_MASTER_TERMWISE_PERIOD")]
    public class FEE_MASTER_TERMWISE_PERIOD_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FMTP_Id { get; set; }
        public long FMT_Id { get; set; }
        public string FMTP_Year { get; set; }
        public string FMTP_FROM_MONTH { get; set; }
        public string FMTP_TO_MONTH { get; set; }
        public long USER_ID { get; set; }
        public long ASMAY_ID { get; set; }

        public string FeeFlag { get; set; }

    }
}
