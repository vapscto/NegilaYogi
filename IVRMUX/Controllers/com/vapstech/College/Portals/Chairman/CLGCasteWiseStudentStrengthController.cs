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
    public class CLGCasteWiseStudentStrengthController : Controller
    {
        CLGCasteWiseStudentStrengthDelegate clgdelegate = new CLGCasteWiseStudentStrengthDelegate();

        [HttpGet]
        [Route("Getdetails")]      
        public CLGCHStudentStrengthDTO Getdetails(CLGCHStudentStrengthDTO data)
        {     
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));          
           // data.AMCST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return clgdelegate.Getdetails(data);
        }

        [Route("yrchange/{id}")]
        public CLGCHStudentStrengthDTO yrchange(int id)
        {
            CLGCHStudentStrengthDTO data = new CLGCHStudentStrengthDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = id;
            return clgdelegate.Getdetails(data);
        }

    }
}
