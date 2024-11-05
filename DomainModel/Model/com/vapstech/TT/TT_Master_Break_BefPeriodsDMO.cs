using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Master_Break_BefPeriods")]
    public class TT_Master_Break_BefPeriodsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTMBBP_Id { get; set; }
        public long TTMB_Id { get; set; }
        public int TTMP_ID { get; set; }           
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
