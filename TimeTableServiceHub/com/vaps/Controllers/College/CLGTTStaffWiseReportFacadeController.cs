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
    public class CLGTTStaffWiseReportFacadeController : Controller
    {
        public CLGTTStaffWiseReportInterface _ttbreaktime;
        public CLGTTStaffWiseReportFacadeController(CLGTTStaffWiseReportInterface maspag)
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
        public CLGTTStaffWiseReportDTO getorgdet([FromBody] CLGTTStaffWiseReportDTO data)
        {
            return _ttbreaktime.getdetails(data);
        }
       
        [Route("getrpt")]
        public CLGTTStaffWiseReportDTO getrpt([FromBody] CLGTTStaffWiseReportDTO data)
        {
            return _ttbreaktime.getreport(data);
        }

        [Route("GetStaffDetails")]
        public Task<CLGTTStaffWiseReportDTO> GetStaffDetails([FromBody] CLGTTStaffWiseReportDTO data)
        {
            return _ttbreaktime.GetStaffDetails(data);
        }
    }
}
