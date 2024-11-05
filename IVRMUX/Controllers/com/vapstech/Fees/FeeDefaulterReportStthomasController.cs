using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class FeeDefaulterReportStthomasController : Controller
    {

        FeeDefaulterReportStthomasDelegate feeTrailAuditreport = new FeeDefaulterReportStthomasDelegate();



        [HttpPost]
        [Route("getreport")]
        public FeeDefaulterReportStthomasDTO getreport([FromBody]FeeDefaulterReportStthomasDTO data123)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_ID = mid;
            data123.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return feeTrailAuditreport.getreport(data123);
        }
    }
}
