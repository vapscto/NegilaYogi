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
    public class INV_VendorPayment_ReportController : Controller
    {
        INV_VendorPayment_ReportDelegate _delegate = new INV_VendorPayment_ReportDelegate();

      
        [Route("getloaddata")]
        public INV_VendorPaymentDTO getloaddata([FromBody] INV_VendorPaymentDTO data)
        {          
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("onreport")]
        public INV_VendorPaymentDTO onreport([FromBody] INV_VendorPaymentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.onreport(data);
        }
        


    }
}
