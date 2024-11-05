using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.admission
{
    [Route("api/[controller]")]
    public class SMSMail_HeaderController : Controller
    {
        SMSMail_HeaderDelegate del = new SMSMail_HeaderDelegate();
        [Route("getalldetails/{id:int}")]
        public SMSMail_HeaderDTO getalldetails(int id)
        {
            SMSMail_HeaderDTO data = new SMSMail_HeaderDTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getalldetails(data);
        }
        [Route("getdata")]
        public SMSMail_HeaderDTO getdata([FromBody]SMSMail_HeaderDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getdata(data);
        }
        [Route("edittab1")]
        public SMSMail_HeaderDTO edittab1([FromBody] SMSMail_HeaderDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.edittab1(data);
        }
        [Route("delete")]
        public SMSMail_HeaderDTO delete([FromBody] SMSMail_HeaderDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.delete(data);
        }
       
    }
}
