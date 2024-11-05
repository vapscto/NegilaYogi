using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Sports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class StudentAgeCalcReportController : Controller
    {
        // GET: api/<controller>

        StudentAgeCalcReportDelegate crStr = new StudentAgeCalcReportDelegate();

        // GET: api/values
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


    }
}
