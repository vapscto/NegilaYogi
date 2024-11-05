using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Employee.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Employee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Employee.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeTTFacade : Controller
    {
        public EmployeeTTInterface _TTl;
        public EmployeeTTFacade(EmployeeTTInterface TTl)
        {
            _TTl = TTl;
        }

        [Route ("getdata")]
        public EmployeeDashboardDTO getdata([FromBody] EmployeeDashboardDTO obj)
        {
            return _TTl.getdata(obj);
        }

        [Route ("getdaily_data")]
        public EmployeeDashboardDTO getdaily_data([FromBody] EmployeeDashboardDTO data)
        {
            return _TTl.getdaily_data(data);
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
