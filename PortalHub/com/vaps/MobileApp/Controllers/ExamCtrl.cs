using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.MobileApp.Interfaces;
using PreadmissionDTOs.com.vaps.MobileApp;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.MobileApp.Controllers
{
    [Route("api/[controller]")]
    public class ExamCtrl : Controller
    {
        public ExamInterface _ExamInterface;

        public ExamCtrl(ExamInterface data)
        {
            _ExamInterface = data;
        }
        [HttpPost]
        [Route("getloaddata")]
        public ExamDTO.getStudent getStudent([FromBody] ExamDTO.getStudent data)
        {
            return _ExamInterface.getStudent(data);
        }
        //getexamdata
        [HttpPost]
        [Route("getexamdata")]
        public ExamDTO.getExamdata getExamdata([FromBody] ExamDTO.getExamdata data)
        {
            return _ExamInterface.getExamdata(data);
        }
        //StudentExamDetails
        [HttpPost]
        [Route("StudentExamDetails")]
        public ExamDTO.studentExamDetails StudentExamDetails([FromBody] ExamDTO.studentExamDetails data)
        {
            return _ExamInterface.studentExamDetails(data);
        }
        //Getdetails_IT
        [HttpPost]
        [Route("Getdetails_IT")]
        public ExamDTO.getdetails_IT Getdetails_IT([FromBody] ExamDTO.getdetails_IT data)
        {
            return _ExamInterface.Getdetails_IT(data);
        }
        //get_Exam_grade_pc
        [HttpPost]
        [Route("get_Exam_grade_pc")]
        public ExamDTO.getdetails_IT get_Exam_grade_pc([FromBody] ExamDTO.getdetails_IT data)
        {
            return _ExamInterface.get_Exam_grade_pc(data);
        }
        //saveddata_pc
        [HttpPost]
        [Route("saveddata_pc")]
        public ExamDTO.getdetails_IT saveddata_pc([FromBody] ExamDTO.getdetails_IT data)
        {
            return _ExamInterface.saveddata_pc(data);
        }
    }
}
