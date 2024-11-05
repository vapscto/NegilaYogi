using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class SeatBlockReportFacadeController : Controller
    {

        public SeatBlockReportInterface _report;
        public SeatBlockReportFacadeController(SeatBlockReportInterface _screport)
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
        //[HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        //[Route("getstuddetails")]
        //public SeatBlockReportDTO getstuddetails([FromBody]SeatBlockReportDTO data)
        //{
        //    return _report.getstuddetails(data);
        //}


        [Route("getdetails")]
        public SeatBlockReportDTO getdetails([FromBody] SeatBlockReportDTO data)
        {
            return _report.getdetails(data);
        }

        [Route("Getstudlist")]
        public SeatBlockReportDTO Getstudlist([FromBody] SeatBlockReportDTO data)
        {
            return _report.Getstudlist(data);
        }

        [Route ("Getreportdetails")]

        public Task<SeatBlockReportDTO> Getreportdetails([FromBody] SeatBlockReportDTO data)
        {
            return _report.Getreportdetails(data);
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
