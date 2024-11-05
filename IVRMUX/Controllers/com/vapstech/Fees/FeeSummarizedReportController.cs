using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class FeeSummarizedReportController :Controller
    {
        FeeSummarizedReportDelegate FSRC = new FeeSummarizedReportDelegate();

        [Route("getalldetails")]
        public FeeTransactionPaymentDTO getalldetails([FromBody]FeeTransactionPaymentDTO data)
        {
           // FeeTransactionPaymentDTO data123 = new FeeTransactionPaymentDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            return FSRC.details(data);
        }

        [HttpPost]
        [Route("getsection")]
        public FeeTransactionPaymentDTO getsection([FromBody]FeeTransactionPaymentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            return FSRC.getsection(data);
        }

        [HttpPost]
        [Route("getstudent")]
        public FeeTransactionPaymentDTO getstudent([FromBody]FeeTransactionPaymentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            return FSRC.getstudent(data);
        }


        [HttpPost]
        [Route("getreport")]
        public FeeTransactionPaymentDTO getreport([FromBody]FeeTransactionPaymentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return FSRC.getradiofiltereddata(data);
        }
        //[Route("getreport/")]
        //public FeeTransactionPaymentDTO getreport([FromBody]FeeTransactionPaymentDTO data)
        //{
        //    return FSRC.getradiofiltereddata(data);
        //}

        [Route("get_groups")]
        public FeeTransactionPaymentDTO get_groups([FromBody]FeeTransactionPaymentDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            return FSRC.get_groups(value);
        }

    }

}
