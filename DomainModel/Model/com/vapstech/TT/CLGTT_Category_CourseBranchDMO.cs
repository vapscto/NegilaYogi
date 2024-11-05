using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Category_CourseBranch")]
    public class CLGTT_Category_CourseBranchDMO:CommonParamDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long TTCCB_Id { get; set; }
       public long TTMC_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public bool TTCC_ActiveFlag { get; set; }

    }
}
