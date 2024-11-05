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
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using corewebapi18072016.Delegates.com.vapstech.Portals.IVRM;


namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class PortalMonthEndReportController : Controller
    {
        PortalMonthEndReportDelegate delgte = new PortalMonthEndReportDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public PortalMonthEndReportDTO getloaddata(int id)
        {
            PortalMonthEndReportDTO data = new PortalMonthEndReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delgte.getloaddata(data);
        }


        [Route("getmonthreport")]
        public PortalMonthEndReportDTO getmonthreport([FromBody]PortalMonthEndReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));                      
            return delgte.getmonthreport(data);
        }






    }
}
