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
    public class VehicalDriverSubstituteFacadeController : Controller
    {
        public VehicalDriverSubstituteInterface _vehicaldriver;
        public VehicalDriverSubstituteFacadeController(VehicalDriverSubstituteInterface detail)
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
        public VehicalDriverSubstituteDTO getdata(int id)
        {
            return _vehicaldriver.getdata(id);
        }
       


        [Route("savedata")]
        public VehicalDriverSubstituteDTO savedata([FromBody] VehicalDriverSubstituteDTO id)
        {
            return _vehicaldriver.savedata(id);
        }
        [Route("get_driver_data")]
        public VehicalDriverSubstituteDTO get_driver_data([FromBody] VehicalDriverSubstituteDTO id)
        {
            return _vehicaldriver.get_driver_data(id);
        }

        [Route("editdata")]
        public VehicalDriverSubstituteDTO editdata([FromBody] VehicalDriverSubstituteDTO data)
        {
            return _vehicaldriver.editdata(data);
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
