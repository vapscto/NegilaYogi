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
    public class EmployeeForm12BBController : Controller
    {
        EmployeeForm12BBDelegate objdelegate1 = new EmployeeForm12BBDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getalldetails")]
        public Employee12BBDTO getalldetails(Employee12BBDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          //  data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return objdelegate1.getalldetails(data);
        }

        //public Employee12BBDTO getyear(Employee12BBDTO year)
        //{
        //    return objdelegate1.getyear(year);
        //}

        [Route("getdaily_data")]
        public Employee12BBDTO getdaily_data([FromBody]Employee12BBDTO data)
        {
            //EmployeeDashboardDTO data = new EmployeeDashboardDTO();
            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return objdelegate1.getdaily_data(data);
        }
      
        [Route("getsalaryalldetails")]
        public Employee12BBDTO getsalaryalldetails([FromBody] Employee12BBDTO data)
        {      
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          
            return objdelegate1.getsalaryalldetails(data);
        }
    }
}
