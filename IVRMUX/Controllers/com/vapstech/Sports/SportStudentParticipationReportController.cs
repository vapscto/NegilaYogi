using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Sports;
using corewebapi18072016.Delegates.com.vapstech.Sports;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class SportStudentParticipationReportController : Controller
    {
        SportStudentParticipationReportDelegate crStr = new SportStudentParticipationReportDelegate();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("Getdetails")]
        public StudentAgeCalcReport_DTO Getdetails(StudentAgeCalcReport_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getdetails(data);
        }


        [Route("showdetails")]
        public StudentAgeCalcReport_DTO showdetails([FromBody] StudentAgeCalcReport_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return crStr.showdetails(data);
        }

        [Route("get_class")]
        public StudentAgeCalcReport_DTO get_class([FromBody]StudentAgeCalcReport_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.get_class(data);
        }

        [Route("get_section")]
        public StudentAgeCalcReport_DTO get_section([FromBody]StudentAgeCalcReport_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.get_section(data);
        }

        [Route("get_student")]
        public StudentAgeCalcReport_DTO get_student([FromBody]StudentAgeCalcReport_DTO data)

        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.get_student(data);
        }

        #region
        //[Route("Getdetails")]
        //public SportStudentParticipationReportDTO Getdetails(SportStudentParticipationReportDTO data)
        //{
        //    data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return crStr.Getdetails(data);
        //}
        //[Route("getevent")]
        //public SportStudentParticipationReportDTO getevent([FromBody] SportStudentParticipationReportDTO data)
        //{
        //    data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return crStr.getevent(data);
        //}
        //[Route("showdetails")]
        //public SportStudentParticipationReportDTO showdetails([FromBody] SportStudentParticipationReportDTO data)
        //{
        //    data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return crStr.showdetails(data);
        //}
        //[Route("get_class")]
        //public SportStudentParticipationReportDTO get_class([FromBody]SportStudentParticipationReportDTO data)
        //{
        //    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return crStr.get_class(data);
        //}
        //[Route("get_section")]
        //public SportStudentParticipationReportDTO get_section([FromBody]SportStudentParticipationReportDTO data)
        //{
        //    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return crStr.get_section(data);
        //}
        //[Route("get_student")]
        //public SportStudentParticipationReportDTO get_student([FromBody]SportStudentParticipationReportDTO data)
        //{
        //    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return crStr.get_student(data);
        //}
        #endregion


    }
}
