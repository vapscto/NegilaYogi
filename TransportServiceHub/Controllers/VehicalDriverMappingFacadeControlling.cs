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
    public class VehicalDriverMappingFacadeControlling : Controller
    {

        public VehicalDriverMappingInterface _vehicaldriver;
        public VehicalDriverMappingFacadeControlling(VehicalDriverMappingInterface detail)
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
        public VehicalDriverMappingDTO getdata(int id)
        {
            return _vehicaldriver.getdata(id);
        }

        [Route("savedata")]
        public VehicalDriverMappingDTO savedata([FromBody] VehicalDriverMappingDTO id)
        {
            return _vehicaldriver.savedata(id);
        }

        [Route("activedeactive")]
        public VehicalDriverMappingDTO activedeactive([FromBody] VehicalDriverMappingDTO data)
        {
            return _vehicaldriver.activedeactive(data);
        }

        [Route("editdata")]
        public VehicalDriverMappingDTO editdata([FromBody] VehicalDriverMappingDTO data)
        {
            return _vehicaldriver.editdata(data);
        }

    }
}
