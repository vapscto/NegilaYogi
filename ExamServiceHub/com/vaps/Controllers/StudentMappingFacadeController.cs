
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]

    public class StudentMappingFacadeController : Controller
    {
        public StudentMappingInterface _studentmapping;
        public StudentMappingFacadeController(StudentMappingInterface studentmapping)
        {
            _studentmapping = studentmapping;
        }

        [Route("Getdetails")]
        public StudentMappingDTO Getdetails([FromBody]StudentMappingDTO data)
        {
            return _studentmapping.Getdetails(data);
        }

        [Route("Studentdetails")]
        public StudentMappingDTO Studentdetails([FromBody]StudentMappingDTO data)
        {
            return _studentmapping.Studentdetails(data);
        }

        [Route("getcategory")]
        public StudentMappingDTO getcategory([FromBody] StudentMappingDTO data)
        {
            return _studentmapping.getcategory(data);

        }

        [Route("getclassid")]
        public StudentMappingDTO getclassid([FromBody] StudentMappingDTO data)
        {
            return _studentmapping.getclassid(data);

        }

        [Route("getsubject")]
        public StudentMappingDTO getsubject([FromBody]StudentMappingDTO data)
        {
            return _studentmapping.getsubject(data);

        }

        [Route("editdetails/{id:int}")]
        public StudentMappingDTO editdetails(int ID)
        {
            return _studentmapping.editdetails(ID);
        }

        [Route("getalldetailsviewrecords/{id:int}")]
        public StudentMappingDTO getalldetailsviewrecords(int ID)
        {
            return _studentmapping.getalldetailsviewrecords(ID);
        }

        [Route("savedetails")]
        public StudentMappingDTO savedetails([FromBody] StudentMappingDTO data)
        {
            return _studentmapping.savedetails(data);
        }

        [Route("deactivate")]
        public StudentMappingDTO deactivate([FromBody] StudentMappingDTO data)
        {
            return _studentmapping.deactivate(data);
        }

        [HttpPost]
        [Route("get_cls_sections")]
        public StudentMappingDTO get_cls_sections([FromBody] StudentMappingDTO org)
        {
            // id = 12;
            return _studentmapping.get_cls_sections(org);
        }

        [Route("OnClickRemove")]
        public StudentMappingDTO OnClickRemove([FromBody] StudentMappingDTO org)
        {
            // id = 12;
            return _studentmapping.OnClickRemove(org);
        }

        //Student Wise Question Paper Type Mapping
        [Route("BindData_PT")]
        public StudentMappingDTO BindData_PT([FromBody] StudentMappingDTO data)
        {
            return _studentmapping.BindData_PT(data);
        }

        [Route("OnChangeYear_GetClass_PT")]
        public StudentMappingDTO OnChangeYear_GetClass_PT([FromBody] StudentMappingDTO categorypage)
        {           
            return _studentmapping.OnChangeYear_GetClass_PT(categorypage);
        }

        [Route("OnChangeClass_GetSection_PT")]
        public StudentMappingDTO OnChangeClass_GetSection_PT([FromBody] StudentMappingDTO categorypage)
        {
            return _studentmapping.OnChangeClass_GetSection_PT(categorypage);
        }

        [Route("OnChangeSection_GetExam_PT")]
        public StudentMappingDTO OnChangeSection_GetExam_PT([FromBody] StudentMappingDTO categorypage)
        {            
            return _studentmapping.OnChangeSection_GetExam_PT(categorypage);
        }

        [Route("OnChangeExam_GetSubject_PT")]
        public StudentMappingDTO OnChangeExam_GetSubject_PT([FromBody] StudentMappingDTO categorypage)
        {
            return _studentmapping.OnChangeExam_GetSubject_PT(categorypage);
        }

        [Route("OnSearch_PT")]
        public StudentMappingDTO OnSearch_PT([FromBody] StudentMappingDTO categorypage)
        {            
            return _studentmapping.OnSearch_PT(categorypage);
        }

        [Route("OnSave_PT")]
        public StudentMappingDTO OnSave_PT([FromBody] StudentMappingDTO categorypage)
        {            
            return _studentmapping.OnSave_PT(categorypage);
        }

        [Route("OnClickRemove_PT")]
        public StudentMappingDTO OnClickRemove_PT([FromBody] StudentMappingDTO categorypage)
        {            
            return _studentmapping.OnClickRemove_PT(categorypage);
        }
    }
}