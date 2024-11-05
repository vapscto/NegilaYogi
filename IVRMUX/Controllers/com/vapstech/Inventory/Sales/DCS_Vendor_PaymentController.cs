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
    public class DCS_Vendor_PaymentController : Controller
    {
        DCS_Vendor_PaymentDelegate _delegate = new DCS_Vendor_PaymentDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_T_SalesDTO getloaddata(int id)
        {
            INV_T_SalesDTO data = new INV_T_SalesDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
      
        [Route("getitem")]
        public INV_T_SalesDTO getitem([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitem(data);
        }
        [Route("getitemDetail")]
        public INV_T_SalesDTO getitemDetail([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));            
            return _delegate.getitemDetail(data);
        }

        [Route("savedetails")]
        public INV_T_SalesDTO savedetails([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedetails(data);
        }
        [Route("getSaletypes")]
        public INV_T_SalesDTO getSaletypes([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));       
            return _delegate.getSaletypes(data);
        }
        [Route("getbilldetails")]
        public INV_T_SalesDTO getbilldetails([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getbilldetails(data);
        }
        [Route("getSaleItemTax")]
        public INV_T_SalesDTO getSaleItemTax([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getSaleItemTax(data);
        }
        [Route("deactive")]
        public INV_T_SalesDTO deactive([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IMFY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("IMFY_Id"));
            return _delegate.deactive(data);
        }
        [Route("deactiveS")]
        public INV_T_SalesDTO deactiveS([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveS(data);
        }
        [Route("deactivetax")]
        public INV_T_SalesDTO deactivetax([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactivetax(data);
        }



    }
}
