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
    public class EmployeeInOutReportFacade : Controller
    {
        public EmployeeInOutReportInterface _org;
        public EmployeeInOutReportFacade(EmployeeInOutReportInterface maspag)
        {
            _org = maspag;
        }
        [HttpPost]
        [Route("getalldetails")]
        public EmployeeInOutReportDTO Getdet([FromBody] EmployeeInOutReportDTO data)
        {
            return _org.getdata(data);
        }
        [HttpPost]
        [Route("get_departments")]
        public EmployeeInOutReportDTO get_departments([FromBody] EmployeeInOutReportDTO data)
        {
            return _org.get_departments(data);
        }
        [HttpPost]
        [Route("get_designation")]
        public EmployeeInOutReportDTO get_designation([FromBody] EmployeeInOutReportDTO data)
        {
            return _org.get_designation(data);
        }
        [HttpPost]
        [Route("get_employee")]
        public EmployeeInOutReportDTO get_employee([FromBody] EmployeeInOutReportDTO data)
        {
            return _org.get_employee(data);
        }
        [Route("getrpt")]
        public EmployeeInOutReportDTO getrpt([FromBody] EmployeeInOutReportDTO org)
        {
            return _org.getreport(org);
        }

        [Route("lateIn_details")]
        public Task<EmployeeInOutReportDTO> lateIn_details(EmployeeInOutReportDTO mi_id)
        {
            return _org.lateIn_details(mi_id);
        }
    }
}
