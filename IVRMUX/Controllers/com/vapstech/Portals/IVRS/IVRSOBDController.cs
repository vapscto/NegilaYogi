using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.IVRS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.IVRS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.IVRS
{
    [Route("api/[controller]")]
    public class IVRSOBDController : Controller
    {
        IVRSOBDDelegate objdl = new IVRSOBDDelegate();
           // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("getalldetails")]
        public IVRSOBD Get([FromQuery] int id)
        {
            IVRSOBD data = new IVRSOBD();
            data.MI_ID= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdl.getdetails(data);
        }
        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("ivrgetstudetails")]       
        public IVRSOBD ivrgetstudetails([FromBody]IVRSOBD value)
        {
            return objdl.ivrgetstudetails(value);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpPost]
        [Route("initiatecalls")]
        public IVRSOBD initiatecalls([FromBody]IVRSOBD value)
        {
            return objdl.initiatecalls(value);
        }
        [HttpPost]
        [Route("initiatecallsmobiglitz")]
        public IVRSOBD initiatecallsmobiglitz([FromBody]IVRSOBD value)
        {
            return objdl.initiatecallsmobiglitz(value);
        }

    }
}
