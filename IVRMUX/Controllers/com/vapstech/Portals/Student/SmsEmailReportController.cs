using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.Student;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace IVRMUX.Controllers.com.vapstech.Portals.Student
{
    [Produces("application/json")]
    [Route("api/SmsEmailReport")]
    public class SmsEmailReportController : Controller
    {
        SmsEmailReportDelegate fdd = new SmsEmailReportDelegate();

        [HttpGet]
        [Route("getloaddata")]
        public SmsEmailReportDTO getloaddata(SmsEmailReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           

            return fdd.getloaddata(data);
        }

        [Route("getdata")]
        public SmsEmailReportDTO getdata([FromBody]SmsEmailReportDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return fdd.getdata(sddto);
        }
    }
}