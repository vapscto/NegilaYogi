using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.FeedBack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.FeedBack;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.FeedBack
{
    [Route("api/[controller]")]
    public class FeedBackReportController : Controller
    {

        public FeedBackReportDelegate _delgate = new FeedBackReportDelegate();

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
        public FeedBackReportDTO getdetails (FeedBackReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.getdetails(data);
        }
        [Route("onchangeradio")]
        public FeedBackReportDTO onchangeradio([FromBody]FeedBackReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.onchangeradio(data);
        }
        

        [Route("getreport")]
        public FeedBackReportDTO getreport([FromBody]FeedBackReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.getreport(data);
        }
        [Route("onchangeyear")]
        public FeedBackReportDTO onchangeyear([FromBody]FeedBackReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.onchangeyear(data);
        }
        [Route("getreportnew")]
        public FeedBackReportDTO getreportnew([FromBody]FeedBackReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.getreportnew(data);
        }
        [Route("onchangefeedback")]
        public FeedBackReportDTO onchangefeedback([FromBody]FeedBackReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.onchangefeedback(data);
        }
        [Route("getstudentlist")]
        public FeedBackReportDTO getstudentlist([FromBody]FeedBackReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.getstudentlist(data);
        }
        
    }
}
