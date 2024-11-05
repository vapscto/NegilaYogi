
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
//using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ExamSubjectMappingFacadeController : Controller
    {
        public ExamSubjectMappingInterface _ExamSubjectMapping;

        public ExamSubjectMappingFacadeController(ExamSubjectMappingInterface ExamSubjectMapping)
        {
            _ExamSubjectMapping = ExamSubjectMapping;
        }

        [Route("Getdetails")]
        public ExamSubjectMappingDTO Getdetails([FromBody]ExamSubjectMappingDTO data)//int IVRMM_Id
        {           
            return _ExamSubjectMapping.Getdetails(data);           
        }

        [Route("editdetails/{id:int}")]
        public ExamSubjectMappingDTO editdetails(int id)
        {
            return _ExamSubjectMapping.editdetails(id);
        }

        [Route("savedetails")]
        public ExamSubjectMappingDTO savedetails([FromBody] ExamSubjectMappingDTO data)
        {
            return _ExamSubjectMapping.savedetails(data);
        }

        [Route("get_category")]
        public ExamSubjectMappingDTO get_category([FromBody] ExamSubjectMappingDTO data)
        {
            return _ExamSubjectMapping.get_category(data);
        }

        [Route("get_subjects")]
        public ExamSubjectMappingDTO get_subjects([FromBody] ExamSubjectMappingDTO data)
        {
            return _ExamSubjectMapping.get_subjects(data);
        }

        [Route("deactivate")]
        public ExamSubjectMappingDTO deactivate([FromBody] ExamSubjectMappingDTO data)
        {           
            return _ExamSubjectMapping.deactivate(data);
        }

        [Route("deactivate_sub")]
        public ExamSubjectMappingDTO deactivate_sub([FromBody] ExamSubjectMappingDTO data)
        {
            return _ExamSubjectMapping.deactivate_sub(data);
        }

        [Route("deactive_sub_exm")]
        public ExamSubjectMappingDTO deactive_sub_exm([FromBody] ExamSubjectMappingDTO data)
        {
            return _ExamSubjectMapping.deactive_sub_exm(data);
        }

        [Route("deactive_sub_subj")]
        public ExamSubjectMappingDTO deactive_sub_subj([FromBody] ExamSubjectMappingDTO data)
        {
            return _ExamSubjectMapping.deactive_sub_subj(data);
        }

        [Route("deactive_sub_subj_subexam")]
        public ExamSubjectMappingDTO deactive_sub_subj_subexam([FromBody] ExamSubjectMappingDTO data)
        {
            return _ExamSubjectMapping.deactive_sub_subj_subexam(data);
        }
        
        [Route("getalldetailsviewrecords/{id:int}")]       
        public ExamSubjectMappingDTO getalldetailsviewrecords(int id)
        {
            return _ExamSubjectMapping.getalldetailsviewrecords(id);
        }

        [Route("getalldetailsviewrecords_subexms/{id:int}")]       
        public ExamSubjectMappingDTO getalldetailsviewrecords_subexms(int id)
        {
            return _ExamSubjectMapping.getalldetailsviewrecords_subexms(id);
        }

        [Route("getalldetailsviewrecords_subsubjs/{id:int}")]     
        public ExamSubjectMappingDTO getalldetailsviewrecords_subsubjs(int id)
        {
            return _ExamSubjectMapping.getalldetailsviewrecords_subsubjs(id);
        }

        [Route("getalldetailsviewrecords_subsubjssunexam/{id:int}")]       
        public ExamSubjectMappingDTO getalldetailsviewrecords_subsubjssunexam(int id)
        {            
            return _ExamSubjectMapping.getalldetailsviewrecords_subsubjssunexam(id);
        }

        [Route("SetSubjectOrder")]
        public ExamSubjectMappingDTO SetSubjectOrder([FromBody] ExamSubjectMappingDTO data)
        {
            return _ExamSubjectMapping.SetSubjectOrder(data);
        }

        [Route("SaveSubjectOrder")]
        public ExamSubjectMappingDTO SaveSubjectOrder([FromBody] ExamSubjectMappingDTO data)
        {
            return _ExamSubjectMapping.SaveSubjectOrder(data);
        }

        [Route("deactive_subj_GradeList")]
        public ExamSubjectMappingDTO deactive_subj_GradeList([FromBody] ExamSubjectMappingDTO data)
        {
            return _ExamSubjectMapping.deactive_subj_GradeList(data);
        }
    }
}
