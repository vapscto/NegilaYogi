using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Interface
{
    public interface TransferAssetsInterface
    {
        Task<TransferAssetsDTO> getloaddata(TransferAssetsDTO data);
        TransferAssetsDTO gettolocations(TransferAssetsDTO data);
        TransferAssetsDTO getitemdetails(TransferAssetsDTO data);

        TransferAssetsDTO savedetails(TransferAssetsDTO data);
        TransferAssetsDTO deactive(TransferAssetsDTO data);




    }
}
