using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;
using CommonLibrary;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class ScheduleReportFacadeController : Controller
    {
        public ScheduleReportInterface _report;
        public ScheduleReportFacadeController(ScheduleReportInterface _screport)
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
        public ScheduleReportDTO getdetails([FromBody]ScheduleReportDTO dd)
        {
            return _report.getdetails(dd);
        }

        [Route("schedulelist")]
        public ScheduleReportDTO schedulelist([FromBody]ScheduleReportDTO data)
        {
            return _report.schedulelist(data);
        }

        // POST api/values
        [HttpPost]

        [Route ("Getreportdetails")]
        public Task<ScheduleReportDTO> Getreportdetails ([FromBody]ScheduleReportDTO data)
        {
            return _report.Getreportdetails(data);
        }

        [Route ("scheduleGetreportdetails")]
        public Task<ScheduleReportDTO> scheduleGetreportdetails([FromBody]ScheduleReportDTO data)
        {
            return _report.scheduleGetreportdetails(data);
        }
        [Route ("remarksGetreportdetails")]
        public Task<ScheduleReportDTO> remarksGetreportdetails([FromBody]ScheduleReportDTO data)
        {
            return _report.remarksGetreportdetails(data);
        }

        [Route("SendMail/")]

        public Task<ScheduleReportDTO> SendMail([FromBody] ScheduleReportDTO reg)
        {
            return _report.sendmail(reg);
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
