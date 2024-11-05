using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Consecutive_College")]
    public class CLGTT_ConsecutiveDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long TTCC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long TTMC_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public decimal TTCC_NoOfPeriods { get; set; }
        public decimal TTCC_AllotPeriods { get; set; }
        public decimal TTCC_RemPeriods { get; set; }
        public decimal TTCC_NoOfConPeriods { get; set; }
        public decimal TTCC_NoOfConDays { get; set; }
        public int TTCC_BefAftApplFlag { get; set; }
        public decimal TTCC_BefAftPeriod { get; set; }
        public int TTCC_BefAftFalg { get; set; }
       // public int TTMP_Id { get; set; }
        public string TTCC_AllotedFlag { get; set; }
        public bool TTCC_ActiveFlag { get; set; }


    }
}
