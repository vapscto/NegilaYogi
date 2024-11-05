using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Interface
{
    public interface AssetTagDisposeInterface
    {
        AssetTagDisposeDTO getloaddata(AssetTagDisposeDTO data);
        AssetTagDisposeDTO getlocation(AssetTagDisposeDTO data);
        AssetTagDisposeDTO getitems(AssetTagDisposeDTO data);
        AssetTagDisposeDTO getitemtagdata(AssetTagDisposeDTO data);
        AssetTagDisposeDTO savedata(AssetTagDisposeDTO data);
        AssetTagDisposeDTO deactive(AssetTagDisposeDTO data);



    }
}
