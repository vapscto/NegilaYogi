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
    public class FeeDetailsReportFacade : Controller
    {
        public FeeDetailsReportInterface _feedefaulters;

        public FeeDetailsReportFacade(FeeDetailsReportInterface maspag)
        {
            _feedefaulters = maspag;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        
        [Route("getdetails")]
        public FeeTransactionPaymentDTO getdetails([FromBody]FeeTransactionPaymentDTO data)
        {
            return _feedefaulters.getdetails(data);
        }


        [Route("getheadisbygrpid")]
        public FeeTransactionPaymentDTO getheadisbygrpid([FromBody]FeeTransactionPaymentDTO dto)
        {
            return _feedefaulters.getheadisbygrpid(dto);
        }

        [Route("getsection")]
        public FeeTransactionPaymentDTO getsection([FromBody]FeeTransactionPaymentDTO dto)
        {
            return _feedefaulters.getsection(dto);
        }

        [HttpPost]
        [Route("radiobtndata")]
        public Task<FeeTransactionPaymentDTO> radiobtndata([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _feedefaulters.radiobtndata(temp);
        }
    }
}
