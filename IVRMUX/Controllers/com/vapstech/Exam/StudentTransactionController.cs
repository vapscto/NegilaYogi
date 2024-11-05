using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class StudentTransactionController : Controller
    {
        StudentTransactionDelegate delegates = new StudentTransactionDelegate();
        [Route("Onload/{id:int}")]
        public StudentTransactionDTO Onload(int id)
        {
            StudentTransactionDTO dto = new StudentTransactionDTO();
            dto.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            dto.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            dto.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));           
            return delegates.getDetails(dto);
        }

        [Route("onchangeyear")]
        public StudentTransactionDTO onchangeyear([FromBody] StudentTransactionDTO dto)
        {
            dto.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            dto.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            dto.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.onchangeyear(dto);
        }
        [Route("onchangeclass")]
        public StudentTransactionDTO onchangeclass([FromBody] StudentTransactionDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.onchangeclass(data);
        }

        [Route("onchangesection")]
        public StudentTransactionDTO onchangesection([FromBody] StudentTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.onchangesection(data);
        }

        [Route("onchangeskills")]
        public StudentTransactionDTO onchangeskills([FromBody] StudentTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.onchangeskills(data);
        }

        [Route("onchangeactivites")]
        public StudentTransactionDTO onchangeactivites([FromBody] StudentTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.onchangeactivites(data);
        }

        [Route("getStudentList")]
        public StudentTransactionDTO getStudentList([FromBody] StudentTransactionDTO dto)
        {
            dto.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            dto.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            dto.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.getStudentList(dto);
        }
        [Route("saveRecord")]
        public StudentTransactionDTO saveRecord([FromBody] StudentTransactionDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.save(data);
        }

        [Route("onchangeactivitesskillflag")]
        public StudentTransactionDTO onchangeactivitesskillflag([FromBody] StudentTransactionDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.onchangeactivitesskillflag(data);
        }

    }
}
