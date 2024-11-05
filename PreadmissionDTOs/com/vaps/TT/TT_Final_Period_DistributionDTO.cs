using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TT_Final_Period_DistributionDTO
    {

        public long TTFPD_Id { get; set; }
        public long MI_Id { get; set; }
        public long TTMC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long IMS_Id { get; set; }
        public decimal TTFPD_TotalPeriods { get; set; }
        public decimal TTFPD_AllotedPeriods { get; set; }
        public decimal TTFPD_AvailablePeriods { get; set; }
        public bool TTFPD_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
