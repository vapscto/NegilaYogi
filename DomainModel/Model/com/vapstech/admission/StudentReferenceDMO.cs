using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("Adm_M_Student_Reference")]
    public class StudentReferenceDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMSTR_Id { get; set; }
        public long MI_Id { get; set; }
        [ForeignKey("AMST_Id")]
        public long AMST_Id { get; set; }
        public long PAMR_Id { get; set; }
        public long? AMSTR_CreatedBy { get; set; }
        public long? AMSTR_UpdatedBy { get; set; }

        // public ICollection<pagemodulemapping> modulepagemapping { get; set; }
    }
}
