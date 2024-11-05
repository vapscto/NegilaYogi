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
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class TrnStudentLocationDetailsController : Controller
    {
        TrnStudentLocationDetailsDelegate rtfd = new TrnStudentLocationDetailsDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("getdata")]
        public TrnStudentLocationDetailsDTO getdata([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return rtfd.getdata(id);
        }

      


        [HttpPost]
        [Route("Getreportdetails")]
        public TrnStudentLocationDetailsDTO Getreportdetails([FromBody] TrnStudentLocationDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return rtfd.Getreportdetails(data);
        }
           [HttpPost]
        [Route("sendmsg")]
        public TrnStudentLocationDetailsDTO sendmsg([FromBody] TrnStudentLocationDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return rtfd.sendmsg(data);
        }






        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
