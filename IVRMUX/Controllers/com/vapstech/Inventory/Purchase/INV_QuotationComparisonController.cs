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
    public class INV_QuotationComparisonController : Controller
    {
        INV_QuotationComparisonDelegate _delegate = new INV_QuotationComparisonDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_QuotationDTO getloaddata(int id)
        {
            INV_QuotationDTO data = new INV_QuotationDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        

        [Route("getpisupplier")]
        public INV_QuotationDTO getpisupplier([FromBody] INV_QuotationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getpisupplier(data);
        }

        [Route("get_Comparison")]
        public INV_QuotationDTO get_Comparison([FromBody] INV_QuotationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.get_Comparison(data);
        }
        [Route("getqtdetails")]
        public INV_QuotationDTO getqtdetails([FromBody] INV_QuotationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getqtdetails(data);
        }
        [Route("savedata")]
        public INV_QuotationDTO savedata([FromBody] INV_QuotationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedata(data);
        }
        

    }
}
