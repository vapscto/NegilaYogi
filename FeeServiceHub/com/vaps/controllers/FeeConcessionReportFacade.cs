using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.controllers
{
   
        [Route("api/[controller]")]

        public class FeeConcessionReportFacade : Controller
        {
        
            public FeeConcessionReportInterface _feedefaulters;

            public FeeConcessionReportFacade(FeeConcessionReportInterface maspag)
            {
                _feedefaulters = maspag;
            }

            [HttpGet]
            public IEnumerable<string> Get()
            {
                return new string[] { "value1", "value2" };
            }




            [Route("getdetails")]
            public FeeTransactionPaymentDTO getorgdet([FromBody]FeeTransactionPaymentDTO dt)
            {
                return _feedefaulters.getdetails(dt);
            }

        [Route("get_groups")]
        public FeeTransactionPaymentDTO get_groups([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _feedefaulters.get_groups(temp);
        }

        [HttpPost]
        [Route("radiobtndata/")]
        public Task<FeeTransactionPaymentDTO> radiobtndata([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _feedefaulters.getradiofiltereddata(temp);
        }
    }
}
