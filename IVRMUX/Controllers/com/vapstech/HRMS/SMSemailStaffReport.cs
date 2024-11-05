using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [Route("api/[controller]")]
    public class SMSemailStaffReport : Controller
    {
        SMSemailStaffReportDelegate del = new SMSemailStaffReportDelegate();
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public SMSemailStaffReportDTO getalldetails(int id)
        {
            SMSemailStaffReportDTO dto = new SMSemailStaffReportDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onloadgetdetails(dto);
        }
        [Route("getreport")]
        public SMSemailStaffReportDTO get_depts([FromBody]SMSemailStaffReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getreport(dto);
        }

        [Route("smsemail")]
        public SMSemailStaffReportDTO get_desig([FromBody]SMSemailStaffReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.smsemail(dto);
        }
        //Destination
        [Route("Destination")]
        public SMSemailStaffReportDTO Destination([FromBody]SMSemailStaffReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Destination(dto);
        }
    }
}
