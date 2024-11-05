

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using corewebapi18072016.Delegates.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ExamImportController : Controller
    {

        ExamImportDelegate exammasterdelStr = new ExamImportDelegate();

        [HttpGet]
        [Route("Getdetails")]
        public ExamMarksDTO Getdetails(ExamMarksDTO data)
        {
            data.Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterdelStr.getdetails(data);
        }

        [HttpPost]

        [Route("onselectAcdYear/")]
        public ExamMarksDTO onselectAcdYear([FromBody] ExamMarksDTO dto)
        {
            dto.Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterdelStr.onselectAcdYear(dto);
        }

        [Route("onselectclass/")]
        public ExamMarksDTO onselectclass([FromBody] ExamMarksDTO dto)
        {
            dto.Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterdelStr.onselectclass(dto);
        }

        [Route("onselectSection/")]
        public ExamMarksDTO onselectSection([FromBody] ExamMarksDTO dto)
        {
            dto.Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterdelStr.onselectSection(dto);
        }

        [Route("onselectExam/")]
        public ExamMarksDTO onselectExam([FromBody] ExamMarksDTO dto)
        {
            dto.Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterdelStr.onselectExam(dto);
        }

        [Route("onsearch/")]
        public ExamMarksDTO onsearch([FromBody] ExamMarksDTO dto)
        {
            dto.Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterdelStr.onsearch(dto);
        }


        [Route("ImportMarks/")]

        public ExamImportStudentDTO ImportMarks([FromBody] ExamImportStudentDTO data)
        {

            data.IP4 = Request.HttpContext.Connection.RemoteIpAddress.ToString();
           // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterdelStr.ImportMarks(data);
        }

    }
}
