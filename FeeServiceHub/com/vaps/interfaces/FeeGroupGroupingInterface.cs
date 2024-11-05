using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface FeeGroupGroupingInterface
    {
       
        FeeGroupMappingDTO SaveYearlyGroupData(FeeGroupMappingDTO org);

        FeeGroupMappingDTO getdetailsY(int id);
        FeeGroupMappingDTO deactivateY(FeeGroupMappingDTO id);
        FeeGroupMappingDTO getpageeditY(int id);


        FeeGroupMappingDTO deleterecY(int id);
    }
}
