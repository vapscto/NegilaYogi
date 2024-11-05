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
    public class CLGStudentRequestReportController : Controller
    {
        CLGStudentRequestReportDelegate del = new CLGStudentRequestReportDelegate();

        [Route("getdata/{id:int}")]
        public CLGStudentReportDTO getdata(int id)
        {
            CLGStudentReportDTO data = new CLGStudentReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getdata(data);
        }
        [HttpPost]
        [Route("getreport")]
        public CLGStudentReportDTO getreport([FromBody] CLGStudentReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getreport(data);
        }
        [HttpPost]
        [Route("getconfirmreport")]
        public CLGStudentReportDTO getconfirmreport([FromBody] CLGStudentReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getconfirmreport(data);
        }
    }
}
