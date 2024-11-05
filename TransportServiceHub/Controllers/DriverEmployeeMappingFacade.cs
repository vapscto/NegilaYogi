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
    public class DriverEmployeeMappingFacade : Controller
    {
        public DriverEmployeeMappingInterface _driveremp;
        public DriverEmployeeMappingFacade(DriverEmployeeMappingInterface detail)
        {
            _driveremp = detail;
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
        public DriverEmployeeMappingDTO getdata(int id)
        {
            return _driveremp.getdata(id);
        }

        [Route("savedata")]
        public DriverEmployeeMappingDTO savedata([FromBody] DriverEmployeeMappingDTO id)
        {
            return _driveremp.savedata(id);
        }



        [Route("edit")]
        public DriverEmployeeMappingDTO edit([FromBody] DriverEmployeeMappingDTO id)
        {
            return _driveremp.edit(id);
        }
        [Route("deletedata")]
        public DriverEmployeeMappingDTO deletedata([FromBody] DriverEmployeeMappingDTO data)
        {
            return _driveremp.deletedata(data);
        }

    }
}
