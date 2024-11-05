using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Interface
{
    public interface DisposeReportInterface
    {
        DisposeAssetsDTO getloaddata(DisposeAssetsDTO data);
        Task<DisposeAssetsDTO> getreport(DisposeAssetsDTO data);

    }
}
