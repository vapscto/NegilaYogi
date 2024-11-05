using IVRMUX.Delegates.com.vapstech.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.Inventory
{
    [Route("api/[controller]")]
    public class INV_PurchaseOrderController : Controller
    {
        INV_PurchaseOrderDelegate _delegate = new INV_PurchaseOrderDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_PurchaseOrderDTO getloaddata(int id)
        {
            INV_PurchaseOrderDTO data = new INV_PurchaseOrderDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("getqtDetail")]
        public INV_PurchaseOrderDTO getqtDetail([FromBody] INV_PurchaseOrderDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getqtDetail(data);
        }
        [Route("getitemtax")]
        public INV_PurchaseOrderDTO getitemtax([FromBody] INV_PurchaseOrderDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitemtax(data);
        }        

        [Route("savedetails")]
        public INV_PurchaseOrderDTO savedetails([FromBody] INV_PurchaseOrderDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedetails(data);
        }

        [Route("deactiveM")]
        public INV_PurchaseOrderDTO deactiveM([FromBody] INV_PurchaseOrderDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveM(data);
        }
        [Route("deactive")]
        public INV_PurchaseOrderDTO deactive([FromBody] INV_PurchaseOrderDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }
        [Route("deactiveTx")]
        public INV_PurchaseOrderDTO deactiveTx([FromBody] INV_PurchaseOrderDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveTx(data);
        }

        [Route("get_modeldetails")]
        public INV_PurchaseOrderDTO get_modeldetails([FromBody] INV_PurchaseOrderDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.get_modeldetails(data);
        }

        





    }
}
