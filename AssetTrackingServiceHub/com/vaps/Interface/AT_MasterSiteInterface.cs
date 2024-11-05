using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Interface
{
  public interface AT_MasterSiteInterface
    {
        AT_MasterSiteDTO getloaddata(AT_MasterSiteDTO data);
        AT_MasterSiteDTO savedetails(AT_MasterSiteDTO data);
        AT_MasterSiteDTO deactive(AT_MasterSiteDTO data);
    }
}
