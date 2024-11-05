using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Interface
{
    public interface AssetTagCheckOutInterface
    {
        AssetTagCheckOutDTO getloaddata(AssetTagCheckOutDTO data);

        AssetTagCheckOutDTO getitems(AssetTagCheckOutDTO data);
        Task<AssetTagCheckOutDTO> getitemtagdata(AssetTagCheckOutDTO data);
        AssetTagCheckOutDTO savedata(AssetTagCheckOutDTO data);
        AssetTagCheckOutDTO deactive(AssetTagCheckOutDTO data);



    }
}
