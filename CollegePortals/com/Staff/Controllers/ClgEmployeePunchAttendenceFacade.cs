using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegePortals.com.Staff.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Portals.Staff;
using PreadmissionDTOs.com.vaps.Portals.Employee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegePortals.com.Staff.Controllers
{
    [Route("api/[controller]")]
    public class ClgEmployeePunchAttendenceFacade : Controller
    {
        public ClgEmployeePunchAttendenceInterface _org;
        public ClgEmployeePunchAttendenceFacade(ClgEmployeePunchAttendenceInterface maspag)
        {
            _org = maspag;
        }
        [HttpPost]
        [Route("getalldetails")]
        public ClgStaffDashboardDTO getalldetails([FromBody] ClgStaffDashboardDTO data)
        {
            return _org.getdata(data);
        }
      
        [Route("getrpt")]
        public async Task< ClgStaffDashboardDTO> getrpt([FromBody] ClgStaffDashboardDTO org)
        {
            return await _org.getreport(org);
        }

      
    }
}
