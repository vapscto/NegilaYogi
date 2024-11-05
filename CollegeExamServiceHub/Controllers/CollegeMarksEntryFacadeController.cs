using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeExamServiceHub.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeExamServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class CollegeMarksEntryFacadeController : Controller
    {
        public CollegeMarksEntryInterface _inte;

        public CollegeMarksEntryFacadeController(CollegeMarksEntryInterface data)
        {
            _inte = data;
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

        [Route("getdetails")]
        public CollegeMarksEntryDTO getdetails([FromBody] CollegeMarksEntryDTO id)
        {
            return _inte.getdetails(id);
        }

        [Route("onchangeyear")]
        public CollegeMarksEntryDTO onchangeyear([FromBody] CollegeMarksEntryDTO dto)
        {
            return _inte.onchangeyear(dto);
        }
        [Route("onchangecourse")]
        public CollegeMarksEntryDTO onchangecourse([FromBody] CollegeMarksEntryDTO dto)
        {
            return _inte.onchangecourse(dto);
        }
        [Route("onchangebranch")]
        public CollegeMarksEntryDTO onchangebranch([FromBody] CollegeMarksEntryDTO dto)
        {
            return _inte.onchangebranch(dto);
        }

        [Route("get_exams")]
        public CollegeMarksEntryDTO get_exams([FromBody] CollegeMarksEntryDTO org)
        {
            return _inte.get_exams(org);
        }
        [Route("get_subjects")]
        public CollegeMarksEntryDTO get_subjects([FromBody] CollegeMarksEntryDTO org)
        {
            return _inte.get_subjects(org);
        }

        [Route("getsubjectscheme")]
        public CollegeMarksEntryDTO getsubjectscheme([FromBody] CollegeMarksEntryDTO org)
        {
            return _inte.getsubjectscheme(org);
        }

        [Route("getsubjectschemetype")]
        public CollegeMarksEntryDTO getsubjectschemetype([FromBody] CollegeMarksEntryDTO org)
        {
            return _inte.getsubjectschemetype(org);
        }

        [Route("onsearch/")]
        public Task<CollegeMarksEntryDTO> onsearch([FromBody] CollegeMarksEntryDTO dto)
        {
            return _inte.onsearch(dto);
        }

        [Route("SaveMarks/")]
        public CollegeMarksEntryDTO SaveMarks([FromBody] CollegeMarksEntryDTO dto)
        {
            return _inte.SaveMarks(dto);
        }
        //onchangesubsubject
        [Route("onchangesubsubject")]
        public CollegeMarksEntryDTO onchangesubsubject([FromBody] CollegeMarksEntryDTO dto)
        {
            return _inte.onchangesubsubject(dto);
        }
        //savemarkMobile
        [Route("onsearchMobile/")]
        public Task<CollegeMarksEntryDTO> onsearchMobile([FromBody] CollegeMarksEntryDTO dto)
        {
            return _inte.onsearchMobile(dto);
        }
    }
}
