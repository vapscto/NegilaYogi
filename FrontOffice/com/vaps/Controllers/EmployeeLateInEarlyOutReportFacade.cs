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
    public class EmployeeLateInEarlyOutReportFacade : Controller
    {
        public EmployeeLateInEarlyOutReportInterface _org;

        public EmployeeLateInEarlyOutReportFacade(EmployeeLateInEarlyOutReportInterface maspag)
        {
            _org = maspag;
        }



        [HttpPost]
        [Route("getalldetails")]
        public EmployeeLateInEarlyOutReportDTO Getdet([FromBody] EmployeeLateInEarlyOutReportDTO data)
        {
            return _org.getdata(data);
        }
        [HttpPost]
        [Route("get_departments")]
        public EmployeeLateInEarlyOutReportDTO get_departments([FromBody] EmployeeLateInEarlyOutReportDTO data)
        {
            return _org.get_departments(data);
        }
        [HttpPost]
        [Route("get_designation")]
        public EmployeeLateInEarlyOutReportDTO get_designation([FromBody] EmployeeLateInEarlyOutReportDTO data)
        {
            return _org.get_designation(data);
        }
        [HttpPost]
        [Route("get_employee")]
        public EmployeeLateInEarlyOutReportDTO get_employee([FromBody] EmployeeLateInEarlyOutReportDTO data)
        {
            return _org.get_employee(data);
        }
        [Route("getrpt")]
        public async Task<EmployeeLateInEarlyOutReportDTO> getrpt([FromBody] EmployeeLateInEarlyOutReportDTO org)
        {
            return await _org.getreport(org);
        }
    }
}
