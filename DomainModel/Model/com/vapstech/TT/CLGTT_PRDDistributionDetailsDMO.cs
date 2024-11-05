using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Final_Period_Distribution_Detailed_College")]
    public class CLGTT_PRDDistributionDetailsDMO: CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTFPDDC_Id { get; set; }
         public long TTFPD_Id { get; set; }
        public long TTMC_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int TTFPDC_TotalPeriods { get; set; }
        public int TTFPDC_AllotedPeriods { get; set; }
        public int TTFPDC_AvailablePeriods { get; set; }
        public bool TTFPDC_ActiveFlag { get; set; }

    }
}
