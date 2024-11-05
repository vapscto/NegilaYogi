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
    public class MarksReportFacadeController : Controller
    {
        public MarksReportInterface _report;
        public MarksReportFacadeController(MarksReportInterface _screport)
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

        [Route ("getdetails")]
        public MarksReportDTO getdetails([FromBody]MarksReportDTO dd)
        {
            return _report.getdetails(dd);
        }

        [Route("schedulelist")]
        public MarksReportDTO schedulelist([FromBody]MarksReportDTO data)
        {
            return _report.schedulelist(data);
        }

        // POST api/values
        [HttpPost]

        [Route ("Getreportdetails")]
        public MarksReportDTO Getreportdetails ([FromBody]MarksReportDTO data)
        {
            return _report.Getreportdetails(data);
        }
        [Route("Getreportdetailssrkvs")]
        public MarksReportDTO Getreportdetailssrkvs([FromBody]MarksReportDTO data)
        {
            return _report.Getreportdetailssrkvs(data);
        }
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
