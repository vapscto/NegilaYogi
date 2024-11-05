using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("Transport_Master_Configuration", Schema = "TRN")]
    public class TRMasterconfigurationDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRMC_ID { get; set; }
        public long MI_ID { get; set; }
        public long ASMAY_ID { get; set; }

        public int TRMC_TRREGULAR { get; set; }
        public int TRMC_TRNEW { get; set; }
        public int TRMC_ADMNEW { get; set; }
        public DateTime? TRMC_STARTDATE { get; set; }

        public DateTime? TRMC_ENDDATE { get; set; }

        public int TRMC_ACTIVE { get; set; }
    }
}
