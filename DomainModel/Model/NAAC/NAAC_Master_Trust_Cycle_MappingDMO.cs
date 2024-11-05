using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.NAAC
{
    [Table("NAAC_Master_Trust_Cycle_Mapping")]
    public class NAAC_Master_Trust_Cycle_MappingDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMATCCM_Id { get; set; }
        public long NCMATC_Id { get; set; }
        public long NCMACY_Id { get; set; }
    }
}
