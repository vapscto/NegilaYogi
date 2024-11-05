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
    public class AT_MasterSiteController : Controller
    {
        AT_MasterSiteDelegate _delegate = new AT_MasterSiteDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public AT_MasterSiteDTO getloaddata(int id)
        {
            AT_MasterSiteDTO data = new AT_MasterSiteDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
        [Route("savedetails")]
        public AT_MasterSiteDTO savedetails([FromBody] AT_MasterSiteDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetails(data);
        }

        [Route("deactive")]
        public AT_MasterSiteDTO deactive([FromBody] AT_MasterSiteDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }




    }
}
