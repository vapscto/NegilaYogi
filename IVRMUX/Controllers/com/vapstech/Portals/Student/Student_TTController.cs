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

    [Route("api/[controller]")]
    public class Student_TTController : Controller
    {
        Student_TTDelegate fdd = new Student_TTDelegate();

        [Route("getloaddata/{id:int}")]
        public StudentDashboardDTO getloaddata(int id)
        {
            StudentDashboardDTO data = new StudentDashboardDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return fdd.getloaddata(data);
        }


        [Route("getStudentTT")]
        public StudentDashboardDTO getStudentTT([FromBody]StudentDashboardDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
           
            return fdd.getStudentTT(sddto);
        }


    }
}
