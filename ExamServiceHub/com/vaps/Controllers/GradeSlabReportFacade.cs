
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
//using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{


    [Route("api/[controller]")]
    public class GradeSlabReportFacade : Controller
    {
        public GradeSlabReportInterface _examcateReport;


        public GradeSlabReportFacade(GradeSlabReportInterface data)
        {
            _examcateReport = data;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("getdetails")]
        public GradeSlabReportDTO getdetails([FromBody]GradeSlabReportDTO data)//int IVRMM_Id
        {

            return _examcateReport.getdetails(data);

        }


        [Route("getAttendetails")]
        public GradeSlabReportDTO getAttendetails([FromBody] GradeSlabReportDTO data)
        {
            return _examcateReport.getAttendetails(data);
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
