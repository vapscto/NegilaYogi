using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_School_Registration_Employee")]
    public class Preadmission_School_Registration_Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PSRE_ID { get; set; }
        public long PASR_Id { get; set; }
        public long? HRME_ID { get; set; }
        public bool? PSRE_ActiveFlag { get; set; }

    }
}
