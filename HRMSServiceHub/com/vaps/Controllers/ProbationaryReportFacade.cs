using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMSServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ProbationaryReportFacade : Controller
    {
        public ProbationaryReportInterface _ads;
        public ProbationaryReportFacade(ProbationaryReportInterface adstu)
        {
            _ads = adstu;
        }
        // GET: api/values
        [Route("getalldetails")]
        public EmployeeProfileReportDTO getalldetails([FromBody]EmployeeProfileReportDTO dto)
        {
            return _ads.getalldetails(dto);
        }
        [Route("getProbationaryReport")]
        public  Task<EmployeeProfileReportDTO> getProbationaryReport([FromBody]EmployeeProfileReportDTO dto)
        {
            return _ads.getProbationaryReport(dto);
        }
        [Route("get_departments")]
        public EmployeeProfileReportDTO get_departments([FromBody]EmployeeProfileReportDTO dto)
        {
            return _ads.get_departments(dto);
        }
        [Route("get_designation")]
        public EmployeeProfileReportDTO get_designation([FromBody]EmployeeProfileReportDTO dto)
        {
            return _ads.get_designation(dto);
        }
    }
}
