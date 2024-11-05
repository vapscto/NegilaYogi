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
    public class StudentTcReportController : Controller
    {
        StudentTcReportDTO dto = new StudentTcReportDTO();
        StudentTcReportDelegate FGD = new StudentTcReportDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public StudentTcReportDTO Get(int id)
        {

            id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FGD.getdetails(id);

           
        }

        // POST api/values
       
        [Route("Getreportdetails")]
        public StudentTcReportDTO Getreportdetails([FromBody] StudentTcReportDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.mid = mid;
            return FGD.GetData(MMD);
        }
        [Route("getclass")]
        public StudentTcReportDTO getclass([FromBody] StudentTcReportDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.mid = mid;
            return FGD.getclass(MMD);
        }
        [Route("getsecton")]
        public StudentTcReportDTO getsecton([FromBody] StudentTcReportDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.mid = mid;
            return FGD.getsecton(MMD);
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
