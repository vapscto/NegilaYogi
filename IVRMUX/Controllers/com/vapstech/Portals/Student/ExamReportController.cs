using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.Portals.Student;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ExamReportController : Controller
    {
        ExamReportDelegate fdd = new ExamReportDelegate();

        [HttpGet]
        [Route("getloaddata")]
        public ExamDTO getloaddata(ExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
           
            return fdd.getloaddata(data);
        }

        [Route("getexamdata")]
        public ExamDTO getexamdata([FromBody]ExamDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));          
            return fdd.getexamdata(sddto);
        }

        [Route("getSubjects")]
        public ExamDTO getSubjects([FromBody]ExamDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));        
            return fdd.getSubjects(sddto);
        }

        [Route("StudentExamDetails")]
        public ExamDTO getStudentExamDetails([FromBody]ExamDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));        
            return fdd.getStudentExamDetails(dto);
        }
    }
}
