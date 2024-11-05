using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Library.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Library.Reports
{
    [Route("api/[controller]")]
    public class SubscriptionReportController : Controller
    {
        private SubscriptionReportDeletgate _delget = new SubscriptionReportDeletgate();

        [Route("getdetails/{id:int}")]
        public NonBookReport_DTO getdetails(int id)
        {
            NonBookReport_DTO obj = new NonBookReport_DTO();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delget.getdetails(obj);
        }

        [Route("get_report")]
        public NonBookReport_DTO get_report([FromBody] NonBookReport_DTO obj)
        {
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delget.get_report(obj);
        }
    }
}
