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
    public class CheckOutAssetsController : Controller
    {
        CheckOutAssetsDelegate _delegate = new CheckOutAssetsDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public CheckOutAssetsDTO getloaddata(int id)
        {
            CheckOutAssetsDTO data = new CheckOutAssetsDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
        [Route("getitems")]
        public CheckOutAssetsDTO getitems([FromBody] CheckOutAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitems(data);
        }
        [Route("savedetails")]
        public CheckOutAssetsDTO savedetails([FromBody] CheckOutAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetails(data);
        }

        [Route("deactive")]
        public CheckOutAssetsDTO deactive([FromBody] CheckOutAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }
        [Route("getcontactperson")]
        public CheckOutAssetsDTO getcontactperson([FromBody] CheckOutAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getcontactperson(data);
        }
        [Route("get_avaiablestock")]
        public CheckOutAssetsDTO get_avaiablestock([FromBody] CheckOutAssetsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.get_avaiablestock(data);
        }



    }
}
