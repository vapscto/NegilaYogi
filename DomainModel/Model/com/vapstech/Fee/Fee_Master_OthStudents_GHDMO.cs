using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Master_OthStudents_GH")]
    public class Fee_Master_OthStudents_GHDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMOSTGH_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMOST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMG_Id { get; set; }
        public bool FMOSTGH_ActiveFlag { get; set; }
        public List<Fee_Master_OthStudents_GH_InstlDMO> Fee_Master_OthStudents_GH_InstlDMO { get; set; }
    }
}
