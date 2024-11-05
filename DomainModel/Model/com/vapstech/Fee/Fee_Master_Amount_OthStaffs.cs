using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Master_Amount_OthStaffs")]
    public class Fee_Master_Amount_OthStaffs:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FMAOST_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMG_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public decimal FMAOST_Amount { get; set; }
        public string FMAOST_OthStaffFlag { get; set; }
        public bool FMAOST_ActiveFlag { get; set; }

    }
}
