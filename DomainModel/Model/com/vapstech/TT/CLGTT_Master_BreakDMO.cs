using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Master_Break_College")]
    public class CLGTT_Master_BreakDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTMBC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long TTMC_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long TTMD_Id { get; set; }
        public decimal TTMBC_AfterPeriod { get; set; }
        public string  TTMBC_BreakName { get; set; }
        public string TTMBC_BreakStartTime { get; set; }
        public string TTMBC_BreakEndTime { get; set; }
        public bool TTMBC_ActiveFlag { get; set; }
        public List<CLGTT_Master_Break_AftPeriodsDMO> CLGTT_Master_Break_AftPeriodsDMO { get; set; }
        public List<CLGTT_Master_Break_BefPeriodsDMO> CLGTT_Master_Break_BefPeriodsDMO { get; set; }

    }
}
