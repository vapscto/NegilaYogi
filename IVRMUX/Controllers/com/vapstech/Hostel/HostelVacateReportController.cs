using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Hostel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Hostel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Hostel
{
    [Route("api/[controller]")]
    public class HostelVacateReportController : Controller
    {
        HostelVacateReportDelegate del = new HostelVacateReportDelegate();

        [Route("loaddata")]
        public HostelVacateReportDTO loaddata([FromBody]HostelVacateReportDTO obj)
        {
            //HostelVacateReportDTO obj = new HostelVacateReportDTO();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //obj.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.loaddata(obj);
        }

        [Route("get_report")]
        public HostelVacateReportDTO get_report([FromBody] HostelVacateReportDTO obj)
        {
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.get_report(obj);
        }

        [Route("get_Studentlist")]
        public HostelVacateReportDTO get_Studentlist([FromBody] HostelVacateReportDTO obj)
        {
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.get_Studentlist(obj);
        }
    }
}
