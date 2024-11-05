using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Fee_Master_Concession")]
    public class Fee_Master_ConcessionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FMCC_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMCC_ConcessionName { get; set; }
        public string FMCC_ConcessionFlag { get; set; }
        public string FMCC_ConcessionApplLimit { get; set; }
        public bool FMCC_ActiveFlag { get; set; }

    }
}
