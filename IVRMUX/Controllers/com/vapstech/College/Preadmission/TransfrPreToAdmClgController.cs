using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Preadmission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Preadmission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Preadmission
{
    [Route("api/[controller]")]
    public class TransfrPreToAdmClgController : Controller
    {
        TransfrPreToAdmClgDelegate del = new TransfrPreToAdmClgDelegate();
      

        [Route("getloaddata")]
        public TransfrPreToAdmDTO getloaddata()
        
        {
            TransfrPreToAdmDTO dto = new TransfrPreToAdmDTO();

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onloadgetdetails(dto);
        }

        [Route("get_branchs")]
        public TransfrPreToAdmDTO get_branchs([FromBody]TransfrPreToAdmDTO dt)
        {
            dt.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id_Preadmission"));
            return del.get_branchs(dt);
        }

        [Route("get_semester")]
        public TransfrPreToAdmDTO get_semester([FromBody]TransfrPreToAdmDTO dt)
        {
            dt.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id_Preadmission"));
            return del.get_semester(dt);
        }

        [HttpPost]
        [Route("searchdata")]
        public TransfrPreToAdmDTO searchdat([FromBody] TransfrPreToAdmDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            
            return del.getserdata(data);
        }
        [Route("exporttoadmission")]
        public TransfrPreToAdmDTO exporttoadmission([FromBody] TransfrPreToAdmDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return del.expoadmi(data);
        }
    }
}
