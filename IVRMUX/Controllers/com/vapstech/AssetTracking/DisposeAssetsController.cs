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
    public class DisposeAssetsController : Controller
    {
        DisposeAssetsDelegate _delegate = new DisposeAssetsDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public DisposeAssetsDTO getloaddata(int id)
        {
            DisposeAssetsDTO data = new DisposeAssetsDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
        [Route("getlocations")]
        public DisposeAssetsDTO getlocations([FromBody] DisposeAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getlocations(data);
        }
        [Route("getitems")]
        public DisposeAssetsDTO getitems([FromBody] DisposeAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitems(data);
        }
        [Route("getdetails")]
        public DisposeAssetsDTO getdetails([FromBody] DisposeAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getdetails(data);
        }

        [Route("savedetails")]
        public DisposeAssetsDTO savedetails([FromBody] DisposeAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetails(data);
        }

        [Route("deactive")]
        public DisposeAssetsDTO deactive([FromBody] DisposeAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }
  
      



    }
}
