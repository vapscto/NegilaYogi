using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Portals.Staff;
using corewebapi18072016.Delegates.com.vapstech.College.Portals.Staff;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClgSalaryDetailsController : Controller
    {
        ClgSalaryDetailsDelegate clgdelegate = new ClgSalaryDetailsDelegate();

        [HttpGet]
        [Route("getloaddata")]
        public ClgPortalHRMSDTO getloaddata(ClgPortalHRMSDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));                     
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.getloaddata(data);
        }

        [Route("getSalary")]
        public ClgPortalHRMSDTO getSalary([FromBody]ClgPortalHRMSDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return clgdelegate.getSalary(data);
        }

        [Route("getsalaryalldetails")]
        public ClgPortalHRMSDTO getsalaryalldetails([FromBody] ClgPortalHRMSDTO data)
        {            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return clgdelegate.getsalaryalldetails(data);
        }

    }
}
