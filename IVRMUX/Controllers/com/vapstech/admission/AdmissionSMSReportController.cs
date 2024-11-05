using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using corewebapi18072016.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class AdmissionSMSReportController : Controller
    {
        AdmissionSMSReportDelegate cwsd = new AdmissionSMSReportDelegate();
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

        [Route("getalldetails/{id:int}")]

        public AdmissionSMSReportDTO getalldetails(int id)
        {
            AdmissionSMSReportDTO dto = new AdmissionSMSReportDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return cwsd.getdetails(dto);

        }


        [HttpPost]
        [Route("Getreportdetails")]
        public AdmissionSMSReportDTO Getreportdetails([FromBody] AdmissionSMSReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cwsd.Getreportdetails(data);
        }





        // POST api/values

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
