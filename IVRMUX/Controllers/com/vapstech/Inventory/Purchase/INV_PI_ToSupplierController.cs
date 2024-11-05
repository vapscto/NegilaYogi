
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
    public class INV_PI_ToSupplierController : Controller
    {
        INV_PI_ToSupplierDelegate _delegate = new INV_PI_ToSupplierDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_PI_ToSupplierDTO getloaddata(int id)
        {
            INV_PI_ToSupplierDTO data = new INV_PI_ToSupplierDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("getpiDetail")]
        public INV_PI_ToSupplierDTO getpiDetail([FromBody] INV_PI_ToSupplierDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getpiDetail(data);
        }

        [Route("savedetails")]
        public INV_PI_ToSupplierDTO savedetails([FromBody] INV_PI_ToSupplierDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedetails(data);
        }

        [Route("deactive")]
        public INV_PI_ToSupplierDTO deactive([FromBody] INV_PI_ToSupplierDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }








    }
}
