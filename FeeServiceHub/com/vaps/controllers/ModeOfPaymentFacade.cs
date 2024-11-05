using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class ModeOfPaymentFacade : Controller
    {
        public ModeOfPaymentInterface inter;
        public ModeOfPaymentFacade(ModeOfPaymentInterface s)
        {
            inter = s;
        }


        [Route("loaddata")]
        public ModeOfPaymentDTO loaddata([FromBody] ModeOfPaymentDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("savedata")]
        public ModeOfPaymentDTO savedata([FromBody] ModeOfPaymentDTO data)
        {
            return inter.savedata(data);
        }
        [Route("deletedata")]
        public ModeOfPaymentDTO deletedata([FromBody] ModeOfPaymentDTO data)
        {
            return inter.deletedata(data);
        }
        [Route("paymentDecative")]
        public ModeOfPaymentDTO paymentDecative([FromBody] ModeOfPaymentDTO data)
        {
            return inter.paymentDecative(data);
        }
    }
}
