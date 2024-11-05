using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Transport;
using PreadmissionDTOs.com.vaps.Transport;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Transport
{
    [Route("api/[controller]")]
    public class RouteSessionScheduleController : Controller
    {
        RouteSessionScheduleDelegate _routeschedule = new RouteSessionScheduleDelegate();
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
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           return _routeschedule.getdata(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("savedata")]
        public RouteSessionScheduleDTO savedata([FromBody] RouteSessionScheduleDTO data)
        {
            data.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _routeschedule.savedata(data);
        }
        [Route("routechange")]
        public RouteSessionScheduleDTO routechange([FromBody] RouteSessionScheduleDTO data)
        {
            data.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _routeschedule.routechange(data);
        }

        [Route("edit")]
        public RouteSessionScheduleDTO edit([FromBody] RouteSessionScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _routeschedule.edit(data);
        }
        [Route("showlocationGrid")]
        public RouteSessionScheduleDTO showlocationGrid([FromBody] RouteSessionScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _routeschedule.showlocationGrid(data);
        }
        [Route("activedeactive")]
        public RouteSessionScheduleDTO activedeactive([FromBody] RouteSessionScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _routeschedule.activedeactive(data);
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
