using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ToppersListReportFacadeController : Controller
    {
        private ToppersListReportInterface _inter;
        public ToppersListReportFacadeController(ToppersListReportInterface obj)
        {
            _inter = obj;
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


        [Route("getdetails")]
        public ToppersListReportDTO Getdetails([FromBody] ToppersListReportDTO data)
        {
            return _inter.getdetails(data);
        }

        [Route("onselectCategory")]
        public ToppersListReportDTO onselectCategory([FromBody] ToppersListReportDTO data)
        {
            return _inter.onselectCategory(data);
        }

        [Route("onselectclass")]
        public ToppersListReportDTO onselectclass([FromBody] ToppersListReportDTO data)
        {
            return _inter.onselectclass(data);
        }
        [Route("onreport")]
        public ToppersListReportDTO onreport([FromBody] ToppersListReportDTO data)
        {
            return _inter.onreport(data);
        }
        [Route("get_sec_exam")]
        public ToppersListReportDTO get_sec_exam([FromBody] ToppersListReportDTO data)
        {
            return _inter.get_sec_exam(data);
        }
        [Route("onselectexam")]
        public ToppersListReportDTO onselectexam([FromBody] ToppersListReportDTO data)
        {
            return _inter.onselectexam(data);
        }
        [Route("get_subject")]
        public ToppersListReportDTO get_subject([FromBody] ToppersListReportDTO data)
        {
            return _inter.get_subject(data);
        }

        [Route("sendsms")]
        public ToppersListReportDTO sendsms([FromBody] ToppersListReportDTO data)
        {
            return _inter.sendsms(data);
        }

        //Kiosk Exam Topper List.
        [Route("KioskExamTopper")]
        public ToppersListReportDTO.KioskExamTopperDTO KioskExamTopper([FromBody] ToppersListReportDTO kiosk)
        {
            return _inter.kioskExamToppers(kiosk);
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
