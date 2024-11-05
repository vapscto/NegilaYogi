using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.College.Portals;
using PreadmissionDTOs.com.vaps.College.Portals;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class CLGCHStudentStrengthController : Controller
    {
        CLGCHStudentStrengthDelegate clgdelegate = new CLGCHStudentStrengthDelegate();

        [HttpGet]
        [Route("Getdetails")]      
        public CLGCHStudentStrengthDTO Getdetails(CLGCHStudentStrengthDTO data)
        {     
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));          
           // data.AMCST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return clgdelegate.Getdetails(data);
        }

        [Route("yrchange")]
        public CLGCHStudentStrengthDTO yrchange([FromBody]CLGCHStudentStrengthDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return clgdelegate.Getdetails(data);
        }

        [HttpGet]
        [Route("Getdetails1")]
        public CLGCHStudentStrengthDTO Getdetails1(CLGCHStudentStrengthDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            // data.AMCST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return clgdelegate.Getdetails1(data);
        }

        [Route("yrchange1")]
        public CLGCHStudentStrengthDTO yrchange1([FromBody]CLGCHStudentStrengthDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return clgdelegate.Getdetails1(data);
        }

    }
}
