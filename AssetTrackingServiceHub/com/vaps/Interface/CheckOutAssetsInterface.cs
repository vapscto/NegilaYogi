using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Interface
{
  public interface CheckOutAssetsInterface
    {
        CheckOutAssetsDTO getloaddata(CheckOutAssetsDTO data);
        CheckOutAssetsDTO getitems(CheckOutAssetsDTO data);
        CheckOutAssetsDTO savedetails(CheckOutAssetsDTO data);
        CheckOutAssetsDTO deactive(CheckOutAssetsDTO data);
        CheckOutAssetsDTO getcontactperson(CheckOutAssetsDTO data);
       CheckOutAssetsDTO get_avaiablestock(CheckOutAssetsDTO data);

        
    }
}
