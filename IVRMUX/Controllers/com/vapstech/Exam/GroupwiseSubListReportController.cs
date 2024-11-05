using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class GroupwiseSubListReportController : Controller
    {
        GroupwiseSubListReportDelegates _delobj = new GroupwiseSubListReportDelegates();

        // GET: api/values
        [HttpGet]

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("getdetails")]
        public GroupwiseSubListReportDTO Get([FromQuery] int id)
        {
             GroupwiseSubListReportDTO data = new GroupwiseSubListReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getdetails(data);
        }

        [Route("onchangeyear")]
        public GroupwiseSubListReportDTO onchangeyear([FromBody]GroupwiseSubListReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onchangeyear(data);
        }

        [Route("onreport")]
        public GroupwiseSubListReportDTO onreport([FromBody]GroupwiseSubListReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onreport(data);
        }              
    }
}
