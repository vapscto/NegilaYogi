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
    public class VehicalRouteMappingFacadeController : Controller
    {
        public VehicalRouteMappingInterface _vehicaldriver;
        public VehicalRouteMappingFacadeController(VehicalRouteMappingInterface detail)
        {
            _vehicaldriver = detail;
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
        public VehicalRouteMappingDTO getdata(int id)
        {
            return _vehicaldriver.getdata(id);
        }


         [Route("savedata")]
        public VehicalRouteMappingDTO savedata([FromBody] VehicalRouteMappingDTO data)
        {
            return _vehicaldriver.savedata(data);
        }


        [Route("editdata")]
        public VehicalRouteMappingDTO editdata([FromBody] VehicalRouteMappingDTO data)
        {
            return _vehicaldriver.editdata(data);
        }


       



        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("activedeactive")]
        public VehicalRouteMappingDTO activedeactive([FromBody] VehicalRouteMappingDTO data)
        {
            return _vehicaldriver.activedeactive(data);
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
