using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class PointsReportFacadeController : Controller
    {
        public PointsReportInterface _report;
        public PointsReportFacadeController(PointsReportInterface _screport)
        {
            _report = _screport;
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
        [Route("getdetails")]
        public PointsReportDTO getdetails([FromBody]PointsReportDTO dd)
        {
            return _report.getdetails(dd);
        }

        // POST api/values
        [HttpPost]
        [Route("Getreportdetails")]
        public Task<PointsReportDTO> Getreportdetails([FromBody]PointsReportDTO data)
        {
            return _report.Getreportdetails(data);
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
