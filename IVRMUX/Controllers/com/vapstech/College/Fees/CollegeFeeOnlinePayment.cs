using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IVRMUX.Delegates.com.vapstech.College.Fees;
using PreadmissionDTOs.com.vaps.College.Fees;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    public class CollegeFeeOnlinePayment : Controller
    {
        CollegeFeeOnlinePaymentDelegate CFOPD = new CollegeFeeOnlinePaymentDelegate();

        [Route("getloaddata")]
        public CollegeFeeTransactionDTO pageload([FromBody] CollegeFeeTransactionDTO data)
        {
            if (data.ASMAY_Id == 0 && data.AMCST_Id==0)
            {
                data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            }
                
           // data.AMCST_Id = 330;

            return CFOPD.pagelod(data);
        }
        [Route("getheaddetails")]
        public CollegeFeeTransactionDTO getheaddetails([FromBody] CollegeFeeTransactionDTO data)
        {
            if (data.ASMAY_Id == 0)
            {
                data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

                data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

                data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            }
           
            //data.AMCST_Id = 330;

            //data.UserId = 330;

            return CFOPD.getheaddetails(data);
        }

        [Route("generatehashsequence")]
        public CollegeFeeTransactionDTO generatehashsequence([FromBody] CollegeFeeTransactionDTO data)
        {
            if (data.ASMAY_Id == 0)
            {
                data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

                data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

                data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            }
                
            //data.AMCST_Id = 330;

            //data.UserId = 330;

            return CFOPD.generatehashsequence(data);
        }
        [Route("generatehashsequencedisplay")]
        public CollegeFeeTransactionDTO generatehashsequencedisplay([FromBody] CollegeFeeTransactionDTO data)
        {
            if (data.ASMAY_Id == 0)
            {
                data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

                data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

                data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            }

            //data.AMCST_Id = 330;

            //data.UserId = 330;

            return CFOPD.generatehashsequencedisplay(data);
        }

        

        [Route("paymentresponse/")]
        public ActionResult paymentresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            string querystring = "";
            try
            {
                dto = CFOPD.getpaymentresponse(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/CollegeFeeOnlinePayment/13?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/CollegeFeeOnlinePayment/13?status=Networkfailure";
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
                dto = CFOPD.PaymentEasebuzzResponse(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/CollegeFeeOnlinePayment/13?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/CollegeFeeOnlinePayment/13?status=Networkfailure";
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
                dto = CFOPD.getpaymentresponse(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/CollegeFeeOnlinePayment/13?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/CollegeFeeOnlinePayment/13?status=Networkfailure";
                }
            }
            catch (Exception e)
            {
                //  dto.returnvalue = "";
            }
            return Redirect(querystring);
        }

        [Route("paymentresponseRAZORPAY/")]
        public ActionResult paymentresponserazorpay(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            string querystring = "";
            response.IVRMOP_MIID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string payid = response.razorpay_payment_id;
            try
            {
                dto = CFOPD.getpaymentresponserazorpay(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/CollegeFeeOnlinePayment/13?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/CollegeFeeOnlinePayment/13?status=Networkfailure";
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
