using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VisitorsManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VisitorsManagement
{
    [Route("api/[controller]")]
    public class V_AppointmentApprovalReportController : Controller
    {
        public V_AppointmentApprovalReportDelegate delobj = new V_AppointmentApprovalReportDelegate();

        [Route("loaddata/{id:int}")]
        public V_AppointmentApprovalReport_DTO loaddata(int id)
        {
            V_AppointmentApprovalReport_DTO dto = new V_AppointmentApprovalReport_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.loaddata(dto);
        }
        [Route("loaddatatoday/{id:int}")]
        public V_AppointmentApprovalReport_DTO loaddatatoday(int id)
        {
            V_AppointmentApprovalReport_DTO dto = new V_AppointmentApprovalReport_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            dto.VMAP_MeetingDateTime = indianTime;
            return delobj.loaddatatoday(dto);
        }


        [HttpPost]
        [Route("report")]
        public V_AppointmentApprovalReport_DTO report([FromBody]V_AppointmentApprovalReport_DTO data)
        {
            // V_AppointmentApprovalReport_DTO data = new V_AppointmentApprovalReport_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.report(data);
        }
        [HttpPost]
        [Route("ondatechange")]
        public V_AppointmentApprovalReport_DTO ondatechange([FromBody]V_AppointmentApprovalReport_DTO data)
        {
            // V_AppointmentApprovalReport_DTO data = new V_AppointmentApprovalReport_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.loaddatatoday(data);
        }

    }
}
