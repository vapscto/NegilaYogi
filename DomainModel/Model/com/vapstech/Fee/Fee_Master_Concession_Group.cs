using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Master_Concession_Group")]
    public class Fee_Master_Concession_Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMCCG_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMCC_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public bool FMCCG_ActiveFlag { get; set; }
        public  long ASMAY_Id { get; set; }

    }
}
