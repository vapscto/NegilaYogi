using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
  public interface SpecialFeeHeadClgInterface
    {
        
        SpecialFeeHeadClgDTO SaveYearlyGroupDataY(SpecialFeeHeadClgDTO org);
        SpecialFeeHeadClgDTO getdetailsY(int id);
        SpecialFeeHeadClgDTO deactivateY(SpecialFeeHeadClgDTO id);
        SpecialFeeHeadClgDTO getpageeditY(int id);
        SpecialFeeHeadClgDTO deleterecY(SpecialFeeHeadClgDTO data);
    }
}
