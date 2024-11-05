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
    public class CollegeBMCPUProgresscardReportFacadeController : Controller
    {
        public CollegeBMCPUProgresscardReportInterface _interface;

        public CollegeBMCPUProgresscardReportFacadeController(CollegeBMCPUProgresscardReportInterface _inte)
        {
            _interface = _inte;
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
        public CollegeBMCPUProgresscardReportDTO Getdetails(CollegeBMCPUProgresscardReportDTO data)
        {
            return _interface.Getdetails(data);
        }

        [Route("OnAcdyear")]
        public CollegeBMCPUProgresscardReportDTO OnAcdyear(CollegeBMCPUProgresscardReportDTO data)
        {
            return _interface.OnAcdyear(data);
        }
        [Route("onchangecourse")]
        public CollegeBMCPUProgresscardReportDTO onchangecourse(CollegeBMCPUProgresscardReportDTO data)
        {
            return _interface.onchangecourse(data);
        }
        [Route("onchangebranch")]
        public CollegeBMCPUProgresscardReportDTO onchangebranch(CollegeBMCPUProgresscardReportDTO data)
        {
            return _interface.onchangebranch(data);
        }
        [Route("onchangesemester")]
        public CollegeBMCPUProgresscardReportDTO onchangesemester(CollegeBMCPUProgresscardReportDTO data)
        {
            return _interface.onchangesemester(data);
        }
        [Route("onchangesection")]
        public CollegeBMCPUProgresscardReportDTO onchangesection(CollegeBMCPUProgresscardReportDTO data)
        {
            return _interface.onchangesection(data);
        }
        [Route("onchangesubjectscheme")]
        public CollegeBMCPUProgresscardReportDTO onchangesubjectscheme(CollegeBMCPUProgresscardReportDTO data)
        {
            return _interface.onchangesubjectscheme(data);
        }
        [Route("onchangeschemetype")]
        public CollegeBMCPUProgresscardReportDTO onchangeschemetype(CollegeBMCPUProgresscardReportDTO data)
        {
            return _interface.onchangeschemetype(data);
        }

        [Route("getreport")]
        public CollegeBMCPUProgresscardReportDTO getreport(CollegeBMCPUProgresscardReportDTO data)
        {
            return _interface.getreport(data);
        }




    }
}
