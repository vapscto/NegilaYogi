using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_ONLINE_PAYMENT")]
    public class Prospepaymentamount 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long IVRMOP_ID { get; set; }

        public string IVRMOP_MERCHANT_KEY { get; set; }
    
        public string IVRMOP_MARCHANT_ID { get; set; }
       
        public string IVRMOP_SALT { get; set; }
       
        public string IVRMOP_BASE_URL { get; set; }
     
        public string IVRMOP_REG_AMOUNT { get; set; }

        public string IVRMOP_PROS_AMOUNT { get; set; }
    
        public long IVRMOP_MIID { get; set; }

        public string Paymenttype { get; set; }


    }
}
