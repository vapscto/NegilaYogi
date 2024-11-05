using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Transport;
using TransportServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TransportServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class MasterVehicleFacadeController : Controller
    {
        public MasterVehicleInterface _vehicle;
        public MasterVehicleFacadeController(MasterVehicleInterface vehicle)
        {
            _vehicle = vehicle;
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
        public MasterVehicleDTO getdata(int id)
        {
            return _vehicle.getdata(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("savedata")]
        public MasterVehicleDTO savedata([FromBody] MasterVehicleDTO id)
        {
            return _vehicle.savedata(id);
        }
        [Route("edit")]
        public MasterVehicleDTO edit([FromBody] MasterVehicleDTO id)
        {
            return _vehicle.edit(id);
        }

        [Route("activedeactive")]
        public MasterVehicleDTO activedeactive([FromBody] MasterVehicleDTO id)
        {
            return _vehicle.activedeactive(id);
        }

        [Route("validaevehicleno")]
        public MasterVehicleDTO validaevehicleno([FromBody] MasterVehicleDTO id)
        {
            return _vehicle.validaevehicleno(id);
        }
        [Route("rcreport")]
        public MasterVehicleDTO rcreport([FromBody] MasterVehicleDTO id)
        {
            return _vehicle.rcreport(id);
        }
        [Route("validaevhassiseno")]
        public MasterVehicleDTO validaevhassiseno([FromBody] MasterVehicleDTO id)
        {
            return _vehicle.validaevhassiseno(id);
        }

        [Route("validaeengineno")]
        public MasterVehicleDTO validaeengineno([FromBody] MasterVehicleDTO id)
        {
            return _vehicle.validaeengineno(id);
        }

        [Route("viewuploadflies")]
        public MasterVehicleDTO viewuploadflies([FromBody] MasterVehicleDTO id)
        {
            return _vehicle.viewuploadflies(id);
        }

        [Route("deleteuploadfile")]
        public MasterVehicleDTO deleteuploadfile([FromBody] MasterVehicleDTO id)
        {
            return _vehicle.deleteuploadfile(id);
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
