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
    public class EmployeeStudentAttendenceDetailsFacade : Controller
    {
        public EmployeeStudentAttendenceDetailsInterface _ChairmanDashboardReport;

        public EmployeeStudentAttendenceDetailsFacade(EmployeeStudentAttendenceDetailsInterface data)
        {
            _ChairmanDashboardReport = data;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("Getdetails")]
        public EmployeeDashboardDTO Getdetails([FromBody] EmployeeDashboardDTO data)
        {
            return _ChairmanDashboardReport.Getdetails(data);
        }

        [HttpPost]
        [Route("getclass")]
        public EmployeeDashboardDTO getclass([FromBody] EmployeeDashboardDTO data)
        {
            return _ChairmanDashboardReport.getclass(data);
        }
        [HttpPost]
        [Route("Getsection")]
        public EmployeeDashboardDTO Getsection([FromBody] EmployeeDashboardDTO data)
        {
            return _ChairmanDashboardReport.Getsection(data);
        }
        [HttpPost]
        [Route("GetAttendence")]
        public EmployeeDashboardDTO GetAttendence([FromBody] EmployeeDashboardDTO data)
        {
            return _ChairmanDashboardReport.GetAttendence(data);
        }

        [HttpPost]
        [Route("GetIndividualAttendence")]
        public Task<EmployeeDashboardDTO> GetIndividualAttendence([FromBody] EmployeeDashboardDTO data)
        {
            return _ChairmanDashboardReport.GetIndividualAttendence(data);
        }

    }
}
