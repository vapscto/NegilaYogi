using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class ModeOfPaymentController : Controller
    {
        ModeOfPaymentDelegate del = new ModeOfPaymentDelegate();

        [Route("loaddata/{id:int}")]

        public ModeOfPaymentDTO loaddata(int id)
        {
            ModeOfPaymentDTO data = new ModeOfPaymentDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("MI_Id"));
            return del.loaddata(data);
        }
        [Route("savedata")]
        public ModeOfPaymentDTO savedata([FromBody] ModeOfPaymentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedata(data);
        }
        [Route("deletedata")]
        public ModeOfPaymentDTO deletedata([FromBody] ModeOfPaymentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deletedata(data);
        }
        [Route("paymentDecative")]
        public ModeOfPaymentDTO paymentDecative([FromBody] ModeOfPaymentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("MI_Id"));
            return del.paymentDecative(data);
        }
}
}
