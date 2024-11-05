using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeSummarizedReportFacade : Controller
    {
        public FeeSummarizedReportInterface _fees;

        public FeeSummarizedReportFacade(FeeSummarizedReportInterface maspag)
        {
            _fees = maspag;
        }


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        
        [Route("getdetails")]
        public FeeTransactionPaymentDTO getorgdet([FromBody] FeeTransactionPaymentDTO data)
        {
            return _fees.getdetails(data);
        }


        [Route("getsection")]
        public FeeTransactionPaymentDTO getsection([FromBody]FeeTransactionPaymentDTO data)
        {
            return _fees.getsection(data);
        }

        [Route("getstudent")]
        public FeeTransactionPaymentDTO getstudent([FromBody]FeeTransactionPaymentDTO data)
        {
            return _fees.getstudent(data);
        }

        [HttpPost]
        [Route("radiobtndata")]
        public Task<FeeTransactionPaymentDTO> radiobtndata([FromBody]FeeTransactionPaymentDTO data)
        {
            return _fees.getradiofiltereddata(data);
        }


        [Route("get_groups")]
        public FeeTransactionPaymentDTO get_groups([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _fees.get_groups(temp);
        }


    }
}
