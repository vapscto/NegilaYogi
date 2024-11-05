using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.TT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.TT
{
    [Route("api/[controller]")]
    public class TTMonthEndReportController : Controller
    {
        TTMonthEndReportDelegate _dd = new TTMonthEndReportDelegate();
           // GET: api/<controller>
           [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails123")]
        public TTMonthEndReportDTO Get([FromQuery] int id)
        {
            TTMonthEndReportDTO data = new TTMonthEndReportDTO();
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _dd.getdata123(data);
        }        
        //  POST api/values

        [HttpPost]
        [Route("getreport")]
        public TTMonthEndReportDTO getreport([FromBody] TTMonthEndReportDTO data123)
        {
            data123.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _dd.getreport(data123);
        }
       
        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
