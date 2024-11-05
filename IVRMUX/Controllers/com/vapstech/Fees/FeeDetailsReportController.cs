using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.Fees;

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]

    public class FeeDetailsReportController:Controller
    {
        FeeDetailsReportDelegate FCWR = new FeeDetailsReportDelegate();
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeTransactionPaymentDTO Get(int id)
        {
            FeeTransactionPaymentDTO data = new FeeTransactionPaymentDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return FCWR.getdetails(data);
        }
       

        [HttpPost]
        [Route("getheadisbygrpid")]
        public FeeTransactionPaymentDTO getheadisbygrpid([FromBody]FeeTransactionPaymentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return FCWR.getheadisbygrpid(data);
        }

        [HttpPost]
        [Route("getsection")]
        public FeeTransactionPaymentDTO getsection([FromBody]FeeTransactionPaymentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return FCWR.getsection(data);
        }


        [HttpPost]
        [Route("radiobtndata")]
        public FeeTransactionPaymentDTO radiobtndata([FromBody]FeeTransactionPaymentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return FCWR.radiobtndata(data);
        }

    }
}

