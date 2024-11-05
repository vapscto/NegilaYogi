using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class VikasaLUFacadeController : Controller
    {
        public VikasaLUInterface _luinter;
        public VikasaLUFacadeController(VikasaLUInterface obj)
        {
            _luinter = obj;
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

        // POST api/values
        [HttpPost]
        [Route("getdetails")]
        public VikasaLUReportDTO Getdetails([FromBody] VikasaLUReportDTO data)
        {
            return _luinter.getdetails(data);
        }

        [Route("onselectAcdYear")]
        public VikasaLUReportDTO onselectAcdYear([FromBody] VikasaLUReportDTO data)
        {
            return _luinter.onselectAcdYear(data);
        }

        [Route("onselectclass")]
        public VikasaLUReportDTO onselectclass([FromBody] VikasaLUReportDTO data)
        {
            return _luinter.onselectclass(data);
        }

        [Route("onselectSection")]
        public VikasaLUReportDTO onselectSection([FromBody] VikasaLUReportDTO data)
        {
            return _luinter.onselectSection(data);
        }

        [Route("onreport")]
        public VikasaLUReportDTO onreport([FromBody] VikasaLUReportDTO data)
        {
            return _luinter.onreport(data);
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
