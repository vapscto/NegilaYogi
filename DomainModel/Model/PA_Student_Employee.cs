using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("PA_Student_Employee")]
    public class PAStudentEmployee : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASE_Id { get; set; }
        public long MI_Id { get; set; }
        public long PASS_Id { get; set; }

        public long PASR_Id { get; set; }
        public long? HRME_Id { get; set; }
        public bool PASE_ActiveFlag  { get; set; }

    }
}
