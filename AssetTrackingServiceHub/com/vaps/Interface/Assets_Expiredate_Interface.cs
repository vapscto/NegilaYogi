using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Interface
{
    public interface Assets_Expiredate_Interface
    {

        Asset_Expiredate_DTO get_expdata(Asset_Expiredate_DTO dto);
    }
}
