using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.IVRS.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.IVRS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.IVRS.Controllers
{
    [Route("api/[controller]")]
    public class IVRSOBDFacadeController : Controller
    {
        public IVRSOBDInterface _intf;
        public IVRSOBDFacadeController(IVRSOBDInterface intf)
        {
            _intf = intf;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpPost]
        [Route("getdetails")]
        public IVRSOBD getdetails([FromBody] IVRSOBD data)
        {
            return _intf.getdetails(data);
        }
        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("ivrgetstudetails")]
        public IVRSOBD ivrgetstudetails([FromBody] IVRSOBD data)
        {
            return _intf.ivrgetstudetails(data);
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
        public IVRSOBD initiatecalls([FromBody] IVRSOBD data)
        {
            return _intf.initiatecalls(data);
        }
          [HttpPost]
        [Route("initiatecallsmobiglitz")]
        public IVRSOBD initiatecallsmobiglitz([FromBody] IVRSOBD data)
        {
            return _intf.initiatecallsmobiglitz(data);
        }

    }
}
