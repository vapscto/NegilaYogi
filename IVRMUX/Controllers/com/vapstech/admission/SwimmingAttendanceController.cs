using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.admission
{
    [Route("api/[controller]")]
    public class SwimmingAttendanceController : Controller
    {

        public SwimmingAttendanceDelegate _delg = new SwimmingAttendanceDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("loaddata/{id:int}")]
        public SwimmingAttendanceDTO loaddata(int id)
        {
            SwimmingAttendanceDTO data = new SwimmingAttendanceDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.loaddata(data);
        }
        [Route("onchnageyear")]
        public SwimmingAttendanceDTO onchnageyear([FromBody]  SwimmingAttendanceDTO data)
        {
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));           
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            data.ASA_Network_IP = remoteIpAddress.ToString();
            return _delg.onchnageyear(data);
        }
        [Route("onchangeclass")]
        public SwimmingAttendanceDTO onchangeclass([FromBody]  SwimmingAttendanceDTO data)
        {
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            data.ASA_Network_IP = remoteIpAddress.ToString();
            return _delg.onchangeclass(data);
        }
        [Route("search")]
        public SwimmingAttendanceDTO search([FromBody]  SwimmingAttendanceDTO data)
        {
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            data.ASA_Network_IP = remoteIpAddress.ToString();
            return _delg.search(data);
        }
        [Route("save")]
        public SwimmingAttendanceDTO save([FromBody]  SwimmingAttendanceDTO data)
        {
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            data.ASA_Network_IP = remoteIpAddress.ToString();
            return _delg.save(data);
        }
        
    }
}
