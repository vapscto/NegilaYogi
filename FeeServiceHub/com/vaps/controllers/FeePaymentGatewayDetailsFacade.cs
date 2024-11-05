using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeePaymentGatewayDetailsFacade : Controller
    {
        public FeePaymentGatewayDetailsInterface _feepaygate;

        public FeePaymentGatewayDetailsFacade(FeePaymentGatewayDetailsInterface feepaygate)
        {
            _feepaygate = feepaygate;
        }
        // GET: api/values
        [Route("getinitialdata/{id:int}")]
        public Fee_PaymentGateway_DetailsDTO getPaymentGatewayDetails(int id)
        {
            return _feepaygate.getPaymentGatewayDetails(id);
        }
        // POST api/values
        [HttpPost]
        public Fee_PaymentGateway_DetailsDTO savePaymentGatewayDetails([FromBody]Fee_PaymentGateway_DetailsDTO data)
        {
            return _feepaygate.savePaymentGatewayDetails(data);
        }

        [Route("editPaymentGatewayDetails/{id:int}")]
        public Fee_PaymentGateway_DetailsDTO editPaymentGatewayDetails(int id)
        {
            return _feepaygate.editPaymentGatewayDetails(id);
        }

        [Route("deletePaymentGatewayDetails/{id:int}")]
        public Fee_PaymentGateway_DetailsDTO deletePaymentGatewayDetails(int id)
        {
            return _feepaygate.deletePaymentGatewayDetails(id);
        }
    }
}
