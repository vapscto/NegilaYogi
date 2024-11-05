using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Feedback.Interface;
using PreadmissionDTOs.NAAC.FeedBack;

namespace NaacServiceHub.Feedback.Facade
{
    [Produces("application/json")]
    [Route("api/FeedBackSchoolReportFacade")]
    public class FeedBackSchoolReportFacadeController : Controller
    {
        public FeedBackSchoolReportInterface _interface;

        public FeedBackSchoolReportFacadeController(FeedBackSchoolReportInterface intf)
        {
            _interface = intf;
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
        public FeedBackSchoolReportDTO getdetails([FromBody]FeedBackSchoolReportDTO data)
        {
            return _interface.getdetails(data);
        }
        [HttpPost]
        [Route("getreport")]
        public FeedBackSchoolReportDTO getreport([FromBody]FeedBackSchoolReportDTO data)
        {
            return _interface.getreport(data);
        }
        [Route("count")]
        public FeedBackSchoolReportDTO count([FromBody]FeedBackSchoolReportDTO data)
        {
            return _interface.count(data);
        }
        //onclass
        [Route("onclass")]
        public FeedBackSchoolReportDTO onclass([FromBody]FeedBackSchoolReportDTO data)
        {
            return _interface.onclass(data);
        }

    }
}