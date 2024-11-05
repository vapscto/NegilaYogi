using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Master_Opening_Balance")]
    public class FeeOpeningBalanceDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMOB_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FTI_Id { get; set; }
        public DateTime? FMOB_EntryDate { get; set; }
        public decimal FMOB_Student_Due { get; set; }
        public decimal FMOB_Institution_Due { get; set; }
        // public string FMT_Name { get; set; }
        // public bool FMT_ActiveFlag { get; set; }
        public long User_Id { get; set; }

    }
}
