using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Interface
{
    public interface AssetTagTransferInterface
    {
        Task<AssetTagTransferDTO> getloaddata(AssetTagTransferDTO data);

        AssetTagTransferDTO getitems(AssetTagTransferDTO data);
        AssetTagTransferDTO gettolocation(AssetTagTransferDTO data);

        Task<AssetTagTransferDTO> getitemtagdata(AssetTagTransferDTO data);
        AssetTagTransferDTO savedata(AssetTagTransferDTO data);
        AssetTagTransferDTO deactive(AssetTagTransferDTO data);



    }
}
