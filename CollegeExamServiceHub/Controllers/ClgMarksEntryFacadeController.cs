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
    public class ClgMarksEntryFacadeController : Controller
    {
        public ClgMarksEntryInterface _inte;

        public ClgMarksEntryFacadeController(ClgMarksEntryInterface data)
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
        public ClgMarksEntryDTO getdetails([FromBody] ClgMarksEntryDTO id)
        {
            return _inte.getdetails(id);
        }

        [Route("onchangeyear")]
        public ClgMarksEntryDTO onchangeyear([FromBody] ClgMarksEntryDTO dto)
        {
            return _inte.onchangeyear(dto);
        }

        [Route("onchangecourse")]
        public ClgMarksEntryDTO onchangecourse([FromBody] ClgMarksEntryDTO dto)
        {
            return _inte.onchangecourse(dto);
        }

        [Route("onchangebranch")]
        public ClgMarksEntryDTO onchangebranch([FromBody] ClgMarksEntryDTO dto)
        {
            return _inte.onchangebranch(dto);
        }

        [Route("get_exams")]
        public ClgMarksEntryDTO Post3([FromBody] ClgMarksEntryDTO org)
        {
            return _inte.get_exams(org);
        }

        [Route("get_subjects")]
        public ClgMarksEntryDTO get_subjects([FromBody] ClgMarksEntryDTO org)
        {
            return _inte.get_subjects(org);
        }

        [Route("getsubjectscheme")]
        public ClgMarksEntryDTO getsubjectscheme([FromBody] ClgMarksEntryDTO org)
        {
            return _inte.getsubjectscheme(org);
        }

        [Route("getsubjectschemetype")]
        public ClgMarksEntryDTO getsubjectschemetype([FromBody] ClgMarksEntryDTO org)
        {
            return _inte.getsubjectschemetype(org);
        }

        [Route("onsearch/")]
        public Task<ClgMarksEntryDTO> onsearch([FromBody] ClgMarksEntryDTO dto)
        {
            return _inte.onsearch(dto);
        }

        [Route("SaveMarks/")]
        public ClgMarksEntryDTO SaveMarks([FromBody] ClgMarksEntryDTO dto)
        {
            return _inte.SaveMarks(dto);
        }


    }
}
