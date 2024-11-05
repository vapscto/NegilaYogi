
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
    public class ExamStandardFacadeController : Controller
    {
        public ExamStandardInterface _ExamStandard;

        public ExamStandardFacadeController(ExamStandardInterface ExamStandard)
        {
            _ExamStandard = ExamStandard;
        }

        [HttpGet]
        [Route("Getdetails/{id:int}")]
        public ExamStandardDTO Getdetails(int id)//int IVRMM_Id
        {
           
            return _ExamStandard.Getdetails(id);
           
        }

        
        
        [HttpPost]
        [Route("savedetails")]
        public ExamStandardDTO savedetails([FromBody] ExamStandardDTO data)
        {
            return _ExamStandard.savedetails(data);
        }
       
           

    }
}
