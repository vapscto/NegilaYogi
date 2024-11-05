using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.Portals.HOD;

using PreadmissionDTOs.com.vaps.Portals.Student;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class HODExamReportController : Controller
    {
        HODExamReportDelegate fdd = new HODExamReportDelegate();

        [HttpGet]
        [Route("getloaddata")]
        public ExamDTO getloaddata(ExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return fdd.getloaddata(data);
        }

        [Route("getexamdata")]
        public ExamDTO getexamdata([FromBody]ExamDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return fdd.getexamdata(sddto);
        }

        [Route("getexamdetails")]
        public ExamDTO getexamdetails([FromBody]ExamDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //sddto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return fdd.getexamdetails(sddto);
        }
        [Route("getsectiondata")]
        public ExamDTO getsectiondata([FromBody]ExamDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return fdd.getsectiondata(sddto);
        }
        [Route("get_classes")]
        public ExamDTO get_classes([FromBody]ExamDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return fdd.get_classes(sddto);
        }

        [Route("getstudentdata")]
        public ExamDTO getstudentdata([FromBody]ExamDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return fdd.getstudentdata(sddto);
        }

    }
}
