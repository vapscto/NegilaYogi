using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_Student_Status_History")]
    public class StudentStatusHistory : CommonParamDMO
    {
        [Key]
        public long PASSH_Id { get; set; }
        public long PASR_Id { get; set; }
        public string PASSH_Status { get; set; }
        public DateTime PASSH_Date { get; set; }
    }
}
