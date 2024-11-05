using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SiblingEmployeeStudentReportFacadeController : Controller
    {
        public SiblingEmployeeStudentReportInterface _interface; 

        public SiblingEmployeeStudentReportFacadeController(SiblingEmployeeStudentReportInterface _inter)
        {
            _interface = _inter;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("getdetails")]
        public SiblingEmployeeStudentReportDTO getdetails([FromBody]SiblingEmployeeStudentReportDTO data)
        {
            return _interface.getdetails(data);
        }

        [Route("getreport")]
        public SiblingEmployeeStudentReportDTO getreport([FromBody]SiblingEmployeeStudentReportDTO data)
        {
            return _interface.getreport(data);
        }
    }
}
