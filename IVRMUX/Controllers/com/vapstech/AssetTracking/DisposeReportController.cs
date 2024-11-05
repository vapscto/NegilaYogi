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
    public class DisposeReportController : Controller
    {
        DisposeReportDelegate _delegate = new DisposeReportDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public DisposeAssetsDTO getloaddata(int id)
        {
            DisposeAssetsDTO data = new DisposeAssetsDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
       
        [Route("getreport")]
        public DisposeAssetsDTO getreport([FromBody] DisposeAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getreport(data);
        }


    }
}
