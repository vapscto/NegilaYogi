using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.FeedBack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.FeedBack;

namespace IVRMUX.Controllers.NAAC.FeedBack
{
    [Produces("application/json")]
    [Route("api/FeedBackSchoolReport")]
    public class FeedBackSchoolReportController : Controller
    {
        public FeedBackSchoolReportDelegate _delgate = new FeedBackSchoolReportDelegate();

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
        public FeedBackSchoolReportDTO getdetails(FeedBackSchoolReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.getdetails(data);
        }
        [HttpPost]
        [Route("getreport")]
        public FeedBackSchoolReportDTO getreport([FromBody]FeedBackSchoolReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.getreport(data);
        }
        //FBGivenCount
        [Route("count")]
        public FeedBackSchoolReportDTO count([FromBody]FeedBackSchoolReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.count(data);
        }
        //onclass
        [Route("onclass")]
        public FeedBackSchoolReportDTO onclass([FromBody]FeedBackSchoolReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.onclass(data);
        }
    }
}