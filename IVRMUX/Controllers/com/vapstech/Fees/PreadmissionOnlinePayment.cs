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


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class PreadmissionOnlinePayment : Controller
    {
        PreadmissionOnlinePaymentDelegate preadonline = new PreadmissionOnlinePaymentDelegate();

        [HttpPost]
        [Route("getalldetails")]
        public FeeStudentTransactionDTO getInitialData([FromBody] FeeStudentTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.Amst_Id = id;
            data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));

            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return preadonline.getInitailData(data);
        }

        [Route("getstudentdetails")]
        public FeeStudentTransactionDTO getstudentdetails([FromBody] FeeStudentTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.Amst_Id = id;
            data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return preadonline.getstudata(data);
        }

        [Route("getamountdetails")]
        public FeeStudentTransactionDTO getamtdetils([FromBody] FeeStudentTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.Amst_Id = id;
            data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return preadonline.getamtdetils(data);
        }

        [Route("generatehashsequence")]
        public FeeStudentTransactionDTO generatehash([FromBody] FeeStudentTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.Amst_Id = id;
            data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
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
                    querystring = "http://localhost:57606/#/app/PreadmissionOnlinePayment/13?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/PreadmissionOnlinePayment/13?status=Networkfailure";
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
            try
            {
                dto = preadonline.getpaymentresponsepaytm(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/PreadmissionOnlinePayment/13?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/PreadmissionOnlinePayment/13?status=Networkfailure";
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
            string payid = response.razorpay_payment_id;
            try
            {
                dto = preadonline.razorgetpaymentresponse(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/PreadmissionOnlinePayment/13?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/PreadmissionOnlinePayment/13?status=Networkfailure";
                }
            }
            catch (Exception e)
            {
                //  dto.returnvalue = "";
            }
            return Redirect(querystring);
        }

        [Route("paymentresponseeasybuzz/")]
        public ActionResult paymentresponseeasybuzz(PaymentDetails.easybuzz response)
        {
            PaymentDetails.easybuzz dto = new PaymentDetails.easybuzz();
            string querystring = "";

            try
            {
                dto = preadonline.getpaymentresponseeasybuzz(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/PreadmissionOnlinePayment/13?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/PreadmissionOnlinePayment/13?status=Networkfailure";
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
