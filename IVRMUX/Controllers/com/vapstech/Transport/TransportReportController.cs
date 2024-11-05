using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Transport;

using corewebapi18072016.Delegates.com.vapstech.Transport;

namespace corewebapi18072016.Controllers.com.vapstech.Transport
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class TransportReportController : Controller
    {
        TransportReportDelegate cwsd = new TransportReportDelegate();
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

        [Route("getalldetails/{id:int}")]

       


        [HttpPost]
        [Route("Getreportdetails")]
        public TransportReportDTO Getreportdetails([FromBody] TransportReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cwsd.Getreportdetails(data);
        }

         // POST api/values

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
