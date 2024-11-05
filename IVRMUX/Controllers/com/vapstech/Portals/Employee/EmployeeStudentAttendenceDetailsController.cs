using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using corewebapi18072016.Delegates.com.vapstech.Portals.Employee;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Employee
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class EmployeeStudentAttendenceDetailsController : Controller
    {
        EmployeeStudentAttendenceDetailsDelegates crStr = new EmployeeStudentAttendenceDetailsDelegates();

        [HttpGet]
        [Route("Getdetails")]
        public EmployeeDashboardDTO Getdetails(EmployeeDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getdetails(data);
        }


        [Route("getclass/{id}")]
        public EmployeeDashboardDTO getclass(int id)
        {
            EmployeeDashboardDTO data = new EmployeeDashboardDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          
            return crStr.getclass(data);
        }
        [HttpPost]
        [Route("Getsection")]
        public EmployeeDashboardDTO Getsection([FromBody] EmployeeDashboardDTO data)
        {
           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return crStr.Getsection(data);

        }

        [HttpPost]
        [Route("GetAttendence")]
        public EmployeeDashboardDTO GetAttendence([FromBody] EmployeeDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));           
            return crStr.GetAttendence(data);

        }
        [HttpPost]
        [Route("GetIndividualAttendence")]
        public EmployeeDashboardDTO GetIndividualAttendence([FromBody] EmployeeDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            return crStr.GetIndividualAttendence(data);

        }

      
     
    }
}
