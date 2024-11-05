using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.Student;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Student;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.Student
{
    [Route("api/[controller]")]
    public class OnlineLeaveAppController : Controller
    {
        // GET: api/<controller>

        OnlineLeaveAppDelegate _delobj = new OnlineLeaveAppDelegate();

       [Route("getdetails/{id:int}")]
        public OnlineLeaveApp_DTO getdetails(int id)
        {
            OnlineLeaveApp_DTO data = new OnlineLeaveApp_DTO();

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));

            return _delobj.getdetails(data);
        }

        [Route("leaveapply")]
        public OnlineLeaveApp_DTO leaveapply([FromBody]OnlineLeaveApp_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));

            return _delobj.leaveapply(data);
        }

        [Route("editdata")]
        public OnlineLeaveApp_DTO editdata([FromBody]OnlineLeaveApp_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));           

            return _delobj.editdata(data);
        }

        [Route("leaveApproved")]
        public OnlineLeaveApp_DTO leaveApproved([FromBody]OnlineLeaveApp_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delobj.leaveApproved(data);
        }

        [Route("leaveRejected")]
        public OnlineLeaveApp_DTO leaveRejected([FromBody]OnlineLeaveApp_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delobj.leaveRejected(data);
        }

        [Route("deactiveY")]
        public OnlineLeaveApp_DTO deactiveY([FromBody]OnlineLeaveApp_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deactiveY(data);
        }

        [Route("cancellationRecord")]
        public OnlineLeaveApp_DTO cancellationRecord([FromBody]OnlineLeaveApp_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.cancellationRecord(data);
        }

        [Route("getdate_sla")]
        public OnlineLeaveApp_DTO getdate_sla(OnlineLeaveApp_DTO data)

        {
        
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return _delobj.getdate_sla(data);
        }
        [Route("getsection/{id:int}")]
        public OnlineLeaveApp_DTO getsection(int id)

        {
            OnlineLeaveApp_DTO dto = new OnlineLeaveApp_DTO();
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMCL_Id = id;
            return _delobj.getsection(dto);
        }
        [Route("getstudent")]
        public OnlineLeaveApp_DTO getstudent([FromBody]OnlineLeaveApp_DTO data)

        {
         
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return _delobj.getstudent(data);
        }
        [Route("get_leave_Report")]
        public OnlineLeaveApp_DTO get_leave_Report([FromBody]OnlineLeaveApp_DTO data)

        {
         
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return _delobj.get_leave_Report(data);
        }
        [Route("get_TC_Report")]
        public TransferCertificate_DTO get_TC_Report([FromBody]TransferCertificate_DTO data)

        {
         
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return _delobj.get_TC_Report(data);
        }
        

    }
}
