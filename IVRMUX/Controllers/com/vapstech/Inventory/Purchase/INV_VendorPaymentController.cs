
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
    public class INV_VendorPaymentController : Controller
    {
        INV_VendorPaymentDelegate _delegate = new INV_VendorPaymentDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_VendorPaymentDTO getloaddata(int id)
        {
            INV_VendorPaymentDTO data = new INV_VendorPaymentDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("getGRNdetail")]
        public INV_VendorPaymentDTO getGRNdetail([FromBody] INV_VendorPaymentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getGRNdetail(data);
        }

        [Route("savedetails")]
        public INV_VendorPaymentDTO savedetails([FromBody] INV_VendorPaymentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedetails(data);
        }

        [Route("deactive")]
        public INV_VendorPaymentDTO deactive([FromBody] INV_VendorPaymentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }
         [Route("deactiveGRN")]
        public INV_VendorPaymentDTO deactiveGRN([FromBody] INV_VendorPaymentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveGRN(data);
        }

          [Route("getmodeldetail")]
        public INV_VendorPaymentDTO getmodeldetail([FromBody] INV_VendorPaymentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getmodeldetail(data);
        }








    }
}
