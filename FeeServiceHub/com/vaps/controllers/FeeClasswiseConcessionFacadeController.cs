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
    public class FeeClasswiseConcessionFacadeController 
    {
        public FeeClasswiseConcessionReportInterface _feedefaulters;

        public FeeClasswiseConcessionFacadeController(FeeClasswiseConcessionReportInterface maspag)
        {
            _feedefaulters = maspag;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getdetails/{id:int}")]
        public FeeTransactionPaymentDTO getdetails(int id)
        {
            return _feedefaulters.getdetails(id);
        }

        [HttpPost]
        [Route("radiobtndata")]
        public Task<FeeTransactionPaymentDTO> radiobtndata([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _feedefaulters.concessiondtl(temp);
        }


    }
}
