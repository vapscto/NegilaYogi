using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.Employee;
using IVRMUX.Delegates.com.vapstech.VMS.Training;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PreadmissionDTOs.com.vaps.VMS.Training;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace IVRMUX.Controllers.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class CovidTestUploadController : Controller
    {
        CovidTestUploadDelegate _del = new CovidTestUploadDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("onloaddata/{id:int}")]
        public CovidTestUploadDTO onloaddata(int id)
        {
            CovidTestUploadDTO data = new CovidTestUploadDTO();
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.onloaddata(data);
        }
        [HttpPost]
        [Route("saverecord")]
        public CovidTestUploadDTO saverecord([FromBody] CovidTestUploadDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.saverecord(data);
        }

        [Route("deactiveY")]
        public CovidTestUploadDTO deactiveY([FromBody] CovidTestUploadDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.deactiveY(data);
        }
    }
}
