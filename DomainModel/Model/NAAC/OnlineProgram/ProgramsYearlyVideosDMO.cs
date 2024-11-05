using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.OnlineProgram
{

    [Table("Programs_Yearly_Videos")]
    public class ProgramsYearlyVideosDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PRYRV_Id { get; set; }
        public long PRYR_Id { get; set; }
        public string PRYRV_Videos { get; set; }
        public bool PRYRV_ActiveFlag { get; set; }
        public long PRYRV_CreatedBy { get; set; }
        public long PRYRV_UpdatedBy { get; set; }
    }
}
