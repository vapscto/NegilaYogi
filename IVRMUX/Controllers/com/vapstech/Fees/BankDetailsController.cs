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
    public class BankDetailsController : Controller
    {

        BankDetailsDelegate del = new BankDetailsDelegate();
        [HttpPost]
        [Route("getalldetails")]
        public BankDetailsDTO getalldetails([FromBody] BankDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getalldetails(data);

        }
        [Route("getdata")]
        public BankDetailsDTO getdata([FromBody]BankDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getdata(data);
        }
        [Route("edittab1")]
        public BankDetailsDTO edittab1([FromBody] BankDetailsDTO data)




        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.edittab1(data);
        }
        [Route("deactive")]

        public BankDetailsDTO deactive([FromBody]BankDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deactive(data);
        }


       
    }
}
