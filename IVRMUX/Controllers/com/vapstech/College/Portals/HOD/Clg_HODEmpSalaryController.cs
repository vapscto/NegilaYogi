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
    public class Clg_HODEmpSalaryController : Controller
    {
        Clg_HODEmpSalaryDelegate clgdelegate = new Clg_HODEmpSalaryDelegate();

        [HttpGet]
        [Route("Getdetails")]      
        public Clg_HODEmpSalaryDTO Getdetails(Clg_HODEmpSalaryDTO data)
        {     
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            // data.AMCST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return clgdelegate.Getdetails(data);
        }

        [Route("yrchange/{id}")]
        public Clg_HODEmpSalaryDTO yrchange(int id)
        {
            Clg_HODEmpSalaryDTO data = new Clg_HODEmpSalaryDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.HRMLY_Id = id;
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return clgdelegate.Getdetails(data);
        }

    }
}
