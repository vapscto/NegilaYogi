using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Portals.Employee;
using corewebapi18072016.Delegates.com.vapstech.College.Portals.Staff;
using PreadmissionDTOs.com.vaps.College.Portals.Staff;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [Route("api/[controller]")]
    public class ClgEmployeePunchAttendenceController : Controller
    {

        ClgEmployeePunchAttendenceDelegate od = new ClgEmployeePunchAttendenceDelegate();

        // GET: api/Academic/5
        [Route("getalldetails/{id:int}")]
        public ClgStaffDashboardDTO getinitialdropdowns(int id)
        {
            ClgStaffDashboardDTO data = new ClgStaffDashboardDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            //int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.userid = UserId;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.getdata(data);
        }

      
        [HttpPost]
        [Route("getrpt")]
        public ClgStaffDashboardDTO getrpt([FromBody] ClgStaffDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.getrpt(data);
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

       
    }
}
