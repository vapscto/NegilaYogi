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
    public class TransferAssetsController : Controller
    {
        TransferAssetsDelegate _delegate = new TransferAssetsDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public TransferAssetsDTO getloaddata(int id)
        {
            TransferAssetsDTO data = new TransferAssetsDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
        [Route("gettolocations")]
        public TransferAssetsDTO gettolocations([FromBody] TransferAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.gettolocations(data);
        }
        [Route("getitemdetails")]
        public TransferAssetsDTO getitemdetails([FromBody] TransferAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitemdetails(data);
        }
       

        [Route("savedetails")]
        public TransferAssetsDTO savedetails([FromBody] TransferAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetails(data);
        }

        [Route("deactive")]
        public TransferAssetsDTO deactive([FromBody] TransferAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }
  
      



    }
}
