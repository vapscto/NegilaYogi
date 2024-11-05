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
    public class INV_PurchaseIndentController : Controller
    {
        INV_PurchaseIndentDelegate _delegate = new INV_PurchaseIndentDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_PurchaseIndentDTO getloaddata(int id)
        {
            INV_PurchaseIndentDTO data = new INV_PurchaseIndentDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("savedetails")]
        public INV_PurchaseIndentDTO savedetails([FromBody] INV_PurchaseIndentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedetails(data);
        }

        [Route("getpidetails")]
        public INV_PurchaseIndentDTO getpidetails([FromBody] INV_PurchaseIndentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getpidetails(data);
        }
        [Route("getprDetail")]
        public INV_PurchaseIndentDTO getprDetail([FromBody] INV_PurchaseIndentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getprDetail(data);
        }

        [Route("deactiveM")]
        public INV_PurchaseIndentDTO deactiveM([FromBody] INV_PurchaseIndentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactiveM(data);
        }
        [Route("deactive")]
        public INV_PurchaseIndentDTO deactive([FromBody] INV_PurchaseIndentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactive(data);
        }
        [Route("get_details")]
        public INV_PurchaseIndentDTO get_details([FromBody] INV_PurchaseIndentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.get_details(data);
        }
        [Route("edit")]
        public INV_PurchaseIndentDTO edit([FromBody] INV_PurchaseIndentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.edit(data);
        }
        [Route("genrateReceipt")]
        public INV_PurchaseIndentDTO genrateReceipt([FromBody] INV_PurchaseIndentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.genrateReceipt(data);
        }


    }
}
