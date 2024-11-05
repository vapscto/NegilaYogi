using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Transport;
using corewebapi18072016.Delegates.com.vapstech.Transport;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Transport
{
    [Route("api/[controller]")]
    public class UsrPwdController : Controller
    {
        UsrPwdDelegate cwsd = new UsrPwdDelegate();

        [Route("getdata/{id:int}")]
        public SMSEmailSendDTO getdata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cwsd.getdata(id);
        }
        // [Route("getalldetails/{id:int}")]


        [HttpPost]
        [Route("Getreportdetails")]
        public SMSEmailSendDTO Getreportdetails([FromBody] SMSEmailSendDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cwsd.Getreportdetails(data);
        }
        // POST api/values



        [HttpPost]
        [Route("createusername")]
        public SMSEmailSendDTO smssend([FromBody] SMSEmailSendDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cwsd.createusername(data);
        }


        [HttpPost]
        [Route("emailsend")]
        public SMSEmailSendDTO emailsend([FromBody] SMSEmailSendDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return cwsd.emailsend(data);
        }

    }
}
