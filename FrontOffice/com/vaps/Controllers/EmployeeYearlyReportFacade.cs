using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FrontOfficeHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.FrontOffice;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOfficeHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeYearlyReportFacade : Controller
    {
        public EmployeeYearlyReportInterface _org;


        public EmployeeYearlyReportFacade(EmployeeYearlyReportInterface maspag)
        {
            _org = maspag;
        }
        [HttpPost]
        [Route("getalldetails")]
        public EmployeeYearlyReportDTO Getdet([FromBody] EmployeeYearlyReportDTO data)
        {
            return _org.getdata(data);
        }
        [HttpPost]
        [Route("get_departments")]
        public EmployeeYearlyReportDTO get_departments([FromBody] EmployeeYearlyReportDTO data)
        {
            return _org.get_departments(data);
        }
        [HttpPost]
        [Route("get_designation")]
        public EmployeeYearlyReportDTO get_designation([FromBody] EmployeeYearlyReportDTO data)
        {
            return _org.get_designation(data);
        }
        [HttpPost]
        [Route("get_employee")]
        public EmployeeYearlyReportDTO get_employee([FromBody] EmployeeYearlyReportDTO data)
        {
            return _org.get_employee(data);
        }
        [Route("getrpt")]
        public async Task<EmployeeYearlyReportDTO> getrpt([FromBody] EmployeeYearlyReportDTO org)
        {
            return await _org.getreport(org);
        }
    }
}
