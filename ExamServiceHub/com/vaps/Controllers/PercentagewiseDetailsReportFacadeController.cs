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
    public class PercentagewiseDetailsReportFacadeController : Controller
    {
        private PercentagewiseDetailsReportInterface _inter;
        public PercentagewiseDetailsReportFacadeController(PercentagewiseDetailsReportInterface obj)
        {
            _inter = obj;
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
        public PercentagewiseDetailsReportDTO Getdetails([FromBody] PercentagewiseDetailsReportDTO data)
        {
            return _inter.getdetails(data);
        }

        [Route("onselectAcdYear")]
        public PercentagewiseDetailsReportDTO onselectAcdYear([FromBody] PercentagewiseDetailsReportDTO data)
        {
            return _inter.onselectAcdYear(data);
        }

        [Route("onselectCategory")]
        public PercentagewiseDetailsReportDTO onselectCategory([FromBody] PercentagewiseDetailsReportDTO data)
        {
            return _inter.onselectCategory(data);
        }

        [Route("onselectclass")]
        public PercentagewiseDetailsReportDTO onselectclass([FromBody] PercentagewiseDetailsReportDTO data)
        {
            return _inter.onselectclass(data);
        }

        [Route("onselectSection")]
        public PercentagewiseDetailsReportDTO onselectSection([FromBody] PercentagewiseDetailsReportDTO data)
        {
            return _inter.onselectSection(data);
        }

        [Route("onreport")]
        public PercentagewiseDetailsReportDTO onreport([FromBody] PercentagewiseDetailsReportDTO data)
        {
            return _inter.onreport(data);
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
