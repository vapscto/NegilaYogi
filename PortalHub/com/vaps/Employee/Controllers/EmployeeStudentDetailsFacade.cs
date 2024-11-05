using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PortalHub.com.vaps.Employee.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Employee.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeStudentDetailsFacade : Controller
    {


        public EmployeeStudentDetailsInterface _TTl;
        public EmployeeStudentDetailsFacade(EmployeeStudentDetailsInterface TTl)
        {
            _TTl = TTl;
        }

        [Route("getdata")]
        public EmployeeDashboardDTO getdata([FromBody] EmployeeDashboardDTO obj)
        {
            return _TTl.getdata(obj);
        }

      
        [Route("get_class")]
        public EmployeeDashboardDTO get_class([FromBody] EmployeeDashboardDTO data)
        {
            return _TTl.get_class(data);
        }
        [Route("get_section")]
        public EmployeeDashboardDTO get_section([FromBody] EmployeeDashboardDTO data)
        {
            return _TTl.get_section(data);
        }
        [Route("get_student")]
        public EmployeeDashboardDTO get_student([FromBody] EmployeeDashboardDTO data)
        {
            return _TTl.get_student(data);
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
