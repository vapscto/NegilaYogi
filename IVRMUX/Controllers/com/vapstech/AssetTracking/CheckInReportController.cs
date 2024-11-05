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
    public class CheckInReportController : Controller
    {
        CheckInReportDelegate _delegate = new CheckInReportDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public CheckInAssetsDTO getloaddata(int id)
        {
            CheckInAssetsDTO data = new CheckInAssetsDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
       
        [Route("getreport")]
        public CheckInAssetsDTO getreport([FromBody] CheckInAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getreport(data);
        }


    }
}
