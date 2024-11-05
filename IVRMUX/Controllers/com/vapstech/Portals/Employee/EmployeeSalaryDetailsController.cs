using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Portals.Employee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class EmployeeSalaryDetailsController : Controller
    {
        EmployeeSalaryDetailsDelegate objdelegate1 = new EmployeeSalaryDetailsDelegate();

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
            return objdelegate1.getalldetails(data);
        }

        [Route("getdaily_data")]
        public EmployeeDashboardDTO getdaily_data([FromBody]EmployeeDashboardDTO data)
        {
            //EmployeeDashboardDTO data = new EmployeeDashboardDTO();
            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return objdelegate1.getdaily_data(data);
        }
        [HttpGet]
        [Route("getsalaryalldetails/{id:int}")]
        public EmployeeDashboardDTO getsalaryalldetails(int id)
        {
            EmployeeDashboardDTO data = new EmployeeDashboardDTO();
            data.HRES_Id = id;
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return objdelegate1.getsalaryalldetails(data);
        }
    }
}
