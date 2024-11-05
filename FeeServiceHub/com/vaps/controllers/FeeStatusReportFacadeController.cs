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
    public class FeeStatusReportFacadeController
    {
        public FeeStatusReportInterface _feedefaulters;

        public FeeStatusReportFacadeController(FeeStatusReportInterface maspag)
        {
            _feedefaulters = maspag;
        }

       
      
        [Route("getdetails")]
        public FeeTransactionPaymentDTO getdetails([FromBody] FeeTransactionPaymentDTO dto)
        {
            return _feedefaulters.getdetails(dto);
        }

        [HttpPost]
        [Route("radiobtndata")]
        public Task<FeeTransactionPaymentDTO> radiobtndata([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _feedefaulters.radiobtndata(temp);
        }

        
        [HttpPost]
        [Route("get_fee_and_stu")]
        public FeeTransactionPaymentDTO get_fee_and_stu([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _feedefaulters.get_fee_and_stu(temp);
        }
         
        [HttpPost]
        [Route("getfeehead_statement_report")]
        public FeeTransactionPaymentDTO getfeehead_statement_report([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _feedefaulters.getfeehead_statement_report(temp);
        }

          
        [HttpPost]
        [Route("getsection")]
        public FeeTransactionPaymentDTO getsection([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _feedefaulters.getsection(temp);
        }
         [Route("GetSettlementReport")]
        public FeeTransactionPaymentDTO GetSettlementReport([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _feedefaulters.GetSettlementReport(temp);
        }



    }
}
