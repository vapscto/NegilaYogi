using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class GroupwiseSubListReportFacadeController : Controller
    {
        private GroupwiseSubListReportInterface _inter;

        public GroupwiseSubListReportFacadeController(GroupwiseSubListReportInterface obj)
        {
            _inter = obj;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [Route("getdetails")]
        public GroupwiseSubListReportDTO Getdetails([FromBody] GroupwiseSubListReportDTO data)
        {
            return _inter.getdetails(data);
        }

        [Route("onchangeyear")]
        public GroupwiseSubListReportDTO onchangeyear([FromBody] GroupwiseSubListReportDTO data)
        {
            return _inter.onchangeyear(data);
        }

        [Route("onreport")]
        public GroupwiseSubListReportDTO onreport([FromBody] GroupwiseSubListReportDTO data)
        {
            return _inter.onreport(data);
        }       
    }
}
