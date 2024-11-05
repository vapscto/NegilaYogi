using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeOnlinePaymentStthomasFacade : Controller
    {
        public FeeOnlinePaymentStthomasInterface _feegrouppage;

        public FeeOnlinePaymentStthomasFacade(FeeOnlinePaymentStthomasInterface maspag)
        {
            _feegrouppage = maspag;
        }

        [HttpPost]
        [Route("getpagedetails")]
        public FeeStudentTransactionDTO getpagedetails([FromBody] FeeStudentTransactionDTO data)
        {
            // id = 12;
            return _feegrouppage.getdetails(data);
        }

        [Route("getamountdetails")]
        public FeeStudentTransactionDTO getamountdetailss([FromBody] FeeStudentTransactionDTO data)
        {
            return _feegrouppage.getamountdet(data);
        }


        [Route("getpaymentresponse/")]
        public PaymentDetails getpaymentresponse([FromBody]PaymentDetails response)
        {

            return _feegrouppage.payuresponse(response);
        }

        [Route("getgrouportermdetails")]
        public FeeStudentTransactionDTO getgrouportermdet([FromBody] FeeStudentTransactionDTO data)
        {
            return _feegrouppage.getgrouportermdeta(data);
        }


        [Route("generatehashsequence")]
        public FeeStudentTransactionDTO generseq([FromBody] FeeStudentTransactionDTO data)
        {
            return _feegrouppage.generatehashsequence(data);
        }

        [Route("getcustomgroups")]
        public FeeStudentTransactionDTO getcusgrps([FromBody] FeeStudentTransactionDTO data)
        {
            return _feegrouppage.getcusgrp(data);
        }

        [Route("billdeskPayment")]
        public PaymentDetails.BillDeskPayment getpaymentresponsebilldesk([FromBody]PaymentDetails.BillDeskPayment response)
        {

            return _feegrouppage.billdeskPayment(response);
        }

        [Route("OnlineTransactionupdate")]
        public void OnlineTransactionupdate([FromBody] FeeStudentTransactionDTO data)
        {

            _feegrouppage.OnlineTransactionupdate(data);
        }

        //[Route("mobilepayuconnect")]
        //public FeeStudentTransactionDTO mobilepayuconnect([FromBody] FeeStudentTransactionDTO data)
        //{

        //    return _feegrouppage.mobilepayuconnect(data);
        //}

        [Route("mobilepayuconnect")]
        public async Task<FeeStudentTransactionDTO> mobilepayuconnect([FromBody] FeeStudentTransactionDTO data)
        {

            return await _feegrouppage.mobilepayuconnect(data);
        }

        [Route("getpaymentresponsePAYTM/")]
        public PaymentDetails.PAYTM getpaymentresponsePAYTM([FromBody]PaymentDetails.PAYTM response)
        {

            return _feegrouppage.paytmresponse(response);
        }

        [Route("Razorpaypaymentresponse/")]
        public PaymentDetails getpaymentresponserazor([FromBody]PaymentDetails response)
        {

            return _feegrouppage.razorresponse(response);
        }

        //RAZORPAY SCHEDULAR API
        [Route("Razorpaypaymentsettlementresponse/")]
        public FeeStudentTransactionDTO Razorpaypaymentsettlementresponse([FromBody]FeeStudentTransactionDTO response)
        {
            return _feegrouppage.Razorpaypaymentsettlementresponse(response);
        }

        //PAYTM SCHEDULAR API
        [Route("transactionstatuspaytm/")]
        public PaymentDetails transactionstatuspaytm([FromBody]PaymentDetails response)
        {
            return _feegrouppage.transactionstatuspaytm(response);
        }

        //RazorPay Transaction Logs
        [Route("RazorpayTransactionLogs/")]
        public FeeStudentTransactionDTO RazorpayTransactionLogs([FromBody]FeeStudentTransactionDTO response)
        {
            return _feegrouppage.RazorpayTransactionLogs(response);
        }

        [Route("getpaymentresponseeasybuzz/")]
        public PaymentDetails.easybuzz getpaymentresponseeasybuzz([FromBody]PaymentDetails.easybuzz response)
        {
            return _feegrouppage.getpaymentresponseeasybuzz(response);
        }


        //Easebuzz SCHEDULAR API
        [Route("Easebuzzsettlementresponse/")]
        public FeeStudentTransactionDTO Easebuzzsettlementresponse([FromBody]FeeStudentTransactionDTO response)
        {
            return _feegrouppage.Easebuzzsettlementresponse(response);
        }
        [Route("RazorpayApi/")]
        public FeeStudentTransactionDTO RazorpayApi([FromBody]FeeStudentTransactionDTO response)
        {
            return _feegrouppage.RazorpayApi(response);
        }


        [Route("Easebuzzpaymentsplitresponse/")]
        public FeeStudentTransactionDTO Easebuzzpaymentsplitresponse([FromBody]FeeStudentTransactionDTO response)
        {
            return _feegrouppage.Easebuzzpaymentsplitresponse(response);
        }
        [Route("getFeetotalamount")]
        public FeeStudentTransactionDTO getFeetotalamount([FromBody] FeeStudentTransactionDTO data)
        {
            return _feegrouppage.getFeetotalamount(data);
        }

        [Route("getpaymentresponseeasybuzzmobile/")]
        public PaymentDetails.easybuzzmobile getpaymentresponseeasybuzzmobile([FromBody]PaymentDetails.easybuzzmobile response)
        {
            return _feegrouppage.getpaymentresponseeasybuzzmobile(response);
        }
    }
}
