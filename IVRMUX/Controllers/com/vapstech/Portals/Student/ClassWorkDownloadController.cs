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
using PreadmissionDTOs.com.vaps.Portals.Employee;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClassWorkDownloadController : Controller
    {
        ClassWorkDownloadDelegate ccd = new ClassWorkDownloadDelegate();

        [HttpGet]
        [Route("getloaddata")]
        public IVRM_ClassWorkDTO getloaddata(IVRM_ClassWorkDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return ccd.getloaddata(data);
        }


        [Route("getwork")]
        public IVRM_ClassWorkDTO getwork([FromBody]IVRM_ClassWorkDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return ccd.getwork(data);
        }


    }
}
