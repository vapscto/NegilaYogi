using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Student_FeeMapping")]
    public class FeeCardDetailsEntryDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FSFM_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FMG_Id { get; set; }
        //[ForeignKey("FTI_Id")]
        public long FTI_Id { get; set; }
        public long FSFM_Amount { get; set; }
        public long FSFM_PaidAmount { get; set; }
      
    }
}
