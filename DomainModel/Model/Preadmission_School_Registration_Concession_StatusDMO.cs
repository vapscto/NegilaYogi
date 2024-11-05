using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_School_Registration_Concession_Status")]
    public class Preadmission_School_Registration_Concession_StatusDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PSRCS_ID { get; set; }
        public long PASR_ID { get; set; }
        public long PASRS_Id { get; set; }
        public bool Flag { get; set; }
    }
}
