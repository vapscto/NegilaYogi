using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TransportServiceHub.Interfaces;
using PreadmissionDTOs.com.vaps.Transport;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TransportServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class MasterRouteScheduleFacadeController : Controller
    {
        public MasterRouteScheduleInterface _interface;
        public MasterRouteScheduleFacadeController(MasterRouteScheduleInterface inte)
        {
            _interface = inte;
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

        [Route("getdata/{id:int}")]
        public MasterRouteScheduleDTO getdata(int id)
        {
            return _interface.getdata(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("savedata")]
        public MasterRouteScheduleDTO savedata([FromBody] MasterRouteScheduleDTO data)
        {
            return _interface.savedata(data);
        }

        [Route("edit")]
        public MasterRouteScheduleDTO edit([FromBody] MasterRouteScheduleDTO data)
        {
            return _interface.edit(data);
        }

        [Route("activedeactive")]
        public MasterRouteScheduleDTO activedeactive([FromBody] MasterRouteScheduleDTO data)
        {
            return _interface.activedeactive(data);
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
