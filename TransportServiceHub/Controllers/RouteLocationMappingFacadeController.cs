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
    public class RouteLocationMappingFacadeController : Controller
    {
        public RouteLocationMappingInterface _rtlo;
        public RouteLocationMappingFacadeController(RouteLocationMappingInterface rtlo)
        {
            _rtlo = rtlo;
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
        public RouteLocationMappingDTO getdata(int id)
        {
            return _rtlo.getdata(id);
        }
        [Route("savedata")]
        public RouteLocationMappingDTO savedata([FromBody] RouteLocationMappingDTO data)
        {
            return _rtlo.savedata(data);
        }
        [Route("getlocations")]
        public RouteLocationMappingDTO getlocations([FromBody] RouteLocationMappingDTO data)
        {
            return _rtlo.getlocations(data);
        }
        [Route("getlocationsarea")]
        public RouteLocationMappingDTO getlocationsarea([FromBody] RouteLocationMappingDTO data)
        {
            return _rtlo.getlocationsarea(data);
        }
        
        [Route("activedeactive")]
        public RouteLocationMappingDTO activedeactive([FromBody] RouteLocationMappingDTO data)
        {
            return _rtlo.activedeactive(data);
        }
        [Route("getOrder")]
        public RouteLocationMappingDTO getOrder([FromBody] RouteLocationMappingDTO data)
        {
            return _rtlo.getOrder(data);
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
