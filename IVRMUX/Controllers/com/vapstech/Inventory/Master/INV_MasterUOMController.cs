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
    public class INV_MasterUOMController : Controller
    {
        INV_MasterUOMDelegate _delegate = new INV_MasterUOMDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_Master_UOMDTO getloaddata(int id)
        {
            INV_Master_UOMDTO data = new INV_Master_UOMDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("savedetails")]
        public INV_Master_UOMDTO savedetails([FromBody] INV_Master_UOMDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetails(data);
        }
       
        [Route("deactive")]
        public INV_Master_UOMDTO deactive([FromBody] INV_Master_UOMDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }

        
    }
}
