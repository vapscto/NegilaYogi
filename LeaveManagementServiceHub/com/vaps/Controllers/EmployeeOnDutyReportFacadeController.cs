using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace LeaveManagementServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
        public class EmployeeOnDutyReportFacadeController : Controller
        {
            public EmployeeOnDutyReportInterface _leave;
            public EmployeeOnDutyReportFacadeController(EmployeeOnDutyReportInterface _leavec)
            {
                _leave = _leavec;
            }
            [HttpPost]
            [Route("getalldetails")]
            public EmployeeOnDutyReportDTO getalldetails([FromBody] EmployeeOnDutyReportDTO data)
            {
                return _leave.getalldetails(data);
            }

        [Route("getEmployeedetailsBySelection")]
        public EmployeeOnDutyReportDTO getEmployeedetailsBySelection([FromBody]EmployeeOnDutyReportDTO dto)
        {
            return _leave.getEmployeedetailsBySelection(dto);
        }

    }
}