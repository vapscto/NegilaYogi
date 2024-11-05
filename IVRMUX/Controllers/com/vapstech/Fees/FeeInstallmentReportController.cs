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
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeInstallmentReportController : Controller
    {
        FeeInstallmentReportDelegate FIR = new FeeInstallmentReportDelegate();
        [HttpGet]
        [Route("getdetails/{id:int}")]
        public FeeTransactionPaymentDTO Get(int id)
        {
            FeeTransactionPaymentDTO dt = new FeeTransactionPaymentDTO();

            dt.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            dt.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            dt.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));


            return FIR.getdetails(dt);

        }


        [HttpPost]
        [Route("getinstallmentid")]
        public FeeTransactionPaymentDTO getinstallmentid([FromBody]FeeTransactionPaymentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            //int asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = asmay_id;

            return FIR.getinstallmentid(data);
        }

        [Route("getinstallmentid1")]
        public FeeTransactionPaymentDTO getinstallmentid1([FromBody]FeeTransactionPaymentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            //int asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = asmay_id;

            return FIR.getinstallmentid1(data);
        }



        [Route("classes")]
        public FeeTransactionPaymentDTO classes([FromBody]FeeTransactionPaymentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            //int asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = asmay_id;
            return FIR.classes(data);
        }

        [Route("group")]
        public FeeTransactionPaymentDTO group([FromBody]FeeTransactionPaymentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            //int asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = asmay_id;
            return FIR.group(data);
        }

        [HttpPost]
        [Route("radiobtndata")]
        public FeeTransactionPaymentDTO radiobtndata([FromBody]FeeTransactionPaymentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return FIR.radiobtndata(data);
        }

    }
}
