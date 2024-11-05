using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class Baldwin_Final_P_ReportController : Controller
    {
        Baldwin_Final_P_ReportDelegates del_fr = new Baldwin_Final_P_ReportDelegates();
        [HttpGet]
        [Route("Getdetails")]
        public Baldwin_Final_P_ReportDTO Getdetails(Baldwin_Final_P_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_fr.Getdetails(data);
        }
        [HttpPost]
        [Route("get_classes")]
        public Baldwin_Final_P_ReportDTO get_classes([FromBody] Baldwin_Final_P_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_fr.get_classes(data);
        }
        [Route("get_sections")]
        public Baldwin_Final_P_ReportDTO get_sections([FromBody] Baldwin_Final_P_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_fr.get_sections(data);
        }
        [Route("get_students")]
        public Baldwin_Final_P_ReportDTO get_students([FromBody] Baldwin_Final_P_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_fr.get_students(data);
        }
        [Route("get_report")]
        public Baldwin_Final_P_ReportDTO get_report([FromBody] Baldwin_Final_P_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_fr.get_report(data);
        }

    }
}
