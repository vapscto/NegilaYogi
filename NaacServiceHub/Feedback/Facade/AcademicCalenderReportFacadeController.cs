using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.FeedBack;
using NaacServiceHub.FeedBack.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.FeedBack.Facade
{
    [Route("api/[controller]")]
    public class AcademicCalenderReportFacadeController : Controller
    {

        public AcademicCalenderReportInterface _interface;

        public AcademicCalenderReportFacadeController(AcademicCalenderReportInterface inte)
        {
            _interface = inte;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getdetails")]
        public AcademicCalenderReportDTO getdetails([FromBody] AcademicCalenderReportDTO data)
        {
            return _interface.getdetails(data);
        }
        [Route("getreport")]
        public AcademicCalenderReportDTO getreport([FromBody] AcademicCalenderReportDTO data)
        {
            return _interface.getreport(data);
        }


    }
}
