
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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ExamCalculation_SSSEController : Controller
    {
        ExamCalculation_SSSEDelegates crStr = new ExamCalculation_SSSEDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public ExamCalculation_SSSEDTO Getdetails(ExamCalculation_SSSEDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));            
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return crStr.Getdetails(data);            
        }
        [HttpPost]
        [Route("get_cls_sections")]
        public ExamCalculation_SSSEDTO get_cls_sections([FromBody] ExamCalculation_SSSEDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            categorypage.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            categorypage.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return crStr.get_cls_sections(categorypage);

        }

        [Route("Calculation")]
        public ExamCalculation_SSSEDTO Calculation([FromBody] ExamCalculation_SSSEDTO categorypage)
        {
            categorypage.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Calculation(categorypage);

        }

        [Route("get_classes")]
        public ExamCalculation_SSSEDTO onselectAcdYear([FromBody] ExamCalculation_SSSEDTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            dto.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return crStr.get_classes(dto);
        }

        [Route("get_exams")]
        public ExamCalculation_SSSEDTO get_exams([FromBody] ExamCalculation_SSSEDTO dto)
        {
            dto.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));           
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.get_exams(dto);
        }

        [Route("onchangeexam")]
        public ExamCalculation_SSSEDTO onchangeexam([FromBody] ExamCalculation_SSSEDTO dto)
        {
            dto.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));           
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.onchangeexam(dto);
        }
       
        [Route("saveapporvecal")]
        public ExamCalculation_SSSEDTO saveapporvecal([FromBody] ExamCalculation_SSSEDTO dto)
        {
            dto.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.saveapporvecal(dto);
        }

        // Student Wise Publish

        [Route("ChangeOfSection")]
        public ExamCalculation_SSSEDTO ChangeOfSection([FromBody] ExamCalculation_SSSEDTO dto)
        {
            dto.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.ChangeOfSection(dto);
        }

        [Route("CheckMarksCalculated")]
        public ExamCalculation_SSSEDTO CheckMarksCalculated([FromBody] ExamCalculation_SSSEDTO dto)
        {
            dto.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.CheckMarksCalculated(dto);
        }

        [Route("SearchStudent")]
        public ExamCalculation_SSSEDTO SearchStudent([FromBody] ExamCalculation_SSSEDTO dto)
        {
            dto.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.SearchStudent(dto);
        }

        [Route("SaveStudentStatus")]
        public ExamCalculation_SSSEDTO SaveStudentStatus([FromBody] ExamCalculation_SSSEDTO dto)
        {
            dto.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.SaveStudentStatus(dto);
        }

        // Promotion
        [Route("onchangesection")]
        public ExamCalculation_SSSEDTO onchangesection([FromBody] ExamCalculation_SSSEDTO dto)
        {
            dto.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.onchangesection(dto);
        }

        [Route("promotionsaveddata")]
        public ExamCalculation_SSSEDTO promotionsaveddata([FromBody] ExamCalculation_SSSEDTO dto)
        {
            dto.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.promotionsaveddata(dto);
        }

        [Route("publishtostudentportal")]
        public ExamCalculation_SSSEDTO publishtostudentportal([FromBody] ExamCalculation_SSSEDTO dto)
        {
            dto.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.publishtostudentportal(dto);
        }
    }
}
