using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_School_Registration_Source")]
    public class PreadmissionStudnetSource : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASRS_Id { get; set; }
        public long PASR_Id { get; set; }
        public long PAMS_Id { get; set; }

    }
}
