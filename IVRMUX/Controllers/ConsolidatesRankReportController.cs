using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [Route("api/[controller]")]
    public class ConsolidatesRankReportController : Controller
    {
        ConsolidatesRankReportDelegate crStr = new ConsolidatesRankReportDelegate();

        [HttpGet]
        [Route("Getdetails")]
        public WrittenTestMarksBindDataDTO Getdetails(WrittenTestMarksBindDataDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.ASMAY_Id = ASMAY_Id;

            return crStr.Getdetails(MMD);
        }

        [Route("getclass/{id}")]
        public WrittenTestMarksBindDataDTO getclass(int id)
        {
            WrittenTestMarksBindDataDTO data = new WrittenTestMarksBindDataDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = id;
            return crStr.getclass(data);
        }
        [HttpPost]
        [Route("Getreport")]
        public WrittenTestMarksBindDataDTO Getreport([FromBody] WrittenTestMarksBindDataDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return crStr.Getreport(data);

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
    }
}
