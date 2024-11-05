using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class MarksEntryHHSController : Controller
    {         
         MarksEntryHHSDelegates del_ssse =new MarksEntryHHSDelegates();       

        [HttpGet]       
        [Route("Getdetails")]
        public MarksEntryHHSDTO Getdetails(MarksEntryHHSDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId")); 
            return del_ssse.Getdetails(data);
        }

        [HttpPost]
        [Route("get_classes")]
        public MarksEntryHHSDTO onselectAcdYear([FromBody] MarksEntryHHSDTO dto)
        {
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));   
            dto.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId")); 
            return del_ssse.get_classes(dto);
        }

        [Route("get_sections")]
        public MarksEntryHHSDTO get_sections([FromBody] MarksEntryHHSDTO dto)
        {
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return del_ssse.get_sections(dto);
        }

        [Route("get_exams")]
        public MarksEntryHHSDTO get_exams([FromBody] MarksEntryHHSDTO dto)
        {
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return del_ssse.get_exams(dto);
        }

        [Route("get_subjects")]
        public MarksEntryHHSDTO get_subjects([FromBody] MarksEntryHHSDTO dto)
        {
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return del_ssse.get_subjects(dto);
        }

        [Route("onsearch")]
        public MarksEntryHHSDTO onsearch([FromBody] MarksEntryHHSDTO dto)
        {
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return del_ssse.onsearch(dto);
        }

        [Route("SaveMarks")]
        public MarksEntryHHSDTO SaveMarks([FromBody] MarksEntryHHSDTO dto)
        {
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            dto.IP4 = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            return del_ssse.SaveMarks(dto);
        }
    }
}