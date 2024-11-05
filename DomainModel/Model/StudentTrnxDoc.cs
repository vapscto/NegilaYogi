using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_School_Registration_Documents")]
    public class StudentTrnxDoc : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASRD_Id { get; set; }
        public long PASR_Id { get; set; }
        public long AMSMD_Id { get; set; }
        public string Document_Path { get; set; }
    }
}
