
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
    public class ExamPassFailConditionController : Controller
    {

        ExamPassFailConditionDelegates ExamSubjectMappingdelStr = new ExamPassFailConditionDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public ExamPassFailConditionDTO Getdetails(ExamPassFailConditionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return ExamSubjectMappingdelStr.Getdetails(data);            
        }
       
        [Route("get_category")]
        public ExamPassFailConditionDTO get_category([FromBody] ExamPassFailConditionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.get_category(data);
        }

        [Route("get_subjects")]
        public ExamPassFailConditionDTO get_subjects([FromBody] ExamPassFailConditionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.get_subjects(data);
        }

        [Route("get_examcondition")]
        public ExamPassFailConditionDTO get_examcondition([FromBody] ExamPassFailConditionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.get_examcondition(data);
        }
        [Route("get_condition")]
        public ExamPassFailConditionDTO get_condition([FromBody] ExamPassFailConditionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.get_condition(data);
        }
        [Route("deactive")]
        public ExamPassFailConditionDTO deactive([FromBody] ExamPassFailConditionDTO data)
        {
            return ExamSubjectMappingdelStr.deactive(data);
        }
        [Route("editdetails/{id:int}")]
        public ExamPassFailConditionDTO editdetails(int ID)
        {
            return ExamSubjectMappingdelStr.editdetails(ID);
        }

        [Route("savedata")]
        public ExamPassFailConditionDTO savedata([FromBody] ExamPassFailConditionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamSubjectMappingdelStr.savedata(data);
        }

    }

}
