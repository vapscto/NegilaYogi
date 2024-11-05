using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;
using CollegeFeeService.com.vaps.Interfaces;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CollegeFeeOnlinePaymentFacade : Controller
    {
        public CollegeFeeOnlinePaymentInterface _MsCInter;
        public CollegeFeeOnlinePaymentFacade(CollegeFeeOnlinePaymentInterface scadm)
        {
            _MsCInter = scadm;
        }
        [Route("getloaddata")]
        public CollegeFeeTransactionDTO pageload([FromBody] CollegeFeeTransactionDTO data)
        {
            return _MsCInter.pageload(data);
        }

        [Route("getheaddetails")]
        public CollegeFeeTransactionDTO getheaddetails([FromBody] CollegeFeeTransactionDTO data)
        {
            return _MsCInter.getheaddetails(data);
        }


        [Route("generatehashsequence")]
        public CollegeFeeTransactionDTO generatehashsequence([FromBody] CollegeFeeTransactionDTO data)
        {
            return _MsCInter.generatehashsequence(data);
        }

        [Route("generatehashsequencedisplay")]
        public CollegeFeeTransactionDTO generatehashsequencedisplay([FromBody] CollegeFeeTransactionDTO data)
        {
            return _MsCInter.generatehashsequencedisplay(data);
        }

        [Route("getpaymentresponse")]
        public PaymentDetails getpaymentresponse([FromBody]PaymentDetails response)
        {
            return _MsCInter.payuresponse(response);
        }

        [Route("getpaymentresponsepaytm")]
        public PaymentDetails.PAYTM getpaymentresponsepaytm([FromBody]PaymentDetails.PAYTM response)
        {
            return _MsCInter.payuresponsepaytm(response);
        }

        [Route("getpaymentresponserazorpay")]
        public PaymentDetails getpaymentresponserazorpay([FromBody]PaymentDetails response)
        {
            return _MsCInter.getpaymentresponserazorpay(response);
        }
        [Route("PaymentEasebuzzResponse")]
        public PaymentDetails.easybuzz PaymentEasebuzzResponse([FromBody]PaymentDetails.easybuzz response)
        {
            return _MsCInter.PaymentEasebuzzResponse(response);
        }


        [Route("mobilepayuconnect")]
        public async Task<CollegeFeeTransactionDTO> mobilepayuconnect([FromBody] CollegeFeeTransactionDTO data)
        {

            return await _MsCInter.mobilepayuconnect(data);
        }
        [Route("getpaymentresponseeasybuzzmobile/")]
        public PaymentDetails.easybuzzmobile getpaymentresponseeasybuzzmobile([FromBody]PaymentDetails.easybuzzmobile response)
        {
            return _MsCInter.getpaymentresponseeasybuzzmobile(response);
        }
    }
}
