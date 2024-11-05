using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers
{
    [Route("api/[controller]")]
    public class PreadmissionNoticeRegistrationReportController : Controller
    {
        PreadmissionNoticeRegistrationReportDelegate del = new PreadmissionNoticeRegistrationReportDelegate();
       
        [Route("get_intial_data/{id:int}")]
        public PreadmissionNoticeRegistrationReportDTO get_intial_data(int id)
        {
            PreadmissionNoticeRegistrationReportDTO data = new PreadmissionNoticeRegistrationReportDTO();

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));          
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));          
            return del.get_intial_data(data);
        }
        [Route("getprospectusno")]
        public PreadmissionNoticeRegistrationReportDTO getprospectusno([FromBody]PreadmissionNoticeRegistrationReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));            
            return del.getprospectusno(data);
        }
        [Route("getstudentlist")]
        public PreadmissionNoticeRegistrationReportDTO getstudentlist([FromBody]PreadmissionNoticeRegistrationReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));            
            return del.getstudentlist(data);
        }
        [Route("addtolist")]
        public PreadmissionNoticeRegistrationReportDTO addtolist([FromBody]PreadmissionNoticeRegistrationReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));  
            return del.addtolist(data);
        }
        [Route("Savedata")]
        public PreadmissionNoticeRegistrationReportDTO Savedata([FromBody] PreadmissionNoticeRegistrationReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));            
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            
            return del.Savedata(data);
        }
        [Route("viewstudent")]
        public PreadmissionNoticeRegistrationReportDTO viewstudent([FromBody]PreadmissionNoticeRegistrationReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));            
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            
            return del.viewstudent(data);
        }
        [Route("Editdata")]
        public PreadmissionNoticeRegistrationReportDTO Editdata([FromBody]PreadmissionNoticeRegistrationReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Editdata(data);
        }
        [Route("printData")]
        public PreadmissionNoticeRegistrationReportDTO printData([FromBody]PreadmissionNoticeRegistrationReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.printData(data);
        }

    }
}
