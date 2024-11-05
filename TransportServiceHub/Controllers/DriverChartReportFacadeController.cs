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
    public class DriverChartReportFacadeController : Controller
    {
        public DriverChartReportInterface driverint;

        public DriverChartReportFacadeController(DriverChartReportInterface driv)
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
        public DriverChartReportDTO getdata(int id)
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
       
        [Route("Getreportdetails")]
        public DriverChartReportDTO Getreportdetails([FromBody] DriverChartReportDTO data)
        {
            return driverint.Getreportdetails(data);
        } 
        [Route("vehicletypechange")]
        public DriverChartReportDTO vehicletypechange([FromBody] DriverChartReportDTO data)
        {
            return driverint.vehicletypechange(data);
        }
      

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
