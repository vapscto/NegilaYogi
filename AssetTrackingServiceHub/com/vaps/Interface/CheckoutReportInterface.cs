using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Interface
{
    public interface CheckoutReportInterface
    {
        CheckOutAssetsDTO getloaddata(CheckOutAssetsDTO data);
        Task<CheckOutAssetsDTO> getreport(CheckOutAssetsDTO data);

    }
}
