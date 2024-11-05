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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeOnlinePaymentVikasaFacade : Controller
    {
        public FeeOnlinePaymentVikasaInterface _feegrouppage;

        public FeeOnlinePaymentVikasaFacade(FeeOnlinePaymentVikasaInterface maspag)
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

       
    }
}
