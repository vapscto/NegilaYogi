using IVRMUX.Delegates.com.vapstech.AssetTracking.AssetTag;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using System;

namespace IVRMUX.Controllers.com.vapstech.AssetTracking.AssetTag
{
    [Route("api/[controller]")]
    public class AssetTagCheckIn_ReportController : Controller
    {
        AssetTagCheckIn_ReportDelegate _delegate = new AssetTagCheckIn_ReportDelegate();

      
        [Route("getloaddata")]
        public AssetTagCheckInDTO getloaddata([FromBody] AssetTagCheckInDTO data)
        {          
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("onreport")]
        public AssetTagCheckInDTO onreport([FromBody] AssetTagCheckInDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.onreport(data);
        }
        


    }
}
