using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers
{
    [Route("api/[controller]")]
    public class TaskCreationFromClintController : Controller
    {

        public TaskCreationFromClintDelegate _objdel = new TaskCreationFromClintDelegate();

        [Route("getdetails/{id:int}")]
        public TaskCreationFromClintDTO getdetails(int id)
        {
            TaskCreationFromClintDTO data = new TaskCreationFromClintDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.getdetails(data);
        }


        [Route("savedata")]
        public TaskCreationFromClintDTO savedata([FromBody]TaskCreationFromClintDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.savedata(data);
        }

        [Route("deactive")]
        public TaskCreationFromClintDTO deactive([FromBody]TaskCreationFromClintDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.deactive(data);
        }





    }
}
