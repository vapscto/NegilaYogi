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
    public class AssetTagTransferController : Controller
    {
        AssetTagTransferDelegate _delegate = new AssetTagTransferDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public AssetTagTransferDTO getloaddata(int id)
        {
            AssetTagTransferDTO data = new AssetTagTransferDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("getitems")]
        public AssetTagTransferDTO getitems([FromBody] AssetTagTransferDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitems(data);
        }
        [Route("gettolocation")]
        public AssetTagTransferDTO gettolocation([FromBody] AssetTagTransferDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.gettolocation(data);
        }
        [Route("getitemtagdata")]
        public AssetTagTransferDTO getitemtagdata([FromBody] AssetTagTransferDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitemtagdata(data);
        }
        [Route("savedata")]
        public AssetTagTransferDTO savedata([FromBody] AssetTagTransferDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedata(data);
        }
        [Route("deactive")]
        public AssetTagTransferDTO deactive([FromBody] AssetTagTransferDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }


    }
}
