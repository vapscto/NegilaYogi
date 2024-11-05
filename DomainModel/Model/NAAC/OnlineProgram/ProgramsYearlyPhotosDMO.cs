using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.OnlineProgram
{
    [Table("Programs_Yearly_Photos")]
    public class ProgramsYearlyPhotosDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PRYRP_Id { get; set; }
        public long PRYR_Id { get; set; }
        public string PRYRP_Photos { get; set; }
        public bool PRYRP_ActiveFlag { get; set; }
        public long PRYRP_CreatedBy { get; set; }
        public long PRYRP_UpdatedBy { get; set; }


    }
}
