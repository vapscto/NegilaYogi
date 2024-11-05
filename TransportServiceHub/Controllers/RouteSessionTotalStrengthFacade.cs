using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.Transport;
using TransportServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TransportServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class RouteSessionTotalStrengthFacade : Controller
    {
        public RouteSessionTotalStrengthInterface _feegrouppagee;
        // GET: api/values
        public RouteSessionTotalStrengthFacade(RouteSessionTotalStrengthInterface maspag)
        {
            _feegrouppagee = maspag;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpPost]

        [Route("Getreportdetails")]
        public RouteSessionTotalStrengthDTO Getreportdetails([FromBody]RouteSessionTotalStrengthDTO data)
        {
            return _feegrouppagee.Getreportdetails(data);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [Route("getdata/{id:int}")]
        public RouteSessionTotalStrengthDTO getdata(int id)
        {
            return _feegrouppagee.getdata(id);
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
