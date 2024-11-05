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
using PreadmissionDTOs.com.vaps.College.Portals;
using corewebapi18072016.Delegates.com.vapstech.College.Portals.Student;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClgStudentFeedbackFormController : Controller
    {
        ClgStudentFeedbackFormDelegate fdd = new ClgStudentFeedbackFormDelegate();

        [HttpGet]
        [Route("getloaddata")]
        public ClgStudentFeedbackFormDTO getloaddata(ClgStudentFeedbackFormDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return fdd.getloaddata(data);
        }

        [Route("savefeedback")]
        public ClgStudentFeedbackFormDTO savefeedback([FromBody]ClgStudentFeedbackFormDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return fdd.savefeedback(data);
        }


        [Route("deactive")]
        public ClgStudentFeedbackFormDTO deactive([FromBody]ClgStudentFeedbackFormDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));          
            return fdd.deactive(data);
        }
    }
}
