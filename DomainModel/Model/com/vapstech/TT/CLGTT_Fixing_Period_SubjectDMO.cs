using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Fixing_Period_Subject_CourseBranch")]
    public class CLGTT_Fixing_Period_SubjectDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long TTFPSUCB_Id { get; set; }
        public long TTFPSU_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long TTFPSUCB_Days { get; set; }
        public bool TTFPSUCB_ActiveFlag { get; set; }

    }
}
