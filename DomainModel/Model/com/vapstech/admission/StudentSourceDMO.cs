using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("Adm_M_Student_Source")]
    public class StudentSourceDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMSTS_Id { get; set; }
        public long MI_Id { get; set; }
        [ForeignKey("AMST_Id")]
        public long AMST_Id { get; set; }
        public long PAMS_Id { get; set; }
        public long? AMSTS_CreatedBy { get; set; }
        public long? AMSTS_UpdatedBy { get; set; }

        // public ICollection<pagemodulemapping> modulepagemapping { get; set; }
    }
}
