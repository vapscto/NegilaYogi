using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TransportServiceHub.Interfaces;
using PreadmissionDTOs.com.vaps.Transport;

namespace TransportServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class MasterFuelFacadeController : Controller
    {

        public MasterFuelInterface _fuelinterface;



        public MasterFuelFacadeController(MasterFuelInterface areaz)
        {
            _fuelinterface = areaz;
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
        public MasterFuelDTO getdata(int id)
        {

            return _fuelinterface.getdata(id);
        }

        [Route("savedata")]
        public MasterFuelDTO savedata([FromBody]MasterFuelDTO data)
        {
            return _fuelinterface.savedata(data);
        }
        [Route("geteditdata")]
        public MasterFuelDTO geteditdata([FromBody] MasterFuelDTO data)
        {
            return _fuelinterface.geteditdata(data);
        }
        [Route("activedeactive")]
        public MasterFuelDTO activedeactive([FromBody] MasterFuelDTO data)
        {
            return _fuelinterface.activedeactive(data);
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

  