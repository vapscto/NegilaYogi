using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Master_OthStudents_GH_Instl")]
    public class Fee_Master_OthStudents_GH_InstlDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMOSTGHI_Id { get; set; }
        [ForeignKey("FMOSTGH_Id")]
        public long FMOSTGH_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
    }
}
