using IVRMUX.Delegates.com.vapstech.AssetTracking.AssetTag;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using System;

namespace IVRMUX.Controllers.com.vapstech.AssetTracking.AssetTag
{
    [Route("api/[controller]")]
    public class AssetTagCheckout_ReportController : Controller
    {
        AssetTagCheckout_ReportDelegate _delegate = new AssetTagCheckout_ReportDelegate();

      
        [Route("getloaddata")]
        public AssetTagCheckOutDTO getloaddata([FromBody] AssetTagCheckOutDTO data)
        {          
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("onreport")]
        public AssetTagCheckOutDTO onreport([FromBody] AssetTagCheckOutDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.onreport(data);
        }
        


    }
}
