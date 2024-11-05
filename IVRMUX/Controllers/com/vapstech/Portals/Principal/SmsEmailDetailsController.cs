using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using corewebapi18072016.Delegates.com.vapstech.Portals.Principal;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Principal
{
    [Route("api/[controller]")]
    public class SmsEmailDetailsController : Controller
    {
        SmsEmailDetailsDelegate objdelegate1 = new SmsEmailDetailsDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    
        [Route("getdata")]
        public SmsEmailDetailsDTO getdata(SmsEmailDetailsDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));          
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return objdelegate1.getdata(data);
        }

        [Route("Getreportdetails")]
        public SmsEmailDetailsDTO Getreportdetails([FromBody] SmsEmailDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return objdelegate1.Getreportdetails(data);
        }
        [Route("Getreportdetails1")]
        public SmsEmailDetailsDTO Getreportdetails1([FromBody] SmsEmailDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return objdelegate1.Getreportdetails1(data);
        }
    }
}
