using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class SeatBlockReportController : Controller
    {
        SeatBlockReportDelegate SBR = new SeatBlockReportDelegate();
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


        //[Route("getnameregno")]
        //public SeatBlockReportDTO getstuddetails([FromBody]SeatBlockReportDTO value)
        //{
        //    return SBR.getstuddetails(value);
        //}

        [Route("getdetails/{id:int}")]
        public SeatBlockReportDTO getdetails( int id)
        {
            SeatBlockReportDTO data = new SeatBlockReportDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.mid = mid;
            return SBR.getdetails(data);
        }

        [Route("Getstudlist")]
        public SeatBlockReportDTO Getstudlist([FromBody] SeatBlockReportDTO stud)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            stud.mid = mid;
            return SBR.Getstudlist(stud);
        }


        [Route("Getreportdetails")]
        public SeatBlockReportDTO Getreportdetails([FromBody] SeatBlockReportDTO stud)
        {
            return SBR.Getreportdetails(stud);
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
