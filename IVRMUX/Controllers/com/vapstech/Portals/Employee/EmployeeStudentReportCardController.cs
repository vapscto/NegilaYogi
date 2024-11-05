using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using corewebapi18072016.Delegates.com.vapstech.Portals.Employee;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Portals.Student;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class EmployeeStudentReportCardController : Controller
    {
        EmployeeStudentReportCardDelegates crStr = new EmployeeStudentReportCardDelegates();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public EmployeeDashboardDTO Getdetails(EmployeeDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getdetails(data);
        }

      
        [Route("savedetails")]
        public EmployeeDashboardDTO savedetails([FromBody] EmployeeDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.showdetails(data);
        }

        [Route("get_class")]
        public EmployeeDashboardDTO get_class([FromBody]EmployeeDashboardDTO data)

        {

            //data.Amst_Id = Id;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;
            return crStr.get_class(data);
        }
        [Route("get_section")]
        public EmployeeDashboardDTO get_section([FromBody]EmployeeDashboardDTO data)

        {

            //data.Amst_Id = Id;
            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return crStr.get_section(data);
        }
        [Route("get_student")]
        public EmployeeDashboardDTO get_student([FromBody]EmployeeDashboardDTO data)

        {

            //data.Amst_Id = Id;
            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return crStr.get_student(data);
        }
        [Route("get_exam")]
        public EmployeeDashboardDTO get_exam([FromBody]EmployeeDashboardDTO data)

        {

            //data.Amst_Id = Id;
            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return crStr.get_exam(data);
        }
        

    }
}
