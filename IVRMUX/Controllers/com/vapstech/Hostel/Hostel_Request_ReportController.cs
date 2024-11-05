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
    public class Hostel_Request_ReportController : Controller
    {
        Hostel_Request_ReportDelegate del = new Hostel_Request_ReportDelegate();

        [Route("getdata/{id:int}")]
        public Hostel_Request_ReportDTO getdata(int id)
        {
            Hostel_Request_ReportDTO data = new Hostel_Request_ReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getdata(data);
        }
        [HttpPost]
        [Route("getreport")]
        public Hostel_Request_ReportDTO getreport([FromBody] Hostel_Request_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getreport(data);
        }
        [HttpPost]
        [Route("getconfirmreport")]
        public Hostel_Request_ReportDTO getconfirmreport([FromBody] Hostel_Request_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getconfirmreport(data);
        }
    }
}
