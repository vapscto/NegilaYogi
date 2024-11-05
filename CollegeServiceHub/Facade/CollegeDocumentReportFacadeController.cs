using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CollegeDocumentReportFacadeController : Controller
    {
        public CollegeDocumentReportInterface _inter;
        public CollegeDocumentReportFacadeController(CollegeDocumentReportInterface p)
        {
            _inter = p;
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
        public CollegeDocumentReportDTO getdetails([FromBody] CollegeDocumentReportDTO data)
        {
            return _inter.getdetails(data);
        }

        [Route("onchangeyear")]
        public CollegeDocumentReportDTO onchangeyear([FromBody] CollegeDocumentReportDTO data)
        {            
            return _inter.onchangeyear(data);
        }

        [Route("onchangecourse")]
        public CollegeDocumentReportDTO onchangecourse([FromBody] CollegeDocumentReportDTO data)
        {
            return _inter.onchangecourse(data);
        }

        [Route("onchangebranch")]
        public CollegeDocumentReportDTO onchangebranch([FromBody] CollegeDocumentReportDTO data)
        {
            return _inter.onchangebranch(data);
        }

        [Route("onchangesemester")]
        public CollegeDocumentReportDTO onchangesemester([FromBody] CollegeDocumentReportDTO data)
        {
            return _inter.onchangesemester(data);
        }

        [Route("onchangesection")]
        public CollegeDocumentReportDTO onchangesection([FromBody] CollegeDocumentReportDTO data)
        {
            return _inter.onchangesection(data);
        }

        [Route("getreportdetails")]
        public CollegeDocumentReportDTO getreportdetails([FromBody] CollegeDocumentReportDTO data)
        {
            return _inter.getreportdetails(data);
        }
        //document view
        [Route("getdetails_view")]
        public CollegeDocumentReportDTO getdetails_view([FromBody]CollegeDocumentReportDTO miid)
        {
            return _inter.getdetails_view(miid);
        }

        [Route("getclgstudata_view")]
        public CollegeDocumentReportDTO getclgstudata_view([FromBody]CollegeDocumentReportDTO miid)
        {
            return _inter.getclgstudata_view(miid);
        }
    }
}
