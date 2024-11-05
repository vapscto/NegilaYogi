using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class ExamWiseRemarksReportController : Controller
    {
        ExamWiseRemarksReportDelegate _delg = new ExamWiseRemarksReportDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("LoadData/{id:int}")]
        public ExamWiseRemarksReportDTO LoadData(int id)
        {
            ExamWiseRemarksReportDTO data = new ExamWiseRemarksReportDTO();
            data.MI_Id =Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.LoadData(data);
        }

        [Route("OnChangeYear")]
        public ExamWiseRemarksReportDTO OnChangeYear([FromBody] ExamWiseRemarksReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeYear(data);
        }

        [Route("OnChangeClass")]
        public ExamWiseRemarksReportDTO OnChangeClass([FromBody] ExamWiseRemarksReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeClass(data);
        }

        [Route("OnChangeSection")]
        public ExamWiseRemarksReportDTO OnChangeSection([FromBody] ExamWiseRemarksReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeSection(data);
        }

        [Route("OnChangeExam")]
        public ExamWiseRemarksReportDTO OnChangeExam([FromBody] ExamWiseRemarksReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeExam(data);
        }

        [Route("GetExamWiseRemarksReport")]
        public ExamWiseRemarksReportDTO GetExamWiseRemarksReport([FromBody] ExamWiseRemarksReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.GetExamWiseRemarksReport(data);
        }

        [Route("GetExamSubjectWiseRemarks_PTReport")]
        public ExamWiseRemarksReportDTO GetExamSubjectWiseRemarks_PTReport([FromBody] ExamWiseRemarksReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.GetExamSubjectWiseRemarks_PTReport(data);
        }

        [Route("GetTermWiseRemarksReport")]
        public ExamWiseRemarksReportDTO GetTermWiseRemarksReport([FromBody] ExamWiseRemarksReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.GetTermWiseRemarksReport(data);
        }
    }
}
