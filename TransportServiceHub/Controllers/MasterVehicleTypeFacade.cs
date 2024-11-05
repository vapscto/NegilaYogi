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
    public class MasterVehicleTypeFacade : Controller
    {
        public MasterVehicleTypeInterface _areaint;

        public MasterVehicleTypeFacade(MasterVehicleTypeInterface areaz)
        {
            _areaint = areaz;  
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
        public MasterVehicleTypeDTO getdata(int id)
        {

            return _areaint.getdata(id);
        }

        [Route("savedata")]
        public MasterVehicleTypeDTO savedata([FromBody]MasterVehicleTypeDTO data)
        {
            return _areaint.savedata(data);
        }
        [Route("geteditdata")]
        public MasterVehicleTypeDTO geteditdata([FromBody] MasterVehicleTypeDTO data)
        {
            return _areaint.geteditdata(data);
        }
        [Route("activedeactive")]
        public MasterVehicleTypeDTO activedeactive([FromBody] MasterVehicleTypeDTO data)
        {
            return _areaint.activedeactive(data);
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