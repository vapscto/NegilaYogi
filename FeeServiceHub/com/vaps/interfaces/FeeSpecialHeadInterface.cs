using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
 public   interface FeeSpecialHeadInterface
    {
        FeeSpecialFeeGroupDTO SaveYearlyGroupDataY(FeeSpecialFeeGroupDTO org);
        FeeSpecialFeeGroupDTO getdetailsY(int id);
        FeeSpecialFeeGroupDTO deactivateY(FeeSpecialFeeGroupDTO id);
        FeeSpecialFeeGroupDTO getpageeditY(int id);
        FeeSpecialFeeGroupDTO deleterecY(FeeSpecialFeeGroupDTO data);
    }
}
