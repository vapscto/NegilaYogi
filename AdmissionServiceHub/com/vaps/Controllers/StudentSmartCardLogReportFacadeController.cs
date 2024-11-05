using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class StudentSmartCardLogReportFacadeController : Controller
    {
        public StudentSmartCardLogReportInterface _feegrouppagee;
        // GET: api/values

        public StudentSmartCardLogReportFacadeController(StudentSmartCardLogReportInterface maspag)
        {
            _feegrouppagee = maspag;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Route("getdetails")]
        public  Task<StudentSmartCardLogReportDTO> getdetails(StudentSmartCardLogReportDTO data)
        {
            return _feegrouppagee.getdetails(data);
        }

      
        [Route("getregnoname")]
        public StudentSmartCardLogReportDTO getregnoname([FromBody]StudentSmartCardLogReportDTO value)
        {
            return _feegrouppagee.getstuddet(value);
        }
        [Route ("Getreportdetails")]
        public  Task<StudentSmartCardLogReportDTO> Getreportdetails([FromBody] StudentSmartCardLogReportDTO report)
        {
            return _feegrouppagee.Getreportdetails(report);
        }

        // POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

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
