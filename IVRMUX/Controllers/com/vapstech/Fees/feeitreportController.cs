using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using Microsoft.Extensions.Options;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.Fees;

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class feeitreportController:Controller
    {
        feeitreportDelegate FGD = new feeitreportDelegate();
       
        // GET api/values/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeTransactionPaymentDTO Get(int id)
        {
            FeeTransactionPaymentDTO dt = new FeeTransactionPaymentDTO();

            //id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dt.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dt.userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dt.yearid = ASMAY_Id;

            return FGD.getdetails(dt);
        }
        [HttpPost]
        [Route("getsection")]
        public FeeTransactionPaymentDTO getsection([FromBody]FeeTransactionPaymentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return FGD.getsection(data);
        }
        [HttpPost]
        [Route("getstudent")]
        public FeeTransactionPaymentDTO getstudent([FromBody]FeeTransactionPaymentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return FGD.getstudent(data);
        }

        [HttpPost]
        [Route("getreceipt")]
        public FeeTransactionPaymentDTO getreceipt([FromBody]FeeTransactionPaymentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return FGD.getreceipt(data);
        }

        [Route("printreceipt")]
        public FeeStudentTransactionDTO printrec([FromBody] FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.yearid = ASMAY_Id;

            return FGD.printrecdelegate(data);
        }

        [HttpPost]
        [Route("getreceiptreport")]
        public FeeTransactionPaymentDTO getreceiptreport([FromBody]FeeTransactionPaymentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return FGD.getreceiptreport(data);
        }
    }
}
