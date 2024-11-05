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
    public class MasterRouteFacadeController : Controller
    {
        public MasterRouteInterface _routein;
        public MasterRouteFacadeController(MasterRouteInterface route)
        {
            _routein = route;
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
        public MasterRouteDTO getdata(int id)
        {
            return _routein.getdata(id);
        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("savedata")]
        public MasterRouteDTO savedata([FromBody] MasterRouteDTO data)
        {
            return _routein.savedata(data);
        }
        [Route("edit")]
        public MasterRouteDTO edit([FromBody] MasterRouteDTO data)
        {
            return _routein.edit(data);
        }
        [Route("activedeactive")]
        public MasterRouteDTO activedeactive([FromBody] MasterRouteDTO data)
        {
            return _routein.activedeactive(data);
        }
        [Route("getstudentlistre")]
        public MasterRouteDTO getstudentlistre([FromBody] MasterRouteDTO data)
        {
            return _routein.getstudentlistre(data);
        }
        [Route("saveorder")]
        public MasterRouteDTO saveorder([FromBody] MasterRouteDTO data)
        {
            return _routein.saveorder(data);
        }
        [Route("saveroutearea")]
        public MasterRouteDTO saveroutearea([FromBody] MasterRouteDTO data)
        {
            return _routein.saveroutearea(data);
        }

        [Route("activedeactiveroutearea")]
        public MasterRouteDTO activedeactiveroutearea([FromBody] MasterRouteDTO data)
        {
            return _routein.activedeactiveroutearea(data);
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
