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
    public class FeeInstallmentReportFacadeController
    {
        public FeeInstallmentReportInterface _feedefaulters;

        public FeeInstallmentReportFacadeController(FeeInstallmentReportInterface maspag)
        {
            _feedefaulters = maspag;
        }

      

        [Route("getdetails")]
        public FeeTransactionPaymentDTO getdetails([FromBody]FeeTransactionPaymentDTO dt)
        {
            return _feedefaulters.getdetails(dt);
        }

        [HttpPost]
        [Route("radiobtndata")]
        public Task<FeeTransactionPaymentDTO> radiobtndata([FromBody]FeeTransactionPaymentDTO data)
        {
            return _feedefaulters.radiobtndata(data);
        }

        [Route("getinstallmentid")]
        public FeeTransactionPaymentDTO getinstallmentid([FromBody]FeeTransactionPaymentDTO dto)
        {
            return _feedefaulters.getinstallmentid(dto);
        }

        [Route("getinstallmentid1")]
        public FeeTransactionPaymentDTO getinstallmentid1([FromBody]FeeTransactionPaymentDTO dto)
        {
            return _feedefaulters.getinstallmentid1(dto);
        }


        [Route("classes")]
        public FeeTransactionPaymentDTO classes([FromBody]FeeTransactionPaymentDTO dto)
        {
            return _feedefaulters.classes(dto);
        }


        [Route("group")]
        public FeeTransactionPaymentDTO group([FromBody]FeeTransactionPaymentDTO dto)
        {
            return _feedefaulters.group(dto);
        }



    }
}
