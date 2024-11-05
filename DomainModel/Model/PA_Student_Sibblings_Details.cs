using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("PA_Student_Sibblings")]
    public class PA_Student_Sibblings : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASS_Id { get; set; }
        public long MI_Id { get; set; }
        public long PASR_Id { get; set; }

        public decimal? PASS_ConcessionPer { get; set; }
        public long? PASS_ConcessionAmt  { get; set; }
        public bool PASS_ActiveFlag { get; set; }

    }
}
