using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("FEE_RAZOR_TRANSFER_API_DETAILS")]
    public class FEE_RAZOR_TRANSFER_API_DETAILS 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMOTT_ID { get; set; }
        public string ORDER_ID { get; set; }
        public string TRANSFER_ID { get; set; }
        public string ENTITY { get; set; }

        public string SOURCE { get; set; }
        public string RECIPIENT { get; set; }
        public string AMOUNT { get; set; }
        public string CREATED_AT { get; set; }
        public DateTime CREATED_BY { get; set; }
        public DateTime UPDATED_BY { get; set; }

        public string PAYMENT_ID { get; set; }
        public string SETTLEMENT_FLAG { get; set; }
        public long MI_ID { get; set; }

    }
}
