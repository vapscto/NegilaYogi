using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using FeeServiceHub.com.vaps.interfaces;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class PreadmissionOnlinePaymentFacade : Controller
    {
        public PreadmissionOnlinePaymentInterface _feegrouppage;

        public PreadmissionOnlinePaymentFacade(PreadmissionOnlinePaymentInterface maspag)
        {
            _feegrouppage = maspag;
        }

        [HttpPost]
        [Route("gettermdetails")]
        public FeeStudentTransactionDTO getpagedetails([FromBody] FeeStudentTransactionDTO data)
        {
            return _feegrouppage.getstudentdetails(data); 
        }

        [Route("getstudentdetails")]
        public FeeStudentTransactionDTO getstuddata([FromBody] FeeStudentTransactionDTO data)
        {
            return _feegrouppage.getdetails(data);
        }

        [Route("generatehashsequence")]
        public FeeStudentTransactionDTO generatehash([FromBody] FeeStudentTransactionDTO data)
        {
            return _feegrouppage.hashgeneration(data);
        }

        [Route("getpaymentresponse/")]
        public PaymentDetails getpaymentresponse([FromBody]PaymentDetails response)
        {
            return _feegrouppage.payuresponse(response);
        }

        [Route("getamountdetails")]
        public FeeStudentTransactionDTO getamountdetails([FromBody] FeeStudentTransactionDTO data)
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
        public FeeStudentTransactionDTO Razorpaypaymentsettlementresponse([FromBody]FeeStudentTransactionDTO response)
        {
            return _feegrouppage.Razorpaypaymentsettlementresponse(response);
        }

        [Route("paytmresponsse/")]
        public PaymentDetails paytmresponsse([FromBody]PaymentDetails response)
        {
            return _feegrouppage.paytmresponsse(response);
        }


        [Route("getpaymentresponseeasybuzz/")]
        public PaymentDetails.easybuzz getpaymentresponseeasybuzz([FromBody]PaymentDetails.easybuzz response)
        {
            return _feegrouppage.getpaymentresponseeasybuzz(response);
        }
    }
}


