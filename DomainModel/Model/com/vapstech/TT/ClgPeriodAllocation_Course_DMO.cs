using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.TT
{

    [Table("TT_Master_Period_CourseBranch")]
  public  class ClgPeriodAllocation_Course_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     public long   TTMPC_Id { get; set; }
         public int TTMP_Id { get; set; }
           public long ASMAY_Id { get; set; }
           public long AMCO_Id { get; set; }
           public long AMB_Id { get; set; }
           public long AMSE_Id { get; set; }
            public bool TTMPC_ActiveFlag { get; set; }
          
    }
}
