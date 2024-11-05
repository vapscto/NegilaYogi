using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface FeeMasterConfigInterface
    {
        FeeMasterConfigurationDTO SaveconfigData(FeeMasterConfigurationDTO org);
        FeeMasterConfigurationDTO getdetailsY(FeeMasterConfigurationDTO data);
        FeeMasterConfigurationDTO editdetails(FeeMasterConfigurationDTO data);
    }
}
