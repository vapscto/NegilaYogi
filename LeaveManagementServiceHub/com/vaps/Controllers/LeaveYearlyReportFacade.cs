using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class LeaveYearlyReportFacade : Controller
    {
        public LeaveYearlyReportInterface _leave;
        public LeaveYearlyReportFacade(LeaveYearlyReportInterface _leavec)
        {
            _leave = _leavec;
        }
        [HttpPost]
        [Route("getleavereport")]
        public LeaveCreditDTO getleavereport([FromBody] LeaveCreditDTO data)
        {
            return _leave.getleavereport(data);
        }
       
        [Route("get_departments")]
        public LeaveCreditDTO get_departments([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_departments(data);
        }
       
        [Route("get_designation")]
        public LeaveCreditDTO get_designation([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_designation(data);
        }
        [Route("get_Employees")]
        public LeaveCreditDTO get_Employees([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_Employees(data);
        }
        [Route("get_report")]
        public Task<EmployeeYearlyReportDTO> get_report([FromBody] EmployeeYearlyReportDTO data)
        {
            return _leave.get_report(data);
        }
    }
}
