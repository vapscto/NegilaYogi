using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Interface
{
    public interface AssetTag_ReportInterface
    {
       Task<AssetTagDTO> getloaddata(AssetTagDTO data);
        Task<AssetTagDTO> onreport(AssetTagDTO data);


    }


}
