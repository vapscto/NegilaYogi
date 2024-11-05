using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Principal.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Principal;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Principal.Controllers
{
    [Route("api/[controller]")]
    public class TimeTableFacade : Controller
    {
        public TimeTableInterface _TTl;
        public TimeTableFacade(TimeTableInterface TTl)
        {
            _TTl = TTl;
        }

        [Route("getdata")]
        public TimeTableDTO getdata([FromBody] TimeTableDTO obj)
        {
            return _TTl.getdata(obj);
        }

        [Route("getdaily_data")]
        public TimeTableDTO getdaily_data([FromBody] TimeTableDTO data)
        {
            return _TTl.getdaily_data(data);
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
