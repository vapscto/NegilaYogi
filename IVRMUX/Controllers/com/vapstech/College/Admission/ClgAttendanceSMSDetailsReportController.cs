using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class ClgAttendanceSMSDetailsReportController : Controller
    {

        ClgAttendanceSMSDetailsReportDelegate del = new ClgAttendanceSMSDetailsReportDelegate();
        [Route("loaddata/{id:int}")]
        public ClgAttendanceSMSDetailsReport_DTO loaddata(int id)
        {
            ClgAttendanceSMSDetailsReport_DTO data = new ClgAttendanceSMSDetailsReport_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }
        [Route("getcourse")]
        public ClgAttendanceSMSDetailsReport_DTO getcourse([FromBody] ClgAttendanceSMSDetailsReport_DTO data)
        {
            
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getcourse(data);
        } [Route("getbranch")]
        public ClgAttendanceSMSDetailsReport_DTO getbranch([FromBody] ClgAttendanceSMSDetailsReport_DTO data)
        {
            
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getbranch(data);
        }[Route("getsemester")]
        public ClgAttendanceSMSDetailsReport_DTO getsemester([FromBody] ClgAttendanceSMSDetailsReport_DTO data)
        {
            
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getsemester(data);
        }[Route("showdetails")]
        public ClgAttendanceSMSDetailsReport_DTO showdetails([FromBody] ClgAttendanceSMSDetailsReport_DTO data)
        {
            
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.showdetails(data);
        }
    }
}
