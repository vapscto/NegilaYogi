using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MarksEntryReportFacadeController : Controller
    {
        MarksEntryReportInterface _inter;
        public MarksEntryReportFacadeController(MarksEntryReportInterface inter)
        {
            _inter = inter;
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
        public MarksEntryReportDTO Getdetails([FromBody] MarksEntryReportDTO data)
        {           
            return _inter.Getdetails(data);
        }

        [Route("get_class")]
        public MarksEntryReportDTO get_class([FromBody] MarksEntryReportDTO data)
        {           
            return _inter.get_class(data);
        }
        [Route("get_section")]
        public MarksEntryReportDTO get_section([FromBody] MarksEntryReportDTO data)
        {           
            return _inter.get_section(data);
        }
        [Route("get_exam")]
        public MarksEntryReportDTO get_exam([FromBody] MarksEntryReportDTO data)
        {           
            return _inter.get_exam(data);
        }

        [Route("get_report")]
        public MarksEntryReportDTO get_report([FromBody] MarksEntryReportDTO data)
        {           
            return _inter.get_report(data);
        }
        [Route("get_markspublishreport")]
        public MarksEntryReportDTO get_markspublishreport([FromBody] MarksEntryReportDTO data)
        {           
            return _inter.get_markspublishreport(data);
        }
    }
}
