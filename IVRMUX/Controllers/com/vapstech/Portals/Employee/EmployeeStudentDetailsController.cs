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
    [Route("api/[controller]")]
    public class EmployeeStudentDetailsController : Controller
    {
        EmployeeStudentDetailsDelegate objdelegate = new EmployeeStudentDetailsDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getalldetails")]
        public EmployeeDashboardDTO getalldetails(EmployeeDashboardDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return objdelegate.getalldetails(data);
        }

        [Route("get_class")]
        public EmployeeDashboardDTO get_class([FromBody]EmployeeDashboardDTO data)

        {

            //data.Amst_Id = Id;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            return objdelegate.get_class(data);
        }
        [Route("get_section")]
        public EmployeeDashboardDTO get_section([FromBody]EmployeeDashboardDTO data)

        {

            //data.Amst_Id = Id;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return objdelegate.get_section(data);
        }
        [Route("get_student")]
        public EmployeeDashboardDTO get_student([FromBody]EmployeeDashboardDTO data)

        {

            //data.Amst_Id = Id;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return objdelegate.get_student(data);
        }

    
    }
}
