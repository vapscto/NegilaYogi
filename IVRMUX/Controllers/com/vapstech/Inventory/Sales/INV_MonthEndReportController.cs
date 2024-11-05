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
    public class INV_MonthEndReportController : Controller
    {
        INV_MonthEndReportDelegate _delegate = new INV_MonthEndReportDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public INV_MonthEndReportDTO getloaddata(int id)
        {
            INV_MonthEndReportDTO data = new INV_MonthEndReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delegate.getloaddata(data);
        }

        [Route("getmonthreport")]
        public INV_MonthEndReportDTO getmonthreport([FromBody] INV_MonthEndReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getmonthreport(data);
        }
     


    }
}
