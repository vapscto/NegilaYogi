using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_SeatBlocked_Students")]
    public class Preadmission_SeatBlocked_Student : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASBS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long PASR_Id { get; set; }
        public DateTime PASBS_Date { get; set; }
        public long IVRMSTAUL_Id { get; set; }
       // public virtual Institution institute { get; set; }
    }
}
