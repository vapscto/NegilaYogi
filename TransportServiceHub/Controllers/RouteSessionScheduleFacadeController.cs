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
    public class RouteSessionScheduleFacadeController : Controller
    {
        public RouteSessionScheduleInterface _interface;
        public RouteSessionScheduleFacadeController(RouteSessionScheduleInterface inte)
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
        public RouteSessionScheduleDTO getdata(int id)
        {
            return _interface.getdata(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("savedata")]
        public RouteSessionScheduleDTO savedata([FromBody] RouteSessionScheduleDTO data)
        {
            return _interface.savedata(data);
        }
        [Route("routechange")]
        public RouteSessionScheduleDTO routechange([FromBody] RouteSessionScheduleDTO data)
        {
            return _interface.routechange(data);
        }

        [Route("edit")]
        public RouteSessionScheduleDTO edit([FromBody] RouteSessionScheduleDTO data)
        {
            return _interface.edit(data);
        }
        [Route("showlocationGrid")]
        public RouteSessionScheduleDTO showlocationGrid([FromBody] RouteSessionScheduleDTO data)
        {
            return _interface.showlocationGrid(data);
        }

        [Route("activedeactive")]
        public RouteSessionScheduleDTO activedeactive([FromBody] RouteSessionScheduleDTO data)
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
