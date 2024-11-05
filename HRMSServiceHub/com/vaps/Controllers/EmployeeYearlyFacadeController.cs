using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using HRMSServicesHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeYearlyFacadeController : Controller
    {
        // GET: api/values
        public EmployeeYearlyReportInterface _ads;

        public EmployeeYearlyFacadeController(EmployeeYearlyReportInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public EmployeeYearlyReportDTO getinitialdata([FromBody]EmployeeYearlyReportDTO dto)
        {
            return _ads.getBasicData(dto);
        }


        [Route("filterEmployeedetailsBySelection")]
        public EmployeeYearlyReportDTO FilterEmployeedetailsBySelection([FromBody]EmployeeYearlyReportDTO dto)
        {
            return _ads.FilterEmployeedetailsBySelection(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public Task<EmployeeYearlyReportDTO> getEmployeedetailsBySelection([FromBody]EmployeeYearlyReportDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }


        [Route("get_depts")]
        public EmployeeYearlyReportDTO get_depts([FromBody]EmployeeYearlyReportDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("get_desig")]
        public EmployeeYearlyReportDTO get_desig([FromBody]EmployeeYearlyReportDTO dto)
        {
            return _ads.get_desig(dto);
        }

        [Route("reportBetweenDatesBySelection")]
        public Task<EmployeeYearlyReportDTO> reportBetweenDatesBySelection([FromBody]EmployeeYearlyReportDTO dto)
        {
            return _ads.reportBetweenDatesBySelection(dto);
        }
    }
}
