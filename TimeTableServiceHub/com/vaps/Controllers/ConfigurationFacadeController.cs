using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTableServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ConfigurationFacadeController : Controller
    {
        private ConfigurationInterface inter;

        public ConfigurationFacadeController(ConfigurationInterface maspag)
        {
            inter = maspag;
        }

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

       

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("getdetails")]
        public TTConfigurationDTO getdetails([FromBody]TTConfigurationDTO id)
        {
            return inter.getdetails(id);
        }
        [HttpPost]
        [Route("savedetail")]
        public TTConfigurationDTO Post([FromBody] TTConfigurationDTO org)
        {
            return inter.savedetail(org);
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
