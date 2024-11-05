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
    public class INV_MasterStoreController : Controller
    {
        INV_MasterStoreDelegate _delegate = new INV_MasterStoreDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_Master_StoreDTO getloaddata(int id)
        {
            INV_Master_StoreDTO data = new INV_Master_StoreDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("savedetails")]
        public INV_Master_StoreDTO savedetails([FromBody] INV_Master_StoreDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetails(data);
        }
       
        [Route("deactive")]
        public INV_Master_StoreDTO deactive([FromBody] INV_Master_StoreDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }

        
    }
}
