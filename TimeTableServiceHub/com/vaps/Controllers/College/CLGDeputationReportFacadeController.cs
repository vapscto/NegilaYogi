using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;
using TimeTableServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class CLGDeputationReportFacadeController : Controller
    {
        public CLGDeputationReportInterface _feetar;

        public CLGDeputationReportFacadeController(CLGDeputationReportInterface maspag)
        {
            _feetar = maspag;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("getdata")]
        public CLGDeputationReportDTO getdata([FromBody] CLGDeputationReportDTO data)
        {
            return _feetar.getdata(data);
        }

        [Route("getreport")]
        public CLGDeputationReportDTO getreport([FromBody] CLGDeputationReportDTO data)
        {
            return _feetar.getreport(data);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
