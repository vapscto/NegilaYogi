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
    public class AssetTagCheckInController : Controller
    {
        AssetTagCheckInDelegate _delegate = new AssetTagCheckInDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public AssetTagCheckInDTO getloaddata(int id)
        {
            AssetTagCheckInDTO data = new AssetTagCheckInDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("getstore")]
        public AssetTagCheckInDTO getstore([FromBody] AssetTagCheckInDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getstore(data);
        }
        [Route("getitems")]
        public AssetTagCheckInDTO getitems([FromBody] AssetTagCheckInDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitems(data);
        }
        [Route("getitemtagdata")]
        public AssetTagCheckInDTO getitemtagdata([FromBody] AssetTagCheckInDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitemtagdata(data);
        }
        [Route("savedata")]
        public AssetTagCheckInDTO savedata([FromBody] AssetTagCheckInDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedata(data);
        }
        [Route("deactive")]
        public AssetTagCheckInDTO deactive([FromBody] AssetTagCheckInDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }


    }
}
