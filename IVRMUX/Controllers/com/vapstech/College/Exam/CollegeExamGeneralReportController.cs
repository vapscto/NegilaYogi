using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Exam
{
    [Route("api/[controller]")]
    public class CollegeExamGeneralReportController : Controller
    {
        CollegeExamGeneralReportDelegate _delg = new CollegeExamGeneralReportDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("MasterGradeReportLoadData/{id:int}")]
        public CollegeExamGeneralReportDTO MasterGradeReportLoadData(int id)
        {
            CollegeExamGeneralReportDTO data = new CollegeExamGeneralReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.MasterGradeReportLoadData(data);
        }

        [Route("MasterGradeReportDetails")]
        public CollegeExamGeneralReportDTO MasterGradeReportDetails([FromBody]  CollegeExamGeneralReportDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.MasterGradeReportDetails(data);
        }

    }
}
