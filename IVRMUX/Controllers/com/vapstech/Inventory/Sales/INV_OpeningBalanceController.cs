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
    public class INV_OpeningBalanceController : Controller
    {
        INV_OpeningBalanceDelegate _delegate = new INV_OpeningBalanceDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_OpeningBalanceDTO getloaddata(int id)
        {
            INV_OpeningBalanceDTO data = new INV_OpeningBalanceDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
      
        [Route("savedetails")]
        public INV_OpeningBalanceDTO savedetails([FromBody] INV_OpeningBalanceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetails(data);
        }
       
        [Route("deactive")]
        public INV_OpeningBalanceDTO deactive([FromBody] INV_OpeningBalanceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }
        [Route("getobdetails")]
        public INV_OpeningBalanceDTO getobdetails([FromBody] INV_OpeningBalanceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getobdetails(data);
        }
        [Route("move_to_stock")]
        public INV_OpeningBalanceDTO move_to_stock([FromBody] INV_OpeningBalanceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.move_to_stock(data);
        }

        
    }
}
