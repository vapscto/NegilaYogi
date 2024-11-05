using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Interface
{
  public interface AT_MasterLocationInterface
    {
        AT_MasterLocationDTO getloaddata(AT_MasterLocationDTO data);
        AT_MasterLocationDTO savedetails(AT_MasterLocationDTO data);
        AT_MasterLocationDTO deactive(AT_MasterLocationDTO data);
      

        
    }
}
