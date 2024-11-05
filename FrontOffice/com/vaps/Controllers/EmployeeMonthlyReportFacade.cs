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
    public class EmployeeMonthlyReportFacade : Controller
    {
        public EmployeeMonthlyReportInterface _org;


        public EmployeeMonthlyReportFacade(EmployeeMonthlyReportInterface maspag)
        {
            _org = maspag;
        }
        [HttpPost]
        [Route("getalldetails")]
        public EmployeeMonthlyReportDTO Getdet([FromBody] EmployeeMonthlyReportDTO data)
        {
            return _org.getdata(data);
        }

        [HttpPost]
        [Route("get_departments")]
        public EmployeeMonthlyReportDTO get_departments([FromBody] EmployeeMonthlyReportDTO data)
        {
            return _org.get_departments(data);
        }

        [HttpPost]
        [Route("get_designation")]
        public EmployeeMonthlyReportDTO get_designation([FromBody] EmployeeMonthlyReportDTO data)
        {
            return _org.get_designation(data);
        }
        [HttpPost]
        [Route("get_employee")]
        public EmployeeMonthlyReportDTO get_employee([FromBody] EmployeeMonthlyReportDTO data)
        {
            return _org.get_employee(data);
        }
        [Route("getrpt")]
        public async Task<EmployeeMonthlyReportDTO> getrpt([FromBody] EmployeeMonthlyReportDTO org)
        {
            return await _org.getreport(org);
        }
        [Route("getOTrpt")]
        public async Task<EmployeeMonthlyReportDTO> getOTrpt([FromBody] EmployeeMonthlyReportDTO org)
        {
            return await _org.getOTrpt(org);
        }
        [Route("getrptStJames")]
        public async Task<EmployeeMonthlyReportDTO> getrptStJames([FromBody] EmployeeMonthlyReportDTO org)
        {
            return await _org.getrptStJames(org);
        }        
    }
}
