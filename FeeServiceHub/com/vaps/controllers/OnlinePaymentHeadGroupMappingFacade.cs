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
    public class OnlinePaymentHeadGroupMappingFacade : Controller
    {
        public OnlinePaymentHeadGroupMappingInterface _feepaymapping;

        public OnlinePaymentHeadGroupMappingFacade(OnlinePaymentHeadGroupMappingInterface feepaymapping)
        {
            _feepaymapping = feepaymapping;
        }
        // GET: api/values
        [Route("onlineMappingDetails/{id:int}")]
        public Fee_OnlinePayment_MappingDTO getPaymentGatewayDetails(int id)
        {
            return _feepaymapping.getDetails(id);
        }
        // POST api/values
        [HttpPost]
        public Fee_OnlinePayment_MappingDTO savePaymentGatewayDetails([FromBody]Fee_OnlinePayment_MappingDTO data)
        {
            return _feepaymapping.saveDetails(data);
        }

        [Route("edit/{id:int}")]
        public Fee_OnlinePayment_MappingDTO editPaymentGatewayDetails(int id)
        {
            return _feepaymapping.editDetails(id);
        }

        [Route("delete/{id:int}")]
        public Fee_OnlinePayment_MappingDTO deletePaymentGatewayDetails(int id)
        {
            return _feepaymapping.deleteDetails(id);
        }

        [Route("groupsel")]
        public Fee_OnlinePayment_MappingDTO groupselection([FromBody] Fee_OnlinePayment_MappingDTO data)
        {
            return _feepaymapping.selecgroup(data);
        }

        [Route("headsel")]
        public Fee_OnlinePayment_MappingDTO headselection([FromBody] Fee_OnlinePayment_MappingDTO data)
        {
            return _feepaymapping.selechead(data);
        }

        [Route("academicsel")]
        public Fee_OnlinePayment_MappingDTO acde([FromBody] Fee_OnlinePayment_MappingDTO data)
        {
            return _feepaymapping.acde(data);
        }

    }
}
