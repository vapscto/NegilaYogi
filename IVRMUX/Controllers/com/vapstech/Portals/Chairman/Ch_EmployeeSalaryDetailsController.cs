using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
//using DomainModel.Model.com.vapstech.p;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [Route("api/[controller]")]
    public class Ch_EmployeeSalaryDetailsController : Controller
    {
        Ch_EmployeeSalaryDetailsDelegate objdelegate1 = new Ch_EmployeeSalaryDetailsDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getalldetails")]
        public Emp_salaryDTO getalldetails(Emp_salaryDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           // data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return objdelegate1.getalldetails(data);
        }

       

        [HttpPost]
        [Route("onyearchange")]
        public Emp_salaryDTO onyearchange([FromBody]Emp_salaryDTO data)
        {
            //Emp_salaryDTO data = new Emp_salaryDTO();

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            // data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return objdelegate1.getalldetails(data);
        }
        

       

        [HttpPost]
        [Route("onmonth")]
        public Emp_salaryDTO onmonth([FromBody]Emp_salaryDTO data)
        {
            //Emp_salaryDTO data = new Emp_salaryDTO();

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            // data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return objdelegate1.onmonth(data);
        }

    }
}
