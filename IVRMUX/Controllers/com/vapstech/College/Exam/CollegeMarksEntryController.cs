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
    public class CollegeMarksEntryController : Controller
    {
        CollegeMarksEntryDelegate objdelegate = new CollegeMarksEntryDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("getalldetails")]
        public CollegeMarksEntryDTO Get([FromQuery] int id)
        {
            CollegeMarksEntryDTO obj = new CollegeMarksEntryDTO();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.username = Convert.ToString(HttpContext.Session.GetString("UserName"));           
            obj.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            obj.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            obj.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return objdelegate.getdetails(obj); ;
        }

        [Route("onchangeyear")]
        public CollegeMarksEntryDTO onchangeyear([FromBody] CollegeMarksEntryDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.username = Convert.ToString(HttpContext.Session.GetString("UserName"));            
            categorypage.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            categorypage.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return objdelegate.onchangeyear(categorypage);
        }

        [Route("onchangecourse")]
        public CollegeMarksEntryDTO onchangecourse([FromBody] CollegeMarksEntryDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.username = Convert.ToString(HttpContext.Session.GetString("UserName"));            
            categorypage.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            categorypage.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return objdelegate.onchangecourse(categorypage);
        }

        [Route("onchangebranch")]
        public CollegeMarksEntryDTO onchangebranch([FromBody] CollegeMarksEntryDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.username = Convert.ToString(HttpContext.Session.GetString("UserName"));            
            categorypage.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            categorypage.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return objdelegate.onchangebranch(categorypage);
        }

        [Route("get_exams")]
        public CollegeMarksEntryDTO get_exams([FromBody] CollegeMarksEntryDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.username = Convert.ToString(HttpContext.Session.GetString("UserName"));            
            categorypage.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            categorypage.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return objdelegate.get_exams(categorypage);
        }

        [Route("get_subjects")]
        public CollegeMarksEntryDTO get_subjects([FromBody] CollegeMarksEntryDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.username = Convert.ToString(HttpContext.Session.GetString("UserName"));            
            categorypage.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            categorypage.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return objdelegate.get_subjects(categorypage);
        }
        [Route("getsubjectscheme")]
        public CollegeMarksEntryDTO getsubjectscheme([FromBody] CollegeMarksEntryDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return objdelegate.getsubjectscheme(dto);
        }

        [Route("getsubjectschemetype")]
        public CollegeMarksEntryDTO getsubjectschemetype([FromBody] CollegeMarksEntryDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return objdelegate.getsubjectschemetype(dto);
        }



        [Route("onsearch/")]
        public CollegeMarksEntryDTO onsearch([FromBody] CollegeMarksEntryDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return objdelegate.onsearch(dto);
        }

        [Route("SaveMarks/")]
        public CollegeMarksEntryDTO SaveMarks([FromBody] CollegeMarksEntryDTO dto)
        {
            dto.IP4 = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            dto.Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));            
            dto.username = Convert.ToString(HttpContext.Session.GetString("UserName"));           
            dto.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return objdelegate.SaveMarks(dto);
        }
    }
}
