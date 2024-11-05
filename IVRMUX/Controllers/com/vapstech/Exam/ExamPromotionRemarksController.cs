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
    public class ExamPromotionRemarksController : Controller
    {
        ExamPromotionRemarksDelegate _delegate = new ExamPromotionRemarksDelegate();
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
        public ExamPromotionRemarksDTO Getdetails(int id)
        {
            ExamPromotionRemarksDTO data = new ExamPromotionRemarksDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delegate.Getdetails(data);
        }

        [Route("get_class")]
        public ExamPromotionRemarksDTO get_class([FromBody]ExamPromotionRemarksDTO data)
        {            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));            
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delegate.get_class(data);
        }
        [Route("get_section")]
        public ExamPromotionRemarksDTO get_section([FromBody]ExamPromotionRemarksDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));           
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delegate.get_section(data);
        }
        [Route("get_exam")]
        public ExamPromotionRemarksDTO get_exam([FromBody]ExamPromotionRemarksDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));            
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delegate.get_exam(data);
        }
        [Route("search_student")]
        public ExamPromotionRemarksDTO search_student([FromBody]ExamPromotionRemarksDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.search_student(data);
        }
        [Route("save_details")]
        public ExamPromotionRemarksDTO save_details([FromBody]ExamPromotionRemarksDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.save_details(data);
        }
        
        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
