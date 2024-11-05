
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.Exam;
using ExamServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MarksEntryFacadeController : Controller
    {
        public MarksEntryInterface _ttcategory;
        public MarksEntryFacadeController(MarksEntryInterface maspag)
        {

            _ttcategory = maspag;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpPost]
        [Route("Getdetails")]
        public ExamMarksDTO Getdetails([FromBody] ExamMarksDTO id)
        {
            return _ttcategory.getdetails(id);
        }

        [Route("onselectAcdYear/")]
        public ExamMarksDTO onselectAcdYear([FromBody] ExamMarksDTO dto)
        {
            return _ttcategory.onselectAcdYear(dto);
        }

        [Route("onselectclass/")]
        public ExamMarksDTO onselectclass([FromBody] ExamMarksDTO dto)
        {
            return _ttcategory.onselectclass(dto);
        }

        [Route("onselectSection/")]
        public ExamMarksDTO onselectSection([FromBody] ExamMarksDTO dto)
        {
            return _ttcategory.onselectSection(dto);
        }

        [Route("onselectExam/")]
        public ExamMarksDTO onselectExam([FromBody] ExamMarksDTO dto)
        {
            return _ttcategory.onselectExam(dto);
        }

        [Route("onselectSubject/")]
        public ExamMarksDTO onselectSubject([FromBody] ExamMarksDTO dto)
        {
            return _ttcategory.onselectSubject(dto);
        }

        [Route("onchangesubsubject/")]
        public ExamMarksDTO onchangesubsubject([FromBody] ExamMarksDTO dto)
        {
            return _ttcategory.onchangesubsubject(dto);
        }

        [Route("onsearch/")]
        public Task<ExamMarksDTO> onsearch([FromBody] ExamMarksDTO dto)
        {
            return _ttcategory.onsearch(dto);
        }

        [Route("SaveMarks/")]
        public ExamMarksDTO SaveMarks([FromBody] ExamMarksDTO dto)
        {
            return _ttcategory.SaveMarks(dto);
        }

        [Route("DeleteMarks")]
        public ExamMarksDTO DeleteMarks([FromBody] ExamMarksDTO dto)
        {
            return _ttcategory.DeleteMarks(dto);
        }

    }
}
