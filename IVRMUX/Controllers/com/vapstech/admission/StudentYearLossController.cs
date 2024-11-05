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
    public class StudentYearLossController : Controller
    {
        StudentYearLosReportDTO data = new StudentYearLosReportDTO();
        StudentyearLossReportDelegate loss = new StudentyearLossReportDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        // GET api/values/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public StudentYearLosReportDTO Get(StudentYearLosReportDTO data1)
        {
        
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data1.mid = mid;
            return loss.getdetails(data1);

        }

        [Route("Getreportdetails")]
        public StudentTcReportDTO Getreportdetails([FromBody] StudentTcReportDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.mid = mid;
            return loss.GetData(MMD);
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
