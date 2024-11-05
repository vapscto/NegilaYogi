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
    public class MasterDriverFacadeController : Controller
    {
        public MasterDriverInterface _driveinf;
        public MasterDriverFacadeController(MasterDriverInterface areaz)
        {
            _driveinf = areaz;
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
        public MasterDriverDTO getdata(int id)
        {
            return _driveinf.getdata(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("checkdrivercode")]
        public MasterDriverDTO checkdrivercode([FromBody] MasterDriverDTO data)
        {
            return _driveinf.checkdrivercode(data);
        }
        [Route("checkdriverdl")]
        public MasterDriverDTO checkdriverdl([FromBody] MasterDriverDTO data)
        {
            return _driveinf.checkdriverdl(data);
        }
        [Route("checkdriverbno")]
        public MasterDriverDTO checkdriverbno([FromBody] MasterDriverDTO data)
        {
            return _driveinf.checkdriverbno(data);
        }
        
        [Route("savedata")]
        public MasterDriverDTO savedata([FromBody] MasterDriverDTO data)
        {
            return _driveinf.savedata(data);
        }
        [Route("editdata")]
        public MasterDriverDTO editdata([FromBody] MasterDriverDTO data)
        {
            return _driveinf.editdata(data);
        }
        [Route("activedeactive")]
        public MasterDriverDTO activedeactive([FromBody] MasterDriverDTO data)
        {
            return _driveinf.activedeactive(data);
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
