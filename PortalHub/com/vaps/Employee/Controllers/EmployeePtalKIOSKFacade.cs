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
    public class EmployeePtalKIOSKFacade : Controller
    {
        public EmployeePtalKIOSKInterface _empl;
        public EmployeePtalKIOSKFacade(EmployeePtalKIOSKInterface empl)
        {
            _empl = empl;
        }

        [Route("getleave_report")]
        public Task<EmployeeKioskLEAVEDTO> getleave_report([FromBody] EmployeeKioskLEAVEDTO obj)
        {
            return _empl.getleave_report(obj);
        }

        [Route("getEmployeedata")]
        public EmployeeKIOSKPortalDTO getEmployeedata([FromBody] EmployeeKIOSKPortalDTO data)
        {
            return _empl.getEmployeedata(data);
        }

        [Route("getEmployeeFullDetails")]
        public EmployeeKIOSKPortalDTO getEmployeeFullDetails([FromBody] EmployeeKIOSKPortalDTO data)
        {
            return _empl.getEmployeeFullDetails(data);
        }

        [Route("getPunchreport")]
        public Task<EmployeeKioskPunchDTO> getPunchreport([FromBody] EmployeeKioskPunchDTO obj)
        {
            return _empl.getPunchreport(obj);
        }


        [Route("getyeardata")]
        public EmployeeKioskSalaryDTO getyeardata([FromBody] EmployeeKioskSalaryDTO obj)
        {
            return _empl.getyeardata(obj);
        }
        [Route("getsalarydetailsdata")]
        public EmployeeKioskSalaryDTO getsalarydetailsdata([FromBody] EmployeeKioskSalaryDTO obj)
        {
            return _empl.getsalarydetailsdata(obj);
        }
        [Route("getTTdata")]
        public EmployeeKioskTimeTableDTO getTTdata([FromBody] EmployeeKioskTimeTableDTO obj)
        {
            return _empl.getTTdata(obj);
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
