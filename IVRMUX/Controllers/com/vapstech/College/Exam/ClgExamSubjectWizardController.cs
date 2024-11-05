using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Exam
{
    [Route("api/[controller]")]
    public class ClgExamSubjectWizardController : Controller
    {
        ClgExamSubjectWizardDelegates ExamSubjectMappingdelStr = new ClgExamSubjectWizardDelegates();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public ClgSubjectWizardDTO Getdetails(ClgSubjectWizardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.Getdetails(data);
        }

        [Route("getbranch")]
        public ClgSubjectWizardDTO getbranch([FromBody]ClgSubjectWizardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.getbranch(data);
        }

        [Route("getsemester")]
        public ClgSubjectWizardDTO getsemester([FromBody]ClgSubjectWizardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.getsemester(data);
        }

        [Route("getsubjectscheme")]
        public ClgSubjectWizardDTO getsubjectscheme([FromBody]ClgSubjectWizardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.getsubjectscheme(data);
        }

        [Route("getsubjectschemetype")]
        public ClgSubjectWizardDTO getsubjectschemetype([FromBody]ClgSubjectWizardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.getsubjectschemetype(data);
        }

        [Route("getsubjectgroup")]
        public ClgSubjectWizardDTO getsubjectgroup([FromBody]ClgSubjectWizardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.getsubjectgroup(data);
        }

        [Route("savedetails")]
        public ClgSubjectWizardDTO savedetails([FromBody] ClgSubjectWizardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.savedetails(data);
        }

        [Route("getalldetailsviewrecords/{id:int}")]
        public ClgSubjectWizardDTO getalldetailsviewrecords(int id)
        {

            return ExamSubjectMappingdelStr.getalldetailsviewrecords(id);
        }

        [Route("getalldetailsviewrecords_subexms/{id:int}")]
        public ClgSubjectWizardDTO getalldetailsviewrecords_subexms(int id)
        {

            return ExamSubjectMappingdelStr.getalldetailsviewrecords_subexms(id);
        }

        [Route("getalldetailsviewrecords_subsubjs/{id:int}")]
        public ClgSubjectWizardDTO getalldetailsviewrecords_subsubjs(int id)
        {

            return ExamSubjectMappingdelStr.getalldetailsviewrecords_subsubjs(id);
        }

        [Route("deactivate_sub")]
        public ClgSubjectWizardDTO deactivate_sub([FromBody] ClgSubjectWizardDTO data)
        {
            return ExamSubjectMappingdelStr.deactivate_sub(data);
        }

        [Route("deactive_sub_exm")]
        public ClgSubjectWizardDTO deactive_sub_exm([FromBody] ClgSubjectWizardDTO data)
        {
            return ExamSubjectMappingdelStr.deactive_sub_exm(data);
        }

        [Route("deactive_sub_subj")]
        public ClgSubjectWizardDTO deactive_sub_subj([FromBody] ClgSubjectWizardDTO data)
        {
            return ExamSubjectMappingdelStr.deactive_sub_subj(data);
        }

        [Route("editdetails/{id:int}")]
        public ClgSubjectWizardDTO editdetails(int id)
        {
            return ExamSubjectMappingdelStr.editdetails(id);
        }

        [Route("deactivate")]
        public ClgSubjectWizardDTO deactivate([FromBody] ClgSubjectWizardDTO data)
        {
            return ExamSubjectMappingdelStr.deactivate(data);
        }

        [Route("getalldetailsviewrecords_subsubjssunexam")]
        public ClgSubjectWizardDTO getalldetailsviewrecords_subsubjssunexam([FromBody] ClgSubjectWizardDTO data)
        {
            return ExamSubjectMappingdelStr.getalldetailsviewrecords_subsubjssunexam(data);
        }

        [Route("deactive_sub_subj_subexam")]
        public ClgSubjectWizardDTO deactive_sub_subj_subexam([FromBody] ClgSubjectWizardDTO data)
        {
            return ExamSubjectMappingdelStr.deactive_sub_subj_subexam(data);
        }

        [Route("get_subjects")]
        public ClgSubjectWizardDTO get_subjects([FromBody] ClgSubjectWizardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.get_subjects(data);
        }

        [Route("SetOrder_SubSubject")]
        public ClgSubjectWizardDTO SetOrder_SubSubject([FromBody] ClgSubjectWizardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.SetOrder_SubSubject(data);
        }
        
    }
}