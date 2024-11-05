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
    public class INV_MasterItemController : Controller
    {
        INV_MasterItemDelegate _delegate = new INV_MasterItemDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public INV_Master_ItemDTO getloaddata(int id)
        {
            INV_Master_ItemDTO data = new INV_Master_ItemDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("savedetails")]
        public INV_Master_ItemDTO savedetails([FromBody] INV_Master_ItemDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetails(data);
        }
       
        [Route("deactive")]
        public INV_Master_ItemDTO deactive([FromBody] INV_Master_ItemDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }
        [Route("deactiveitax")]
        public INV_Master_ItemDTO deactiveitax([FromBody] INV_Master_ItemDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveitax(data);
        }

        
        [Route("itemTax")]
        public INV_Master_ItemDTO itemTax([FromBody] INV_Master_ItemDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.itemTax(data);
        }
        

    }
}
