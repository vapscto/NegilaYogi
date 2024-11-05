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
    public class MasterLocationFacadeController : Controller
    {
        public MasterLocationInterface _loction;

        public MasterLocationFacadeController(MasterLocationInterface areaz)
        {
            _loction = areaz;
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
        public MasterLocationDTO getdata(int id)
        {
            return _loction.getdata(id);
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("savedata")]
        public MasterLocationDTO savedata([FromBody] MasterLocationDTO data)
        {
            return _loction.savedata(data);
        }

        [Route("activedeactive")]
        public MasterLocationDTO activedeactive([FromBody] MasterLocationDTO data)
        {          
            return _loction.activedeactive(data);
        }
        [Route("edit")]
        public MasterLocationDTO edit([FromBody] MasterLocationDTO data)
        {           
            return _loction.edit(data);
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
