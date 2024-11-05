using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Fixing_Day_Subject_CourseBranch")]
    public class CLGTT_Fixing_Day_SubjectDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTFDSUCB_Id { get; set; }
        public long TTFDSU_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long TTFDSUCB_Periods { get; set; }
        public bool TTFDSUCB_ActiveFlag { get; set; }
        
    }
}
