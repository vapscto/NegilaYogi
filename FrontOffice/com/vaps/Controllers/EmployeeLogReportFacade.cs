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
    public class EmployeeLogReportFacade : Controller
    {
        public EmployeeLogReportInterface _org;

        public EmployeeLogReportFacade(EmployeeLogReportInterface maspag)
        {
            _org = maspag;
        }
        // [HttpPost]
        [Route("getalldetails")]
        public EmployeeLogReportDTO Getdet([FromBody] EmployeeLogReportDTO data)
        {
            return _org.getdata(data);
        }
        [HttpPost]
        [Route("get_departments")]
        public EmployeeLogReportDTO get_departments([FromBody] EmployeeLogReportDTO data)
        {
            return _org.get_departments(data);
        }
        [HttpPost]
        [Route("get_designation")]
        public EmployeeLogReportDTO get_designation([FromBody] EmployeeLogReportDTO data)
        {
            return _org.get_designation(data);
        }

        [HttpPost]
        [Route("get_employee")]
        public EmployeeLogReportDTO get_employee([FromBody] EmployeeLogReportDTO data)
        {
            return _org.get_employee(data);
        }

        [Route("getrpt")]
        public async Task<EmployeeLogReportDTO> getrpt([FromBody] EmployeeLogReportDTO org)
        {
            return await _org.getreport(org);
        }
        [Route("getsiglerpt")]
        public Task<EmployeeLogReportDTO> getsiglerpt([FromBody] EmployeeLogReportDTO org)
        {
            return _org.getsiglerpt(org);
        }
    }
}
