using IVRMUX.Delegates.com.vapstech.AssetTracking;
using IVRMUX.Delegates.com.vapstech.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.AssetTracking
{
    [Route("api/[controller]")]
    public class AssetsReportController : Controller
    {
        AssetsReportDelegate _delegate = new AssetsReportDelegate();

        [Route("getloaddata")]
        public CheckOutAssetsDTO getloaddata([FromBody] CheckOutAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("getreport")]
        public CheckOutAssetsDTO getreport([FromBody] CheckOutAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getreport(data);
        }


    }
}
