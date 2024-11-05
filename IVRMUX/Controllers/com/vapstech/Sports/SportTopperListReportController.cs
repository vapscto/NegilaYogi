using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Sports;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class SportTopperListReportController : Controller
    {
        SportTopperListReportDelegate crStr = new SportTopperListReportDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails")]
        public SportStudentParticipationReportDTO Getdetails(SportStudentParticipationReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getdetails(data);
        }


        [Route("showdetails")]
        public SportStudentParticipationReportDTO showdetails([FromBody] SportStudentParticipationReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return crStr.showdetails(data);
        }

        [Route("get_class")]
        public SportStudentParticipationReportDTO get_class([FromBody]SportStudentParticipationReportDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            return crStr.get_class(data);
        }

        [Route("get_section")]
        public SportStudentParticipationReportDTO get_section([FromBody]SportStudentParticipationReportDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return crStr.get_section(data);
        }

        [Route("get_student")]
        public SportStudentParticipationReportDTO get_student([FromBody]SportStudentParticipationReportDTO data)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return crStr.get_student(data);
        }




    }
}
