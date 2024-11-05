using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [Route("api/[controller]")]
    public class StaffCompliantsController : Controller
    {
        StaffCompliantsDelegate _delg = new StaffCompliantsDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("loaddata/{id:int}")]
        public StaffCompliantsDTO loaddata (int id)
        {
            StaffCompliantsDTO data = new StaffCompliantsDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.loaddata(data);
        }

        [Route("OnChangeEmployee")]
        public StaffCompliantsDTO OnChangeEmployee([FromBody] StaffCompliantsDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeEmployee(data);
        }

        [Route("SaveDetails")]
        public StaffCompliantsDTO SaveDetails([FromBody] StaffCompliantsDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveDetails(data);
        }

        [Route("EditDetails")]
        public StaffCompliantsDTO EditDetails([FromBody] StaffCompliantsDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.EditDetails(data);
        }

        [Route("ActiveDeativeEmployeeCompliantsDetails")]
        public StaffCompliantsDTO ActiveDeativeEmployeeCompliantsDetails([FromBody] StaffCompliantsDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.ActiveDeativeEmployeeCompliantsDetails(data);
        }

        [Route("GetReport")]
        public StaffCompliantsDTO GetReport([FromBody] StaffCompliantsDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.GetReport(data);
        }
        [Route("GetViewStaffLoaddata")]
        public StaffCompliantsDTO GetViewStaffLoaddata([FromBody] StaffCompliantsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.GetViewStaffLoaddata(data);
        }
    }
}
