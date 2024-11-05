
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Placement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Placement;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Placement
{

    [Route("api/[controller]")]
    public class CISReport : Controller
    {
        CISReportDelegate del = new CISReportDelegate();
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("getdetails/{id:int}")]
        public PL_CampusInterview_ScheduleDTO getdetails(int id)
        {
            PL_CampusInterview_ScheduleDTO data = new PL_CampusInterview_ScheduleDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getdetails(data);
        }
        //report
        [Route("report")]
        public PL_CampusInterview_ScheduleDTO report([FromBody]  PL_CampusInterview_ScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.report(data);
        }

    }
}
