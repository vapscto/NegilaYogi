using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.SeatingArrangment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.SeatingArrangment;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.SeatingArrangment
{
    [Route("api/[controller]")]
    public class SA_ReportController : Controller
    {
        SA_ReportDelegate _delg = new SA_ReportDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetExamDateloaddata/{id:int}")]
        public SA_ReportDTO GetExamDateloaddata(int id)
        {
            SA_ReportDTO data = new SA_ReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.GetExamDateloaddata(data);
        }
        [Route("OnChangeyear")]
        public SA_ReportDTO OnChangeyear([FromBody]  SA_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.OnChangeyear(data);
        }

        [Route("GetAbsentStudentReport")]
        public SA_ReportDTO GetAbsentStudentReport([FromBody]  SA_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.GetAbsentStudentReport(data);
        }

        [Route("GetMalpracticeStudentReport")]
        public SA_ReportDTO GetMalpracticeStudentReport([FromBody]  SA_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.GetMalpracticeStudentReport(data);
        }
    }
}
