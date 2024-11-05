using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class EBSonlinepaymentFacade : Controller
    {
        public EBSonlinepaymentInterface _feegrouppage;

        public EBSonlinepaymentFacade(EBSonlinepaymentInterface maspag)
        {
            _feegrouppage = maspag;
        }    

        // GET api/values/5
        [Route("getdetails")]
        public FeeStudentTransactionDTO getorgdet([FromBody] FeeStudentTransactionDTO id)
        {
            return _feegrouppage.getdetails(id);
        }

        [Route("getpaymentresponse/")]
        public PaymentDetails getpaymentresponse([FromBody]PaymentDetails response)
        {

            return _feegrouppage.payuresponse(response);
        }
    }
}
