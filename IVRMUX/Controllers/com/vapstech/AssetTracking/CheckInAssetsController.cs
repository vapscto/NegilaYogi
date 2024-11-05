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
    public class CheckInAssetsController : Controller
    {
        CheckInAssetsDelegate _delegate = new CheckInAssetsDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public CheckInAssetsDTO getloaddata(int id)
        {
            CheckInAssetsDTO data = new CheckInAssetsDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
        [Route("getStore")]
        public CheckInAssetsDTO getStore([FromBody] CheckInAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getStore(data);
        }
        [Route("getitems")]
        public CheckInAssetsDTO getitems([FromBody] CheckInAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitems(data);
        }
        [Route("getdetails")]
        public CheckInAssetsDTO getdetails([FromBody] CheckInAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getdetails(data);
        }
        

        [Route("savedetails")]
        public CheckInAssetsDTO savedetails([FromBody] CheckInAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetails(data);
        }

        [Route("deactive")]
        public CheckInAssetsDTO deactive([FromBody] CheckInAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }
  
       


    }
}
