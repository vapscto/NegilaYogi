using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeExamServiceHub.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeExamServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class ClgExamSubjectWizardFacadeController : Controller
    {
        public ClgExamSubjectWizardInterface _ExamSubjectMapping;

        public ClgExamSubjectWizardFacadeController(ClgExamSubjectWizardInterface ExamSubjectMapping)
        {
            _ExamSubjectMapping = ExamSubjectMapping;
        }


        [Route("Getdetails")]
        public ClgSubjectWizardDTO Getdetails([FromBody]ClgSubjectWizardDTO data)//int IVRMM_Id
        {
            return _ExamSubjectMapping.Getdetails(data);
        }

        [Route("getbranch")]
        public ClgSubjectWizardDTO getbranch([FromBody]ClgSubjectWizardDTO data)
        {
            return _ExamSubjectMapping.getbranch(data);
        }

        [Route("getsemester")]
        public ClgSubjectWizardDTO getsemester([FromBody]ClgSubjectWizardDTO data)
        {
            return _ExamSubjectMapping.getsemester(data);
        }

        [Route("getsubjectscheme")]
        public ClgSubjectWizardDTO getsubjectscheme([FromBody]ClgSubjectWizardDTO data)
        {
            return _ExamSubjectMapping.getsubjectscheme(data);
        }

        [Route("getsubjectschemetype")]
        public ClgSubjectWizardDTO getsubjectschemetype([FromBody]ClgSubjectWizardDTO data)
        {
            return _ExamSubjectMapping.getsubjectschemetype(data);
        }

        [Route("getsubjectgroup")]
        public ClgSubjectWizardDTO getsubjectgroup([FromBody]ClgSubjectWizardDTO data)
        {
            return _ExamSubjectMapping.getsubjectgroup(data);
        }

        [Route("savedetails")]
        public ClgSubjectWizardDTO savedetails([FromBody] ClgSubjectWizardDTO data)
        {
            return _ExamSubjectMapping.savedetails(data);
        }

        [Route("getalldetailsviewrecords/{id:int}")]        
        public ClgSubjectWizardDTO getalldetailsviewrecords(int id)
        {
            return _ExamSubjectMapping.getalldetailsviewrecords(id);
        }

        [Route("getalldetailsviewrecords_subexms/{id:int}")]
        public ClgSubjectWizardDTO getalldetailsviewrecords_subexms(int id)
        {
            return _ExamSubjectMapping.getalldetailsviewrecords_subexms(id);
        }

        [Route("getalldetailsviewrecords_subsubjs/{id:int}")]
        public ClgSubjectWizardDTO getalldetailsviewrecords_subsubjs(int id)
        {
            return _ExamSubjectMapping.getalldetailsviewrecords_subsubjs(id);
        }

        [Route("deactivate_sub")]
        public ClgSubjectWizardDTO deactivate_sub([FromBody] ClgSubjectWizardDTO data)
        {
            return _ExamSubjectMapping.deactivate_sub(data);
        }

        [Route("deactive_sub_exm")]
        public ClgSubjectWizardDTO deactive_sub_exm([FromBody] ClgSubjectWizardDTO data)
        {
            return _ExamSubjectMapping.deactive_sub_exm(data);
        }

        [Route("deactive_sub_subj")]
        public ClgSubjectWizardDTO deactive_sub_subj([FromBody] ClgSubjectWizardDTO data)
        {
            return _ExamSubjectMapping.deactive_sub_subj(data);
        }

        [Route("editdetails/{id:int}")]
        public ClgSubjectWizardDTO editdetails(int id)
        {
            return _ExamSubjectMapping.editdetails(id);
        }

        [Route("deactivate")]
        public ClgSubjectWizardDTO deactivate([FromBody] ClgSubjectWizardDTO data)
        {
            return _ExamSubjectMapping.deactivate(data);
        }

        [Route("getalldetailsviewrecords_subsubjssunexam")]
        public ClgSubjectWizardDTO getalldetailsviewrecords_subsubjssunexam([FromBody] ClgSubjectWizardDTO data)
        {
            return _ExamSubjectMapping.getalldetailsviewrecords_subsubjssunexam(data);
        }

        [Route("deactive_sub_subj_subexam")]
        public ClgSubjectWizardDTO deactive_sub_subj_subexam([FromBody] ClgSubjectWizardDTO data)
        {
            return _ExamSubjectMapping.deactive_sub_subj_subexam(data);
        }

        [Route("get_subjects")]
        public ClgSubjectWizardDTO get_subjects([FromBody] ClgSubjectWizardDTO data)
        {
            return _ExamSubjectMapping.get_subjects(data);
        }

        [Route("SetOrder_SubSubject")]
        public ClgSubjectWizardDTO SetOrder_SubSubject([FromBody] ClgSubjectWizardDTO data)
        {
            return _ExamSubjectMapping.SetOrder_SubSubject(data);
        }
    }
}
