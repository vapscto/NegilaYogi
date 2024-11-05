using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.FeedBack.Interface;
using PreadmissionDTOs.FeedBack;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.FeedBack.Facade
{
    [Route("api/[controller]")]
    public class FeedBackReportFacadeController : Controller
    {
        public FeedBackReportInterface _interface;

        public FeedBackReportFacadeController(FeedBackReportInterface intf)
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
        public FeedBackReportDTO getdetails([FromBody]FeedBackReportDTO data)
        {            
            return _interface.getdetails(data);
        }

        [Route("onchangeradio")]
        public FeedBackReportDTO onchangeradio([FromBody]FeedBackReportDTO data)
        {
            return _interface.onchangeradio(data);
        }

        [Route("getreport")]
        public FeedBackReportDTO getreport([FromBody]FeedBackReportDTO data)
        {            
            return _interface.getreport(data);
        }
        [Route("onchangeyear")]
        public FeedBackReportDTO onchangeyear([FromBody]FeedBackReportDTO data)
        {
            return _interface.onchangeyear(data);
        }
        [Route("getreportnew")]
        public FeedBackReportDTO getreportnew([FromBody]FeedBackReportDTO data)
        {
            return _interface.getreportnew(data);
        }
        [Route("onchangefeedback")]
        public FeedBackReportDTO onchangefeedback([FromBody]FeedBackReportDTO data)
        {
            return _interface.onchangefeedback(data);
        }
        [Route("getstudentlist")]
        public FeedBackReportDTO getstudentlist([FromBody]FeedBackReportDTO data)
        {
            return _interface.getstudentlist(data);
        }
        
    }
}
