using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeePaymentGatewayDetailsController : Controller
    {
        FeePaymentGatewayDetailsDelegate gate = new FeePaymentGatewayDetailsDelegate();
        // GET: api/values
        
        [Route("getPaymentGatewayDetails/{id:int}")]
        public Fee_PaymentGateway_DetailsDTO getPaymentGatewayDetails(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return gate.getInitailData(id);
        }
        // POST api/values
        [HttpPost]
        public Fee_PaymentGateway_DetailsDTO savePaymentGatewayDetails([FromBody] Fee_PaymentGateway_DetailsDTO data)
        {
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return gate.savedata(data);
        }
        [Route("editPaymentGatewayDetails/{id:int}")]
        public Fee_PaymentGateway_DetailsDTO editPaymentGatewayDetails(int id)
        {
            return gate.editdata(id);
        }
        [Route("deletePaymentGatewayDetails/{id:int}")]
        public Fee_PaymentGateway_DetailsDTO deletePaymentGatewayDetails(int id)
        {
            return gate.deletedata(id);
        }
    }
}
