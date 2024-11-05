
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
    public class StudentMappingController : Controller
    {

        StudentMappingDelegates mStudentMappingdelStr = new StudentMappingDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails")]
        public StudentMappingDTO Getdetails(StudentMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            StudentMappingDTO tdata= mStudentMappingdelStr.Getdetails(data);
            tdata.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return tdata;
        }

        [Route("Studentdetails")]
        public StudentMappingDTO Studentdetails([FromBody]StudentMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mStudentMappingdelStr.Studentdetails(data);
        }

        [Route("getcategory/{id:int}")]
        public StudentMappingDTO getcategory(int id)
        {
            StudentMappingDTO data = new StudentMappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = id; 
            return mStudentMappingdelStr.getcategory(data);
        }

        [Route("getclassid")]
        public StudentMappingDTO getclassid([FromBody] StudentMappingDTO data)
        {            
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));            
            return mStudentMappingdelStr.getclassid(data);
        }

        [Route("getsubject/{id:int}")]
        public StudentMappingDTO getsubject(int id)
        {
            StudentMappingDTO data = new StudentMappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.EMG_Id = id;
            return mStudentMappingdelStr.getsubject(data);
        }

        [Route("savedetails")]
        public StudentMappingDTO savedetails([FromBody] StudentMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return mStudentMappingdelStr.savedetails(data);
        }

        [Route("deactivate")]
        public StudentMappingDTO deactivate([FromBody] StudentMappingDTO data)
        {            
            return mStudentMappingdelStr.deactivate(data);
        }
        
        [Route("editdetails/{id:int}")]
        public StudentMappingDTO editdetails(int ID)
        {
            return mStudentMappingdelStr.editdetails(ID);         
        }

        [Route("getalldetailsviewrecords/{id:int}")]
        public StudentMappingDTO getalldetailsviewrecords(int ID)
        {
            return mStudentMappingdelStr.getalldetailsviewrecords(ID);
        }

        [HttpPost]

        [Route("get_cls_sections")]
        public StudentMappingDTO get_cls_sections([FromBody] StudentMappingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mStudentMappingdelStr.get_cls_sections(categorypage);
        }

        [Route("OnClickRemove")]
        public StudentMappingDTO OnClickRemove([FromBody] StudentMappingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mStudentMappingdelStr.OnClickRemove(categorypage);
        }


        //Student Wise Question Paper Type Mapping
        [Route("BindData_PT/{id:int}")]
        public StudentMappingDTO BindData_PT(int id)
        {
            StudentMappingDTO data = new StudentMappingDTO
            {
                MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id")),                
                ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id")),
                UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId")),
            };
            return mStudentMappingdelStr.BindData_PT(data);
        }

        [Route("OnChangeYear_GetClass_PT")]
        public StudentMappingDTO OnChangeYear_GetClass_PT([FromBody] StudentMappingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return mStudentMappingdelStr.OnChangeYear_GetClass_PT(categorypage);
        }

        [Route("OnChangeClass_GetSection_PT")]
        public StudentMappingDTO OnChangeClass_GetSection_PT([FromBody] StudentMappingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return mStudentMappingdelStr.OnChangeClass_GetSection_PT(categorypage);
        }

        [Route("OnChangeSection_GetExam_PT")]
        public StudentMappingDTO OnChangeSection_GetExam_PT([FromBody] StudentMappingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return mStudentMappingdelStr.OnChangeSection_GetExam_PT(categorypage);
        }

        [Route("OnChangeExam_GetSubject_PT")]
        public StudentMappingDTO OnChangeExam_GetSubject_PT([FromBody] StudentMappingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return mStudentMappingdelStr.OnChangeExam_GetSubject_PT(categorypage);
        }

        [Route("OnSearch_PT")]
        public StudentMappingDTO OnSearch_PT([FromBody] StudentMappingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mStudentMappingdelStr.OnSearch_PT(categorypage);
        }

        [Route("OnSave_PT")]
        public StudentMappingDTO OnSave_PT([FromBody] StudentMappingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return mStudentMappingdelStr.OnSave_PT(categorypage);
        }

        [Route("OnClickRemove_PT")]
        public StudentMappingDTO OnClickRemove_PT([FromBody] StudentMappingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return mStudentMappingdelStr.OnClickRemove_PT(categorypage);
        }
    }
}
