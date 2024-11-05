using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class IVRM_PushNotificationController : Controller
    {

        // GET: api/values
        IVRM_PushNotificationDelegate _msg = new IVRM_PushNotificationDelegate();

        [Route("savedetail")]
        public IVRM_PushNotificationDTO savedetail([FromBody]IVRM_PushNotificationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _msg.savedetail(data);
        }

        [Route("Getdetails/{id:int}")]
        public IVRM_PushNotificationDTO Getdetails(int id)
        {
            IVRM_PushNotificationDTO obj = new IVRM_PushNotificationDTO();
            obj.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            obj.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            obj.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            obj.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            obj.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));

            return _msg.Getdetails(obj);
        }
        [Route("deactivate")]
        public IVRM_PushNotificationDTO deactivate([FromBody]IVRM_PushNotificationDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return _msg.deactivate(data);
        }

        [Route("editnoticestf")]
        public IVRM_PushNotificationDTO editnoticestf([FromBody] IVRM_PushNotificationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _msg.editnoticestf(data);
        }

        [Route("editnoticestud")]
        public IVRM_PushNotificationDTO editnoticestud([FromBody] IVRM_PushNotificationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _msg.editnoticestud(data);
        }

        [Route("Deactivatemain")]
        public IVRM_PushNotificationDTO Deactivatemain([FromBody] IVRM_PushNotificationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _msg.Deactivatemain(data);
        }

        [Route("Deactivatestud")]
        public IVRM_PushNotificationDTO Deactivatestud([FromBody] IVRM_PushNotificationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _msg.Deactivatestud(data);
        }
        [Route("get_modeldata")]
        public IVRM_PushNotificationDTO get_modeldata([FromBody] IVRM_PushNotificationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _msg.get_modeldata(data);
        }


        
    }
}
