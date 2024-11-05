using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Interface
{
    public interface AssetTagInterface
    {
        AssetTagDTO getloaddata(AssetTagDTO data);
        Task<AssetTagDTO> getdata(AssetTagDTO data);
        AssetTagDTO savedata(AssetTagDTO data);
        AssetTagDTO deactive(AssetTagDTO data);

        

    }
}
