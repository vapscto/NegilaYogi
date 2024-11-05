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
    public class CollegeStudyCertificateReportFacadeController : Controller
    {

        public CollegeStudyCertificateReportInterface _interface;

        public CollegeStudyCertificateReportFacadeController(CollegeStudyCertificateReportInterface _interf)
        {
            _interface = _interf;
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

        [Route("getdata")]
        public CollegeStudyCertificateReportDTO getdata([FromBody] CollegeStudyCertificateReportDTO data)
        {
            return _interface.getdata(data);
        }
        [Route("onchangeyear")]
        public CollegeStudyCertificateReportDTO onchangeyear([FromBody] CollegeStudyCertificateReportDTO data)
        {

            return _interface.onchangeyear(data);
        }
        [Route("onchangecourse")]
        public CollegeStudyCertificateReportDTO onchangecourse([FromBody] CollegeStudyCertificateReportDTO data)
        {

            return _interface.onchangecourse(data);
        }
        [Route("onchangebranch")]
        public CollegeStudyCertificateReportDTO onchangebranch([FromBody] CollegeStudyCertificateReportDTO data)
        {

            return _interface.onchangebranch(data);
        }
        [Route("onchangesemester")]
        public CollegeStudyCertificateReportDTO onchangesemester([FromBody] CollegeStudyCertificateReportDTO data)
        {

            return _interface.onchangesemester(data);
        }

        [Route("searchfilter")]
        public CollegeStudyCertificateReportDTO searchfilter([FromBody] CollegeStudyCertificateReportDTO data)
        {

            return _interface.searchfilter(data);
        }
        [Route("onclickreport")]
        public CollegeStudyCertificateReportDTO onclickreport([FromBody] CollegeStudyCertificateReportDTO data)
        {

            return _interface.onclickreport(data);
        }
    }
}
