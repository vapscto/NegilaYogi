
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using Microsoft.AspNetCore.HttpOverrides;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]

    [Route("api/[controller]")]
    public class ExamEntryController : Controller
    {
        MarksEntryDelegates exammasterdelStr = new MarksEntryDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails")]
        public ExamMarksDTO Getdetails(ExamMarksDTO data)
        {
            data.Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));           
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return exammasterdelStr.getdetails(data);            
        }

        [HttpPost]

        [Route("onselectAcdYear/")]
        public ExamMarksDTO onselectAcdYear ([FromBody] ExamMarksDTO dto)
        {
            dto.Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));            
            dto.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return exammasterdelStr.onselectAcdYear(dto);
        }

        [Route("onselectclass/")]
        public ExamMarksDTO onselectclass([FromBody] ExamMarksDTO dto)
        {
            dto.Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return exammasterdelStr.onselectclass(dto);
        }

        [Route("onselectSection/")]
        public ExamMarksDTO onselectSection([FromBody] ExamMarksDTO dto)
        {
            dto.Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return exammasterdelStr.onselectSection(dto);
        }

        [Route("onselectExam/")]
        public ExamMarksDTO onselectExam([FromBody] ExamMarksDTO dto)
        {
            dto.Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return exammasterdelStr.onselectExam(dto);
        }

        [Route("onselectSubject/")]
        public ExamMarksDTO onselectSubject([FromBody] ExamMarksDTO dto)
        {
            dto.Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return exammasterdelStr.onselectSubject(dto);
        }

        [Route("onchangesubsubject/")]
        public ExamMarksDTO onchangesubsubject([FromBody] ExamMarksDTO dto)
        {
            dto.Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return exammasterdelStr.onchangesubsubject(dto);
        }

        [Route("onsearch/")]
        public ExamMarksDTO onsearch([FromBody] ExamMarksDTO dto)
        {
            dto.Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return exammasterdelStr.onsearch(dto);
        }

        [Route("SaveMarks/")]
        public ExamMarksDTO SaveMarks([FromBody] ExamMarksDTO dto)
        {
            dto.IP4 = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            dto.Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return exammasterdelStr.SaveMarks(dto);
        }

        [Route("DeleteMarks")]
        public ExamMarksDTO DeleteMarks([FromBody] ExamMarksDTO dto)
        {
            dto.IP4 = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            dto.Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return exammasterdelStr.DeleteMarks(dto);
        }
    }
}
