
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
    public class ExamSubjectMappingController : Controller
    {
        ExamSubjectMappingDelegates ExamSubjectMappingdelStr = new ExamSubjectMappingDelegates();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails")]
        public ExamSubjectMappingDTO Getdetails(ExamSubjectMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return ExamSubjectMappingdelStr.Getdetails(data);
        }

        [Route("editdetails/{id:int}")]
        public ExamSubjectMappingDTO editdetails(int id)
        {
            return ExamSubjectMappingdelStr.editdetails(id);
        }

        [Route("savedetails")]
        public ExamSubjectMappingDTO savedetails([FromBody] ExamSubjectMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return ExamSubjectMappingdelStr.savedetails(data);
        }

        [Route("get_category")]
        public ExamSubjectMappingDTO get_category([FromBody] ExamSubjectMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return ExamSubjectMappingdelStr.get_category(data);
        }

        [Route("get_subjects")]
        public ExamSubjectMappingDTO get_subjects([FromBody] ExamSubjectMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return ExamSubjectMappingdelStr.get_subjects(data);
        }

        [Route("deactivate")]
        public ExamSubjectMappingDTO deactivate([FromBody] ExamSubjectMappingDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.deactivate(data);
        }

        [Route("deactivate_sub")]
        public ExamSubjectMappingDTO deactivate_sub([FromBody] ExamSubjectMappingDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.deactivate_sub(data);
        }

        [Route("deactive_sub_exm")]
        public ExamSubjectMappingDTO deactive_sub_exm([FromBody] ExamSubjectMappingDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.deactive_sub_exm(data);
        }

        [Route("deactive_sub_subj")]
        public ExamSubjectMappingDTO deactive_sub_subj([FromBody] ExamSubjectMappingDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.deactive_sub_subj(data);
        }

        [Route("deactive_sub_subj_subexam")]
        public ExamSubjectMappingDTO deactive_sub_subj_subexam([FromBody] ExamSubjectMappingDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.deactive_sub_subj_subexam(data);
        }
        
        [Route("getalldetailsviewrecords/{id:int}")]
        public ExamSubjectMappingDTO getalldetailsviewrecords(int id)
        {
            return ExamSubjectMappingdelStr.getalldetailsviewrecords(id);
        }

        [Route("getalldetailsviewrecords_subexms/{id:int}")]
        public ExamSubjectMappingDTO getalldetailsviewrecords_subexms(int id)
        {
            return ExamSubjectMappingdelStr.getalldetailsviewrecords_subexms(id);
        }

        [Route("getalldetailsviewrecords_subsubjs/{id:int}")]
        public ExamSubjectMappingDTO getalldetailsviewrecords_subsubjs(int id)
        {
            return ExamSubjectMappingdelStr.getalldetailsviewrecords_subsubjs(id);
        }

        [Route("getalldetailsviewrecords_subsubjssunexam/{id:int}")]
        public ExamSubjectMappingDTO getalldetailsviewrecords_subsubjssunexam(int id)
        {
            return ExamSubjectMappingdelStr.getalldetailsviewrecords_subsubjssunexam(id);
        }

        [Route("SetSubjectOrder")]
        public ExamSubjectMappingDTO SetSubjectOrder([FromBody] ExamSubjectMappingDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.SetSubjectOrder(data);
        }

        [Route("SaveSubjectOrder")]
        public ExamSubjectMappingDTO SaveSubjectOrder([FromBody] ExamSubjectMappingDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.SaveSubjectOrder(data);
        }  

        [Route("deactive_subj_GradeList")]
        public ExamSubjectMappingDTO deactive_subj_GradeList([FromBody] ExamSubjectMappingDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.deactive_subj_GradeList(data);
        }        
    }
}