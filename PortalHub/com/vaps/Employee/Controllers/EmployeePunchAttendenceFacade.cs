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
    public class EmployeePunchAttendenceFacade : Controller
    {
        public EmployeePunchAttendenceInterface _org;
        public EmployeePunchAttendenceFacade(EmployeePunchAttendenceInterface maspag)
        {
            _org = maspag;
        }
        [HttpPost]
        [Route("getalldetails")]
        public EmployeeDashboardDTO getalldetails([FromBody] EmployeeDashboardDTO data)
        {
            return _org.getdata(data);
        }
      
        [Route("getrpt")]
        public async Task< EmployeeDashboardDTO> getrpt([FromBody] EmployeeDashboardDTO org)
        {
            return await _org.getreport(org);
        }

      
    }
}
