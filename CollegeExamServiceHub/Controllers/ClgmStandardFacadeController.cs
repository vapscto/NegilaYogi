using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeExamServiceHub.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeExamServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class ClgmStandardFacadeController : Controller
    {
        public ClgExamStandardInterface _ExamStandard;

        public ClgmStandardFacadeController(ClgExamStandardInterface ExamStandard)
        {
            _ExamStandard = ExamStandard;
        }
        [Route("Getdetails")]
        public ExamStandardDTO Getdetails([FromBody]ExamStandardDTO data)
        {
            return _ExamStandard.Getdetails(data);
        }
       
        [HttpPost]
        [Route("savedetails")]
        public ExamStandardDTO savedetails([FromBody] ExamStandardDTO data)
        {
            return _ExamStandard.savedetails(data);
        }



    }
}
