using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Interface
{
    public interface AssetTagCheckInInterface
    {
        AssetTagCheckInDTO getloaddata(AssetTagCheckInDTO data);
        AssetTagCheckInDTO getstore(AssetTagCheckInDTO data);
        AssetTagCheckInDTO getitems(AssetTagCheckInDTO data);
        AssetTagCheckInDTO getitemtagdata(AssetTagCheckInDTO data);
        AssetTagCheckInDTO savedata(AssetTagCheckInDTO data);
        AssetTagCheckInDTO deactive(AssetTagCheckInDTO data);



    }
}
