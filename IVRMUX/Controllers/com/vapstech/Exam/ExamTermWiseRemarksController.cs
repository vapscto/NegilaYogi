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
    public class ExamTermWiseRemarksController : Controller
    {
        ExamTermWiseRemarksDelegate _delg = new ExamTermWiseRemarksDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("Getdetails/{id:int}")]
        public ExamTermWiseRemarksDTO Getdetails(int id)
        {
            ExamTermWiseRemarksDTO data = new ExamTermWiseRemarksDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.Getdetails(data);
        }

        [Route("get_class")]
        public ExamTermWiseRemarksDTO get_class([FromBody]ExamTermWiseRemarksDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.get_class(data);
        }

        [Route("get_section")]
        public ExamTermWiseRemarksDTO get_section([FromBody]ExamTermWiseRemarksDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.get_section(data);
        }

        [Route("get_term")]
        public ExamTermWiseRemarksDTO get_term([FromBody]ExamTermWiseRemarksDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.get_term(data);
        }

        [Route("search_student")]
        public ExamTermWiseRemarksDTO search_student([FromBody]ExamTermWiseRemarksDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.search_student(data);
        }

        [Route("save_details")]
        public ExamTermWiseRemarksDTO save_details([FromBody]ExamTermWiseRemarksDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.save_details(data);
        }

        [Route("edit_details")]
        public ExamTermWiseRemarksDTO edit_details([FromBody]ExamTermWiseRemarksDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.edit_details(data);
        }

        // Term Wise Participate
        [Route("Getdetails_Participate/{id:int}")]
        public ExamTermWiseRemarksDTO Getdetails_Participate(int id)
        {
            ExamTermWiseRemarksDTO data = new ExamTermWiseRemarksDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.Getdetails_Participate(data);
        }        

        [Route("search_student_participate")]
        public ExamTermWiseRemarksDTO search_student_participate([FromBody]ExamTermWiseRemarksDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.search_student_participate(data);
        }

        [Route("save_participate_details")]
        public ExamTermWiseRemarksDTO save_participate_details([FromBody]ExamTermWiseRemarksDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.save_participate_details(data);
        }

        [Route("ViewStudentParticipateDetails")]
        public ExamTermWiseRemarksDTO ViewStudentParticipateDetails([FromBody]ExamTermWiseRemarksDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.ViewStudentParticipateDetails(data);
        }
    }
}
