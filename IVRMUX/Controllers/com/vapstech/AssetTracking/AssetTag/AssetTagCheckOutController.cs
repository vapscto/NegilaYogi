using IVRMUX.Delegates.com.vapstech.AssetTracking;
using IVRMUX.Delegates.com.vapstech.AssetTracking.AssetTag;
using IVRMUX.Delegates.com.vapstech.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.AssetTracking.AssetTag
{
    [Route("api/[controller]")]
    public class AssetTagCheckOutController : Controller
    {
        AssetTagCheckOutDelegate _delegate = new AssetTagCheckOutDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public AssetTagCheckOutDTO getloaddata(int id)
        {
            AssetTagCheckOutDTO data = new AssetTagCheckOutDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("getitems")]
        public AssetTagCheckOutDTO getitems([FromBody] AssetTagCheckOutDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitems(data);
        }
        [Route("getitemtagdata")]
        public AssetTagCheckOutDTO getitemtagdata([FromBody] AssetTagCheckOutDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitemtagdata(data);
        }
        [Route("savedata")]
        public AssetTagCheckOutDTO savedata([FromBody] AssetTagCheckOutDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedata(data);
        }
        [Route("deactive")]
        public AssetTagCheckOutDTO deactive([FromBody] AssetTagCheckOutDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }


    }
}
