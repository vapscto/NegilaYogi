using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Final_Period_Distribution")]
    public class TT_Final_Period_DistributionDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long TTFPD_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_Id { get; set; }
        public int TTFPD_TotWeekPeriods { get; set; }
        public bool TTFPD_ActiveFlag { get; set; }
        public int TTFPD_StaffConsecutive { get; set; }

        public List<CLGTT_PRDDistributionDetailsDMO> CLGTT_PRDDistributionDetailsDMO { get; set; }
    }
}
