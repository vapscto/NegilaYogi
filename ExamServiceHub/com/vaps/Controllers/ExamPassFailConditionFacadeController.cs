
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
    public class ExamPassFailConditionFacadeController : Controller
    {
        public ExamPassFailConditionInterface _ExamSubjectMapping;

        public ExamPassFailConditionFacadeController(ExamPassFailConditionInterface ExamSubjectMapping)
        {
            _ExamSubjectMapping = ExamSubjectMapping;
        }


        [Route("Getdetails")]
        public ExamPassFailConditionDTO Getdetails([FromBody]ExamPassFailConditionDTO data)//int IVRMM_Id
        {
            return _ExamSubjectMapping.Getdetails(data);
        }
        [Route("get_category")]
        public ExamPassFailConditionDTO get_category([FromBody] ExamPassFailConditionDTO data)
        {
            return _ExamSubjectMapping.get_category(data);
        }
        [Route("get_subjects")]
        public ExamPassFailConditionDTO get_subjects([FromBody] ExamPassFailConditionDTO data)
        {
            return _ExamSubjectMapping.get_subjects(data);
        }
        [Route("get_examcondition")]
        public ExamPassFailConditionDTO get_examcondition([FromBody] ExamPassFailConditionDTO data)
        {
            return _ExamSubjectMapping.get_examcondition(data);
        }
        [Route("get_condition")]
        public ExamPassFailConditionDTO get_condition([FromBody] ExamPassFailConditionDTO data)
        {
            return _ExamSubjectMapping.get_condition(data);
        }
        [Route("savedata")]
        public ExamPassFailConditionDTO savedata([FromBody] ExamPassFailConditionDTO data)
        {
            return _ExamSubjectMapping.savedata(data);
        }
        [Route("deactive")]
        public ExamPassFailConditionDTO deactive([FromBody] ExamPassFailConditionDTO data)
        {
            return _ExamSubjectMapping.deactive(data);

        }
        [Route("editdetails/{id:int}")]
        public ExamPassFailConditionDTO editdetails(int ID)
        {
            return _ExamSubjectMapping.editdetails(ID);
        }

    }
}
