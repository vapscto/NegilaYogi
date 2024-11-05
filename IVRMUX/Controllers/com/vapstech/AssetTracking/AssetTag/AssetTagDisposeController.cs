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
    public class AssetTagDisposeController : Controller
    {
        AssetTagDisposeDelegate _delegate = new AssetTagDisposeDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public AssetTagDisposeDTO getloaddata(int id)
        {
            AssetTagDisposeDTO data = new AssetTagDisposeDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("getlocation")]
        public AssetTagDisposeDTO getlocation([FromBody] AssetTagDisposeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getlocation(data);
        }
        [Route("getitems")]
        public AssetTagDisposeDTO getitems([FromBody] AssetTagDisposeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitems(data);
        }
        [Route("getitemtagdata")]
        public AssetTagDisposeDTO getitemtagdata([FromBody] AssetTagDisposeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitemtagdata(data);
        }
        [Route("savedata")]
        public AssetTagDisposeDTO savedata([FromBody] AssetTagDisposeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedata(data);
        }
        [Route("deactive")]
        public AssetTagDisposeDTO deactive([FromBody] AssetTagDisposeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }


    }
}
