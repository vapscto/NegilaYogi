using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Inventory.Master;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Inventory.Master
{
    [Route("api/[controller]")]
    public class INV_StockSummary : Controller
    {
        INV_StockSummaryDelegate _delegate = new INV_StockSummaryDelegate();


        [Route("getloaddata")]
        public INV_StockSummaryDTO getloaddata([FromBody] INV_StockSummaryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("onreport")]
        public INV_StockSummaryDTO onreport([FromBody] INV_StockSummaryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.onreport(data);
        }
        [Route("onreporttwo")]
        public INV_StockSummaryDTO onreporttwo([FromBody] INV_StockSummaryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.onreporttwo(data);
        }
        [Route("get_load_onchange")]
        public INV_StockSummaryDTO get_load_onchange([FromBody] INV_StockSummaryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.get_load_onchange(data);
        }
        //onreportthree
        [Route("onreportthree")]
        public INV_StockSummaryDTO onreportthree([FromBody] INV_StockSummaryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.onreportthree(data);
        }
        //getstudent
        [Route("getstudent")]
        public INV_StockSummaryDTO getstudent([FromBody] INV_StockSummaryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getstudent(data);
        }
        //getstudent

        [Route("onreportstock")]
        public INV_StockSummaryDTO onreportstock([FromBody] INV_StockSummaryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.onreportstock(data);
        }
    }
}
