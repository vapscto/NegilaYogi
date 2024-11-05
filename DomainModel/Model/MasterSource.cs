using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_Master_Source")]
    public class MasterSource : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAMS_Id { get; set; }
        public string PAMS_SourceName { get; set; }
        public string PAMS_SourceDesc { get; set; }
    }
}
