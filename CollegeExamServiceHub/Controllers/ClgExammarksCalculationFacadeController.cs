using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeExamServiceHub.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeExamServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class ClgExammarksCalculationFacadeController : Controller
    {
        public ClgExammarksCalculationInterface _exmcalReport;

        public ClgExammarksCalculationFacadeController(ClgExammarksCalculationInterface data)
        {
            _exmcalReport = data;
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
        
        [Route("Getdetails")]
        public ClgMarksCalculationsDTO Getdetails([FromBody]ClgMarksCalculationsDTO data)
        {
            return _exmcalReport.Getdetails(data);
        }
        [Route("Calculation")]
        public ClgMarksCalculationsDTO Calculation([FromBody] ClgMarksCalculationsDTO org)
        {
            return _exmcalReport.Calculation(org);
        }   
    }
}
