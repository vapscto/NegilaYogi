using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Interface
{
  public interface DisposeAssetsInterface
    {
        DisposeAssetsDTO getloaddata(DisposeAssetsDTO data);
        DisposeAssetsDTO getlocations(DisposeAssetsDTO data);
        DisposeAssetsDTO getitems(DisposeAssetsDTO data);
        DisposeAssetsDTO getdetails(DisposeAssetsDTO data);
        
        DisposeAssetsDTO savedetails(DisposeAssetsDTO data);
        DisposeAssetsDTO deactive(DisposeAssetsDTO data);
    
      

        
    }
}
