using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTableServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class CLGTTCourseReportBWMCFacadeController : Controller
    {
        public CLGTTCourseReportBWMCInterface _ttbreaktime;
        public CLGTTCourseReportBWMCFacadeController(CLGTTCourseReportBWMCInterface maspag)
        {
            _ttbreaktime = maspag;
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

        [Route("getdetails")]
        public CLGTTCourseReportBWMCDTO getorgdet([FromBody] CLGTTCourseReportBWMCDTO data)
        {
            return _ttbreaktime.getdetails(data);
        }
        [Route("getclass_catg")]
        public CLGTTCourseReportBWMCDTO getclass_catg([FromBody] CLGTTCourseReportBWMCDTO data)
        {
            return _ttbreaktime.getclass_catg(data);
        }
        [Route("getrpt")]
        public CLGTTCourseReportBWMCDTO getrpt([FromBody] CLGTTCourseReportBWMCDTO data)
        {
            return _ttbreaktime.getreport(data);
        }
       
    }
}
