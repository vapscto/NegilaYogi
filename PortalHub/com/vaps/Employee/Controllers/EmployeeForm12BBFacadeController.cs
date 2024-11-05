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
    public class EmployeeForm12BBFacadeController : Controller
    {
        public EmployeeForm12BBInterface __Salary;
        public EmployeeForm12BBFacadeController(EmployeeForm12BBInterface Salary)
        {
            __Salary = Salary;
        }


        [Route("getsalaryalldetails")]
        public Employee12BBDTO getsalaryalldetails([FromBody] Employee12BBDTO data)
        {
            return __Salary.getsalaryalldetails(data);
        }

        //[Route("getsalaryalldetails")]
        //public Employee12BBDTO getsalaryalldetails([FromBody] Employee12BBDTO data)
        //{
        //    return __Salary.getsalaryalldetails(data);
        //}

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
        [Route("getdata")]
        public Employee12BBDTO getdata([FromBody] Employee12BBDTO obj)
        {
            return __Salary.getdata(obj);
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