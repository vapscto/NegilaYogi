using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Master_Break_BefPeriods_College")]
    public class CLGTT_Master_Break_BefPeriodsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTMBBPC_Id { get; set; }
        public long TTMBC_Id { get; set; }
        public int TTMP_Id { get; set; }

    }
}
