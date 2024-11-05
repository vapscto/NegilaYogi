using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Adm_Master_Student_PA")]
    public class Adm_Master_Student_PA : CommonParamDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMSTPA_Id { get; set; }

        public long AMST_Id { get; set; }

        public long PASR_Id  { get; set; }

    }
}
