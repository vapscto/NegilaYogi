using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using corewebapi18072016.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class StudentSmartCardLogReportsController : Controller
    {
        StudentSmartCardLogReportDTO data = new StudentSmartCardLogReportDTO();
        StudentSmartCardLogReportDelegate log = new StudentSmartCardLogReportDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }



        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public StudentSmartCardLogReportDTO Get(int id)
        {

            return log.getdetails(id);

        }


       
        [Route("getnameregno")]
        public StudentSmartCardLogReportDTO getstuddetails([FromBody]StudentSmartCardLogReportDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            value.asmay_id = ASMAY_Id;

            return log.getstuddet(value);
        }



        [Route("Getreportdetails")]
        public StudentSmartCardLogReportDTO Getreportdetails([FromBody]StudentSmartCardLogReportDTO data)
        {
            return log.Getreportdetails(data);
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
    }
}
