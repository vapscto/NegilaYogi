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
    public class Stu_FeedbackController : Controller
    {
        Stu_FeedbackDelegate fdd = new Stu_FeedbackDelegate();

        [HttpGet]
        [Route("getloaddata")]
        public Stu_FeedbackDTO getloaddata(Stu_FeedbackDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return fdd.getloaddata(data);
        }

        [Route("savecomment")]
        public Stu_FeedbackDTO savecomment([FromBody]Stu_FeedbackDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));

            return fdd.savecomment(sddto);
        }

        [Route("getexamdetails")]
        public Stu_FeedbackDTO getexamdetails([FromBody]Stu_FeedbackDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            sddto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return fdd.getexamdetails(sddto);
        }

    }
}
