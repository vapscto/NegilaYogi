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
    public class DriverChartFacadeController : Controller
    {
        public DriverChartInterface driverint;

        public DriverChartFacadeController(DriverChartInterface driv)
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
        public DriverChartDTO getdata(int id)
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
        public DriverChartDTO savedata([FromBody] DriverChartDTO data)
        {
            return driverint.savedata(data);
        }
        [Route("Onvahiclechange")]
        public DriverChartDTO Onvahiclechange([FromBody] DriverChartDTO data)
        {
            return driverint.Onvahiclechange(data);
        }
        [Route("deleterecord")]
        public DriverChartDTO deleterecord([FromBody] DriverChartDTO data)
        {
            return driverint.deleterecord(data);
        }
        
        [Route("edit")]
        public DriverChartDTO edit([FromBody] DriverChartDTO data)
        {
            return driverint.edit(data);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
