using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Transport;
using PreadmissionDTOs.com.vaps.Transport;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Transport
{
    [Route("api/[controller]")]
    public class MasterRouteController : Controller
    {
        MasterRouteDelegate _route = new MasterRouteDelegate();
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
        public MasterRouteDTO getdata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _route.getdata(id);
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
       
        [Route("savedata")]
        public MasterRouteDTO savedata([FromBody]MasterRouteDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _route.savedata(data);
        }

        [Route("edit")]
        public MasterRouteDTO edit([FromBody] MasterRouteDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _route.edit(data);
        }
        [Route("activedeactive")]
        public MasterRouteDTO activedeactive([FromBody] MasterRouteDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _route.activedeactive(data);
        }
        [Route("getstudentlistre")]
        public MasterRouteDTO getstudentlistre([FromBody] MasterRouteDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _route.getstudentlistre(data);
        }
        [Route("saveorder")]
        public MasterRouteDTO saveorder([FromBody] MasterRouteDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _route.saveorder(data);
        }
        [Route("saveroutearea")]
        public MasterRouteDTO saveroutearea([FromBody]MasterRouteDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _route.saveroutearea(data);
        }
        [Route("activedeactiveroutearea")]
        public MasterRouteDTO activedeactiveroutearea([FromBody] MasterRouteDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _route.activedeactiveroutearea(data);
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
