using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Others_Concession")]
    public class Fee_Others_ConcessionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FOC_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMOST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMA_Id { get; set; }
        public long FMG_Id { get; set; }

        public long FMH_Id { get; set; }
        public string FOC_ConcessionReason { get; set; }
        public string FOC_ConcessionType { get; set; }

        public bool FOC_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
