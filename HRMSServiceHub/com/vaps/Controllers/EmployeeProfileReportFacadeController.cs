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
    public class EmployeeProfileReportFacadeController : Controller
    {
        // GET: api/values
        public EmployeeProfileReportInterface _ads;

        public EmployeeProfileReportFacadeController(EmployeeProfileReportInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public EmployeeProfileReportDTO getinitialdata([FromBody]EmployeeProfileReportDTO dto)
        {
            return _ads.getBasicData(dto);
        }


        [Route("filterEmployeedetailsBySelection")]
        public EmployeeProfileReportDTO FilterEmployeedetailsBySelection([FromBody]EmployeeProfileReportDTO dto)
        {
            return _ads.FilterEmployeedetailsBySelection(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public Task<EmployeeProfileReportDTO> getEmployeedetailsBySelection([FromBody]EmployeeProfileReportDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }


        [Route("get_depts")]
        public EmployeeProfileReportDTO get_depts([FromBody]EmployeeProfileReportDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("get_desig")]
        public EmployeeProfileReportDTO get_desig([FromBody]EmployeeProfileReportDTO dto)
        {
            return _ads.get_desig(dto);
        }
    }
}
