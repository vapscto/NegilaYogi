using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeFeeService.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CollegePreAdmOnlinePaymentFacade : Controller
    {
        public CollegePreAdmOnlinePaymentInterface _feegrouppage;

        public CollegePreAdmOnlinePaymentFacade(CollegePreAdmOnlinePaymentInterface maspag)
        {
            _feegrouppage = maspag;
        }

        [HttpPost]
        [Route("gettermdetails")]
        public CollegeFeeTransactionDTO getpagedetails([FromBody] CollegeFeeTransactionDTO data)
        {
            return _feegrouppage.getstudentdetails(data);
        }

        [Route("getstudentdetails")]
        public CollegeFeeTransactionDTO getstuddata([FromBody] CollegeFeeTransactionDTO data)
        {
            return _feegrouppage.getdetails(data);
        }

        [Route("generatehashsequence")]
        public CollegeFeeTransactionDTO generatehash([FromBody] CollegeFeeTransactionDTO data)
        {
            return _feegrouppage.hashgeneration(data);
        }

        [Route("getpaymentresponse/")]
        public PaymentDetails getpaymentresponse([FromBody]PaymentDetails response)
        {
            return _feegrouppage.payuresponse(response);
        }

        [Route("getamountdetails")]
        public CollegeFeeTransactionDTO getamountdetails([FromBody] CollegeFeeTransactionDTO data)
        {
            return _feegrouppage.getamountdetails(data);
        }

        [Route("getpaymentresponsepaytm/")]
        public PaymentDetails.PAYTM getpaymentresponsepaytm([FromBody]PaymentDetails.PAYTM response)
        {
            return _feegrouppage.payuresponsepaytm(response);
        }

        [Route("razorgetpaymentresponse/")]
        public PaymentDetails razorgetpaymentresponse([FromBody]PaymentDetails response)
        {
            return _feegrouppage.razorgetpaymentresponse(response);
        }

        [Route("Razorpaypaymentsettlementresponse/")]
        public CollegeFeeTransactionDTO Razorpaypaymentsettlementresponse([FromBody]CollegeFeeTransactionDTO response)
        {
            return _feegrouppage.Razorpaypaymentsettlementresponse(response);
        }

        [Route("paytmresponsse/")]
        public PaymentDetails paytmresponsse([FromBody]PaymentDetails response)
        {
            return _feegrouppage.paytmresponsse(response);
        }
    }
}
