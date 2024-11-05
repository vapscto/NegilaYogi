using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PreadmissionDTOs.com.vaps.College.Portals;
using corewebapi18072016.Delegates.com.vapstech.College.Portals.Student;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClgFeeDetailsController : Controller
    {
        ClgFeeDetailsDelegate deleg = new ClgFeeDetailsDelegate();

        [HttpGet]
        [Route("getloaddata")]
        public ClgPortalFeeDTO getloaddata(ClgPortalFeeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return deleg.getloaddata(data);
        }

        [Route("Getdetails")]
        public ClgPortalFeeDTO Getdetails([FromBody]ClgPortalFeeDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));        

            return deleg.Getdetails(sddto);
        }


    }
}
