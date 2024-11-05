using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    public class CollegePreAdmOnlinePaymentController : Controller
    {
        CollegePreAdmOnlinePaymentDelegate preadonline = new CollegePreAdmOnlinePaymentDelegate();

        [HttpPost]
        [Route("getalldetails")]
        public CollegeFeeTransactionDTO getInitialData([FromBody] CollegeFeeTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.Amst_Id = id;
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return preadonline.getInitailData(data);
        }

        [Route("getstudentdetails")]
        public CollegeFeeTransactionDTO getstudentdetails([FromBody] CollegeFeeTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.Amst_Id = id;
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return preadonline.getstudata(data);
        }

        [Route("getamountdetails")]
        public CollegeFeeTransactionDTO getamtdetils([FromBody] CollegeFeeTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.Amst_Id = id;
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return preadonline.getamtdetils(data);
        }

        [Route("generatehashsequence")]
        public CollegeFeeTransactionDTO generatehash([FromBody] CollegeFeeTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.Amst_Id = id;
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return preadonline.generatehash(data);
        }

        [Route("paymentresponse/")]
        public ActionResult paymentresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            string querystring = "";
            try
            {
                dto = preadonline.getpaymentresponse(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/CollegePreAdmOnlinePayment/13?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/CollegePreAdmOnlinePayment/13?status=Networkfailure";
                }
            }
            catch (Exception e)
            {
                //  dto.returnvalue = "";
            }
            return Redirect(querystring);
        }

        [Route("paymentresponsePAYTM/")]
        public ActionResult paymentresponse(PaymentDetails.PAYTM response)
        {
            PaymentDetails.PAYTM dto = new PaymentDetails.PAYTM();
            string querystring = "";
            response.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            try
            {
                dto = preadonline.getpaymentresponsepaytm(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/CollegePreAdmOnlinePayment/13?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/CollegePreAdmOnlinePayment/13?status=Networkfailure";
                }
            }
            catch (Exception e)
            {
                //  dto.returnvalue = "";
            }
            return Redirect(querystring);
        }

        [Route("Razorpaypaymentresponse/")]
        public ActionResult razorpaymentresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            string querystring = "";
            response.IVRMOP_MIID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            response.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            string payid = response.razorpay_payment_id;
            try
            {
                dto = preadonline.razorgetpaymentresponse(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/CollegePreAdmOnlinePayment/13?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/CollegePreAdmOnlinePayment/13?status=Networkfailure";
                }
            }
            catch (Exception e)
            {
                //  dto.returnvalue = "";
            }
            return Redirect(querystring);
        }
    }
}
