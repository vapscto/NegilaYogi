using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeYearlygroupHeadMappingDTO
    {
        public long FYGHM_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FMI_Id { get; set; }
        public string FYGHM_FineApplicableFlag { get; set; }
        public string FYGHM_Common_AmountFlag { get; set; }
        public string FYGHM_ActiveFlag { get; set; }

        public Array fillmastergroup { get; set; }
        public Array fillcompany { get; set; }
        public Array fillmasterhead { get; set; }
        public Array fillinstallment { get; set; }

        public bool returnval { get; set; }
        public FeeYearlygroupHeadMappingDTO[] savetmpdata { get; set; }
        public FeeYearlygroupHeadMappingDTO[] savetmpdatains { get; set; }
        public FeeYearlygroupHeadMappingDTO[] savetmpflag { get; set; }
        public Array alldata { get; set; }

        public string FMG_GroupName { get; set; }
        public string FMH_FeeName { get; set; }
        public string FMI_Name { get; set; }

        public string displaymessage { get; set; }

        public Array academicdrp { get; set; }
        public long userid { get; set; }

    }
}
