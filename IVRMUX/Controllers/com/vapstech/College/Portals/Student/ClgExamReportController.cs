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
using PreadmissionDTOs.com.vaps.College.Portals.Student;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClgExamReportController : Controller
    {
        ClgExamReportDelegate fdd = new ClgExamReportDelegate();

        [HttpGet]
        [Route("getloaddata")]
        public ClgExamDTO getloaddata(ClgExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
           
            return fdd.getloaddata(data);
        }

        [Route("getexamdata")]
        public ClgExamDTO getexamdata([FromBody]ClgExamDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));          
            return fdd.getexamdata(sddto);
        }

        [Route("getSubjects")]
        public ClgExamDTO getSubjects([FromBody]ClgExamDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));        
            return fdd.getSubjects(sddto);
        }

        [Route("StudentExamDetails")]
        public ClgExamDTO getStudentExamDetails([FromBody]ClgExamDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));        
            return fdd.getStudentExamDetails(dto);
        }
    }
}
