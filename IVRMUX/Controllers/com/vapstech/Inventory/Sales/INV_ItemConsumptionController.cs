using IVRMUX.Delegates.com.vapstech.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.Inventory
{
    [Route("api/[controller]")]
    public class INV_ItemConsumptionController : Controller
    {
        INV_ItemConsumptionDelegate _delegate = new INV_ItemConsumptionDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_ItemConsumptionDTO getloaddata(int id)
        {
            INV_ItemConsumptionDTO data = new INV_ItemConsumptionDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("savedetails")]
        public INV_ItemConsumptionDTO savedetails([FromBody] INV_ItemConsumptionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.savedetails(data);
        }

        [Route("deactive")]
        public INV_ItemConsumptionDTO deactive([FromBody] INV_ItemConsumptionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }
        [Route("deactiveSub")]
        public INV_ItemConsumptionDTO deactiveSub([FromBody] INV_ItemConsumptionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveSub(data);
        }
        [Route("getobdetails")]
        public INV_ItemConsumptionDTO getobdetails([FromBody] INV_ItemConsumptionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getobdetails(data);
        }
        [Route("getICDetails")]
        public INV_ItemConsumptionDTO getICDetails([FromBody] INV_ItemConsumptionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getICDetails(data);
        }
        [Route("getsection")]
        public INV_ItemConsumptionDTO getclass([FromBody] INV_ItemConsumptionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getsection(data);
        }
         [Route("getstudent")]
        public INV_ItemConsumptionDTO getstudent([FromBody] INV_ItemConsumptionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getstudent(data);
        }


    }
}
