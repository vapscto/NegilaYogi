using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ExamTermWiseRemarksFacadeController : Controller
    {
        public ExamTermWiseRemarksInterface _interface;

        public ExamTermWiseRemarksFacadeController(ExamTermWiseRemarksInterface _inter)
        {
            _interface = _inter;
        }

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

        [Route("Getdetails")]
        public ExamTermWiseRemarksDTO Getdetails([FromBody] ExamTermWiseRemarksDTO data)
        {
            return _interface.Getdetails(data);
        }

        [Route("get_class")]
        public ExamTermWiseRemarksDTO get_class([FromBody] ExamTermWiseRemarksDTO data)
        {
            return _interface.get_class(data);
        }

        [Route("get_section")]
        public ExamTermWiseRemarksDTO get_section([FromBody] ExamTermWiseRemarksDTO data)
        {
            return _interface.get_section(data);
        }

        [Route("get_term")]
        public ExamTermWiseRemarksDTO get_term([FromBody] ExamTermWiseRemarksDTO data)
        {
            return _interface.get_term(data);
        }

        [Route("search_student")]
        public ExamTermWiseRemarksDTO search_student([FromBody] ExamTermWiseRemarksDTO data)
        {
            return _interface.search_student(data);
        }

        [Route("save_details")]
        public ExamTermWiseRemarksDTO save_details([FromBody] ExamTermWiseRemarksDTO data)
        {
            return _interface.save_details(data);
        }

        [Route("edit_details")]
        public ExamTermWiseRemarksDTO edit_details([FromBody] ExamTermWiseRemarksDTO data)
        {
            return _interface.edit_details(data);
        }

        // Term Wise Participate

        [Route("Getdetails_Participate")]
        public ExamTermWiseRemarksDTO Getdetails_Participate([FromBody] ExamTermWiseRemarksDTO data)
        {
            return _interface.Getdetails_Participate(data);
        }

        [Route("search_student_participate")]
        public ExamTermWiseRemarksDTO search_student_participate([FromBody] ExamTermWiseRemarksDTO data)
        {
            return _interface.search_student_participate(data);
        }

        [Route("save_participate_details")]
        public ExamTermWiseRemarksDTO save_participate_details([FromBody] ExamTermWiseRemarksDTO data)
        {
            return _interface.save_participate_details(data);
        }

        [Route("ViewStudentParticipateDetails")]
        public ExamTermWiseRemarksDTO ViewStudentParticipateDetails([FromBody] ExamTermWiseRemarksDTO data)
        {
            return _interface.ViewStudentParticipateDetails(data);
        }
    }
}