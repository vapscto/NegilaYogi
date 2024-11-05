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
    public class INV_QuotationController : Controller
    {
        INV_QuotationDelegate _delegate = new INV_QuotationDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_QuotationDTO getloaddata(int id)
        {
            INV_QuotationDTO data = new INV_QuotationDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("getpiDetail")]
        public INV_QuotationDTO getpiDetail([FromBody] INV_QuotationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getpiDetail(data);
        }
        [Route("getquotationdetails")]
        public INV_QuotationDTO getquotationdetails([FromBody] INV_QuotationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getquotationdetails(data);
        }

        

        [Route("savedetails")]
        public INV_QuotationDTO savedetails([FromBody] INV_QuotationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedetails(data);
        }

        [Route("deactiveM")]
        public INV_QuotationDTO deactiveM([FromBody] INV_QuotationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveM(data);
        }
        [Route("deactive")]
        public INV_QuotationDTO deactive([FromBody] INV_QuotationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }







    }
}
