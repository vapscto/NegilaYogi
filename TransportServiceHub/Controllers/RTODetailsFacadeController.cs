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
    public class RTODetailsFacadeController : Controller
    {
        public RTODetailsInterface driverint;

        public RTODetailsFacadeController(RTODetailsInterface driv)
        {
            driverint = driv;
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
        public RTODetailsDTO getdata(int id)
        {
            return driverint.getdata(id);
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
        public RTODetailsDTO savedata([FromBody] RTODetailsDTO data)
        {
            return driverint.savedata(data);
        }
        [Route("Onvahiclechange")]
        public RTODetailsDTO Onvahiclechange([FromBody] RTODetailsDTO data)
        {
            return driverint.Onvahiclechange(data);
        }

        
        [Route("edit")]
        public RTODetailsDTO edit([FromBody] RTODetailsDTO data)
        {
            return driverint.edit(data);
        }

        [Route("deleterecord")]
        public RTODetailsDTO deleterecord([FromBody] RTODetailsDTO data)
        {
            return driverint.deleterecord(data);
        }

        
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
