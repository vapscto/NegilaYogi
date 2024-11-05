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
    public class MsterSessionFacadeController : Controller
    {
        public MsterSessionInterface _sessint;
        public MsterSessionFacadeController(MsterSessionInterface sess)
        {
            _sessint = sess;
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
        public MsterSessionDTO getdata(int id)
        {
            return _sessint.getdata(id);
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
        [Route("savedata")]
        public MsterSessionDTO savedata([FromBody] MsterSessionDTO data)
        {
            return _sessint.savedata(data);
        }
        [Route("edit")]
        public MsterSessionDTO edit([FromBody] MsterSessionDTO data)
        {
            return _sessint.edit(data);
        }
        [Route("activedeactive")]
        public MsterSessionDTO activedeactive([FromBody] MsterSessionDTO data)
        {
            return _sessint.activedeactive(data);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
