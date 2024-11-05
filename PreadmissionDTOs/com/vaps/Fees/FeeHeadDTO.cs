using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeHeadDTO:CommonParamDTO
    {
        public long FMH_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMH_FeeName { get; set; }
        public string FMH_Flag { get; set; }
        public bool FMH_RefundFlag { get; set; }
        public bool FMH_PDAFlag { get; set; }
        public bool FMH_SpecialFeeFlag { get; set; }
        public int FMH_Order { get; set; }
        public bool FMH_ActiveFlag { get; set; }
        public long user_id { get; set; }



        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array GroupHeadData { get; set; }

        public bool dupr { get; set; }

        public FeeHeadDTO[] CourseDTO { get; set; }

        public string message { get; set; }
    }
}
