using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using AdmissionServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class StudenttcReportcustomFacadeController : Controller
    {
        public StudenttcReportcustomInterface _studTC;
       

        public StudenttcReportcustomFacadeController(StudenttcReportcustomInterface studTC)
        {
            _studTC = studTC;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getstuddetails")]
        public StudentAttendanceReportDTO getstuddetails([FromBody]StudentAttendanceReportDTO data)
        {
            return _studTC.getstuddetails(data);
        }
        //getalldetails

        // GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
        // GET: api/values
        //[HttpGet]
        // load initial dropdown
        [Route("getinitialdata/{mi_id:int}")]
        public Task<StudentAttendanceReportDTO> getinitialdata(int mi_id)
        {
            return _studTC.getInitailData(mi_id);
        }
        [Route("getstudlist/{id:int}")]
        public StudentAttendanceReportDTO getstudlist(int id)
        {
            return _studTC.getstudlist(id);
        }
        // POST api/values
       [HttpPost]
  
        public StudentAttendanceReportDTO getTCdata([FromBody] StudentAttendanceReportDTO data)
        {
            return _studTC.getTCdata(data);
        }
        [Route("getTcdetailsbwmc")]
        public StudentAttendanceReportDTO getTcdetailsbwmc([FromBody] StudentAttendanceReportDTO data)
        {
            return _studTC.getTcdetailsbwmc(data);
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
