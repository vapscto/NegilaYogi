using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class StudentActiveInactiveReportController : Controller
    {
        public StudentActiveInactiveReportDelegate _deg = new StudentActiveInactiveReportDelegate();
        
        [Route("getdata/{id:int}")]
        public StudentActiveInactiveReportDTO getdata(int id)
        {
            StudentActiveInactiveReportDTO data = new StudentActiveInactiveReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _deg.getdata(data);
        }
        [Route("onacademicyearchange")]
        public StudentActiveInactiveReportDTO onacademicyearchange([FromBody] StudentActiveInactiveReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _deg.onacademicyearchange(data);
        }
        [Route("oncoursechange")]
        public StudentActiveInactiveReportDTO oncoursechange([FromBody] StudentActiveInactiveReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _deg.oncoursechange(data);
        }
        [Route("onbranchchange")]
        public StudentActiveInactiveReportDTO onbranchchange([FromBody] StudentActiveInactiveReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _deg.onbranchchange(data);
        }
        [Route("onchangesemester")]
        public StudentActiveInactiveReportDTO onchangesemester([FromBody] StudentActiveInactiveReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _deg.onchangesemester(data);
        }        
        [Route("getreport")]
        public StudentActiveInactiveReportDTO getreport([FromBody] StudentActiveInactiveReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _deg.getreport(data);
        }


    }
}
