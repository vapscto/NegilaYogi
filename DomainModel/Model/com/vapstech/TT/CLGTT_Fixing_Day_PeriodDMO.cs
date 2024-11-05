using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Fixing_Day_Period_College")]
    public class CLGTT_Fixing_Day_PeriodDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTFDPC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long TTMC_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long TTMD_Id { get; set; }
        public int TTMP_Id { get; set; }
        public string TTFDPC_AllotedFlag { get; set; }
        public bool TTFDPC_ActiveFlag { get; set; }

    }
}
