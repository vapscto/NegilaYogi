
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
using ExamServiceHub.com.vaps.Services;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ExamImportFacadeController : Controller
    {
        public ExamImportInterface _ttcategory;

        public ExamImportFacadeController(ExamImportInterface adstu)
        {

            _ttcategory = adstu;
        }

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

        [Route("onsearch/")]
        public Task<ExamMarksDTO> onsearch([FromBody] ExamMarksDTO dto)
        {
            return _ttcategory.onsearch(dto);
        }

        [Route("ImportMarks/")]
        public ExamImportStudentDTO ImportMarks([FromBody] ExamImportStudentDTO stud)
        {
            return _ttcategory.ImportMarks(stud);
        }
    }
}
