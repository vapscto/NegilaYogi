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
    public class AssetTagController : Controller
    {
        AssetTagDelegate _delegate = new AssetTagDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public AssetTagDTO getloaddata(int id)
        {
            AssetTagDTO data = new AssetTagDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
        [Route("getdata")]
        public AssetTagDTO getdata([FromBody] AssetTagDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getdata(data);
        }
        [Route("savedata")]
        public AssetTagDTO savedata([FromBody] AssetTagDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedata(data);
        }
        [Route("deactive")]
        public AssetTagDTO deactive([FromBody] AssetTagDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }
        

    }
}
