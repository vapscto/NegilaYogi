using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PortalHub.com.vaps.Employee.Interfaces;
namespace PortalHub.com.vaps.Employee.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeForm16FacadeController : Controller
    {
        public EmployeeForm16Interface __Salary;
        public EmployeeForm16FacadeController(EmployeeForm16Interface Salary)
        {
            __Salary = Salary;
        }

        [Route("getdata")]
        public Employee16DTO getdata([FromBody] Employee16DTO obj)
        {
            return __Salary.getdata(obj);
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