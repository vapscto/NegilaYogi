using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Interface
{
  public interface CheckInAssetsInterface
    {
        CheckInAssetsDTO getloaddata(CheckInAssetsDTO data);
        CheckInAssetsDTO getStore(CheckInAssetsDTO data);
        CheckInAssetsDTO getitems(CheckInAssetsDTO data);
        CheckInAssetsDTO getdetails(CheckInAssetsDTO data);        
        CheckInAssetsDTO savedetails(CheckInAssetsDTO data);
        CheckInAssetsDTO deactive(CheckInAssetsDTO data);
    
      

        
    }
}
